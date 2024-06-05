using System;

namespace Bubla
{
    public class OfflineTracker
    {
        private static TimeSpan timeSpanData;

        public static void Init()
        {
            try
            {
                string[] time = Save.SRead("offline").Split(',');
                DateTime dateTime = new DateTime(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]), int.Parse(time[3]), int.Parse(time[4]), 0);
                timeSpanData = DateTime.Now.Subtract(dateTime);
            }
            catch (NullReferenceException e) { }
        }

        public int Minutes
        {
            get { return (int)timeSpanData.TotalMinutes; }
        }

        public int Hours
        {
            get { return (int)timeSpanData.TotalHours; }
        }

        public int Days
        {
            get { return (int)timeSpanData.TotalDays; }
        }

        public float Months
        {
            get { return (float)timeSpanData.TotalDays / 31; }
        }

        public float Years
        {
            get { return (float)timeSpanData.TotalDays / 365; }
        }
    }
}
