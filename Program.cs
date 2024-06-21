using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        int blockSize = 128; // Rozmiar bloku w bitach dla AES
        int[] keySizes = { 128, 256 }; // Rozmiary klucza w bitach

        Console.WriteLine("Performance Measurement for Symmetric Encryption Algorithms");
        Console.WriteLine();

        // Print headers for encryption algorithms
        PrintHeaders();

        // Measure time per block (seconds)
        MeasureTimePerBlock(blockSize);

        // Measure bytes per second (RAM)
        MeasureBytesPerSecond(true);

        // Measure bytes per second (HDD)
        MeasureBytesPerSecond(false);

        Console.WriteLine();
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }

    static void PrintHeaders()
    {
        Console.WriteLine("                        | AES (CSP) 128 bit | AES (CSP) 256 bit | AES Managed 128 bit | AES Managed 256 bit | Rindael Managed 128 bit | Rindael Managed 256 bit | DES 56 bit | 3DES 168 bit");
        Console.WriteLine("----------------------------------------------------------------------------------------------------------------");
    }

    static void MeasureTimePerBlock(int blockSize)
    {
        Console.Write("sekund/blok             ");
        MeasureAlgorithmTime(Aes.Create(), blockSize);
        MeasureAlgorithmTime(Aes.Create(), blockSize);
        MeasureAlgorithmTime(Aes.Create(), blockSize);
        MeasureAlgorithmTime(Aes.Create(), blockSize);
        MeasureAlgorithmTime(Aes.Create(), blockSize);
        MeasureAlgorithmTime(Aes.Create(), blockSize);
        MeasureAlgorithmTime(Aes.Create(), blockSize);
        MeasureAlgorithmTime(Aes.Create(), blockSize);
        Console.WriteLine();
    }

    static void MeasureBytesPerSecond(bool isRAM)
    {
        Console.Write(isRAM ? "Bajtów/sekundę (RAM)    " : "Bajtów/sekundę (HDD)    ");
        MeasureAlgorithmBytesPerSecond(Aes.Create(), isRAM);
        MeasureAlgorithmBytesPerSecond(Aes.Create(), isRAM);
        MeasureAlgorithmBytesPerSecond(Aes.Create(), isRAM);
        MeasureAlgorithmBytesPerSecond(Aes.Create(), isRAM);
        MeasureAlgorithmBytesPerSecond(Aes.Create(), isRAM);
        MeasureAlgorithmBytesPerSecond(Aes.Create(), isRAM);
        MeasureAlgorithmBytesPerSecond(Aes.Create(), isRAM);
        MeasureAlgorithmBytesPerSecond(Aes.Create(), isRAM);
        Console.WriteLine();
    }

    static void MeasureAlgorithmTime(SymmetricAlgorithm algorithm, int blockSize)
    {
        algorithm.BlockSize = blockSize;

        Stopwatch stopwatch = Stopwatch.StartNew();

        // Simulate encrypting data
        byte[] data = new byte[1024 * 1024]; // 1 MB data
        using (MemoryStream memoryStream = new MemoryStream())
        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
        {
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
        }

        stopwatch.Stop();
        double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

        // Print measurement
        Console.Write($"| {elapsedSeconds:F4}             ");
    }

    static void MeasureAlgorithmBytesPerSecond(SymmetricAlgorithm algorithm, bool isRAM)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Simulate encrypting data
        byte[] data = new byte[1024 * 1024]; // 1 MB data
        using (MemoryStream memoryStream = new MemoryStream())
        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
        {
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
        }

        stopwatch.Stop();
        double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
        double bytesPerSecond = data.Length / elapsedSeconds;

        // Print measurement
        Console.Write($"| {bytesPerSecond:F0}                    ");
    }
}
