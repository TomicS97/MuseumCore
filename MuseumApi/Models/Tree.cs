using Newtonsoft.Json;
using System.Collections.Generic;

namespace MuseumApi.Controllers
{
    internal class Tree
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("collection")]
        public List<Tree> Collection { get; set; }
    }
}