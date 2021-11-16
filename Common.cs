using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public static void LuuThongTinTaiKhoanKhiKetThuc(BindingList<ModelAccount> listAccNews)
        {
            var link = link_account;
            if (listAccNews != null)
            {
                var accounts = File.ReadAllText(link);
                var listAcc = JArray.Parse(accounts);
                for (int i = 0; i < listAccNews.Count(); i++)
                {
                    listAcc[i]["note"] = listAccNews[i].note;
                    listAcc[i]["uid"] = listAccNews[i].uid;
                    listAcc[i]["pass"] = listAccNews[i].pass;
                    listAcc[i]["_2fa"] = listAccNews[i]._2fa;
                    listAcc[i]["proxy"] = listAccNews[i].proxy;
                    listAcc[i]["stt"] = listAccNews[i].stt;
                    listAcc[i]["status"] = listAccNews[i].status;
                }
                File.WriteAllText(link, JsonConvert.SerializeObject(listAcc));
            }
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


        public static string GetOneImage(string link)
        {
            var currentPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string[] filePaths = Directory.GetFiles(link);
            return filePaths[new Random().Next(0, filePaths.Count())];
        }

        public static void ChangeNameImg(string link)
        {
            var currentPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string[] filePaths = Directory.GetFiles(link);
            foreach (var filePath in filePaths)
            {

                string path = filePath;
                string newFileName = Guid.NewGuid().ToString();
                string file = Path.GetFileNameWithoutExtension(path);
                string ext = Path.GetExtension(path);
                string NewPath = path.Replace(file, "tinh123");

                System.IO.File.Move(link + "\\" + file + ext, link + "\\" + newFileName + ext);

                //string path = filePath;
                //string dir = Path.GetDirectoryName(path);
                //string ext = Path.GetExtension(path);
                //path = Path.Combine(dir, newFileName + ext); // @"photo\myFolder\image-resize.jpg"
            }

        }
    }
}
