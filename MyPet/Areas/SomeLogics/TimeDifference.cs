using System;

namespace MyPet.Areas.SomeLogics
{
    public class TimeDifference
    {

        public static string GetDifference(DateTime dateTime)
        {
            TimeSpan timeSpan = DateTime.Now - dateTime;
            int seconds = (int)timeSpan.TotalSeconds;
            int minutes = (int)timeSpan.TotalMinutes;
            int hours = (int)timeSpan.TotalHours;
            int days = (int)timeSpan.TotalDays;

            if(seconds == 0)
            {
                return $"только что";
            }
            if (seconds < 60)
            {
                return $"{seconds} {(seconds == 1 ? "секунду" : seconds < 5 ? "секунды" : "секунд")} назад";
            }
            else if (minutes < 60)
            {
                return $"{minutes} {(minutes == 1 ? "минуту" : minutes < 5 ? "минуты" : "минут")} назад";
            }
            else if (hours < 24)
            {
                return $"{hours} {(hours == 1 ? "час" : hours < 5 ? "часа" : "часов")} назад";
            }
            else
            {
                return $"{days} {(days == 1 ? "день" : days < 5 ? "дня" : "дней")} назад";
            }
        }

        public static string GetDifferenceEng(DateTime _dateTime)
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