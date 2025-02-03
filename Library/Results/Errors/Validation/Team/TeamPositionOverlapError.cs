namespace Library.Results.Errors.Validation.Team
{
    public class TeamPositionOverlapError : ValidationError
    {
        public TeamPositionOverlapError(string message) : base(message)
        {
            WithMetadata("key", nameof(TeamPositionOverlapError));
        }

        public TeamPositionOverlapError() : this("Team cannot have multiple player profiles in a repeating position")
        {
        }

        public TeamPositionOverlapError(int position) : this($"Team cannot have multiple player profiles in a repeating position: {position}")
        {
        }

        public TeamPositionOverlapError(IEnumerable<int> positions) : this($"Team cannot have multiple player profiles in repeating positions: {string.Join(", ", positions)}")
        {
        }
    }
}
