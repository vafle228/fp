using System.Drawing;
using FuncTools;
using TagCloud.CloudLayouter.Settings.Generators;

namespace TagCloud.CloudLayouter.PointLayouter.PointGenerator.Generators;

public class PolarArchimedesSpiral(double radius, double angleOffset) : IPointGenerator
{
    public double Radius { get; } = radius;
    public double AngleOffset { get; } = angleOffset * Math.PI / 180;

    public PolarArchimedesSpiral(PolarSpiralSettings settings) 
        : this(settings.Radius, settings.AngleOffset) 
    { }

    public Result<IEnumerable<Point>> StartFrom(Point startPoint)
    {
        if (Radius <= 0 || angleOffset <= 0)
        {
            var argName = Radius <= 0 ? nameof(radius) : nameof(angleOffset);
            return Result.Fail<IEnumerable<Point>>($"Spiral params should be positive: {argName}");
        }
        return PointGenerator(startPoint).AsResult();
    }

    private IEnumerable<Point> PointGenerator(Point startPoint)
    {
        var currentAngle = 0.0;
        while (true)
        {
            var polarCoordinate = Radius / (2 * Math.PI) * currentAngle;
            
            var xOffset = (int)Math.Round(polarCoordinate * Math.Cos(currentAngle));
            var yOffset = (int)Math.Round(polarCoordinate * Math.Sin(currentAngle));
            
            yield return startPoint + new Size(xOffset, yOffset);
            
            currentAngle += AngleOffset;
        }
        // ReSharper disable once IteratorNeverReturns
    }
}