namespace Library.Results.Errors.Validation.Team
{
    public class TeamContainsPlayerError : ValidationError
    {
        public TeamContainsPlayerError() : base("Team already contains the applying player profile") { }
    }
}
