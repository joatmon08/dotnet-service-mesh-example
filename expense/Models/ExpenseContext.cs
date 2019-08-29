using Microsoft.EntityFrameworkCore;

namespace Expense.Models
{
    public class ExpenseContext : DbContext
    {
        public ExpenseContext(DbContextOptions<ExpenseContext> options)
            : base(options)
        {
        }

        public DbSet<ExpenseItem> ExpenseItems { get; set; }
    }
}