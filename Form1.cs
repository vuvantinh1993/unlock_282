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
                        new Thread(async delegate ()
                        {
                            dgvAccounts["status", row].Value = "Đắt đầu!";
                            Thread.Sleep(500);
                            if (dgvAccounts["TenTDS", row].Value != null && dgvAccounts["TenTDS", row].Value.ToString() != "")
                            {
                                var j = row;
                                CheckStop(j);

                                try
                                {
                                    SetUpChrome(j);
                                    var facebook = new FaceBook(dgvAccounts, j, chromeDrivers[j]);
                                    facebook.DangNhap();
                                    

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

        public void CheckStop(int rowIndex)
        {
            while (cbStop.Checked)
            {
                Thread.Sleep(1000);
                dgvAccounts["status", rowIndex].Value = "Đang tạm dừng";
            }
            dgvAccounts["status", rowIndex].Value = "Huỷ bỏ tạm dừng tiếp tục đi làm";
        }

        public bool SetUpChrome(int rowIndex)
        {
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            ChromeOptions chromeOptions = new ChromeOptions { };

            chromeDriverService.SuppressInitialDiagnosticInformation = true;
            chromeDriverService.HideCommandPromptWindow = true;
            try
            {
                chromeOptions.AddArguments(new string[]
                {
                    "--disable-blink-features=AutomationControlled",
                    "--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36",
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
                var kcNgang = with / 5;
                var kcRong = with / 5;
                var x = rowIndex / 6;
                var y = rowIndex % 6;
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
    }
}
