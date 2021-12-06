using MovieScanToJson;
using System.Diagnostics;


string[] hds = { @"\\192.168.178.39\hd1", @"\\192.168.178.39\hd2", @"\\192.168.178.39\hd3", @"\\192.168.178.39\hd4", @"\\192.168.178.39\hd5" };
// string[] mvs = { @"\\192.168.178.39\movies1", @"\\192.168.178.39\movies2", @"\\192.168.178.39\movies3", @"\\192.168.178.39\movies4", @"\\192.168.178.39\movies5" };


#region Explorer NAS Login

if (!Directory.Exists(hds[0]))
{
    Console.WriteLine("Remember to first login into NAS before accessing it!");
    Console.WriteLine("Do you want to open explorer to login? (y/n)");
    var yesno = Console.ReadLine();
    if (yesno != null)
    {
        if (yesno.ToLower() == "yes" || yesno.ToLower() == "y")
        {
            ProcessStartInfo psi = new();
            psi.FileName = @"explorer.exe";
            psi.Arguments = @"\\192.168.178.39\";
            Process p = new() { StartInfo = psi };
            p.Start();
            Console.WriteLine("Press enter to continue after login.");
            Console.ReadLine();
        }
    }
}

#endregion


var movies = MovieGlobberAsync.GetAllMoviesSorted(hds);

Console.WriteLine("Found " + movies.Count + " movies.");
Console.WriteLine("Writing found movies to json file");
await JsonHelper.WriteToFileAsync<List<MovieModel>>(movies);
Console.WriteLine($"Written json to {JsonHelper.jsonfile}");