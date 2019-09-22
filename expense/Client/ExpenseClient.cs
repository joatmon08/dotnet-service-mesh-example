using Newtonsoft.Json;
using System.Collections.Generic;
using Expense.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Expense.Client
{
  public class ExpenseClient : IExpenseClient
  {
    string _url;
    public static HttpClient client = new HttpClient();

    public ExpenseClient(string url)
    {
      _url = url;
    }

    public async Task<List<ExpenseItem>> GetExpensesForTrip(string tripId) {
      var result = await client.GetStringAsync(_url + "/api/expense/trip/" + tripId).ConfigureAwait(false);
      return JsonConvert.DeserializeObject<List<ExpenseItem>>(result);
    }

    public async Task<string> GetExpenseVersion() {
      var result = await client.GetStringAsync(_url + "/api").ConfigureAwait(false);
      return result.ToString();
    }
  }
}