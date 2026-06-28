namespace P01_HospitalDatabase.Helpers
{
    public interface IEntityValidator
    {
        bool TryValidate<TEntity>(TEntity entity, out IReadOnlyList<string> errors)
            where TEntity : class;
    }
}
