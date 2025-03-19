namespace Library.Results.Errors.Validation.Team
{
    public class TeamContainsPlayerError : ValidationError
    {
        public TeamContainsPlayerError(object? data = null) : this("Team already contains the applying player profile", data)
        {
        }

        public TeamContainsPlayerError(string message, object? data = null) : base(message, data)
        {
            Metadata["ReasonName"] = nameof(TeamContainsPlayerError);
        }
    }
}
