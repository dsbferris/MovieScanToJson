using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieScanToJson
{
    public static class GlobbingHelper
    {

        private static MovieModel AddInfoFileContentToMovieModel(MovieModel movie)
        {
            if (movie.FilePath == null) return movie;

            _ = GetInfoFileText(movie.FilePath);
            //TODO IMPLEMENT FILE SCRAPPING!
            throw new NotImplementedException();
        }

        private static string GetInfoFileText(string moviefilepath)
        {
            if (moviefilepath.Contains(".001")) moviefilepath = moviefilepath.Replace(".001", string.Empty);
            string infofilepath = Path.ChangeExtension(moviefilepath, "xml");
            if (!File.Exists(infofilepath)) return string.Empty;
            return File.ReadAllText(infofilepath);
        }

        private static MovieModel MovieModelFromFile(string filepath)
        {
            FileInfo file = new(filepath);
            MovieModel movie = new();
            //TODO CHECK FOR INFO FILE AT THE SAME LOCATION!
            movie.Name = file.Name;
            movie.FileSize = file.Length;
            movie.FilePath = filepath;
            //movie = AddInfoFileContentToMovieModel((MovieModel)movie);
            return movie;
        }

        public static IEnumerable<MovieModel>? GlobMovies(string rootdir)
        {
            List<string> extensions = new() { "*.mp4", "*.mkv", "*.ts", "*.tx", "*.avi" };
            List<string> subdir_extensions = new();
            foreach(var ext in extensions)
            {
                subdir_extensions.Add("*/*" + ext);
            }
            extensions.AddRange(subdir_extensions);

            Matcher matcher = new();
            matcher.AddIncludePatterns(extensions);
            var files = matcher.GetResultsInFullPath(rootdir);
            foreach (var file in files)
            {
                Console.WriteLine("Processing " + file);
                yield return MovieModelFromFile(file);
            }
        }
    }
}
