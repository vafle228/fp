using System.Globalization;
using System.Text;
using FluentAssertions;
using TagCloud.WordsReader;
using TagCloud.WordsReader.Readers;
using TagCloudTests.WordsReader.Tools;

namespace TagCloudTests.WordsReader;

[TestFixture]
public class WordsReadersTest
{
    private const string FILE_CONTENT = 
        "Hello world! Hello world! I'm wanna say hello to you batya!";
    
    private static IEnumerable<TestCaseData> WordsReadersTestCases
    {
        get
        {
            yield return new TestCaseData(new WordFileReader("Samples/text.docx"));
            yield return new TestCaseData(new FileReader("Samples/text.txt", Encoding.UTF8));
            yield return new TestCaseData(new CsvFileReader("Samples/text.csv", CultureInfo.InvariantCulture));
        }
    }
    
    [TestCaseSource(nameof(WordsReadersTestCases))]
    public void WordsReaders_ReadWords_ShouldReadAllWords(IWordsReader reader)
    {
        var words = reader.ReadWords();
        words.ToText(" ").Should().Be(FILE_CONTENT);
    }
}