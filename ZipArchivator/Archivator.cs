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

        // вакуумирование и транспортировка вещей из шкафа на багажную ленту 
        ReadFileAndGZipProcess(sourceFileName, GZipCompress);

        // транспортировка вещей с ленты в чемодан
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

        // вакуумирование и транспортировка вещей из шкафа на багажную ленту 
        ReadFileAndGZipProcess(sourceFileName, GZipDecompress);

        // транспортировка вещей с ленты в чемодан
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
                _ = originalStream.Read(_buffer, 0, _buffer.Length); //из шкафа достаю 1 предмет (1мб)
                gzipAction(_buffer.Length);
            }

            //онли фор литл фингс (< 1 mb)
            _ = originalStream.Read(_buffer, 0, _remainder); // то что меньше 1 мб надо тоже положить 
            gzipAction(_remainder);
        }
    }

    private void GZipCompress(int chunkSize)
    {
        using (MemoryStream ms = new MemoryStream()) //управление памятью
        {
            using (GZipStream gZipStream = new GZipStream(ms, CompressionMode.Compress)) //вакуумируем вещь из шкафа
            {
                gZipStream.Write(_buffer, 0,
                    chunkSize); // buffer - вещь из шкафа (в руках держу) и выкачюваю воздух
            }
            var processedBuffer = ms.ToArray(); //Ура! вытянутый воздух 
            _blocksRead.Enqueue(processedBuffer); // кладем вакуумированную вещь на багажную ленту 
        }
    }
    
    private void GZipDecompress(int chunkSize)
    {
        using (MemoryStream ms = new MemoryStream()) //управление памятью
        {
            using (GZipStream gZipStream = new GZipStream(ms, CompressionMode.Decompress)) //вакуумируем вещь из шкафа
            {
                _ = gZipStream.Read(_buffer, 0,
                    chunkSize); // buffer - вещь из шкафа (в руках держу) и выкачюваю воздух
            }
            var processedBuffer = ms.ToArray(); //Ура! вытянутый воздух 
            _blocksRead.Enqueue(processedBuffer); // кладем вакуумированную вещь на багажную ленту 
        }
    }

    private void WriteBlocksToOutputFile(string destinationFileName)
    {
        using (FileStream outputStream = new FileStream(destinationFileName, FileMode.Create, FileAccess.Write))
        {
            while (_blocksRead.Count() > 0) // пока на ленте что-то есть
            {
                var currentBlock = _blocksRead.Dequeue(); // снять с багажной ленты
                outputStream.Write(currentBlock, 0, currentBlock.Length); // положить в чемодан
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