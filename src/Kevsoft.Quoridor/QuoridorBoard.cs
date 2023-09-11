namespace Kevsoft.Quoridor;

public record QuoridorBoard(Players Players)
{
    private readonly Square[][] _squares = QuoridorBoardBuilder.BuildSquares(Players);
    public Players Players { get; } = Players;

    public Square GetSquare(BoardCoordinates coordinates)
        => _squares.GetSquare(coordinates);
    
    public Square FindPlayer(Player player)
        => _squares.SelectMany(x => x)
            .First(x => x.Pawn?.Player == player);

    internal void MovePlayer(MovePawn move)
    {
        var findPlayer = FindPlayer(move.Player);
        var newSquare = GetSquare(move.Coordinates);

        if (newSquare is { Pawn: not null })
            throw new InvalidOperationException("Cannot move to a square that is already occupied");
        
        var pawn = findPlayer.Pawn;
        findPlayer.Pawn = null;
        newSquare.Pawn = pawn;
    }
}