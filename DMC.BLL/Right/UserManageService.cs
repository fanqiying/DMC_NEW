using System;
using System.Collections.Generic;
using System.Data;
using DMC.Model;
using Utility.HelpClass;
using DMC.DAL;
using DMC.BLL.Right;
namespace DMC.BLL
{
    /// <summary>
    /// 用戶資料權限業務操作類
    /// code by jeven
    /// </summary>
    public class UserManageService
    {
        private UserManageDAL userManage = new UserManageDAL();

        /// <summary>
        /// 用戶登陸
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="pwd">密碼</param>
        /// <param name="companyId">公司編號</param>
        /// <param name="UserList">帳號列表</param>
        /// <returns>
        /// EU0001	帳號輸入錯誤；
        /// EU0002	密碼輸入錯誤；
        /// EU0030	公司選擇錯誤；
        /// EU0004	域帳號登陸失敗；
        /// SU0001	用戶登陸成功；
        /// SU0002	域帳號認證成功；
        /// </returns>
        public UserInfo UserLogon(string account, string pwd, string companyId, string languageId, string area, out string error, out List<UserInfo> acountList)
        {
            error = string.Empty;
            UserInfo userInfo = new UserInfo();
            acountList = new List<UserInfo>();
            if (IsExitUserID(account))//验证账号
            {
                if (IsUsedUserID(account))
                {
                    string checkpwd = UserRightManageService.EnCryptPassword(pwd);
                    if (IsOKPwd(account, checkpwd))//验证密码
                    {
                        if (IsOKcomp(account, checkpwd, companyId))//验证公司别
                        {
                            userInfo = GetUserInfoByID(account);
                            if (userInfo != null && userInfo.userID != "")
                            {
                                userInfo.Company.companyID = companyId;
                                userInfo.LanguageId = languageId;
                                //存储用户的登录信息
                                SetUserCookie(userInfo);
                                error = "SU0001";
                            }
                        }
                        else
                        {
                            error = "EU0030";
                        }
                    }
                    else
                    {
                        error = "EU0002";//密碼輸入錯誤
                    }
                }
                else
                {
                    error = "EU0010";
                }
            }
            //域账号登录
            else if (IsDomainID(account))
            {
                if (IsDomainComp(account, companyId))
                {
                    List<UserInfo> userList = GetUserInfoByDomain(account, companyId);
                    foreach (UserInfo userItem in userList)
                    {
                        if (string.IsNullOrEmpty(area))
                        {
                            userInfo = userItem;
                            error = CurrentADLogon(userItem, account, pwd);
                            acountList = userList;
                            break;
                        }
                        else if (userItem.domainAddr == area)
                        {
                            userInfo = userItem;
                            error = CurrentADLogon(userItem, account, pwd);
                            acountList = userList.FindAll(o => o.domainAddr == area);
                            break;
                        }
                    }
                    if (error == "SU0002")
                    {
                        userInfo.Company.companyID = companyId;
                        userInfo.LanguageId = languageId;
                        SetUserCookie(userInfo);
                    }
                }
                else
                {
                    error = "EU0030";//域账号公司别选择错误
                }
            }
            else
            {
                error = "EU0001"; //账号不存在
            }
            return userInfo;
        }

        private string CurrentADLogon(UserInfo userItem, string account, string pwd)
        {
            if (ADManage.ADLogon(userItem.domainAddr, account, pwd))
            {
                return "SU0002";
            }
            else
            {
                return "EU0004";
            }
        }

        //jeven_xiao add  (写用户cookie)
        private void SetUserCookie(UserInfo u)
        {
            System.Web.HttpContext.Current.Session["UserMain"] = u;
            int ct = 24 * 30;
            GetData.setCookie("UserID", u.userID, DateTime.Now.AddHours(ct), "");
            GetData.setCookie("UserDept", u.userDept, DateTime.Now.AddHours(ct), "");
            GetData.setCookie("UserComp", u.Company.companyID, DateTime.Now.AddHours(ct), "");
            GetData.setCookie("LanguageID", u.LanguageId, DateTime.Now.AddHours(ct), "");
        }


        /// <summary>
        /// 取当前session中的用户信息，如果超时就重新获取
        /// jeven_xiao add 
        /// 2013-6-18
        /// shenglin_yu modify
        /// 2016-2-2
        /// </summary>
        /// <returns>UserInfo 实体</returns>
        public UserInfo GetUserMain()
        {
            UserInfo user = new UserInfo();
            object userObj = GetData.GetSessionObj("UserMain");
            if (userObj != null)
            {
                try
                {
                    user = (UserInfo)userObj;
                }
                catch
                {
                    user.userID = "";

                }
            }
            return user;
        }



        /// <summary>
        /// 獲取用戶或權限類別的程式權限設置樹
        /// </summary>
        /// <param name="type">類別：1、用戶；2、權限類別</param>
        /// <param name="typeId">用戶編號或權限編號</param>
        /// <param name="langId">語言編號</param>
        /// <param name="MenuTree">返回設置的樹結構</param>
        /// <returns>
        /// EU0005 用戶不存在
        /// EU0006 語言類別不存在
        /// EU0007 類別設置錯誤
        /// ER0009 權限類別編號不存在
        /// 返回空表示成功
        /// </returns>
        public string GetSettingMenuTree(string isUserOrRose, string typeId, string langId, ref DataTable MenuTree)
        {
            return userManage.GetSettingMenuTree(isUserOrRose, typeId, langId, ref MenuTree);
        }

        /// <summary>
        /// 獲取菜單的主目錄
        /// </summary>
        /// <param name="langId">語言編號</param>
        /// <param name="MenuTree">返回菜單樹</param>
        /// <returns>
        /// EU0006 語言類別不存在
        /// 返回空表示成功
        /// </returns>
        public string GetMainMenu(string langId, ref DataTable MenuTree)
        {
            return userManage.GetMainMenu(langId, ref MenuTree);
        }
        /// <summary>
        /// 根據用戶名稱的關鍵字帶出包含該關鍵字的所有用戶列表
        /// </summary>
        /// <param name="keyname">用戶名稱關鍵字</param>
        /// <returns>List</returns>
        public static List<userShort> GetUserInfo(string keyname)
        {
            List<userShort> listT = new List<userShort>();
            List<userShort> listInfo = new List<userShort>();
            if (!Cache.Exists("Cache_UserInfo"))
            {
                listT = ReturnUserCCache();
            }
            else
            {
                listT = Cache.GetCache("Cache_UserInfo") as List<userShort>;
            }

            //检索字母

            listInfo = listT.FindAll(r => r.userID.ToLower().IndexOf(keyname.ToLower()) > -1 ? true : false);

            return listInfo;
        }
        /// <summary>
        /// 返回用戶列表并設置用戶資料緩存
        /// </summary>
        /// <returns></returns>
        public static List<userShort> ReturnUserCCache()
        {
            List<userShort> listT = new List<userShort>();
            if (!Cache.Exists("Cache_UserInfo"))
            {
                try
                {
                    UserManageDAL da = new UserManageDAL();
                    DataTable dtt = da.GetUserInfo();
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        userShort tinfo = new userShort();
                        tinfo.userID = dtt.Rows[i]["userID"].To_String();
                        tinfo.userName = dtt.Rows[i]["userName"].To_String();
                        tinfo.userMail = dtt.Rows[i]["userMail"].To_String();
                        listT.Add(tinfo);
                    }

                    if (listT.Count > 0)
                        Cache.SetCache("Cache_UserInfo", listT, 60 * 60 * 8);
                }
                catch
                {
                }

            }
            else
            {
                listT = Cache.GetCache("Cache_UserInfo") as List<userShort>;
            }

            return listT;
        }


        /// <summary>
        /// 验证系统账号是否存在
        /// add by jeven_xiao
        /// 20313-6-25
        /// </summary>
        /// <param name="userID">系统账号</param>
        /// <returns>ture or false</returns>
        public bool IsExitUserID(string userID)
        {
            return userManage.IsExitUserID(userID);
        }




        /// <summary>
        /// 验证系统账号是否是有效的帳號
        /// add by jeven_xiao
        /// 20313-6-25
        /// </summary>
        /// <param name="userID">系统账号</param>
        /// <returns>ture or false</returns>
        public bool IsUsedUserID(string userID)
        {
            return userManage.IsUsedUserID(userID);
        }


        /// <summary>
        /// 验证系统密码是否正确
        /// add by jeven_xiao
        /// 2013-6-25
        /// </summary>
        /// <param name="userID">账号</param>
        /// <param name="userPwd">账号密码</param>
        /// <returns>true or false</returns>
        public bool IsOKPwd(string userID, string userPwd)
        {
            return userManage.IsOKPwd(userID, userPwd);
        }

        /// <summary>
        /// 验证用户的公司别是否正确
        /// add by jeven_xiao
        /// </summary>
        /// <param name="userID">系统账号</param>
        /// <param name="userPwd">账号密码</param>
        /// <param name="compID">账号所属公司别</param>
        /// <returns>true or bool</returns>
        public bool IsOKcomp(string userID, string userPwd, string compID)
        {
            return userManage.IsOKcomp(userID, userPwd, compID);
        }

        /// <summary>
        /// 根据用户ID取得用户详细信息的实体
        /// add by jeven_xiao
        /// </summary>
        /// <param name="userID">用户的账号</param>
        /// <returns>userinfo  实体</returns>
        public UserInfo GetUserInfoByID(string userID)
        {
            return userManage.GetUserInfoByID(userID);
        }
        /// <summary>
        /// 根據域帳號獲取用戶的基本信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="compId"></param>
        /// <returns></returns>
        public List<UserInfo> GetUserInfoByDomain(string account, string compId)
        {
            return userManage.GetUserInfoByDomain(account, compId);
        }

        /// <summary>
        /// 验证是否是域账号
        /// add by jeven_xiao 
        /// </summary>
        /// <param name="userID">用户的账号</param>
        /// <returns>true or false</returns>
        public bool IsDomainID(string userID)
        {
            return userManage.IsDomainID(userID);
        }

        /// <summary>
        /// 验证是否是域账号
        /// add by jeven_xiao 
        /// </summary>
        /// <param name="userID">用户的账号</param>
        /// <param name="compID">公司别ID</param>
        /// <returns>true or false</returns>
        public bool IsDomainComp(string userID, string compID)
        {
            return userManage.IsDomainComp(userID, compID);
        }

        /// <summary>
        /// 更新用戶登錄資料
        /// </summary>
        /// <param name="userID">登錄用戶ID</param>
        /// <param name="ip">最後登錄IP地址</param>
        /// <param name="time">最後登錄時間</param>
        /// <returns></returns>
        public bool ModUserLoginInfo(string userID, string ip, DateTime time)
        {
            return userManage.ModUserLoginInfo(userID, ip, time);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userID">用户的账号</param>
        /// <param name="pwd">用户的新密码</param>
        /// <returns>bool</returns>
        public bool ModUserPwd(string userID, string pwd)
        {
            return userManage.ModUserPwd(userID, pwd);

        }

        /// <summary>
        /// 验证新密码是否正确
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="oldPwd"></param>
        /// <returns></returns>
        public bool IsOkOldPwd(string userID, string oldPwd)
        {
            return userManage.IsOkOldPwd(userID, oldPwd);
        }

        /// <summary>
        /// 验证删除员工时是否删除账号
        /// </summary>
        /// <param name="empID">員工編號</param>
        /// <returns>bool</returns>
        public bool IsExitUserIDByEmpID(string empID)
        {
            return userManage.IsExitUserIDByEmpID(empID);
        }

    }
}
