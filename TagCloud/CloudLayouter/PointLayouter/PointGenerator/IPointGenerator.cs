using System.Drawing;
using FuncTools;

namespace TagCloud.CloudLayouter.PointLayouter.PointGenerator;

public interface IPointGenerator
{
    /*
     * Infinite point generator
     */
    public Result<IEnumerable<Point>> StartFrom(Point startPoint);
}