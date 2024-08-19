using System;

namespace ZipArchivator;

class Program
{
    private const string originalPath = "Hydrogen.mp3";
    private const string decompressedPath = "Hydrogen2.mp3";

    public static void Main(string[] args)
    {
        Archivator archivator = new Archivator();
        archivator.Archive(originalPath);
        archivator.Unarchive(decompressedPath);
    }
}
