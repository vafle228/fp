using System.Drawing;
using FuncTools;

namespace TagCloud.CloudLayouter;

public interface ICloudLayouter
{
    public Result<Rectangle> PutNextRectangle(Size rectangleSize);
}