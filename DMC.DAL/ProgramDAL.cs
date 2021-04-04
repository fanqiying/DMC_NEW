using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using DMC.DAL.Script;
using DMC.Model;
namespace DMC.DAL
{
    /// <summary>
    /// 程式數據訪問操作類
    /// code by jeven_xiao
    /// 2013-6-9
    /// </summary>
    public class ProgramDAL
    {
        private IProgram script = ScriptFactory.GetScript<IProgram>();
        /// <summary>
        /// 增加新的程式
        /// </summary>
        /// <param name="prog">程式实体</param>
        /// <returns>bool</returns>
        public bool AddProgram(t_Program prog)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("programID", DbType.String, prog.programID, 20));
            param.Add(DBFactory.Helper.FormatParameter("menuId", DbType.String, prog.menuId, 20));
            param.Add(DBFactory.Helper.FormatParameter("programName", DbType.String, prog.programName, 20));
            param.Add(DBFactory.Helper.FormatParameter("menuUrl", DbType.String, prog.menuUrl, 500));
            param.Add(DBFactory.Helper.FormatParameter("functionStr", DbType.String, prog.functionStr, 500));
            param.Add(DBFactory.Helper.FormatParameter("createTime", DbType.DateTime, System.DateTime.Now));
            param.Add(DBFactory.Helper.FormatParameter("usy", DbType.String, prog.usy, 1));
            param.Add(DBFactory.Helper.FormatParameter("createrID", DbType.String, prog.createrID, 20));
            param.Add(DBFactory.Helper.FormatParameter("cDeptID", DbType.String, prog.cDeptID, 20));
            param.Add(DBFactory.Helper.FormatParameter("orderid", DbType.Int32, prog.orderid.Value));
            param.Add(DBFactory.Helper.FormatParameter("IsMobile", DbType.String, prog.IsMobile, 1));
            param.Add(DBFactory.Helper.FormatParameter("MobileUrl", DbType.String, prog.MobileUrl, 500));
            using (Trans tr = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(script.AddProgram, param.ToArray()) > 0)
                    {
                        tr.Commit();
                        return true;
                    }
                    else
                        return false;
                }

                catch (Exception ex)
                {
                    tr.RollBack();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 更新程式与菜单的关联
        /// </summary>
        /// <param name="programID"></param>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public bool ModProgMenu(string programID, string menuID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("programID", DbType.String, programID, 20));
            param.Add(DBFactory.Helper.FormatParameter("menuID", DbType.String, menuID, 20));

            using (Trans tr = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(script.ModProgMenu, param.ToArray()) > 0)
                    {
                        tr.Commit();
                        return true;
                    }
                    else
                        return false;
                }

                catch (Exception ex)
                {
                    tr.RollBack();
                    throw ex;
                }
            }

        }

        /// <summary>
        /// 设置程式与菜单的关联
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        public bool AddProgMenu(t_ProgRefMenu pm)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("programID", DbType.String, pm.programID, 20));
            param.Add(DBFactory.Helper.FormatParameter("menuID", DbType.String, pm.menuID, 20));
            param.Add(DBFactory.Helper.FormatParameter("usy", DbType.String, pm.usy, 1));

            using (Trans tr = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(script.AddProgMenu, param.ToArray()) > 0)
                    {
                        tr.Commit();
                        return true;
                    }
                    else
                        return false;
                }

                catch (Exception ex)
                {
                    tr.RollBack();
                    throw ex;
                }
            }

        }

        /// <summary>
        /// 增加程式的基本执行功能
        /// </summary>
        /// <returns></returns>
        public bool SetProgFunc(List<string> funcIDList, string programID)
        {
            using (Trans tr = new Trans())
            {
                try
                {
                    foreach (string s in funcIDList)
                    {
                        List<DbParameter> param = new List<DbParameter>();
                        param.Add(DBFactory.Helper.FormatParameter("functionID", DbType.String, s, 20));
                        param.Add(DBFactory.Helper.FormatParameter("programID", DbType.String, programID, 20));
                        DBFactory.Helper.ExecuteNonQuery(script.SetProgFunc, param.ToArray());

                    }
                    tr.Commit();
                    return true;
                }

                catch (Exception ex)
                {
                    tr.RollBack();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 验证程式编号是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsExitProg(string programID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("programID", System.Data.DbType.String, programID, 20));
            return DBFactory.Helper.ExecuteScalar(script.IsExitProg, param.ToArray()) != null ? true : false;

        }
        /// <summary>
        /// 验证是否存在程式的基本功能
        /// </summary>
        /// <param name="programID"></param>
        /// <returns></returns>
        public bool IsExitProFunc(string programID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("programID", System.Data.DbType.String, programID, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.IsExitProgFunc, param.ToArray()));
            return result != 0;
        }

        /// <summary>
        /// 验证该条程式是否设置菜单关联
        /// </summary>
        /// <param name="programID"></param>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public bool IsExitProgVsMenu(string programID)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("programID", System.Data.DbType.String, programID, 20));
            int result = Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.IsExitProgVsMenu, param.ToArray()));
            return result != 0;
        }

        /// <summary>
        /// 删除某一只程式相关的基本执行功能
        /// </summary>
        /// <param name="idList">程式ID</param>
        /// <returns></returns>
        public bool DelProgFunc(string programID)
        {
            using (Trans tr = new Trans())
            {
                try
                {

                    List<DbParameter> param = new List<DbParameter>();
                    param.Add(DBFactory.Helper.FormatParameter("programID", DbType.String, programID, 20));
                    DBFactory.Helper.ExecuteNonQuery(script.DelProgFunc, param.ToArray());


                    tr.Commit();
                    return true;
                }

                catch (Exception ex)
                {
                    tr.RollBack();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 删除程式
        /// </summary>
        /// <param name="idlist">程式ID</param>
        /// <returns>bool</returns>
        public bool DelProgram(List<string> idlist)
        {
            using (Trans tr = new Trans())
            {
                try
                {
                    foreach (string s in idlist)
                    {
                        List<DbParameter> param = new List<DbParameter>();
                        param.Add(DBFactory.Helper.FormatParameter("idList", DbType.String, s, 20));
                        DBFactory.Helper.ExecuteNonQuery(script.DelProgram, param.ToArray());

                    }
                    tr.Commit();
                    return true;
                }

                catch (Exception ex)
                {
                    tr.RollBack();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 取得格式化后的程式数据
        /// </summary>
        /// <param name="languageID"></param>
        /// <returns></returns>
        public DataTable ProgramDT(string languageID)
        {

            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("LanguageId", System.Data.DbType.String, languageID, 20));
            return DBFactory.Helper.ExecuteDataSet(script.IsExitProgFunc, param.ToArray()).Tables[0];

        }

        /// <summary>
        /// 取得程式的基本功能列表，从多语言配置中
        /// </summary>
        /// <param name="languageID"></param>
        /// <param name="programID"></param>
        /// <returns></returns>
        public DataTable ProgramFuncList(string languageID, string programID)
        {

            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("LanguageId", System.Data.DbType.String, languageID, 20));
            param.Add(DBFactory.Helper.FormatParameter("ProgramId", System.Data.DbType.String, programID, 20));
            return DBFactory.Helper.ExecuteDataSet(script.ProgramFuncList, param.ToArray()).Tables[0];

        }

        /// <summary>
        /// 獲取程式編號列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetProgramIdList()
        {
            return DBFactory.Helper.ExecuteDataSet(script.ProgramIdList, null).Tables[0];
        }

        /// <summary>
        /// 修改程式资料信息
        /// </summary>
        /// <param name="prog"></param>
        /// <returns></returns>
        public bool ModProgram(t_Program prog)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("programID", DbType.String, prog.programID, 20));
            param.Add(DBFactory.Helper.FormatParameter("menuId", DbType.String, prog.menuId, 20));
            param.Add(DBFactory.Helper.FormatParameter("programName", DbType.String, prog.programName, 20));
            param.Add(DBFactory.Helper.FormatParameter("menuUrl", DbType.String, prog.menuUrl, 500));
            param.Add(DBFactory.Helper.FormatParameter("functionStr", DbType.String, prog.functionStr, 500));
            param.Add(DBFactory.Helper.FormatParameter("modTime", DbType.DateTime, System.DateTime.Now));
            param.Add(DBFactory.Helper.FormatParameter("usy", DbType.String, prog.usy, 1));
            param.Add(DBFactory.Helper.FormatParameter("updaterID", DbType.String, prog.updaterID, 20));
            param.Add(DBFactory.Helper.FormatParameter("uDeptID", DbType.String, prog.uDeptID, 20));
            param.Add(DBFactory.Helper.FormatParameter("orderid", DbType.Int32, prog.orderid.Value));
            param.Add(DBFactory.Helper.FormatParameter("IsMobile", DbType.String, prog.IsMobile, 1));
            param.Add(DBFactory.Helper.FormatParameter("MobileUrl", DbType.String, prog.MobileUrl, 500));

            using (Trans tr = new Trans())
            {
                try
                {
                    if (DBFactory.Helper.ExecuteNonQuery(script.ModProgram, param.ToArray()) > 0)
                    {
                        tr.Commit();
                        return true;
                    }
                    else
                        return false;
                }

                catch (Exception ex)
                {
                    tr.RollBack();
                    throw ex;
                }
            }
        }





    }
}
