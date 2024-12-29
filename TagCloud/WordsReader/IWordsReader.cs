using FuncTools;

namespace TagCloud.WordsReader;

public interface IWordsReader
{
    /*
     * Reads data from specific source
     */
    Result<List<string>> ReadWords();
}