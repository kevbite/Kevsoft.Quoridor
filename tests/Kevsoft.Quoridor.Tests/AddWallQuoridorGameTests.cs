using FluentAssertions;

namespace Kevsoft.Quoridor.Tests;

public class AddWallQuoridorGameTests
{
    [Fact]
    public void GivenAGame_WhenAddingVerticalWall_ShouldReturnSuccessAndNullConnectingSquares()
    {
        var game = QuoridorGame.New(Players.Two);

        var coordinates = new BoardCoordinates('a', 1);
        game.Play(new AddWall(Player.One, coordinates, WallDirection.Vertical))
            .Success.Should().BeTrue();

        game.Board.GetSquare(coordinates)
            .EastSquare.Should().BeNull();

        game.Board.GetSquare(coordinates.Add(Direction.East))
            .WestSquare.Should().BeNull();

        game.Board.GetSquare(coordinates.Add(Direction.North))
            .EastSquare.Should().BeNull();

        game.Board.GetSquare(coordinates.Add(Direction.East).Add(Direction.North))
            .WestSquare.Should().BeNull();
    }

    [Fact]
    public void GivenAGame_WhenAddingVerticalWall_ShouldReturnSuccessAndBoardWallsAreCorrect()
    {
        var game = QuoridorGame.New(Players.Two);

        var coordinates = new BoardCoordinates('a', 1);
        game.Play(new AddWall(Player.One, coordinates, WallDirection.Vertical))
            .Success.Should().BeTrue();

        game.GetWalledSquares()
            .Should()
            .HaveCount(2)
            .And
            .BeEquivalentTo(new[]
                {
                    new Wall(coordinates, WallDirection.Vertical),
                    new Wall(coordinates.Add(Direction.North), WallDirection.Vertical),
                }
            );
    }

    [Fact]
    public void GivenAGame_WhenAddingHorizontalWall_ShouldReturnSuccessAndNullConnectingSquares()
    {
        var game = QuoridorGame.New(Players.Two);

        var coordinates = new BoardCoordinates('a', 1);
        game.Play(new AddWall(Player.One, coordinates, WallDirection.Horizontal))
            .Success.Should().BeTrue();

        game.Board.GetSquare(coordinates)
            .NorthSquare.Should().BeNull();

        game.Board.GetSquare(coordinates.Add(Direction.North))
            .SouthSquare.Should().BeNull();

        game.Board.GetSquare(coordinates.Add(Direction.East))
            .NorthSquare.Should().BeNull();

        game.Board.GetSquare(coordinates.Add(Direction.East).Add(Direction.North))
            .SouthSquare.Should().BeNull();
    }

    [Fact]
    public void GivenAGame_WhenAddingHorizontalWall_ShouldReturnSuccessAndBoardWallsAreCorrect()
    {
        var game = QuoridorGame.New(Players.Two);

        var coordinates = new BoardCoordinates('a', 1);
        game.Play(new AddWall(Player.One, coordinates, WallDirection.Horizontal))
            .Success.Should().BeTrue();

        game.GetWalledSquares()
            .Should()
            .HaveCount(2)
            .And
            .BeEquivalentTo(new[]
                {
                    new Wall(coordinates, WallDirection.Horizontal),
                    new Wall(coordinates.Add(Direction.East), WallDirection.Horizontal),
                }
            );
    }
}