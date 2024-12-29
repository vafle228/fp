using Autofac;
using CommandLine;
using TagCloud;
using TagCloud.CloudLayouter;
using TagCloud.CloudLayouter.PointLayouter;
using TagCloud.CloudLayouter.PointLayouter.PointGenerator;
using TagCloud.CloudLayouter.PointLayouter.PointGenerator.Generators;
using TagCloud.CloudLayouter.Settings.Generators;
using TagCloud.ImageGenerator;
using TagCloud.ImageSaver;
using TagCloud.WordsFilter;
using TagCloud.WordsFilter.Filters;
using TagCloud.WordsReader;
using TagCloud.WordsReader.Readers;

namespace TagCloudClient;

internal class Program
{
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(settings =>
            {
                var container = BuildContainer(settings);
                var generator = container.Resolve<CloudGenerator>();
                Console.WriteLine("File saved in " + generator.GenerateTagCloud());
            });
    }

    private static IContainer BuildContainer(Options settings)
    {
        var builder = new ContainerBuilder();

        RegisterSettings(builder, settings);
        RegisterLayouters(builder, settings);
        RegisterWordsReaders(builder, settings);
        RegisterWordsFilters(builder, settings);

        builder.RegisterType<CloudGenerator>().AsSelf();
        builder.RegisterType<BitmapGenerator>().AsSelf();
        builder.RegisterType<BitmapFileSaver>().As<IImageSaver>();

        return builder.Build();
    }

    private static void RegisterSettings(ContainerBuilder builder, Options settings)
    {
        builder.RegisterInstance(SettingsFactory.BuildBitmapSettings(settings)).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildFileSaveSettings(settings)).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildCsvReaderSettings(settings)).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildWordReaderSettings(settings)).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildFileReaderSettings(settings)).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildPolarSpiralSettings(settings)).AsSelf();
        builder.Register(context => SettingsFactory.BuildPointLayouterSettings(
            settings, context.Resolve<IPointGenerator>())).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildSquareSpiralSettings(settings)).AsSelf();
    }

    private static void RegisterWordsReaders(ContainerBuilder builder, Options settings)
    {
        builder
            .RegisterType<FileReader>().As<IWordsReader>()
            .OnlyIf(_ => Path.GetExtension(settings.FilePath) == ".txt");

        builder
            .RegisterType<CsvFileReader>().As<IWordsReader>()
            .OnlyIf(_ => Path.GetExtension(settings.FilePath) == ".csv");
        
        builder
            .RegisterType<WordFileReader>().As<IWordsReader>()
            .OnlyIf(_ => Path.GetExtension(settings.FilePath) == ".docx");
    }

    private static void RegisterWordsFilters(ContainerBuilder builder, Options settings)
    {
        builder.RegisterType<LowercaseFilter>().As<IWordsFilter>();
        builder.RegisterType<BoringWordsFilter>().As<IWordsFilter>();
    }

    private static void RegisterLayouters(ContainerBuilder builder, Options settings)
    {
        builder
            .RegisterType<PolarArchimedesSpiral>().As<IPointGenerator>()
            .OnlyIf(_ => settings.UsingGenerator == PossibleGenerators.POLAR_SPIRAL);
        
        builder
            .RegisterType<SquareArchimedesSpiral>().As<IPointGenerator>()
            .OnlyIf(_ => settings.UsingGenerator == PossibleGenerators.SQUARE_SPIRAL);
        builder.RegisterType<PointCloudLayouter>().As<ICloudLayouter>();
    }
}