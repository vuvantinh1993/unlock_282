﻿using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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

        public Form1()
        {
            InitializeComponent();
            dgvAccounts.AutoGenerateColumns = false;
            this.MaximumSize = new System.Drawing.Size(1005, Screen.PrimaryScreen.WorkingArea.Height);
            this.dgvAccounts.DataSource = Common.DocFileTaiKhoan();
        }

        private void run_Click(object sender, EventArgs e)
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
                                        TaoChrome(j);
                                        var facebook = new FaceBook(dgvAccounts, j, chromeDrivers[j]);
                                        dgvAccounts["status", row].Value = "Đăng Nhập Facebook";
                                        facebook.DangNhap();

                                        await facebook.GoCheckPoint282Async();

                                        throw new Exception();

                                    }
                                    catch (Exception e2)
                                    {
                                        try
                                        {
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

        public async Task GiaiCheckPointAsync(int rowIndex)
        {
            try
            {
                TaoChrome(rowIndex);
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

        private void TaoChrome(int rowIndex)
        {
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            ChromeOptions chromeOptions = new ChromeOptions { };


            var proxsplit = dgvAccounts["proxy", rowIndex].Value.ToString().Split(':');
            var ip = $"{proxsplit[0]}:{proxsplit[1]}";
            var  username = proxsplit[2];
            var pass = proxsplit[3];
            chromeOptions.AddExtension("extension_2_0_0_0.crx");
            chromeOptions.AddArgument(string.Format("--proxy-server={0}", ip));

            SetUpChrome(rowIndex, chromeDriverService, chromeOptions);

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
            Thread.Sleep(1000);

        }

        public bool SetUpChrome(int rowIndex, ChromeDriverService chromeDriverService, ChromeOptions chromeOptions)
        {

            chromeDriverService.SuppressInitialDiagnosticInformation = true;
            chromeDriverService.HideCommandPromptWindow = true;
            try
            {
                chromeOptions.AddArguments(new string[]
                {
                    "--disable-blink-features=AutomationControlled",
                    "--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36",
                    "--disable-notifications",
                    "--disable-popup-blocking",
                    "--disable-geolocation",
                    "--no-sandbox",
                    "--disable-gpu",
                    //"--app=https:/m.facebook.com"
                });
                try
                {
                    string linkFolderProfile = $"{Directory.GetCurrentDirectory()}\\profileAuto";
                    var name = rowIndex % 6;
                    chromeOptions.AddArguments("user-data-dir=" + $"{linkFolderProfile}" + "\\" + name);

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
                    menu_dgv.Items.Add("GiaiCheckPoint").Name = "GiaiCheckPoint";
                    menu_dgv.Items.Add("Copy UID").Name = "Copy UID";
                    menu_dgv.Show(dgvAccounts, new Point(e.X, e.Y));
                    menu_dgv.ItemClicked += new ToolStripItemClickedEventHandler(my_menu_ItemChicked);
                }
            }
        }

        private void my_menu_ItemChicked(object sender, ToolStripItemClickedEventArgs e)
        {
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
                case "Delete":
                    foreach (DataGridViewRow row in dgvAccounts.SelectedRows)
                    {
                        Xoa1TaiKhoan(row.Index);
                        dgvAccounts.Rows.RemoveAt(row.Index);
                    }
                    break;
            }
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
            liststr.Reverse();
            var str = string.Join("\n", liststr);
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
                if (text.Contains("Giải oke !!!"))
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

        

        private void DenLaySDT(int rowIndex, FaceBook facebook)
        {
            
            dgvAccounts["status", rowIndex].Value = "Đăng Nhập Facebook";
            facebook.DangNhap();
            
        }


        private void LayLaiMaOTP(int rowIndex, FaceBook facebook)
        {
            try
            {
                dgvAccounts["status", rowIndex].Value = "Lấy lại otp";
                facebook.LayLaiMaCode();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return;
        }

        private void button1_Click(object sender, EventArgs e)
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
                                        TaoChrome(j);
                                        var facebook = new FaceBook(dgvAccounts, j, chromeDrivers[j]);


                                        dgvAccounts["status", row].Value = "Đăng Nhập Facebook";
                                        DenLaySDT(j, facebook);


                                        var otpsim = new Chothuesimcode();
                                        var sdt = await otpsim.GetPhone();
                                        facebook.GoCheckpoint1(sdt);

                                        dgvAccounts["status", j].Value = "Lấy mã otp";

                                        var otp = "";
                                        var m = 0;
                                    laylaicode:
                                        var n = 0;
                                        while (n <= 11)
                                        {
                                            Thread.Sleep(3000);
                                            otp = await otpsim.GetCode();
                                            if (otp != "")
                                                break;
                                            n++;
                                        }
                                        if (otp == "" && m < 8)
                                        {
                                            m++;
                                            LayLaiMaOTP(j, facebook);
                                            goto laylaicode;
                                        }
                                        if (otp == "")
                                        {
                                            dgvAccounts["status", j].Value = "Không về code";
                                            return;
                                        }
                                        //

                                        facebook.GhiOtpVaUpAnh(otp);
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

        private void opennow_Click(object sender, EventArgs e)
        {
            try
            {
                this.i = 0;
                var soluong = (int)nbrLuong.Value;
                int iThread = 0;
                int Multi_Thread = soluong;
                var listAccs = (BindingList<ModelAccount>)dgvAccounts.DataSource;
                var solan = ((BindingList<ModelAccount>)dgvAccounts.DataSource).Count() / Multi_Thread;
                var tasks = new Task[listAccs.Count()];
                var tasksCon = new Task[listAccs.Count()];
                var listTask = new Task[5000];


                Task c = new Task(async () =>
                {
                    for (int k = 0; k <= solan; k++)
                    {
                        Thread.Sleep(1000);
                        

                        for (int i = soluong * k; i < soluong * (k + 1) && i < listAccs.Count(); i++)
                        {
                            var rowIndex = i;

                            try
                            {
                                dgvAccounts["status", rowIndex].Value = "Khởi tạo Chrome";
                                TaoChrome(rowIndex);
                                var facebook = new FaceBook(dgvAccounts, rowIndex, chromeDrivers[rowIndex]);

                                tasks[rowIndex] = Task.Run(async () =>
                                {
                                    tasksCon[rowIndex] = Task.Run(async () =>
                                    {
                                        try
                                        {
                                            DenLaySDT(rowIndex, facebook);
                                           
                                        }
                                        catch (Exception)
                                        {
                                            try
                                            {
                                                chromeDrivers[rowIndex].Quit();
                                                chromeDrivers[rowIndex] = null;
                                            }
                                            catch (Exception)
                                            {
                                            }
                                            return;
                                        }
                                    });
                                    Task.WaitAll(tasksCon.Where(x => x != null).ToArray());
                                    tasksCon = new Task[listAccs.Count()];

                                    if (chromeDrivers[rowIndex] == null)
                                        return;

                                    // Lấy lại mã code
                                    dgvAccounts["status", rowIndex].Value = "Lấy mã otp";

                                    var otpsim = checkBox1.Checked ? (ResolveCaptcha)new OtpSim(): (ResolveCaptcha)new Chothuesimcode();
                                    try
                                    {
                                        var sdt = await otpsim.GetPhone();
                                        facebook.GoCheckpoint1(sdt);
                                    }
                                    catch (Exception)
                                    {
                                        chromeDrivers[rowIndex].Manage().Cookies.DeleteAllCookies();
                                        chromeDrivers[rowIndex].Quit();
                                        return;
                                    }

                                    if (chromeDrivers[rowIndex] == null)
                                        return;

                                    var otp = "";
                                    var m = 0;
                                laylaicode:
                                    
                                    otp = await otpsim.GetCode();
                                        
                                    if (otp == "" && m < 8)
                                    {
                                        m++;
                                        dgvAccounts["status", rowIndex].Value = "Yêu cầu lại mã mới";
                                        chromeDrivers[rowIndex].FindElement(By.XPath("//div[text()='Gửi lại mã xác nhận']")).Click();
                                        goto laylaicode;
                                    }
                                    if (otp == "")
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Không về code";
                                        try
                                        {
                                            chromeDrivers[rowIndex].Manage().Cookies.DeleteAllCookies();
                                            chromeDrivers[rowIndex].Quit();
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        return;
                                    }
                                    //

                                    facebook.GhiOtpVaUpAnh(otp);
                                    try
                                    {
                                        chromeDrivers[rowIndex].Manage().Cookies.DeleteAllCookies();
                                        chromeDrivers[rowIndex].Quit();
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    return;
                                });
                            }
                            catch (Exception f)
                            {
                                chromeDrivers[rowIndex].Quit();
                                return;
                            }

                        }
                        Task.WaitAll(tasks.Where(x => x != null).ToArray());
                        tasks = new Task[listAccs.Count()];
                    }
                });
                c.Start();

            }
            catch (Exception)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.i = 0;
                var soluong = (int)nbrLuong.Value;
                int iThread = 0;
                int Multi_Thread = soluong;
                var listAccs = (BindingList<ModelAccount>)dgvAccounts.DataSource;
                var solan = ((BindingList<ModelAccount>)dgvAccounts.DataSource).Count() / Multi_Thread;
                var tasks = new Task[listAccs.Count()];
                var tasksCon = new Task[listAccs.Count()];
                var listTask = new Task[5000];


                Task c = new Task(async () =>
                {
                    for (int k = 0; k < solan; k++)
                    {
                        Thread.Sleep(1000);


                        for (int i = soluong * k; i < soluong * (k + 1) && i < listAccs.Count(); i++)
                        {
                            var rowIndex = i;

                            try
                            {
                                dgvAccounts["status", rowIndex].Value = "Khởi tạo Chrome";
                                TaoChrome(rowIndex);
                                var facebook = new FaceBook(dgvAccounts, rowIndex, chromeDrivers[rowIndex]);

                                tasks[rowIndex] = Task.Run(async () =>
                                {
                                    tasksCon[rowIndex] = Task.Run(async () =>
                                    {
                                        try
                                        {
                                            DenLaySDT(rowIndex, facebook);

                                        }
                                        catch (Exception)
                                        {
                                            try
                                            {
                                                chromeDrivers[rowIndex].Quit();
                                                chromeDrivers[rowIndex] = null;
                                            }
                                            catch (Exception)
                                            {
                                            }
                                            return;
                                        }
                                    });
                                    Task.WaitAll(tasksCon.Where(x => x != null).ToArray());
                                    tasksCon = new Task[listAccs.Count()];

                                    if (chromeDrivers[rowIndex] == null)
                                        return;

                                    // Lấy lại mã code
                                    dgvAccounts["status", rowIndex].Value = "Lấy mã otp";

                                    var otpsim = new Chothuesimcode();
                                    try
                                    {
                                        var sdt = await otpsim.GetPhone();
                                        facebook.GoCheckpoint1(sdt);
                                    }
                                    catch (Exception)
                                    {
                                        chromeDrivers[rowIndex].Manage().Cookies.DeleteAllCookies();
                                        chromeDrivers[rowIndex].Quit();
                                        return;
                                    }

                                    if (chromeDrivers[rowIndex] == null)
                                        return;

                                    var otp = "";
                                    var m = 0;
                                laylaicode:

                                    otp = await otpsim.GetCode();

                                    if (otp == "" && m < 8)
                                    {
                                        m++;
                                        dgvAccounts["status", rowIndex].Value = "Yêu cầu lại mã mới";
                                        chromeDrivers[rowIndex].FindElement(By.XPath("//div[text()='Gửi lại mã xác nhận']")).Click();
                                        goto laylaicode;
                                    }
                                    if (otp == "")
                                    {
                                        dgvAccounts["status", rowIndex].Value = "Không về code";
                                        try
                                        {
                                            chromeDrivers[rowIndex].Manage().Cookies.DeleteAllCookies();
                                            chromeDrivers[rowIndex].Quit();
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        return;
                                    }
                                    //

                                    facebook.GhiOtpVaUpAnh(otp);
                                    try
                                    {
                                        chromeDrivers[rowIndex].Manage().Cookies.DeleteAllCookies();
                                        chromeDrivers[rowIndex].Quit();
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    return;
                                });
                            }
                            catch (Exception f)
                            {
                                chromeDrivers[rowIndex].Quit();
                                return;
                            }

                        }
                        Task.WaitAll(tasks.Where(x => x != null).ToArray());
                        tasks = new Task[listAccs.Count()];
                    }
                });
                c.Start();

            }
            catch (Exception)
            {

            }
        }

    }

}
