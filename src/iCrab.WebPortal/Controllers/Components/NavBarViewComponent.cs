﻿using iCrabee.WebPortal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace iCrabee.WebPortal.Controllers.Components
{
    public class NavBarViewComponent : ViewComponent
    {
        private ICategoryApiClient _categoryApiClient;

        public NavBarViewComponent(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryApiClient.GetCategories();
            return View("Default", categories);
        }
    }
}
