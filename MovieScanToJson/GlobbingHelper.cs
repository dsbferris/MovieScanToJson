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
            //TODO ADD EXTENSION
            string infofilepath = Path.ChangeExtension(moviefilepath, "INFOFILEEXTENSION!");
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
            movie = AddInfoFileContentToMovieModel((MovieModel)movie);
            return movie;
        }

        public static IEnumerable<MovieModel>? GlobMovies(string rootdir)
        {
            Matcher matcher = new();
            matcher.AddIncludePatterns(new[] { "*.mp4", "*.mkv", "*.ts", "*.tx", "*.avi" });
            PatternMatchingResult result = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(rootdir)));
            if (!result.HasMatches) yield break;
            else
            {
                foreach(var file in result.Files)
                {
                    yield return MovieModelFromFile(Path.Combine(rootdir, file.Path));
                }
            }
        }
    }
}
