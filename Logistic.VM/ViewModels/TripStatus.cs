using System.ComponentModel;

namespace Logistic.VM.ViewModels
{
    public enum TripStatus
    {
        [Description("<span class=\"uk-label uk-label-warning\">In future</span>")]
        P,
        [Description("<span class=\"uk-label\">Current</span>")]
        S,
        [Description("<span class=\"uk-label uk-label-danger\">Delayed</span>")]
        D,
        [Description("<span class=\"uk-label uk-label-success\">Completed</span>")]
        F,
        [Description("<span class=\"uk-label uk-label-cancelled\">Cancelled</span>")]
        C,
    }
}
