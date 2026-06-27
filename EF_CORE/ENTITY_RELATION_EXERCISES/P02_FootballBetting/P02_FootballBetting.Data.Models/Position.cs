using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models;

public class Position
{
    public Position()
    {
        Players = new HashSet<Player>();
    }

    [Key]
    public int PositionId { get; set; }

    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = null!;

    public ICollection<Player> Players { get; set; }
}
