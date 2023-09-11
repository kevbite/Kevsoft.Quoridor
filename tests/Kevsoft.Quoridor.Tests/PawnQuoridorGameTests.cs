using FluentAssertions;

namespace Kevsoft.Quoridor.Tests;

public class PawnQuoridorGameTests
{
    [Theory]
    [InlineData(Players.Two, Player.One)]
    [InlineData(Players.Two, Player.Two)]
    [InlineData(Players.Four, Player.One)]
    [InlineData(Players.Four, Player.Two)]
    [InlineData(Players.Four, Player.Three)]
    [InlineData(Players.Four, Player.Four)]
    public void GivenATwoPlayerGame_WhenPlayerWrongPlayer_ShouldReturnSuccessAsFalseAndInvalidPlayer(Players players,
        Player player)
    {
        var game = QuoridorGame.New(players);
        if (player is Player.One)
        {
            var square = game.Board.FindPlayer(player);
            game.Play(new MovePawn(Player.One, square.Coordinates.Move(Direction.West)))
                .Success.Should().BeTrue();
        }

        var result = game.Play(new MovePawn(player, new BoardCoordinates('a', 1)));

        result.Should().BeEquivalentTo(new
        {
            Success = false
        }).And.BeOfType<InvalidPlayer>();
    }

    [Fact]
    public void GivenATwoPlayerGame_WhenPlayerOneMovesToTheOppositeSide_ShouldCompleteGameAndWin()
    {
        var game = QuoridorGame.New(Players.Two);

        for (var i = 0; i < 8; i++)
        {
            var player1 = game.Board.FindPlayer(Player.One);
            game.Play(new MovePawn(Player.One, player1.Coordinates.Move(Direction.North)))
                .Success.Should().BeTrue();

            var player2 = game.Board.FindPlayer(Player.Two);
            game.Play(new MovePawn(Player.Two, player2.Coordinates.Move(i < 3 ? Direction.East : Direction.West)))
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
            game.Play(new MovePawn(Player.One, player1.Coordinates.Move(i < 3 ? Direction.East : Direction.West)))
                .Success.Should().BeTrue();

            var player2 = game.Board.FindPlayer(Player.Two);
            game.Play(new MovePawn(Player.Two, player2.Coordinates.Move(Direction.South)))
                .Success.Should().BeTrue();
        }

        game.Finished.Should().BeTrue();
        game.Winner.Should().Be(Player.Two);
    }
}