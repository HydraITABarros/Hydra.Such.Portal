using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.CCP
{
    public class DateAndTimeView
    {
        public string DateString { get; set; }
        public string TimeString { get; set; }

        public DateAndTimeView(DateTime _DateTime)
        {
            DateString = _DateTime.ToShortDateString();
            TimeString = _DateTime.ToShortTimeString();
        }

        public DateAndTimeView(DateTime _Date, TimeSpan _Time)
        {
            DateString = _Date.ToShortDateString();
            TimeString = _Time.ToString();
        }
    }
}
