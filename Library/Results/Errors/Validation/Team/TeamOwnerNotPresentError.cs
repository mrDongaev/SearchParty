namespace Library.Results.Errors.Validation.Team
{
    public class TeamOwnerNotPresentError : ValidationError
    {
        public TeamOwnerNotPresentError(string message) : base(message)
        {
            WithMetadata("key", nameof(TeamOwnerNotPresentError));
        }

        public TeamOwnerNotPresentError() : this("Team must contain a player profile, created by its owner")
        {
        }
    }
}
