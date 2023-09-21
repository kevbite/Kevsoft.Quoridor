using FluentAssertions;

namespace Kevsoft.Quoridor.Tests;

public class PawnQuoridorGameTests
{
    [Theory]
    [InlineData(Players.Two, Direction.North)]
    [InlineData(Players.Two, Direction.South)]
    [InlineData(Players.Two, Direction.East)]
    [InlineData(Players.Two, Direction.West)]
    [InlineData(Players.Four, Direction.North)]
    [InlineData(Players.Four, Direction.South)]
    [InlineData(Players.Four, Direction.East)]
    [InlineData(Players.Four, Direction.West)]
    public void GivenAGame_WhenMovingAValidMove_ShouldReturnSuccess(Players players, Direction direction)
    {
        var game = QuoridorGame.New(players);

        foreach (var (player, _) in BoardCoordinates.InitialPlayerCoordinates[players])
        {
            game.Play(new MovePawn(player, game.Board.FindPlayer(player).Coordinates.MoveAwayFromEdge()))
                .Success.Should().BeTrue();
        }

        foreach (var (player, _) in BoardCoordinates.InitialPlayerCoordinates[players])
        {
            game.Play(new MovePawn(player, game.Board.FindPlayer(player).Coordinates.Add(direction)))
                .Success.Should().BeTrue();
        }
    }

    public static IEnumerable<object[]> InvalidPlayerMoveCoordinatesForEachPlayersAndGame =>
        BoardCoordinates.InitialPlayerCoordinates.SelectMany(players =>
            players.Value.SelectMany(player =>
                BoardCoordinates.All.Except(new[] { player.Value }.Concat(player.Value.CoordinatesAround()))
                    .Select(coordinates => new object[] { players.Key, player.Key, coordinates })
            )
        );

    [Theory]
    [MemberData(nameof(InvalidPlayerMoveCoordinatesForEachPlayersAndGame))]
    public void GivenAGame_WhenMovingAInvalidValidMove_ShouldReturnSuccessFalseAndNotChangePlayer(Players players, Player player,
        BoardCoordinates coordinates)
    {
        var game = QuoridorGame.New(players);

        foreach (var (currentPlayer, _) in BoardCoordinates.InitialPlayerCoordinates[players])
        {
            if (currentPlayer == player)
            {
                var nextPlayer = game.NextPlayer;
                game.Play(new MovePawn(currentPlayer, coordinates))
                    .Success.Should().BeFalse();
                game.NextPlayer.Should().Be(nextPlayer);
                
                break;
            }

            game.Play(new MovePawn(currentPlayer, game.Board.FindPlayer(currentPlayer).Coordinates.MoveAwayFromEdge()))
                .Success.Should().BeTrue();
        }
    }

    [Theory]
    [InlineData(Players.Two, Player.One)]
    [InlineData(Players.Two, Player.Two)]
    [InlineData(Players.Four, Player.One)]
    [InlineData(Players.Four, Player.Two)]
    [InlineData(Players.Four, Player.Three)]
    [InlineData(Players.Four, Player.Four)]
    public void GivenATwoPlayerGame_WhenPlayingWrongPlayer_ShouldReturnSuccessAsFalseAndInvalidPlayer(Players players,
        Player player)
    {
        var game = QuoridorGame.New(players);
        if (player is Player.One)
        {
            var square = game.Board.FindPlayer(player);
            game.Play(new MovePawn(Player.One, square.Coordinates.Add(Direction.West)))
                .Success.Should().BeTrue();
        }

        var result = game.Play(new MovePawn(player, new BoardCoordinates('a', 1)));

        result.Should().BeEquivalentTo(new
        {
            Success = false,
            Failure = Failure.InvalidPlayer
        });
    }

    [Fact]
    public void GivenATwoPlayerGame_WhenPlayerOneMovesToTheOppositeSide_ShouldCompleteGameAndWin()
    {
        var game = QuoridorGame.New(Players.Two);

        for (var i = 0; i < 8; i++)
        {
            var player1 = game.Board.FindPlayer(Player.One);
            game.Play(new MovePawn(Player.One, player1.Coordinates.Add(Direction.North)))
                .Success.Should().BeTrue();

            var player2 = game.Board.FindPlayer(Player.Two);
            game.Play(new MovePawn(Player.Two, player2.Coordinates.Add(i < 3 ? Direction.East : Direction.West)))
                .Success.Should().BeTrue();
        }

        game.Finished.Should().BeTrue();
        game.Winner.Should().Be(Player.One);
    }

    [Fact]
    public void GivenATwoPlayerGame_WhenPlayerTwoMovesToTheOppositeSide_ShouldCompleteGameAndWin()
    {
        var game = QuoridorGame.New(Players.Two);

        for (var i = 0; i < 8; i++)
        {
            var player1 = game.Board.FindPlayer(Player.One);
            game.Play(new MovePawn(Player.One, player1.Coordinates.Add(i < 3 ? Direction.East : Direction.West)))
                .Success.Should().BeTrue();

            var player2 = game.Board.FindPlayer(Player.Two);
            game.Play(new MovePawn(Player.Two, player2.Coordinates.Add(Direction.South)))
                .Success.Should().BeTrue();
        }

        game.Finished.Should().BeTrue();
        game.Winner.Should().Be(Player.Two);
    }
}