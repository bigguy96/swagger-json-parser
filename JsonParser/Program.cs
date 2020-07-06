using JsonParser.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

            // reformat the json and create a new file which will allow to get info.
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

            // create new file with restructured json.
            await File.WriteAllTextAsync(System.IO.Path.Combine(mydocs, "GCWeb", "swagger_test.json"), paths.ToString());

            // get information from new json file.
            var jfile = await File.ReadAllTextAsync(System.IO.Path.Combine(mydocs, "GCWeb", "swagger_test.json"));
            var pathData = JsonConvert.DeserializeObject<Rootobject>(jfile);
            var apiList = new List<Api>();

            foreach (var item in pathData.paths)
            {
                var verb = string.Empty;
                var section = string.Empty;
                var summary = string.Empty;
                var parameters = new List<ApiParameters>();

                if (item.get != null)
                {
                    verb = "get";
                    section = item.get.tags[0];
                    summary = item.get.summary;
                    parameters = GetApiParameters(item.get.parameters);
                }
                else if (item.post != null)
                {
                    verb = "post";
                    section = item.post.tags[0];
                    summary = item.post.summary;
                    parameters = GetApiParameters(item.post.parameters);
                }
                else if (item.put != null)
                {
                    verb = "put";
                    section = item.put.tags[0];
                    summary = item.put.summary;
                    parameters = GetApiParameters(item.put.parameters);
                }
                else if (item.delete != null)
                {
                    verb = "delete";
                    section = item.delete.tags[0];
                    summary = item.delete.summary;
                    parameters = GetApiParameters(item.delete.parameters);
                }
                else if (item.head != null)
                {
                    verb = "head";
                    section = item.head.tags[0];
                    summary = item.head.summary;
                    parameters = GetApiParameters(item.head.parameters);
                }
                else
                {
                    verb = "n/a";
                }

                apiList.Add(new Api
                {
                    Endpoint = item.enpoint,
                    Verb = verb,
                    Section = section,
                    Description = summary,
                    Parameters = parameters
                });
            }

            // api list ordered by section.
            apiList = apiList.OrderBy(x => x.Section).ToList();

            // display api data.
            foreach (var section in apiList)
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
                    Console.WriteLine($"parameter reference: {param.Reference}");
                }
            }

            // Suspend the screen.  
            Console.ReadLine();            
        }

        private static List<ApiParameters> GetApiParameters(Parameter[] parameters)
        {
            return parameters
                   .Select(parameter => new ApiParameters
                   {
                       Name = parameter.name,
                       Location = parameter.@in,
                       Description = parameter.description,
                       Type = parameter.type,
                       Enums = parameter.@enum,
                       Required = parameter.required,
                       Reference = parameter?.schema?.@ref
                   }).ToList();
        }
    }
}

//https://stackoverflow.com/questions/29326796/deserialize-json-with-unknown-fields-properties
//https://stackoverflow.com/questions/14714085/parsing-through-json-in-json-net-with-unknown-property-names
//https://stackoverflow.com/questions/20318261/dynamic-object-with-dollar-on-string
//https://github.com/Swagger2Markup/swagger2markup