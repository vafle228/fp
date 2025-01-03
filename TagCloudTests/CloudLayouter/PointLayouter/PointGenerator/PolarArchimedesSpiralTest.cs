﻿using System.Drawing;
using FluentAssertions;
using TagCloud.CloudLayouter.PointLayouter.PointGenerator.Generators;

namespace TagCloudTests.CloudLayouter.PointLayouter.PointGenerator;

[TestFixture]
public class PolarArchimedesSpiralTest
{
    private const string NOT_POSITIVE_ARGUMENT_ERROR = "Spiral params should be positive";
    
    [TestCase(2, 3)]
    [TestCase(10.1, 15.6)]
    [TestCase(200.1111111, 23)]
    public void PolarArchimedesSpiral_InitAtGivenPointAngleAndRadius(double radius, double angleOffset)
    {
        var polarSpiral = new PolarArchimedesSpiral(radius, angleOffset);

        polarSpiral.Radius.Should().Be(radius);
        polarSpiral.AngleOffset.Should().Be(angleOffset * Math.PI / 180);
    }

    [TestCase(0.0000, 11, Description = "Zero radius error")]
    [TestCase(-10, -100, Description = "Negative error with respect to radius")]
    [TestCase(52, double.MinValue, Description = "Negative angle offset error")]
    public void PolarArchimedesSpiral_ThrowError_OnNotPositiveNumber(double radius, double angleOffset)
    {
        var negativeParameter = radius <= 0 ? nameof(radius) : nameof(angleOffset);
        var pointGenerator = new PolarArchimedesSpiral(radius, angleOffset).StartFrom(Point.Empty);
        
        pointGenerator.Error.Should()
            .BeEquivalentTo($"{NOT_POSITIVE_ARGUMENT_ERROR}: {negativeParameter}");
        pointGenerator.IsSuccess.Should().BeFalse();
    }
    
        
    [Test]
    public void PolarArchimedesSpiral_CalculateSpecialPoints()
    {
        var polarSpiral = new PolarArchimedesSpiral(2, 360);
        var expected = new[]
        {
            new Point(0, 0), new Point(2, 0), new Point(4, 0), 
            new Point(6, 0), new Point(8, 0), new Point(10, 0)
        };
        
        var pointGenerator = polarSpiral.StartFrom(new Point(0, 0)).GetValueOrThrow();
        var expectedAndReceived = expected.Zip(pointGenerator);
        
        foreach (var (expectedPoint, receivedPoint) in expectedAndReceived)
        {
            expectedPoint.Should().BeEquivalentTo(receivedPoint);
        }
    }

    [TestCase(1)]
    [TestCase(10)]
    [TestCase(100)]
    public void PolarArchimedesSpiral_GenerateCircleLikeShape(int radius)
    {
        var polarSpiral = new PolarArchimedesSpiral(radius, 5);
        var pointGenerator = polarSpiral.StartFrom(new Point(0, 0)).GetValueOrThrow();

        for (var k = 1; k <= 10; k++)
        {
            var radiusSquare = k * radius * k * radius;
            pointGenerator
                .Skip((k - 1) * 10)
                .Take(10)
                .All(p => p.X * p.X + p.Y * p.Y < radiusSquare)
                .Should().BeTrue("Circle like shape should be generated");
        }
    }
}