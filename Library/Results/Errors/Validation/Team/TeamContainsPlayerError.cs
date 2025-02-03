namespace Library.Results.Errors.Validation.Team
{
    public class TeamContainsPlayerError : ValidationError
    {
        public TeamContainsPlayerError() : this("Team already contains the applying player profile")
        {
        }

        public TeamContainsPlayerError(string message) : base(message)
        {
            WithMetadata("key", nameof(TeamContainsPlayerError));
        }
    }
}
