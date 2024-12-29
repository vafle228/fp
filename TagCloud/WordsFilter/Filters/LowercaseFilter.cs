namespace TagCloud.WordsFilter.Filters;

public class LowercaseFilter : IWordsFilter
{
    public List<string> ApplyFilter(List<string> words) 
        => words.Select(w => w.ToLower()).ToList();
}