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
        // ���������� �� ���� (���������)
        // �������� �� ����� ��

        // �������������� ���������� �����
        // ���������

        Validate(fileName);

        using FileStream originalStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        using FileStream outputStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write);
        using GZipStream gZipStream = new GZipStream(outputStream, CompressionMode.Compress);
        originalStream.CopyTo(gZipStream);
    }

    public void Unarchive(string fileName)
    {
        using FileStream originalStream = new FileStream(zipPath, FileMode.Open, FileAccess.Read);
        using FileStream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
        using GZipStream gZipStream = new GZipStream(originalStream, CompressionMode.Decompress);
        gZipStream.CopyTo(outputStream);
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
