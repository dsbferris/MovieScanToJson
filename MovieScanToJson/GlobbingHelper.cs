using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AMI = MovieScanToJson.AdditionalMovieInfo;

namespace MovieScanToJson
{
    public static class GlobbingHelper
    {

        private static MovieModel AddInfoFileContentToMovieModel(MovieModel movie)
        {
            if (movie.FilePath == null) return movie;

            var infotext = ReadInfoFileText(movie.FilePath);
            if (!string.IsNullOrEmpty(infotext))
            {
                
                AMI info = new();

                foreach (var line in infotext.Split('\n', StringSplitOptions.TrimEntries))
                {
                    if (line.Contains(AMI.XmlTitle))
                    {
                        //info.Title = AMI.GetLineContent(line);
                    }
                    else if (line.Contains(AMI.XmlGenre))
                    {
                        //info.Genre = AMI.GetLineContent(line);
                    }
                    else if (line.Contains(AMI.XmlDescription))
                    {
                        //THIS WILL NOT WORK WITH LINES....
                        //REDO THIS HOLE THING
                        //info.Description = AMI.GetLineContent(line);
                    }
                    else if (line.Contains(AMI.XmlAgeRating))
                    {
                        //var age_string = AMI.GetLineContent(line);
                        //int age = -1;
                        //int.TryParse(age_string, out age);
                        //if (age != -1) info.AgeRating = age;
                    }
                    else if (line.Contains(AMI.XmlLengthSeconds))
                    {
                        //Set Seconds
                    }
                    else if(info.LengthSeconds == null && line.Contains(AMI.XmlLengthMinutes))
                    {
                        //Set Minutes
                    }
                    else if (line.Contains(AMI.XmlSerienName))
                    {
                        //Set Serienname
                    }
                    else if (line.Contains(AMI.XmlAudio))
                    {
                        //Add Audio
                    }

                    
                    /*
                    epgtitle        - Title
                    info1           - Genre
                    info2           - Desc
                    parentallockage - AgeRating
                    length          - LengthMinutes
                    reclength       - LengthSeconds
                    seriename       - SerienName
                    audiopids->audio name=".." AudioLanguages[]
                        */

                    
                }
                if (AMI.Equals(info, new AMI())) Console.WriteLine("AMI is blank.");
                else movie.Info = info;
            }
            return movie;
        }

        private static string ReadInfoFileText(string moviefilepath)
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
            string name = file.Name.Replace(".001", string.Empty).Replace(file.Extension, string.Empty);

            movie.Name = name;
            movie.FileSize = file.Length;
            movie.FilePath = filepath;
            movie = AddInfoFileContentToMovieModel((MovieModel)movie);
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
