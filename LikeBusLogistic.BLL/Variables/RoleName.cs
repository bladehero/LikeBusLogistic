using System;
using System.ComponentModel;

namespace LikeBusLogistic.BLL.Variables
{
    [Flags]
    public enum RoleName
    {
        Unknown = 0,
        [Description("Администратор")]
        Administrator = 1,
        [Description("Модератор")]
        Moderator = 2,
        [Description("Оператор")]
        Operator = 4
    }
}
