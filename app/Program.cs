// See https://aka.ms/new-console-template for more information
using System.CommandLine;
using App.Console.Commands;
using App.Graph;

YamlReader.DataRoot = Path.GetFullPath(@"../../../Graph/data");

var alttpr = new RootCommand("The Legend of Zelda: A Link to the Past Randomizer");
alttpr.AddCommand(new Randomize());

return alttpr.Invoke(args);
