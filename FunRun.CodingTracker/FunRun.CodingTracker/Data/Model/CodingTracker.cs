namespace FunRun.CodingTracker.Data.Model;

public class CodingTrackerModel
{
    public long Id { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public long Duration { get; init; }

    public CodingTrackerModel() { }

    public CodingTrackerModel(long id, DateTime startTime, DateTime endTime, long duration)
    {
        Id = id;
        StartTime = startTime;
        EndTime = endTime;
        Duration = duration;
    }
}
