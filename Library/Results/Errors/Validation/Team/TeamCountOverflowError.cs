namespace Library.Results.Errors.Validation.Team
{
    public class TeamCountOverflowError : ValidationError
    {
        public TeamCountOverflowError(int maxTeamCount) : base($"Team cannot have more than {maxTeamCount} members") { }
    }
}
