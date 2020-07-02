namespace JsonParser.Classes
{
    public class Path
    {
        public string enpoint { get; set; }
        public Post post { get; set; }
        public Get get { get; set; }
        public Put put { get; set; }
        public Delete delete { get; set; }
        public Head head { get; set; }
    }
}