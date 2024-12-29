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
    {
        Rectangle placedRect;
        try
        {
            placedRect = pointGenerator.StartFrom(Center)
                .Except(placedPoints)
                .Select(p => CreateRectangle(p, rectangleSize))
                .First(r => !placedRectangles.Any(r.IntersectsWith));
        }
        catch (InvalidOperationException)
        {
            throw new ArgumentException("There are no more points in generator");
        }

        placedRectangles.Add(placedRect);
        placedPoints.Add(placedRect.Location - placedRect.Size / 2);
        
        return placedRect;
    }

    private static Rectangle CreateRectangle(Point center, Size rectangleSize)
    {
        var rectangleUpperLeft = center - rectangleSize / 2;
        return new Rectangle(rectangleUpperLeft, rectangleSize);
    }
}