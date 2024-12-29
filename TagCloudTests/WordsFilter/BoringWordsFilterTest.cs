using FluentAssertions;
using TagCloud.WordsFilter.Filters;

namespace TagCloudTests.WordsFilter;

[TestFixture]
public class BoringWordsFilterTest
{
    private readonly BoringWordsFilter filter = new();

    [Test]
    public void BoringWordsFilter_ApplyFilter_ShouldRemovePrimitiveWords()
    {
        List<string> words = ["a", "the", "hello"];
        var filtered = filter.ApplyFilter(words);
        filtered.Should().BeEquivalentTo(["hello"], options => options.WithStrictOrdering());
    }
}