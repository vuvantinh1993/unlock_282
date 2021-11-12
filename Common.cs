using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace unlock_282
{
    public  static class Common
    {
        public static string link_account = "nick.txt";
        public static string link_image = "";


        public static string RandomHo()
        {
            var list = new List<string>() { "Nguyễn", "Trần", "Lê", "Phạm", "Hoàng", "Phan", "Vũ", "Võ", "Đặng", "Bùi", "Đỗ" };
            return list[new Random().Next(0, list.Count())];
        }

        public static string RanTenDem()
        {
            var list = new List<string>() { "Hoài", "Thuỳ", "Tú", "Bích", "Bảo", "Nguyệt", "Bảo", "Minh", "Hương", "Lan", "Linh", "Mai", "Hạc", "Nhật", "Huyền", "Vinh", "Vân", "Hạnh", "Dung", "Thiên", "Hải", "Hướng", "Kim", "Minh", "Trúc", "Hạ", "Hồng", "Khánh", "Lam", "Lệ", "Bảo", "Linh", "Ngân", "Hà", "Vân", "An", "Mai", "Nhật", "Tâm", "Thanh", "Thu", "Diệu", "Ánh", "Đinh", "Quỳnh", "Thanh", "Mai", "Bích", "Hiểu", "Song", "Vy", "Hoàng", "Trúc", "Tuệ", "Bạch", "Ái", "Gia", "Thảo", "Thuỷ", "Hương", "Lưu", "Ban", "Nhật", "Yên", "Hải", "Thuỵ", "Thiện", "Thiên", "Bích", "Đông", "Khánh", "Minh", "Thi", "Thảo", "Ánh", "Thuỷ", "Gia", "Thảo", "An", "Vân", "Hoàng", "Linh", "Băng", "Tâm", "Tố", "Tuyết", "Đan", "Giang", "Hà", "Thiên", "Anh", "Diễm", "Nguyên", "Thanh", "Lệ", "Quế", "Diễm", "Vân", "Bảo", "Tuệ", "Bảo", "Nhã", "Mỹ", "Khánh", "Minh", "Châu", "Tường", "Tâm", "Yên", "Tường", "Hải", "Vy", "Hải", "Vân", "Hương", "Nguyệt", "Thanh", "Kim", "Kim", "Kim", "Kim", "Kim", "Diệp", "Diệp", "Diệp", "Diệp", "Diệp", "Nhã", "Nhã", "Nhã", "Nhã", "Nhã", "Thục", "Thục", "Thục", "Thục", "Thục", "Tường", "Tường", "Tường", "Tường", "Tường", "Bảo", "Bảo", "Bảo", "Bảo", "Bảo", "Diễm", "Diễm", "Diễm", "Diễm", "Diễm", "Ái", "Ái", "Ái", "Ái", "Ái", "Trúc", "Trúc", "Trúc", "Trúc", "Trúc", "Lý", "Lý", "Lý", "Lý", "Lý", "Khả", "Khả", "Khả", "Khả", "Khả", "Mai", "Mai", "Mai", "Mai", "Mai", "Tuệ", "Bảo", "Nhã", "Mỹ", "Khánh", "Minh", "Châu", "Tường", "Tâm", "Yên", "Tường", "Hải", "Nhã", "Vy", "Hải", "Vân", "Hương", "Nguyệt", "Thanh", "Kim", "Kim", "Trúc", "Lệ", "Nhật", "Bạch", "Hồng", "Bảo", "Hoàng", "Thanh", "Thi", "Bình", "Thùy", "Nguyệt", "Linh", "Mai", "Trúc", "Phong", "Tuyết", "Vàng", "Hạ", "Thùy", "Tuyết", "Tố", "Yên", "Diễm", "Tú", "Đông", "Ái", "Gia", "Hướng", "Thanh", "Đan", "Tuệ", "Kỳ", "Huyền", "Mẫn", "Hiền", "Bảo", "Tuệ", "Quỳnh", "Minh", "Minh", "Minh", "Minh" };
            return list[new Random().Next(0, list.Count())];
        }

        public static string RanTen()
        {
            var list = new List<string>() { "An", "Anh", "Anh", "Bình", "Châu", "Châu", "Chi", "Chi", "Chi", "Chi", "Cúc", "Dạ", "Diệu", "Diệu", "Du", "Dung", "Duyên", "Dương", "Dương", "Đan", "Đan", "Đào", "Giang", "Giang", "Giang", "Giang", "Giang", "Hà", "Hà", "Hà", "Hà", "Hạ", "Hạ", "Hạ", "Hằng", "Hằng", "Hằng", "Hiền", "Hồng", "Hương", "Hương", "Hương", "Khôi", "Lam", "Lam", "Lam", "lam", "Lan", "Lâm", "Lâm", "Liên", "Linh", "Linh", "Linh", "Linh", "Ly", "Ly", "Mai", "Mai", "Mai", "Miên", "Miên", "Mỹ", "Nga", "Ngân", "Nghi", "Ngôn", "Nguyên", "Nguyệt", "Nguyệt", "Nhi", "Nhi", "Nhiên", "Phi", "Sa", "San", "Tâm", "Tâm", "tâm", "Thanh", "Thanh", "Thanh", "Thanh", "Thảo", "Thảo", "Thảo", "Thảo", "Thu", "Thu", "Thư", "Thường", "Vy", "Anh", "Hân", "Đan", "Anh", "Huyền", "Khuê", "Anh", "Lam", "Đan", "An", "Vy", "Đường", "Anh", "Ngân", "Giang", "Giang", "Anh", "Tú", "Gia", "Hân", "Thiên", "Ý", "Bảo", "An", "Mỹ", "Ánh", "Dương", "Thảo", "Nguyên", "Thảo", "Chi", "Chi", "Mai", "Hạ", "Lam", "Hạ", "Vũ", "Uyên", "Vân", "Anh", "Thư", "Tiên", "Quỳnh", "Mai", "Tú", "Vi", "Bảo", "Quyên", "Quỳnh", "Nhi", "Thùy", "Hân", "Hạnh", "Duyên", "Đoan", "Trang", "Bảo", "Lan", "Thảo", "Chi", "Lan", "Vy", "Trâm", "Anh", "Lam", "Cúc", "Anh", "Thư", "Tâm", "An", "An", "Nhiên", "Minh", "Châu", "Khánh", "Hân", "Lan", "Chi", "Linh", "Chi", "Mai", "Chi", "Minh", "Nguyệt", "Mỹ", "Yến", "Diệp", "Sương", "Tuệ", "Lâm", "Tuyết", "Băng", "Thảo", "Chi", "Chi", "Trinh", "Quế", "Chi", "Quỳnh", "Anh", "Quỳnh", "Chi", "Đoan", "Trang", "Đan", "Quỳnh", "Đan", "Tâm", "Đinh", "Hương", "Bích", "Thảo", "An", "Hạ", "Ánh", "Dương", "Ánh", "Hoa", "Bảo", "Châu", "Diệu", "Huyền", "Bích", "Diễm", "Lan", "Lan", "Hương", "Gia", "Linh", "Anh", "Hân", "Đan", "Anh", "Huyền", "Khuê", "Anh", "Lam", "Đan", "An", "Vy", "Đường", "Anh", "Ngân", "Giang", "Giang", "Anh", "Tú", "Gia", "Hân", "Thiên", "Ý", "Lâm", "Băng", "Lệ", "Liên", "Liên", "Bích", "Xuân", "Xuân", "Xuân", "Yên", "Vân", "Uyển", "Lan", "Lan", "Quỳnh", "Lan", "Lan", "Anh", "Băng", "Anh", "Tâm", "Nga", "Bằng", "Thư", "Linh", "Nghi", "Linh", "Hân", "Dương", "Mẫn", "Thanh", "Lâm", "Diệu", "Anh", "Nhi", "Nhi", "Nhi", "Nhi", "Nhi", "Châu", "Khuê", "Tâm", "Tuệ" };
            return list[new Random().Next(0, list.Count())];
        }

        public static string ChuyenVKeySangEkey(string text)
        {
            string result = text.ToLower();
            result = Regex.Replace(result, "à", "af");
            result = Regex.Replace(result, "á", "as");
            result = Regex.Replace(result, "ạ", "aj");
            result = Regex.Replace(result, "ả", "ar");
            result = Regex.Replace(result, "ã", "ax");
            result = Regex.Replace(result, "â", "aa");
            result = Regex.Replace(result, "ầ", "aaf");
            result = Regex.Replace(result, "ấ", "aas");
            result = Regex.Replace(result, "ậ", "aaj");
            result = Regex.Replace(result, "ẩ", "aar");
            result = Regex.Replace(result, "ẫ", "aax");
            result = Regex.Replace(result, "ă", "aw");
            result = Regex.Replace(result, "ằ", "awf");
            result = Regex.Replace(result, "ắ", "awr");
            result = Regex.Replace(result, "ặ", "awj");
            result = Regex.Replace(result, "ẳ", "aws");
            result = Regex.Replace(result, "ẵ", "awx");
            result = Regex.Replace(result, "è", "ef");
            result = Regex.Replace(result, "é", "es");
            result = Regex.Replace(result, "ẹ", "ej");
            result = Regex.Replace(result, "ẻ", "er");
            result = Regex.Replace(result, "ẽ", "ex");
            result = Regex.Replace(result, "ê", "ee");
            result = Regex.Replace(result, "ề", "eef");
            result = Regex.Replace(result, "ế", "ees");
            result = Regex.Replace(result, "ệ", "eej");
            result = Regex.Replace(result, "ể", "eer");
            result = Regex.Replace(result, "ễ", "eex");
            result = Regex.Replace(result, "ì", "if");
            result = Regex.Replace(result, "í", "is");
            result = Regex.Replace(result, "ị", "ij");
            result = Regex.Replace(result, "ỉ", "ir");
            result = Regex.Replace(result, "ĩ", "ix");
            result = Regex.Replace(result, "ò", "of");
            result = Regex.Replace(result, "ó", "os");
            result = Regex.Replace(result, "ọ", "oj");
            result = Regex.Replace(result, "ỏ", "or");
            result = Regex.Replace(result, "õ", "ox");
            result = Regex.Replace(result, "ô", "oo");
            result = Regex.Replace(result, "ồ", "oof");
            result = Regex.Replace(result, "ố", "oos");
            result = Regex.Replace(result, "ộ", "ooj");
            result = Regex.Replace(result, "ổ", "oor");
            result = Regex.Replace(result, "ỗ", "oox");
            result = Regex.Replace(result, "ơ", "ow");
            result = Regex.Replace(result, "ờ", "owf");
            result = Regex.Replace(result, "ớ", "ows");
            result = Regex.Replace(result, "ợ", "owj");
            result = Regex.Replace(result, "ở", "owr");
            result = Regex.Replace(result, "ỡ", "owx");
            result = Regex.Replace(result, "ù", "uf");
            result = Regex.Replace(result, "ú", "us");
            result = Regex.Replace(result, "ụ", "uj");
            result = Regex.Replace(result, "ủ", "ur");
            result = Regex.Replace(result, "ũ", "ux");
            result = Regex.Replace(result, "ư", "uw");
            result = Regex.Replace(result, "ừ", "uwf");
            result = Regex.Replace(result, "ứ", "uws");
            result = Regex.Replace(result, "ự", "uwj");
            result = Regex.Replace(result, "ử", "uwr");
            result = Regex.Replace(result, "ữ", "uwx");
            result = Regex.Replace(result, "ỳ", "yf");
            result = Regex.Replace(result, "ý", "ys");
            result = Regex.Replace(result, "ỵ", "yj");
            result = Regex.Replace(result, "ỷ", "yr");
            result = Regex.Replace(result, "ỹ", "yx");
            result = Regex.Replace(result, "đ", "dd");
            return result;
        }

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


        public static string GetOneImage()
        {
            var currentPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string[] filePaths = Directory.GetFiles($"{currentPath}image");
            return filePaths[new Random().Next(0, filePaths.Count())];
        }
    }
}
