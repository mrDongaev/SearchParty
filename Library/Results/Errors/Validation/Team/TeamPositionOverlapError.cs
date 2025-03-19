namespace Library.Results.Errors.Validation.Team
{
    public class TeamPositionOverlapError : ValidationError
    {
        public TeamPositionOverlapError(string message, object? data = null) : base(message, data)
        {
            WithMetadata("key", nameof(TeamPositionOverlapError));
        }

        public TeamPositionOverlapError(object? data = null) : this("Team cannot have multiple player profiles in a repeating position", data)
        {
        }

        public TeamPositionOverlapError(int position, object? data = null) : this($"Team cannot have multiple player profiles in a repeating position: {position}", data)
        {
        }

        public TeamPositionOverlapError(IEnumerable<int> positions, object? data = null) : this($"Team cannot have multiple player profiles in repeating positions: {string.Join(", ", positions)}", data)
        {
        }
    }
}
