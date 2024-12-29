using FuncTools;
using TagCloud.ImageGenerator;
using TagCloud.ImageSaver;
using TagCloud.WordsFilter;
using TagCloud.WordsReader;

namespace TagCloud;

#pragma warning disable CA1416
public class CloudGenerator(
    IImageSaver saver,
    IWordsReader reader, 
    BitmapGenerator imageGenerator,
    IEnumerable<IWordsFilter> filters)
{
    private const int MIN_FONT_SIZE = 10;
    private const int MAX_FONT_SIZE = 80;

    public Result<string> GenerateTagCloud() 
        => reader
            .ReadWords()
            .Then(BuildFreqDict)
            .Then(ToWordTagList)
            .Then(imageGenerator.GenerateWindowsBitmap)
            .Then(saver.Save);
    
    private static Result<List<WordTag>> ToWordTagList(Dictionary<string, int> freqDict)
        => freqDict.Values.Max().AsResult().Then(
            m => freqDict.Select(p => ToWordTag(p, m)).ToList());
    
    private Result<Dictionary<string, int>> BuildFreqDict(List<string> words) 
        => ApplyFilters(words).Then(wl => wl
            .GroupBy(w => w)
            .OrderByDescending(g => g.Count())
            .ToDictionary(g => g.Key, g => g.Count()));

    private Result<List<string>> ApplyFilters(List<string> words)
        => filters.Aggregate(words.AsResult(), (c, f) => c.Then(f.ApplyFilter));

    private static int TransformFreqToSize(int freq, int maxFreq) 
        => (int)(MIN_FONT_SIZE + (float)freq / maxFreq * (MAX_FONT_SIZE - MIN_FONT_SIZE));

    private static WordTag ToWordTag(KeyValuePair<string, int> pair, int maxFreq)
        => new(pair.Key, TransformFreqToSize(pair.Value, maxFreq));
}
#pragma warning restore CA1416