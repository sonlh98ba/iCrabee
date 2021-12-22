using iCrabee.ViewModels.Contents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace iCrabee.WebPortal.Services
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<List<CategoryVM>> GetCategories()
        {
            return await GetListAsync<CategoryVM>("/api/categories");
        }

        public async Task<CategoryVM> GetCategoryById(int id)
        {
            return await GetAsync<CategoryVM>($"/api/categories/{id}");
        }
    }
}
