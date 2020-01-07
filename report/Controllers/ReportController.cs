using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Report.Models;
using Expense.Client;
using System.Collections.Generic;
using Expense.Models;
using Toggle;

namespace expense.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ReportController : ControllerBase
    {
        private readonly IExpenseClient _client;
        private readonly IToggleClient _toggleClient;

        public ReportController(IExpenseClient client, IToggleClient toggleClient)
        {
            _client = client;
            _toggleClient = toggleClient;
        }

        [HttpGet("expense/version")]
        public async Task<ActionResult<string>> GetVersion()
        {
            return await _client.GetExpenseVersion();
        }

        [HttpGet("trip/{id}")]
        public async Task<ActionResult<ReportTotal>> GetReportForTrip(string id)
        {
            var items = await _client.GetExpensesForTrip(id);
            List<ExpenseItem> copied = new List<ExpenseItem>(items);
            var report = CalculateTotal(id, copied);
            if (report == null)
            {
                return NotFound();
            }
            return report;
        }

        private ReportTotal CalculateTotal(string tripId, IList<ExpenseItem> items)
        {
        decimal total = 0;
        foreach (ExpenseItem item in items)
        {
            total += item.Cost;
        }

        if (_toggleClient.GetToggleValue("enable-average").Result) {
            decimal average = 0;
            average = total / items.Count;
            return new ReportTotal {
                TripId = tripId,
                Total = total,
                Average = average,
                Expenses = items
            };
        }

        ReportTotal reportTotal = new ReportTotal
        {
            TripId = tripId,
            Total = total,
            Expenses = items
        };
        return reportTotal;
        }
    }
}