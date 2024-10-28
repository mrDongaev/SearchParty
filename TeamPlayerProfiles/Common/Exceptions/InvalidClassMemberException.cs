namespace Common.Exceptions
{
    public class InvalidClassMemberException(string memberName, string className) : ArgumentException($"{memberName} property does not exist on {className} type or the property type doesn't match the class member")
    {
    }
}
