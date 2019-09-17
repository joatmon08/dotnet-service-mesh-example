using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Expense.Models;
using OpenTracing;

namespace expense.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseContext _context;
        private readonly ITracer _tracer;

        public ExpenseController(IExpenseContext context, ITracer tracer)
        {
            _context = context;
            _tracer = tracer;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseItem>>> GetExpenseItems()
        {
            using (IScope scope = _tracer.BuildSpan("expense-list").StartActive(finishSpanOnDispose: true))
            {
                return await _context.ListAsync();
            }
        }

        [HttpGet("trip/{tripId}")]
        public async Task<ActionResult<IEnumerable<ExpenseItem>>> GetExpenseItemsForTrip(string tripId)
        {
            using (IScope scope = _tracer.BuildSpan("expense-list-by-trip-id").WithTag("tripId", tripId).StartActive(finishSpanOnDispose: true))
            {
                var items = await _context.ListAsyncByTripId(tripId);
                return items;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseItem>> GetExpenseItem(string id)
        {
            using (IScope scope = _tracer.BuildSpan("expense-get-by-id").WithTag("id", id).StartActive(finishSpanOnDispose: true))
            {
                var expenseItem = await _context.GetExpense(id);
                if (expenseItem == null)
                {
                    return NotFound();
                }
                return expenseItem;
            }
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseItem>> PostExpenseItem(ExpenseItem item)
        {
            using (IScope scope = _tracer.BuildSpan("expense-create").StartActive(finishSpanOnDispose: true))
            {
                await _context.AddExpenseItem(item);
                return CreatedAtAction(nameof(GetExpenseItems), new { id = item.Id }, item);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenseItem(string id, ExpenseItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            using (IScope scope = _tracer.BuildSpan("expense-update").StartActive(finishSpanOnDispose: true))
            {
                await _context.UpdateExpenseItem(item);
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseItem(string id)
        {
            using (IScope scope = _tracer.BuildSpan("expense-delete").StartActive(finishSpanOnDispose: true))
            {
                var item = await _context.GetExpense(id);
                if (item == null)
                {
                    return NotFound();
                }
                await _context.DeleteExpenseItem(item);
                return NoContent();
            }
        }
    }
}