using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace PBL5.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this System.Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .GetName();
        }

        public static int CountWeekendDaysInAMonth(DateTime date)
        {
            var count = 0;
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                var dateOfMonth = new DateTime(date.Year, date.Month, day);
                if (dateOfMonth.DayOfWeek == DayOfWeek.Saturday || dateOfMonth.DayOfWeek == DayOfWeek.Sunday)
                {
                    count++;
                }
            }

            return count;
        }

        public static int CountWeekendDaysToPresentDay(DateTime startDate, DateTime endDate)
        {
            int count = 0;

            while (startDate <= endDate)
            {
                if (startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    count++;
                }

                startDate = startDate.AddDays(1);
            }

            return count;
        }

        public static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}