using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MuseumApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static MuseumApi.Models.Collection;

namespace MuseumApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MuseumController : ControllerBase
    {
        [HttpGet]
        [Route("getTree")]
        public ActionResult GetTree()                             //Get value from Tree file.
        {
            var dir = System.IO.Directory.GetCurrentDirectory();

            var path = Path.Combine(dir, @"JSON/tree.json");
            string jsonFile;

            using (var reader = new StreamReader(path))
            {
                jsonFile = reader.ReadToEnd();
            }
            var result = JsonConvert.DeserializeObject<Tree>(jsonFile);
            return Ok(result);
        }

        public ActionResult Get()
        {
            return Ok("WORKING!!");
        }


        [HttpPost]
        [Route("getCollection")]
        public ActionResult GetCollection([FromBody] int id)        //Get value from collection file.
        {
            var dir = System.IO.Directory.GetCurrentDirectory();

            var path = Path.Combine(dir, @"JSON/collection.json");
            string jsonFile;
            using (var reader = new StreamReader(path))
            {
                jsonFile = reader.ReadToEnd();
            }
            var json = JsonConvert.DeserializeObject<Collection>(jsonFile);

            var result = json.Collections.FirstOrDefault(x => x.Id == id);

            return Ok(result);
        }

        [HttpPost]
        [Route("editArt")]
        public ActionResult EditArt([FromBody] int id)
        {
            var dir = System.IO.Directory.GetCurrentDirectory();

            var path = Path.Combine(dir, @"JSON/collection.json");
            string jsonFile;
            using (var reader = new StreamReader(path))
            {
                jsonFile = reader.ReadToEnd();
            }
            var json = JsonConvert.DeserializeObject<Collection>(jsonFile);

            var result = json.Collections.FirstOrDefault(x => x.Id == id);

            return Ok(result);
        }


        [HttpPost]
        [Route("saveEditArt")]
        public ActionResult SaveEditArt([FromBody] CollectionAtrubitus collectionAtrubitus)         //Save edit collection
        {
            var dir = System.IO.Directory.GetCurrentDirectory();

            var path = Path.Combine(dir, @"JSON/collection.json");
            string jsonFile;
            using (var reader = new StreamReader(path))
            {
                jsonFile = reader.ReadToEnd();
            }
            var json = JsonConvert.DeserializeObject<Collection>(jsonFile);
            foreach (var item in json.Collections)
            {
                if (item.Id == collectionAtrubitus.Id)
                {
                    item.Description = collectionAtrubitus.Description;
                    item.Url = collectionAtrubitus.Url;
                    item.Name = collectionAtrubitus.Name;
                    break;
                }
            }

            var path2 = Path.Combine(dir, @"JSON/tree.json");
            string jsonFile2;
            using (var reader = new StreamReader(path2))
            {
                jsonFile2 = reader.ReadToEnd();
            }
            var json2 = JsonConvert.DeserializeObject<Tree>(jsonFile2);
            foreach (var item in json2.Collection)
            {
                foreach (var item2 in item.Collection)
                {
                    if (item2.Id == collectionAtrubitus.Id)
                    {
                        item2.Name = collectionAtrubitus.Name;
                    }
                }
            }

            var result = JsonConvert.SerializeObject(json);
            var result2 = JsonConvert.SerializeObject(json2);
            using (var writer = new StreamWriter(path))
            {
                writer.Write(result);
            }
            using (var writer = new StreamWriter(path2))
            {
                writer.Write(result2);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("getTreeSearch")]
        public ActionResult GetTreeSearch([FromBody] Key name)
        {
            var dir = System.IO.Directory.GetCurrentDirectory();

            var path = Path.Combine(dir, @"JSON/tree.json");
            string jsonFile;
            using (var reader = new StreamReader(path))
            {
                jsonFile = reader.ReadToEnd();
            }
            Tree tree = JsonConvert.DeserializeObject<Tree>(jsonFile);
            if (String.IsNullOrEmpty(name.key))
                return Ok(tree);

            Tree treeFiltered = new Tree
            {
                Id = tree.Id,
                Name = tree.Name,
                Type = tree.Type,
                Collection = new List<Tree>()
            };

            foreach (var coll in tree.Collection)
            {
                if (coll.Collection.Any(x => x.Name.Contains(name.key)))
                {
                    Tree newTree = new Tree();
                    newTree.Id = coll.Id;
                    newTree.Name = coll.Name;
                    newTree.Type = coll.Type;
                    newTree.Collection = new List<Tree>();

                    foreach (var coll1 in coll.Collection)
                    {
                        if (coll1.Name.Contains(name.key))
                        {
                            newTree.Collection.Add(coll1);
                        }
                    }

                    treeFiltered.Collection.Add(newTree);
                }
            }
            return Ok(treeFiltered);
        }
    }
}
