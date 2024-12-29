namespace TagCloud.WordsReader;

public interface IWordsReader
{
    /*
     * Reads data from specific source
     */
    List<string> ReadWords();
}