using System.Collections.Generic;
using System.Text;
using System.Data;
using DMC.Model;
using DMC.DAL;

namespace DMC.BLL
{
    public class SysParaSettingService
    {
        public static Dictionary<string, object> ParaCache = new Dictionary<string, object>();

        SysParaSettingDAL _dal = new SysParaSettingDAL();
        /// <summary>
        /// 新增系統參數
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string AddSysPara(SysParaSettingModel model)
        {
            string result = "";
            if (string.IsNullOrEmpty(model.CompanyId))
                result = "SPS001";//公司編號不能為空 
            else if (string.IsNullOrEmpty(model.ParaKey))
                result = "SPS002";//參數編號不能為空
            else if (string.IsNullOrEmpty(model.ParaName))
                result = "SPS003";//參數名稱不能為空
            else if (string.IsNullOrEmpty(model.ParaContent))
                result = "SPS004";//參數內容不能為空
            else if (_dal.ExistsPara(model.CompanyId, model.ParaKey).Rows.Count > 0)
                result = "SPS005";//參數已存在，請勿進行重複添加

            try
            {
                if (string.IsNullOrEmpty(result))
                {
                    if (_dal.AddSysPara(model))
                    {
                        result = "SPS006";//保存成功
                    }
                    else
                    {
                        result = "SPS007";//保存失敗
                    }
                }

            }
            catch
            {
                result = "SPS007";//保存失敗
            }
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ModSysPara(SysParaSettingModel model)
        {
            string result = "";
            if (string.IsNullOrEmpty(model.CompanyId))
                result = "SPS001";//公司編號不能為空 
            else if (string.IsNullOrEmpty(model.ParaKey))
                result = "SPS002";//參數編號不能為空
            else if (string.IsNullOrEmpty(model.ParaName))
                result = "SPS003";//參數名稱不能為空
            else if (string.IsNullOrEmpty(model.ParaContent))
                result = "SPS004";//參數內容不能為空
            else if (_dal.ExistsPara(model.CompanyId, model.ParaKey).Rows.Count <= 0)
                result = "SPS008";//參數已不存在，修改失敗

            try
            {
                if (string.IsNullOrEmpty(result))
                {
                    if (_dal.EditSysPara(model))
                    {
                        result = "SPS006";//保存成功
                        //修改時，如果存在緩存，則更新緩存
                        ParaCache.Clear();//有更新则情况缓存，重新加载
                    }
                    else
                    {
                        result = "SPS007";//保存失敗
                    }
                }
            }
            catch
            {
                result = "SPS007";//保存失敗
            }
            return result;
        }

        /// <summary>
        /// 刪除系統參數
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DelSysPara(string CompanyId, string ParaKey)
        {
            return _dal.DelSysPara(CompanyId, ParaKey);
        }

        /// <summary>
        /// 獲取參數的值
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="ParaKey"></param>
        /// <returns></returns>
        public string GetParaValue(string CompanyId, string ParaKey)
        {
            string result = "";
            string key = CompanyId + ParaKey;
            if (ParaCache.ContainsKey(key))
            {
                result = ParaCache[key].ToString();
            }
            else
            {
                DataTable dt = _dal.ExistsPara(CompanyId, ParaKey);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["ParaContent"].ToString();
                }
                ParaCache[key] = result;
            }
            return result;
        }


        /// <summary>
        /// 按條件搜索訂變資料數據
        /// </summary>
        /// <param name="InputDefaultKey">默認關鍵字</param>
        /// <param name="total">總計多少條</param>
        /// <param name="pageCount">總計多少页碼</param>
        /// <param name="model">查詢條件實體</param>
        /// <returns>DataTable</returns>        
        public DataTable Search(out int total, out int pageCount, dynamic model)
        {
            pageCount = 0;
            total = 0;
            DataTable dt = null;
            StringBuilder sb = new StringBuilder();
            string strWhere = string.Empty;
            string strFields = string.Empty;
            string strTable = string.Empty;
            if (model != null)
            {
                string type = model.type;
                switch (type)
                {
                    case "ByKey":
                        string key = model.KeyWord;
                        if (!string.IsNullOrEmpty(key))
                        {
                            strWhere += string.Format("CompanyID like N'%{0}%' or ParaKey like N'%{0}%' or ParaName like N'%{0}%' or ParaContent like N'%{0}%' ", key);

                        }
                        break;
                }
                strFields += " * ";
                strTable = " t_SysParaSetting ";
                PageManage newPageManage = new PageManage();
                return newPageManage.PageView(strTable, "AutoID", model.page, model.rows, strFields, " AutoID desc", strWhere, out total, out pageCount);
            }
            return dt;
        }
    }
}
