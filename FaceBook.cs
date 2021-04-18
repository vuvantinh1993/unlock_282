using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace unlock_282
{
    class FaceBook
    {
        private DataGridView dgvAccounts;
        private int rowIndex;
        public IWebDriver chromeDriver;
        private bool chidangnhapbanguidpass;
        private string tokenfb;
        private string proxy;
        private string _urlMLogin { get => "https://m.facebook.com/login.php"; }

        public FaceBook(DataGridView dgvAccounts, int rowIndex, IWebDriver chromeDriver)
        {
            this.dgvAccounts = dgvAccounts;
            this.rowIndex = rowIndex;
            this.chromeDriver = chromeDriver;
        }

        public bool DangNhap()
        {
            try
            {
                dgvAccounts["status", rowIndex].Value = "Đăng nhập Fb bằng Uid và Pass";
                DangNhapVoiUidVaPass(dgvAccounts.Rows[rowIndex].Cells["id"].Value.ToString(), dgvAccounts.Rows[rowIndex].Cells["pass"].Value.ToString());
            }
            catch (Exception e)
            {
                dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thất bại";
                throw new Exception($"Đăng nhập{e.Message}");
            }
            chromeDriver.Url = _urlMLogin;
            Thread.Sleep(1000);
            if (chromeDriver.Url.Contains("1501092823525282"))
            {
                dgvAccounts["status", rowIndex].Value = "Tài khoản bị checkpoint, hãy kiểm tra lại";
                return true;
            }
            dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thất bại";
            throw new Exception();
        }

        public bool GoCheckPoint282()
        {
            chromeDriver.FindElement(By.Name("action_proceed")).Click();

            return true;
        }


        public bool DangNhapVoiUidVaPass(string userId, string pass)
        {
            try
            {
                chromeDriver.Url = "https://m.facebook.com/login";
                try
                {
                    chromeDriver.FindElement(By.Id("accept-cookie-banner-label")).Click();
                }
                catch (Exception)
                {
                }
                Thread.Sleep(1000);
                chromeDriver.FindElement(By.Id("m_login_email")).SendKeys(userId);
                chromeDriver.FindElement(By.Id("m_login_password")).SendKeys(pass);
                chromeDriver.FindElement(By.Name("login")).Click();
                // neu co 2Fa thif dang nhap
                Thread.Sleep(1000);
                if (chromeDriver.PageSource.Contains("Đăng nhập bằng một lần nhấn"))
                {
                    chromeDriver.FindElement(By.XPath("//a[contains(@href,'login')]")).Click();
                }
                var _2fa = dgvAccounts["Fa", rowIndex].Value.ToString().Replace(" ", "");
                if (_2fa != "")
                {
                    Thread.Sleep(1500);
                    var code = _2Fa.Lay2FaFB(_2fa);
                    dgvAccounts["status", rowIndex].Value = $"Nhập mã 2Fa là {code}";
                    chromeDriver.FindElement(By.Name("approvals_code")).SendKeys(code);
                    chromeDriver.FindElement(By.Name("submit[Submit Code]")).Click();
                    Thread.Sleep(500);
                    chromeDriver.FindElement(By.Name("submit[Continue]")).Click();
                }
                Thread.Sleep(1000);
                if (chromeDriver.PageSource.Contains("Đăng nhập bằng một lần nhấn"))
                {
                    chromeDriver.FindElement(By.XPath("//a[contains(@href,'login')]")).Click();
                }
                Thread.Sleep(1000);
                try
                {
                    chromeDriver.FindElement(By.Id("checkpointSubmitButton-actual-button")).Click();
                    Thread.Sleep(1000);
                    chromeDriver.FindElement(By.Id("checkpointSubmitButton-actual-button")).Click();
                    Thread.Sleep(1000);
                    chromeDriver.FindElement(By.Id("checkpointSubmitButton-actual-button")).Click();
                    Thread.Sleep(1000);
                    chromeDriver.FindElement(By.Id("checkpointSubmitButton-actual-button")).Click();
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
            return true;
        }
    }
}
