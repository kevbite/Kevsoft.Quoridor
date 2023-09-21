namespace Kevsoft.Quoridor;

public class QuoridorGame
{
    private readonly Players _players;
    public QuoridorBoard Board { get; }

    public Player NextPlayer { get; private set; }

    public QuoridorGame(Players players, QuoridorBoard board)
    {
        _players = players;
        Board = board;
        NextPlayer = Player.One;
    }

    public static QuoridorGame New(Players players)
    {
        var (squares, pawns) = SquaresBuilder.BuildSquares(players);
        return new(players, new QuoridorBoard(squares, pawns));
    }

    private void ChangePlayer()
    {
        NextPlayer = _players switch
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
            return new FailurePlayResult(Failure.InvalidPlayer);

        var result = move switch
        {
            MovePawn m => MovePawn(m),
            AddWall m => AddWall(m),
            _ => throw new InvalidOperationException()
        };

        if (result.Success)
            ChangePlayer();

        return result;
    }

    private PlayResult MovePawn(MovePawn move)
    {
        if (Board.MovePlayer(move))
            return new SuccessPlayResult();

        return new FailurePlayResult(Failure.InvalidMove);
    }

    private PlayResult AddWall(AddWall addWall)
    {
        if (Board.AddWall(addWall))
            return new SuccessPlayResult();

        return new FailurePlayResult(Failure.InvalidMove);
    }

    public bool Finished => Winner is not null;

    public Player? Winner
    {
        get
        {
            var playerCoordinate = BoardCoordinates.InitialPlayerCoordinates[_players];

            foreach (var (player, _) in playerCoordinate)
            {
                var playerSquare = Board.Pawns[player].Square;
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

    public IEnumerable<Square> GetPlayerSquares()
    {
        return Board.Pawns.Values.Select(x => x.Square);
    }

    public IEnumerable<Wall> GetWalledSquares()
    {
        return BoardCoordinates.All.Select(x => Board.GetSquare(x))
                .Where(x => x is { NorthSquare: null } or { EastSquare: null })
                .SelectMany(x =>
                {
                    var walls = new List<Wall>();
                    if (x is { NorthSquare: null, Coordinates.Number: not BoardCoordinates.MaxNumber })
                        walls.Add(new(x.Coordinates, WallDirection.Horizontal));
                    if (x is { EastSquare: null, Coordinates.Letter: not BoardCoordinates.MaxLetter })
                        walls.Add(new(x.Coordinates, WallDirection.Vertical));
                    return walls;
                })
            ;
    }
}

public record Wall(BoardCoordinates Coordinates, WallDirection Direction);

public record Move(Player Player);

public record MovePawn(Player Player, BoardCoordinates Coordinates) : Move(Player);

public record AddWall(Player Player, BoardCoordinates Coordinates, WallDirection WallDirection) : Move(Player);

public enum WallDirection
{
    Horizontal,
    Vertical
}

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

public class FailurePlayResult : PlayResult
{
    public FailurePlayResult(Failure failure) : base(false)
    {
        Failure = failure;
    }

    public Failure Failure { get; }
}

public enum Failure
{
    InvalidPlayer,
    InvalidMove
}