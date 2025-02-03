namespace Library.Results.Errors.Validation.Team
{
    public class TeamCountOverflowError : ValidationError
    {
        public TeamCountOverflowError(string message) : base(message)
        {
            WithMetadata("key", nameof(TeamCountOverflowError));
        }

        public TeamCountOverflowError(int maxTeamCount) : this($"Team cannot have more than {maxTeamCount} members")
        {
        }
    }
}
