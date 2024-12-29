using System.Drawing;

namespace TagCloud.CloudLayouter.PointLayouter.PointGenerator;

public interface IPointGenerator
{
    /*
     * Infinite point generator
     */
    public IEnumerable<Point> StartFrom(Point startPoint);
}