namespace Kevsoft.Quoridor;

public class Square
{
    public Square(BoardCoordinates coordinates)
    {
        Coordinates = coordinates;
    }

    public BoardCoordinates Coordinates { get; }

    public Pawn? Pawn { get; internal set; }
    public Square? NorthSquare { get; internal set; }
    public Square? SouthSquare { get; internal set; }
    public Square? EastSquare { get; internal set; }
    public Square? WestSquare { get; internal set; }
    
}