using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;
using System;

namespace Key2Joy.Util;

/// <summary>
/// Utilities for JSON deserializing, based on:
/// Source: https://github.com/dotnet/runtime/issues/29538#issuecomment-1330494636
/// </summary>
public static class JsonUtilities
{
    // Dynamically attach a JsonSerializerOptions copy that is configured using PopulateTypeInfoResolver
    private static readonly ConditionalWeakTable<JsonSerializerOptions, JsonSerializerOptions> populateMap = new();

    /// <summary>
    /// Deserializes json into an existing object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="destination"></param>
    /// <param name="options"></param>
    public static void PopulateObject<T>(string json, T destination, JsonSerializerOptions options = null)
    {
        options = GetOptionsWithPopulateResolver(options);
        Debug.Assert(options.TypeInfoResolver is PopulateTypeInfoResolver);
        PopulateTypeInfoResolver.populateObject = destination;
        try
        {
            var result = JsonSerializer.Deserialize<T>(json, options);
            Debug.Assert(ReferenceEquals(result, destination));
        }
        finally
        {
            PopulateTypeInfoResolver.populateObject = null;
        }
    }

    private static JsonSerializerOptions GetOptionsWithPopulateResolver(JsonSerializerOptions options)
    {
        options ??= JsonSerializerOptions.Default;

        if (!populateMap.TryGetValue(options, out var populateResolverOptions))
        {
            JsonSerializer.Serialize(value: 0, options); // Force a serialization to mark options as read-only
            Debug.Assert(options.TypeInfoResolver != null);

            populateResolverOptions = new JsonSerializerOptions(options)
            {
                TypeInfoResolver = new PopulateTypeInfoResolver(options.TypeInfoResolver)
            };

            populateMap.Add(options, populateResolverOptions);
        }

        return populateResolverOptions;
    }

    private class PopulateTypeInfoResolver : IJsonTypeInfoResolver
    {
        private readonly IJsonTypeInfoResolver jsonTypeInfoResolver;

        [ThreadStatic]
        internal static object populateObject;

        public PopulateTypeInfoResolver(IJsonTypeInfoResolver jsonTypeInfoResolver)
            => this.jsonTypeInfoResolver = jsonTypeInfoResolver;

        public JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            var typeInfo = this.jsonTypeInfoResolver.GetTypeInfo(type, options);
            if (typeInfo != null && typeInfo.Kind != JsonTypeInfoKind.None)
            {
                var defaultCreateObjectDelegate = typeInfo.CreateObject;
                typeInfo.CreateObject = () =>
                {
                    var result = populateObject;
                    if (result != null)
                    {
                        // Clean up to prevent reuse in recursive scenaria
                        populateObject = null;
                    }
                    else
                    {
                        // Fall back to the default delegate
                        result = defaultCreateObjectDelegate.Invoke();
                    }

                    return result!;
                };
            }

            return typeInfo;
        }
    }
}
