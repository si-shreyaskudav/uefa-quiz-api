using System;

namespace  Gaming.Quiz.Contracts.Configuration
{

    #region " Background Serives "

    public class Services
    {
        public Notification Notification { get; set; }
        public Interval PointCal { get; set; }
        public Interval ServiceReminder { get; set; }
        public Interval Analytics { get; set; }
        public Interval PercentileUpdate { get; set; }
        public Interval EODsettlement { get; set; }
        public Interval GamedayMapping { get; set; }
    }

    public class Interval
    {
        public Int32 IntervalMinutes { get; set; }
    }

    public class Notification
    {
        public String Sender { get; set; }
        public String Recipient { get; set; }
    }

    #endregion
}
