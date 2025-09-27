using PopupLib;

namespace TetriSH;

public class TetrisEngine {
    public static Tuple<int, int>[,,] tetrominoes = new Tuple<int, int>[,,] {
        { // straight
            { new(-1, 0), new(0, 0), new(1, 0), new(2, 0) },
            { new(0, -1), new(0, 0), new(0, 1), new(0, 2) },
            { new(1, 0), new(0, 0), new(-1, 0), new(-2, 0) },
            { new(0, 1), new(0, 0), new(0, -1), new(0, -2) }
        },
        { // square
            { new(0, 0), new(1, 0), new(0, 1), new(1, 1) },
            { new(0, 0), new(1, 0), new(0, 1), new(1, 1) },
            { new(0, 0), new(1, 0), new(0, 1), new(1, 1) },
            { new(0, 0), new(1, 0), new(0, 1), new(1, 1) }
        },
        { // T
            { new(-1, 0), new(0, 0), new(1, 0), new(0, 1) },
            { new(-1, 0), new(0, -1), new(0, 0), new(0, 1) },
            { new(-1, 0), new(0, 0), new(1, 0), new(0, -1) },
            { new(1, 0), new(0, -1), new(0, 0), new(0, 1) }
        },
        { // L
            { new(0, 0), new(0, -1), new(0, 1), new(1, 1) },
            { new(0, 0), new(-1, 0), new(1, 0), new(-1, 1) },
            { new(0, 0), new(0, -1), new(0, 1), new(-1, -1) },
            { new(0, 0), new(-1, 0), new(1, 0), new(1, -1) }
        },
        { // J
            { new(0, 0), new(0, -1), new(0, 1), new(-1, 1) },
            { new(0, 0), new(-1, 0), new(1, 0), new(-1, -1) },
            { new(0, 0), new(0, -1), new(0, 1), new(1, -1) },
            { new(0, 0), new(-1, 0), new(1, 0), new(1, 1) }
        },
        { // S
            { new(0, 0), new(1, 0), new(0, -1), new(-1, -1) },
            { new(0, 0), new(-1, 0), new(-1, 1), new(0, -1) },
            { new(0, 0), new(1, 0), new(0, -1), new(-1, -1) },
            { new(0, 0), new(-1, 0), new(-1, 1), new(0, -1) }
        },
        { // Z
            { new(0, 0), new(-1, 0), new(0, -1), new(1, -1) },
            { new(0, 0), new(-1, 0), new(-1, -1), new(0, 1) },
            { new(0, 0), new(-1, 0), new(0, -1), new(1, -1) },
            { new(0, 0), new(-1, 0), new(-1, -1), new(0, 1) },
        }
    };

    
    public List<Tuple<int, int, string>> Pile = [];
    //           x    y    idx  rot
    Piece piece;
    private int piecemiw = 0;
    private int piecemaw = 0;
    private int piecemih = 0;
    private int piecemah = 0;
    public static List<string> cpairs = [];
    public int Width { get; set; }
    public int Height { get; set; }
    public TetrisEngine(int width, int height) {
        Width = width;
        Height = height;
        NewPiece();
    }

    void NewPiece() {
        List<string> chars = [];
        Random rand = new Random();
        for (int i = 0; i < 4; i++)
            chars.Add(cpairs[rand.Next(cpairs.Count)]);
        piece = new(Width / 2, 2, rand.Next(7), /*chars.ToArray()*/ "AA", "BB", "CC", "DD");
        RefreshPieceRange();
    }

    void RefreshPieceRange() {
        piecemiw = 0;
        piecemaw = 0;
        piecemih = 0;
        piecemah = 0;
        for (int i = 0; i < 4; i++) {
            var cell = tetrominoes[piece.Type, piece.Rotation, i];
            piecemiw = Math.Min(piecemiw, cell.Item1);
            piecemaw = Math.Max(piecemaw, cell.Item1);
            piecemih = Math.Min(piecemih, cell.Item2);
            piecemah = Math.Max(piecemah, cell.Item2);
        }
    }
    DateTime lastUpdate = DateTime.Now;
    private bool paused = false;
    
    void Refresh() {
        int x = Console.WindowWidth / 2 - Width;
        int y = Console.WindowHeight / 2 - Height / 2;
        Console.SetCursorPosition(x - 1, y - 2);
        Console.Write($"╔"+ "╡TetriSH╞".PadRight(Width*2, '═') + "╗");
        Console.SetCursorPosition(x - 1, y - 1);
        Console.Write($"║"+ " Lines: 0 Time: 0:00".PadRight(Width*2) + "║░");
        Console.SetCursorPosition(x - 1, y);
        Console.Write($"╟{new string('─', Width*2)}╢░");
        for (int i = 0; i < Height; i++) {
            Console.SetCursorPosition(x - 1, y + i + 1);
            Console.Write($"║{new string(' ', Width*2)}║░");
        }
        Console.SetCursorPosition(x - 1, Console.WindowHeight/2 + Height/2 + 1);
        Console.Write($"╚{new string('═', Width*2)}╝░");
        Console.SetCursorPosition(x, Console.WindowHeight/2 + Height/2 + 2);
        Console.Write(new string('░', Width*2+2));
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.Black;
        for (int i = 0; i < 4; i++) {
            var cell = tetrominoes[piece.Type, piece.Rotation, i];
            Console.SetCursorPosition(x + piece.X*2 + cell.Item1*2, y + piece.Y + cell.Item2 + 1);
            Console.Write(piece.Chars[i]);
        }
        Console.ResetColor();
        foreach (var block in Pile) {
            Console.SetCursorPosition(x + block.Item1*2, y + block.Item2);
            Console.Write(block.Item3);
        }
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"({piecemiw}, {piecemaw})");
        Console.WriteLine($"({piecemih}, {piecemah})");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"({piece.X}, {piece.Y}, {piece.Rotation})");
        Console.WriteLine($"({Width}, {Height})");
        Console.ResetColor();
        if (paused) {
            Console.SetCursorPosition(Console.WindowWidth/2 - 4, Console.WindowHeight/2);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Write(" Paused ");
            Console.ResetColor();
        }
        toRefresh = false;
    }

    private int pW = 0;
    private int pH = 0;
    bool toRefresh;
    // Will implement once the pile is done
    bool CheckCollision() {
        return true;
    }
    public void Run() {
        while (true) {
            if (pW != Console.WindowWidth || pH != Console.WindowHeight)
                toRefresh = true;
            if (Console.WindowWidth < Width*2 + 2 || Console.WindowHeight < Height + 3) {
                Console.Clear();
                Popup.Quick(
                    $"This terminal is too small (Minimum size: {Width * 2 + 2}x{Height + 3} characters). Please increase the size of your terminal and press any key to continue.", title: "Screen too small", type: PopupType.Error);
                toRefresh = true;
                continue;
            }
            if (toRefresh)
                Refresh();
            if (Console.KeyAvailable) {
                var key = Console.ReadKey(true);
                toRefresh = true;
                if (paused) {
                    paused = false;
                    continue;
                }

                int dx = 0;
                if (key.Key == ConsoleKey.Q && SelectPopup.Quick("Are you sure you want to quit?", ["Yes", "No"],
                        title: "Confirm", type: PopupType.Question) == "Yes") {
                    break;
                } else if (key.Key == ConsoleKey.LeftArrow) {
                    piece.X--;
                    dx = -1;
                } else if (key.Key == ConsoleKey.RightArrow) {
                    piece.X++;
                    dx = 1;
                } else if (key.Key == ConsoleKey.DownArrow) {
                    piece.Y++;
                } else if (key.Key == ConsoleKey.Z) {
                    piece.Rotation--;
                    if (piece.Rotation < 0)
                        piece.Rotation = 3;
                    RefreshPieceRange();
                } else if (key.Key == ConsoleKey.X) {
                    piece.Rotation++;
                    if (piece.Rotation >= 4)
                        piece.Rotation = 0;
                    RefreshPieceRange();
                } else if (key.Key == ConsoleKey.P) {
                    paused = true;
                }
            }

            if ((DateTime.Now - lastUpdate).TotalMilliseconds > 1000 && !paused) {
                lastUpdate = DateTime.Now;
                piece.Y++;
                toRefresh = true;
                if (piece.Y + piecemah == Height) {
                    for (int i = 0; i < 4; i++) {
                        var cell = tetrominoes[piece.Type, piece.Rotation, i];
                        Pile.Add(new(piece.X + cell.Item1, piece.Y + cell.Item2, piece.Chars[i]));
                    }
                    NewPiece();
                }
            }

            if (piecemiw + piece.X < 0)
                piece.X++;
            else if (piecemaw + piece.X >= Width)
                piece.X--;
            
            if (piecemih + piece.Y < 0)
                piece.Y++;
            else if (piecemah + piece.Y >= Height)
                piece.Y--;
            
            pW = Console.WindowWidth;
            pH = Console.WindowHeight;
        }
    }
}

struct Piece {
    public int X { get; set; }
    public int Y { get; set; }
    public int Type { get; set; }
    public int Rotation { get; set; } = 0;
    public String[] Chars { get; set; }

    public Piece(int x, int y, int type, params string[] chars) {
        X = x;
        Y = y;
        Type = type;
        Chars = chars;
    }
}