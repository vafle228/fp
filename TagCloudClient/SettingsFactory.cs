using TagCloud.CloudLayouter.PointLayouter.PointGenerator;
using TagCloud.CloudLayouter.Settings;
using TagCloud.CloudLayouter.Settings.Generators;
using TagCloud.ImageGenerator;
using TagCloud.ImageSaver;
using TagCloud.WordsReader.Settings;

namespace TagCloudClient;

public static class SettingsFactory
{
    public static FileReaderSettings BuildFileReaderSettings(Options options)
        => new(options.FilePath, options.UsingEncoding);

    public static BitmapSettings BuildBitmapSettings(Options options)
        => new(options.Size, options.Font, options.BackgroundColor, options.ForegroundColor);

    public static PolarSpiralSettings BuildPolarSpiralSettings(Options options)
        => new(options.Step, options.DeltaAngle);

    public static SquareSpiralSettings BuildSquareSpiralSettings(Options options)
        => new(options.Step);

    public static PointLayouterSettings BuildPointLayouterSettings(Options options, IPointGenerator generator)
        => new(options.Center, generator);

    public static WordFileReaderSettings BuildWordReaderSettings(Options options)
        => new(options.FilePath);
    
    public static CsvFileReaderSettings BuildCsvReaderSettings(Options options) 
        => new(options.FilePath, options.Culture);

    public static FileSaveSettings BuildFileSaveSettings(Options options)
        => new(options.ImageName, options.ImageFormat);
}