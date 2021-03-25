using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuseumApi.Models
{
    public class Collection
    {
        [JsonProperty("collection")]
        public List<CollectionAtrubitus> Collections { get; set; }

        public class CollectionAtrubitus
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Url { get; set; }
            public string Description { get; set; }
        }
    }
}
