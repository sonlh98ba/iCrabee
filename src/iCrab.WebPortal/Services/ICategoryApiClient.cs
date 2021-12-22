using iCrabee.ViewModels.Contents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iCrabee.WebPortal.Services
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryVM>> GetCategories();
        Task<CategoryVM> GetCategoryById(int id);
    }
}
