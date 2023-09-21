namespace Kevsoft.Quoridor;

public static class SquaresBuilder
{
    public static (Square[][] squares, IReadOnlyDictionary<Player, Pawn> pawns) BuildSquares(Players players)
    {
        static void LinkSquares(Square[][] squares)
        {
            foreach (var coordinates in BoardCoordinates.All)
            {
                var (x, y) = coordinates.ToGridCoordinates();

                squares[x][y].NorthSquare = y == 8 ? null : squares[x][y + 1];
                squares[x][y].SouthSquare = y == 0 ? null : squares[x][y - 1];
                squares[x][y].EastSquare = x == 8 ? null : squares[x + 1][y];
                squares[x][y].WestSquare = x == 0 ? null : squares[x - 1][y];
            }
        }

        var squares = new Square[9][];
        for (var x = 0; x < 9; x++)
        {
            squares[x] = new Square[9];
            for (var y = 0; y < 9; y++)
            {
                squares[x][y] = new Square(new BoardCoordinates((char)('a' + x), y + 1));
            }
        }

        LinkSquares(squares);

        var pawns = PlacePlayers(squares, players);

        return (squares, pawns);
    }

    private static IReadOnlyDictionary<Player, Pawn> PlacePlayers(Square[][] squares, Players players)
    {
        return BoardCoordinates.InitialPlayerCoordinates[players]
            .Select(x =>
            {
                var playerSquare = squares.GetSquare(x.Value);
                return new Pawn(x.Key, playerSquare);
            })
            .ToDictionary(x => x.Player, x => x);
    }
}