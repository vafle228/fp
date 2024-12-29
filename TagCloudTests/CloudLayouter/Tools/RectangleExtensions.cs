using System.Drawing;

namespace TagCloudTests.CloudLayouter.Tools;

public static class RectangleExtensions
{
    public static double Area(this Rectangle rectangle)
    {
        return rectangle.Width * rectangle.Height;
    }
}