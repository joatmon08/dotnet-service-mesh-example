using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Expense.Models
{
    public interface IVersionContext
    {
        string GetVersion();
    }
}