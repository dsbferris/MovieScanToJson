
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

List<List<string>> list_of_list_of_folders = new();
list_of_list_of_folders.Add(new () {
    @"\\192.168.178.39\hd1", @"\\192.168.178.39\hd2", @"\\192.168.178.39\hd3", @"\\192.168.178.39\hd4", @"\\192.168.178.39\hd5" });
list_of_list_of_folders.Add(new () {
    @"\\192.168.178.39\movies1", @"\\192.168.178.39\movies2", @"\\192.168.178.39\movies3", @"\\192.168.178.39\movies4", @"\\192.168.178.39\movies5"});

foreach(var list_of_folders in list_of_list_of_folders)
{

}