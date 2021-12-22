using iCrabee.ViewModels.Contents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace iCrabee.WebPortal.Services
{
    public class LabelApiClient : BaseApiClient, ILabelApiClient
    {
        public LabelApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<LabelVM> GetLabelById(string labelId)
        {
            return await GetAsync<LabelVM>($"/api/labels/{labelId}");
        }

        public async Task<List<LabelVM>> GetPopularLabels(int take)
        {
            return await GetListAsync<LabelVM>($"/api/labels/popular/{take}");
        }
    }
}
