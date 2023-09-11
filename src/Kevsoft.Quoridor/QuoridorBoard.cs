namespace Kevsoft.Quoridor;

public record QuoridorBoard(Players Players)
{
    private readonly Square[][] _squares = QuoridorBoardBuilder.BuildSquares(Players);
    public Players Players { get; } = Players;

    public Square GetSquare(BoardCoordinates coordinates)
        => _squares.GetSquare(coordinates);
}