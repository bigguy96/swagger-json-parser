using System.Linq;

namespace JsonParser.Classes
{
    public class Parameters
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Enumeration => Enums !=null ?  string.Join(",", Enums) : "";
        public string[] Enums { get; set; }
        public bool Required { get; set; }
    }
}

//https://stackoverflow.com/questions/29326796/deserialize-json-with-unknown-fields-properties