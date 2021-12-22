using iCrabee.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace iCrabee.WebPortal.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PaginationBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
