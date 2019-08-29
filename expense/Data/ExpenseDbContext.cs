using Expense.Models;
using Microsoft.EntityFrameworkCore;

namespace Expense.Data
{
    public class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
        {
        }

        public DbSet<ExpenseItem> ExpenseItems { get; set; }
    }
}