using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LikeBusLogistic.Web.Variables
{
    public static class GlobalVariables
    {
    }

    public enum RoleNames
    {
        [Description("Администратор")]
        Administrator,
        [Description("Модератор")]
        Moderator,
        [Description("Оператор")]
        Operator
    }
}
