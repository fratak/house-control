﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseControl.Sunset;

namespace HouseControl.Library
{
    public static class ScheduleHelper
    {
        private static ITimeProvider timeProvider;
        public static ITimeProvider TimeProvider
        {
            get
            {
                if (timeProvider == null)
                    timeProvider = new CurrentTimeProvider();
                return timeProvider;
            }
            set { timeProvider = value; }
        }

        private static ISunsetProvider sunsetProvider;
        public static ISunsetProvider SunsetProvider
        {
            get
            {
                if (sunsetProvider == null)
                    sunsetProvider = new SolarCalculatorSunsetProvider();
                return sunsetProvider;
            }
            set { sunsetProvider = value; }
        }

        public static DateTimeOffset Now()
        {
            return TimeProvider.Now();
        }

        public static DateTime Today()
        {
            return TimeProvider.Now().Date;
        }

        public static DateTime Yesterday()
        {
            return TimeProvider.Now().Date.AddDays(-1);
        }

        public static TimeSpan DurationFromNow(this DateTimeOffset checkTime)
        {
            return (checkTime - TimeProvider.Now()).Duration();
        }

        public static bool IsInPast(this DateTimeOffset checkTime)
        {
            return checkTime < TimeProvider.Now();
        }

        public static DateTimeOffset RollForwardToNextDay(ScheduleInfo info)
        {
            if (info.EventTime.IsInPast())
            {
                var nextDay = info.EventTime.Date + TimeSpan.FromDays(1);
                switch (info.TimeType)
                {
                    case ScheduleTimeType.Standard:
                        return nextDay + info.EventTime.TimeOfDay + info.RelativeOffset;
                    case ScheduleTimeType.Sunset:
                        return SunsetProvider.GetSunset(nextDay) + info.RelativeOffset;
                    case ScheduleTimeType.Sunrise:
                        return SunsetProvider.GetSunrise(nextDay) + info.RelativeOffset;
                }
            }
            return info.EventTime;
        }

        public static DateTimeOffset RollForwardToNextWeekdayDay(ScheduleInfo info)
        {
            if (info.EventTime.IsInPast())
            {
                var nextDay = Today() + TimeSpan.FromDays(1);
                while (nextDay.DayOfWeek == DayOfWeek.Saturday
                    || nextDay.DayOfWeek == DayOfWeek.Sunday)
                {
                    nextDay = nextDay.AddDays(1);
                }
                switch (info.TimeType)
                {
                    case ScheduleTimeType.Standard:
                        return nextDay + info.EventTime.TimeOfDay + info.RelativeOffset;
                    case ScheduleTimeType.Sunset:
                        return SunsetProvider.GetSunset(nextDay) + info.RelativeOffset;
                    case ScheduleTimeType.Sunrise:
                        return SunsetProvider.GetSunrise(nextDay) + info.RelativeOffset;
                }
            }
            return info.EventTime;
        }

        public static DateTimeOffset RollForwardToNextWeekendDay(ScheduleInfo info)
        {
            if (info.EventTime.IsInPast())
            {
                var nextDay = Today() + TimeSpan.FromDays(1);
                while (nextDay.DayOfWeek != DayOfWeek.Saturday
                    && nextDay.DayOfWeek != DayOfWeek.Sunday)
                {
                    nextDay = nextDay = nextDay.AddDays(1);
                }
                switch (info.TimeType)
                {
                    case ScheduleTimeType.Standard:
                        return nextDay + info.EventTime.TimeOfDay + info.RelativeOffset;
                    case ScheduleTimeType.Sunset:
                        return SunsetProvider.GetSunset(nextDay) + info.RelativeOffset;
                    case ScheduleTimeType.Sunrise:
                        return SunsetProvider.GetSunrise(nextDay) + info.RelativeOffset;
                }
            }
            return info.EventTime;
        }
    }
}
