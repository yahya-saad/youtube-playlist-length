namespace PlaylistLengthCalculator.Models
{
    public class Playlist
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<Video> Videos { get; set; }

        public TimeSpan TotalDuration => Videos.Aggregate(TimeSpan.Zero, (sum, video) => sum.Add(video.Duration));

        public TimeSpan AverageVideoDuration => Videos.Count > 0 ? TimeSpan.FromTicks((long)Videos.Average(v => v.Duration.Ticks)) : TimeSpan.Zero;

        public TimeSpan GetDurationAtSpeed(double speed) => TimeSpan.FromTicks((long)(TotalDuration.Ticks / speed));
    }
}
