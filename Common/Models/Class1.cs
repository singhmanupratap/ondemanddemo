using Newtonsoft.Json;
public class Property
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("value")]
    public string Value { get; set; }
}
