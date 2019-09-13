using Report.Models;
using Expense.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using Expense.Models;

namespace Report.Contexts
{
  public class ReportContext : BaseContext, IReportContext
  {
    public ReportContext(ExpenseClient expenseClient) : base(expenseClient)
    {
    }

    public async Task<ReportTotal> GetReportTotal(string tripId)
    {

      var items = await _expenseClient.GetExpensesForTrip(tripId);
      return CalculateTotal(tripId, items);
    }

    public ReportTotal CalculateTotal(string tripId, IList<ExpenseItem> items)
    {
      decimal total = 0;
      foreach (ExpenseItem item in items)
      {
        total += item.Cost;
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