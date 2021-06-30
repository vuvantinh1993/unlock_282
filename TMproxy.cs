using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlock_282
{
    public class TMproxy
    {
        private string _key;
        public TMproxy(string key)
        {
            _key = key;
        }

        public string GetCurrentProxy()
        {
            var str = "Lỗi: ";
            try
            {
                TMProxyHelper.TMAPIHelper.GetCurrentProxy(_key);
                var tmproxy = JsonConvert.DeserializeObject<JObject>(TMProxyHelper.TMAPIHelper.GetCurrentProxy(_key));
                str = tmproxy["data"]["https"].ToString();
            }
            catch (Exception e)
            {
                str += e.Message;
            }
            return str;
        }

        public string ResetProxy(string location = "1")
        {
            var str = "Lỗi: ";
            try
            {
                //var tmproxy = TMProxyHelper.TMAPIHelper.GetNewProxy(_key, "", location);

                var tmproxy = JsonConvert.DeserializeObject<JObject>(TMProxyHelper.TMAPIHelper.GetNewProxy(_key, "", location));
                if (tmproxy["message"].ToString() == "")
                {
                    str = tmproxy["data"]["https"].ToString();
                }
                else
                {
                    str += tmproxy["message"].ToString();
                }
            }
            catch (Exception e)
            {
                str += e.Message;
            }
            return str;
        }

    }
}
