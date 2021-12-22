using iCrabee.BackendServer.Authorization;
using iCrabee.BackendServer.Constant;
using iCrabee.BackendServer.Data;
using iCrabee.BackendServer.Data.Entities;
using iCrabee.BackendServer.Helpers;
using iCrabee.BackendServer.Services;
using iCrabee.ViewModels;
using iCrabee.ViewModels.Contents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iCrabee.BackendServer.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;

        public CategoriesController(ApplicationDbContext context,
            ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.CONTENT_CATEGORY, CommandCode.CREATE)]
        [ApiValidationFilter]
        public async Task<IActionResult> PostCategory([FromBody] CategoryCreateRequest request)
        {
            var category = new Category()
            {
                Name = request.Name,
                ParentId = request.ParentId,
                SortOrder = request.SortOrder,
                SeoAlias = request.SeoAlias,
                SeoDescription = request.SeoDescription
            };
            _context.Categories.Add(category);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                await _cacheService.RemoveAsync("Categories");

                return CreatedAtAction(nameof(GetById), new { id = category.Id }, request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Create category failed"));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var cachedData = await _cacheService.GetAsync<List<CategoryVM>>("Categories");
            if (cachedData == null)
            {
                var categorys = await _context.Categories.ToListAsync();

                var categoryVMs = categorys.Select(c => CreateCategoryVM(c)).ToList();
                await _cacheService.SetAsync("Categories", categoryVMs);
                cachedData = categoryVMs;
            }

            return Ok(cachedData);
        }

        [HttpGet("filter")]
        [ClaimRequirement(FunctionCode.CONTENT_CATEGORY, CommandCode.VIEW)]
        public async Task<IActionResult> GetCategoriesPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.Contains(filter)
                || x.Name.Contains(filter));
            }
            var totalRecords = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var data = items.Select(c => CreateCategoryVM(c)).ToList();

            var pagination = new Pagination<CategoryVM>
            {
                Items = data,
                TotalRecords = totalRecords,
            };
            return Ok(pagination);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new ApiNotFoundResponse($"Category with id: {id} is not found"));

            CategoryVM categoryVM = CreateCategoryVM(category);

            return Ok(categoryVM);
        }

        [HttpPut("{id}")]
        [ClaimRequirement(FunctionCode.CONTENT_CATEGORY, CommandCode.UPDATE)]
        [ApiValidationFilter]
        public async Task<IActionResult> PutCategory(int id, [FromBody] CategoryCreateRequest request)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new ApiNotFoundResponse($"Category with id: {id} is not found"));

            if (id == request.ParentId)
            {
                return BadRequest(new ApiBadRequestResponse("Category cannot be a child itself."));
            }

            category.Name = request.Name;
            category.ParentId = request.ParentId;
            category.SortOrder = request.SortOrder;
            category.SeoDescription = request.SeoDescription;
            category.SeoAlias = request.SeoAlias;

            _context.Categories.Update(category);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                await _cacheService.RemoveAsync("Categories");

                return NoContent();
            }
            return BadRequest(new ApiBadRequestResponse("Update category failed"));
        }

        [HttpDelete("{id}")]
        [ClaimRequirement(FunctionCode.CONTENT_CATEGORY, CommandCode.DELETE)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new ApiNotFoundResponse($"Category with id: {id} is not found"));

            _context.Categories.Remove(category);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                await _cacheService.RemoveAsync("Categories");

                CategoryVM categoryVM = CreateCategoryVM(category);
                return Ok(categoryVM);
            }
            return BadRequest();
        }

        private static CategoryVM CreateCategoryVM(Category category)
        {
            return new CategoryVM()
            {
                Id = category.Id,
                Name = category.Name,
                SortOrder = category.SortOrder,
                ParentId = category.ParentId,
                NumberOfTickets = category.NumberOfTickets,
                SeoDescription = category.SeoDescription,
                SeoAlias = category.SeoAlias
            };
        }
    }
}
