using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Key2Joy.Setup.AppReleases;

internal class Release
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    //[JsonProperty("author")]
    //public Author Author { get; set; }

    [JsonPropertyName("tag_name")]
    public string TagName { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("draft")]
    public bool Draft { get; set; }

    [JsonPropertyName("prerelease")]
    public bool Prerelease { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("published_at")]
    public DateTime PublishedAt { get; set; }

    [JsonPropertyName("assets")]
    public List<Asset> Assets { get; set; }

    [JsonPropertyName("body")]
    public string Body { get; set; }
}
