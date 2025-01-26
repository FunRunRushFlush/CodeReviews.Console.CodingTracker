namespace FunRun.CodingTracker.Data.Model;

public record CodingSession(long Id, DateTime startTime, DateTime endTime)
{
    public long Id =Id;
    public DateTime StartTime = startTime;
    public DateTime EndTime = endTime;
    public TimeSpan Duration = CalculatingDuration(startTime, endTime);

    private static TimeSpan CalculatingDuration(DateTime startTime, DateTime endTime)
    {
        return endTime - startTime;
    }
}
