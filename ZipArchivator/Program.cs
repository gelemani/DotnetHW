using System;

namespace ZipArchivator;

class Program
{
    private const string originalPath = "C:\\Users\\1\\OneDrive\\Документы\\root\\portfolio\\index.html";
    
    public static void Main(string[] args)
    {
        Archivator archivator = new Archivator();
        //archivator.Archive(originalPath);
        archivator.Unarchive("index.html");
    }
}
