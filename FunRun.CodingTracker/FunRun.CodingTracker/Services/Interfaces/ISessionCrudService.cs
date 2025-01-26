using FunRun.CodingTracker.Data.Model;

namespace FunRun.CodingTracker.Services.Interfaces
{
    public interface ISessionCrudService
    {
        void CreateSession(CodingSession codingSession);
        void DeleteSession(CodingSession codingSession);
        List<CodingSession> GetAllSessions();
        void UpdateSession(CodingSession codingSession);
    }
}