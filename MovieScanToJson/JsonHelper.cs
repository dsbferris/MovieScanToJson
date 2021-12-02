using System.Text.Json;

namespace MovieScanToJson
{
    public static class JsonHelper
    {
        private const string jsonfile = "movies.json";
        private static readonly JsonSerializerOptions options = new() { WriteIndented=true};

        public static async void WriteToFileAsync<T>(T data, string filepath = jsonfile)
        {
            using FileStream fs = File.Create(filepath);
            await JsonSerializer.SerializeAsync(fs, data, options);
        }

        public static async Task<T?> ReadFromFileAsync<T>(string filepath = jsonfile)
        {
            using FileStream fs = File.OpenRead(filepath);
            return await JsonSerializer.DeserializeAsync<T>(fs, options);
        }

        public static async void WriteToStream<T>(T data, Stream stream)
        {
            await JsonSerializer.SerializeAsync(stream, data, options);
        }

        public static async Task<T?> ReadFromStreamAsync<T>(Stream stream)
        {
            return await JsonSerializer.DeserializeAsync<T>(stream, options);
        }
    }
}
