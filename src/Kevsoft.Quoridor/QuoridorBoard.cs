namespace Kevsoft.Quoridor;

public class QuoridorBoard
{
    private readonly Square[][] _squares;

    public QuoridorBoard(Square[][] squares)
    {
        _squares = squares;
    }

    public Square GetSquare(BoardCoordinates coordinates)
        => _squares.GetSquare(coordinates);
    
    public Square FindPlayer(Player player)
        => _squares.SelectMany(x => x)
            .First(x => x.Pawn?.Player == player);

    internal bool MovePlayer(MovePawn move)
    {
        var findPlayer = FindPlayer(move.Player);

        switch (findPlayer)
        {   
            case { Pawn: null }:
                throw new InvalidOperationException("Cannot move to a square that is already occupied");
            
            case { Pawn: var pawn, NorthSquare: var northSquare } when move.Coordinates == northSquare?.Coordinates:
                northSquare.Pawn = pawn;
                break;
            
            case { Pawn: var pawn, SouthSquare: var southSquare } when move.Coordinates == southSquare?.Coordinates:
                southSquare.Pawn = pawn;
                break;

            case { Pawn: var pawn, EastSquare: var eastSquare } when move.Coordinates == eastSquare?.Coordinates:
                eastSquare.Pawn = pawn;
                break;
            case { Pawn: var pawn, WestSquare: var westSquare } when move.Coordinates == westSquare?.Coordinates:
                westSquare.Pawn = pawn;
                break;
            default:
                return false;
        }
        findPlayer.Pawn = null;
        return true;
    }

}