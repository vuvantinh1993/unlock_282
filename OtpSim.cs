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
   public class OtpSim
   {
        private string key = "4efce2d92c776bf3e0e57ee0347de7aa";
        private HttpClient _httpClient;
        private string _session = "";
        public OtpSim()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri( "http://www.otpsim.com/api/") };

        }

        public async Task<string> GetPhone()
        {
            try
            {
                var http = await _httpClient.GetAsync($"phones/request?token={key}&service=7&network=1,3");
                var ress = http.Content.ReadAsStringAsync();
                var rs = JsonConvert.DeserializeObject<ModelGetPhone>(ress.Result.ToString());
                if(rs.status_code == "200")
                {
                    _session = rs.data.session;
                    return rs.data.phone_number;
                }
            }
            catch (Exception e)
            {
            }
            throw new Exception();
        }


        public async Task<string> GetCode()
        {
            try
            {
                var i = 0;
                while(i <= 310)
                {
                    var http = await _httpClient.GetAsync($"sessions/{_session}?token={key}");
                    var ress = http.Content.ReadAsStringAsync();
                    var rs = JsonConvert.DeserializeObject<ModelGetCode>(ress.Result.ToString());
                    if (rs.status_code == "200")
                    {
                        if(rs.data.messages != null)
                        {
                            return rs.data.messages[0].otp;
                        }
                    }
                    Thread.Sleep(5000);
                    i += 5;
                }
            }
            catch (Exception e3)
            {

            }
            throw new Exception();
        }
    }

    public class ModelGetPhone
    {
        public string status_code { get; set; }
        public bool success { get; set; }
        public DataModel data { get; set; }

        public class DataModel
        {
            public string phone_number { get; set; }
            public string session { get; set; }
        }
    }

    public class ModelGetCode
    {
        public string status_code { get; set; }
        public bool success { get; set; }
        public DataModel data { get; set; }

        public class DataModel
        {
            public ModelMess[] messages { get; set; }
            
            public class ModelMess
            {
                public string otp { get; set; }
            }
        }
    }
}
