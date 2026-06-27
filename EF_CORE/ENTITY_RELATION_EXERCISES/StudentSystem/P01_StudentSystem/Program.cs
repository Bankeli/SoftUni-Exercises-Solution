using P01_StudentSystem.Data;

namespace P01_StudentSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
			try
			{
                using var dbContext = new StudentSystemContext();
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
}
