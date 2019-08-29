using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expense.Models;

namespace expense.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseContext _context;

        public ExpenseController(ExpenseContext context)
        {
            _context = context;

            if (_context.ExpenseItems.Count() == 0)
            {
                _context.ExpenseItems.Add(new ExpenseItem { Name = "Seattle" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseItem>>> GetExpenseItems()
        {
            return await _context.ExpenseItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseItem>> GetExpenseItems(long id)
        {
            var expenseItem = await _context.ExpenseItems.FindAsync(id);

            if (expenseItem == null)
            {
                return NotFound();
            }

            return expenseItem;
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseItem>> PostExpenseItem(ExpenseItem item)
        {
            _context.ExpenseItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExpenseItems), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenseItem(long id, ExpenseItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseItem(long id)
        {
            var todoItem = await _context.ExpenseItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.ExpenseItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}