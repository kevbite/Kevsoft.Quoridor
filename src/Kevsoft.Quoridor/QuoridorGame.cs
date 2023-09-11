namespace Kevsoft.Quoridor;

public class QuoridorGame
{
    public QuoridorBoard Board { get; }

    public Player NextPlayer { get; private set; }

    private QuoridorGame(QuoridorBoard board)
    {
        Board = board;
        NextPlayer = Player.One;
    }

    public static QuoridorGame New(Players players)
        => new(new QuoridorBoard(players));

    private void ChangePlayer()
    {
        NextPlayer = Board.Players switch
        {
            Players.Two => NextPlayer switch
            {
                Player.One => Player.Two,
                _ => Player.One
            },
            Players.Four => NextPlayer switch
            {
                Player.One => Player.Two,
                Player.Two => Player.Three,
                Player.Three => Player.Four,
                Player.Four => Player.One,
                _ => throw new InvalidOperationException()
            },
            _ => throw new InvalidOperationException()
        };
    }

    public PlayResult Play(Move move)
    {
        if (move.Player != NextPlayer)
            return new InvalidPlayer();

        var result =  move switch
        {
            MovePawn m => MovePawn(m),
            _ => throw new InvalidOperationException()
        };
        
        if(result.Success)
            ChangePlayer();

        return result;
    }

    private PlayResult MovePawn(MovePawn move)
    {
        Board.MovePlayer(move);
        return new SuccessPlayResult();
    }

    public bool Finished => Winner is not null;

    public Player? Winner
    {
        get
        {
            var playerCoordinate = BoardCoordinates.InitialPlayerCoordinates[Board.Players];

            foreach (var (player, startingCoordinates) in playerCoordinate)
            {
                var playerSquare = Board.FindPlayer(player);
                switch (player)
                {
                    case Player.One when playerSquare.Coordinates.Number == BoardCoordinates.MaxNumber:
                        return player;
                    case Player.Two when playerSquare.Coordinates.Number == BoardCoordinates.MinNumber:
                        return player;
                }
            }

            return null;
        }
    }
}

public record Move(Player Player);

public record MovePawn(Player Player, BoardCoordinates Coordinates) : Move(Player);

public class PlayResult
{
    public PlayResult(bool success)
    {
        Success = success;
    }

    public bool Success { get; }
}

public class SuccessPlayResult : PlayResult
{
    public SuccessPlayResult() : base(true)
    {
    }
}

public class InvalidPlayerMove : PlayResult
{
    public InvalidPlayerMove() : base(false)
    {
    }
};

public class InvalidPlayer : PlayResult
{
    public InvalidPlayer() : base(false)
    {
    }
};