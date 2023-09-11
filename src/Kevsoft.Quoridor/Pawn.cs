namespace Kevsoft.Quoridor;

public record Pawn(Player Player)
{
    public Player Player { get; } = Player;
}