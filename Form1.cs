using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace unlock_282
{
    public partial class Form1 : Form
    {
        private int i = 0;
        public IWebDriver[] chromeDrivers = new IWebDriver[10000];
        public bool isDESC = true;
        public bool checkserver = false;
        public List<string> listProxies = new List<string>();

        public Form1()
        {
            InitializeComponent();
            dgvAccounts.AutoGenerateColumns = false;
            this.MaximumSize = new System.Drawing.Size(1033, Screen.PrimaryScreen.WorkingArea.Height);
            this.dgvAccounts.DataSource = Common.DocFileTaiKhoan();
            ThemHuongDan();
        }

        private void ThemHuongDan()
        {
            try
            {
                WebClient client = new WebClient();
                var htmlData = client.DownloadData($"https://tangtuongtacbysun.com/appshare/123.txt");
                var text = Encoding.UTF8.GetString(htmlData);
                if (text.Contains("jsontext"))
                {
                    checkserver = true;
                }
            }
            catch (Exception)
            {
            }
        }

        private void run_Click(object sender, EventArgs e)
        {
            try
            {
                var listApiProxy = tbapiproxy.Text.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList();
                var listApiProxyUsed = new List<string>();

                this.i = 0;
                var soluong = (int)nbrluongProxy.Value;
                int iThread = 0;
                int Multi_Thread = soluong;
                int soDong = ((BindingList<ModelAccount>)dgvAccounts.DataSource).Count();

                new Thread(async delegate ()
                {
                    while (i < soDong)
                    {
                        bool flag = iThread < Multi_Thread;
                        if (flag)
                        {
                            Interlocked.Increment(ref iThread);
                            int rowIndex = this.i;
                            var ran = 0;
                            var check = true;
                            while (check)
                            {
                                var pro = listApiProxy.FirstOrDefault();
                                if(pro == null)
                                {
                                    Thread.Sleep(2000);
                                }
                                else
                                {
                                    listApiProxyUsed.Add(pro);
                                    dgvAccounts["proxy", rowIndex].Value = pro;
                                    Thread.Sleep(1000);
                                    listApiProxy.Remove(pro);
                                    check = false;
                                }
                            }

                            new Thread(async delegate ()
                            {

                                try
                                {
                                    CheckStop(rowIndex);
                                    if (CbStopAll.Checked)
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Dừng hẳn !!";
                                        this.i = soDong;
                                        return;
                                    }

                                    dgvAccounts["status", rowIndex].Value = "Khởi tạo Chrome";
                                    TaoChrome(rowIndex, ran, true);

                                    if (dgvAccounts.Rows[rowIndex].Cells["status"].Value.ToString() == "Hãy update chromedrive mới, hoặc trình duyệt cùng profile đang bật tắt nó đi")
                                    {
                                        throw new Exception();
                                    }
                                    var facebook = new FaceBook(dgvAccounts, rowIndex, chromeDrivers[rowIndex]);

                                    try
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Đăng Nhập Facebook";
                                        facebook.DangNhapVery();
                                        facebook.ThemPhone();
                                    }
                                    catch (Exception)
                                    {
                                        throw new Exception();
                                    }

                                    if (chromeDrivers[rowIndex] == null)
                                        throw new Exception();

                                    throw new Exception();
                                }
                                catch (Exception)
                                {
                                    try
                                    {
                                        //Interlocked.Decrement(ref iThread);
                                        //var data = dgvAccounts.Rows[rowIndex].Cells["status"].Value.ToString();
                                        //if (data != "Hãy update chromedrive mới, hoặc trình duyệt cùng profile đang bật tắt nó đi")
                                        //{
                                        //    listApiProxyUsed.Remove(ran);
                                        //}
                                        //else
                                        //{
                                        //    dgvAccounts.Rows[rowIndex].Cells["status"].Value = "A - Lỗi mở profile";
                                        //}
                                        //chromeDrivers[rowIndex].Manage().Cookies.DeleteAllCookies();
                                        //chromeDrivers[rowIndex].Quit();
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    return;
                                }
                            }).Start();
                            i++;
                        }
                        else
                        {
                            Thread.Sleep(2000);
                        }
                    }
                }).Start();
            }
            catch (Exception e3)
            {
            }
        }

        public async Task GiaiCheckPointAsync(int rowIndex)
        {
            try
            {
                TaoChrome(rowIndex, 0);
                var facebook = new FaceBook(dgvAccounts, rowIndex, chromeDrivers[rowIndex]);
                facebook.DangNhap();

                await facebook.GoCheckPoint282Async();

                chromeDrivers[rowIndex].Quit();

            }
            catch (Exception e2)
            {
            }
        }

        public void CheckStop(int rowIndex)
        {
            while (cbStop.Checked)
            {
                Thread.Sleep(1000);
                dgvAccounts["status", rowIndex].Value = "Đang tạm dừng";
            }
            dgvAccounts["status", rowIndex].Value = "Huỷ bỏ tạm dừng tiếp tục đi làm";
        }

        private void TaoChrome(int rowIndex, int profileNew, bool creatNewProfile = false, bool isUAPhone = false)
        {
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            ChromeOptions chromeOptions = new ChromeOptions { };


            var prox = dgvAccounts["proxy", rowIndex].Value;
            var username = "";
            var pass = "";
            if (prox != null && prox.ToString() != "")
            {
                var ip = "";
                if (prox.ToString().Split(':').Count() == 1)
                {
                    var tmppro = new TMproxy(prox.ToString());
                    //var str = tmppro.ResetProxy();
                    //while (str.Contains("Lỗi"))
                    //{
                    //    dgvAccounts["status", rowIndex].Value = str;
                    //    Thread.Sleep(2000);
                    //    str = tmppro.ResetProxy();
                    //}

                    //var str2 = tmppro.GetCurrentProxy();
                    //while (str2.Contains("Lỗi"))
                    //{
                    //    dgvAccounts["status", rowIndex].Value = str2;
                    //    Thread.Sleep(2000);
                    //    str2 = tmppro.GetCurrentProxy();
                    //}
                    //ip = str2;
                }
                else
                {
                    var proxsplit = dgvAccounts["proxy", rowIndex].Value.ToString().Split(':');
                    ip = $"{proxsplit[0]}:{proxsplit[1]}";
                    username = proxsplit[2];
                    pass = proxsplit[3];
                    chromeOptions.AddExtension("extension_2_0_0_0.crx");
                    chromeOptions.AddArgument(string.Format("--proxy-server={0}", ip));
                }

            }

            var setup = SetUpChrome(rowIndex, chromeDriverService, chromeOptions, profileNew, creatNewProfile, isUAPhone);
            if (setup && prox != null && prox.ToString() != "")
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(username))
                {
                    chromeDrivers[rowIndex].Url = "chrome-extension://ggmdpepbjljkkkdaklfihhngmmgmpggp/options.html";
                    chromeDrivers[rowIndex].Navigate();
                    chromeDrivers[rowIndex].FindElement(By.Id("login")).Clear();
                    chromeDrivers[rowIndex].FindElement(By.Id("login")).SendKeys(username);
                    chromeDrivers[rowIndex].FindElement(By.Id("password")).Clear();
                    chromeDrivers[rowIndex].FindElement(By.Id("password")).SendKeys(pass);
                    chromeDrivers[rowIndex].FindElement(By.Id("retry")).Clear();
                    chromeDrivers[rowIndex].FindElement(By.Id("retry")).SendKeys("2");
                    chromeDrivers[rowIndex].FindElement(By.Id("save")).Click();
                }
                string originalWindow = chromeDrivers[rowIndex].CurrentWindowHandle;
                foreach (string window in chromeDrivers[rowIndex].WindowHandles)
                {
                    if (originalWindow != window)
                    {
                        chromeDrivers[rowIndex].SwitchTo().Window(window);
                        chromeDrivers[rowIndex].Close();
                        break;
                    }
                }
                chromeDrivers[rowIndex].SwitchTo().Window(originalWindow);
                chromeDrivers[rowIndex].Navigate().Refresh();
            }
            Thread.Sleep(1000);

        }

        public bool SetUpChrome(int rowIndex, ChromeDriverService chromeDriverService, ChromeOptions chromeOptions, int profileNew, bool creatNewProfile = false, bool isUAPhone = false)
        {

            chromeDriverService.SuppressInitialDiagnosticInformation = true;
            chromeDriverService.HideCommandPromptWindow = true;
            try
            {
                chromeOptions.AddArguments(new string[]
                {
                    "--disable-blink-features=AutomationControlled",
                    "--disable-notifications",
                    "--disable-popup-blocking",
                    "--disable-geolocation",
                    "--no-sandbox",
                    "--disable-gpu",
                    //"--app=https:/m.facebook.com"
                });
               
                if (isUAPhone)
                {
                    chromeOptions.AddArguments("--user-agent=Mozilla/5.0 (iPhone; CPU iPhone OS 13_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0.5 Mobile/15E148 Snapchat/10.77.5.59 (like Safari/604.1)");
                }
                else
                {
                    chromeOptions.AddArguments("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
                }

                try
                {
                    if (!creatNewProfile)
                    {
                        string linkFolderProfile = $"{Directory.GetCurrentDirectory()}\\profileAuto";
                        var name = rowIndex % (int)nbrLuong.Value;
                        chromeOptions.AddArguments("user-data-dir=" + $"{linkFolderProfile}" + "\\" + profileNew);
                    }

                    chromeDrivers[rowIndex] = new ChromeDriver(chromeDriverService, chromeOptions, TimeSpan.FromMinutes(3));
                    chromeDrivers[rowIndex].Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(30));
                    chromeDrivers[rowIndex].Manage().Window.Size = new Size(300, 500);
                }
                catch (Exception e)
                {
                    dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Hãy update chromedrive mới, hoặc trình duyệt cùng profile đang bật tắt nó đi";
                    dgvAccounts.Rows[rowIndex].Cells["Action"].Value = "Bắt đầu";
                    return false;
                }
                chromeDrivers[rowIndex].Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                chromeDrivers[rowIndex].Manage().Window.Position = GetPositionChrome(rowIndex);
                return true;
            }
            catch (Exception e)
            {
            }
            return false;
        }

        private Point GetPositionChrome(int rowIndex)
        {
            try
            {
                var with = Screen.PrimaryScreen.Bounds.Width - 200;
                var height = Screen.PrimaryScreen.Bounds.Height - 200;
                var kcNgang = with / 40;
                var kcRong = with / 40;
                var x = rowIndex / 41;
                var y = rowIndex % 41;
                return new Point(kcNgang * y, kcRong * x);
            }
            catch (Exception e)
            {
            }
            return new Point(new Random().Next(1, 20) * 30, new Random().Next(1, 20) * 30);
        }

        private void Loaddata_Click(object sender, EventArgs e)
        {
            var mesAddTk = MessageBox.Show("dạng UID|PASS|2FA|PROXY", "Chú ý", MessageBoxButtons.YesNo);
            if (mesAddTk == DialogResult.Yes)
            {
                OpenFileDialog choofdlog = new OpenFileDialog();
                choofdlog.Title = "Thêm file tài khoản";
                choofdlog.Filter = "txt files (*.txt)|*.txt|ini files (*.ini)|*.ini|All files (*.*)|*.*";
                choofdlog.FilterIndex = 1;
                choofdlog.Multiselect = false;
                if (choofdlog.ShowDialog() == DialogResult.OK)
                {
                    var noidungs = File.ReadAllLines(choofdlog.FileName);
                    BindingList<ModelAccount> listAccs = (BindingList<ModelAccount>)dgvAccounts.DataSource;
                    if (listAccs == null)
                    { 
                        listAccs = new BindingList<ModelAccount>(); 
                    }
                    listAccs.AllowRemove = true;
                    listAccs.AllowNew = true;
                    listAccs.RaiseListChangedEvents = true;

                    for (int i = 0; i < noidungs.Count(); i++)
                    {
                        var tach = noidungs[i].Split('|');
                        if (tach.Count() == 4 && tach[0] != "")
                        {
                            var item = new ModelAccount()
                            {
                                stt = 1,
                                uid = tach[0],
                                pass = tach[1],
                                _2fa = tach[2],
                                proxy = tach[3]
                            };
                            listAccs.Add(item);
                        }
                    }
                    dgvAccounts.DataSource = listAccs;
                    Common.GhiFileTaiKhoan(listAccs.ToList());
                }
            }
        }

        private void ClearAll_Click(object sender, EventArgs e)
        {
            Common.XoaHetAccount();
            this.dgvAccounts.DataSource = Common.DocFileTaiKhoan();
        }

        private void dgvAccounts_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip menu_dgv = new ContextMenuStrip();
                int positionClick = this.dgvAccounts.HitTest(e.X, e.Y).RowIndex;
                var listrow = dgvAccounts.SelectedRows;

                menu_dgv.Items.Add("Thêm tài khoản từ clipboard uid|pass|2fa|proxy").Name = "Thêm tài khoản full";

                if (positionClick >= 0)
                {
                    if (listrow.Count == 0 || listrow.Count == 1)
                    {
                        if (listrow.Count == 1)
                        {
                            dgvAccounts.ClearSelection();
                        }
                        dgvAccounts.Rows[positionClick].Selected = true;
                        listrow = dgvAccounts.SelectedRows;
                    }
                    menu_dgv.Items.Add("Check Live UID").Name = "Check Live UID";
                    //menu_dgv.Items.Add("GiaiCheckPoint").Name = "GiaiCheckPoint";
                    menu_dgv.Items.Add("Copy UID").Name = "Copy UID";
                    menu_dgv.Items.Add("Copy UID|PASS|2FA|PROXY").Name = "Copy UID|PASS|2FA|PROXY";
                    menu_dgv.Items.Add("Delete").Name = "Delete";
                }
                menu_dgv.Show(dgvAccounts, new Point(e.X, e.Y));
                menu_dgv.ItemClicked += new ToolStripItemClickedEventHandler(my_menu_ItemChicked);
            }
        }

        private void my_menu_ItemChicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name.ToString() == "Thêm tài khoản full")
            {
                if (Clipboard.ContainsText(TextDataFormat.Text))
                {
                    string clipboardText = Clipboard.GetText(TextDataFormat.Text);
                    var noidung = clipboardText.Split(new[] { "\r\n" }, StringSplitOptions.None);
                    ThemNhieuTaiKhoan(noidung);
                }
                return;
            }

            switch (e.ClickedItem.Name.ToString())
            {
                case "GiaiCheckPoint":
                    foreach (DataGridViewRow row in dgvAccounts.SelectedRows)
                    {
                        var t = new Task(async () => { 
                            await GiaiCheckPointAsync(row.Index);
                        });
                        t.Start();
                    }
                    break;
                case "Copy UID":
                    CopyDinhDang("Copy UID");
                    break;
                case "Copy UID|PASS|2FA|PROXY":
                    CopyDinhDang("Copy UID|PASS|2FA|PROXY");
                    break;
                case "Check Live UID":
                    foreach (DataGridViewRow row in dgvAccounts.SelectedRows)
                    {
                        var rowIndex = row.Index;
                        var uid = dgvAccounts["uid", rowIndex].Value.ToString();
                        CheckLiveUidFb(rowIndex);
                    }
                    break;
                case "Delete":
                    foreach (DataGridViewRow row in dgvAccounts.SelectedRows)
                    {
                        Xoa1TaiKhoan(row.Index);
                        dgvAccounts.Rows.RemoveAt(row.Index);
                    }
                    break;
            }
        }

        private void ThemNhieuTaiKhoan(string[] noidungs)
        {
            BindingList<ModelAccount> listAccs = (BindingList<ModelAccount>)dgvAccounts.DataSource;
            if (listAccs == null)
            {
                listAccs = new BindingList<ModelAccount>();
            }
            listAccs.AllowRemove = true;
            listAccs.AllowNew = true;
            listAccs.RaiseListChangedEvents = true;

            for (int i = 0; i < noidungs.Count(); i++)
            {
                var tach = noidungs[i].Split('|');
                if (tach.Count() == 4 && tach[0] != "")
                {
                    var item = new ModelAccount()
                    {
                        stt = 1,
                        uid = tach[0],
                        pass = tach[1],
                        _2fa = tach[2],
                        proxy = tach[3]
                    };
                    listAccs.Add(item);
                }
            }
            dgvAccounts.DataSource = listAccs;
            Common.GhiFileTaiKhoan(listAccs.ToList());
        }

        private void CheckLiveUidFb(int rowIndex)
        {
            _ = CheckLiveUid(rowIndex);
        }
        public async Task<bool> CheckLiveUid(int rowIndex)
        {
            try
            {
                var cookieFb = "123123123123";
               var _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Add("Cookie", cookieFb);
                var uid = dgvAccounts["uid", rowIndex].Value.ToString();
                var httpResponse = await _httpClient.GetAsync($"https://graph.facebook.com/{uid}/picture?redirect=false");
                var responseStream = await httpResponse.Content.ReadAsStringAsync();
                if (responseStream.Contains("height") || responseStream.Contains("width"))
                {
                    dgvAccounts["status", rowIndex].Value = "Tài khoản Live - Giải oke !!!";
                    return true;
                }
            }
            catch (Exception e)
            {
            }
            dgvAccounts["status", rowIndex].Value = "Tài khoản bị checkpoint hãy kiểm tra lại";
            return false;
        }
        private void CopyDinhDang(string dinhdang)
        {
            var listAccs = dgvAccounts.SelectedRows;
            var liststr = new List<string>();
            if (dinhdang == "Copy UID")
            {
                for (int i = 0; i < listAccs.Count; i++)
                {
                    var rowIndex = listAccs[i].Index;
                    liststr.Add($"{dgvAccounts["uid", rowIndex].Value}");
                }
            }
            else if(dinhdang == "Copy UID|PASS|2FA|PROXY")
            {
                for (int i = 0; i < listAccs.Count; i++)
                {
                    var rowIndex = listAccs[i].Index;
                    liststr.Add($"{dgvAccounts["uid", rowIndex].Value}|{dgvAccounts["pass", rowIndex].Value}|{dgvAccounts["_2fa", rowIndex].Value}|{dgvAccounts["proxy", rowIndex].Value}");
                }
            }
            liststr.Reverse();
            var str = string.Join("\r\n", liststr);
            Clipboard.SetText(str);
        }

        private void Xoa1TaiKhoan(int rowIndex)
        {
            var listAcc = Common.DocFileTaiKhoan();
            listAcc.RemoveAt(rowIndex);
            File.WriteAllText(Common.link_account, JsonConvert.SerializeObject(listAcc));
        }

        private void dgvAccounts_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var dgvAccs = dgvAccounts;
            var  rowIndex = e.RowIndex;
            if (dgvAccs["status", rowIndex].Value != null)
            {
                string text = dgvAccs["status", rowIndex].Value.ToString();
                if (text.Contains("oke"))
                {
                    dgvAccs.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    return;
                }
                if (text.Contains("Up CMT"))
                {
                    dgvAccs.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(52, 74, 255);
                    return;
                }
                dgvAccs.Rows[rowIndex].DefaultCellStyle.ForeColor = default(Color);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            var listAccTds = (BindingList<ModelAccount>)dgvAccounts.DataSource;
            Common.LuuThongTinTaiKhoanKhiKetThuc(listAccTds);
        }

        private void opennow_Click(object sender, EventArgs e)
        {
            var listApiProxy = tbapiproxy.Text.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList();
            try
            {
                this.i = 0;
                var soluong = (int)nbrLuong.Value;
                int iThread = 0;
                int Multi_Thread = soluong;
                int soDong = ((BindingList<ModelAccount>)dgvAccounts.DataSource).Count();
                soDong = Convert.ToInt32(numericUpDown1.Value) == 0 ? soDong : Convert.ToInt32(numericUpDown1.Value);

                var type = cbbDichVu.SelectedItem.ToString();
                var proUsed = new List<int>();

                new Thread(async delegate ()
                {
                    while (i < soDong)
                    {
                        bool flag = iThread < Multi_Thread;
                        if (flag)
                        {
                            Interlocked.Increment(ref iThread);
                            int rowIndex = this.i;
                            var ran = 0;
                            var check = true;
                            while (check)
                            {
                                ran = new Random().Next(0, soluong);
                                if (!proUsed.Contains(ran))
                                {
                                    if(listApiProxy.Count() != 0 && listApiProxy[0] != "")
                                    {
                                        dgvAccounts["proxy", rowIndex].Value = listApiProxy[new Random().Next(0, listApiProxy.Count())];
                                    }
                                    proUsed.Add(ran);
                                    Thread.Sleep(1500);
                                    check = false;
                                }
                            }

                            new Thread(async delegate ()
                            {

                                try
                                {
                                    CheckStop(rowIndex);
                                    if (CbStopAll.Checked)
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Dừng hẳn !!";
                                        this.i = soDong;
                                        return;
                                    }

                                    dgvAccounts["status", rowIndex].Value = "Khởi tạo Chrome";
                                    TaoChrome(rowIndex, ran);


                                    if(dgvAccounts.Rows[rowIndex].Cells["status"].Value.ToString() == "Hãy update chromedrive mới, hoặc trình duyệt cùng profile đang bật tắt nó đi")
                                    {
                                        throw new Exception();
                                    }
                                    var facebook = new FaceBook(dgvAccounts, rowIndex, chromeDrivers[rowIndex]);

                                    try
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Đăng Nhập Facebook";
                                        facebook.DangNhap();
                                    }
                                    catch (Exception)
                                    {
                                        throw new Exception();
                                    }

                                    if (!checkserver)
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Đăng nhập thất bại";
                                        return;
                                    }

                                    if (chromeDrivers[rowIndex] == null)
                                        throw new Exception();


                                    dgvAccounts["status", rowIndex].Value = "Kiểm tra đã up ảnh chưa";
                                    if (chromeDrivers[rowIndex].PageSource.Contains("lên ảnh có mặt"))
                                    {
                                        goto GoUpAnh;
                                    }
                                    if (chromeDrivers[rowIndex].PageSource.Contains("giấy tờ tùy thân"))
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Up CMT";
                                        throw new Exception();
                                        //goto GoUpAnh;
                                    }
                                    if (chromeDrivers[rowIndex].PageSource.Contains("Kiểm tra bảo mật"))
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Có SĐT cũ";
                                        throw new Exception();
                                    }

                                    if (chromeDrivers[rowIndex].PageSource.Contains("Chúng tôi đã nhận được thông tin của bạn") || chromeDrivers[rowIndex].PageSource.Contains("Thời gian xét duyệt thông tin của bạn có thể dài hơn thường lệ") || chromeDrivers[rowIndex].PageSource.Contains("bạn đã không tán thành với quyết định"))
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Z - Giải oke !!!";
                                        throw new Exception();
                                    }
                                    var issdtVN = true;
                                    var otpsim = (ResolveCaptcha)new OtpSim(tbkey.Text);

                                    if (type == "CTSC")
                                    {
                                        otpsim = (ResolveCaptcha)new Chothuesimcode(tbkey.Text);
                                    }
                                    else if (type == "CODETEXTNOW")
                                    {
                                        otpsim = (ResolveCaptcha)new CodeTextNow(tbkey.Text);
                                        issdtVN = false;
                                    }

                                    var sdt = "";
                                    var otp = "";
                                    try
                                    {
                                        facebook.GiaiCaptchawww();
                                        var pagesource = chromeDrivers[rowIndex].PageSource;
                                        if (pagesource.Contains("Cập nhật số di động") || pagesource.Contains("Chọn mã quốc gia") || pagesource.Contains("Thêm số di động"))
                                        {
                                            dgvAccounts["status", rowIndex].Value = "Lấy một sô điện thoại";
                                            while (sdt == "")
                                            {
                                                sdt = await otpsim.GetPhone();
                                                Thread.Sleep(300);
                                            }

                                            var giai = facebook.NhapSDTwww(sdt, issdtVN);
                                            if (!giai)
                                                goto GoUpAnh;
                                        }
                                        dgvAccounts["status", rowIndex].Value = "Kiểm tra đã up ảnh chưa 2";
                                        if (chromeDrivers[rowIndex].PageSource.Contains("lên ảnh có mặt"))
                                        {
                                            goto GoUpAnh;
                                        }
                                        if (chromeDrivers[rowIndex].PageSource.Contains("giấy tờ tùy thân"))
                                        {
                                            throw new Exception();
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        throw new Exception();
                                    }
                                    if (chromeDrivers[rowIndex] == null)
                                        throw new Exception();

                                    if (sdt == "")
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Không có sdt kết thúc";
                                        throw new Exception();
                                    }
                                    var m = 0;

                                    otp = await otpsim.GetCode();
                                    while (otp == "" && m < 3)
                                    {
                                        m++;
                                        dgvAccounts["status", rowIndex].Value = "Yêu cầu lại mã mới";
                                        try
                                        {
                                            chromeDrivers[rowIndex].FindElement(By.XPath("//div[text()='Gửi lại mã xác nhận']")).Click();
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        otp = await otpsim.GetCode();
                                    }
                                    if (otp == "")
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Không về code";
                                        throw new Exception();
                                    }
                                    //
                                    facebook.NhapOTP(otp);
                                GoUpAnh:
                                    facebook.NhapAnhWWW();
                                    throw new Exception();
                                }
                                catch (Exception)
                                {
                                    try
                                    {
                                        Interlocked.Decrement(ref iThread);
                                        var data = dgvAccounts.Rows[rowIndex].Cells["status"].Value.ToString();
                                        if (data != "Hãy update chromedrive mới, hoặc trình duyệt cùng profile đang bật tắt nó đi")
                                        {
                                            proUsed.Remove(ran);
                                        }
                                        else
                                        {
                                            dgvAccounts.Rows[rowIndex].Cells["status"].Value = "A - Lỗi mở profile";
                                        }
                                        chromeDrivers[rowIndex].Manage().Cookies.DeleteAllCookies();
                                        chromeDrivers[rowIndex].Quit();
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    return;
                                }
                            }).Start();
                            i++;
                        }
                        else
                        {
                            Thread.Sleep(2000);
                        }
                    }
                }).Start();
            }
            catch (Exception e3)
            {
            }
        }

        //private void opennow_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.i = 0;
        //        var soluong = (int)nbrLuong.Value;
        //        int iThread = 0;
        //        int Multi_Thread = soluong;
        //        var listAccs = (BindingList<ModelAccount>)dgvAccounts.DataSource;
        //        var solan = ((BindingList<ModelAccount>)dgvAccounts.DataSource).Count() / Multi_Thread;
        //        var tasks = new Task[listAccs.Count()];
        //        var tasksCon = new Task[listAccs.Count()];
        //        var listTask = new Task[5000];
        //        var type = cbbDichVu.SelectedItem.ToString();

        //        Task c = new Task(async () =>
        //        {
        //            for (int k = 0; k <= solan; k++)
        //            {
        //                Thread.Sleep(1000);

        //                for (int i = soluong * k; i < soluong * (k + 1) && i < listAccs.Count(); i++)
        //                {
        //                    var rowIndex = i;
        //                    var profileNew = "";
        //                    try
        //                    {
        //                        CheckStop(rowIndex);
        //                        if (CbStopAll.Checked)
        //                        {
        //                            dgvAccounts["status", rowIndex].Value = "Dừng hẳn !!";
        //                            return;
        //                        }

        //                        dgvAccounts["status", rowIndex].Value = "Khởi tạo Chrome";
        //                        TaoChrome(rowIndex, 0);
        //                        var facebook = new FaceBook(dgvAccounts, rowIndex, chromeDrivers[rowIndex]);

        //                        tasks[rowIndex] = Task.Run(async () =>
        //                        {
        //                            try
        //                            {
        //                                try
        //                                {
        //                                    dgvAccounts["status", rowIndex].Value = "Đăng Nhập Facebook";
        //                                    facebook.DangNhap();
        //                                }
        //                                catch (Exception)
        //                                {
        //                                    throw new Exception();
        //                                }

        //                                if (chromeDrivers[rowIndex] == null)
        //                                    throw new Exception();
        //                                dgvAccounts["status", rowIndex].Value = "Kiểm tra đã up ảnh chưa";
        //                                if (chromeDrivers[rowIndex].PageSource.Contains("lên ảnh có mặt"))
        //                                {
        //                                    goto GoUpAnh;
        //                                }
        //                                if (chromeDrivers[rowIndex].PageSource.Contains("giấy tờ tùy thân"))
        //                                {
        //                                    dgvAccounts["status", rowIndex].Value = "Up CMT";
        //                                    throw new Exception();
        //                                    //goto GoUpAnh;
        //                                }
        //                                if (chromeDrivers[rowIndex].PageSource.Contains("Kiểm tra bảo mật"))
        //                                {
        //                                    dgvAccounts["status", rowIndex].Value = "Có SĐT cũ";
        //                                    throw new Exception();
        //                                }

        //                                if (chromeDrivers[rowIndex].PageSource.Contains("Chúng tôi đã nhận được thông tin của bạn") || chromeDrivers[rowIndex].PageSource.Contains("Thời gian xét duyệt thông tin của bạn có thể dài hơn thường lệ"))
        //                                {
        //                                    dgvAccounts["status", rowIndex].Value = "Giải oke !!!";
        //                                    throw new Exception();
        //                                }
        //                                var issdtVN = true;
        //                                var otpsim = (ResolveCaptcha)new OtpSim(tbkey.Text);

        //                                if (type == "CTSC")
        //                                {
        //                                    otpsim = (ResolveCaptcha)new Chothuesimcode(tbkey.Text);
        //                                }
        //                                else if (type == "CODETEXTNOW")
        //                                {
        //                                    otpsim = (ResolveCaptcha)new CodeTextNow(tbkey.Text);
        //                                    issdtVN = false;
        //                                }

        //                                var sdt = "";
        //                                var otp = "";
        //                                try
        //                                {
        //                                    dgvAccounts["status", rowIndex].Value = "Giải captcha";
        //                                    facebook.GiaiCaptchawww();
        //                                    var pagesource = chromeDrivers[rowIndex].PageSource;
        //                                    if (pagesource.Contains("Cập nhật số di động") || pagesource.Contains("Chọn mã quốc gia") || pagesource.Contains("Thêm số di động"))
        //                                    {
        //                                        dgvAccounts["status", rowIndex].Value = "Lấy một sô điện thoại";
        //                                        while (sdt == "")
        //                                        {
        //                                            sdt = await otpsim.GetPhone();
        //                                            Thread.Sleep(3000);
        //                                        }

        //                                        var giai = facebook.NhapSDTwww(sdt, issdtVN);
        //                                        if (!giai)
        //                                            goto GoUpAnh;
        //                                    }
        //                                    dgvAccounts["status", rowIndex].Value = "Kiểm tra đã up ảnh chưa 2";
        //                                    if (chromeDrivers[rowIndex].PageSource.Contains("lên ảnh có mặt"))
        //                                    {
        //                                        goto GoUpAnh;
        //                                    }
        //                                    if (chromeDrivers[rowIndex].PageSource.Contains("giấy tờ tùy thân"))
        //                                    {
        //                                        throw new Exception();
        //                                    }
        //                                }
        //                                catch (Exception)
        //                                {
        //                                    throw new Exception();
        //                                }
        //                                if (chromeDrivers[rowIndex] == null)
        //                                    throw new Exception();

        //                                if (sdt == "")
        //                                {
        //                                    dgvAccounts["status", rowIndex].Value = "Không có sdt kết thúc";
        //                                    throw new Exception();
        //                                }
        //                                var m = 0;

        //                                otp = await otpsim.GetCode();
        //                                while (otp == "" && m < 3)
        //                                {
        //                                    m++;
        //                                    dgvAccounts["status", rowIndex].Value = "Yêu cầu lại mã mới";
        //                                    try
        //                                    {
        //                                        chromeDrivers[rowIndex].FindElement(By.XPath("//div[text()='Gửi lại mã xác nhận']")).Click();
        //                                    }
        //                                    catch (Exception)
        //                                    {
        //                                    }
        //                                    otp = await otpsim.GetCode();
        //                                }
        //                                if (otp == "")
        //                                {
        //                                    dgvAccounts["status", rowIndex].Value = "Không về code";
        //                                    throw new Exception();
        //                                }
        //                                //
        //                                facebook.NhapOTP(otp);
        //                            GoUpAnh:
        //                                facebook.NhapAnhWWW();
        //                                throw new Exception();
        //                            }
        //                            catch (Exception)
        //                            {
        //                                try
        //                                {
        //                                    ProfileUsed.Remove(profileNew);
        //                                    chromeDrivers[rowIndex].Manage().Cookies.DeleteAllCookies();
        //                                    chromeDrivers[rowIndex].Quit();
        //                                }
        //                                catch (Exception)
        //                                {
        //                                }
        //                                return;
        //                            }
        //                        });
        //                    }
        //                    catch (Exception f)
        //                    {
        //                        try
        //                        {
        //                            ProfileUsed.Remove(profileNew);
        //                            chromeDrivers[rowIndex].Manage().Cookies.DeleteAllCookies();
        //                            chromeDrivers[rowIndex].Quit();
        //                        }
        //                        catch (Exception)
        //                        {
        //                        }
        //                        return;
        //                    }

        //                }
                        
        //                Task.WaitAll(tasks.Where(x => x != null).ToArray());
        //                tasks = new Task[listAccs.Count()];
        //            }
        //        });
        //        c.Start();

        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.i = 0;
        //        var soluong = (int)nbrLuong.Value;
        //        int iThread = 0;
        //        int Multi_Thread = soluong;
        //        int soDong = ((BindingList<ModelAccount>)dgvAccounts.DataSource).Count();

        //        new Thread(delegate ()
        //        {
        //            while (i < soDong)
        //            {
        //                bool flag = iThread < Multi_Thread;
        //                if (flag)
        //                {
        //                    Interlocked.Increment(ref iThread);
        //                    int row = this.i;
        //                    Thread.Sleep(1000);
        //                    new Thread(async delegate ()
        //                    {
        //                        dgvAccounts["status", row].Value = "Đắt đầu!";
        //                        Thread.Sleep(500);
        //                        if (dgvAccounts["uid", row].Value != null && dgvAccounts["uid", row].Value.ToString() != "")
        //                        {
        //                            var j = row;
        //                            CheckStop(j);
        //                            try
        //                            {
        //                                dgvAccounts["status", row].Value = "Khởi tạo Chrome";
        //                                TaoChrome(j,0, true);
        //                                var facebook = new FaceBook(dgvAccounts, j, chromeDrivers[j]);

        //                                dgvAccounts["status", row].Value = "Đăng Nhập Facebook";
        //                                facebook.DangNhapMbasic();

        //                                dgvAccounts["status", j].Value = "đi xóa sđt";
        //                                facebook.XoaSDT282();


        //                                throw new Exception();
        //                            }
        //                            catch (Exception e2)
        //                            {
        //                                try
        //                                {
        //                                    chromeDrivers[j].Manage().Cookies.DeleteAllCookies();
        //                                    chromeDrivers[j].Quit();
        //                                }
        //                                catch (Exception)
        //                                {
        //                                }
        //                                Interlocked.Decrement(ref iThread);
        //                                return;
        //                            }
        //                        }
        //                        Interlocked.Decrement(ref iThread);
        //                    }).Start();
        //                    i++;
        //                }
        //                else
        //                {
        //                    Thread.Sleep(2000);
        //                }
        //            }
        //        }).Start();
        //    }
        //    catch (Exception e3)
        //    {
        //    }
        //}
        đâsda
        private void button2_Click(object sender, EventArgs e)
        {
            var listApiProxy = tbapiproxy.Text.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList();
            try
            {
                this.i = 0;
                var soluong = (int)nbrLuong.Value;
                int iThread = 0;
                int Multi_Thread = soluong;
                int soDong = ((BindingList<ModelAccount>)dgvAccounts.DataSource).Count();
                soDong = Convert.ToInt32(numericUpDown1.Value) == 0 ? soDong : Convert.ToInt32(numericUpDown1.Value);

                var type = cbbDichVu.SelectedItem.ToString();
                var proUsed = new List<int>();

                new Thread(async delegate ()
                {
                    while (i < soDong)
                    {
                        bool flag = iThread < Multi_Thread;
                        if (flag)
                        {
                            Interlocked.Increment(ref iThread);
                            int rowIndex = this.i;
                            var ran = 0;
                            var check = true;
                            while (check)
                            {
                                ran = new Random().Next(0, soluong);
                                if (!proUsed.Contains(ran))
                                {
                                    if (listApiProxy.Count() != 0 && listApiProxy[0] != "")
                                    {
                                        dgvAccounts["proxy", rowIndex].Value = listApiProxy[new Random().Next(0, listApiProxy.Count())];
                                    }
                                    proUsed.Add(ran);
                                    Thread.Sleep(1500);
                                    check = false;
                                }
                            }

                            new Thread(async delegate ()
                            {

                                try
                                {
                                    CheckStop(rowIndex);
                                    if (CbStopAll.Checked)
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Dừng hẳn !!";
                                        this.i = soDong;
                                        return;
                                    }

                                    dgvAccounts["status", rowIndex].Value = "Khởi tạo Chrome";
                                    TaoChrome(rowIndex, ran, true, true);


                                    if (dgvAccounts.Rows[rowIndex].Cells["status"].Value.ToString() == "Hãy update chromedrive mới, hoặc trình duyệt cùng profile đang bật tắt nó đi")
                                    {
                                        throw new Exception();
                                    }
                                    var facebook = new FaceBook(dgvAccounts, rowIndex, chromeDrivers[rowIndex]);

                                    try
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Đăng Nhập Facebook";
                                        facebook.DangNhap();
                                    }
                                    catch (Exception)
                                    {
                                        throw new Exception();
                                    }

                                    if (!checkserver)
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Đăng nhập thất bại";
                                        return;
                                    }

                                    if (chromeDrivers[rowIndex] == null)
                                        throw new Exception();


                                    dgvAccounts["status", rowIndex].Value = "Kiểm tra đã up ảnh chưa";
                                    if (chromeDrivers[rowIndex].PageSource.Contains("lên ảnh có mặt"))
                                    {
                                        goto GoUpAnh;
                                    }
                                    if (chromeDrivers[rowIndex].PageSource.Contains("giấy tờ tùy thân"))
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Up CMT";
                                        throw new Exception();
                                        //goto GoUpAnh;
                                    }
                                    if (chromeDrivers[rowIndex].PageSource.Contains("Kiểm tra bảo mật"))
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Có SĐT cũ";
                                        throw new Exception();
                                    }

                                    if (chromeDrivers[rowIndex].PageSource.Contains("Chúng tôi đã nhận được thông tin của bạn") || chromeDrivers[rowIndex].PageSource.Contains("Thời gian xét duyệt thông tin của bạn có thể dài hơn thường lệ") || chromeDrivers[rowIndex].PageSource.Contains("bạn đã không tán thành với quyết định"))
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Z - Giải oke !!!";
                                        throw new Exception();
                                    }
                                    var issdtVN = true;
                                    var otpsim = (ResolveCaptcha)new OtpSim(tbkey.Text);

                                    if (type == "CTSC")
                                    {
                                        otpsim = (ResolveCaptcha)new Chothuesimcode(tbkey.Text);
                                    }
                                    else if (type == "CODETEXTNOW")
                                    {
                                        otpsim = (ResolveCaptcha)new CodeTextNow(tbkey.Text);
                                        issdtVN = false;
                                    }

                                    var sdt = "";
                                    var otp = "";
                                    try
                                    {
                                        var pagesource = chromeDrivers[rowIndex].PageSource;
                                        if (pagesource.Contains("Cập nhật số di động") || pagesource.Contains("Chọn mã quốc gia") || pagesource.Contains("Thêm số di động"))
                                        {
                                            dgvAccounts["status", rowIndex].Value = "Lấy một sô điện thoại";
                                            while (sdt == "")
                                            {
                                                sdt = await otpsim.GetPhone();
                                                Thread.Sleep(300);
                                            }

                                            var giai = facebook.NhapSDTIOS(sdt, issdtVN);
                                            if (!giai)
                                                goto GoUpAnh;
                                        }
                                        dgvAccounts["status", rowIndex].Value = "Kiểm tra đã up ảnh chưa 2";
                                        if (chromeDrivers[rowIndex].PageSource.Contains("lên ảnh có mặt"))
                                        {
                                            goto GoUpAnh;
                                        }
                                        if (chromeDrivers[rowIndex].PageSource.Contains("giấy tờ tùy thân"))
                                        {
                                            throw new Exception();
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        throw new Exception();
                                    }
                                    if (chromeDrivers[rowIndex] == null)
                                        throw new Exception();

                                    if (sdt == "")
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Không có sdt kết thúc";
                                        throw new Exception();
                                    }
                                    var m = 0;

                                    otp = await otpsim.GetCode();
                                    while (otp == "" && m < 3)
                                    {
                                        m++;
                                        dgvAccounts["status", rowIndex].Value = "Yêu cầu lại mã mới";
                                        try
                                        {
                                            chromeDrivers[rowIndex].FindElement(By.XPath("//button[@value='Gửi lại mã xác nhận']")).Click();
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        otp = await otpsim.GetCode();
                                    }
                                    if (otp == "")
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Không về code";
                                        throw new Exception();
                                    }
                                    //
                                    facebook.NhapOTPIOS(otp);
                                GoUpAnh:
                                    facebook.NhapAnhWWWIOS();
                                    throw new Exception();
                                }
                                catch (Exception)
                                {
                                    try
                                    {
                                        Interlocked.Decrement(ref iThread);
                                        var data = dgvAccounts.Rows[rowIndex].Cells["status"].Value.ToString();
                                        if (data != "Hãy update chromedrive mới, hoặc trình duyệt cùng profile đang bật tắt nó đi")
                                        {
                                            proUsed.Remove(ran);
                                        }
                                        else
                                        {
                                            dgvAccounts.Rows[rowIndex].Cells["status"].Value = "A - Lỗi mở profile";
                                        }
                                        chromeDrivers[rowIndex].Manage().Cookies.DeleteAllCookies();
                                        chromeDrivers[rowIndex].Quit();
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    return;
                                }
                            }).Start();
                            i++;
                        }
                        else
                        {
                            Thread.Sleep(2000);
                        }
                    }
                }).Start();
            }
            catch (Exception e3)
            {
            }
        }



        
        private void dgvAccounts_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnName = dgvAccounts.Columns[e.ColumnIndex].Name;
            SortHeard(dgvAccounts, columnName, sender, e);
            var list = (BindingList<ModelAccount>)dgvAccounts.DataSource;
            Common.LuuThongTinTaiKhoanKhiKetThuc(list);
        }

        private void SortHeard(DataGridView dgvAccs, string columnName, object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((columnName == "An" || columnName == "Stop") && e.Button == MouseButtons.Left)
            {
                foreach (DataGridViewRow row in dgvAccs.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[e.ColumnIndex];
                    chk.Value = !(chk.Value == null ? false : (bool)chk.Value);
                }
            }
            if (columnName == "XuHT" || columnName == "note" || columnName == "status" || columnName == "tenTDS" || columnName == "pass" || columnName == "Proxy")
            {
                var list = (IList<ModelAccount>)dgvAccs.DataSource;
                if (list == null || list.Count() == 0)
                    return;
                if (columnName == "status" && e.Button == MouseButtons.Left)
                {
                    var listnew = list.OrderByDescending(x => x.status).ToList();
                    if (isDESC)
                    {
                        listnew = list.OrderBy(x => x.status).ToList();
                        isDESC = false;
                    }
                    else
                    {
                        isDESC = true;
                    }
                    dgvAccs.DataSource = new BindingList<ModelAccount>(listnew);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                this.i = 0;
                var soluong = (int)nbrLuong.Value;
                int iThread = 0;
                int Multi_Thread = soluong;
                int soDong = ((BindingList<ModelAccount>)dgvAccounts.DataSource).Count();

                new Thread(delegate ()
                {
                    while (i < soDong)
                    {
                        bool flag = iThread < Multi_Thread;
                        if (flag)
                        {
                            Interlocked.Increment(ref iThread);
                            int row = this.i;
                            Thread.Sleep(1000);
                            new Thread(async delegate ()
                            {
                                dgvAccounts["status", row].Value = "Đắt đầu!";
                                Thread.Sleep(500);
                                if (dgvAccounts["uid", row].Value != null && dgvAccounts["uid", row].Value.ToString() != "")
                                {
                                    var j = row;
                                    CheckStop(j);

                                    try
                                    {
                                        dgvAccounts["status", row].Value = "Khởi tạo Chrome";
                                        TaoChrome(j,0, true);
                                        var facebook = new FaceBook(dgvAccounts, j, chromeDrivers[j]);
                                        dgvAccounts["status", row].Value = "Đăng Nhập Facebook";
                                        facebook.DangNhap();
                                        throw new Exception();
                                    }
                                    catch (Exception e2)
                                    {
                                        try
                                        {
                                            chromeDrivers[j].Manage().Cookies.DeleteAllCookies();
                                            chromeDrivers[j].Quit();
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        Interlocked.Decrement(ref iThread);
                                        return;
                                    }
                                }
                                Interlocked.Decrement(ref iThread);
                            }).Start();
                            i++;
                        }
                        else
                        {
                            Thread.Sleep(2000);
                        }
                    }
                }).Start();
            }
            catch (Exception e3)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Process[] chromeInstances = Process.GetProcessesByName("chrome");
                foreach (Process p in chromeInstances)
                    p.Kill();
                Process[] chromes = Process.GetProcessesByName("chromedriver");
                foreach (Process p in chromes)
                    p.Kill();
            }
            catch (Exception)
            {
            }
        }
    }

}
