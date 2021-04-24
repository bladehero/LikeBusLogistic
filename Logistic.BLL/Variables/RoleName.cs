using System;
using System.ComponentModel;

namespace Logistic.BLL.Variables
{
    [Flags]
    public enum RoleName
    {
        Unknown = 0,
        [Description("Administrator")]
        Administrator = 1,
        [Description("Moderator")]
        Moderator = 2,
        [Description("Operator")]
        Operator = 4
    }
}
