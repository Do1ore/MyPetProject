using System;

namespace MyPet.Areas.SomeLogics
{
    public class TimeDifference
    { 

        public static string GetDifference(DateTime _dateTime)
        {
            TimeSpan timeSpan = DateTime.Now - _dateTime;

            if (timeSpan.TotalSeconds < 60)
            {
                return $"{Math.Round(timeSpan.TotalSeconds, 0)} seconds ago";
            }
            else if (timeSpan.TotalMinutes < 60)
            {
                return $"{Math.Round(timeSpan.TotalMinutes, 0)} minutes ago";
            }
            else if (timeSpan.TotalHours < 24)
            {
                return $"{Math.Round(timeSpan.TotalHours, 0)} hours ago";
            }
            else
            {
                return $"{Math.Round(timeSpan.TotalDays, 0)} days ago";
            }
        }
    }
}