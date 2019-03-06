using Newtonsoft.Json;

namespace pc_remote_server.Server
{
    public struct CommandResponse
    {
        [JsonProperty("id")] public int Id;
        [JsonProperty("data")] public object Data;
    }
}