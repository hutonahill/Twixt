
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        if (location.Row == null) {
            throw new ArgumentNullException($"{nameof(location)}.{nameof(Position.Row)}", 
                "Points need to have a row.");
        }
        
        if (location.Column == null) {
            throw new ArgumentNullException($"{nameof(location)}.{nameof(Position.Column)}", 
                "Points need to have a column.");
        }
        
        Location = location;
    }

    public Point(double x, double y, uint row, uint column) {
        Location = new Position(x, y, row, column);
    }

    public static Point GetPointFromBoard(JObject data, MainWindow board) {
        if (!data.TryGetValue(nameof(Position.Row), out JToken? rowToken)) {
            throw new ArgumentException($"Expected JObject to contain key {nameof(Position.Row)}.", nameof(data));
        }
        
        if (rowToken.Type != JTokenType.Integer) {
            throw new ArgumentException($"{nameof(data)}[{nameof(Position.Row)}] should be an JTokenType.Integer, " +
                                        $"but is a JTokenType.{rowToken.Type}", nameof(data));
        }

        int rawRow = rowToken.Value<int>();

        if (rawRow < 0) {
            throw new ArgumentException($"expected a positive integer at {nameof(data)}[{nameof(Position.Row)}]," + 
                                        $"but got a negative value", $"{nameof(data)}[{nameof(Position.Row)}]");
        }

        uint row = (uint)rawRow;
        
        
        if (!data.TryGetValue(nameof(Position.Column), out JToken? columnToken)) {
            throw new ArgumentException($"Expected JObject to contain key {nameof(Position.Column)}.", nameof(data));
        }

        if (columnToken.Type != JTokenType.Integer) {
            throw new ArgumentException($"{nameof(data)}[{nameof(Position.Column)}] should be a JTokenType.Integer, " +
                                        $"but is a JTokenType.{columnToken.Type}", nameof(data));
        }

        int rawColumn = columnToken.Value<int>();

        if (rawColumn < 0) {
            throw new ArgumentException($"Expected a positive integer at {nameof(data)}[{nameof(Position.Column)}], " +
                                        $"but got a negative value.", $"{nameof(data)}[{nameof(Position.Column)}]");
        }

        uint column = (uint)rawColumn;

        return board.getPoint(row, column);
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

    public JObject ToJson() {
        return new JObject {
            { nameof(Position.Row), Location.Row },
            { nameof(Position.Column), Location.Column }
        };
    }
}
    
    