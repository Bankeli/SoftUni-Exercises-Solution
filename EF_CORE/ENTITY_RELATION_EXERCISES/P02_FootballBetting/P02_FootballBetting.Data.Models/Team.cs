using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Team
{
    public Team()
    {
        Players = new HashSet<Player>();
        HomeGames = new HashSet<Game>();
        AwayGames = new HashSet<Game>();
    }

    [Key]
    public int TeamId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(2048)]
    public string LogoUrl { get; set; } = null!;

    [Required]
    [MaxLength(3)]
    public string Initials { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Budget { get; set; }

    public int PrimaryKitColorId { get; set; }

    [ForeignKey(nameof(PrimaryKitColorId))]
    [InverseProperty(nameof(Color.PrimaryKitTeams))]
    public Color PrimaryKitColor { get; set; } = null!;

    public int SecondaryKitColorId { get; set; }

    [ForeignKey(nameof(SecondaryKitColorId))]
    [InverseProperty(nameof(Color.SecondaryKitTeams))]
    public Color SecondaryKitColor { get; set; } = null!;

    public int TownId { get; set; }

    [ForeignKey(nameof(TownId))]
    public Town Town { get; set; } = null!;

    public ICollection<Player> Players { get; set; }

    [InverseProperty(nameof(Game.HomeTeam))]
    public ICollection<Game> HomeGames { get; set; }

    [InverseProperty(nameof(Game.AwayTeam))]
    public ICollection<Game> AwayGames { get; set; }
}
