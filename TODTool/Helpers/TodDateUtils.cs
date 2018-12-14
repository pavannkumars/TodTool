using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace TODTool.Helpers
{
    public class TodDateUtils
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        public static DateTime GetCurrentTimeInIST()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        }

        public static DateTime GetFutureTimeInIST(int futuredays)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.Date.AddDays(futuredays).Date, INDIAN_ZONE);
        }

        public static string GetWeekOfYearFor(DateTime date)
        {
            // Gets the Calendar instance associated with a CultureInfo.
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            // Displays the total number of weeks in the current year.
            DateTime LastDay = new System.DateTime(date.Year, 12, 31);

            // Displays the number of the current week relative to the beginning of the year.
            Console.WriteLine("The CalendarWeekRule used for the en-US culture is {0}.", myCWR);
            Console.WriteLine("The FirstDayOfWeek used for the en-US culture is {0}.", myFirstDOW);
            Console.WriteLine("Therefore, the current week is Week {0} of the current year.", myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW));

            Console.WriteLine("There are {0} weeks in the current year ({1}).", myCal.GetWeekOfYear(LastDay, myCWR, myFirstDOW), LastDay.Year);
            return myCal.GetWeekOfYear(date, myCWR, myFirstDOW).ToString();
        }

    }
}