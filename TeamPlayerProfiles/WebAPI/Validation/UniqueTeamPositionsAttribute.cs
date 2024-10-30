using System.ComponentModel.DataAnnotations;
using WebAPI.Contracts.Team;

namespace WebAPI.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public class UniqueTeamPositionsAttribute : ValidationAttribute
    {
        public UniqueTeamPositionsAttribute()
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            bool isSet = value.GetType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ISet<>)) && value.GetType().GetGenericArguments().Any(x => x == typeof(UpdateTeamPlayers.Request));
            if (value != null && isSet)
            {
                var players = value as IEnumerable<UpdateTeamPlayers.Request>;
                if (players.Select(p => p.Position).ToHashSet().Count == players.Count())
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Players in team must have unique positions");
                }
            }
            else
            {
                return new ValidationResult($"Player list is null or does not satisfy type: {typeof(UpdateTeamPlayers.Request).FullName}");
            }

        }
    }
}
