namespace Library.Results.Errors.Validation.Team
{
    public class TeamCountOverflowError : ValidationError
    {
        public TeamCountOverflowError(string message, object? data = null) : base(message, data)
        {
            Metadata["ReasonName"] = nameof(TeamCountOverflowError);
        }

        public TeamCountOverflowError(int maxTeamCount, object? data = null) : this($"Team cannot have more than {maxTeamCount} members", data)
        {
        }
    }
}
