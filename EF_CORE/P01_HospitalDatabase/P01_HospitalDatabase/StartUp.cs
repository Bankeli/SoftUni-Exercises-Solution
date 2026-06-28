using P01_HospitalDatabase.Core;
using P01_HospitalDatabase.Data;

namespace P01_HospitalDatabase
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            try
            {
                using var dbContext = new HospitalContext();
                dbContext.Database.EnsureCreated();

                var engine = new Engine(dbContext);
                engine.Run();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }
        }
    }
}
