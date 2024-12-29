using System.Drawing;
using TagCloud.CloudLayouter.PointLayouter.PointGenerator;

namespace TagCloud.CloudLayouter.Settings;

public record PointLayouterSettings(Point Center, IPointGenerator Generator);