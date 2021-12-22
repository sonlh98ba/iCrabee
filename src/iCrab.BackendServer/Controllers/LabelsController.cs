using iCrabee.BackendServer.Constant;
using iCrabee.BackendServer.Data;
using iCrabee.BackendServer.Helpers;
using iCrabee.BackendServer.Services;
using iCrabee.ViewModels.Contents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iCrabee.BackendServer.Controllers
{
    public class LabelsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;

        public LabelsController(ApplicationDbContext context, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _context = context;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string id)
        {
            var label = await _context.Labels.FindAsync(id);
            if (label == null)
                return NotFound(new ApiNotFoundResponse($"Label with id: {id} is not found"));

            var labelVM = new LabelVM()
            {
                Id = label.Id,
                Name = label.Name
            };

            return Ok(labelVM);
        }

        [HttpGet("popular/{take:int}")]
        [AllowAnonymous]
        public async Task<List<LabelVM>> GetPopularLabels(int take)
        {
            var cachedData = await _cacheService.GetAsync<List<LabelVM>>(CacheConstants.PopularLabels);
            if (cachedData == null)
            {
                var query = from l in _context.Labels
                            join lik in _context.LabelInKnowledgeBases on l.Id equals lik.LabelId
                            group new { l.Id, l.Name } by new { l.Id, l.Name } into g
                            select new
                            {
                                g.Key.Id,
                                g.Key.Name,
                                Count = g.Count()
                            };
                var labels = await query.OrderByDescending(x => x.Count).Take(take)
                    .Select(l => new LabelVM()
                    {
                        Id = l.Id,
                        Name = l.Name
                    }).ToListAsync();
                await _cacheService.SetAsync(CacheConstants.PopularLabels, labels);
                cachedData = labels;
            }

            return cachedData;
        }
    }
}
