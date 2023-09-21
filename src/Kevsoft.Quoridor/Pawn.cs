namespace Kevsoft.Quoridor;

public class Pawn
{
    public Pawn(Player player, Square square)
    {
        Player = player;
        Square = square;
        Square.Pawn = this;
    }

    public Player Player { get; }

    public Square Square { get; private set; }

    private void SetSquare(Square square)
    {
        Square.Pawn = null;
        Square = square;
        Square.Pawn = this;
    }

    public bool Move(BoardCoordinates coordinates)
    {
        if (coordinates == Square.NorthSquare?.Coordinates)
            SetSquare(Square.NorthSquare);
        else if (coordinates == Square.SouthSquare?.Coordinates)
            SetSquare(Square.SouthSquare);
        else if (coordinates == Square.EastSquare?.Coordinates)
            SetSquare(Square.EastSquare);
        else if (coordinates == Square.WestSquare?.Coordinates)
            SetSquare(Square.WestSquare);
        else
            return false;


        return true;
    }
}