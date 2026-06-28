namespace P01_HospitalDatabase.IO
{
    public class ConsoleReader : IConsoleReader
    {
        public string ReadRequiredText(string message)
        {
            while (true)
            {
                System.Console.Write(message);
                string? input = System.Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }

                System.Console.WriteLine("Value is required.");
            }
        }

        public string? ReadOptionalText(string message)
        {
            System.Console.Write(message);
            string? input = System.Console.ReadLine();

            return string.IsNullOrWhiteSpace(input)
                ? null
                : input.Trim();
        }

        public int ReadInt(string message)
        {
            while (true)
            {
                System.Console.Write(message);
                if (int.TryParse(System.Console.ReadLine(), out int number))
                {
                    return number;
                }

                System.Console.WriteLine("Enter a valid number.");
            }
        }

        public DateTime ReadDate(string message)
        {
            while (true)
            {
                System.Console.Write(message);
                if (DateTime.TryParse(System.Console.ReadLine(), out DateTime date))
                {
                    return date;
                }

                System.Console.WriteLine("Enter a valid date.");
            }
        }

        public bool ReadBool(string message)
        {
            while (true)
            {
                System.Console.Write(message);
                string? input = System.Console.ReadLine()?.Trim().ToLower();

                if (input == "y" || input == "yes")
                {
                    return true;
                }

                if (input == "n" || input == "no")
                {
                    return false;
                }

                System.Console.WriteLine("Enter y or n.");
            }
        }
    }
}
