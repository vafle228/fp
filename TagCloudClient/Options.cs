using System.Drawing;
using System.Globalization;
using System.Text;
using CommandLine;
using TagCloud.CloudLayouter.Settings.Generators;

namespace TagCloudClient;

public class Options
{
    [Value(0, Required = true, HelpText = "Source file path")]
    public string FilePath { get; set; }

    [Option('e', "encoding",
        Required = false,
        HelpText = "Source encoding")]
    public string EncodingName { get; set; } = "utf-8";
    public Encoding UsingEncoding => Encoding.GetEncoding(EncodingName);
    
    [Option('s', "size", 
        Required = false, 
        HelpText = "Image size")]
    public Size Size { get; set; } = new(1920, 1080);

    [Option('f', "font", 
        Required = false, 
        HelpText = "Words font")]
    public FontFamily Font { get; set; } = new("Arial");

    [Option('b', "background-color",
        Required = false,
        HelpText = "Background color")]
    public Color BackgroundColor { get; set; } = Color.Black;

    [Option('c', "word-color", 
        Required = false, 
        HelpText = "Words color")]
    public Color ForegroundColor { get; set; } = Color.White;

    [Option("image-name", 
        Required = false, 
        HelpText = "Image name")]
    public string ImageName { get; set; } = "result";
    
    [Option("image-format", 
        Required = false, 
        HelpText = "Image format")]
    public string ImageFormat { get; set; } = "png";

    [Option("delta-angle", 
        Required = false, 
        HelpText = "Delta of the angle for the spiral.")]
    public double DeltaAngle { get; set; } = 0.5;

    [Option("step",
        Required = false,
        HelpText = "Step between the turns of the spiral")]
    public int Step { get; set; } = 1;

    [Option("center",
        Required = false,
        HelpText = "The center of the cloud in the image")]
    public Point Center { get; set; } = new(1920 / 2, 1080 / 2);

    [Option('g', "generator",
        Required = false,
        HelpText = "Point generator to use")]
    public PossibleGenerators UsingGenerator { get; set; } = PossibleGenerators.POLAR_SPIRAL;
    
    [Option("culture", 
        Required = false, 
        HelpText = "CSV culture information")]
    public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
}