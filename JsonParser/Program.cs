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

            var current = string.Empty;
            var previous = string.Empty;
            var template = await File.ReadAllTextAsync(System.IO.Path.Combine(mydocs, "GCWeb", "template.html"));
            var html = new StringBuilder("");

            html.AppendLine(@"<div class=""row"">");
            html.AppendLine(@"<div class=""col-2"">");
            html.AppendLine(@"<nav class=""navbar bg-light"">");
            html.AppendLine(@"<ul class=""nav navbar-nav"">");

            var grouped = apiList.GroupBy(x => x.Section);
            foreach (var section in grouped)
            {                
                html.AppendLine(@$"<li class=""nav-item""><a class=""nav-link"" href=""#{section.Key}"">{section.Key}</a></li>");
            }

            html.AppendLine("</ul>");
            html.AppendLine("</nav>");
            html.AppendLine("</div>");
            html.AppendLine(@"<div class=""col-10"">");
            html.AppendLine(@"<div class=""container1"">");
            html.AppendLine("<h1>API List</h1>");

            // display api data.
            foreach (var section in apiList)
            {
                current = section.Section;

                if (!current.Equals(previous))
                {
                    //html.AppendLine("<details>");
                    html.AppendLine(@$"<h2 id=""{section.Section}"">{section.Section}</h2>");
                    html.AppendLine("<hr />");
                }

                html.AppendLine(@"<div class=""card border-dark"">");
                html.AppendLine($@"<div class=""card-header""><h3 class=""bg-light"">{section.Verb.ToUpper()} - {section.Endpoint}</h3></div>");
                html.AppendLine(@"<div class=""card-body"">");
                html.AppendLine("<h4>Description</h4>");
                html.AppendLine(section.Description);

                html.AppendLine("<h4>Parameters</h4>");
                html.AppendLine(@"<table class=""table table-bordered table-hover"">");
                html.AppendLine(@"<thead class=""thead-dark"">");
                html.AppendLine("<tr>");
                html.AppendLine(@"<th scope=""col"">Name</th>");
                html.AppendLine(@"<th scope=""col"">Location</th>");
                html.AppendLine(@"<th scope=""col"">Descrption</th>");
                html.AppendLine(@"<th scope=""col"">Type</th>");
                html.AppendLine(@"<th scope=""col"">Enum</th>");
                html.AppendLine(@"<th scope=""col"">Is Required</th>");
                html.AppendLine(@"<th scope=""col"">Reference</th>");
                html.AppendLine("</tr>");
                html.AppendLine("</thead>");
                html.AppendLine("<tbody>");

                foreach (var param in section.Parameters)
                {
                    html.AppendLine("<tr>");
                    html.AppendLine($"<td>{param.Name}</td>");
                    html.AppendLine($"<td>{param.Location}</td>");
                    html.AppendLine($"<td>{param.Description}</td>");
                    html.AppendLine($"<td>{param.Type}</td>");
                    html.AppendLine($"<td>{param.Enumeration}</td>");
                    html.AppendLine($"<td>{param.Required}</td>");
                    html.AppendLine($"<td>{param.Reference}</td>");
                }

                html.AppendLine("</tbody>");
                html.AppendLine("</table>");

                html.AppendLine("<h4>Responses</h4>");
                html.AppendLine(@"<table class=""table table-bordered table-hover"">");
                html.AppendLine(@"<thead class=""thead-dark"">");
                html.AppendLine("<tr>");
                html.AppendLine(@"<th scope=""col"">HTTP Code</th>");
                html.AppendLine(@"<th scope=""col"">Descrption</th>");
                html.AppendLine("</tr>");
                html.AppendLine("</thead>");
                html.AppendLine("<tbody>");
                html.AppendLine("<tr>");
                html.AppendLine("<td>200</td>");
                html.AppendLine("<td>Ok</td>");
                html.AppendLine("</tr>");
                html.AppendLine("<tr>");
                html.AppendLine("<td>400</td>");
                html.AppendLine("<td>Bad Request</td>");
                html.AppendLine("</tr>");
                html.AppendLine("<tr>");
                html.AppendLine("<td>401</td>");
                html.AppendLine("<td>Unautorized</td>");
                html.AppendLine("</tr>");
                html.AppendLine("</tbody>");
                html.AppendLine("</table>");
                html.AppendLine("</div>");
                html.AppendLine("</div>");

                previous = current;
            }

            html.AppendLine("</div>");
            html.AppendLine("</div>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");

            template += html.ToString();

            await File.WriteAllTextAsync(System.IO.Path.Combine(mydocs, "GCWeb", "apilist.html"), template);

            // Suspend the screen.
            Console.WriteLine("done!");
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