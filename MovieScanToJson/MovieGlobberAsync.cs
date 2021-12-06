using AMI = MovieScanToJson.AdditionalMovieInfo;

namespace MovieScanToJson
{
    public static class MovieGlobberAsync
    {
        private static async Task<MovieModel> AddInfoFileContentToMovieModel(MovieModel movie)
        {
            if (movie.FilePath == null) return movie;

            var infotext = await ReadInfoFileTextAsync(movie.FilePath);
            if (!string.IsNullOrEmpty(infotext))
            {
                AMI info = new(infotext);
                //to check if all fields are empty, compare to new empty AMI.
                if (AMI.Equals(info, new AMI())) Console.WriteLine("AMI is empty.");
                else movie.Info = info;
            }
            return movie;
        }

        private static async Task<string> ReadInfoFileTextAsync(string moviefilepath)
        {
            if (moviefilepath.Contains(".001")) moviefilepath = moviefilepath.Replace(".001", string.Empty);
            string infofilepath = Path.ChangeExtension(moviefilepath, "xml");
            if (!File.Exists(infofilepath)) return string.Empty;
            return await File.ReadAllTextAsync(infofilepath);
        }

        private static async Task<MovieModel> MovieModelFromFile(string filepath)
        {
            FileInfo file = new(filepath);
            MovieModel movie = new();
            string name = file.Name.Replace(".001", string.Empty).Replace(file.Extension, string.Empty);

            movie.Name = name;
            movie.FileSize = file.Length;
            movie.FilePath = filepath;
            movie = await AddInfoFileContentToMovieModel(movie);
            return movie;
        }

        private static async IAsyncEnumerable<MovieModel> GetAllMoviesAsync(string rootdir)
        {
            string[] extensions = { ".ts", ".mp4", ".mkv", ".tx", ".avi" };
            Queue<string> folders = new();
            folders.Enqueue(rootdir);
            EnumerationOptions enumOptions = new()
            {
                RecurseSubdirectories = false,
                IgnoreInaccessible = true,
                MatchType = MatchType.Simple
            };
            while (folders.Count > 0)
            {
                var current_dir = folders.Dequeue();
                foreach (var dir in Directory.EnumerateDirectories(current_dir, "*", enumOptions))
                {
                    folders.Enqueue(dir);
                }
                foreach (var file in Directory.EnumerateFiles(current_dir, "*", enumOptions))
                {
                    string ext = file[file.LastIndexOf('.')..];
                    if (extensions.Contains(ext))
                    {
                        Console.WriteLine("Processing " + file);
                        yield return await MovieModelFromFile(file);
                    }
                }
            }
        }

        public static List<MovieModel> GetAllMoviesSorted(string[] rootdirs)
        {
            List<MovieModel> movies = new();
            List<Task> tasks = new();
            foreach (var d in rootdirs)
            {
                tasks.Add(Task.Run(async () =>
                {
                    await foreach (var m in GetAllMoviesAsync(d))
                    {
                        movies.Add(m);
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine("Sorting list");
            // added ! NULL-toleranter Operator to bypass warning
            movies.Sort((a, b) => a!.Name!.CompareTo(b.Name));
            return movies;
        }
    }
}
