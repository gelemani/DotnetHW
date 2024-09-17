using System.IO.Compression;
using System.IO;
using System;
using System.Collections.Generic;
using System.Data;

namespace ZipArchivator
{
    class Archivator
    {
        private const string zipPath = "blablabla.7z";
        public const string lasertagMusicPath = "Hydrogen.mp3";
        private const int _bufferSize = 1024 * 1024;

        public void Archive(string fileName)
        {
            Validate(fileName);

            var fileSize = new FileInfo(fileName).Length;
            int iterations = (int)(fileSize / _bufferSize);
            int remainder = (int)(fileSize % _bufferSize);
            var buffer = new byte[_bufferSize];

            Queue<byte[]> blocksRead = new(iterations + 1);

            // вакуумирование и транспортировка вещей из шкафа на багажную ленту 
            ReadFileAndArchiveBlocks(fileName, iterations, remainder, buffer, blocksRead);

            // транспортировка вещей с ленты в чемодан
            WriteBlocksToOutputFile(blocksRead);
        }

        private void ReadFileAndArchiveBlocks(string fileName, int iterations, int remainder, byte[] buffer, Queue<byte[]> blocksRead)
        {
            using (FileStream originalStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                int bytesRead;
                for (int i = 0; i < iterations; i++)
                {
                    bytesRead = originalStream.Read(buffer, 0, buffer.Length); //из шкафа достаю 1 предмет (1мб)
                    using (MemoryStream ms = new MemoryStream()) //управление памятью
                    {
                        using (GZipStream gZipStream = new GZipStream(ms, CompressionMode.Compress)) //вакуумируем вещь из шкафа
                        {
                            gZipStream.Write(buffer, 0, buffer.Length); // buffer - вещь из шкафа (в руках держу) и выкачюваю воздух
                        }
                        var compressedBuffer = ms.ToArray(); //Ура! вытянутый воздух 
                        blocksRead.Enqueue(compressedBuffer); // кладем вакуумированную вещь на багажную ленту
                    }
                }

                //онли фор литл фингс (< 1 mb)
                bytesRead = originalStream.Read(buffer, 0, remainder); // то что меньше 1 мб надо тоже положить 
                using (MemoryStream ms = new MemoryStream()) //управление памятью
                {
                    using (GZipStream gZipStream = new GZipStream(ms, CompressionMode.Compress)) //вакуумируем вещь из шкафа
                    {
                        gZipStream.Write(buffer, 0, remainder); // из той маленькой вещи (которую в руках держу) я выкачюваю воздух
                    }
                    var compressedBuffer = ms.ToArray(); //Ура! вытянутый воздух 
                    blocksRead.Enqueue(compressedBuffer); // кладем вакуумированную вещь на багажную ленту
                }

            }
        }
        
        private static void WriteBlocksToOutputFile(Queue<byte[]> blocksRead)
        {
            using (FileStream outputStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write))
            {
                while (blocksRead.Count() > 0) // пока на ленте что-то есть
                {
                    var currentBlock = blocksRead.Dequeue(); // снять с багажной ленты
                    outputStream.Write(currentBlock, 0, currentBlock.Length); // положить в чемодан
                }
            }
        }

        public void Unarchive(string zipPath)
        {
            Validate(zipPath);

            //using FileStream originalStream = new FileStream(zipPath, FileMode.Open, FileAccess.Read);
            //using FileStream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            //using GZipStream gZipStream = new GZipStream(originalStream, CompressionMode.Decompress);
            //gZipStream.CopyTo(outputStream);

            string fileName = lasertagMusicPath;
            var fileSize = new FileInfo(zipPath).Length;
            var buffer = new byte[_bufferSize];

            using (FileStream originalStream = new FileStream(zipPath, FileMode.Open, FileAccess.Read))
            {
                using (FileStream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    int bytesRead;
                    int iterations = (int)(fileSize / _bufferSize);
                    int remainder = (int)(fileSize % _bufferSize);

                    Queue<byte[]> blocksRead = new(iterations + 1);

                    for (int i = 0; i < iterations; i++)
                    {
                        bytesRead = originalStream.Read(buffer, 0, buffer.Length);
                        blocksRead.Enqueue((byte[])buffer.Clone());
                    }

                    bytesRead = originalStream.Read(buffer, 0, remainder);
                    blocksRead.Enqueue((byte[])buffer.Clone());

                    while (blocksRead.Count() > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            var currentBlock = blocksRead.Dequeue();
                            using (GZipStream gZipStream = new GZipStream(ms, CompressionMode.Compress))
                            {
                                gZipStream.Write(currentBlock, 0, currentBlock.Length);
                            }

                            var compressedBuffer = ms.ToArray();
                            outputStream.Write(compressedBuffer, 0, compressedBuffer.Length);
                        }
                    }

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
}
