using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace unlock_282
{
    public class Otpmmo : ResolveCaptcha
    {
        //private string key = "07bf9f80";
        private string key;
        private HttpClient _httpClient;
        private string _session = "";
        private string sdt = "";
        private string coookie = "";
        public Otpmmo(string _key)
        {
            _httpClient = new HttpClient() { };
            key = _key;
        }

        public Otpmmo(string _key, string cookie2)
        {
            _httpClient = new HttpClient() { };
            key = _key;
            coookie = cookie2;
        }

        public async Task<string> GetPhone2()
        {
            try
            {
                var http = await _httpClient.GetAsync($"http://otpmmo.xyz/textnow/api.php?apikey={key}&type=getphone&qty=1");
                var ress = await http.Content.ReadAsStringAsync();
                var rs = new Regex(@"\d{5,}").Match(ress.ToString()).Value;
                sdt = rs;
                return rs;
            }
            catch (Exception e)
            {
            }
            return "";
        }
        public async Task<string> GetPhone()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Remove("cookie");
                //_httpClient.DefaultRequestHeaders.Remove("user-agent");
                //_httpClient.DefaultRequestHeaders.Remove("x-requested-with");
                _httpClient.DefaultRequestHeaders.Add("Cookie", "PHPSESSID=61d0j15554fqu1deovt37u93nr; SL_GWPT_Show_Hide_tmp=1; SL_wptGlobTipTmp=1");
                //_httpClient.DefaultRequestHeaders.Add("user-agent", "PostmanRuntime/7.28.4");
                //_httpClient.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");
                //_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));//ACCEPT header
                //_httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data;");
                //var httpResponse = await _httpClient.PostAsync("https://otpmmo.com/textnow/ajaxgetdata.php", new System.Net.Http.FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
                //    new KeyValuePair<string, string>("type", "laysdt"),
                //    new KeyValuePair<string, string>("soluong", "1"),
                //}));
                //Thread.Sleep(new Random().Next(1000, 5000));

                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("type", "laythongtin")
                });
                var myHttpClient = new HttpClient();

                var httpResponse2 = await myHttpClient.PostAsync("https://otpmmo.com/textnow/ajaxgetdata.php", formContent);
                //httpResponse2.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                //httpResponse2.Content.Headers.ContentType.CharSet = @"utf-8";
                var ress = await httpResponse2.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<ModelGetDtaWWeb>>(ress);
                foreach (var item in data)
                {
                    if (!DocSdt().Contains(item.phonecode))
                    {
                        sdt = item.phonecode;
                        GhiSdt(sdt);
                        return sdt;
                    }
                }
            }
            catch (Exception e)
            {
            }
            return "";
        }

        private string DocSdt()
        {
            return File.ReadAllText(Common.linkSdt);
        }

        private void GhiSdt(string sdt)
        {
            while (true)
            {
                try
                {
                    File.AppendAllText(Common.linkSdt, sdt);
                    return;
                }
                catch (Exception)
                {
                    Thread.Sleep(new Random().Next(1000));
                }
            }
        }

        public async Task<string> GetCode()
        {
            try
            {
                var i = 0;
                while (i <= 77)
                {
                    var http = await _httpClient.GetAsync($"https://otpmmo.online/textnow/api.php?apikey={key}&type=getotp&sdt={sdt}");
                    var ress = await http.Content.ReadAsStringAsync();
                    if (ress.Contains("otp"))
                    {
                        var text = new Regex(@"otp.{15}").Match(ress).Value;
                        var otp = new Regex(@"\d{5,}").Match(text).Value;
                        if (otp != "")
                        {
                            return otp;
                        }
                    }
                    Thread.Sleep(5000);
                    i += 5;
                }
            }
            catch (Exception e3)
            {

            }
            return "";
        }
    }

    public class ModelGetDtaWWeb
    {
        public string phonecode { get; set; }
    }

    public class ModelGetPhoneOTPMMO
    {
        public string result { get; set; }
    }

    public class ModelOTP
    {
        public List<ModelData> Result { get; set; }
        public class ModelData
        {
            public string otp { get; set; }
        }

    }

}
