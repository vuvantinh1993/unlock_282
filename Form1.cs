using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
                                        TaoChrome(j);
                                        var facebook = new FaceBook(dgvAccounts, j, chromeDrivers[j]);
                                       facebook.DangNhap();

                                        await facebook.GoCheckPoint282Async();

                                        chromeDrivers[j].Quit();

                                    }
                                    catch (Exception e2)
                                    {

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
                    "--user-agent=Mozilla/5.0 (Linux; Android 10; SM-G970F) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3396.81 Mobile Safari/537.36",
                    "--disable-notifications",
                    "--disable-popup-blocking",
                    "--disable-geolocation",
                    "--no-sandbox",
                    "--disable-gpu"
                });
                try
                {
                    chromeDrivers[rowIndex] = new ChromeDriver(chromeDriverService, chromeOptions, TimeSpan.FromMinutes(3));
                    chromeDrivers[rowIndex].Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(30));
                    chromeDrivers[rowIndex].Manage().Window.Size = new Size(500, 500);
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
                var with = Screen.PrimaryScreen.Bounds.Width - 500;
                var height = Screen.PrimaryScreen.Bounds.Height - 500;
                var kcNgang = with / 8;
                var kcRong = with / 8;
                var x = rowIndex / 9;
                var y = rowIndex % 9;
                return new Point(kcNgang * y, kcRong * x);
            }
            catch (Exception e)
            {
            }
            return new Point(new Random().Next(1, 10) * 50, new Random().Next(1, 10) * 50);
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
                    menu_dgv.Items.Add("Delete").Name = "Delete";
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
    }
}
