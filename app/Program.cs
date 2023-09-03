// See https://aka.ms/new-console-template for more information
using System.CommandLine;
using App.Console.Commands;
using App.Graph;

Console.WriteLine("Hello, World!");

YamlReader.DataRoot = Path.GetFullPath(@"..\..\..\Graph\data");
var items = YamlReader.LoadItems();
var edges = YamlReader.LoadEdgesFromDirectory("Edges\\base");
var entrances_nutty = YamlReader.LoadEntrances("nutty");
var entrances_simple = YamlReader.LoadEntrances("simple");
var entrances_vanilla = YamlReader.LoadEntrances("vanilla");
var vertices = YamlReader.LoadVerticesFromDirectory();


var alttpr = new RootCommand("The Legend of Zelda: A Link to the Past Randomizer");
alttpr.AddCommand(new Randomize());

return alttpr.Invoke(args);
