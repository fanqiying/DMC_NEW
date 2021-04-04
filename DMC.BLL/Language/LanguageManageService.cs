using System;
using System.Collections.Generic;
using System.Data;
using DMC.DAL;
using DMC.Model;
using Utility.HelpClass;
namespace DMC.BLL
{
    /// <summary>
    /// 多語言配置管理
    /// 處理多語言業務相關操作
    /// code by klint
    /// 2013-7-2
    /// </summary>
    public class LanguageManageService
    {
        private LanguageManageDAL languageManage = new LanguageManageDAL();
        private PageManage pageView = new PageManage();
        /// <summary>
        /// 資源文件分页搜索
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="total"></param>
        /// <param name="Where"></param>
        /// <returns></returns>        
        public DataTable Search(int pageSize, int pageIndex, out int pageCount, out int total, string Where = "")
        {
            return pageView.PageView("t_Language_Resources", "ResourceId", pageIndex, pageSize, "*", "AutoId ASC", Where, out total, out pageCount);
        }

        /// <summary>
        /// 添加新增國際化字段
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string CreateLanguage(t_Language_Resources model, List<LangValue> valueList)
        {
            if (string.IsNullOrEmpty(model.ResourceId))
                return "EL0001";//資源編號必須輸入
            if (string.IsNullOrEmpty(model.ResourceType))
                return "EL0002";//必須資源類別
            if (string.IsNullOrEmpty(model.DefaultValue))
                return "EL0003";//必須設置默認值
            //1、檢查是否重複
            if (ExistKey(model.ResourceId))
                return "EL0004";//語言編號已存在
            //2、檢查默認值是否存在
            //if (languageManage.ExistsDefaultValue(model.DefaultValue))
            //    return "EL0010";//默認值已設定
            //檢查是否超過長度
            if (model.ResourceId.Length > 20)
                return "EL0011";

            if (languageManage.CreateLanguage(model, valueList))
            {
                return "success";//新增語言成功
            }
            else
            {
                return "fail";//新增語言失敗
            }
        }

        /// <summary>
        /// 檢查編號是否已存在
        /// </summary>
        /// <param name="ResourceId"></param>
        /// <returns></returns>
        public bool ExistKey(string ResourceId)
        {
            if (string.IsNullOrEmpty(ResourceId))
                return false;
            return languageManage.ExistKey(ResourceId);
        }

        /// <summary>
        /// 多語言刪除
        /// </summary>
        /// <param name="keyIdList"></param>
        /// <returns></returns>
        public string DeleteLanguage(List<string> keyIdList)
        {
            if (keyIdList == null || keyIdList.Count < 1)
                return "EL0005";//請選擇需要刪除的列

            if (languageManage.DeleteLanguageByKey(keyIdList))
            {
                return "success";//刪除成功
            }
            else
            {
                return "fail";//刪除失敗
            }
        }

        /// <summary>
        /// 通過自增列刪除資源語言
        /// </summary>
        /// <param name="autoIdList">自增Id列表</param>
        /// <returns></returns>
        public string DeleteLanguageByAutoId(List<Int32> autoIdList)
        {
            if (autoIdList == null || autoIdList.Count < 1)
                return "EL0005";//請選擇需要刪除的列

            if (languageManage.DeleteLanguageByAutoId(autoIdList))
            {
                return "success";//刪除成功
            }
            else
            {
                return "fail";//刪除成功
            }
        }

        /// <summary>
        /// 多語言修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="valueList"></param>
        /// <returns></returns>
        public string ModifyLanguage(t_Language_Resources model, List<LangValue> valueList)
        {
            if (string.IsNullOrEmpty(model.ResourceId))
                return "EL0001";//資源編號必須輸入
            if (string.IsNullOrEmpty(model.ResourceType))
                return "EL0002";//必須選擇語言類別
            if (string.IsNullOrEmpty(model.DefaultValue))
                return "EL0003";//必須設置默認值
            if (!ExistKey(model.ResourceId))
                return "EL0009";//資源已失效，請重新刷新數據
            //2、檢查默認值是否存在
            //if (languageManage.ExistsDefaultValue(model.DefaultValue, model.ResourceId))
            //    return "EL0010";//默認值已設定

            if (languageManage.ModifyLanguage(model, valueList))
            {
                return "success";//修改語言成功
            }
            else
            {
                return "fail";//修改語言失敗
            }
        }

        /// <summary>
        /// 添加語言類別
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string AddLanguageType(t_Language_Type model)
        {
            if (string.IsNullOrEmpty(model.LanguageId))
                return "EL0006";//請設置語言編號
            if (string.IsNullOrEmpty(model.LanguageName))
                return "EL0007";//請設置語言名稱
            //默認設置為有效
            if (string.IsNullOrEmpty(model.Usy))
                model.Usy = "Y";
            //檢查資源編號是否存在
            if (languageManage.ExistsLanguageId(model.LanguageId))
                return "EL0008";//語言編號已存在，請重新設置
            if (languageManage.AddLanguageType(model))
            {
                return "success";//新增語言類別成功
            }
            else
            {
                return "fail";//新增語言類別失敗
            }
        }

        /// <summary>
        /// 刪除語言類別
        /// </summary>
        /// <param name="LanguageIdList"></param>
        /// <returns></returns>
        public string DeleteLanguageTypeByKey(List<string> LanguageIdList)
        {
            if (LanguageIdList == null || LanguageIdList.Count < 1)
                return "EL0005";//請選擇需要刪除的列

            if (languageManage.DeleteLanguageTypeByKey(LanguageIdList))
            {
                return "success";//刪除成功
            }
            else
            {
                return "fail";//刪除失敗
            }
        }
        /// <summary>
        /// 刪除所有語言類別
        /// </summary>
        public void DeleteAllLanguageType()
        {
            DataTable dt = GetAllLanguageType();
            List<string> LanguageIdList = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                LanguageIdList.Add(dr["LanguageId"].ToString());
            }
            DeleteLanguageTypeByKey(LanguageIdList);
        }
        /// <summary>
        /// 修改語言別
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string UpdateLanguageType(t_Language_Type model)
        {
            if (string.IsNullOrEmpty(model.LanguageId))
                return "EL0006";//請設置語言編號
            if (string.IsNullOrEmpty(model.LanguageName))
                return "EL0007";//請設置語言名稱
            //默認設置為有效
            if (string.IsNullOrEmpty(model.Usy))
                model.Usy = "Y";
            //檢查資源編號是否存在
            if (!languageManage.ExistsLanguageId(model.LanguageId))
                return "EL0009";//資源已失效，請重新刷新數據

            if (languageManage.UpdateLanguageType(model))
            {
                return "success";//新增語言類別成功
            }
            else
            {
                return "fail";//新增語言類別失敗
            }
        }

        /// <summary>
        /// 是否存在語言類別Id
        /// </summary>
        /// <param name="LanguageId"></param>
        /// <returns></returns>
        public bool ExistsLanguageId(string LanguageId)
        {
            if (string.IsNullOrEmpty(LanguageId))
                return false;
            return languageManage.ExistsLanguageId(LanguageId);
        }

        /// <summary>
        /// 獲取所有語言類別
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllLanguageType()
        {
            return languageManage.GetAllLanguageType();
        }

        /// <summary>
        /// 返回多語言的值
        /// </summary>
        /// <param name="ResourceId">資源Id</param>
        /// <param name="LanguageId">語言別</param>
        /// <returns></returns>
        public string GetResourceText(string ResourceId, string LanguageId)
        {
            return languageManage.GetResourceText(ResourceId, LanguageId);
        }
        /// <summary>
        /// 獲取程序對應的資源
        /// </summary>
        /// <param name="ProgramId"></param>
        /// <param name="LanguageId"></param>
        /// <param name="ResourceIdList"></param>
        /// <returns></returns>
        public List<LanguageKeyValue> GetResourcesByProgram(string ProgramId, string LanguageId, List<string> ResourceIdList)
        {
            List<LanguageKeyValue> result = (List<LanguageKeyValue>)Cache.GetCache(ProgramId + LanguageId);
            if (result == null)
            {
                result = GetResourceByKey(LanguageId, ResourceIdList);
                Cache.SetCache(ProgramId + LanguageId, null, 24 * 60 * 60);
            }
            return result;
        }

        /// <summary>
        /// 返回多語言下拉框的值
        /// </summary>
        /// <param name="GroupKey">資源Id</param>
        /// <param name="LanguageId">語言別</param>
        /// <returns></returns>
        public DataTable GetResourceSelect(string GroupKey, string LanguageId)
        {
            return languageManage.GetResourceSelect(GroupKey, LanguageId);
        }

        /// <summary>
        /// 獲取多語言的值
        /// </summary>
        /// <param name="ResourceId"></param>
        /// <returns></returns>
        public DataTable GetResourceValues(string ResourceId)
        {
            return languageManage.GetResourceValues(ResourceId);
        }

        /// <summary>
        /// 設置國際化
        /// </summary>
        public void GenerateLanguageBit()
        {
            Cache.ClearAll();
            //獲取所有的語言類別
            DataTable dt = GetAllLanguageType();
            foreach (DataRow dr in dt.Rows)
            {
                string key = "Res" + dr["LanguageId"].ToString();
                if (Cache.Exists(key))
                    Cache.Remove(key);
                List<LanguageKeyValue> list = new List<LanguageKeyValue>();
                DataTable languageBit = languageManage.GetResourceByType(dr["LanguageId"].ToString());
                foreach (DataRow item in languageBit.Rows)
                {
                    LanguageKeyValue res = new LanguageKeyValue();
                    res.Key = item["LanguageKey"].ToString();
                    res.Value = item["LanguageValue"].ToString();
                    list.Add(res);
                }
                Cache.SetCache(key, list, 24 * 60 * 60);
            }
        }
        /// <summary>
        /// 獲取指定範圍內的資源
        /// </summary>
        /// <param name="LanguageId"></param>
        /// <param name="keyList"></param>
        /// <returns></returns>
        public List<LanguageKeyValue> GetResourceByKey(string LanguageId, List<string> keyList)
        {
            string key = "Res" + LanguageId;
            List<LanguageKeyValue> list = new List<LanguageKeyValue>();

            if (keyList == null || keyList.Count <= 0)
                return list;

            if (!Cache.Exists(key))
            {
                DataTable languageBit = languageManage.GetResourceByType(LanguageId);
                foreach (DataRow item in languageBit.Rows)
                {
                    LanguageKeyValue res = new LanguageKeyValue();
                    res.Key = item["LanguageKey"].ToString();
                    res.Value = item["LanguageValue"].ToString();
                    res.LanguageId = LanguageId;
                    list.Add(res);
                }
                Cache.SetCache(key, list, 24 * 60 * 60);
            }
            else
            {
                list = (List<LanguageKeyValue>)Cache.GetCache(key);
            }
            return list.FindAll(o => keyList.Contains(o.Key));
        }
    }
}
