using System;
using Expense.Models;
using System.Collections.Generic;

namespace Report.Models
{
    public class ReportTotal
    {
        public string TripId { get; set; }
        public IList<ExpenseItem> Expenses { get; set; } = new List<ExpenseItem>();
        public decimal Total { get; set; }
    }
}