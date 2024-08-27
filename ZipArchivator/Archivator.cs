using System.IO.Compression;
using System.IO;
using System;
using System.Data;

namespace ZipArchivator;

class Archivator
{
    private const string zipPath = "blablabla.7z";
    public const string lasertagMusicPath = "Hydrogen.mp3";

    public void Archive(string fileName)
    {
        // существует ли файл (валидация)
        // проверка не архив ли

        // заархивировать содержимое файла
        // сохранить

        Validate(fileName);

        //using FileStream originalStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //using FileStream outputStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write);
        //using GZipStream gZipStream = new GZipStream(outputStream, CompressionMode.Compress);
        //originalStream.CopyTo(gZipStream);

        var buffer = new byte[1024 * 1024];
        using (FileStream originalStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            using (FileStream outputStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write))
            {
                int bytesRead;
                while ((bytesRead = originalStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    using (MemoryStream ms = new MemoryStream()) 
                    {
                        using (GZipStream gZipStream = new GZipStream(ms, CompressionMode.Compress))
                        { 
                            gZipStream.Write(buffer, 0, buffer.Length);
                        }
                        var compressedBuffer = ms.ToArray();
                        outputStream.Write(compressedBuffer, 0, compressedBuffer.Length);
                    }
                }
            }
        }
    }

    public void Unarchive(string zipPath)
    {
        //using FileStream originalStream = new FileStream(zipPath, FileMode.Open, FileAccess.Read);
        //using FileStream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
        //using GZipStream gZipStream = new GZipStream(originalStream, CompressionMode.Decompress);
        //gZipStream.CopyTo(outputStream);

        var buffer = new byte[1024 * 1024];
        using (FileStream originalStream = new FileStream(zipPath, FileMode.Open, FileAccess.Read))
        {
            using (FileStream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                int bytesRead;
                while ((bytesRead = originalStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (GZipStream gZipStream = new GZipStream(ms, CompressionMode.Compress))
                        {
                            gZipStream.Write(buffer, 0, buffer.Length);
                        }
                        var compressedBuffer = ms.ToArray();
                        outputStream.Write(compressedBuffer, 0, compressedBuffer.Length);
                    }
                }
            }
        }


    static void Validate(string fileName)
    {
        if (!File.Exists(fileName))
        {
            throw new FileNotFoundException();
        }
        if (ifArchive(fileName))
        {
            throw new Exception();
        }
    }

    static bool ifArchive(string fileName)
    {
        return false;
    }    
}
