using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;


namespace SynthesizerUI.Converters;

public class TickMarkConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        double height = (double)values[0];
        int tickCount = (int)values[1];
        double index = (double)values[2];

        double radius = height / 2;
        double angle = (index / (tickCount - 1)) * 360;
        double y = radius * (1 - Math.Cos(angle * Math.PI / 180));
        double startX = -radius;
        double startY = radius - y;
        double endX = radius;
        double endY = radius - y;

        var pathFigure = new PathFigure();
        pathFigure.StartPoint = new Point(startX, startY);
        pathFigure.Segments.Add(new PolyLineSegment(new Point[] { new Point(endX, endY) }, true));
        return new PathGeometry() { Figures = { pathFigure } };
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}