namespace Common.Exceptions
{
    public class TeamCountOverflowException(uint maxTeamCount) : Exception($"Team cannot have more than {maxTeamCount} members")
    {
    }
}
