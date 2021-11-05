using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                DangNhapVoiUidVaPass(dgvAccounts.Rows[rowIndex].Cells["uid"].Value.ToString(), dgvAccounts.Rows[rowIndex].Cells["pass"].Value.ToString());
            }
            catch (Exception e)
            {
                dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thất bại";
                throw new Exception($"Đăng nhập{e.Message}");
            }
            if(chromeDriver.PageSource.Contains("Chúng tôi đã xem xét lại và không hủy bỏ quyết định này.") || chromeDriver.PageSource.Contains("Chúng tôi không thể xem xét lại quyết định này vì tài khoản của bạn đã bị vô hiệu hóa từ lâu."))
            {
                dgvAccounts["status", rowIndex].Value = "Die hẳn";
                throw new Exception();
            }

            if(chromeDriver.Url.Contains("referrer=disabled_checkpoint"))
            {
                dgvAccounts["status", rowIndex].Value = "Tài khoản die -- tải xuống DL";
                throw new Exception();
            }
            try
            {
                chromeDriver.FindElement(By.Name("action_proceed")).Click();
            }
            catch (Exception)
            {
            }


            chromeDriver.Url = "https://www.facebook.com/";
            Thread.Sleep(1000);

            try
            {
                chromeDriver.FindElements(By.XPath("//span[text()='Request a Review']"))[1].Click();
            }
            catch (Exception)
            {

            }

            if (chromeDriver.Url.Contains("1501092823525282"))
            {
                dgvAccounts["status", rowIndex].Value = "Tài khoản bị checkpoint 282";
                return true;
            }
            if (chromeDriver.Url.Contains("828281030927956"))
            {
                dgvAccounts["status", rowIndex].Value = "Tài khoản bị checkpoint 956";
                throw new Exception();
            }
            if (chromeDriver.Url.Contains("checkpoint"))
            {
                dgvAccounts["status", rowIndex].Value = "Tài Khoản Live !!";
                throw new Exception();
            }
            dgvAccounts["status", rowIndex].Value = "Không phải checkpoint 282";
            throw new Exception();
        }

        public void DangNhapVoiCookie(string cookies)
        {
            cookies = cookies.Replace(" ", "");
            foreach (string item in cookies.Split(';'))
            {
                if (item.Split('=').Count() == 2)
                {
                    chromeDriver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie(item.Split('=')[0], item.Split('=')[1]));
                }
            }
        }

        public bool DangNhapVery()
        {
            try
            {
                dgvAccounts["status", rowIndex].Value = "Đi đăng nhập Facebook";
                chromeDriver.Url = "https://mbasic.facebook.com/login.php";
                if (LaTrangDangNhap())
                {
                    if (dgvAccounts.Rows[rowIndex].Cells["_2fa"].Value != null && dgvAccounts.Rows[rowIndex].Cells["_2fa"].Value.ToString() != "")
                    {
                        dgvAccounts["status", rowIndex].Value = "Đăng nhập Fb bằng cookie";
                        chromeDriver.Manage().Cookies.DeleteAllCookies();
                        DangNhapVoiCookie(dgvAccounts.Rows[rowIndex].Cells["_2fa"].Value.ToString());
                    }
                    //chromeDriver.Url = _urlMLogin;
                    //if (LaTrangDangNhap())
                    //{
                    //    dgvAccounts["cookie", rowIndex].Value = "";
                    //    dgvAccounts["status", rowIndex].Value = "Đăng nhập Fb bằng Uid và Pass";
                    //    DangNhapVoiUidVaPass(dgvAccounts.Rows[rowIndex].Cells["id"].Value.ToString(), dgvAccounts.Rows[rowIndex].Cells["pass"].Value.ToString());
                    //}
                }
            }
            catch (Exception e)
            {
                dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thất bại";
                throw new Exception();
            }
            chromeDriver.Url = "https://mbasic.facebook.com/login.php";
            Thread.Sleep(1000);
            if (chromeDriver.Url.Contains("m.facebook.com/nt/screen/?params=") || chromeDriver.PageSource.Contains("Tài khoản của bạn đã bị vô hiệu hóa") || chromeDriver.PageSource.Contains("Your Account Has Been Disabled") || chromeDriver.PageSource.Contains("Chúng tôi cần thêm thông tin"))
            {
                dgvAccounts["status", rowIndex].Value = "Tài khoản bị checkpoint, hãy kiểm tra lại";
                throw new Exception();
            }
            if (!LaTrangDangNhap())
            {
                dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thành công";
                return true;
            }
            dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thất bại";
            throw new Exception();
        }

        public bool ThemPhone()
        {
            try
            {
                chromeDriver.FindElement(By.Name("new")).SendKeys("+1213213123");

                chromeDriver.FindElement(By.Name("submit_new_number")).Click();
                return true;
            }
            catch (Exception e)
            {

            }
            return true;
        }

        private bool LaTrangDangNhap()
        {
            if (chromeDriver.Url.Contains("login"))
            {
                return true;
            }
            return false;
        }

        public bool DangNhap2()
        {
            try
            {
                dgvAccounts["status", rowIndex].Value = "Đăng nhập Fb bằng Uid và Pass";
                DangNhapVoiUidVaPass(dgvAccounts.Rows[rowIndex].Cells["uid"].Value.ToString(), dgvAccounts.Rows[rowIndex].Cells["pass"].Value.ToString());
            }
            catch (Exception e)
            {
                dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thất bại";
                throw new Exception($"Đăng nhập{e.Message}");
            }

            try
            {
                chromeDriver.FindElement(By.Name("action_proceed")).Click();
            }
            catch (Exception)
            {
            }

            chromeDriver.Url = "https://m.facebook.com/";
            if (chromeDriver.Url.Contains("checkpoint") || chromeDriver.PageSource.Contains("Tạo tài khoản mới"))
            {
                dgvAccounts["status", rowIndex].Value = "Tài khoản bị checkpoint, hoặc đăg nhập thất bại";
                throw new Exception();
            }
            dgvAccounts["status", rowIndex].Value = "Tài khoản oke";
            return true;
        }

        public bool DangNhapMbasic()
        {
            try
            {
                dgvAccounts["status", rowIndex].Value = "Đăng nhập Fb bằng Uid và Pass";
                DangNhapVoiUidVaPass(dgvAccounts.Rows[rowIndex].Cells["uid"].Value.ToString(), dgvAccounts.Rows[rowIndex].Cells["pass"].Value.ToString());
            }
            catch (Exception e)
            {
                dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thất bại";
                throw new Exception($"Đăng nhập{e.Message}");
            }

            try
            {
                chromeDriver.FindElement(By.Name("action_proceed")).Click();
            }
            catch (Exception)
            {
            }

            chromeDriver.Url = "https://mbasic.facebook.com/";
            if (chromeDriver.Url.Contains("1501092823525282"))
            {
                dgvAccounts["status", rowIndex].Value = "Tài khoản bị checkpoint 282";
                return true;
            }
            if (!chromeDriver.Url.Contains("login") && !chromeDriver.PageSource.Contains("tài khoản của bạn đã bị khóa"))
            {
                dgvAccounts["status", rowIndex].Value = "Tài khoản oke";
                throw new Exception();
            }
            var num = "";
            num = new Regex(@"\d+").Match(chromeDriver.Url).Value;
            dgvAccounts["status", rowIndex].Value = $"Check point dạng khác {num}";
            throw new Exception();
        }

        public void XoaSDT282()
        {
            try
            {
                var mkcu = dgvAccounts["pass", rowIndex].Value.ToString();
                try
                {
                    dgvAccounts["status", rowIndex].Value = "Chọn gỡ số";
                    chromeDriver.Url = "https://mbasic.facebook.com/settings/sms/?refid=70";
                    chromeDriver.FindElement(By.XPath("//a[text()='Gỡ']")).Click();
                    Thread.Sleep(1000);
                    chromeDriver.FindElement(By.Name("remove_phone_warning_acknwoledged")).Click();
                    chromeDriver.FindElement(By.XPath("//input[@value='Gỡ số']")).Click();

                    dgvAccounts["status", rowIndex].Value = "Nhập mk cũ";
                    Thread.Sleep(1000);
                    chromeDriver.FindElement(By.Name("save_password")).SendKeys(mkcu);
                    chromeDriver.FindElement(By.XPath("//input[@value='Gỡ điện thoại']")).Click();
                    chromeDriver.Url = "https://m.facebook.com";
                    Thread.Sleep(2000);
                    dgvAccounts["status", rowIndex].Value = "Gỡ sđt oke !!!";
                }
                catch (Exception)
                {
                    dgvAccounts["status", rowIndex].Value = "Không tồn tại sđt !!!";
                }
                //chromeDriver.FindElement(By.XPath("//img[contains(@src, 'https://static.xx.fbcdn.net/rsrc.php/v3/ys/r/b5OQbxkXYGI.png')]")).Click();
                //Thread.Sleep(1000);
                //dgvAccounts["status", rowIndex].Value = "Chọn gỡ số";
                //chromeDriver.FindElement(By.XPath("//div[text()='Gỡ']")).Click();
                //Thread.Sleep(2000);
                //chromeDriver.FindElement(By.Name("remove_phone_warning_acknwoledged")).Click();
                //chromeDriver.FindElement(By.XPath("//span[text()='Gỡ số']")).Click();

                //dgvAccounts["status", rowIndex].Value = "Nhập mk cũ";
                //Thread.Sleep(2000);
                //chromeDriver.FindElement(By.Name("save_password")).SendKeys(mkcu);
                //chromeDriver.FindElement(By.XPath("//span[text()='Gỡ điện thoại']")).Click();

            }
            catch (Exception e)
            {

            }
        }


        public void GiaiCaptchawww()
        {
            // cần check lại có capcha hay ko ở đây
            try
            {
                chromeDriver.FindElement(By.XPath("//span[text()='Yêu cầu xem xét lại']")).Click();
                Thread.Sleep(7000);
            }
            catch (Exception)
            {
            }
            try
            {
                chromeDriver.FindElement(By.XPath("//span[text()='Phản đối quyết định']")).Click();
                Thread.Sleep(7000);
            }
            catch (Exception)
            {
            }

            if (chromeDriver.PageSource.Contains("Giúp chúng tôi xác nhận đó là bạn"))
            {
                Thread.Sleep(15000);
                var i = 0;
                while (i < 40)
                {
                    chromeDriver.FindElement(By.XPath("//span[text()='Tiếp tục']")).Click();
                    Thread.Sleep(5000);
                    if (!chromeDriver.PageSource.Contains("Giúp chúng tôi xác nhận đó là bạn"))
                    {
                        break;
                    }
                    i++;
                }
            }
        }

        //public async Task GiaiCaptchawwwAsync()
        //{
        //    // cần check lại có capcha hay ko ở đây
        //    try
        //    {
        //        chromeDriver.FindElement(By.XPath("//span[text()='Yêu cầu xem xét lại']")).Click();
        //        Thread.Sleep(7000);
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    try
        //    {
        //        if (chromeDriver.PageSource.Contains("Giúp chúng tôi xác nhận đó là bạn"))
        //        {
        //            var capt = new ResoveCaptcha();
        //            dgvAccounts["status", rowIndex].Value = "Đi lấy KEY CaptCha";
        //            await capt.SendKeyAsync();
        //            dgvAccounts["status", rowIndex].Value = "Đi lấy TOKEN CaptCha";
        //            var tokenCapt = await capt.GetToken();
        //            if (tokenCapt == "")
        //            {
        //                dgvAccounts["status", rowIndex].Value = "Giải captcha không thành ông";
        //                throw new Exception();
        //            }

        //            IJavaScriptExecutor js = chromeDriver as IJavaScriptExecutor;
        //            js.ExecuteScript($"document.getElementById(\"g-recaptcha-response\").innerHTML=\"{tokenCapt}\";");
        //            js.ExecuteScript($"successCallback({tokenCapt});");
        //            Thread.Sleep(1500);
        //            chromeDriver.FindElement(By.XPath("//span[text()='Tiếp tục']")).Click();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception();
        //    }

        //}

        public bool NhapSDTIOS(string sdt, bool isVN)
        {
            try
            {
                dgvAccounts["status", rowIndex].Value = "Thay số điện thoại khác";
                chromeDriver.FindElements(By.XPath("//span[text()='Cập nhật số di động']"))[1].Click();
                Thread.Sleep(2000);
            }
            catch (Exception)
            {
            }

            try
            {
                var selectElement = chromeDriver.FindElement(By.Name("country_code"));
                var selectObject = new SelectElement(selectElement);
                if (isVN)
                {
                    selectObject.SelectByValue("VN");
                }
                else
                {
                    selectObject.SelectByValue("US");
                }
                Thread.Sleep(1000);
                chromeDriver.FindElement(By.Name("contact_point")).SendKeys(sdt);
                Thread.Sleep(1000);
                chromeDriver.FindElement(By.XPath("//button[contains(@value,'Gửi mã')]")).Click();
                throw new Exception();
            }
            catch (Exception e)
            {
                Thread.Sleep(10000);
                if (chromeDriver.PageSource.Contains("lên ảnh có mặt"))
                {
                    return false;
                }

                if (chromeDriver.PageSource.Contains("Chúng tôi đã nhận được thông tin của bạn"))
                {
                    dgvAccounts["status", rowIndex].Value = "Z - Giải oke !!!";
                    throw new Exception();
                }
                if (chromeDriver.PageSource.Contains("số điện thoại bạn đang cố gắng xác minh"))
                {
                    dgvAccounts["status", rowIndex].Value = "Số điện thoại được dùng nhiều lần";
                    throw new Exception();
                }
                if (chromeDriver.PageSource.Contains("Bạn đã yêu cầu quá nhiều mã SMS"))
                {
                    dgvAccounts["status", rowIndex].Value = "Yêu cầu quá nhiều sms chặn 24h";
                    throw new Exception();
                }
                if (chromeDriver.PageSource.Contains("Gửi lại mã xác nhận"))
                {
                    return true;
                }
                dgvAccounts["status", rowIndex].Value = "Không nhập sdt được";
                throw new Exception();
            }
        }

        public bool NhapSDTwww(string sdt, bool isVN)
        {
            try
            {
                dgvAccounts["status", rowIndex].Value = "Thay số điện thoại khác";
                chromeDriver.FindElements(By.XPath("//span[text()='Cập nhật số di động']"))[1].Click();
                Thread.Sleep(2000);
            }
            catch (Exception)
            {
            }

            try
            {
                if (chromeDriver.PageSource.Contains("Chọn mã quốc gia"))
                {
                    chromeDriver.FindElement(By.XPath("//label[@aria-label='Chọn mã quốc gia']")).Click();
                    Thread.Sleep(2000);
                    var ck = 0;
                    while (ck < 10)
                    {
                        ck++;
                        try
                        {
                            if (isVN)
                            {
                                chromeDriver.FindElement(By.XPath("//span[text()='Việt Nam (+84)']")).Click();
                            }
                            else
                            {
                                chromeDriver.FindElement(By.XPath("//span[text()='Hợp chủng quốc Hoa Kỳ (+1)']")).Click();
                            }
                            break;
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    chromeDriver.FindElement(By.Name("phone")).SendKeys(sdt);
                    chromeDriver.FindElement(By.XPath("//span[contains(text(),'Gửi mã')]")).Click();
                }
                else
                {
                    IWebElement element = chromeDriver.FindElement(By.XPath("//span[text()='Mã quốc gia']"));
                    IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                    js.ExecuteScript("arguments[0].click();", element);

                    Thread.Sleep(2000);
                    var ck = 0;
                    while (ck < 10)
                    {
                        ck++;
                        try
                        {
                            if (isVN)
                            {
                                chromeDriver.FindElement(By.XPath("//span[text()='Việt Nam (+84)']")).Click();
                            }
                            else
                            {
                                chromeDriver.FindElement(By.XPath("//span[text()='Hợp chủng quốc Hoa Kỳ (+1)']")).Click();
                            }
                            break;
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    chromeDriver.FindElement(By.Name("phone")).SendKeys(sdt);
                    chromeDriver.FindElement(By.XPath("//span[contains(text(),'Gửi mã')]")).Click();
                }
                throw new Exception();
            }
            catch (Exception e)
            {
                Thread.Sleep(10000);
                if (chromeDriver.PageSource.Contains("lên ảnh có mặt"))
                {
                    return false;
                }

                if (chromeDriver.PageSource.Contains("Chúng tôi đã nhận được thông tin của bạn"))
                {
                    dgvAccounts["status", rowIndex].Value = "Z - Giải oke !!!";
                    throw new Exception();
                }
                if (chromeDriver.PageSource.Contains("số điện thoại bạn đang cố gắng xác minh"))
                {
                    dgvAccounts["status", rowIndex].Value = "Số điện thoại được dùng nhiều lần";
                    throw new Exception();
                }
                if(chromeDriver.PageSource.Contains("Bạn đã yêu cầu quá nhiều mã SMS"))
                {
                    dgvAccounts["status", rowIndex].Value = "Yêu cầu quá nhiều sms chặn 24h";
                    throw new Exception();
                }
                if(chromeDriver.PageSource.Contains("Gửi lại mã xác nhận"))
                {
                    return true;
                }
                dgvAccounts["status", rowIndex].Value = "Không nhập sdt được";
                throw new Exception();
            }
        }

        public void GoCheckpointMBasic(string sdt)
        {
            // cần check lại có capcha hay ko ở đây
            try
            {
                chromeDriver.FindElement(By.Name("action_proceed")).Click();
                Thread.Sleep(7000);
            }
            catch (Exception)
            {
            }

            if (chromeDriver.PageSource.Contains("Giúp chúng tôi xác nhận đó là bạn"))
            {
                Thread.Sleep(15000);
                var i = 0;
                while (i < 40)
                {
                    chromeDriver.FindElement(By.XPath("//span[text()='Tiếp tục']")).Click();
                    Thread.Sleep(5000);
                    if (!chromeDriver.PageSource.Contains("Giúp chúng tôi xác nhận đó là bạn"))
                    {
                        break;
                    }
                    i++;
                }
            }

            try
            {
                dgvAccounts["status", rowIndex].Value = "Thay số điện thoại khác";
                //chromeDriver.FindElement(By.XPath("//span[text()='Tiếp tục']")).Click();
            }
            catch (Exception)
            {

                throw;
            }

            try
            {
                //var selectElement = 
                chromeDriver.FindElement(By.XPath("//label[@aria-label='Chọn mã quốc gia']")).Click();
                Thread.Sleep(2000);
                var ck = 0;
                while (ck < 10)
                {
                    ck++;
                    try
                    {
                        chromeDriver.FindElement(By.XPath("//span[text()='Việt Nam (+84)']")).Click();
                        break;
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(1000);
                    }
                }
                chromeDriver.FindElement(By.Name("phone")).SendKeys(sdt);
                chromeDriver.FindElement(By.XPath("//span[text()='Gửi mã']")).Click();

                //var selectObject = new SelectElement(selectElement);
                //selectObject.SelectByValue("VN");
            }
            catch (Exception)
            {
                if (chromeDriver.PageSource.Contains("Chúng tôi đã nhận được thông tin của bạn"))
                {
                    dgvAccounts["status", rowIndex].Value = "Z - Giải oke !!!";
                    throw new Exception();
                }
                dgvAccounts["status", rowIndex].Value = "Không nhập sdt được";
                throw new Exception();
            }

        }

        public  void NhapOTP(string otp)
        {
            if (otp != "")
            {
                try
                {
                    dgvAccounts["status", rowIndex].Value = $"Nhập mã otp sim {otp}";
                    chromeDriver.FindElement(By.XPath("//input[contains(@id,'jsc_c')]")).SendKeys(otp);
                    chromeDriver.FindElement(By.XPath("//span[text()='Tiếp']")).Click();
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                }
            }
            dgvAccounts["status", rowIndex].Value = $"Đã nhập xong";
        }
        public void NhapOTPIOS(string otp)
        {
            if (otp != "")
            {
                try
                {
                    dgvAccounts["status", rowIndex].Value = $"Nhập mã otp sim {otp}";
                    chromeDriver.FindElement(By.Name("code")).SendKeys(otp);
                    chromeDriver.FindElement(By.XPath("//button[@value='Tiếp']")).Click();
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                }
            }
            dgvAccounts["status", rowIndex].Value = $"Đã nhập xong";
        }

        public void NhapAnhWWW()
        {
            try
            {

                var d = 0;
                while (!chromeDriver.PageSource.Contains("image/*,image/heif,image/heic") && d < 30)
                {
                    Thread.Sleep(1000);
                    d++;
                }
                Thread.Sleep(1000);
                var linkanh = Common.GetOneImage();
                chromeDriver.FindElement(By.XPath("//input[@accept='image/*,image/heif,image/heic']")).SendKeys(linkanh);
                Thread.Sleep(1500);
                chromeDriver.FindElement(By.XPath("//span[text()='Tiếp tục']")).Click();
                Thread.Sleep(2000);

                var m = 0;
                while (!chromeDriver.PageSource.Contains("Chúng tôi đã nhận được thông tin của bạn") && m < 10)
                {
                    chromeDriver.FindElement(By.XPath("//span[text()='Tiếp tục']")).Click();
                    Thread.Sleep(2000);
                    m++;
                }
                if (m >= 9)
                {
                    dgvAccounts["status", rowIndex].Value = "Up CMT";
                    return;
                }
                if(chromeDriver.PageSource.Contains("Chúng tôi đã nhận được thông tin của bạn"))
                {
                    dgvAccounts["status", rowIndex].Value = "Z - Giải oke !!!";
                    return;
                }
                else
                {
                    dgvAccounts["status", rowIndex].Value = "Không rõ kết quả";
                    return;
                }
            }
            catch (Exception)
            {
            }
            dgvAccounts["status", rowIndex].Value = "UP ảnh không thành công";
            return;
        }

        public void NhapAnhWWWIOS()
        {
            try
            {

                var d = 0;
                while (!chromeDriver.PageSource.Contains("image/*") && d < 30)
                {
                    Thread.Sleep(1000);
                    d++;
                }
                Thread.Sleep(1000);
                var linkanh = Common.GetOneImage();
                chromeDriver.FindElement(By.XPath("//input[@accept='image/*']")).SendKeys(linkanh);
                Thread.Sleep(1500);
                chromeDriver.FindElement(By.XPath("//button[@value='Tiếp tục']")).Click();
                Thread.Sleep(5000);

               
                if (chromeDriver.PageSource.Contains("giấy tờ tùy thân"))
                {
                    dgvAccounts["status", rowIndex].Value = "Up CMT";
                    return;
                }
                if (chromeDriver.PageSource.Contains("Chúng tôi đã nhận được thông tin của bạn"))
                {
                    dgvAccounts["status", rowIndex].Value = "Z - Giải oke !!!";
                    return;
                }
                else
                {
                    dgvAccounts["status", rowIndex].Value = "Không rõ kết quả";
                    return;
                }
            }
            catch (Exception)
            {
            }
            dgvAccounts["status", rowIndex].Value = "UP ảnh không thành công";
            return;
        }

        public void LayLaiMaCode()
        {
            dgvAccounts["status", rowIndex].Value = "Yêu cầu lại mã mới";
            chromeDriver.FindElement(By.XPath("//div[text()='Gửi lại mã xác nhận']")).Click();
        }

        public async Task<bool> GoCheckPoint282Async()
        {
            // cần check lại có capcha hay ko ở đây
            chromeDriver.Url = "https://www.facebook.com/";
            Thread.Sleep(1000);
            chromeDriver.FindElement(By.XPath("//span[text()='Yêu cầu xem xét lại']")).Click();
            // đoạn này đợi gải captcha
            chromeDriver.Url = "https://m.facebook.com/login.php";

            Thread.Sleep(500);
            try
            {
                chromeDriver.FindElement(By.Name("action_proceed")).Click();
                Thread.Sleep(3000);
            }
            catch (Exception)
            {
            }

            try
            {
                var selectElement = chromeDriver.FindElement(By.Name("country_code"));
                var selectObject = new SelectElement(selectElement);
                selectObject.SelectByValue("VN");
            }
            catch (Exception)
            {
                dgvAccounts["status", rowIndex].Value = "Có captcha";
                throw new Exception();
            }

            var otpsim = new Chothuesimcode("");

            dgvAccounts["status", rowIndex].Value = "Lấy 1 số điện thoại";
            var sdt = await otpsim.GetPhone();

            chromeDriver.FindElement(By.Name("contact_point")).SendKeys(sdt.ToString());
            chromeDriver.FindElement(By.Name("action_set_contact_point")).Click();

            var otp = "";
            try
            {
                var i = 0;
                while (i <= 8)
                {
                    Thread.Sleep(5000);
                    dgvAccounts["status", rowIndex].Value = "Đang lấy mã OTP sim";
                    otp = await otpsim.GetCode();
                    if (otp != "")
                        break;
                    dgvAccounts["status", rowIndex].Value = "Yêu cầu lại mã mới";
                    chromeDriver.FindElement(By.Id("action_resend_code")).Click();
                    i++;
                }
            }
            catch (Exception e5)
            {
                dgvAccounts["status", rowIndex].Value = "Không về code";
                throw new Exception();
            }

            dgvAccounts["status", rowIndex].Value = $"Mã otp sim {otp}";
            chromeDriver.FindElement(By.Name("code")).SendKeys(otp);
            chromeDriver.FindElement(By.XPath("//button[@name='action_submit_code']")).Click();
            Thread.Sleep(1000);

            var d = 0;
            while (!chromeDriver.PageSource.Contains("mobile_image_data") || d > 12)
            {
                Thread.Sleep(1000);
                d++;
            }
            Thread.Sleep(1000);
            var linkanh = Common.GetOneImage();
            chromeDriver.FindElement(By.Id("mobile_image_data")).SendKeys(linkanh);
            Thread.Sleep(500);
            chromeDriver.FindElement(By.Name("action_upload_image")).Click();
            Thread.Sleep(1000);

            var m = 0;
            while (!chromeDriver.PageSource.Contains("Chúng tôi đã nhận được thông tin của bạn") && m < 10)
            {
                Thread.Sleep(1000);
                m++;
            }
            if (m >= 9)
            {
                dgvAccounts["status", rowIndex].Value = "Up CMT";
                return false;
            }
            dgvAccounts["status", rowIndex].Value = "Z - Giải oke !!!";
            return true;
        }


        public bool DangNhapVoiUidVaPass(string userId, string pass)
        {
            try
            {
                chromeDriver.Url = "https://m.facebook.com/login";
                chromeDriver.Manage().Cookies.DeleteAllCookies();
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
                var _2fa = dgvAccounts["_2fa", rowIndex].Value.ToString().Replace(" ", "");
                if (_2fa != "")
                {
                    Thread.Sleep(3000);
                    var code = _2Fa.Lay2FaFB(_2fa);
                    dgvAccounts["status", rowIndex].Value = $"Nhập mã 2Fa là {code}";
                    chromeDriver.FindElement(By.Name("approvals_code")).SendKeys(code);
                    chromeDriver.FindElement(By.Name("submit[Submit Code]")).Click();
                    Thread.Sleep(500);
                    chromeDriver.FindElement(By.Name("submit[Continue]")).Click();
                }
                //Thread.Sleep(1000);
                //if (chromeDriver.PageSource.Contains("Đăng nhập bằng một lần nhấn"))
                //{
                //    chromeDriver.FindElement(By.XPath("//a[contains(@href,'login')]")).Click();
                //}
                //Thread.Sleep(1000);
                try
                {
                    if(chromeDriver.PageSource.Contains("Chúng tôi đã xem xét lại và không hủy bỏ quyết định này.") || chromeDriver.PageSource.Contains("Chúng tôi không thể xem xét lại quyết định này vì tài khoản của bạn đã bị vô hiệu hóa từ lâu."))
                    {
                        return true;
                    }
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
                if (chromeDriver.PageSource.Contains("Chúng tôi đã xem xét lại và không hủy bỏ quyết định này.") || chromeDriver.PageSource.Contains("Chúng tôi không thể xem xét lại quyết định này vì tài khoản của bạn đã bị vô hiệu hóa từ lâu."))
                {
                    return true;
                }
                try
                {
                    chromeDriver.FindElement(By.XPath("//div[@data-sigil='more_language']")).Click();
                    Thread.Sleep(1000);
                    chromeDriver.FindElement(By.XPath("//span[@value='vi_VN']")).Click();
                    chromeDriver.Url = "https://www.facebook.com";
                    return true;
                }
                catch (Exception)
                {
                }
                //try
                //{
                //    chromeDriver.Url = "https://mbasic.facebook.com";
                //    chromeDriver.FindElement(By.XPath("//div[@class='ce cf cg']")).Click();
                //    //chromeDriver.FindElement(By.XPath("//div[@aria-label='Complete list of languages']")).Click();
                //    Thread.Sleep(1000);
                //    chromeDriver.FindElement(By.XPath("//input[@value='Tiếng Việt']")).Click();
                //    chromeDriver.Url = "https://www.facebook.com";
                //}
                //catch (Exception)
                //{

                //}
            }
            catch (Exception)
            {
            }
            return true;
        }
    }
}
