using System.Drawing;
using FuncTools;
using TagCloud.CloudLayouter.Settings.Generators;

namespace TagCloud.CloudLayouter.PointLayouter.PointGenerator.Generators;

public class SquareArchimedesSpiral(int step) : IPointGenerator
{
    public int Step { get; } = step;

    public SquareArchimedesSpiral(SquareSpiralSettings settings)
        : this(settings.Step)
    { }

    public Result<IEnumerable<Point>> StartFrom(Point startPoint)
        => Step > 0 
            ? PointGenerator(startPoint).AsResult()
            : Result.Fail<IEnumerable<Point>>("Step should be positive number");

    private IEnumerable<Point> PointGenerator(Point startPoint)
    {
        var neededPoints = 1;
        var pointsToPlace = 1;
        var direction = Direction.Up;
        var currentPoint = startPoint;

        while (true)
        {
            yield return currentPoint;
            
            pointsToPlace--;
            currentPoint += GetOffsetSize(direction);
            
            if (pointsToPlace == 0)
            {
                direction = direction.AntiClockwiseRotate();
                if (direction is Direction.Up or Direction.Down) neededPoints++;
                pointsToPlace = neededPoints;
            }
        }
        // ReSharper disable once IteratorNeverReturns
    }
    
    private Size GetOffsetSize(Direction direction) => direction switch
    {
        Direction.Up => new Size(0, Step),
        Direction.Right => new Size(Step, 0),
        Direction.Down => new Size(0, -Step),
        Direction.Left => new Size(-Step, 0),
        _ => throw new ArgumentOutOfRangeException(nameof(direction))
    };
}