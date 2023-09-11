namespace Kevsoft.Quoridor;

public static class SquareArrayExtensions
{
    public static Square GetSquare(this Square[][] squares, BoardCoordinates coordinates)
    {
        var (x, y) = coordinates.ToGridCoordinates();
        
        return squares[x][y];
    }
}