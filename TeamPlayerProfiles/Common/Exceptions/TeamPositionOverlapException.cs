namespace Common.Exceptions
{
    public class TeamPositionOverlapException() : Exception("Team cannot have multiple player profiles in the same position within the team")
    {
    }
}
