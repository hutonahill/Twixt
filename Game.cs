
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    
    public Game() {
        
    }

    public Game(JObject data, MainWindow board) {
        
    }

    public JObject ToJson() {
        JObject gameData = new JObject {
            { nameof(redsTurn), redsTurn },
            { nameof(blackPlays), new JArray(blackPlays.Select(bp => bp.ToJson())) },
            { nameof(redPlays), new JArray(redPlays.Select(rp=> rp.ToJson())) }
        };

        return gameData;
    }

}