using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlock_282
{
    public  static class Common
    {
        public static string link_account = "nick.txt";
        public static string link_image = "";

        public static void GhiFileTaiKhoan(List<ModelAccount> listAccs)
        {
            File.WriteAllText(link_account, JsonConvert.SerializeObject(listAccs));
        }

        public static void XoaHetAccount()
        {
            File.WriteAllText(link_account, "");
        }

        public static BindingList<ModelAccount> DocFileTaiKhoan()
        {
            DanhSoLaiVaXoaTaiKhoanKoDungConvertStatus();
            var accounts = File.ReadAllText(link_account);
            var listAcc = JsonConvert.DeserializeObject<BindingList<ModelAccount>>(accounts);
            return listAcc;
        }

        private static void DanhSoLaiVaXoaTaiKhoanKoDungConvertStatus()
        {
            var accounts = File.ReadAllText(link_account);
            var listAccs = JsonConvert.DeserializeObject<List<ModelAccount>>(accounts);
            if (listAccs == null)
                return;
            var listAccsnew = listAccs.ToList();
            for (int i = 0; i < listAccsnew.Count(); i++)
            {
                listAccsnew[i].stt = i + 1;
                if (listAccsnew[i].note != "Oke!!" )
                {
                    listAccsnew[i].note = "chưa chạy";
                }
            }
            Common.GhiFileTaiKhoan(listAccsnew);
        }


        public static string GetOneImage()
        {
            var currentPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string[] filePaths = Directory.GetFiles($"{currentPath}image");
            return filePaths[new Random().Next(0, filePaths.Count())];
        }
    }
}
