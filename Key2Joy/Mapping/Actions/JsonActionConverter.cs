using Key2Joy.Contracts.Mapping;
using Key2Joy.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    internal struct JsonActionWithType
    {
        [JsonPropertyName("$type")]
        public string FullTypeName { get; set; }
        public AbstractAction Action { get; set; }

        public JsonActionWithType() { }
    }

    internal class JsonActionConverter : JsonConverter<AbstractAction>
    {
        private IDictionary<string, MappingTypeFactory> allowedTypes;

        public JsonActionConverter()
        {
            allowedTypes = new Dictionary<string, MappingTypeFactory>();

            foreach (var actionFactory in ActionsRepository.GetAllActions().Select(x => x.Value))
            {
                allowedTypes.Add(actionFactory.FullTypeName, actionFactory);
            }

            foreach (var triggerFactory in TriggersRepository.GetAllTriggers().Select(x => x.Value))
            {
                allowedTypes.Add(triggerFactory.FullTypeName, triggerFactory);
            }
        }

        private static string GetFullTypeName(string typeInfoTypeName)
        {
            return typeInfoTypeName.Split(',')[0];
        }

        private JsonSerializerOptions GetOptionsWithoutSelf(JsonSerializerOptions options)
        {
            var newOptions = new JsonSerializerOptions(options);
            var thisConverter = newOptions.Converters.SingleOrDefault(c => c is JsonActionConverter);

            if (thisConverter != null)
            {
                newOptions.Converters.Remove(thisConverter);
            }

            return newOptions;
        }

        public override AbstractAction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var json = JsonDocument.ParseValue(ref reader);

            var typeProperty = json.RootElement.GetProperty("$type");
            var type = GetFullTypeName(typeProperty.GetString());

            if (!allowedTypes.TryGetValue(type, out var factory))
            {
                throw new RemotingException($"The type {type} is not allowed.");
            }

            var actionRootProperty = json.RootElement.GetProperty("action");

            // Create a dictionary of all properties
            var properties = new Dictionary<string, object>();
            
            foreach (var property in actionRootProperty.EnumerateObject())
            {
                if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    throw new NotImplementedException($"The value kind {property.Value.ValueKind} is not yet supported.");
                }
                else if (property.Value.ValueKind == JsonValueKind.String)
                {
                    properties.Add(property.Name, property.Value.GetString());
                }
                else if (property.Value.ValueKind == JsonValueKind.Number)
                {
                    properties.Add(property.Name, property.Value.GetInt32());
                }
                else if (property.Value.ValueKind == JsonValueKind.True || property.Value.ValueKind == JsonValueKind.False)
                {
                    properties.Add(property.Name, property.Value.GetBoolean());
                }
                else
                {
                    throw new NotImplementedException($"The value kind {property.Value.ValueKind} is not supported.");
                }
            }

            var name = properties["name"];
            properties.Remove("name");

            var action = factory.CreateInstance<AbstractAction>(new object[]
            {
                name,
                properties,
            });

            return action;
        }

        public override void Write(Utf8JsonWriter writer, AbstractAction value, JsonSerializerOptions options)
        {
            string realTypeName;

            if (RemotingServices.IsTransparentProxy(value))
            {
                var objRef = RemotingServices.GetObjRefForProxy(value);
                realTypeName = objRef.TypeInfo.TypeName;

                if (!allowedTypes.ContainsKey(GetFullTypeName(realTypeName)))
                {
                    throw new ArgumentException("Only allowed types may be serialized to the preset");
                }
            }
            else
            {
                realTypeName = value.GetType().FullName;

                if (!allowedTypes.ContainsKey(realTypeName))
                {
                    throw new ArgumentException("Only allowed types may be serialized to the preset");
                }
            }

            JsonSerializer.Serialize(writer, new JsonActionWithType
            {
                Action = value,
                FullTypeName = realTypeName,
            }, GetOptionsWithoutSelf(options));
        }
    }
}
