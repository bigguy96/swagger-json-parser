using JsonParser.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JsonParser
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            // Read the file and display it line by line.  
            var counter = 0;
            var line = string.Empty;
            var lines = new List<string>();
            var sb = new System.Text.StringBuilder("");
            var mydocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var json = System.IO.Path.Combine(mydocs, "GCWeb", "swagger_paths.json");
            using (var reader = new StreamReader(json))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line.Trim());
                }
            }

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
                    sb.Append($"{item.Replace("{", "")} [");
                    //sb.AppendLine("{");
                }
                else if (item.Contains("api/v"))
                {
                    sb.Append("{");
                    sb.Append(@$"""enpoint"":{item.Replace(":", "").Substring(0, item.Length - 2)},");
                }
                else if (counter == 5)
                {
                    sb.Append(item.Replace("}", "},"));
                    counter++;
                    //sb.AppendLine(item.Replace("}","}],")); 
                }
                else
                {
                    sb.Append(item);
                }
            }

            sb.Remove(sb.Length - 3, 2);
            sb.AppendLine("]}");
            sb.Replace("200", "r200");
            sb.Replace("$ref", "ref");
            sb.Replace("enum", "renum");

            await File.WriteAllTextAsync(System.IO.Path.Combine(mydocs, "GCWeb", "swagger_test.json"), sb.ToString());

            var jfile = await File.ReadAllTextAsync(System.IO.Path.Combine(mydocs, "GCWeb", "swagger_test.json"));
            var r = JsonConvert.DeserializeObject<Rootobject>(jfile);
            var list = new List<Api>();

            var post = r.paths.Where(w => w.post != null)
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

            var get = r.paths.Where(w => w.get != null)
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

            var put = r.paths.Where(w => w.put != null)
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

            var delete = r.paths.Where(w => w.delete != null)
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

            var head = r.paths.Where(w => w.head != null)
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

            list.AddRange(post);
            list.AddRange(get);
            list.AddRange(put);
            list.AddRange(delete);
            list.AddRange(head);

            var sections = list.GroupBy(x => x.Section);

            foreach (var section in sections)
            {
                Console.WriteLine("*******************************************************");
                Console.WriteLine($"Section name: {section.Key}");
                Console.WriteLine("*******************************************************");

                foreach (var item in section)
                {
                    Console.WriteLine("############################################");
                    Console.WriteLine($"endpoint: {item.Endpoint}");
                    Console.WriteLine($"verb: {item.Verb}");
                    Console.WriteLine($"section: {item.Section}");
                    Console.WriteLine($"description: {item.Description}");

                    foreach (var param in item.Parameters)
                    {
                        Console.WriteLine($"parameter name: {param.Name}");
                        Console.WriteLine($"parameter location: {param.Location}");
                        Console.WriteLine($"parameter description: {param.Description}");
                        Console.WriteLine($"parameter type: {param.Type}");
                        Console.WriteLine($"parameter enumeration: {param.Enumeration}");
                        Console.WriteLine($"parameter required: {param.Required}");
                    }
                    Console.WriteLine("############################################");
                }                
            }            

            // Suspend the screen.  
            Console.ReadLine();
        }
    }
}

//https://stackoverflow.com/questions/29326796/deserialize-json-with-unknown-fields-properties