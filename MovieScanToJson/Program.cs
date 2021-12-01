
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

List<List<string>> list_of_list_of_folders = new();
list_of_list_of_folders.Add(new () {
    @"\\location1", @"\\location2", @"\\location3", @"\\location4", @"\\location5" });
list_of_list_of_folders.Add(new () {
    @"\\movies1", @"\\movies2", @"\\movies3", @"\\movies4", @"\\movies5"});

foreach(var list_of_folders in list_of_list_of_folders)
{

}