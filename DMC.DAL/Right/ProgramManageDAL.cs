using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DMC.DAL.Script;

namespace DMC.DAL
{
    /// <summary>
    /// 權限類別和個人的程式權限、操作和資料權限管理設置
    /// </summary>
    public class ProgramManageDAL
    {
        private IProgramRight script = ScriptFactory.GetScript<IProgramRight>();
        private IActionRight actionScript = ScriptFactory.GetScript<IActionRight>();
        #region 程式權限
        /// <summary>
        /// 程式權限搜索
        /// </summary>
        /// <param name="isUser">是否為用戶</param>
        /// <param name="IdKey">用戶編號或權限類別編號</param>
        /// <param name="KeyWord">關鍵字</param>
        /// <param name="LangId">語言別</param>
        /// <returns>返回已授權的程式列表</returns>
        public DataTable SearchProgram(bool isUser, string IdKey, string KeyWord, string LanguageId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter(isUser ? "UserId" : "RoseId", System.Data.DbType.String, IdKey, 20));
            param.Add(DBFactory.Helper.FormatParameter("KeyWord", System.Data.DbType.String, KeyWord, 20));
            param.Add(DBFactory.Helper.FormatParameter("LangId", System.Data.DbType.String, LanguageId, 20));
            string procName = isUser ? script.SearchProgramRightByUser : script.SearchProgramRightByRose;
            DataTable dt = DBFactory.Helper.ExecuteDataSet(procName, param.ToArray()).Tables[0];
            return dt;
        }

        /// <summary>
        /// 程式權限添加
        /// </summary>
        /// <param name="isUser">是否為用戶</param>
        /// <param name="IdKey">用戶編號或權限類別編號</param>
        /// <param name="ProgramIdList">程式編號列表</param>
        /// <returns>
        /// SR0005：程式權限添加成功
        /// </returns>
        public bool AddProgram(bool isUser, string IdKey, List<string> ProgramIdList)
        {
            List<string> historyProgramId = new List<string>();
            DataTable historyList = SearchProgram(isUser, IdKey, "", "");
            foreach (DataRow dr in historyList.Rows)
            {
                historyProgramId.Add(dr["ProgramId"].ToString());
            }
            //獲取到歷史程式并進行比較，進行新增和刪除操作
            List<string> deleteProgramId = historyProgramId.FindAll(o => !ProgramIdList.Contains(o));
            List<string> newProgramId = ProgramIdList.FindAll(o => !historyProgramId.Contains(o));

            using (Trans t = new Trans())
            {
                try
                {
                    bool isResult = DelProgram(isUser, IdKey, deleteProgramId, t);
                    if (isResult)
                    {
                        string sql = isUser ? script.AddProgramRightByUser : script.AddProgramRightByRose;
                        foreach (string ProgramId in newProgramId)
                        {
                            //需要判断程式是否存在
                            if (!ExistsProgramId(ProgramId))
                            {
                                isResult = false;
                                break;
                            }
                            List<DbParameter> param = new List<DbParameter>();
                            param.Add(DBFactory.Helper.FormatParameter(isUser ? "UserId" : "RoseId", System.Data.DbType.String, IdKey, 20));
                            param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
                            int result = DBFactory.Helper.ExecuteNonQuery(sql, param.ToArray(), t);
                            isResult = result == 1;
                            if (!isResult)
                                break;

                            AddAllProgramAction(isUser, IdKey, ProgramId, t);
                        }
                    }
                    if (isResult)
                    {
                        t.Commit();
                        return true;
                    }
                    else
                    {
                        t.RollBack();
                        return false;
                    }
                }
                catch
                {
                    t.RollBack();
                    return false;
                }
            }
        }

        public bool ExistsProgramId(string ProgramId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.ExistsProgramId, param.ToArray()));
            return result != 0;
        }

        /// <summary>
        /// 程式權限刪除
        /// </summary>
        /// <param name="isUser">是否為用戶</param>
        /// <param name="IdKey">用戶編號或權限類別編號</param>
        /// <param name="ProgramId">程式編號</param>
        /// <returns>
        /// SR0004：刪除程式權限成功
        /// </returns>
        public bool DelProgram(bool isUser, string IdKey, Trans t)
        {
            try
            {
                string sql = isUser ? script.DeleteAllProgramRightByUser : script.DeleteAllProgramRightByRose;
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter(isUser ? "UserId" : "RoseId", System.Data.DbType.String, IdKey, 20));
                DBFactory.Helper.ExecuteNonQuery(sql, param.ToArray(), t);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 程式權限刪除
        /// </summary>
        /// <param name="isUser">是否為用戶</param>
        /// <param name="IdKey">用戶編號或權限類別編號</param>
        /// <param name="ProgramId">程式編號</param>
        /// <returns>
        /// SR0004：刪除程式權限成功
        /// </returns>
        public bool DelProgram(bool isUser, string IdKey, List<string> ProgramIdList, Trans t)
        {
            string sql = isUser ? script.DeleteProgramRightByUser : script.DeleteProgramRightByRose;
            bool isResult = true;
            foreach (string ProgramId in ProgramIdList)
            {
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter(isUser ? "UserId" : "RoseId", System.Data.DbType.String, IdKey, 20));
                param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
                int result = DBFactory.Helper.ExecuteNonQuery(sql, param.ToArray());
                isResult = (result == 1);
                if (!isResult)
                    break;
            }
            return isResult;
        }

        /// <summary>
        /// 程式權限刪除
        /// </summary>
        /// <param name="isUser">是否為用戶</param>
        /// <param name="IdKey">用戶編號或權限類別編號</param>
        /// <param name="ProgramId">程式編號</param>
        /// <returns>
        /// SR0004：刪除程式權限成功
        /// </returns>
        public bool DelProgram(bool isUser, string IdKey, List<string> ProgramIdList)
        {
            using (Trans t = new Trans())
            {
                try
                {
                    string sql = isUser ? script.DeleteProgramRightByUser : script.DeleteProgramRightByRose;
                    bool isResult = true;
                    foreach (string ProgramId in ProgramIdList)
                    {
                        List<DbParameter> param = new List<DbParameter>();
                        param.Add(DBFactory.Helper.FormatParameter(isUser ? "UserId" : "RoseId", System.Data.DbType.String, IdKey, 20));
                        param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
                        int result = DBFactory.Helper.ExecuteNonQuery(sql, param.ToArray());
                        isResult = (result >= 1);
                        if (!isResult)
                            break;
                    }
                    if (isResult)
                    {
                        t.Commit();
                        return true;
                    }
                    else
                    {
                        t.RollBack();
                        return false;
                    }
                }
                catch
                {
                    t.RollBack();
                    return false;
                }
            }
        }
        #endregion

        #region 執行權限
        /// <summary>
        /// 獲取程式的執行權限
        /// </summary>
        /// <param name="isUser">是否為用戶</param>
        /// <param name="IdKey">用戶編號或權限類別編號</param>
        /// <param name="ProgramId">程式編號</param>
        /// <param name="LangId">語言別</param>
        /// <returns>返回操作的狀態</returns>
        public DataTable ReadProgramAction(bool isUser, string IdKey, string ProgramId, string LanguageId)
        {
            string sql = isUser ? actionScript.ReadProgramActionByUser : actionScript.ReadProgramActionByRose;
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter(isUser ? "UserId" : "RoseId", System.Data.DbType.String, IdKey, 20));
            param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
            param.Add(DBFactory.Helper.FormatParameter("LanguageId", System.Data.DbType.String, LanguageId, 20));
            DataTable dt = DBFactory.Helper.ExecuteDataSet(sql, param.ToArray()).Tables[0];
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isUser"></param>
        /// <param name="IdKey"></param>
        /// <param name="ProgramId"></param>
        /// <returns></returns>
        public bool AddAllProgramAction(bool isUser, string IdKey, string ProgramId)
        {
            try
            {
                string sql = isUser ? actionScript.AddAllProgramActionByUser : actionScript.AddAllProgramActionByRose;
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter(isUser ? "UserId" : "RoseId", System.Data.DbType.String, IdKey, 20));
                param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
                DBFactory.Helper.ExecuteNonQuery(sql, param.ToArray());
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isUser"></param>
        /// <param name="IdKey"></param>
        /// <param name="ProgramId"></param>
        /// <returns></returns>
        public bool AddAllProgramAction(bool isUser, string IdKey, string ProgramId, Trans t)
        {
            try
            {
                string sql = isUser ? actionScript.AddAllProgramActionByUser : actionScript.AddAllProgramActionByRose;
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter(isUser ? "UserId" : "RoseId", System.Data.DbType.String, IdKey, 20));
                param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
                DBFactory.Helper.ExecuteNonQuery(sql, param.ToArray(), t);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 程式操作權限保存
        /// </summary>
        /// <param name="isUser">是否為用戶</param>
        /// <param name="IdKey">用戶編號或權限類別編號</param>
        /// <param name="ProgramId">程式編號</param>
        /// <param name="ActionIdList">操作列表</param>
        /// <returns></returns>
        public bool SaveProgramAction(bool isUser, string IdKey, string ProgramId, List<string> ActionIdList)
        {
            using (Trans t = new Trans())
            {
                try
                {
                    bool isResult = true;
                    string addSql = isUser ? actionScript.AddProgramActionByUser : actionScript.AddProgramActionByRose;
                    string deleteSql = isUser ? actionScript.DeleteProgramActionByUser : actionScript.DeleteProgramActionByRose;
                    //第一步，进行历史数据删除
                    List<DbParameter> delParam = new List<DbParameter>();
                    delParam.Add(DBFactory.Helper.FormatParameter(isUser ? "UserId" : "RoseId", System.Data.DbType.String, IdKey, 20));
                    delParam.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
                    DBFactory.Helper.ExecuteNonQuery(deleteSql, delParam.ToArray(), t);
                    //第二步，添加所选中的操作
                    foreach (string ActionId in ActionIdList)
                    {
                        List<DbParameter> addParam = new List<DbParameter>();
                        addParam.Add(DBFactory.Helper.FormatParameter(isUser ? "UserId" : "RoseId", System.Data.DbType.String, IdKey, 20));
                        addParam.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
                        addParam.Add(DBFactory.Helper.FormatParameter("ActionId", System.Data.DbType.String, ActionId, 20));
                        int result = DBFactory.Helper.ExecuteNonQuery(addSql, addParam.ToArray(), t);
                        isResult = result == 1;
                        if (!isResult)
                            break;
                    }
                    if (isResult)
                    {
                        t.Commit();
                        return true;
                    }
                    else
                    {
                        t.RollBack();
                        return false;
                    }
                }
                catch
                {
                    t.RollBack();
                    return false;
                }
            }
        }

        /// <summary>
        /// 删除使用者或权限类别对应程式的操作权限
        /// </summary>
        /// <param name="isUser"></param>
        /// <param name="IdKey"></param>
        /// <param name="ProgramId"></param>
        /// <returns></returns>
        public bool DeleteProgramAction(bool isUser, string IdKey, string ProgramId)
        {
            try
            {
                string sql = isUser ? actionScript.DeleteProgramActionByUser : actionScript.DeleteProgramActionByRose;
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter(isUser ? "UserId" : "RoseId", System.Data.DbType.String, IdKey, 20));
                param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, ProgramId, 20));
                DBFactory.Helper.ExecuteNonQuery(sql, param.ToArray());
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }


}
