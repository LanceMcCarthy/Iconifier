using Iconifier.Models;
using SkiaSharp;
using System.Text.Json;

Console.WriteLine("*************************************************************");
Console.WriteLine("*********** Hello, welcome to the icon generator! ***********");
Console.WriteLine("*************************************************************");

while (true)
{
    // ***** Phase 1 - Get user input ***** //

    // Get the path to the source image file
    Console.WriteLine("Enter the path to your source image file (default is test file):");
    var imageFilePath = Console.ReadLine();
    if (string.IsNullOrEmpty(imageFilePath)) imageFilePath = Path.Join(Directory.GetCurrentDirectory(), "original.png");

    // Get the path to the icon json config file
    Console.WriteLine("Enter the path to your icon json config file (default is starter config):");
    var jsonPath = Console.ReadLine();
    if (string.IsNullOrEmpty(jsonPath)) jsonPath = Path.Join(Directory.GetCurrentDirectory(), "starter-config.json");

    // Get the destination directory to save the icons to
    Console.WriteLine("Enter the destination directory to save the icons to (default is current directory):");
    var destDirectory = Console.ReadLine();
    if (string.IsNullOrEmpty(destDirectory)) destDirectory = Directory.GetCurrentDirectory();


    // ***** Phase 2 - Deserialize config ***** //

    var jsonText = File.ReadAllText(jsonPath);
    var configRoot = JsonSerializer.Deserialize<ConfigRoot>(jsonText);

    if (configRoot == null)
    {
        Console.WriteLine("Exiting! Deserialization Error. the json is likely not formatted correctly, please see this example for reference: https://gist.github.com/LanceMcCarthy/63815c41569ad877f24121be679d3638");
        return;
    }


    // ***** Phase 3 - Load the original image ***** //

    using var imgStream = File.OpenRead(imageFilePath);
    using var codec = SKCodec.Create(imgStream);
    using var originalBitmap = SKBitmap.Decode(codec);


    // ***** Phase 4 - Resize original and save a copy ***** //

    if (configRoot.IconDefinitions == null)
    {
        Console.WriteLine("You have not defined any icon definitions, please see this example for reference: https://gist.github.com/LanceMcCarthy/63815c41569ad877f24121be679d3638");
        return;
    }

    foreach (var iconDef in configRoot.IconDefinitions)
    {
        using var resized = originalBitmap.Resize(new SKImageInfo(iconDef.Height, iconDef.Width), SKFilterQuality.None);
        using var image = SKImage.FromBitmap(resized);
        using var output = new MemoryStream();

        image.Encode(SKEncodedImageFormat.Png, 0).SaveTo(output);

        File.WriteAllBytes(Path.Combine(destDirectory, $"{iconDef.Width}x{iconDef.Height}.png"), output.ToArray());

        Console.WriteLine($"Generated {iconDef.Width}x{iconDef.Height}.png!");
    }

    Console.WriteLine("Done! Do you want to do another one? [y/N]:");

    if (Console.ReadLine()?.ToLower() != "y") break;
}
