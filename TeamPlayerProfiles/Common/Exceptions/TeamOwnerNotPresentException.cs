namespace Common.Exceptions
{
    public class TeamOwnerNotPresentException() : Exception($"Team must contain a player profile, created by its owner")
    {
    }
}
