using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using TwixtCode;

namespace Twixt;

public partial class MainWindow : Window
{
    // Define the list of points
        private List<Point> points = new List<Point>();

        public static readonly double CellSize = 30;
        public static readonly uint NumRows  = 24;
        public static readonly uint NumCols = 24;
        
        private DispatcherTimer timer;

        private List<UIElement> Effects = new List<UIElement>();

        private Point? SelectedPoint;

        public event EventHandler<Point>? PointClicked;

        public void AddEffect(UIElement effect) {
            Effects.Add(effect);
            Board.Children.Add(effect);
        }
        
        private List<UIElement> Pieces = new List<UIElement>();

        public MainWindow() {
            InitializeComponent();  // Initializes components from XAML
            
            timer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(33) // 30 FPS = 33ms per frame
            };
            timer.Tick += Timer_Tick; // Event handler for each timer tick
            timer.Start();
            
            DrawGrid();
        }

        public Point getPoint(uint row, uint column) {
            
            Point? output = points.FirstOrDefault(p => p.getRow() == row && p.getColumn() == column);

            if (output == null) {
                throw new ArgumentException($"No point found at the specified location: Row {row}, Column {column}.");
            }

            return output;
        }
        
        private void Timer_Tick(object? sender, EventArgs e)
        {
            // This will run every 33 milliseconds (about 30 FPS)

            // Update the UI (e.g., render points, update logic)
            Render();
        }

        private void DrawGrid() {
            Board.Width = NumCols * CellSize;
            Board.Height = NumRows * CellSize;
            
            Rectangle background = new Rectangle {
                Width = Board.Width,
                Height = Board.Height,
                Fill = Brushes.White
            };
            Board.Children.Add(background);

            // Draw dots and create the points list
            points.Clear();  // Clear any existing points before rendering
            double yPos = CellSize/2;  // Initialize yPos for the first row

            for (uint y = 0; y < NumRows; y++) {
                double xPos = CellSize/2;  // Initialize xPos for each row

                for (uint x = 0; x < NumCols; x++) {
                    // Create a new Point at the current position
                    Point point = new Point(xPos, yPos, x, y);
                    points.Add(point);

                    // Draw the dot at the position
                    Board.Children.Add(point.getElement());
                    

                    // Advance xPos by the cell size for the next point
                    xPos += CellSize;
                }

                // After completing one row, advance yPos for the next row
                yPos += CellSize;
            }

            // Draw horizontal and vertical lines at the top, bottom, left, and right
            Position topStart = new Position(0, CellSize);
            Position topEnd = new Position(NumCols * CellSize, CellSize);
            DrawLine(topStart, topEnd, Brushes.Red, 1);

            Position bottomStart = new Position(0, (NumRows - 1) * CellSize);
            Position bottomEnd = new Position(NumCols * CellSize, (NumRows - 1) * CellSize);
            DrawLine(bottomStart, bottomEnd, Brushes.Red, 1);

            Position leftStart = new Position(CellSize, 0);
            Position leftEnd = new Position(CellSize, NumRows * CellSize);
            DrawLine(leftStart, leftEnd, Brushes.Black, 1);

            Position rightStart = new Position((NumCols-1) * CellSize, 0);
            Position rightEnd = new Position((NumCols-1) * CellSize, NumRows * CellSize);
            DrawLine(rightStart, rightEnd, Brushes.Black, 1);
        }

        private Line DrawLine(Position start, Position end, Brush color, double thickness) {
            Line line = new Line {
                X1 = start.X,
                Y1 = start.Y,
                X2 = end.X,
                Y2 = end.Y,
                Stroke = color,
                StrokeThickness = thickness
            };

            Board.Children.Add(line);

            return line;
        }
        
        private void Render() {
            // Clear any previous points from the canvas
            foreach (UIElement uiElement in Effects) {
                Board.Children.Remove(uiElement);
            }
            
            Effects.Clear();
        }

        private void Board_MouseLeave(object sender, MouseEventArgs e) {
            if (SelectedPoint != null) {
                Board.Children.Remove(SelectedPoint.getEffect());
                SelectedPoint = null;
            }
        }
        
        private void Board_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (SelectedPoint != null) {
                PointClicked?.Invoke(this, SelectedPoint);
            }
        }

        
        private void SelectPoint(Position position) {
            // Find the closest point within the grid

            Point? closestPoint = points.MinBy(p => p.DistanceTo(position));

            if (closestPoint != null && SelectedPoint != closestPoint) {
                if (SelectedPoint != null) {
                    Board.Children.Remove(SelectedPoint?.getEffect());
                }

                SelectedPoint = closestPoint;
                Board.Children.Add(closestPoint.getEffect());
            }
            else if (closestPoint == null) {
                SelectedPoint = null;
            }
        }
        
        private void Board_MouseMove (object sender, MouseEventArgs e) {
            // Get the mouse position relative to the Canvas
            System.Windows.Point mousePosition = e.GetPosition(Board);

            // Convert the mouse position to a Position object
            Position position = new Position(mousePosition.X, mousePosition.Y); //BREAKPOINT

            // Select the closest point to the mouse position
            SelectPoint(position);
        }

    }
