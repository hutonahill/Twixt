using System.DirectoryServices.ActiveDirectory;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TwixtCode;

namespace Twixt;

public class Point {
    public Position Location { get; private set; }

    private const double DotSize = 5;
    private const double SelectedModifier = 1.5;

    private Ellipse? element;

    private Ellipse? effect;

    public Point(Position location) {
        Location = location;
    }

    public Point(double x, double y) {
        Location = new Position(x, y);
    }

    public double getSize() {
        return DotSize;
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
}
    
    