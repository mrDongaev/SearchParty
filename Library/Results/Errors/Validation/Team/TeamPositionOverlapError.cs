namespace Library.Results.Errors.Validation.Team
{
    public class TeamPositionOverlapError : ValidationError
    {
        public TeamPositionOverlapError() : base("Team cannot have multiple player profiles in a repeating position") { }

        public TeamPositionOverlapError(int position) : base($"Team cannot have multiple player profiles in a repeating position: {position}") { }

        public TeamPositionOverlapError(IEnumerable<int> positions) : base($"Team cannot have multiple player profiles in repeating positions: {string.Join(", ", positions)}") { }
    }
}
