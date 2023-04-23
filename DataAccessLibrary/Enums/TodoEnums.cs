using System.Runtime.Serialization;

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

    public enum Permission
    {
        [EnumMember(Value = "Owner")]
        Owner,
        [EnumMember(Value = "Collaborator")]
        Collaborator,
        [EnumMember(Value = "Denied")]
        Denied
    }
}
