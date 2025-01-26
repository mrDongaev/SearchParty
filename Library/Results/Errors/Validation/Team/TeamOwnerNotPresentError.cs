namespace Library.Results.Errors.Validation.Team
{
    public class TeamOwnerNotPresentError : ValidationError
    {
        public TeamOwnerNotPresentError() : base("Team must contain a player profile, created by its owner") { }
    }
}
