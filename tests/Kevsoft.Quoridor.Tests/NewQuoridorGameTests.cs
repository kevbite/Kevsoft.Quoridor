using FluentAssertions;

namespace Kevsoft.Quoridor.Tests;

public class NewQuoridorGameTests
{
    [Fact]
    public void WhenCreatingANewTwoPlayerQuoridorGame_ShouldStartPlayersInCorrectPositions()
    {
        var game = QuoridorGame.New(Players.Two);

        var initialPlayer1Square = game.Board.GetSquare(new BoardCoordinates('e', 1));
        initialPlayer1Square.Pawn.Should().Be(new Pawn(Player.One));

        var initialPlayer2Square = game.Board.GetSquare(new BoardCoordinates('e', 9));
        initialPlayer2Square.Pawn.Should().Be(new Pawn(Player.Two));
    }

    [Fact]
    public void WhenCreatingANewFourPlayerQuoridorGame_ShouldStartPlayersInCorrectPositions()
    {
        var game = QuoridorGame.New(Players.Four);

        var initialPlayer1Square = game.Board.GetSquare(new BoardCoordinates('e', 1));
        initialPlayer1Square.Pawn.Should().Be(new Pawn(Player.One));

        var initialPlayer2Square = game.Board.GetSquare(new BoardCoordinates('a', 5));
        initialPlayer2Square.Pawn.Should().Be(new Pawn(Player.Two));

        var initialPlayer3Square = game.Board.GetSquare(new BoardCoordinates('e', 9));
        initialPlayer3Square.Pawn.Should().Be(new Pawn(Player.Three));

        var initialPlayer4Square = game.Board.GetSquare(new BoardCoordinates('i', 5));
        initialPlayer4Square.Pawn.Should().Be(new Pawn(Player.Four));
    }

    [Theory]
    [InlineData(Players.Two, 2)]
    [InlineData(Players.Four, 4)]
    public void WhenCreatingANewQuoridorGame_ShouldOnlyHaveCorrectNumberOfPlayersOnBoard(Players players,
        int expectedPlayersOnBoard)
    {
        var game = QuoridorGame.New(players);

        BoardCoordinates.All.Select(game.Board.GetSquare)
            .Count(x => x.Pawn is not null)
            .Should()
            .Be(expectedPlayersOnBoard);
    }

    [Theory]
    [InlineData(Players.Two)]
    [InlineData(Players.Four)]
    public void WhenCreatingANewQuoridorGame_AllEdgeSquaresAreNotLinkedButLinkedInwards(Players players)
    {
        var game = QuoridorGame.New(players);

        for (var x = 'a'; x <= 'i'; x++)
        {
            var bottomSquare = game.Board.GetSquare(new BoardCoordinates(x, 1));
            bottomSquare.NorthSquare.Should()
                .BeSameAs(game.Board.GetSquare(bottomSquare.Coordinates.Add(Direction.North)));
            bottomSquare.SouthSquare.Should().BeNull();
            bottomSquare.EastSquare.Should()
                .Be(x is 'i' ? null : game.Board.GetSquare(bottomSquare.Coordinates.Add(Direction.East)));
            bottomSquare.WestSquare.Should()
                .Be(x is 'a' ? null : game.Board.GetSquare(bottomSquare.Coordinates.Add(Direction.West)));

            var topSquare = game.Board.GetSquare(new BoardCoordinates(x, 9));
            topSquare.NorthSquare.Should().BeNull();
            topSquare.SouthSquare.Should()
                .BeSameAs(game.Board.GetSquare(topSquare.Coordinates.Add(Direction.South)));
            topSquare.EastSquare.Should()
                .Be(x is 'i' ? null : game.Board.GetSquare(topSquare.Coordinates.Add(Direction.East)));
            topSquare.WestSquare.Should()
                .Be(x is 'a' ? null : game.Board.GetSquare(topSquare.Coordinates.Add(Direction.West)));
        }

        for (var y = 1; y <= 9; y++)
        {
            var leftSquare = game.Board.GetSquare(new BoardCoordinates('a', y));
            leftSquare.NorthSquare.Should()
                .Be(y is 9 ? null : game.Board.GetSquare(leftSquare.Coordinates.Add(Direction.North)));
            leftSquare.SouthSquare.Should()
                .Be(y is 1 ? null : game.Board.GetSquare(leftSquare.Coordinates.Add(Direction.South)));
            leftSquare.WestSquare.Should().BeNull();
            leftSquare.EastSquare.Should().BeSameAs(game.Board.GetSquare(leftSquare.Coordinates.Add(Direction.East)));

            var rightSquare = game.Board.GetSquare(new BoardCoordinates('i', y));
            rightSquare.NorthSquare.Should()
                .Be(y is 9 ? null : game.Board.GetSquare(rightSquare.Coordinates.Add(Direction.North)));
            rightSquare.SouthSquare.Should()
                .Be(y is 1 ? null : game.Board.GetSquare(rightSquare.Coordinates.Add(Direction.South)));
            rightSquare.EastSquare.Should().BeNull();
            rightSquare.WestSquare.Should()
                .BeSameAs(game.Board.GetSquare(rightSquare.Coordinates.Add(Direction.West)));
        }
    }

    [Theory]
    [InlineData(Players.Two)]
    [InlineData(Players.Four)]
    public void WhenCreatingANewQuoridorGame_AllInnerSquaresAreLinked(Players players)
    {
        var game = QuoridorGame.New(players);

        for (var x = 'b'; x <= 'h'; x++)
        {
            for (var y = 2; y <= 8; y++)
            {
                var boardCoordinates = new BoardCoordinates(x, y);
                var square = game.Board.GetSquare(boardCoordinates);
                var expected = game.Board.GetSquare(boardCoordinates.Add(Direction.North));
                square.NorthSquare.Should().BeSameAs(expected);
                square.SouthSquare.Should().BeSameAs(game.Board.GetSquare(boardCoordinates.Add(Direction.South)));
                square.EastSquare.Should().BeSameAs(game.Board.GetSquare(boardCoordinates.Add(Direction.East)));
                square.WestSquare.Should().BeSameAs(game.Board.GetSquare(boardCoordinates.Add(Direction.West)));
            }
        }
    }

    [Theory]
    [InlineData(Players.Two)]
    [InlineData(Players.Four)]
    public void WhenCreatingANewQuoridorGame_ShouldWinnerBeNullAndFinishedBeFalse(Players players)
    {
        var game = QuoridorGame.New(players);

        game.Finished.Should().BeFalse();
        game.Winner.Should().BeNull();
    }
}