using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using DMC.DAL.Script;
using DMC.Model;
namespace DMC.DAL
{
    /// <summary>
    /// 多語言配置管理
    /// </summary>
    public class LanguageManageDAL
    {
        private ILanguage script = ScriptFactory.GetScript<ILanguage>(); 

        /// <summary>
        /// 添加新增國際化字段
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CreateLanguage(t_Language_Resources model, List<LangValue> valueList)
        {
            //參數設置
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("ResourceId", System.Data.DbType.String, model.ResourceId, 20));
            param.Add(DBFactory.Helper.FormatParameter("ResourceType", System.Data.DbType.String, model.ResourceType, 2));
            param.Add(DBFactory.Helper.FormatParameter("DefaultValue", System.Data.DbType.String, model.DefaultValue, 255));
            param.Add(DBFactory.Helper.FormatParameter("GroupKey", System.Data.DbType.String, model.GroupKey, 20));
            param.Add(DBFactory.Helper.FormatParameter("GroupValue", System.Data.DbType.String, model.GroupValue, 20));
            param.Add(DBFactory.Helper.FormatParameter("Usy", System.Data.DbType.String, model.Usy, 1));
            param.Add(DBFactory.Helper.FormatParameter("CreateDeptId", System.Data.DbType.String, model.CreateDeptId, 20));
            param.Add(DBFactory.Helper.FormatParameter("CreateUser", System.Data.DbType.String, model.CreateUser, 20));
            param.Add(DBFactory.Helper.FormatParameter("CreateTime", System.Data.DbType.DateTime, System.DateTime.Now));

            using (Trans t = new Trans())
            {
                try
                {
                    //添加資源基本屬性
                    int result = DBFactory.Helper.ExecuteNonQuery(script.CreateLanguageResources, param.ToArray(), t);
                    if (result != 1)
                        t.RollBack();
                    else
                    {
                        //添加各種國際化的值
                        if (valueList != null && valueList.Count > 0)
                            foreach (LangValue value in valueList)
                            {
                                try
                                {
                                    List<DbParameter> para = new List<DbParameter>();
                                    para.Add(DBFactory.Helper.FormatParameter("ResourceId", System.Data.DbType.String, value.ResourceId, 20));
                                    para.Add(DBFactory.Helper.FormatParameter("LanguageId", System.Data.DbType.String, value.LanguageId, 20));
                                    para.Add(DBFactory.Helper.FormatParameter("DisplayValue", System.Data.DbType.String, value.DisplayValue, 255));
                                    result = DBFactory.Helper.ExecuteNonQuery(script.CreateLanguageValue, para.ToArray(), t);
                                    if (result != 1)
                                    {
                                        t.RollBack();
                                        break;
                                    }
                                }
                                catch
                                {
                                    t.RollBack();
                                    result = 0;
                                }
                            }
                    }
                    if (result == 1)
                        t.Commit();

                    return result == 1;
                }
                finally
                {
                    t.DbConnection.Close();
                    t.Dispose();
                }
            }
        }
        /// <summary>
        /// 檢查編號是否已存在
        /// </summary>
        /// <param name="ResourceId"></param>
        /// <returns></returns>
        public bool ExistKey(string ResourceId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("ResourceId", System.Data.DbType.String, ResourceId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.ExistKey, param.ToArray()));
            return result != 0;
        }
        /// <summary>
        /// 編輯語言記錄
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyLanguage(t_Language_Resources model, List<LangValue> valueList)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("ResourceId", System.Data.DbType.String, model.ResourceId, 20));
            param.Add(DBFactory.Helper.FormatParameter("ResourceType", System.Data.DbType.String, model.ResourceType, 2));
            param.Add(DBFactory.Helper.FormatParameter("DefaultValue", System.Data.DbType.String, model.DefaultValue, 255));
            param.Add(DBFactory.Helper.FormatParameter("GroupKey", System.Data.DbType.String, model.GroupKey, 20));
            param.Add(DBFactory.Helper.FormatParameter("GroupValue", System.Data.DbType.String, model.GroupValue, 20));
            param.Add(DBFactory.Helper.FormatParameter("Usy", System.Data.DbType.String, model.Usy, 1));
            param.Add(DBFactory.Helper.FormatParameter("UpdateDeptId", System.Data.DbType.String, model.UpdateDeptId, 20));
            param.Add(DBFactory.Helper.FormatParameter("UpdateUser", System.Data.DbType.String, model.UpdateUser, 20));
            param.Add(DBFactory.Helper.FormatParameter("UpdateTime", System.Data.DbType.DateTime, System.DateTime.Now));

            using (Trans t = new Trans())
            {
                try
                {
                    int result = DBFactory.Helper.ExecuteNonQuery(script.ModifyLanguage, param.ToArray());
                    if (result == 1)
                    {
                        List<DbParameter> clearParam = new List<DbParameter>();
                        clearParam.Add(DBFactory.Helper.FormatParameter("ResourceId", System.Data.DbType.String, model.ResourceId, 20));
                        DBFactory.Helper.ExecuteNonQuery(script.ClearLanguageValue, clearParam.ToArray());
                        if (valueList != null && valueList.Count > 0)
                            foreach (LangValue value in valueList)
                            {
                                List<DbParameter> para = new List<DbParameter>();
                                para.Add(DBFactory.Helper.FormatParameter("ResourceId", System.Data.DbType.String, value.ResourceId, 20));
                                para.Add(DBFactory.Helper.FormatParameter("LanguageId", System.Data.DbType.String, value.LanguageId, 20));
                                para.Add(DBFactory.Helper.FormatParameter("DisplayValue", System.Data.DbType.String, value.DisplayValue, 255));
                                DBFactory.Helper.ExecuteNonQuery(script.CreateLanguageValue, para.ToArray(), t);
                            }
                    }
                    t.Commit();
                    return true;
                }
                catch
                {
                    t.RollBack();
                    return false;
                }
                finally
                {
                    t.DbConnection.Close();
                    t.Dispose();
                }
            }
        }

        /// <summary>
        /// 刪除國際化編號信息
        /// </summary>
        /// <param name="ResourceId"></param>
        /// <returns></returns>
        public bool DeleteLanguageByKey(List<string> ResourceIdList)
        {
            using (Trans t = new Trans())
            {
                try
                {
                    foreach (string ResourceId in ResourceIdList)
                    {
                        List<DbParameter> param = new List<DbParameter>();
                        param.Add(DBFactory.Helper.FormatParameter("ResourceId", System.Data.DbType.String, ResourceId, 20));
                        DBFactory.Helper.ExecuteScalar(script.DeleteLanguageByKey, param.ToArray());
                    }
                    t.Commit();
                    return true;
                }
                catch
                {
                    t.RollBack();
                    return false;
                }
                finally
                {
                    t.DbConnection.Close();
                    t.Dispose();
                }
            }
        }

        /// <summary>
        /// 通過autoid進行數據刪除
        /// </summary>
        /// <param name="autoId"></param>
        /// <returns></returns>
        public bool DeleteLanguageByAutoId(List<int> autoIdList)
        {
            using (Trans t = new Trans())
            {
                try
                {
                    foreach (int autoId in autoIdList)
                    {
                        List<DbParameter> param = new List<DbParameter>();
                        param.Add(DBFactory.Helper.FormatParameter("AutoId", System.Data.DbType.String, autoId));
                        DBFactory.Helper.ExecuteScalar(script.DeleteLanguageByKey, param.ToArray());
                    }
                    t.Commit();
                    return true;
                }
                catch
                {
                    t.RollBack();
                    return false;
                }
                finally
                {
                    t.DbConnection.Close();
                    t.Dispose();
                }
            }

        }

        /// <summary>
        /// 添加語言類別
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddLanguageType(t_Language_Type model)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("LanguageId", System.Data.DbType.String, model.LanguageId, 20));
            param.Add(DBFactory.Helper.FormatParameter("LanguageName", System.Data.DbType.String, model.LanguageName, 20));
            param.Add(DBFactory.Helper.FormatParameter("Usy", System.Data.DbType.String, model.Usy, 1));
            param.Add(DBFactory.Helper.FormatParameter("Description", System.Data.DbType.String, model.Description, 20));
            param.Add(DBFactory.Helper.FormatParameter("CreateDeptId", System.Data.DbType.String, model.CreateDeptId, 20));
            param.Add(DBFactory.Helper.FormatParameter("CreateUser", System.Data.DbType.String, model.CreateUser, 20));
            param.Add(DBFactory.Helper.FormatParameter("CreateTime", System.Data.DbType.DateTime, System.DateTime.Now));
            //添加語言類別
            int result = DBFactory.Helper.ExecuteNonQuery(script.AddLanguageType, param.ToArray());
            return result == 1;
        }
        /// <summary>
        /// 刪除語言類別
        /// </summary>
        /// <param name="LanguageIdList"></param>
        /// <returns></returns>
        public bool DeleteLanguageTypeByKey(List<string> LanguageIdList)
        {
            using (Trans t = new Trans())
            {
                try
                {
                    foreach (string LanguageId in LanguageIdList)
                    {
                        List<DbParameter> param = new List<DbParameter>();
                        param.Add(DBFactory.Helper.FormatParameter("LanguageId", System.Data.DbType.String, LanguageId, 20));
                        DBFactory.Helper.ExecuteScalar(script.DeleteLanguageType, param.ToArray());
                    }
                    t.Commit();
                    return true;
                }
                catch
                {
                    t.RollBack();
                    return false;
                }
                finally
                {
                    t.DbConnection.Close();
                    t.Dispose();
                }
            }
        }

        /// <summary>
        /// 修改語言類別
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateLanguageType(t_Language_Type model)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("LanguageId", System.Data.DbType.String, model.LanguageId, 20));
            param.Add(DBFactory.Helper.FormatParameter("LanguageName", System.Data.DbType.String, model.LanguageName, 20));
            param.Add(DBFactory.Helper.FormatParameter("Usy", System.Data.DbType.String, model.Usy, 1));
            param.Add(DBFactory.Helper.FormatParameter("Description", System.Data.DbType.String, model.Description, 20));
            param.Add(DBFactory.Helper.FormatParameter("UpdateDeptId", System.Data.DbType.String, model.UpdateDeptId, 20));
            param.Add(DBFactory.Helper.FormatParameter("UpdateUser", System.Data.DbType.String, model.UpdateUser, 20));
            param.Add(DBFactory.Helper.FormatParameter("UpdateTime", System.Data.DbType.DateTime, System.DateTime.Now));
            //添加語言類別
            int result = DBFactory.Helper.ExecuteNonQuery(script.UpdateLanguageType, param.ToArray());
            return result == 1;
        }
        /// <summary>
        /// 獲取所有的語言類別
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllLanguageType()
        {
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.GetAllLanguageType, null).Tables[0];
            return dt;
        }
        /// <summary>
        /// 是否存在語言類別Id
        /// </summary>
        /// <param name="LanguageId"></param>
        /// <returns></returns>
        public bool ExistsLanguageId(string LanguageId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("LanguageId", System.Data.DbType.String, LanguageId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.ExistsLanguageType, param.ToArray()));
            return result != 0;
        }
        /// <summary>
        /// 檢查默認值是否存在
        /// </summary>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public bool ExistsDefaultValue(string DefaultValue, string ResourceId = "")
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("DefaultValue", System.Data.DbType.String, DefaultValue, 255));
            param.Add(DBFactory.Helper.FormatParameter("ResourceId", System.Data.DbType.String, ResourceId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.ExistsDefaultValue, param.ToArray()));
            return result != 0;
        }

        /// <summary>
        /// 根據語言類別獲取對應的多語言值
        /// </summary>
        /// <param name="ResourceId"></param>
        /// <param name="LanguageId"></param>
        /// <returns></returns>
        public string GetResourceText(string ResourceId, string LanguageId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("ResourceId", System.Data.DbType.String, ResourceId, 20));
            param.Add(DBFactory.Helper.FormatParameter("LanguageId", System.Data.DbType.String, LanguageId, 20));
            return Convert.ToString(DBFactory.Helper.ExecuteScalar(script.GetResourceText, param.ToArray()));
        }
        /// <summary>
        /// 根據組的標示，獲取多語言下拉框的值
        /// </summary>
        /// <param name="GroupKey"></param>
        /// <param name="LanguageId"></param>
        /// <returns>
        /// DataTable{Key,Text}
        /// </returns>
        public DataTable GetResourceSelect(string GroupKey, string LanguageId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("GroupKey", System.Data.DbType.String, GroupKey, 128));
            param.Add(DBFactory.Helper.FormatParameter("LanguageId", System.Data.DbType.String, LanguageId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.GetResourceSelect, param.ToArray()).Tables[0];
			if (GroupKey != "RemitBank") {
				DataRow dr = dt.NewRow();
				dr["DisplayID"] = "";
				dr["DisplayText"] = "All";
				dt.Rows.InsertAt(dr, 0);
			}
            return dt;
        }
        /// <summary>
        /// 獲取多語言的值
        /// </summary>
        /// <param name="ResourceId"></param>
        /// <returns></returns>
        public DataTable GetResourceValues(string ResourceId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("ResourceId", System.Data.DbType.String, ResourceId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.GetResourceValues, param.ToArray()).Tables[0];
            return dt;
        }

        /// <summary>
        /// 獲取對應語言類別的所有值
        /// </summary>
        /// <param name="LanguageId"></param>
        /// <returns></returns>
        public DataTable GetResourceByType(string LanguageId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("LanguageId", System.Data.DbType.String, LanguageId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.GetResourceByType, param.ToArray()).Tables[0];
            return dt;
        }
    }
}
