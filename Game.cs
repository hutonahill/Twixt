
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace Twixt;

public class Game {
    private List<Point> blackPlays = new List<Point>();
    
    private List<Point> redPlays = new List<Point>();
    
    [JsonIgnore]
    private List<Line> blackLines = new List<Line>();
    
    [JsonIgnore]
    private List<Line> redLines = new List<Line>();

    // red starts first
    private bool redsTurn = true;
    
    
}