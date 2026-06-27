using P02_FootballBetting.Data;

namespace P02_FootballBetting;

public class StartUp
{
    static void Main(string[] args)
    {
        try
        {
            FootballBettingContext dbContext = new FootballBettingContext();
            dbContext.Database.EnsureDeleted();

            dbContext.Database.EnsureCreated();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
