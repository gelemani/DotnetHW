namespace task4._4.Models;
using System.Collections.Generic;

public class BlogEntity {
    public string Article { get; set; }
    public string Text { get; set; }
    public List<string> Tags { get; set; }
    public string ImagePath { get; set; }
}   