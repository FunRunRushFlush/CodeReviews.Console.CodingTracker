
using FunRun.CodingTracker.Data.Interfaces;
using FunRun.CodingTracker.Data.Model;
using FunRun.CodingTracker.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunRun.CodingTracker.Services;

public class SessionCrudService : ISessionCrudService
{
    private readonly IDataAccess _data;
    private readonly ILogger<SessionCrudService> _log;

    public SessionCrudService(IDataAccess data, ILogger<SessionCrudService> log)
    {
        _data = data;
        _log = log;
    }

    public List<CodingSession> GetAllSessions()
    {
        try
        {
            return _data.SqlGetAllCodingSessions();
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message, ex);

            return new List<CodingSession>();
        }
    }

    public void CreateSession(CodingSession codingSession)
    {
        try
        {
            _data.InsertCodingSession(codingSession);
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message, ex);
        }
    }
    public void UpdateSession(CodingSession codingSession)
    {
        try
        {
            _data.UpdateCodingSession(codingSession);
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message, ex);
        }
    }
    public void DeleteSession(CodingSession codingSession)
    {
        try
        {
            _data.DeleteCodingSession(codingSession);
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message, ex);
        }
    }

}
