Console.WriteLine("Hello, World!");
Console.OutputEncoding = System.Text.Encoding.UTF8;

string name = "Розанов Владислав Александрович";
string group = "9/3-РПО-24/1";

Console.WriteLine($"Привет! Меня зовут {name}, группа {group}.");
Console.WriteLine($"Версия .NET: {Environment.Version}");
Console.WriteLine($"Система: {System.Runtime.InteropServices.RuntimeInformation.OSDescription}");