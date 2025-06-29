namespace VehicleDetailsLookup.Client.Helpers
{
    public static class TimeSpanHelper
    {
        public static string GetTimeSpan(DateTime dateTime)
        {
            var timeSpan = DateTime.UtcNow - dateTime;

            if (timeSpan.TotalMinutes < 1)
                return "Just now";

            if (timeSpan.TotalHours < 1)
            {
                int minutes = (int)timeSpan.TotalMinutes;
                return minutes == 1 ? "1 minute ago" : $"{minutes} minutes ago";
            }

            if (timeSpan.TotalDays < 1)
            {
                int hours = (int)timeSpan.TotalHours;
                return hours == 1 ? "1 hour ago" : $"{hours} hours ago";
            }

            int days = (int)timeSpan.TotalDays;
            return days == 1 ? "1 day ago" : $"{days} days ago";
        }
    }
}
