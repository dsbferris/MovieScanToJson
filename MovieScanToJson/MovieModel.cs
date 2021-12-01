using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieScanToJson
{
    public class MovieModel
    {
        public bool Favourite { get; set; }

        public string? Name { get; set; }
        public long FileSize { get; set; }
        public string? FilePath { get; set; }
        public int? RuntimeSeconds { get; set; }
        public string? Description { get; set; }
        public int? ReleaseYear { get; set; }


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

	public class MovieList
    {
		public List<MovieModel>? Movies { get; set; }
    }
}
