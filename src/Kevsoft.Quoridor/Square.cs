namespace Kevsoft.Quoridor;

public class Square
{
    public Square(BoardCoordinates coordinates)
    {
        Coordinates = coordinates;
    }

    public BoardCoordinates Coordinates { get; }
    public Pawn? Pawn { get; set; }
    public Square? NorthSquare { get; set; }
    public Square? SouthSquare { get; set; }
    public Square? EastSquare { get; set; }
    public Square? WestSquare { get; set; }
}