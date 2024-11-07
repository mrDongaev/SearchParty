namespace Common.Exceptions
{
    public class TeamCountOverflowException(int maxTeamCount) : Exception($"Team cannot have more than {maxTeamCount} members")
    {
    }
}
