using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace unlock_282
{
    class ObcProxy
    {
        HttpClient _httpClient;
        private string host = "http://192.168.1.14:6868/";
        private string ip = "192.168.1.14";
        public  ObcProxy(){
            _httpClient = new HttpClient() { BaseAddress = new Uri("http://192.168.1.14:6868/") };
        }

        public async Task ResetIPAsync(string port)
        {
            var http = await _httpClient.GetAsync($"reset?proxy={port}");
            var ress = http.Content.ReadAsStringAsync();
        }

        public async Task CheckActiveProxy(string port)
        {
            while (true)
            {
                var http = await _httpClient.GetAsync($"status?proxy={ip}:{port}");
                var ress = http.Content.ReadAsStringAsync();
                var rs = JsonConvert.DeserializeObject<JObject>(ress.ToString());
                if(Boolean.Parse(rs["status"].ToString()))
                {
                    return;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
