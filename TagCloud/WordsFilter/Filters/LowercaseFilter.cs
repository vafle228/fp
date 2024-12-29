using FuncTools;

namespace TagCloud.WordsFilter.Filters;

public class LowercaseFilter : IWordsFilter
{
    public Result<List<string>> ApplyFilter(List<string> words) 
        => words.Select(w => w.ToLower()).ToList();
}