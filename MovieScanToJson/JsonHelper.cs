using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System;

namespace MovieScanToJson
{
    public static class JsonHelper
    {
        public const string jsonfile = "movies.json";
        public const string cryptojsonfile = "movies.json.encrypted";

        private const string KeyString = "WatchingMovies12"; //256-bit Key
        private static readonly byte[] Key = Encoding.Unicode.GetBytes(KeyString);

        private static readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true };
        private static readonly FileStreamOptions writeOptions = new()
        {
            Mode = FileMode.CreateNew,
            Access = FileAccess.Write,
            Options = FileOptions.Asynchronous
        };
        private static readonly FileStreamOptions readOptions = new()
        {
            Access = FileAccess.Read,
            Mode = FileMode.Open,
            Options = FileOptions.Asynchronous
        };

        public static async Task SerializeToFileAsync<T>(T data, string filepath = jsonfile)
        {
            using FileStream fs = File.Create(filepath);
            await JsonSerializer.SerializeAsync(fs, data, jsonOptions);
        }

        public static async Task<T?> DeserializeFromFileAsync<T>(string filepath = jsonfile)
        {
            using FileStream fs = File.OpenRead(filepath);
            return await JsonSerializer.DeserializeAsync<T>(fs, jsonOptions);
        }

        public static async Task SerializeToEncryptedFile<T>(T data, string filepath = cryptojsonfile)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            await using FileStream fs = new(filepath, writeOptions);

            //write IV to begin of file
            await fs.WriteAsync(aes.IV.AsMemory(0, aes.IV.Length));

            await using CryptoStream cs = new(fs, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await JsonSerializer.SerializeAsync<T>(cs, data, jsonOptions);
        }

        public static async Task<T?> DeserializeFromEncryptedFile<T>(string filepath = cryptojsonfile)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            await using FileStream fs = new(filepath, readOptions);

            //read IV from begin of file
            byte[] iv = new byte[aes.IV.Length];
            int numBytesToRead = aes.IV.Length;
            int numBytesAlreadyRead = 0;
            while(numBytesToRead > 0)
            {
                int n = await fs.ReadAsync(iv.AsMemory(numBytesAlreadyRead, numBytesToRead));
                if (n == 0) break;
                numBytesAlreadyRead += n;
                numBytesToRead -= n;
            }
            aes.IV = iv;

            await using CryptoStream cs = new(fs, aes.CreateDecryptor(), CryptoStreamMode.Read);
            return await JsonSerializer.DeserializeAsync<T>(cs, jsonOptions);
        }
    }
}
