using Dapper;
using FunRun.CodingTracker.Data.Interfaces;
using FunRun.CodingTracker.Data.Model;
using System.Data;

namespace FunRun.CodingTracker.Data;

public class DataAccess : IDataAccess
{
    private IDbConnection _connection;

    public DataAccess(IDbConnection connection)
    {
        _connection = connection;
    }

    public List<CodingSession> SqlGetAllCodingSessions()
    {
        List<CodingSession> codeSess = new List<CodingSession>();
        string query = "SELECT * FROM CodingTracker";
        List<CodingTrackerModel> codingTracker = _connection.Query<CodingTrackerModel>(
            query,
            new { Id = "id", StartTime = "startTime", EndTime = "endTime", Duration = "duration" }
        ).ToList();

        foreach (CodingTrackerModel model in codingTracker)
        {
            codeSess.Add(new CodingSession(model.Id, model.StartTime, model.EndTime));
        }
        return codeSess;
    }
    public void InsertCodingSession(CodingSession session)
    {
        const string query = @"
                INSERT INTO CodingTracker (StartTime, EndTime, Duration)
                VALUES (@StartTime, @EndTime, @Duration);
            ";

        _connection.Execute(query, new
        {
            session.StartTime,
            session.EndTime,
            Duration = session.Duration.TotalMinutes
        });
    }
    public void UpdateCodingSession(CodingSession session)
    {
        string query = $@"
                UPDATE {DbConsts.TableName}
                SET {DbConsts.StartTime} = @StartTime,
                    {DbConsts.EndTime} = @EndTime,
                    {DbConsts.Duration} = @Duration
                WHERE {DbConsts.Id} = @Id;
            ";

        _connection.Execute(query, new
        {
            Id = session.Id,
            StartTime = session.StartTime,
            EndTime = session.EndTime,
            Duration = session.Duration.TotalMinutes
        });
    }

    public void DeleteCodingSession(CodingSession session)
    {

        string query = $@"
            Delete From {DbConsts.TableName}
            WHERE {DbConsts.Id} = @Id;
        ";

        _connection.Execute(query, new
        {
            Id = session.Id,
        });
    }


}
