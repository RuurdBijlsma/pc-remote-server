using Newtonsoft.Json;

namespace pc_remote_server.Server
{
    public struct Command
    {
        [JsonProperty("id")]
        public int Id;
        [JsonProperty("action")]
        public string Action;
        [JsonProperty("value")]
        public string Value;
    }
}