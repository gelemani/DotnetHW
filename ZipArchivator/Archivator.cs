using System.IO.Compression;
using System.IO;
using System;
using System.Data;

namespace ZipArchivator;

class Archivator
{
    private const string zipPath = "blablabla.7z";

    public void Archive(string fileName)
    {
        // существует ли файл (валидация)
        // проверка не архив ли

        // заархивировать содержимое файла
        // сохранить

        Validate(fileName);

        FileStream originalStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        FileStream outputStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write);
        GZipStream gZipStream = new GZipStream(outputStream, CompressionMode.Compress); 
        originalStream.CopyTo(gZipStream);

        originalStream.Dispose();   
        outputStream.Dispose(); 
        gZipStream.Dispose();
    }

    public void Unarchive(string fileName)
    {
        FileStream originalStream = new FileStream(zipPath, FileMode.Open, FileAccess.Read);
        FileStream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
        GZipStream gZipStream = new GZipStream(originalStream, CompressionMode.Decompress);
        gZipStream.CopyTo(outputStream);

        originalStream.Dispose();
        outputStream.Dispose();
        gZipStream.Dispose();
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
