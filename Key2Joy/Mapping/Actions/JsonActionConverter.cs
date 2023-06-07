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
    internal struct JsonMappingAspectWithType
    {
        [JsonPropertyName("$type")]
        public string FullTypeName { get; set; }
        public MappingAspectOptions Options { get; set; }

        public JsonMappingAspectWithType() { }
    }

    internal class JsonMappingAspectConverter<T> : JsonConverter<T> where T : AbstractMappingAspect
    {
        private IDictionary<string, MappingTypeFactory> allowedTypes;

        public JsonMappingAspectConverter()
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

        /// <summary>
        /// Prevent recursion by not including this converter in child (de)serializations
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private JsonSerializerOptions GetOptionsWithoutSelf(JsonSerializerOptions options)
        {
            var newOptions = new JsonSerializerOptions(options);
            var thisConverter = newOptions.Converters.SingleOrDefault(c => c is JsonMappingAspectConverter<T>);

            if (thisConverter != null)
            {
                newOptions.Converters.Remove(thisConverter);
            }

            return newOptions;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var json = JsonDocument.ParseValue(ref reader);

            var typeProperty = json.RootElement.GetProperty("$type");
            var type = GetFullTypeName(typeProperty.GetString());

            if (!allowedTypes.TryGetValue(type, out var factory))
            {
                throw new RemotingException($"The type {type} is not allowed.");
            }

            var actionRootProperty = json.RootElement.GetProperty(
                options.PropertyNamingPolicy.ConvertName(
                    nameof(JsonMappingAspectWithType.Options)
                )
            );

            // Create options for all properties
            var actionOptions = new MappingAspectOptions();
            
            foreach (var property in actionRootProperty.EnumerateObject())
            {
                if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    throw new NotImplementedException($"The value kind {property.Value.ValueKind} is not yet supported.");
                }
                else if (property.Value.ValueKind == JsonValueKind.String)
                {
                    actionOptions.Add(property.Name, property.Value.GetString());
                }
                else if (property.Value.ValueKind == JsonValueKind.Number)
                {
                    actionOptions.Add(property.Name, property.Value.GetInt32());
                }
                else if (property.Value.ValueKind == JsonValueKind.True || property.Value.ValueKind == JsonValueKind.False)
                {
                    actionOptions.Add(property.Name, property.Value.GetBoolean());
                }
                else if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    actionOptions.Add(property.Name, null);
                }
                else
                {
                    throw new NotImplementedException($"The value kind {property.Value.ValueKind} is not supported.");
                }
            }

            var name = actionOptions[nameof(AbstractMappingAspect.Name)] as string;
            //options.Remove("name");

            var action = factory.CreateInstance<AbstractMappingAspect>(new object[]
            {
                name,
            });
            action.LoadOptions(actionOptions);

            return (T)action;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
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
            var test = value.SaveOptions();
            JsonSerializer.Serialize(writer, new JsonMappingAspectWithType
            {
                Options = value.SaveOptions(),
                FullTypeName = realTypeName,
            }, GetOptionsWithoutSelf(options));
        }
    }
}
