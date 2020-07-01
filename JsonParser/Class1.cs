using System;
using System.Collections.Generic;
using System.Text;

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
        public Parameter1[] parameters { get; set; }
        public Responses1 responses { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
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

    public class Schema
    {
        public string type { get; set; }
    }

    public class Parameter
    {
        public string name { get; set; }
        public string @in { get; set; }
        public string description { get; set; }
        public bool required { get; set; }
        public Schema1 schema { get; set; }
        public string type { get; set; }
        public string format { get; set; }
        public string[] renum { get; set; }
    }

    public class Schema1
    {
        public string _ref { get; set; }
        public string type { get; set; }
    }

   

    public class Responses1
    {
        public R2001 r200 { get; set; }
    }

    public class R2001
    {
        public string description { get; set; }
        public Schema2 schema { get; set; }
    }

    public class Schema2
    {
        public string type { get; set; }
    }

    public class Parameter1
    {
        public string name { get; set; }
        public string @in { get; set; }
        public bool required { get; set; }
        public string type { get; set; }
        public string format { get; set; }
        public string description { get; set; }
        public string[] renum { get; set; }
    }

    public class Put
    {
        public string[] tags { get; set; }
        public string summary { get; set; }
        public string operationId { get; set; }
        public string[] consumes { get; set; }
        public string[] produces { get; set; }
        public Parameter2[] parameters { get; set; }
        public Responses2 responses { get; set; }
        public string description { get; set; }
    }

    public class Responses2
    {
        public R2002 r200 { get; set; }
    }

    public class R2002
    {
        public string description { get; set; }
        public Schema3 schema { get; set; }
    }

    public class Schema3
    {
        public string type { get; set; }
    }

    public class Parameter2
    {
        public string name { get; set; }
        public string @in { get; set; }
        public string description { get; set; }
        public bool required { get; set; }
        public string type { get; set; }
        public string format { get; set; }
        public string[] renum { get; set; }
        public Schema4 schema { get; set; }
    }

    public class Schema4
    {
        public string type { get; set; }
        public string format { get; set; }
        public string _ref { get; set; }
    }

    public class Delete
    {
        public string[] tags { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
        public string operationId { get; set; }
        public object[] consumes { get; set; }
        public string[] produces { get; set; }
        public Parameter3[] parameters { get; set; }
        public Responses3 responses { get; set; }
    }

    public class Responses3
    {
        public R2003 r200 { get; set; }
    }

    public class R2003
    {
        public string description { get; set; }
        public Schema5 schema { get; set; }
    }

    public class Schema5
    {
        public string type { get; set; }
    }

    public class Parameter3
    {
        public string name { get; set; }
        public string @in { get; set; }
        public string description { get; set; }
        public bool required { get; set; }
        public string type { get; set; }
        public string format { get; set; }
        public string[] renum { get; set; }
    }

    public class Head
    {
        public string[] tags { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
        public string operationId { get; set; }
        public object[] consumes { get; set; }
        public string[] produces { get; set; }
        public Parameter4[] parameters { get; set; }
        public Responses4 responses { get; set; }
    }

    public class Responses4
    {
        public R2004 r200 { get; set; }
    }

    public class R2004
    {
        public string description { get; set; }
        public Schema6 schema { get; set; }
    }

    public class Schema6
    {
        public string type { get; set; }
    }

    public class Parameter4
    {
        public string name { get; set; }
        public string @in { get; set; }
        public string description { get; set; }
        public bool required { get; set; }
        public string type { get; set; }
        public string format { get; set; }
        public string[] renum { get; set; }
    }

}
