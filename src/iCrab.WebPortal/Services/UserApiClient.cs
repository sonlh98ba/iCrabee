using iCrabee.ViewModels;
using iCrabee.ViewModels.Contents;
using iCrabee.ViewModels.Systems;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace iCrabee.WebPortal.Services
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        public UserApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<UserVM> GetById(string id)
        {
            return await GetAsync<UserVM>($"/api/users/{id}", true);
        }

        public async Task<Pagination<KnowledgeBaseQuickVM>> GetKnowledgeBasesByUserId(string userId, int pageIndex, int pageSize)
        {
            var apiUrl = $"/api/users/{userId}/knowledgeBases?pageIndex={pageIndex}&pageSize={pageSize}";
            return await GetAsync<Pagination<KnowledgeBaseQuickVM>>(apiUrl, true);
        }
    }
}
