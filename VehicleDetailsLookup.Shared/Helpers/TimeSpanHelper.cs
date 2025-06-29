namespace VehicleDetailsLookup.Shared.Helpers
{
    public static class TimeSpanHelper
    {
        public static string GetTimeSpan(DateTime dateTime, bool simplified)
        {
            var timeSpan = DateTime.UtcNow - dateTime;

            if (timeSpan.TotalMinutes < 1)
                return simplified ? "now" : "Just now";

            if (timeSpan.TotalHours < 1)
            {
                int minutes = (int)timeSpan.TotalMinutes;

                if (simplified)
                    return $"{minutes}m ago";

                return minutes == 1 ? "1 minute ago" : $"{minutes} minutes ago";
            }

            if (timeSpan.TotalDays < 1)
            {
                int hours = (int)timeSpan.TotalHours;

                if (simplified)
                    return $"{hours}h ago";

                return hours == 1 ? "1 hour ago" : $"{hours} hours ago";
            }

            int days = (int)timeSpan.TotalDays;

            if (simplified)
                return $"{days}d ago";

            return days == 1 ? "1 day ago" : $"{days} days ago";
        }
    }
}
