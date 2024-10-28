using System.ComponentModel;

namespace Common.Exceptions
{
    public class InvalidEnumMemberException(string enumArg, string enumTypeName) : InvalidEnumArgumentException($"{enumArg} does not exist on {enumTypeName} enum type")
    {
    }
}
