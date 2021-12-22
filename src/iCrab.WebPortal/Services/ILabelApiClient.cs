using iCrabee.ViewModels.Contents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iCrabee.WebPortal.Services
{
    public interface ILabelApiClient
    {
        Task<List<LabelVM>> GetPopularLabels(int take);

        Task<LabelVM> GetLabelById(string labelId);
    }
}
