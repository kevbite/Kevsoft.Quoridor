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
        switch (players)
        {
            case Players.Two:
            {
                var player1Square = squares.GetSquare(new BoardCoordinates('e', 1));
                player1Square.Pawn = new Pawn(Player.One);

                var player2Square = squares.GetSquare(new BoardCoordinates('e', 9));
                player2Square.Pawn = new Pawn(Player.Two);

                break;
            }
            case Players.Four:
            {
                var player1Coordinates = new BoardCoordinates('e', 1);
                var player2Coordinates = new BoardCoordinates('a', 5);
                var player3Coordinates = new BoardCoordinates('e', 9);
                var player4Coordinates = new BoardCoordinates('i', 5);

                var player1Square = squares.GetSquare(player1Coordinates);
                player1Square.Pawn = new Pawn(Player.One);

                var player2Square = squares.GetSquare(player2Coordinates);
                player2Square.Pawn = new Pawn(Player.Two);

                var player3Square = squares.GetSquare(player3Coordinates);
                player3Square.Pawn = new Pawn(Player.Three);

                var player4Square = squares.GetSquare(player4Coordinates);
                player4Square.Pawn = new Pawn(Player.Four);

                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(players), "Only two or four players are supported");
        }
    }
}