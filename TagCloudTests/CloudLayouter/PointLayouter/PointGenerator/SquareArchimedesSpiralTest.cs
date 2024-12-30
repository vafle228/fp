using System.Drawing;
using FluentAssertions;
using TagCloud.CloudLayouter.PointLayouter.PointGenerator.Generators;

namespace TagCloudTests.CloudLayouter.PointLayouter.PointGenerator;

[TestFixture]
public class SquareArchimedesSpiralTest
{
    private const string NOT_POSITIVE_STEP_ERROR = "Step should be positive number";
    
    [TestCase(2)]
    [TestCase(10)]
    [TestCase(200)]
    public void SquareArchimedesSpiral_InitAtGivenPointAndStep(int step)
    {
        var squareSpiral = new SquareArchimedesSpiral(step);
        squareSpiral.Step.Should().Be(step);
    }
    
    [TestCase(0, Description = "Zero step error")]
    [TestCase(-10, Description = "Negative step error")]
    [TestCase(int.MinValue, Description = "Smallest step still error")]
    public void SquareArchimedesSpiral_ThrowError_OnNotPositiveNumber(int step)
    {
        var pointGenerator = new SquareArchimedesSpiral(step).StartFrom(Point.Empty);
        pointGenerator.Error.Should().BeEquivalentTo(NOT_POSITIVE_STEP_ERROR);
        pointGenerator.IsSuccess.Should().BeFalse();
    }
    
    [Test]
    public void SquareArchimedesSpiral_CalculateSpecialPoints()
    {
        var squareSpiral = new SquareArchimedesSpiral(5);
        var expected = new[]
        {
            new Point(0, 0), new Point(0, 5), new Point(-5, 5), 
            new Point(-5, 0), new Point(-5, -5), new Point(0, -5)
        };
        
        var pointGenerator = squareSpiral.StartFrom(new Point(0, 0)).GetValueOrThrow();
        var expectedAndReceived = expected.Zip(pointGenerator);
        
        foreach (var (expectedPoint, receivedPoint) in expectedAndReceived)
        {
            expectedPoint.Should().BeEquivalentTo(receivedPoint);
        }
    }
    
    [TestCase(1)]
    [TestCase(10)]
    [TestCase(100)]
    public void SquareArchimedesSpiral_GenerateRectLikeShape(int step)
    {
        var squareSpiral = new SquareArchimedesSpiral(step);
        var pointGenerator = squareSpiral.StartFrom(new Point(0, 0)).GetValueOrThrow();

        for (var k = 1; k <= 50; k++)
        {
            var side = k / 2 * step + step;
            pointGenerator
                .Skip((k - 1) * 5)
                .Take(5)
                .All(p => (-side <= p.X && p.X <= side) && (-side <= p.Y && p.Y <= side))
                .Should().BeTrue("Square like shape should be generated");
        }
    }
}