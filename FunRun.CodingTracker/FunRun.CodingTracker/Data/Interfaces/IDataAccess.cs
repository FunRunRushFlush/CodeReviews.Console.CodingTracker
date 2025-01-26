using FunRun.CodingTracker.Data.Model;

namespace FunRun.CodingTracker.Data.Interfaces;

public interface IDataAccess
{
    void DeleteCodingSession(CodingSession session);
    void InsertCodingSession(CodingSession session);
    List<CodingSession> SqlGetAllCodingSessions();
    void UpdateCodingSession(CodingSession session);
}