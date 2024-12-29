using System.Drawing;
using FuncTools;
using TagCloud.CloudLayouter.PointLayouter.PointGenerator;
using TagCloud.CloudLayouter.Settings;

namespace TagCloud.CloudLayouter.PointLayouter;

public class PointCloudLayouter(Point center, IPointGenerator pointGenerator) : ICloudLayouter
{
    private readonly List<Point> placedPoints = [];
    private readonly List<Rectangle> placedRectangles = [];
    
    public PointCloudLayouter(PointLayouterSettings settings)
        : this(settings.Center, settings.Generator) 
    { }

    public Point Center { get; } = center;

    public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        => TryPutNext(rectangleSize) is var rect && rect != Rectangle.Empty 
            ? RememberRectangle(rect).AsResult()
            : Result.Fail<Rectangle>("There are no more points in generator");
    
    private Rectangle RememberRectangle(Rectangle rect)
    {
        placedRectangles.Add(rect);
        placedPoints.Add(rect.Location - rect.Size / 2);
        return rect;
    }
    
    private Rectangle TryPutNext(Size rectangleSize) 
        => pointGenerator.StartFrom(Center)
            .Except(placedPoints)
            .Select(p => CreateRectangle(p, rectangleSize))
            .FirstOrDefault(r => !placedRectangles.Any(r.IntersectsWith), Rectangle.Empty);

    private static Rectangle CreateRectangle(Point center, Size rectangleSize)
    {
        var rectangleUpperLeft = center - rectangleSize / 2;
        return new Rectangle(rectangleUpperLeft, rectangleSize);
    }
}