using System.IO.Compression;

namespace ZipArchivator;

class Archivator
{
    // private const string ZipPath = "blablabla.7z";
    // public const string LasertagMusicPath = "Hydrogen.mp3";
    private const int BufferSize = 1024 * 1024;
    private int _iterations;
    private int _remainder;
    private byte[] _buffer = new byte[BufferSize];
    private long _fileSize;
    private Queue<byte[]> _blocksRead = new();
        
    public void Archive(string sourceFileName)
    {
        Validate(sourceFileName);

        string destinationFileName = GetDestinationFileNameArchive(sourceFileName);

        _fileSize = new FileInfo(sourceFileName).Length; 
        _iterations = (int)(_fileSize / BufferSize);
        _remainder = (int)(_fileSize % BufferSize);

        _blocksRead = new(_iterations + 1);

        // �������������� � ��������������� ����� �� ����� �� �������� ����� 
        ReadFileAndGZipProcess(sourceFileName, GZipCompress);

        // ��������������� ����� � ����� � �������
        WriteBlocksToOutputFile(destinationFileName);
    }

    public void Unarchive(string sourceFileName)
    {
        Validate(sourceFileName);
        
        string destinationFileName = GetDestinationFileNameUnarchive(sourceFileName);
            
        _fileSize = new FileInfo(sourceFileName).Length; 
        _iterations = (int)(_fileSize / BufferSize);
        _remainder = (int)(_fileSize % BufferSize);
            
        _blocksRead = new(_iterations + 1);

        // �������������� � ��������������� ����� �� ����� �� �������� ����� 
        ReadFileAndGZipProcess(sourceFileName, GZipDecompress);

        // ��������������� ����� � ����� � �������
        WriteBlocksToOutputFile(destinationFileName);
    }

    private void ReadFileAndGZipProcess(
        string fileName,
        Action<int> gzipAction)
    {    
        using (FileStream originalStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            for (int i = 0; i < _iterations; i++)
            {
                _ = originalStream.Read(_buffer, 0, _buffer.Length); //�� ����� ������ 1 ������� (1��)
                gzipAction(_buffer.Length);
            }

            //���� ��� ���� ����� (< 1 mb)
            _ = originalStream.Read(_buffer, 0, _remainder); // �� ��� ������ 1 �� ���� ���� �������� 
            gzipAction(_remainder);
        }
    }

    private void GZipCompress(int chunkSize)
    {
        using (MemoryStream ms = new MemoryStream()) //���������� �������
        {
            using (GZipStream gZipStream = new GZipStream(ms, CompressionMode.Compress)) //����������� ���� �� �����
            {
                gZipStream.Write(_buffer, 0,
                    chunkSize); // buffer - ���� �� ����� (� ����� �����) � ��������� ������
            }
            var processedBuffer = ms.ToArray(); //���! ��������� ������ 
            _blocksRead.Enqueue(processedBuffer); // ������ ��������������� ���� �� �������� ����� 
        }
    }
    
    private void GZipDecompress(int chunkSize)
    {
        using (MemoryStream ms = new MemoryStream()) //���������� �������
        {
            using (GZipStream gZipStream = new GZipStream(ms, CompressionMode.Decompress)) //����������� ���� �� �����
            {
                _ = gZipStream.Read(_buffer, 0,
                    chunkSize); // buffer - ���� �� ����� (� ����� �����) � ��������� ������
            }
            var processedBuffer = ms.ToArray(); //���! ��������� ������ 
            _blocksRead.Enqueue(processedBuffer); // ������ ��������������� ���� �� �������� ����� 
        }
    }

    private void WriteBlocksToOutputFile(string destinationFileName)
    {
        using (FileStream outputStream = new FileStream(destinationFileName, FileMode.Create, FileAccess.Write))
        {
            while (_blocksRead.Count() > 0) // ���� �� ����� ���-�� ����
            {
                var currentBlock = _blocksRead.Dequeue(); // ����� � �������� �����
                outputStream.Write(currentBlock, 0, currentBlock.Length); // �������� � �������
            }
        }
    }

    private static string GetDestinationFileNameArchive(string sourceFileName)
    {
        return sourceFileName + ".gz";
    }
        
    private static string GetDestinationFileNameUnarchive(string sourceFileName)
    {
        return sourceFileName[0..^3];
    }
        
    static void Validate(string fileName)
    {
        if (!File.Exists(fileName))
        {
            throw new FileNotFoundException();
        }

        if (IfArchive(fileName))
        {
            throw new Exception();
        }
    }

    static bool IfArchive(string fileName)
    {
        return false;

    }
}