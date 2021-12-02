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
        psi.Arguments = @"\\sambaserver\";
        Process p = new() { StartInfo = psi };
        p.Start();
        Console.WriteLine("Press enter to continue after login.");
        Console.ReadLine();
    }
}

#endregion


List<List<string>> list_of_list_of_folders = new();
list_of_list_of_folders.Add(new () {
    @"\\location1", @"\\location2", @"\\location3", @"\\location4", @"\\location5" });
list_of_list_of_folders.Add(new () {
    @"\\movies1", @"\\movies2", @"\\movies3", @"\\movies4", @"\\movies5"});

var dir = list_of_list_of_folders[0][0];
var movies_enum = GlobbingHelper.GlobMovies(dir);
List<MovieModel> movies;
if (movies_enum != null)
{
    movies = movies_enum.ToList();
    Console.WriteLine("Found " + movies.Count + " movies");
}
Console.ReadLine();
return;