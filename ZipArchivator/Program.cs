namespace ZipArchivator;

class Program
{
    private const string originalPath = "Hydrogen.mp3";
    private const string decompressedPath = "Hydrogen.mp3.gz";

    public static void Main(string[] args)
    {
        Archivator archivator = new Archivator();
        archivator.Archive(originalPath);
        Thread.Sleep(5000);
        archivator.Unarchive(decompressedPath);
        
        Console.WriteLine("Finished!");
        
        // 1-ый поток берет кусок исходного файла (мп3), архивирует и записывает в некую очередь
        // 2-ой поток берет элемент из очереди и записывает в итоговый файл (архив)
        // статический объект синхронизации и ConcurrentQueue<T> Класс
    }
}
