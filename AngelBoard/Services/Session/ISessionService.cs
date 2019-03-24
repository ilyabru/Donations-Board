using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AngelBoard.Models;

namespace AngelBoard.Services
{
    public interface ISessionService
    {
        Task<Guid> InitializeSession(Guid savedSessionId);
        
        Task<Guid> CreateSession();
        Task<ObservableCollection<Session>> GetSessions();
        Task<Session> GetSession(Guid sessionId);
    }
}