using System.Collections.Generic;
using DMC.DAL;
using DMC.Model;
using System.Data;
using Utility.HelpClass;

namespace DMC.BLL
{
    /// <summary>
    /// 用戶以及用戶權限業務操作類
    /// code by klint 
    /// 2013-7-3
    /// </summary>
    public class UserRightManageService
    {
        //實例化數據訪問層
        private UserRightManageDAL userRightManage = new UserRightManageDAL();
        private CompanyManageDAL companyManage = new CompanyManageDAL();
        private SupplyManageDAL supplyManage = new SupplyManageDAL();
        private RoseManageDAL roseManage = new RoseManageDAL();
        private DeptDAL deptManage = new DeptDAL();
        private PageManage pageView = new PageManage();

        /// <summary>
        /// 搜索系統用戶
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">第幾页</param>
        /// <param name="pageCount">總页數</param>
        /// <param name="total">總條數</param>
        /// <param name="Where">搜索條件</param>
        /// <returns></returns>
        public DataTable Search(int pageSize, int pageIndex, out int pageCount, out int total, string Where = "")
        {
            return pageView.PageView("t_User", "userID", pageIndex, pageSize, "*", "AutoId ASC", Where, out total, out pageCount);
        }

        /// <summary>
        /// 獲取用戶權限類別
        /// </summary>
        /// <param name="UserId">用戶編號</param>
        /// <param name="UserType">用戶類型</param>
        /// <param name="KeyWord">關鍵字</param>
        /// <returns>DataTable</returns>
        public DataTable GetUserRose(string UserId, string UserType, string KeyWord)
        {
            return userRightManage.GetUserRose(UserId, UserType, KeyWord);
        }
        /// <summary>
        /// 自動獲取對應員工、供應商和客戶的信息
        /// </summary>
        /// <param name="Keyword">關鍵字</param>
        /// <param name="UserType">用戶類別</param>
        /// <returns>DataTable</returns>
        public DataTable JointESC(string Keyword, string UserType)
        {
            return userRightManage.JointESC(Keyword, UserType);
        }
        /// <summary>
        /// 新增用戶，帶權限
        /// </summary>
        /// <param name="model">用戶實體</param>
        /// <param name="RoseIdList">權限ID</param>
        /// <param name="CompanyIdList">公司別ID字符串</param>
        /// <param name="SupplyIdList">供應商ID字符串</param>
        /// <returns>string</returns>
        public string AddUserInfo(t_User model, List<string> RoseIdList, List<string> CompanyIdList, List<string> SupplyIdList)
        {
            string result = "";
            if (string.IsNullOrEmpty(model.userID))
                result = "EU0001";//用戶編號設置錯誤
            if (string.IsNullOrEmpty(result) && userRightManage.ExistsUserId(model.userID))
                result = "EU0002";//用戶編號已刪除
            if (string.IsNullOrEmpty(result) && string.IsNullOrEmpty(model.userName))
                result = "EU0003";//用戶姓名必須輸入
            if (string.IsNullOrEmpty(result) && string.IsNullOrEmpty(model.userType))
                result = "EU0004";//用戶類型必須輸入

            if (!string.IsNullOrEmpty(model.userDept) && !deptManage.IsExitDept(model.userDept))
            {
                result = "EB0023";
            }
            //如果域账号不为空时，则验证域账号的正确性
            if (!string.IsNullOrEmpty(model.domainID.Trim()) && !string.IsNullOrEmpty(model.domainAddr.Trim()))
            {
                if (!ADManage.ExistWinUser(model.domainAddr, model.domainID))
                {
                    result = "NotDomain";
                }
            }

            if (string.IsNullOrEmpty(result))
                switch (model.userType)
                {
                    case "01":
                        if (!userRightManage.IsEmployee(model.userNo))
                            result = "EU0005";//員工編號設置錯誤
                        break;
                    case "02":
                        if (!userRightManage.IsSupply(model.userNo))
                            result = "EU0006";//供應商編號設置錯誤
                        break;
                    case "03":
                        //if (!userRightManage.IsCustomer(model.userNo))
                        //    result = "EU0007";//供應商編號設置錯誤
                        break;
                }

            if (string.IsNullOrEmpty(result))
            {
                using (Trans t = new Trans())
                {
                    try
                    {
                        bool isResult = true;
                        if (model.userType == "01")
                        {
                            //更新員工信息表中對應的信息
                            EmpDAL empManage = new EmpDAL();
                            empManage.UpdateEmpEmailInfo(model.userNo, model.userName, model.userMail, t);
                        }
                        //empManage.
                        //添加員工基本信息
                        isResult = userRightManage.AddUserInfo(model, t);
                        //添加員工的權限列表
                        if (isResult)
                        {
                            foreach (string RoseId in RoseIdList)
                            {
                                if (!roseManage.ExistsRoseId(RoseId, t))
                                {
                                    result = "ER0007";
                                    isResult = false;
                                    break;
                                }
                                isResult = userRightManage.AddUserRose(model.userID, RoseId, t);
                                if (!isResult)
                                    break;
                            }
                        }
                        //添加公司權限
                        if (isResult)
                        {
                            foreach (string CompanyId in CompanyIdList)
                            {
                                if (!companyManage.ExistsCompanyId(CompanyId, t))
                                {
                                    result = "EU0008";//公司編號不存在
                                    isResult = false;
                                    break;
                                }

                                isResult = companyManage.AddUserCompanyRight(model.userID, CompanyId, t);
                                if (!isResult)
                                    break;
                            }
                        }

                        //添加供應商權限
                        if (isResult)
                        {
                            foreach (string SupplyId in SupplyIdList)
                            {
                                if (!supplyManage.ExistsSupplyId(SupplyId, t))
                                {
                                    result = "ER0018";//供應商不存在
                                    isResult = false;
                                    break;
                                }
                                isResult = supplyManage.AddSupplyByUser(model.userID, SupplyId, t);
                                if (!isResult)
                                    break;
                            }
                        }

                        if (isResult)
                        {
                            t.Commit();
                            result = "success";
                        }
                        else
                        {
                            t.RollBack();
                            result = "fail";
                        }
                    }
                    catch
                    {
                        t.RollBack();
                    }
                    finally
                    {
                        t.DbConnection.Close();
                        t.Dispose();
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 修改用戶詳細信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="RoseIdList">權限ID編號</param>
        /// <param name="CompanyIdList">公司ID編號</param>
        /// <param name="SupplyIdList">供應商編號</param>
        /// <returns></returns>
        public string ModUserInfo(t_User model, List<string> RoseIdList, List<string> CompanyIdList, List<string> SupplyIdList)
        {
            string result = "";
            if (string.IsNullOrEmpty(model.userID))
                result = "EU0001";//用戶編號設置錯誤
            if (string.IsNullOrEmpty(result) && !userRightManage.ExistsUserId(model.userID))
                result = "EU0009";//用戶編號已存在
            if (string.IsNullOrEmpty(result) && string.IsNullOrEmpty(model.userName))
                result = "EU0003";//用戶姓名必須輸入
            if (string.IsNullOrEmpty(result) && string.IsNullOrEmpty(model.userType))
                result = "EU0004";//用戶類型必須輸入

            if (!string.IsNullOrEmpty(model.userDept) && !deptManage.IsExitDept(model.userDept))
            {
                result = "EB0023";
            }

            //如果域账号不为空时，则验证域账号的正确性
            if (!string.IsNullOrEmpty(model.domainID.Trim()) && !string.IsNullOrEmpty(model.domainAddr.Trim()))
            {
                if (!ADManage.ExistWinUser(model.domainAddr, model.domainID))
                {
                    result = "NotDomain";
                }
            }

            if (string.IsNullOrEmpty(result))
                switch (model.userType)
                {
                    case "01":
                        if (!userRightManage.IsEmployee(model.userNo))
                            result = "EU0005";//員工編號設置錯誤
                        break;
                    case "02":
                        if (!userRightManage.IsSupply(model.userNo))
                            result = "EU0006";//供應商編號設置錯誤
                        break;
                    case "03":
                        //if (!userRightManage.IsCustomer(model.userNo))
                        //    result = "EU0007";//供應商編號設置錯誤
                        break;
                }

            if (string.IsNullOrEmpty(result))
            {
                using (Trans t = new Trans())
                {
                    try
                    {
                        bool isResult = true;
                        if (model.userType == "01")
                        {
                            //更新員工信息表中對應的信息
                            EmpDAL empManage = new EmpDAL();
                            empManage.UpdateEmpEmailInfo(model.userNo, model.userName, model.userMail, t);
                        }
                        //修改員工基本信息
                        isResult = userRightManage.UpdateUserInfo(model, t);
                        //修改員工的權限列表
                        if (isResult)
                            isResult = ModUserRoseRight(model.userID, model.userType, RoseIdList, t);
                        //修改公司權限
                        if (isResult)
                            isResult = ModUserCompanyRight(model.userID, CompanyIdList, t);

                        //修改供應商權限
                        if (isResult)
                            isResult = ModUserSupplyRight(model.userID, SupplyIdList, t);

                        if (isResult)
                        {
                            t.Commit();
                            result = "success";
                        }
                        else
                        {
                            t.RollBack();
                            result = "fail";
                        }
                    }
                    catch
                    {
                        t.RollBack();
                        if (string.IsNullOrEmpty(result))
                            result = "fail";
                    }
                    finally
                    {
                        t.DbConnection.Close();
                        t.Dispose();
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 修改用戶權限
        /// </summary>
        /// <param name="UserId">用戶編號</param>
        /// <param name="UserType">用戶類別</param>
        /// <param name="RoseIdList">角色ID編號</param>
        /// <param name="t">事物</param>
        /// <returns>bool</returns>
        public bool ModUserRoseRight(string UserId, string UserType, List<string> RoseIdList, Trans t)
        {
            bool isResult = true;
            List<string> historyRoseIdList = new List<string>();
            DataTable dtRose = userRightManage.GetUserRose(UserId, "", t);
            foreach (DataRow dr in dtRose.Rows)
            {
                if (dr["Usy"].ToString() == "Y")
                    historyRoseIdList.Add(dr["RoseId"].ToString());
            }
            List<string> deleteRoseIdList = historyRoseIdList.FindAll(o => !RoseIdList.Contains(o));
            List<string> newRoseIdList = RoseIdList.FindAll(o => !historyRoseIdList.Contains(o));

            foreach (string RoseId in newRoseIdList)
            {
                if (!roseManage.ExistsRoseId(RoseId, t))
                {
                    isResult = false;
                    break;
                }
                isResult = userRightManage.AddUserRose(UserId, RoseId, t);
                if (!isResult)
                    break;
            }

            foreach (string RoseId in deleteRoseIdList)
            {
                isResult = userRightManage.DeleteUserRose(UserId, RoseId, t);
                if (!isResult)
                    break;
            }
            return isResult;
        }

        public bool SaveUserProgramCompanyRight(string UserId, string ProgramId, List<string> CompanyIdList)
        {
            List<string> historyCompanyIdList = new List<string>();
            DataTable dtCompany = companyManage.ReadProgramCompanyRight(UserId, ProgramId);
            foreach (DataRow dr in dtCompany.Rows)
            {
                if (dr["Usy"].ToString() == "Y")
                    historyCompanyIdList.Add(dr["companyID"].ToString());
            }
            List<string> addList = CompanyIdList.FindAll(o => !historyCompanyIdList.Contains(o));
            List<string> deleteList = historyCompanyIdList.FindAll(o => !CompanyIdList.Contains(o));
            bool isResult = true;
            foreach (string CompanyId in addList)
            {
                if (!companyManage.ExistsCompanyId(CompanyId))
                {
                    isResult = false;
                    break;
                }
                isResult = companyManage.AddUserProgramCompanyRight(UserId, ProgramId, CompanyId);
                if (!isResult)
                    break;
            }
            foreach (string CompanyId in deleteList)
            {
                isResult = companyManage.DeleteProgramCompanyRight(UserId, ProgramId, CompanyId);
                if (!isResult)
                    break;
            }
            return true;
        }

        public bool ModUserCompanyRight(string UserId, List<string> CompanyIdList, Trans t)
        {
            bool isResult = true;
            List<string> historyCompanyIdList = new List<string>();
            DataTable dtCompany = companyManage.ReadUserCompanyRight(UserId, t);
            foreach (DataRow dr in dtCompany.Rows)
            {
                historyCompanyIdList.Add(dr["CompId"].ToString());
            }
            List<string> addList = CompanyIdList.FindAll(o => !historyCompanyIdList.Contains(o));
            List<string> deleteList = historyCompanyIdList.FindAll(o => !CompanyIdList.Contains(o));
            foreach (string CompanyId in addList)
            {
                if (!companyManage.ExistsCompanyId(CompanyId, t))
                {
                    isResult = false;
                    break;
                }
                isResult = companyManage.AddUserCompanyRight(UserId, CompanyId, t);
                if (!isResult)
                    break;
            }
            foreach (string CompanyId in deleteList)
            {
                isResult = companyManage.DeleteUserCompanyRight(UserId, CompanyId);
                if (!isResult)
                    break;
            }
            return true;
        }

        public bool ModUserSupplyRight(string UserId, List<string> SupplyIdList, Trans t)
        {
            bool isResult = true;
            List<string> historyList = new List<string>();
            DataTable dtSupply = supplyManage.ReadSupplyByUser(UserId, t);
            foreach (DataRow dr in dtSupply.Rows)
            {
                if (dr["Usy"].ToString() == "Y")
                    historyList.Add(dr["suppNumber"].ToString());
            }
            List<string> deleteList = historyList.FindAll(o => !SupplyIdList.Contains(o));
            List<string> addList = SupplyIdList.FindAll(o => !historyList.Contains(o));
            foreach (string SupplyId in addList)
            {
                if (!supplyManage.ExistsSupplyId(SupplyId, t))
                {
                    isResult = false;
                    break;
                }
                isResult = supplyManage.AddSupplyByUser(UserId, SupplyId, t);
                if (!isResult)
                    break;
            }
            foreach (string SupplyId in deleteList)
            {
                isResult = supplyManage.DeleteSupplyRightByUser(UserId, t);
                if (!isResult)
                    break;
            }
            return isResult;
        }

        public string DeleteUserInfo(List<string> UserIdList)
        {
            string result = "";
            using (Trans t = new Trans())
            {
                try
                {
                    bool isResult = true;
                    foreach (string UserId in UserIdList)
                    {
                        if (!userRightManage.ExistsUserId(UserId, t))
                        {
                            result = "EU0009";
                            isResult = false;
                            break;
                        }
                        isResult = userRightManage.DeleteUserInfo(UserId, t);
                        if (!isResult)
                            break;
                    }
                    if (isResult)
                    {
                        t.Commit();
                        result = "success";
                    }
                    else
                    {
                        t.RollBack();
                        result = "fail";
                    }
                }
                catch
                {
                    t.RollBack();
                    if (string.IsNullOrEmpty(result))
                        result = "fail";
                }
                finally
                {
                    t.DbConnection.Close();
                    t.Dispose();
                }
            }
            return result;
        }

        /// <summary>
        /// 重置密碼
        /// </summary>
        /// <param name="UserId">用戶ID</param>
        /// <returns>結果</returns>
        public string ResetPwd(string UserId, out string pwd)
        {
            pwd = GenString();
            if (!userRightManage.ExistsUserId(UserId))
            {
                return "EU0009";
            }
            string savepwd = EnCryptPassword(pwd);
            if (userRightManage.ResetPwd(UserId, savepwd))
            {
                return "success";
            }
            else
            {
                return "fail";
            }
        }
        /// <summary>
        /// 獲取域名服務器
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetDomainServer()
        {
            return userRightManage.GetDomainServer();
        }

        static string[] chars = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q",
            "r", "s", "t", "u", "v", "w", "x", "y", "z",
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q",
            "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        public static string GenString()
        {
            int len = chars.Length;
            System.Random r = new System.Random();
            string pwd = string.Format("{0}{1}{2}{3}{4}{5}",
                chars[r.Next(0, len)],
                chars[r.Next(0, len)],
                chars[r.Next(0, len)],
                chars[r.Next(0, len)],
                chars[r.Next(0, len)],
                chars[r.Next(0, len)]);
            return pwd;
        }
        /// <summary>
        /// 密碼加密
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string EnCryptPassword(string strSource)
        {
            byte[] dataToHash = (new System.Text.ASCIIEncoding()).GetBytes(strSource);
            byte[] hashvalue = (System.Security.Cryptography.CryptoConfig.CreateFromName("MD5") as System.Security.Cryptography.HashAlgorithm).ComputeHash(dataToHash);
            int i;
            string result = string.Empty;
            for (i = 4; i < 11; i++)
            {
                result += Microsoft.VisualBasic.Conversion.Hex(hashvalue[i]).ToLower();
            }
            return result;
        } 
    }
}
