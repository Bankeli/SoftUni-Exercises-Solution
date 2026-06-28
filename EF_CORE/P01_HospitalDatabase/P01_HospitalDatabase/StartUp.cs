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

                dbContext.Database.EnsureDeleted();

                dbContext.Database.EnsureCreated();

                Console.WriteLine("DB is created successfully");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
