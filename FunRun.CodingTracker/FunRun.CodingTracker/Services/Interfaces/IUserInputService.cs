using FunRun.CodingTracker.Data.Model;

namespace FunRun.CodingTracker.Services.Interfaces
{
    public interface IUserInputService
    {
        CodingSession ValidateUserSessionInput(CodingSession? codingSession = null);
    }
}