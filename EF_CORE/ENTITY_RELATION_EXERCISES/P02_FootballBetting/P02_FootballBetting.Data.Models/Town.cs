using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Town
{
    public Town()
    {
        Teams = new HashSet<Team>();
        Players = new HashSet<Player>();
    }

    [Key]
    public int TownId { get; set; }

    [Required]
    [MaxLength(80)]
    public string Name { get; set; } = null!;

    public int CountryId { get; set; }

    [ForeignKey(nameof(CountryId))]
    public Country Country { get; set; } = null!;

    public ICollection<Team> Teams { get; set; }

    public ICollection<Player> Players { get; set; }
}
