namespace Kevsoft.Quoridor.Tests;

public static class BoardCoordinatesExtensions
{
    public static BoardCoordinates MoveAwayFromEdge(this BoardCoordinates coordinates)
    {
        return coordinates switch
        {
            { Letter: BoardCoordinates.MinLetter } => coordinates.Add(Direction.East),
            { Letter: BoardCoordinates.MaxLetter } => coordinates.Add(Direction.West),
            { Number: BoardCoordinates.MinNumber } => coordinates.Add(Direction.North),
            { Number: BoardCoordinates.MaxNumber } => coordinates.Add(Direction.South),
            _ => throw new InvalidOperationException()
        };
    }
    
    public static IEnumerable<BoardCoordinates> CoordinatesAround(this BoardCoordinates coordinates)
    {
        foreach (var direction in Directions.All)
        {
            if (coordinates.TryAdd(direction, out var c))
                yield return c;
        }
    }
}