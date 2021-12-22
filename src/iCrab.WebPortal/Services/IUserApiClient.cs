using iCrabee.ViewModels;
using iCrabee.ViewModels.Contents;
using iCrabee.ViewModels.Systems;
using System.Threading.Tasks;

namespace iCrabee.WebPortal.Services
{
    public interface IUserApiClient
    {
        Task<UserVM> GetById(string id);

        Task<Pagination<KnowledgeBaseQuickVM>> GetKnowledgeBasesByUserId(string userId, int pageIndex, int pageSize);
    }
}
