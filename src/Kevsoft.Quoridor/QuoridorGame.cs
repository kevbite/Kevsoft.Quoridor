namespace Kevsoft.Quoridor;

public class QuoridorGame
{
    public QuoridorBoard Board { get; }

    private QuoridorGame(QuoridorBoard board)
    {
        Board = board;
    }

    public static QuoridorGame New(Players players)
        => new(new QuoridorBoard(players));
}