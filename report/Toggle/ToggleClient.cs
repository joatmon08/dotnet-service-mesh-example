using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Consul;

namespace Toggle
{
  public class ToggleClient : IToggleClient
  {
    private readonly ConsulClient _consulClient;
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly string _consulAddress;
    public ToggleClient(string consulAddress, ConsulClient consulClient)
    {
      _consulClient = consulClient;
      _consulAddress = consulAddress;
    }

    public async Task<bool> GetToggleValue(string name)
    {
      var getPair = await _consulClient.KV.Get("toggles/" + name);
      if (getPair.StatusCode != System.Net.HttpStatusCode.OK)
      {
        return false;
      }
      if (Encoding.UTF8.GetString(getPair.Response.Value, 0, getPair.Response.Value.Length) == "true")
      {
        return true;
      }
      return false;
    }

    public async Task<bool> ToggleForExperiment(string name) {
      HttpResponseMessage result = await _httpClient.GetAsync(_consulAddress + "/v1/config/service-router/" + name).ConfigureAwait(false);
      if (result.StatusCode != System.Net.HttpStatusCode.OK)
      {
        return false;
      }
      return true;
    }

    public async Task<bool> ToggleForDatacenter()
    {
      var getPair = await _consulClient.KV.Get("toggles/datacenters");
      if (getPair.StatusCode == System.Net.HttpStatusCode.NotFound)
      {
        return false;
      }
      var datacenterList = Encoding.UTF8.GetString(getPair.Response.Value, 0, getPair.Response.Value.Length);
      var getDatacenters = await _consulClient.Catalog.Datacenters();
      return datacenterList.Contains(getDatacenters.Response[0]);
    }
  }
}