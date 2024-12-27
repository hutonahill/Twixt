using System.Text.Json.Serialization;
using Twixt;

namespace TwixtCode;

using System;

public class Position {
    [JsonIgnore]
    public double X { get; private set; }
    
    [JsonIgnore]
    public double Y { get; private set; }
    
    public uint? Column { get; private set; }
    public uint? Row { get; private set; }

    public Position(double x, double y) {
        X = x;
        Y = y;
    }

    public Position(double x, double y, uint row, uint column) {
        X = x;
        Y = y;

        if (row > MainWindow.NumRows) {
            throw new ArgumentOutOfRangeException(nameof(row), "Row position may not exceed number of rows");
        }

        if (column > MainWindow.NumCols) {
            throw new ArgumentOutOfRangeException(nameof(column),
                "Column Position may not exceed the number of columns");
        }
        
        Row = row;
        Column = column;
    }

    public double DistanceTo(Position other) {
        
        double dx = X - other.X;
        double dy = Y - other.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public override string ToString() {
        return $"({X}, {Y})";
    }
}

