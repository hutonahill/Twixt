
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;
using TwixtCode;

namespace Twixt;

public class Point {
    private Position Location;
    
    [JsonIgnore]
    private const double DotSize = 5;
    [JsonIgnore]
    private const double SelectedModifier = 1.5;
    
    [JsonIgnore]
    private Ellipse? element;
    
    [JsonIgnore]
    private Ellipse? effect;

    public Point(Position location) {
        Location = location;
    }

    public Point(double x, double y, uint row, uint column) {
        Location = new Position(x, y, row, column);
    }

    public double getSize() {
        return DotSize;
    }

    public uint getRow() {
        if (Location.Row == null) {
            throw new UnreachableException("Point always defines row");
        }
        return (uint)Location.Row;
    }
    
    public uint getColumn() {
        if (Location.Column == null) {
            throw new UnreachableException("Point always defines column");
        }
        return (uint)Location.Column;
    }

    public Ellipse getElement() {
        if (element == null) {
            element = new Ellipse {
                Width = DotSize,
                Height = DotSize,
                Fill = Brushes.Blue
            };
        
            Canvas.SetLeft(element, Location.X - DotSize / 2);
            Canvas.SetTop(element, Location.Y - DotSize / 2);
        }
        
        return element;
    }

    public Ellipse getEffect() {
        if (effect == null) {
            double actualSize = DotSize * SelectedModifier;
            effect = new Ellipse {
                Width = actualSize,  
                Height = actualSize, 
                Fill = Brushes.Green
            };
        
            Canvas.SetLeft(effect, Location.X - actualSize / 2);
            Canvas.SetTop(effect, Location.Y - actualSize / 2);
        }
        
        return effect;
    }

    public double DistanceTo(Position position) {
        return Location.DistanceTo(position);
    }

    public double DistanceTo(Point point) {
        return Location.DistanceTo(point.Location);
    }
}
    
    