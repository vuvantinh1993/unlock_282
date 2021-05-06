using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace unlock_282
{
   public class Chothuesimcode: ResolveCaptcha
    {
        //private string key = "07bf9f80";
        private string key;
        private HttpClient _httpClient;
        private string _session = "";
        public Chothuesimcode(string _key)
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri("https://chothuesimcode.com/api") };
            key = _key;
        }

        public async Task<string> GetPhone()
        {
            try
            {
                var http = await _httpClient.GetAsync($"?act=number&apik={key}&appId=1001");
                var ress = http.Content.ReadAsStringAsync();
                var rs = JsonConvert.DeserializeObject<ModelGetPhoneSimThue>(ress.Result.ToString());
                if(rs.Msg == "OK")
                {
                    _session = rs.Result.Id;
                    return rs.Result.Number;
                }
            }
            catch (Exception e)
            {
            }
            return "";
        }


        public async Task<string> GetCode()
        {
            try
            {
                var i = 0;
                while(i <= 40)
                {
                    var http = await _httpClient.GetAsync($"?act=code&apik={key}&id={_session}");
                    var ress = http.Content.ReadAsStringAsync();
                    var rs = JsonConvert.DeserializeObject<ModelGetCodeSimThue>(ress.Result.ToString());
                    if (rs.Msg == "Đã nhận được code")
                    {
                        if(rs.Result != null)
                        {
                            return rs.Result.Code;
                        }
                    }
                    if(rs.Msg == "không nhận được code, quá thời gian chờ")
                    {
                        return "";
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

    public class ModelGetPhoneSimThue
    {
        public string Msg { get; set; }
        public DataModel Result { get; set; }

        public class DataModel
        {
            public string Number { get; set; }
            public string Id { get; set; }
        }
    }

    public class ModelGetCodeSimThue
    {
        public string Msg { get; set; }
        public DataModel Result { get; set; }

        public class DataModel
        {
            public string Code { get; set; }
            
        }
    }
}
