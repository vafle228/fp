using System.Drawing;
using FuncTools;

namespace TagCloud.ImageSaver;

public interface IImageSaver
{
    public Result<string> Save(Bitmap image);
}