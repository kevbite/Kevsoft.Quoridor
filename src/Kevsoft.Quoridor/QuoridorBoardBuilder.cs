namespace Kevsoft.Quoridor;

public static class QuoridorBoardBuilder
{
    public static Square[][] BuildSquares(Players players)
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

        PlacePlayers(squares, players);

        return squares;
    }

    private static void PlacePlayers(Square[][] squares, Players players)
    {
        var playerCoordinate = BoardCoordinates.InitialPlayerCoordinates[players];

        foreach (var (player, coordinates) in playerCoordinate)
        {
            var player1Square = squares.GetSquare(coordinates);
            player1Square.Pawn = new Pawn(player);
        }
    }
}