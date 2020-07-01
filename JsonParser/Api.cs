using System.Collections.Generic;

namespace JsonParser.Classes
{
    public class Api
    {
        public string Endpoint { get; set; }
        public string Verb { get; set; }
        public string Section { get; set; }
        public string Description { get; set; }
        public IEnumerable<Parameters> Parameters { get; set; }
    }
}