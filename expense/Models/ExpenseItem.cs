using System;

namespace Expense.Models
{
    public class ExpenseItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TripId { get; set; }

        public decimal Cost { get; set; }
        public string Currency {get; set; }

        public DateTime? Date {get; set; }
    }
}