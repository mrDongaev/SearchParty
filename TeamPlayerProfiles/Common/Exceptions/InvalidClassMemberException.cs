using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class InvalidClassMemberException(string memberName, string className) : ArgumentException($"{memberName} property does not exist on {className} type or the property type doesn't match the class member")
    {
    }
}
