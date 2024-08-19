using System;

namespace ZipArchivator;

class Program
{
    private const string originalPath = "index.html";
    private const string decompressedPath = "index2.html";

    public static void Main(string[] args)
    {
        Archivator archivator = new Archivator();
        archivator.Archive(originalPath);
        archivator.Unarchive(decompressedPath);
    }
}
