namespace Kevsoft.Quoridor;

/// <summary>
/// Board coordinates in algebraic notation
/// </summary>
public readonly struct BoardCoordinates
{
    /// <summary>
    /// Board coordinates in algebraic notation
    /// </summary>
    /// <param name="letter"></param>
    /// <param name="number"></param>
    public BoardCoordinates(char letter, int number)
    {
        EnsureValidBoardCoordinates(letter, number);
        Letter = letter;
        Number = number;
    }

    public static BoardCoordinates FromGridCoordinates(int x, int y)
    {
        return new BoardCoordinates((char)('a' + x), y);
    }

    public (int x, int y) ToGridCoordinates()
    {
        return (Letter - 'a', Number - 1);
    }

    public static IEnumerable<BoardCoordinates> All
    {
        get
        {
            for (var x = 'a'; x <= 'i'; x++)
            {
                for (var y = 1; y <= 9; y++)
                {
                    yield return new BoardCoordinates(x, y);
                }
            }
        }
    }

    private void EnsureValidBoardCoordinates(char letter, int number)
    {
        if (number is < 1 or > 9)
        {
            throw new ArgumentOutOfRangeException(nameof(number), "Number must be between 1 and 9 inclusive");
        }

        if (letter is < 'a' or > 'i')
        {
            throw new ArgumentOutOfRangeException(nameof(number), "Letter must be between 'a' and 'i' inclusive");
        }
    }

    public BoardCoordinates Move(Direction direction) =>
        direction switch
        {
            Direction.North => new BoardCoordinates(Letter, Number + 1),
            Direction.South => new BoardCoordinates(Letter, Number - 1),
            Direction.East => new BoardCoordinates((char)(Letter + 1), Number),
            Direction.West => new BoardCoordinates((char)(Letter - 1), Number),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Invalid direction to move")
        };

    /// <summary></summary>
    public char Letter { get; }

    /// <summary></summary>
    public int Number { get; }
}