using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace unlock_282
{
    public class TMPProxy
    {
        public async Task<string> GetNewProxyAsync(string key)
        {
            try
            {

                var _httpClient = new HttpClient();
                var model = new ModelPostTmp()
                {
                    api_key = key,
                    id_location = 0,
                    sign = "string"
                };
                var content = JsonConvert.SerializeObject(model);
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                var httpResponse = await _httpClient.PostAsync($"https://tmproxy.com/api/proxy/get-new-proxy", httpContent);
                var rsOld = await httpResponse.Content.ReadAsStringAsync();
                var ipOld = JsonConvert.DeserializeObject<ProxyTmp>(rsOld);
                return ipOld.data.https;
            }
            catch (Exception)
            {
            }
            return "";
        }
    }

    public class ModelPostTmp
    {
        public string api_key { get; set; }
        public string sign { get; set; }
        public int id_location { get; set; }
    }

    public class ProxyTmp
    {
        public string message { get; set; }
        public DataTmpproxy data { get; set; }
    }
    public class DataTmpproxy
    {
        public string https { get; set; }
        public string ip_allow { get; set; }
    }
}
