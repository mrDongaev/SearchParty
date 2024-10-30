using System.ComponentModel.DataAnnotations;
using WebAPI.Contracts.User;

namespace WebAPI.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public class MmrValuesValidAttribute : ValidationAttribute
    {
        public MmrValuesValidAttribute()
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            bool validType = value.GetType() == typeof(GetConditionalUser.Request);
            if (value != null && validType)
            {
                var conditionConfig = value as GetConditionalUser.Request;
                if (conditionConfig.MinMmr != null && conditionConfig.MaxMmr != null && conditionConfig.MinMmr.Input < conditionConfig.MaxMmr.Input)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Minimum MMR must be lower than maximum MMR");
                }
            }
            else
            {
                return new ValidationResult($"Request is null or does not satisfy type: {typeof(GetConditionalUser.Request).FullName}");
            }

        }
    }
}
