﻿using MovieScanToJson;
using System.Diagnostics;


string[] hds = { @"\\d1", @"\\d2", @"\\d3", @"\\d4", @"\\d5" };
// string[] mvs = { @"\\m1", @"\\m2", @"\\m3", @"\\m4", @"\\m5" };

void checkNas()
{
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
}


async Task readMoviesAndSaveToEncryptedJson()
{
    var movies = MovieGlobberAsync.GetAllMoviesSorted(hds);

    Console.WriteLine("Found " + movies.Count + " movies.");
    Console.WriteLine("Writing found movies to json file");
    await JsonHelper.SerializeToEncryptedFile<List<MovieModel>>(movies);
    Console.WriteLine($"Written encrypted json to {JsonHelper.cryptojsonfile}");
}


checkNas();
await readMoviesAndSaveToEncryptedJson();
