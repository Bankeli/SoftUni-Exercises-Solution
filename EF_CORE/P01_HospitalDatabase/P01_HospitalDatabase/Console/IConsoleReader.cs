namespace P01_HospitalDatabase.IO
{
    public interface IConsoleReader
    {
        string ReadRequiredText(string message);

        string? ReadOptionalText(string message);

        int ReadInt(string message);

        DateTime ReadDate(string message);

        bool ReadBool(string message);
    }

}
