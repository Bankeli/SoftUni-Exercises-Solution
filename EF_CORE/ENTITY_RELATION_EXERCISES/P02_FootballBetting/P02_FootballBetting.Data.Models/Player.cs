using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Player
{
    public Player()
    {
        PlayersStatistics = new HashSet<PlayerStatistic>();
    }

    [Key]
    public int PlayerId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public int SquadNumber { get; set; }

    public bool IsInjured { get; set; }

    public int PositionId { get; set; }

    [ForeignKey(nameof(PositionId))]
    public Position Position { get; set; } = null!;

    public int TeamId { get; set; }

    [ForeignKey(nameof(TeamId))]
    public Team Team { get; set; } = null!;

    public int TownId { get; set; }

    [ForeignKey(nameof(TownId))]
    public Town Town { get; set; } = null!;

    public ICollection<PlayerStatistic> PlayersStatistics { get; set; }
}
