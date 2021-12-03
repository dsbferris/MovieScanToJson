using Microsoft.Extensions.FileSystemGlobbing;
using MovieScanToJson;
using System.Diagnostics;
using System.Text;


#region Explorer NAS Login

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

#endregion


List<List<string>> list_of_list_of_folders = new();
list_of_list_of_folders.Add(new () {
    @"\\192.168.178.39\hd1", @"\\192.168.178.39\hd2", @"\\192.168.178.39\hd3", @"\\192.168.178.39\hd4", @"\\192.168.178.39\hd5" });
list_of_list_of_folders.Add(new () {
    @"\\192.168.178.39\movies1", @"\\192.168.178.39\movies2", @"\\192.168.178.39\movies3", @"\\192.168.178.39\movies4", @"\\192.168.178.39\movies5"});

var sample_dir = list_of_list_of_folders[0][0];

List<MovieModel> movies = new();
List<Task> tasks = new();

var result = await Task.Run(() => GlobbingHelper.GlobMovies(sample_dir));
foreach(var folder in list_of_list_of_folders[0])
{
    Console.WriteLine("Starting task for " + folder);
#pragma warning disable CS8604 // Mögliches Nullverweisargument.
    movies.AddRange(await Task.Run(() => GlobbingHelper.GlobMovies(folder)));
#pragma warning restore CS8604 // Mögliches Nullverweisargument.
}

Console.WriteLine("Sorting list");
#pragma warning disable CS8602 // Dereferenzierung eines möglichen Nullverweises.
movies.Sort((a, b) => a.Name.CompareTo(b.Name));
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.

foreach (var movie in movies)
{
    Console.WriteLine(movie.Name);
}
Console.WriteLine("Found " + movies.Count + " movies.");
Console.WriteLine("Writing found movies to json file");
JsonHelper.WriteToFileAsync<List<MovieModel>>(movies);