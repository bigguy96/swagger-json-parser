using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            var json = Path.Combine(mydocs, "GCWeb", "swagger_paths.json");
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

            await File.WriteAllTextAsync(Path.Combine(mydocs, "GCWeb", "swagger_test.json"), sb.ToString());

            var jfile = await File.ReadAllTextAsync(Path.Combine(mydocs, "GCWeb", "swagger_test.json"));
            //var swagger = JObject.Parse(jfile);
            //var ds=  JsonConvert.DeserializeAnonymousType(jfile, null);

            var r = JsonConvert.DeserializeObject<Classes.Rootobject>(jfile);

            var post = r.paths.Where(w => w.post != null)
                .Select(p => new
                {
                    p.enpoint,
                    section = p.post.tags[0],
                    p.post.summary,
                    parameters = p.post.parameters.Select(pa => new { pa.name, pa.@in, pa.description, pa.renum, pa.required })
                });

            foreach (var item in post)
            {
                Console.WriteLine("*******************************************************");
                Console.WriteLine($"endpoint: {item.enpoint}");
                Console.WriteLine($"section: {item.section}");
                Console.WriteLine($"description: {item.summary}");

                foreach (var param in item.parameters)
                {
                    Console.WriteLine($"parameter name: {param.name}");
                    Console.WriteLine($"parameter location: {param.@in}");
                    Console.WriteLine($"parameter description: {param.description}");
                    Console.WriteLine($"parameter required: {param.required}");
                }

                //foreach (var response in item.post.responses.r200.description)
                //{

                //}

            }

            // Suspend the screen.  
            Console.ReadLine();
        }
    }
}