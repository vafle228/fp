using FluentAssertions;
using TagCloud.WordsFilter.Filters;

namespace TagCloudTests.WordsFilter;

[TestFixture]
public class LowercaseFilterTest
{
    private readonly LowercaseFilter filter = new();
    
    [Test]
    public void LowercaseFilter_ApplyFilter_ShouldLowerAllWords()
    {
        List<string> words = ["Hello", "WORLD"];
        var filtered = filter.ApplyFilter(words);
        filtered.Should().BeEquivalentTo(["hello", "world"], options => options.WithStrictOrdering());
    }
}