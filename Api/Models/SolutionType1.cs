using Newtonsoft.Json;
using System.Collections.Generic;
public class SolutionType
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("description")]
    public string Description { get; set; }
    [JsonProperty("shortDescription")]
    public string ShortDescription { get; set; }
    [JsonProperty("illustrationUrl")]
    public string IllustrationUrl { get; set; }
    [JsonProperty("githubUrl")]
    public string GithubUrl { get; set; }
    [JsonProperty("price")]
    public double Price { get; set; }
    [JsonProperty("properties")]
    public List<Property> Properties { get; set; }
    [JsonProperty("services")]
    public List<string> Services { get; set; }
    [JsonProperty("standardServices")]
    public object StandardServices { get; set; }
    [JsonProperty("isTeaser")]
    public bool IsTeaser { get; set; }
    [JsonProperty("terms")]
    public string Terms { get; set; }
    [JsonProperty("additionalOptions")]
    public object AdditionalOptions { get; set; }
    [JsonProperty("resources")]
    public List<string> Resources { get; set; }
    [JsonProperty("bingMapRequired")]
    public bool BingMapRequired { get; set; }
}
