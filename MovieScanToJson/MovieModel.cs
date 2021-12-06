namespace MovieScanToJson
{
    public class MovieModel
    {
        public string? Name { get; set; }
        public long FileSize { get; set; }
        public string? FilePath { get; set; }
        public AdditionalMovieInfo? Info { get; set; }

        public static string GetBytesReadable(long? value)
        {
            if (!value.HasValue) return string.Empty;
            long i = value.Value;

            // Get absolute value
            long absolute_i = (i < 0 ? -i : i);
            // Determine the suffix and readable value
            string suffix;
            double readable;
            if (absolute_i >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                readable = (i >> 50);
            }
            else if (absolute_i >= 0x4000000000000) // Petabyte
            {
                suffix = "PB";
                readable = (i >> 40);
            }
            else if (absolute_i >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = (i >> 30);
            }
            else if (absolute_i >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = (i >> 20);
            }
            else if (absolute_i >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (i >> 10);
            }
            else if (absolute_i >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = i;
            }
            else
            {
                return i.ToString("0 B"); // Byte
            }
            // Divide by 1024 to get fractional value
            readable /= 1024;
            // Return formatted number with suffix
            return readable.ToString("0.## ") + suffix;
        }
    }

    public class AdditionalMovieInfo
    {
        /// <summary>
        /// epgtitle
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// info1
        /// </summary>
        public string? Genre { get; set; }

        /// <summary>
        /// info2
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// parentallockage
        /// </summary>
        public int? AgeRating { get; set; }

        /// <summary>
        /// reclength
        /// </summary>
        public int? LengthSeconds { get; set; }

        /// <summary>
        /// length
        /// </summary>
        public int? LengthMinutes { get; set; }

        /*
			<audiopids selected="1024">
				<audio pid="1024" name="Deutsch"/>
				<audio pid="1025" name="Englisch"/>
				<audio pid="1027" name="Dolby Digital 5.1 (AC3)"/>
			</audiopids>
		 */
        /// <summary>
        /// audiopids->audio name="..."
        /// </summary>
        public string[]? AudioLanguages { get; set; }

        /// <summary>
        /// seriename
        /// </summary>
        public string? SerienName { get; set; }



        public const string XmlTitle = "epgtitle";
        public const string XmlGenre = "info1";
        public const string XmlDescription = "info2";
        public const string XmlAgeRating = "parentallockage";
        public const string XmlLengthSeconds = "reclength";
        public const string XmlLengthMinutes = "length";
        public const string XmlAudios = "audiopids";
        public const string XmlAudio = "audio";
        public const string XmlSerienName = "seriename";

        private static string GetBegin(string attribute) => "<" + attribute + ">";
        private static string GetEnd(string attribute) => "</" + attribute + ">";

        private static string? GetContent(string infotext, string attribute, bool replaceLineEnding = true)
        {
            if (!string.IsNullOrEmpty(infotext))
            {
                if (!string.IsNullOrEmpty(attribute))
                {
                    var attr_begin = GetBegin(attribute);
                    var attr_end = GetEnd(attribute);
                    var begin = infotext.IndexOf(attr_begin);
                    var end = infotext.LastIndexOf(attr_end);
                    if (begin != -1 && end != -1)
                    {
                        //vllt index mit +- 1 justieren
                        var text = infotext.Substring(begin + attr_begin.Length, end - begin - attr_end.Length + 1);
                        if (replaceLineEnding) text = text.ReplaceLineEndings(" ");
                        text = text.Trim();
                        return string.IsNullOrWhiteSpace(text) ? null : text;
                    }
                }
            }
            return null;
        }

        private static string[]? GetAudios(string infotext)
        {
            var audiopids = GetContent(infotext, XmlAudios, false);
            if (!string.IsNullOrEmpty(audiopids))
            {
                var audios = new List<string>();
                //The | makes use of both options
                foreach (var line in audiopids.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    var attr_begin = "name=\"";
                    var attr_end = "\"";
                    int begin = line.IndexOf(attr_begin);
                    int end = line.LastIndexOf(attr_end);
                    var text = line.Substring(begin + attr_begin.Length, end - begin - attr_begin.Length - attr_end.Length + 1);
                    audios.Add(text);
                }
                if (audios.Count > 0) return audios.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Initalizes AMI and fills in all Infos from Infotext
        /// </summary>
        /// <param name="infotext">Infotext from Infofile for movie</param>
        public AdditionalMovieInfo(string infotext)
        {
            Title = GetContent(infotext, XmlTitle);
            Genre = GetContent(infotext, XmlGenre);
            Description = GetContent(infotext, XmlDescription);
            SerienName = GetContent(infotext, XmlSerienName);

            if (int.TryParse(GetContent(infotext, XmlLengthSeconds), out int seconds)) LengthSeconds = seconds;
            if (int.TryParse(GetContent(infotext, XmlLengthMinutes), out int minutes)) LengthMinutes = minutes;
            if (int.TryParse(GetContent(infotext, XmlAgeRating), out int age)) AgeRating = age;

            AudioLanguages = GetAudios(infotext);
        }

        /// <summary>
        /// Empty constructor for Comparison of AMI Class
        /// </summary>
        public AdditionalMovieInfo()
        {

        }
    }

}
