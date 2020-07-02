namespace JsonParser.Classes
{
    public class Parameter
    {
        public string name { get; set; }
        public string @in { get; set; }
        public string description { get; set; }
        public bool required { get; set; }
        public Schema schema { get; set; }
        public string type { get; set; }
        public string format { get; set; }
        public string[] renum { get; set; }
    }
}