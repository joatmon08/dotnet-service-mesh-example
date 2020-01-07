using System.Text;
using System.Threading.Tasks;
using Consul;

namespace Toggle
{
  public class ToggleClient : IToggleClient
  {
    private readonly ConsulClient _consulClient;
    public ToggleClient(ConsulClient consulClient)
    {
      _consulClient = consulClient;
    }

    public async Task<bool> GetToggleValue(string name) {
      var getPair = await _consulClient.KV.Get("toggles/" + name);
      if (Encoding.UTF8.GetString(getPair.Response.Value, 0, getPair.Response.Value.Length) == "true") {
        return true;
      }
      return false;
    }
  }
}