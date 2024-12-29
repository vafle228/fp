using FuncTools;
using WeCantSpell.Hunspell;

namespace TagCloud.WordsFilter.Filters;

public class BoringWordsFilter : IWordsFilter
{
    private const string ENGLISH_DIC = "./Dictionaries/English (American).dic";
    private const string ENGLISH_AFF = "./Dictionaries/English (American).aff";

    public Result<List<string>> ApplyFilter(List<string> words)
        => File.Exists(ENGLISH_DIC) && File.Exists(ENGLISH_AFF) 
            ? WordList.CreateFromFiles(ENGLISH_DIC, ENGLISH_AFF)
                .AsResult()
                .Then(wl => words.Where(w => !IsBoring(w, wl)).ToList()) 
            : Result.Fail<List<string>>("Cannot find dictionaries");
    
    private static WordEntryDetail[] CheckDetails(string word, WordList wordList)
    {
        var details = wordList.CheckDetails(word);
        return wordList[string.IsNullOrEmpty(details.Root) ? word : details.Root];
    }

    private static bool IsBoring(string word, WordList wordList)
    {
        var details = CheckDetails(word, wordList);
        if (details.Length == 0 || details[0].Morphs.Count == 0) return false;
        
        var po = details[0].Morphs[0];
        return po is "po:pronoun" or "po:preposition" or "po:determiner" or "po:conjunction";
    }
}