using System.DirectoryServices;
using System.Runtime.InteropServices;
using System;


namespace Utility.HelpClass
{
    /// <summary>
    /// 域帳號管理
    /// </summary>
    public static class ADManage
    {
        [DllImport("advapi32.dll")]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        /// <summary>
        /// 域帳號登陸(本地驗證)
        /// </summary>
        /// <param name="ADServerName">域名稱</param>
        /// <param name="ADAccount">域帳號</param>
        /// <param name="ADPwd">域密碼</param>
        /// <returns>True:驗證成功；False:登陸失敗</returns>
        public static bool ADLogon(string ADServerName, string ADAccount, string ADPwd)
        {
            DirectoryEntry entry = new DirectoryEntry("WinNT:" + ADServerName, ADAccount, ADPwd);
            const int LOGON32_LOGON_INTERACTIVE = 2;//通過網絡驗證域合法性
            const int LOGON32_PROVIDER_DEFAULT = 0; //使用默認的Windows 2000/NT NTLM驗證方式
            IntPtr tokenHandle = new IntPtr(0);
            tokenHandle = IntPtr.Zero;
            string test = entry.Username;
            bool checkok = LogonUser(ADAccount, ADServerName, ADPwd, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref tokenHandle);
            return checkok;
        }

        /// <summary>
        /// 域帳號授權認證
        /// </summary>
        /// <param name="ADServerName">域名稱</param>
        /// <param name="ADAccount">域帳號</param>
        /// <param name="ADPwd">域密碼</param>
        /// <returns>True:驗證成功；False:登陸失敗</returns>
        public static bool ADAuthenticate(string ADServerName, string ADAccount, string ADPwd)
        {
            bool isLogin = false;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}", ADServerName), ADAccount, ADPwd);

                entry.RefreshCache();
                isLogin = true;
            }
            catch
            {
                isLogin = false;
            }
            return isLogin;
        }

        private static DirectoryEntry entry = new DirectoryEntry("WinNT:");
        /// <summary>
        /// 檢查用戶帳號是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool ExistWinUser(string servername, string username)
        {
            try
            {
                foreach (DirectoryEntry domain in entry.Children)
                {
                    if (domain.Name.ToLower() == servername.ToLower())
                    {
                        DirectoryEntry pp = domain.Children.Find(username, "user"); 
                        try
                        {
                            pp.RefreshCache();
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
