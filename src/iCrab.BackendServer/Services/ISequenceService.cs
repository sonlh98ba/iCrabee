using System.Threading.Tasks;

namespace iCrabee.BackendServer.Services
{
    public interface ISequenceService
    {
        Task<int> GetKnowledgeBaseNewId();
    }
}
