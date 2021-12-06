using MovieScanToJson;
using System.Diagnostics;


string[] hds = { @"\\location1", @"\\location2", @"\\location3", @"\\location4", @"\\location5" };
// string[] mvs = { @"\\movies1", @"\\movies2", @"\\movies3", @"\\movies4", @"\\movies5" };


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
            psi.Arguments = @"\\sambaserver\";
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