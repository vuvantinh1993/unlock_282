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
   public class ResoveCaptcha
    {
        private string sitekey = "6Lc9qjcUAAAAADTnJq5kJMjN9aD1lxpRLMnCS2TR";
        private string pageurl = "https://m.facebook.com/checkpoint/1501092823525282/?next=https%3A%2F%2Fm.facebook.com%2Flogin.php&_rdr";
        private HttpClient _httpClient;
        private string key_captcha= "point_ce8116c04f96ef61e9bda31ebc10c877";
        private string id_captcha;

        public ResoveCaptcha()
        {
            var _httpClient = new HttpClient() { BaseAddress = new Uri("https://captcha69.com/in.php") };

        }

        public async Task SendKeyAsync()
        {
            var _httpClient = new HttpClient();
            var httpResponse = await  _httpClient.GetAsync($"?key={key_captcha}&googlekey={sitekey}&pageurl={pageurl}&submit=Send+in.php");
            var responseStream = await httpResponse.Content.ReadAsStringAsync();
            id_captcha = JsonConvert.DeserializeObject<JObject>(responseStream).ToString();
        }


        public async Task<string> GetToken()
        {
            var i = 0;
            var token = "";
            while (i < 300 && token == "")
            {
                var _httpClient = new HttpClient();
                var httpResponse = await _httpClient.GetAsync($"?key={key_captcha}&id={id_captcha}&submit=GET+res.php");
                token = await httpResponse.Content.ReadAsStringAsync();
                Thread.Sleep(5000);
                i += 5;
            }
            return token;
        }
    }
}
