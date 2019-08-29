using System;
using System.ComponentModel.DataAnnotations;

namespace Expense.Models
{
    public class ExpenseItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long TripId { get; set; }

        public float Cost { get; set; }
        public string Currency {get; set; }

        public DateTime? Date {get; set; }
    }
}