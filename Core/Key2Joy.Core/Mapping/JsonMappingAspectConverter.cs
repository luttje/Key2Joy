using Jint;
using Jint.Native;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Plugins;
using System;
using System.Collections;
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

        /// <summary>
        /// Prevent recursion by not including this converter in child (de)serializations
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private JsonSerializerOptions GetOptionsWithoutSelf(JsonSerializerOptions options)
        {
            var newOptions = new JsonSerializerOptions(options);
            //var thisConverter = newOptions.Converters.SingleOrDefault(c => c is JsonMappingAspectConverter<T>);

            //if (thisConverter != null)
            //{
            //    newOptions.Converters.Remove(thisConverter);
            //}

            return newOptions;
        }

        private AbstractMappingAspect ParseJson(JsonDocument json, JsonSerializerOptions options)
        {
            var typeProperty = json.RootElement.GetProperty("$type");
            var type = MappingTypeHelper.EnsureSimpleTypeName(typeProperty.GetString());

            if (!allowedTypes.TryGetValue(type, out var factory))
            {
                throw new RemotingException($"The type {type} is not allowed.");
            }

            var actionRootProperty = json.RootElement.GetProperty(
                options.PropertyNamingPolicy.ConvertName(
                    nameof(JsonMappingAspectWithType.Options)
                )
            );

            var mappingAspectOptions = new MappingAspectOptions();

            foreach (var property in actionRootProperty.EnumerateObject())
            {
                if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    throw new NotImplementedException($"The value kind {property.Value.ValueKind} is not yet supported.");
                }
                else if (property.Value.ValueKind == JsonValueKind.String)
                {
                    mappingAspectOptions.Add(property.Name, property.Value.GetString());
                }
                else if (property.Value.ValueKind == JsonValueKind.Number)
                {
                    mappingAspectOptions.Add(property.Name, property.Value.GetInt32());
                }
                else if (property.Value.ValueKind == JsonValueKind.True || property.Value.ValueKind == JsonValueKind.False)
                {
                    mappingAspectOptions.Add(property.Name, property.Value.GetBoolean());
                }
                else if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    mappingAspectOptions.Add(property.Name, null);
                }
                else if (property.Value.ValueKind == JsonValueKind.Array)
                {
                    var list = new List<object>();

                    foreach (var item in property.Value.EnumerateArray())
                    {
                        var rawJson = item.GetRawText();
                        var document = JsonDocument.Parse(rawJson);
                        list.Add(ParseJson(document, options));
                    }

                    mappingAspectOptions.Add(property.Name, list);
                }
                else
                {
                    throw new NotImplementedException($"The value kind {property.Value.ValueKind} is not supported.");
                }
            }

            var name = mappingAspectOptions[nameof(AbstractMappingAspect.Name)] as string;
            //options.Remove("name");

            var mappingAspect = factory.CreateInstance<AbstractMappingAspect>(new object[]
            {
                name,
            });
            mappingAspect.LoadOptions(mappingAspectOptions);

            return (T)mappingAspect;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var json = JsonDocument.ParseValue(ref reader);

            return (T)ParseJson(json, options);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            string realTypeName = MappingTypeHelper.GetTypeFullName(allowedTypes, value);
            
            JsonSerializer.Serialize(writer, new JsonMappingAspectWithType
            {
                Options = value.SaveOptions(),
                FullTypeName = realTypeName,
            }, GetOptionsWithoutSelf(options));
        }
    }
}
