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
        private string pageurl = "https://www.fbsbx.com/";
        private HttpClient _httpClient;
        private string key_captcha= "point_59c4a8da699f90c50577a8e02879bf84";
        private string id_captcha;

        public ResoveCaptcha()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri("https://captcha69.com/") };
        }

        public async Task SendKeyAsync()
        {
            try
            {
                var responseStream = "";
                while(!responseStream.Contains("OK|"))
                {
                    var httpResponse = await  _httpClient.GetAsync($"in.php?key={key_captcha}&googlekey={sitekey}&pageurl={pageurl}&submit=Send+in.php");
                    responseStream = await httpResponse.Content.ReadAsStringAsync();
                    id_captcha = responseStream.Replace("OK|", "");
                }
            }
            catch (Exception e)
            {
            }
        }


        public async Task<string> GetToken()
        {
            var i = 0;
            var responseStream = "";
            var token = "";
            try
            {
                while (i < 300 && !responseStream.Contains("OK|"))
                {
                    var httpResponse = await _httpClient.GetAsync($"res.php?key={key_captcha}&id={id_captcha}&submit=GET+res.php");
                    responseStream = await httpResponse.Content.ReadAsStringAsync();
                    Thread.Sleep(5000);
                    i += 5;
                }
                token.Replace("OK|", "");
            }
            catch (Exception e)
            {
            }
            return token;
        }
    }
}
