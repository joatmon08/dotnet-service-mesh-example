using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Report.Models;

namespace expense.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportContext _context;

        public ReportController(IReportContext context)
        {
            _context = context;
        }

        [HttpGet("expense/version")]
        public async Task<ActionResult<string>> GetVersion()
        {
            return await _context.GetExpenseVersion();
        }

        [HttpGet("trip/{id}")]
        public async Task<ActionResult<ReportTotal>> GetReportForTrip(string id)
        {
            var report = await _context.GetReportTotal(id);
            if (report == null)
            {
                return NotFound();
            }
            return report;
        }

    }
}