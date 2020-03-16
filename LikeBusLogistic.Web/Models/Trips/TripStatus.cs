using System.ComponentModel;

namespace LikeBusLogistic.Web.Models.Trips
{
    public enum TripStatus
    {
        [Description("<span class=\"uk-label-warning\">Будущие</span>")]
        P,
        [Description("<span class=\"uk-label\">Текущие</span>")]
        S,
        [Description("<span class=\"uk-label-danger\">Задержанные</span>")]
        D,
        [Description("<span class=\"uk-label-success\">Завершенные</span>")]
        F,
    }
}
