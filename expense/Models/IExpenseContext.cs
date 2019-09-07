using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Expense.Models
{
    public interface IExpenseContext
    {
          Task<ActionResult<IEnumerable<ExpenseItem>>> ListAsync();
          Task<ExpenseItem> GetExpense(string id);
          Task<int> AddExpenseItem(ExpenseItem item);
          Task<int> UpdateExpenseItem(ExpenseItem item);
          Task<int> DeleteExpenseItem(ExpenseItem item);
    }
}