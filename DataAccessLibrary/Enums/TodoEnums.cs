using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Enums
{
    public enum Status
    {
        [EnumMember(Value = "New")]
        New,
        [EnumMember(Value = "Current")]
        Current,
        [EnumMember(Value = "Completed")]
        Completed
    }
}
