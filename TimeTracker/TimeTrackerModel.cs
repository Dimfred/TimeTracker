using System;

namespace TimeTracker
{
    public class TrackedItem
    {
        public string Name { get; set; }
        public Time Time { get; set; }

        public TrackedItem(string name, string time = "00:00:00")
        {
            Name = name;
            Time = new Time(time);
        }
    }

    public class Time
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        public Time(string timeStr /*format 00:00:00*/)
        {
            var timeArr = timeStr.Split(':');
            Hours = Convert.ToInt32(timeArr[0]);
            Minutes = Convert.ToInt32(timeArr[1]);
            Seconds = Convert.ToInt32(timeArr[2]);
        }

        public override string ToString()
        {
            return Hours.ToString("D2") + ":" + Minutes.ToString("D2")
                + ":" + Seconds.ToString("D2");
        }

        public void Count()
        {
            Seconds += 1;
            if (Seconds == 60)
            {
                Seconds = 0;
                Minutes += 1;
            }

            if (Minutes == 60)
            {
                Minutes = 0;
                Hours += 1;
            }
        }
    }
}
