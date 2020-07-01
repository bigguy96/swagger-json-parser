namespace JsonParser.Classes
{
    public class Rootobject
    {
        public Path[] paths { get; set; }
    }

    public class Path
    {
        public string enpoint { get; set; }
        public Post post { get; set; }
        public Get get { get; set; }
        public Put put { get; set; }
        public Delete delete { get; set; }
        public Head head { get; set; }
    }

    public class Post
    {
        public string[] tags { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
        public string operationId { get; set; }
        public string[] consumes { get; set; }
        public string[] produces { get; set; }
        public Parameter[] parameters { get; set; }
        public Responses responses { get; set; }
    }

    public class Get
    {
        public string[] tags { get; set; }
        public string operationId { get; set; }
        public object[] consumes { get; set; }
        public string[] produces { get; set; }
        public Parameter[] parameters { get; set; }
        public Responses responses { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
    }      

    public class Put
    {
        public string[] tags { get; set; }
        public string summary { get; set; }
        public string operationId { get; set; }
        public string[] consumes { get; set; }
        public string[] produces { get; set; }
        public Parameter[] parameters { get; set; }
        public Responses responses { get; set; }
        public string description { get; set; }
    }

    public class Delete
    {
        public string[] tags { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
        public string operationId { get; set; }
        public object[] consumes { get; set; }
        public string[] produces { get; set; }
        public Parameter[] parameters { get; set; }
        public Responses responses { get; set; }
    } 

    public class Head
    {
        public string[] tags { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
        public string operationId { get; set; }
        public object[] consumes { get; set; }
        public string[] produces { get; set; }
        public Parameter[] parameters { get; set; }
        public Responses responses { get; set; }
    }

    public class Responses
    {
        public R200 r200 { get; set; }
    }
    public class R200
    {
        public string description { get; set; }
        public Schema schema { get; set; }
    }

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

    public class Schema
    {
        public string type { get; set; }
        public string format { get; set; }
        public string _ref { get; set; }
    }   
}