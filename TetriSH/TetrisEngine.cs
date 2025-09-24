namespace TetriSH;

public class TetrisEngine {
    public static Tuple<int, int>[,,] tetrominoes = new Tuple<int, int>[,,] {
        { // straight
            { new(-1, 0), new(0, 0), new(1, 0), new(2, 0) },
            { new(0, -1), new(0, 0), new(0, 1), new(0, 2) },
            { new(-2, 0), new(-1, 0), new(0, 0), new(1, 0) },
            { new(0, -2), new(0, -1), new(0, 0), new(0, 1) }
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
            { new(0, 0), new(-1, 0), new(-1, -1), new(0, 1) },
            { new(0, 0), new(1, 0), new(0, -1), new(-1, -1) },
            { new(0, 0), new(-1, 0), new(-1, -1), new(0, 1) }
        },
        { // Z
            { new(0, 0), new(-1, 0), new(0, -1), new(1, -1) },
            { new(0, 0), new(-1, 0), new(-1, 1), new(0, -1) },
            { new(0, 0), new(-1, 0), new(0, -1), new(1, -1) },
            { new(0, 0), new(-1, 0), new(-1, 1), new(0, -1) }
        }
    };
}