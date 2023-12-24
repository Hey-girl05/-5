using System;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

public class Figure
{
    public string Name { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public static class FileHandler
{
    public static Figure Read(string filePath)
    {
        var extension = Path.GetExtension(filePath);
        switch (extension)
        {
            case ".txt":
                return ReadTxt(filePath);
            case ".json":
                return ReadJson(filePath);
            case ".xml":
                return ReadXml(filePath);
            default:
                throw new NotSupportedException("File format not supported.");
        }
    }

    public static void Write(string filePath, Figure figure)
    {
        var extension = Path.GetExtension(filePath);
        switch (extension)
        {
            case ".txt":
                WriteTxt(filePath, figure);
                break;
            case ".json":
                WriteJson(filePath, figure);
                break;
            case ".xml":
                WriteXml(filePath, figure);
                break;
            default:
                throw new NotSupportedException("File format not supported.");
        }
    }

    private static Figure ReadTxt(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        if (lines.Length != 3) throw new InvalidDataException("Invalid file format.");
        var figure = new Figure
        {
            Name = lines[0],
            Width = int.Parse(lines[1]),
            Height = int.Parse(lines[2])
        };
        return figure;
    }

    private static Figure ReadJson(string filePath)
    {
        var json = File.ReadAllText(filePath);
        var figure = JsonConvert.DeserializeObject<Figure>(json);
        return figure;
    }

    private static Figure ReadXml(string filePath)
    {
        var serializer = new XmlSerializer(typeof(Figure));
        using (var stream = File.OpenRead(filePath))
        {
            var figure = (Figure)serializer.Deserialize(stream);
            return figure;
        }
    }

    private static void WriteTxt(string filePath, Figure figure)
    {
        var lines = new string[] { figure.Name, figure.Width.ToString(), figure.Height.ToString() };
        File.WriteAllLines(filePath, lines);
    }

    private static void WriteJson(string filePath, Figure figure)
    {
        var json = JsonConvert.SerializeObject(figure, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    private static void WriteXml(string filePath, Figure figure)
    {
        var serializer = new XmlSerializer(typeof(Figure));
        using (var stream = File.Create(filePath))
        {
            serializer.Serialize(stream, figure);
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter file path:");
        var filePath = Console.ReadLine();

        var figure = FileHandler.Read(filePath);

        Console.WriteLine($"Name: {figure.Name}");
        Console.WriteLine($"Width: {figure.Width}");
        Console.WriteLine($"Height: {figure.Height}");

        while (true)
        {
            Console.WriteLine("Press F1 to save, or Escape to exit.");
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.F1)
            {
                FileHandler.Write(filePath, figure);
                Console.WriteLine("File saved.");
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                break;
            }
        }
    }
}

