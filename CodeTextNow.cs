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
   public class CodeTextNow : ResolveCaptcha
    {
        //private string key = "07bf9f80";
        private string key;
        private HttpClient _httpClient;
        private string _session = "";
        public CodeTextNow(string _key)
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri("http://codetextnow.com/api.php") };
            key = _key;
        }

        public async Task<string> GetPhone()
        {
            try
            {
                var http = await _httpClient.GetAsync($"?apikey={key}&action=create-request&serviceId=1&count=1");
                var ress = http.Content.ReadAsStringAsync();
                var rs = JsonConvert.DeserializeObject<ModelGetPhoneTextNow>(ress.Result.ToString());
                _session = rs.results.data[0].requestId;
                return rs.results.data[0].sdt;
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
                while (i <= 77)
                {
                    var http = await _httpClient.GetAsync($"?apikey={key}&action=data-request&requestId={_session}");
                    var ress = http.Content.ReadAsStringAsync();
                    var rs = JsonConvert.DeserializeObject<ModelGetCodeTextNow>(ress.Result.ToString());
                    var otp = rs.data[0].otp;
                    if(otp != "")
                    {
                        return otp;

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

    public class ModelGetPhoneTextNow
    {
        public DataModel results { get; set; }

        public class DataModel
        {
            public List<Data2> data { get; set; }

            public class Data2 
            {
                public string sdt { get; set; }
                public string requestId { get; set; }
            }

        }
    }

    public class ModelGetCodeTextNow
    {
        public List<DataModel> data { get; set; }

        public class DataModel
        {
            public string otp { get; set; }
            
        }
    }
}
