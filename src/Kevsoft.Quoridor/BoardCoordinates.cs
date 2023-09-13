namespace Kevsoft.Quoridor;

/// <summary>
/// Board coordinates in algebraic notation
/// </summary>
public readonly record struct BoardCoordinates
{
    public const char MinLetter = 'a';
    public const char MaxLetter = 'i';
    public const int MinNumber = 1;
    public const int MaxNumber = 9;

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
        return new BoardCoordinates((char)(MinLetter + x), y);
    }

    public (int x, int y) ToGridCoordinates()
    {
        return (Letter - MinLetter, Number - MinNumber);
    }

    public static IEnumerable<BoardCoordinates> All
    {
        get
        {
            for (var x = MinLetter; x <= MaxLetter; x++)
            {
                for (var y = MinNumber; y <= MaxNumber; y++)
                {
                    yield return new BoardCoordinates(x, y);
                }
            }
        }
    }

    public static IDictionary<Players, IDictionary<Player, BoardCoordinates>> InitialPlayerCoordinates { get; } =
        new Dictionary<Players, IDictionary<Player, BoardCoordinates>>
        {
            [Players.Two] = new Dictionary<Player, BoardCoordinates>
            {
                [Player.One] = new('e', MinNumber),
                [Player.Two] = new('e', MaxNumber)
            },
            [Players.Four] = new Dictionary<Player, BoardCoordinates>
            {
                [Player.One] = new('e', MinNumber),
                [Player.Two] = new(MinLetter, 5),
                [Player.Three] = new('e', MaxNumber),
                [Player.Four] = new(MaxLetter, 5)
            }
        };

    private void EnsureValidBoardCoordinates(char letter, int number)
    {
        if (number is < MinNumber or > MaxNumber)
        {
            throw new ArgumentOutOfRangeException(nameof(number), number, "Number must be between 1 and 9 inclusive");
        }

        if (letter is < MinLetter or > MaxLetter)
        {
            throw new ArgumentOutOfRangeException(nameof(letter), letter,
                "Letter must be between 'a' and 'i' inclusive");
        }
    }

    public bool TryAdd(Direction direction, out BoardCoordinates boardCoordinates)
    {
        switch (direction)
        {
                case Direction.North when Number + 1 <= MaxNumber:
                    boardCoordinates = new BoardCoordinates(Letter, Number + 1);
                    return true;
                case Direction.South when Number - 1 >= MinNumber:
                    boardCoordinates = new BoardCoordinates(Letter, Number - 1);
                    return true;
                case Direction.East when Letter + 1 <= MaxLetter:
                    boardCoordinates = new BoardCoordinates((char)(Letter + 1), Number);
                    return true;
                case Direction.West when Letter - 1 >= MinLetter:
                    boardCoordinates = new BoardCoordinates((char)(Letter - 1), Number);
                    return true;
                default:
                    boardCoordinates = default;
                    return false;
        }

        ;
    }

    public BoardCoordinates Add(Direction direction)
    {
        if(TryAdd(direction, out var boardCoordinates))
            return boardCoordinates;
        
        throw new InvalidOperationException("Cannot add direction");
    }

    /// <summary></summary>
    public char Letter { get; }

    /// <summary></summary>
    public int Number { get; }
}