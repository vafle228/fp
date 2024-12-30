using System.Drawing;
using FluentAssertions;
using TagCloud.CloudLayouter;
using TagCloud.CloudLayouter.PointLayouter;
using TagCloud.CloudLayouter.PointLayouter.PointGenerator;
using TagCloud.CloudLayouter.PointLayouter.PointGenerator.Generators;
using TagCloudTests.CloudLayouter.Tools;

namespace TagCloudTests.CloudLayouter.PointLayouter;

[TestFixture]
public class PointCloudLayouterTest
{
    private List<Rectangle> rectangles = [];
    
    private static IEnumerable<TestCaseData> InitCenterAtGivenPointTestCases
    {
        get
        {
            yield return new TestCaseData(new Point(0, 0));
            yield return new TestCaseData(new Point(-10, 10));
            yield return new TestCaseData(new Point(2000, 10000));
        }
    }
    private static IEnumerable<TestCaseData> DifferentPointGeneratorTestCase
    {
        get
        {
            yield return new TestCaseData(new SquareArchimedesSpiral(5));
            yield return new TestCaseData(new PolarArchimedesSpiral(10, 10));
        }
    }
    private static IEnumerable<TestCaseData> LayingDensityTestCase
    {
        get
        {
            yield return new TestCaseData(new SquareArchimedesSpiral(5), 0.42);
            yield return new TestCaseData(new PolarArchimedesSpiral(2, 1), 0.45);
        }
    }
    
    [TestCaseSource(nameof(InitCenterAtGivenPointTestCases))]
    public void SpiralCloudLayouter_InitCenterAtGivenPoint(Point center)
    {
        var pointGenerator = new SquareArchimedesSpiral(1);
        var layouter = new PointCloudLayouter(center, pointGenerator);
        
        layouter.Center.Should().BeEquivalentTo(center);
    }
    
    [Test]
    public void SpiralCloudLayouter_PutNextRectangle_ShouldPutFirstRectAtCenter()
    {
        var squareSize = new Size(100, 100);
        var pointGenerator = new SquareArchimedesSpiral(1);
        var layouter = new PointCloudLayouter(new Point(0, 0), pointGenerator);
        
        var rect = layouter.PutNextRectangle(squareSize).GetValueOrThrow();
        rectangles = [rect];
        
        rect.Should().BeOfType<Rectangle>();
        rect.Size.Should().BeEquivalentTo(squareSize);
        rect.Location.Should().BeEquivalentTo(new Point(-50, -50));
    }

    [TestCaseSource(nameof(DifferentPointGeneratorTestCase))]
    [Repeat(10)]
    public void SpiralCloudLayouter_PutNextRectangle_AllRectsShouldNotIntersect(IPointGenerator pointGenerator)
    {
        var layouter = new PointCloudLayouter(new Point(0, 0), pointGenerator);
        rectangles = PlaceRectangles(10, layouter);

        var intersectedRect = () => rectangles
            .First(r1 => rectangles
                .Except(Enumerable.Repeat(r1, 1))
                .Any(r1.IntersectsWith)
            );
        
        intersectedRect.Should().Throw<InvalidOperationException>("No intersection found");
    }

    [TestCaseSource(nameof(LayingDensityTestCase))]
    [Repeat(10)]
    public void SpiralCloudLayouter_PutNextRectangle_SatisfyDensityMinimum(IPointGenerator pointGenerator, double min)
    {
        var layouter = new PointCloudLayouter(new Point(0, 0), pointGenerator);
        rectangles = PlaceRectangles(100, layouter);

        var claimedArea = FindClaimedRectangle(rectangles).Area();
        var totalArea = rectangles.Select(r => r.Area()).Sum();

        (totalArea / claimedArea).Should().BeApproximately(1, min);
    }

    private static List<Rectangle> PlaceRectangles(int count, ICloudLayouter layouter)
    {
        var rectangleSizes = Enumerable.Range(0, count)
            .Select(_ => GetRandomSize(10, 25))
            .Select(s => layouter.PutNextRectangle(s).GetValueOrThrow());
        return rectangleSizes.ToList();
    }

    private static Size GetRandomSize(int min, int max)
    {
        var random = new Random();
        return new Size(random.Next(min, max), random.Next(min, max));
    }

    private static Rectangle FindClaimedRectangle(List<Rectangle> rects)
    {
        var leftX = rects.Min(r => r.Left);
        var leftY = rects.Min(r => r.Top);
        
        var width = rects.Max(r => r.Right) - leftX;
        var height = rects.Max(r => r.Bottom) - leftY;
        
        return new Rectangle(leftX, leftY, width, height);
    }
}