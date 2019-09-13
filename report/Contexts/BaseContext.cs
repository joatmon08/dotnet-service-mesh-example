using Expense.Client;
namespace Report.Contexts
{
    public abstract class BaseContext
    {
        protected readonly ExpenseClient _expenseClient;

        public BaseContext(ExpenseClient expenseClient)
        {
            _expenseClient = expenseClient;
        }
    }
}