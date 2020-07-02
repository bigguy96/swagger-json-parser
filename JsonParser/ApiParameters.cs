namespace JsonParser.Classes
{
    public class ApiParameters
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