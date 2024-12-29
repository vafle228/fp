using FuncTools;

namespace TagCloud.WordsFilter;

public interface IWordsFilter
{
    /*
     * Apply some function on words list
     */
    Result<List<string>> ApplyFilter(List<string> words);
}