using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BottleSplitter.Infrastructure;

public interface ISessionManager
{
    Task<Session?> GetSessionToken();
    Task SetSessionToken(Session session);
}

public class SessionManager(ProtectedLocalStorage protectedLocalStorage) : ISessionManager
{
    private Session? _session;

    public async Task<Session?> GetSessionToken()
    {
        if (_session is null)
        {
            var storageResult = await protectedLocalStorage.GetAsync<Session>("token");
            if (storageResult.Success)
            {
                _session = storageResult.Value;
            }
        }
        return _session;
    }
     public async Task SetSessionToken(Session session)
     {
         await protectedLocalStorage.SetAsync("token", session);
         _session = session;
     }
}

public record Session(string Type, string? AccessToken, string? Email, string? Name, string? UserId);
