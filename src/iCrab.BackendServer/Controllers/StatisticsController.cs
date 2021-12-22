using iCrabee.BackendServer.Authorization;
using iCrabee.BackendServer.Constant;
using iCrabee.BackendServer.Data;
using iCrabee.ViewModels.Statistics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace iCrabee.BackendServer.Controllers
{
    public class StatisticsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("monthly-comments")]
        [ClaimRequirement(FunctionCode.STATISTIC, CommandCode.VIEW)]
        public async Task<IActionResult> GetMonthlyNewComments(int year)
        {
            var data = await _context.Comments.Where(x => x.CreateDate.Date.Year == year)
                .GroupBy(x => x.CreateDate.Date.Month)
                .OrderBy(x => x.Key)
                .Select(g => new MonthlyCommentsVM()
                {
                    Month = g.Key,
                    NumberOfComments = g.Count()
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("monthly-newkbs")]
        [ClaimRequirement(FunctionCode.STATISTIC, CommandCode.VIEW)]
        public async Task<IActionResult> GetMonthlyNewKbs(int year)
        {
            var data = await _context.KnowledgeBases.Where(x => x.CreateDate.Date.Year == year)
                .GroupBy(x => x.CreateDate.Date.Month)
                .Select(g => new MonthlyNewKbsVM()
                {
                    Month = g.Key,
                    NumberOfNewKbs = g.Count()
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("monthly-registers")]
        [ClaimRequirement(FunctionCode.STATISTIC, CommandCode.VIEW)]
        public async Task<IActionResult> GetMonthlyNewRegisters(int year)
        {
            var data = await _context.Users.Where(x => x.CreateDate.Date.Year == year)
               .GroupBy(x => x.CreateDate.Date.Month)
               .Select(g => new MonthlyNewKbsVM()
               {
                   Month = g.Key,
                   NumberOfNewKbs = g.Count()
               })
               .ToListAsync();

            return Ok(data);
        }
    }
}
