using System.Text.Json;

namespace MovieScanToJson
{
    public static class JsonHelper
    {
        public static readonly string jsonfile = "movies.json";

        public static async void WriteToFileAsync<T>(T data, string filepath)
        {
            using FileStream fs = File.Create(filepath);
            await JsonSerializer.SerializeAsync(fs, data, new JsonSerializerOptions() { WriteIndented=true});
        }

        public static async Task<T?> ReadFromFileAsync<T>(string filepath)
        {
            using FileStream fs = File.OpenRead(filepath);
            return await JsonSerializer.DeserializeAsync<T>(fs, new JsonSerializerOptions() { WriteIndented=true});
        }

        public static async void WriteToStream<T>(T data, Stream stream)
        {
            await JsonSerializer.SerializeAsync(stream, data);
        }

        public static async Task<T?> ReadFromStreamAsync<T>(Stream stream)
        {
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
    }
}
