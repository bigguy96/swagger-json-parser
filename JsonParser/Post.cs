﻿namespace JsonParser.Classes
{
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
}