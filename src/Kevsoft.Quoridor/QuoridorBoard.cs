namespace Kevsoft.Quoridor;

public class QuoridorBoard
{
    private readonly Square[][] _squares;
    private readonly IReadOnlyDictionary<Player, Pawn> _pawns;

    public IReadOnlyDictionary<Player, Pawn> Pawns => _pawns;
    public QuoridorBoard(Square[][] squares, IReadOnlyDictionary<Player, Pawn> pawns)
    {
        _squares = squares;
        _pawns = pawns;
    }

    public Square GetSquare(BoardCoordinates coordinates)
        => _squares.GetSquare(coordinates);
    
    public Square FindPlayer(Player player)
        => _squares.SelectMany(x => x)
            .First(x => x.Pawn?.Player == player);

    internal bool MovePlayer(MovePawn move)
    {
        return _pawns[move.Player].Move(move.Coordinates);
    }

    public bool AddWall(AddWall addWall)
    {
        var square = GetSquare(addWall.Coordinates);

        switch (addWall.WallDirection)
        {
            case WallDirection.Horizontal:
            {
                square.NorthSquare.SouthSquare = null;
                square.EastSquare.NorthSquare.SouthSquare = null;
                square.EastSquare.NorthSquare = null;
                square.NorthSquare = null;
                break;
            }
            case WallDirection.Vertical:
            {
                square.EastSquare.WestSquare = null;
                square.NorthSquare.EastSquare.WestSquare = null;
                square.NorthSquare.EastSquare = null;
                square.EastSquare = null;
                break;
            }
            default:
                throw new InvalidOperationException();
        }

        return true;
    }
}