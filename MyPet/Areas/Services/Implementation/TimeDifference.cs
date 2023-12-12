using MyPet.Areas.Services.Abstractions;

namespace MyPet.Areas.Services.Implementation
{
    public class TimeDifference : ITimeDifference
    {
        public string GetDifference(DateTime dateTime)
        {
            TimeSpan timeSpan = DateTime.Now - dateTime;
            int seconds = (int)timeSpan.TotalSeconds;
            int minutes = (int)timeSpan.TotalMinutes;
            int hours = (int)timeSpan.TotalHours;
            int days = (int)timeSpan.TotalDays;

            if (seconds == 0)
            {
                return $"òîëüêî ÷òî";
            }
            if (seconds < 60)
            {
                return $"{seconds} {(seconds == 1 ? "ñåêóíäó" : seconds < 5 ? "ñåêóíäû" : "ñåêóíä")} íàçàä";
            }
            else if (minutes < 60)
            {
                return $"{minutes} {(minutes == 1 ? "ìèíóòó" : minutes < 5 ? "ìèíóòû" : "ìèíóò")} íàçàä";
            }
            else if (hours < 24)
            {
                return $"{hours} {(hours == 1 ? "÷àñ" : hours < 5 ? "÷àñà" : "÷àñîâ")} íàçàä";
            }
            else
            {
                return $"{days} {(days == 1 ? "äåíü" : days < 5 ? "äíÿ" : "äíåé")} íàçàä";
            }
        }

        public string GetDifferenceEng(DateTime _dateTime)
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