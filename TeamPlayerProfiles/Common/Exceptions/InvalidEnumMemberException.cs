using Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class InvalidEnumMemberException(string enumArg, string enumTypeName) : InvalidEnumArgumentException($"{enumArg} does not exist on {enumTypeName} enum type")
    {
    }
}
