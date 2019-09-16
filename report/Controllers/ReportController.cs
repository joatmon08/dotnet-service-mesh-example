using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Report.Models;
using OpenTracing;

namespace expense.Controllers
{
    [Route("api/[controller]/trip")]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ReportTotal>> GetReportForTrip(string id)
        {
            using (IScope scope = _tracer.BuildSpan("report-total").StartActive(finishSpanOnDispose: true))
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