using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Mapping.Actions;
using Key2Joy.Mapping.Triggers;
using Key2Joy.Plugins;

namespace Key2Joy.Mapping;

internal struct JsonMappingAspectWithType
{
    [JsonPropertyName("$type")]
    public string FullTypeName { get; set; }

    public MappingAspectOptions Options { get; set; }

    public JsonMappingAspectWithType()
    { }
}

internal class JsonMappingAspectConverter<T> : JsonConverter<T> where T : AbstractMappingAspect
{
    private readonly IDictionary<string, MappingTypeFactory> allowedTypes;

    public JsonMappingAspectConverter()
    {
        this.allowedTypes = new Dictionary<string, MappingTypeFactory>();

        foreach (var actionFactory in ActionsRepository.GetAllActions().Select(x => x.Value))
        {
            this.allowedTypes.Add(actionFactory.FullTypeName, actionFactory);
        }

        foreach (var triggerFactory in TriggersRepository.GetAllTriggers().Select(x => x.Value))
        {
            this.allowedTypes.Add(triggerFactory.FullTypeName, triggerFactory);
        }
    }

    /// <summary>
    /// Prevent recursion by not including this converter in child (de)serializations
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    private JsonSerializerOptions GetOptionsWithoutSelf(JsonSerializerOptions options)
    {
        JsonSerializerOptions newOptions = new(options);
        //var thisConverter = newOptions.Converters.SingleOrDefault(c => c is JsonMappingAspectConverter<T>);

        //if (thisConverter != null)
        //{
        //    newOptions.Converters.Remove(thisConverter);
        //}

        return newOptions;
    }

    private AbstractMappingAspect ParseJson(Type typeToConvert, JsonDocument json, JsonSerializerOptions options)
    {
        MappingAspectOptions mappingAspectOptions = new();
        var typeProperty = json.RootElement.GetProperty("$type");
        var type = MappingTypeHelper.EnsureSimpleTypeName(typeProperty.GetString());
        MappingTypeFactory failingFactory;

        if (typeToConvert == typeof(AbstractAction))
        {
            failingFactory = new MappingTypeFactory(
                typeof(DisabledAction).FullName,
                typeof(DisabledAction).GetCustomAttribute<ActionAttribute>());
        }
        else if (typeToConvert == typeof(AbstractTrigger))
        {
            failingFactory = new MappingTypeFactory(
                typeof(DisabledTrigger).FullName,
                typeof(DisabledTrigger).GetCustomAttribute<TriggerAttribute>());
        }
        else
        {
            throw new NotSupportedException($"Type {type} is not supported for {json.RootElement}.");
        }

        if (!this.allowedTypes.TryGetValue(type, out var factory))
        {
            if (typeToConvert == typeof(AbstractAction))
            {
                mappingAspectOptions.Add(nameof(DisabledAction.ActionName), type);
            }
            else if (typeToConvert == typeof(AbstractTrigger))
            {
                mappingAspectOptions.Add(nameof(DisabledTrigger.TriggerName), type);
            }

            factory = failingFactory;
        }

        AbstractMappingAspect mappingAspect;

        try
        {
            var aspectRootProperty = json.RootElement.GetProperty(
                options.PropertyNamingPolicy.ConvertName(
                    nameof(JsonMappingAspectWithType.Options)
                )
            );

            foreach (var property in aspectRootProperty.EnumerateObject())
            {
                if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    mappingAspectOptions.Add(property.Name, null);
                }
                else if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    throw new NotImplementedException($"The value kind {property.Value.ValueKind} is not yet supported.");
                }
                else if (property.Value.ValueKind == JsonValueKind.String)
                {
                    mappingAspectOptions.Add(property.Name, property.Value.GetString());
                }
                else if (property.Value.ValueKind == JsonValueKind.Number)
                {
                    mappingAspectOptions.Add(property.Name, property.Value.GetDouble());
                }
                else if (property.Value.ValueKind is JsonValueKind.True or JsonValueKind.False)
                {
                    mappingAspectOptions.Add(property.Name, property.Value.GetBoolean());
                }
                else if (property.Value.ValueKind == JsonValueKind.Array)
                {
                    List<object> list = new();

                    foreach (var item in property.Value.EnumerateArray())
                    {
                        var rawJson = item.GetRawText();
                        var document = JsonDocument.Parse(rawJson);
                        list.Add(this.ParseJson(typeToConvert, document, options));
                    }

                    mappingAspectOptions.Add(property.Name, list);
                }
                else
                {
                    throw new NotImplementedException($"The value kind {property.Value.ValueKind} is not supported for {json.RootElement}.");
                }
            }

            var name = mappingAspectOptions[nameof(AbstractMappingAspect.Name)] as string;
            mappingAspect = factory.CreateInstance<AbstractMappingAspect>(new object[]
            {
                name,
            });
            mappingAspect.LoadOptions(mappingAspectOptions);
        }
        catch (Exception ex)
        {
            if (typeToConvert == typeof(AbstractAction))
            {
                if (!mappingAspectOptions.ContainsKey(nameof(DisabledAction.ActionName)))
                {
                    mappingAspectOptions.Add(nameof(DisabledAction.ActionName), type);
                }
            }
            else if (typeToConvert == typeof(AbstractTrigger))
            {
                if (!mappingAspectOptions.ContainsKey(nameof(DisabledTrigger.TriggerName)))
                {
                    mappingAspectOptions.Add(nameof(DisabledTrigger.TriggerName), type);
                }
            }
            mappingAspectOptions.Remove(nameof(AbstractMappingAspect.Name)); // Ensure we put the error in name
            mappingAspect = failingFactory.CreateInstance<AbstractMappingAspect>(new object[]
            {
                ex.Message,
            });
            mappingAspect.LoadOptions(mappingAspectOptions);
        }

        return (T)mappingAspect;
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = JsonDocument.ParseValue(ref reader);

        return (T)this.ParseJson(typeToConvert, json, options);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var realTypeName = MappingTypeHelper.GetTypeFullName(this.allowedTypes, value);

        JsonSerializer.Serialize(writer, new JsonMappingAspectWithType
        {
            Options = value.SaveOptions(),
            FullTypeName = realTypeName,
        }, this.GetOptionsWithoutSelf(options));
    }
}
