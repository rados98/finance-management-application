using System;

namespace Projekat.Helpers
{
    class DateTimeHelper
    {
        public static DateTime Tomorrow => DateTime.Today.AddDays(1);
    }
}
