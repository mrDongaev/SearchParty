namespace Library.Results.Errors.Validation.Team
{
    public class TeamOwnerNotPresentError : ValidationError
    {
        public TeamOwnerNotPresentError(string message, object? data = null) : base(message, data)
        {
            WithMetadata("key", nameof(TeamOwnerNotPresentError));
        }

        public TeamOwnerNotPresentError(object? data = null) : this("Team must contain a player profile, created by its owner", data)
        {
        }
    }
}
