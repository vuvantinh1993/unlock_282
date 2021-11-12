using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace unlock_282
{
    public class Adapter
    {
        public ManagementObject adapter;
        public string adaptername;
        public string customname;
        public int devnum;
        public static List<Adapter> listCard = new List<Adapter>();

        public Adapter(ManagementObject a, string aname, string cname, int n)
        {
            this.adapter = a;
            this.adaptername = aname;
            this.customname = cname;
            this.devnum = n;
        }
        public Adapter(NetworkInterface i) : this(i.Description) { }

        public Adapter(string aname)
        {
            this.adaptername = aname;
            var searcher = new ManagementObjectSearcher("select * from win32_networkadapter where Name='" + adaptername + "'");
            var found = searcher.Get();
            this.adapter = found.Cast<ManagementObject>().FirstOrDefault();

            try
            {
                var match = Regex.Match(adapter
                    .Path.RelativePath, "\\\"(\\d+)\\\"$");
                this.devnum = int.Parse(match.Groups[1].Value);
            }
            catch
            {
                return;
            }
            this.customname = NetworkInterface.GetAllNetworkInterfaces().Where(
                i => i.Description == adaptername
            ).Select(
                i => " (" + i.Name + ")"
            ).FirstOrDefault();
        }

        public NetworkInterface ManagedAdapter
        {
            get
            {
                return NetworkInterface.GetAllNetworkInterfaces().Where(
                    nic => nic.Description == this.adaptername
                ).FirstOrDefault();
            }
        }
        public string RegistryKey
        {
            get
            {
                return String.Format(@"SYSTEM\ControlSet001\Control\Class\{{4D36E972-E325-11CE-BFC1-08002BE10318}}\{0:D4}", this.devnum);
            }
        }
        public bool SetRegistryMac2(string value)
        {
            bool shouldReenable = false;
            try
            {
                if (value.Length > 0 && !Adapter.IsValidMac(value, false))
                    throw new Exception(value + " is not a valid mac address");

                using (RegistryKey regkey = Registry.LocalMachine.OpenSubKey(this.RegistryKey, true))
                {
                    if (regkey.GetValue("AdapterModel") as string != this.adaptername
                        && regkey.GetValue("DriverDesc") as string != this.adaptername)
                        return false;
                    var result = (uint)adapter.InvokeMethod("Disable", null);
                    if (result != 0)
                        throw new Exception("Failed to disable network adapter.");
                    shouldReenable = true;
                    if (regkey != null)
                    {
                        if (value.Length > 0)
                            regkey.SetValue("NetworkAddress", value, RegistryValueKind.String);
                        else
                            regkey.DeleteValue("NetworkAddress");
                    }

                    return true;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            finally
            {
                if (shouldReenable)
                {
                    uint result = (uint)adapter.InvokeMethod("Enable", null);
                    if (result != 0)
                        MessageBox.Show("Failed to re-enable network adapter.");
                }
            }
        }
        public static string GetNewMac()
        {
            System.Random r = new System.Random();
            byte[] bytes = new byte[6];
            r.NextBytes(bytes);

            // Set second bit to 1
            bytes[0] = (byte)(bytes[0] | 0x02);
            // Set first bit to 0
            bytes[0] = (byte)(bytes[0] & 0xfe);

            return MacToString(bytes);
        }

        public static bool IsValidMac(string mac, bool actual)
        {
            // 6 bytes == 12 hex characters (without dashes/dots/anything else)
            if (mac.Length != 12)
                return false;

            // Should be uppercase
            if (mac != mac.ToUpper())
                return false;

            // Should not contain anything other than hexadecimal digits
            if (!Regex.IsMatch(mac, "^[0-9A-F]*$"))
                return false;

            if (!actual) // The second character should be a 2, 6, A or E
            {
                char c = mac[1];
                return (c == '2' || c == '6' || c == 'A' || c == 'E');
            }

            return true;
        }

        public static bool IsValidMac(byte[] bytes, bool actual)
        {
            return IsValidMac(Adapter.MacToString(bytes), actual);
        }

        public static string MacToString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToUpper();
        }

        public string SetRegistryMac(Adapter adapter)
        {
            var macnew = Adapter.GetNewMac();
            if (adapter.SetRegistryMac2(macnew))
            {
                System.Threading.Thread.Sleep(100);
                return macnew;
            }
            return "";
        }
    }


}
