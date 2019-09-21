using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Report.Models;
using OpenTracing;

namespace expense.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportContext _context;
        private readonly ITracer _tracer;

        public ReportController(IReportContext context, ITracer tracer)
        {
            _context = context;
            _tracer = tracer;
        }

        [HttpGet("expense/version")]
        public async Task<ActionResult<string>> GetVersion()
        {
            using (IScope scope = _tracer.BuildSpan("report-expense-version").StartActive(finishSpanOnDispose: true))
            {
                return await _context.GetExpenseVersion();
            }
        }

        [HttpGet("trip/{id}")]
        public async Task<ActionResult<ReportTotal>> GetReportForTrip(string id)
        {
            using (IScope scope = _tracer.BuildSpan("report-total").WithTag("tripId", id).StartActive(finishSpanOnDispose: true))
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
}