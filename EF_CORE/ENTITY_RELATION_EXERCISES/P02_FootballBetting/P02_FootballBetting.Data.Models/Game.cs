using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Game
{
    public Game()
    {
        Bets = new HashSet<Bet>();
        PlayersStatistics = new HashSet<PlayerStatistic>();
    }

    [Key]
    public int GameId { get; set; }

    public int HomeTeamId { get; set; }

    [ForeignKey(nameof(HomeTeamId))]
    [InverseProperty(nameof(Team.HomeGames))]
    public Team HomeTeam { get; set; } = null!;

    public int AwayTeamId { get; set; }

    [ForeignKey(nameof(AwayTeamId))]
    [InverseProperty(nameof(Team.AwayGames))]
    public Team AwayTeam { get; set; } = null!;

    public int HomeTeamGoals { get; set; }

    public int AwayTeamGoals { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal HomeTeamBetRate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal AwayTeamBetRate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DrawBetRate { get; set; }

    public DateTime DateTime { get; set; }

    public GameResult Result { get; set; }

    public ICollection<Bet> Bets { get; set; }

    public ICollection<PlayerStatistic> PlayersStatistics { get; set; }
}
