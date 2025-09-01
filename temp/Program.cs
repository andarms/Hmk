using System;
using System.Reflection;
using DotTiled;

Console.WriteLine("DotTiled API Exploration");
Console.WriteLine("========================");

// Get all public types in DotTiled assembly
var assembly = typeof(DotTiled.Map).Assembly;
var types = assembly.GetExportedTypes();

foreach (var type in types.OrderBy(t => t.Name))
{
  Console.WriteLine($"Type: {type.FullName}");

  if (type.Name == "Map")
  {
    Console.WriteLine("  Map class methods:");
    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Where(m => !m.IsSpecialName);
    foreach (var method in methods)
    {
      Console.WriteLine($"    {method.Name}({string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"))})");
    }
  }

  if (type.Name == "Loader")
  {
    Console.WriteLine("  Loader class methods:");
    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
        .Where(m => !m.IsSpecialName);
    foreach (var method in methods)
    {
      Console.WriteLine($"    {method.Name}({string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"))})");
    }
  }
}
