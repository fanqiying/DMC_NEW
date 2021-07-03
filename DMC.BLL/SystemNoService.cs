using System.Collections.Generic;
using DMC.DAL;
using System.Data;
using DMC.Model;
using System.Web.SessionState;

namespace DMC.BLL
{
    /// <summary>
    /// 系統單據編號管理
    /// </summary>
    public class SystemNoService : IRequiresSessionState
    { 
        private PageManage pageView = new PageManage();
        private SystemNoDAL _dal = new SystemNoDAL();

        /// <summary>
        /// 分页搜索獲取所有的系統單號列表
        /// </summary>
        /// <param name="pageSize">每页顯示多少條</param>
        /// <param name="pageIndex">页碼索引</param>
        /// <param name="pageCount">總共多少页</param>
        /// <param name="total">總記錄數</param>
        /// <param name="Where">查詢條件</param>
        /// <returns>DataTable</returns>        
        public DataTable Search(int pageSize, int pageIndex, out int pageCount, out int total, string Where = "")
        {
            string showField = "AutoID,CompanyId,ModuleType,ModularType,Category,ReceiptType,keyword,";
            showField += "DateType,CodeLen,Mark,Usy,CreateUserId as createrid,CreateTime,";
            showField += "CreateDeptId as cdeptid,UpdateUserId as updaterid,UpdateDeptId as udeptid,";
            showField += "UpdateTime as lastmodtime,CurrCode,GeneratTime";
            return pageView.PageView("t_SystemNo", "AutoID", pageIndex, pageSize, showField, "AutoId ASC", Where, out total, out pageCount);
        }

        /// <summary>
        /// 獲取程式對應的單據別列表
        /// </summary>
        /// <param name="ProgramId">程式Id</param>
        /// <returns>DataTable</returns>
        public DataTable GetReceiptList(string ProgramId)
        {
            return _dal.GetReceiptList(ProgramId);
        }
        /// <summary>
        /// 添加單據別維護
        /// </summary>
        /// <param name="model">實體</param>
        /// <returns>string</returns>
        public string AddSystemNo(t_SystemNo model)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(model.CompanyId))
            {
                result = "ESN001";//請選擇公司別
            }
            else if (string.IsNullOrEmpty(model.ModuleType))
            {
                result = "ESN002";//請選擇模塊
            }
            else if (string.IsNullOrEmpty(model.ModularType))
            {
                result = "ESN003";//請選擇模組
            }
            else if (string.IsNullOrEmpty(model.Category))
            {
                result = "ESN004";//請選擇類別
            }
            else if (string.IsNullOrEmpty(model.ReceiptType))
            {
                result = "ESN005";//請選擇單據別
            }
            else if (string.IsNullOrEmpty(model.keyword))
            {
                result = "ESN006";//請選擇關鍵字
            }
            else if (string.IsNullOrEmpty(model.DateType))
            {
                result = "ESN011";//請選擇公司別
            }
            else if (model.CodeLen == null || model.CodeLen == 0)
            {
                result = "ESN007";//請設置流水碼的長度
            }
            else if (_dal.IsExitt_SystemNo(model))
            {
                result = "ESN008";//單據別已存在
            }
            else if (_dal.IsRepeatRule(model))
            {
                result = "ESN012";
            }

            if (string.IsNullOrEmpty(result))
            {
                if (_dal.Addt_SystemNo(model))
                {
                    result = "success";
                }
                else
                {
                    result = "fail";
                }
            }
            return result;
        }
        /// <summary>
        /// 修改單據別對應的信息
        /// </summary>
        /// <param name="model">實體</param>
        /// <returns>string</returns>
        public string ModSystemNo(t_SystemNo model)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(model.CompanyId))
            {
                result = "ESN001";//請選擇公司別
            }
            else if (string.IsNullOrEmpty(model.ModuleType))
            {
                result = "ESN002";//請選擇模塊
            }
            else if (string.IsNullOrEmpty(model.ModularType))
            {
                result = "ESN003";//請選擇模組
            }
            else if (string.IsNullOrEmpty(model.Category))
            {
                result = "ESN004";//請選擇類別
            }
            else if (string.IsNullOrEmpty(model.ReceiptType))
            {
                result = "ESN005";//請選擇單據別
            }
            else if (string.IsNullOrEmpty(model.keyword))
            {
                result = "ESN006";//請選擇關鍵字
            }
            else if (string.IsNullOrEmpty(model.DateType))
            {
                result = "ESN011";//請選擇公司別
            }
            else if (model.CodeLen == null || model.CodeLen == 0)
            {
                result = "ESN007";//請設置流水碼的長度
            }
            else if (!_dal.IsExitt_SystemNo(model))
            {
                result = "ESN009";//單據別不存在
            }
            else if (model.AutoID == 0)
            {
                result = "ESN010";//未指定需要修改的數據
            }
            else if (_dal.IsRepeatRule(model))
            {
                result = "ESN012";
            }

            if (string.IsNullOrEmpty(result))
            {
                if (_dal.Modt_SystemNo(model))
                {
                    result = "success";
                }
                else
                {
                    result = "fail";
                }
            }
            return result;
        }

        /// <summary>
        /// 刪除單據編號
        /// </summary>
        /// <param name="ids">單據編號</param>
        /// <returns>string</returns>
        public string DelSystemNo(List<string> ids)
        {
            string result = string.Empty;
            if (ids != null && ids.Count > 0)
            {
                using (Trans t = new Trans())
                {
                    try
                    {
                        bool isResult = true;
                        foreach (string autoId in ids)
                        {
                            isResult = _dal.Delt_SystemNo(autoId, t);
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
                        result = "fail";
                    }
                }
            }
            else
            {
                result = "EL0005";//請設置需要刪除的數據
            }

            return result;
        }

        /// <summary>
        /// 獲取系統單號
        /// </summary>
        /// <param name="ReceiptType">單據類別</param>
        /// <param name="keyword">關鍵字</param>
        /// <returns>string</returns>
        public string GetSysNo(string keyword)
        {
            string CompanyId = string.Empty;
            if (System.Web.HttpContext.Current != null)
            {
                //獲取當前用戶
                object obj = System.Web.HttpContext.Current.Session["UserMain"];
                if (obj != null)
                {
                    UserInfo userInfo = (UserInfo)obj;
                    CompanyId = userInfo.Company.companyID;
                }
            }

            return _dal.GetSysNoNew(CompanyId, keyword);
        }


        /// <summary>
        /// 獲取系統單號
        /// </summary>
        /// <param name="ReceiptType">單據類別</param>
        /// <param name="keyword">關鍵字</param>
        /// <returns>string</returns>
        public string GetSysNoWebService(string keyword, string CompanyId)
        {
            return _dal.GetSysNoNew(CompanyId, keyword);
        }


        /// <summary>
        /// 獲取全局單號
        /// </summary>
        /// <param name="ReceiptType">單據類別</param>
        /// <param name="keyword">關鍵字</param>
        /// <returns>單號</returns>
        public string GetGlobelNo(string ReceiptType, string keyword, string SplitStr = "-")
        {
            return _dal.GetGlobelNo(ReceiptType, keyword, SplitStr);
        }
        /// <summary>
        /// 根據單據別和關鍵字，產生單流水單號
        /// </summary>
        /// <param name="ReceiptType">單據類別</param>
        /// <param name="keyword">關鍵字</param>
        /// <returns>string</returns>
        public string GetSerialNo(string CompanyId, string keyword)
        {
            return _dal.GetSysNo(CompanyId, keyword);
        }
        /// <summary>
        /// 根据类别获取流水码
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="Category"></param>
        /// <param name="Keyword"></param>
        /// <param name="SplitStr"></param>
        /// <returns></returns>
        public string GetCategoryNo(string CompanyId, string Category, string Keyword, string SplitStr = "-")
        {
            return _dal.GetCategoryNo(CompanyId, Category, Keyword, SplitStr);
        }

        public string GetCategoryNoByThread(string CompanyId, string Category, string Keyword, string SplitStr = "-")
        {
            return _dal.GetCategoryNoByThread(CompanyId, Category, Keyword, SplitStr);
        }
    }
}
