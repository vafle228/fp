using TagCloud.ImageGenerator;
using TagCloud.ImageSaver;
using TagCloud.WordsFilter;
using TagCloud.WordsReader;

namespace TagCloud;

public class CloudGenerator(
    IImageSaver saver,
    IWordsReader reader, 
    BitmapGenerator imageGenerator,
    IEnumerable<IWordsFilter> filters)
{
    private const int MIN_FONT_SIZE = 10;
    private const int MAX_FONT_SIZE = 80;

    public string GenerateTagCloud()
    {
        var words = reader.ReadWords();

        var freqDict = filters
            .Aggregate(words, (c, f) => f.ApplyFilter(c))
            .GroupBy(w => w)
            .OrderByDescending(g => g.Count())
            .ToDictionary(g => g.Key, g => g.Count());
        
        var maxFreq = freqDict.Values.Max();
        var tagsList = freqDict.Select(pair => ToWordTag(pair, maxFreq)).ToList();
        
        return saver.Save(imageGenerator.GenerateWindowsBitmap(tagsList));
    }

    private static int TransformFreqToSize(int freq, int maxFreq) 
        => (int)(MIN_FONT_SIZE + (float)freq / maxFreq * (MAX_FONT_SIZE - MIN_FONT_SIZE));

    private static WordTag ToWordTag(KeyValuePair<string, int> pair, int maxFreq)
        => new(pair.Key, TransformFreqToSize(pair.Value, maxFreq));
}