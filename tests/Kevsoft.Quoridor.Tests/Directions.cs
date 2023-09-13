namespace Kevsoft.Quoridor.Tests;

public static class Directions
{
    public static IEnumerable<Direction> All { get; } = new[] { Direction.North, Direction.South, Direction.East, Direction.West };
}