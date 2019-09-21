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
      client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
      _url = url;
    }

    public async Task<List<ExpenseItem>> GetExpensesForTrip(string tripId) {
      var result = await client.GetStringAsync(_url + "/api/expense/trip/" + tripId);
      return JsonConvert.DeserializeObject<List<ExpenseItem>>(result);
    }

    public async Task<string> GetExpenseVersion() {
      return await client.GetStringAsync(_url + "/api");
    }
  }
}