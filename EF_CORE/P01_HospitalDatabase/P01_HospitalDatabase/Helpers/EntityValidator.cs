using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Helpers
{
    public class EntityValidator : IEntityValidator
    {
        public bool TryValidate<TEntity>(TEntity entity, out IReadOnlyList<string> errors)
            where TEntity : class
        {
            var validationContext = new ValidationContext(entity);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                entity,
                validationContext,
                validationResults,
                validateAllProperties: true);

            errors = validationResults
                .Select(r => r.ErrorMessage ?? "Validation error.")
                .ToList();

            return isValid;
        }
    }
}
