using MovieScanToJson;
using System.Diagnostics;


string[] hds = { @"\\192.168.178.39\hd1", @"\\192.168.178.39\hd2", @"\\192.168.178.39\hd3", @"\\192.168.178.39\hd4", @"\\192.168.178.39\hd5" };
// string[] mvs = { @"\\192.168.178.39\movies1", @"\\192.168.178.39\movies2", @"\\192.168.178.39\movies3", @"\\192.168.178.39\movies4", @"\\192.168.178.39\movies5" };

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
                psi.Arguments = @"\\192.168.178.39\";
                Process p = new() { StartInfo = psi };
                p.Start();
                Console.WriteLine("Press enter to continue after login.");
                Console.ReadLine();
            }
        }
    }
}

async Task readAndEncryptFileAndExit()
{
    var json_movies = await JsonHelper.DeserializeFromFileAsync<List<MovieModel>>();
    if (json_movies == null)
    {
        Console.WriteLine("json_movies is null...");
        return;
    }
    else
    {
        await JsonHelper.SerializeToEncryptedFile<List<MovieModel>>(json_movies);
        var crypto_movies = await JsonHelper.DeserializeFromEncryptedFile<List<MovieModel>>();
        if (crypto_movies == null)
        {
            Console.WriteLine("crypto_movies is null...");
            return;
        }
        else
        {
            Console.WriteLine("About to exit");
            return;
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

// await readAndEncryptFileAndExit();
checkNas();
await readMoviesAndSaveToEncryptedJson();
