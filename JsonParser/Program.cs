using JsonParser.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JsonParser
{
    class Program
    {
        // trying out ways to read the paths and references.
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            // Read the file and display it line by line.  
            var counter = 0;
            var line = string.Empty;
            var lines = new List<string>();
            var paths = new StringBuilder("");
            var mydocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var json = System.IO.Path.Combine(mydocs, "GCWeb", "swagger_paths.json");
            
            //read file line by line.
            using (var reader = new StreamReader(json))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line.Trim());
                }
            }

            // format file which will allow to get info.
            foreach (var item in lines)
            {
                if (counter == 6)
                {
                    counter = 0;
                    continue;
                }

                if (item.Equals("}"))
                {
                    counter++;
                }
                else
                {
                    counter = 0;
                }

                if (item.Contains("paths"))
                {
                    paths.Append($"{item.Replace("{", "")} [");
                }
                else if (item.Contains("api/v"))
                {
                    paths.Append("{");
                    paths.Append(@$"""enpoint"":{item.Replace(":", "").Substring(0, item.Length - 2)},");
                }
                else if (counter == 5)
                {
                    paths.Append(item.Replace("}", "},"));
                    counter++;
                }
                else
                {
                    paths.Append(item);
                }
            }

            // some clean up.
            paths.Remove(paths.Length - 3, 2);
            paths.AppendLine("]}");
            paths.Replace("200", "r200");
            paths.Replace("$ref", "ref");
            paths.Replace("enum", "renum");

            // create new file with modified json.
            await File.WriteAllTextAsync(System.IO.Path.Combine(mydocs, "GCWeb", "swagger_test.json"), paths.ToString());

            // get information from new json file.
            var jfile = await File.ReadAllTextAsync(System.IO.Path.Combine(mydocs, "GCWeb", "swagger_test.json"));
            var pathData = JsonConvert.DeserializeObject<Rootobject>(jfile);
            var apiList = new List<Api>();

            // get post data.
            var post = pathData.paths.Where(w => w.post != null)
                .Select(p => new Api
                {
                    Endpoint = p.enpoint,
                    Verb = "post",
                    Section = p.post.tags[0],
                    Description = p.post.summary,
                    Parameters = p.post.parameters.Select(pa => new ApiParameters
                    {
                        Name = pa.name,
                        Location = pa.@in,
                        Description = pa.description,
                        Type = pa.type,
                        Enums = pa.renum,
                        Required = pa.required
                    })
                });

            // get get data.
            var get = pathData.paths.Where(w => w.get != null)
                .Select(p => new Api
                {
                    Endpoint = p.enpoint,
                    Verb = "get",
                    Section = p.get.tags[0],
                    Description = p.get.summary,
                    Parameters = p.get.parameters.Select(pa => new ApiParameters
                    {
                        Name = pa.name,
                        Location = pa.@in,
                        Description = pa.description,
                        Type = pa.type,
                        Enums = pa.renum,
                        Required = pa.required
                    })
                });

            // get put data.
            var put = pathData.paths.Where(w => w.put != null)
                .Select(p => new Api
                {
                    Endpoint = p.enpoint,
                    Verb = "put",
                    Section = p.put.tags[0],
                    Description = p.put.summary,
                    Parameters = p.put.parameters.Select(pa => new ApiParameters
                    {
                        Name = pa.name,
                        Location = pa.@in,
                        Description = pa.description,
                        Type = pa.type,
                        Enums = pa.renum,
                        Required = pa.required
                    })
                });

            // get delete data.
            var delete = pathData.paths.Where(w => w.delete != null)
                .Select(p => new Api
                {
                    Endpoint = p.enpoint,
                    Verb = "delete",
                    Section = p.delete.tags[0],
                    Description = p.delete.summary,
                    Parameters = p.delete.parameters.Select(pa => new ApiParameters
                    {
                        Name = pa.name,
                        Location = pa.@in,
                        Description = pa.description,
                        Type = pa.type,
                        Enums = pa.renum,
                        Required = pa.required
                    })
                });

            // get head data.
            var head = pathData.paths.Where(w => w.head != null)
                .Select(p => new Api
                {
                    Endpoint = p.enpoint,
                    Verb = "head",
                    Section = p.head.tags[0],
                    Description = p.head.summary,
                    Parameters = p.head.parameters.Select(pa => new ApiParameters
                    {
                        Name = pa.name,
                        Location = pa.@in,
                        Description = pa.description,
                        Type = pa.type,
                        Enums = pa.renum,
                        Required = pa.required
                    })
                });

            // add data into one list.
            apiList.AddRange(post);
            apiList.AddRange(get);
            apiList.AddRange(put);
            apiList.AddRange(delete);
            apiList.AddRange(head);

            // api list ordered by section.
            var sections = apiList.OrderBy(x => x.Section).ToList();

            // display api data.
            foreach (var section in sections)
            {
                Console.WriteLine("############################################");
                Console.WriteLine($"Section name: {section.Section}");
                Console.WriteLine("############################################");

                Console.WriteLine($"endpoint: {section.Endpoint}");
                Console.WriteLine($"verb: {section.Verb}");
                Console.WriteLine($"section: {section.Section}");
                Console.WriteLine($"description: {section.Description}");

                foreach (var param in section.Parameters)
                {
                    Console.WriteLine($"parameter name: {param.Name}");
                    Console.WriteLine($"parameter location: {param.Location}");
                    Console.WriteLine($"parameter description: {param.Description}");
                    Console.WriteLine($"parameter type: {param.Type}");
                    Console.WriteLine($"parameter enumeration: {param.Enumeration}");
                    Console.WriteLine($"parameter required: {param.Required}");
                }
            }

            // read references file and append stringbuilder.
            var referencesJson = await File.ReadAllTextAsync(System.IO.Path.Combine(mydocs, "GCWeb", "references.json"));
            var referencesSb = new StringBuilder("");
            using (var reader = new StreamReader(System.IO.Path.Combine(mydocs, "GCWeb", "references.json")))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    referencesSb.Append(line.Trim());
                }
            }

            // get reference section names.
            // parse json file.
            var referenceSections = referencesSb.ToString().Split(new[] { "}}}," }, StringSplitOptions.RemoveEmptyEntries);
            var referenceNames = referenceSections.Select(x => x.Split('{', StringSplitOptions.RemoveEmptyEntries)[0]);
            var jo = JObject.Parse(referencesJson);            

            foreach (var referenceName in referenceNames)
            {
                var match = Regex.Match(referenceName, @"""(.*?)""");
                var name = match.Value[1..^1];
                var data = (JObject)jo[name]["properties"];

                Console.WriteLine("############################################");
                foreach (var d in data)
                {
                    Console.WriteLine($"Key: {d.Key} Value: {d.Value}");
                }
                Console.WriteLine("############################################");
            }
            // Suspend the screen.  
            Console.ReadLine();
        }
    }
}

//https://stackoverflow.com/questions/29326796/deserialize-json-with-unknown-fields-properties
//https://stackoverflow.com/questions/14714085/parsing-through-json-in-json-net-with-unknown-property-names