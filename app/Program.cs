// See https://aka.ms/new-console-template for more information
using App.Graph;

Console.WriteLine("Hello, World!");

YamlReader.DataRoot = "C:\\Users\\Orphis\\Documents\\Dev\\alttp_vt_randomizer_bhaal\\app\\Graph\\data";
var items = YamlReader.LoadItems();
var edges = YamlReader.LoadEdgesFromDirectory("Edges\\base");
var entrances_nutty = YamlReader.LoadEntrances("nutty");
var entrances_simple = YamlReader.LoadEntrances("simple");
var entrances_vanilla = YamlReader.LoadEntrances("vanilla");
var vertices = YamlReader.LoadVerticesFromDirectory();

