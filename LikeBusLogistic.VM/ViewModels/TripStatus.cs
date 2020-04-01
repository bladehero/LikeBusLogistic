using System.ComponentModel;

namespace LikeBusLogistic.VM.ViewModels
{
    public enum TripStatus
    {
        [Description("<span class=\"uk-label uk-label-warning\">Будущий</span>")]
        P,
        [Description("<span class=\"uk-label\">Текущий</span>")]
        S,
        [Description("<span class=\"uk-label uk-label-danger\">Задержанный</span>")]
        D,
        [Description("<span class=\"uk-label uk-label-success\">Завершенный</span>")]
        F,
        [Description("<span class=\"uk-label uk-label-cancelled\">Отмененный</span>")]
        C,
    }
}
