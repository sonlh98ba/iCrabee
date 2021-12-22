using System.Threading.Tasks;

namespace iCrabee.BackendServer.Services
{
    public interface IOneSignalService
    {
        Task SendAsync(string title, string message, string url);
    }
}
