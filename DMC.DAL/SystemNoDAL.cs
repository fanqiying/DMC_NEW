using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using DMC.DAL.Script;
using DMC.Model;
namespace DMC.DAL
{
    public class SystemNoDAL
    {
        private It_SystemNo script = ScriptFactory.GetScript<It_SystemNo>();
        /// <summary>
        /// 添加單據
        /// <summary>
        public bool Addt_SystemNo(t_SystemNo model)
        {
            try
            {
                List<DbParameter> param = new List<DbParameter>();
                param.Add(DBFactory.Helper.FormatParameter("CompanyId", DbType.String, model.CompanyId, 20));
                param.Add(DBFactory.Helper.FormatParameter("ModuleType", DbType.String, model.ModuleType, 20));
                param.Add(DBFactory.Helper.FormatParameter("ModularType", DbType.String, model.ModularType, 20));
                param.Add(DBFactory.Helper.FormatParameter("Category", DbType.String, model.Category, 20));
                param.Add(DBFactory.Helper.FormatParameter("ReceiptType", DbType.String, model.ReceiptType, 20));
                param.Add(DBFactory.Helper.FormatParameter("keyword", DbType.String, model.keyword, 20));
                param.Add(DBFactory.Helper.FormatParameter("DateType", DbType.String, model.DateType, 20));
                param.Add(DBFactory.Helper.FormatParameter("CodeLen", DbType.String, model.CodeLen, 4));
                param.Add(DBFactory.Helper.FormatParameter("Mark", DbType.String, model.Mark, 128));
                param.Add(DBFactory.Helper.FormatParameter("Usy", DbType.String, model.Usy, 1));
                param.Add(DBFactory.Helper.FormatParameter("CreateUserId", DbType.String, model.CreateUserId, 20));
                param.Add(DBFactory.Helper.FormatParameter("CreateTime", DbType.DateTime, model.createTime, 8));
                param.Add(DBFactory.Helper.FormatParameter("CreateDeptId", DbType.String, model.CreateDeptId, 20));
                return DBFactory.Helper.ExecuteNonQuery(script.Addt_SystemNo, param.ToArray()) > 0 ? true : false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 修改單據
        /// <summary>
        public bool Modt_SystemNo(t_SystemNo model)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("AutoID", DbType.String, model.AutoID, 4));
            param.Add(DBFactory.Helper.FormatParameter("DateType", DbType.String, model.DateType, 20));
            param.Add(DBFactory.Helper.FormatParameter("CodeLen", DbType.String, model.CodeLen, 4));
            param.Add(DBFactory.Helper.FormatParameter("Mark", DbType.String, model.Mark, 128));
            param.Add(DBFactory.Helper.FormatParameter("Usy", DbType.String, model.Usy, 1));
            param.Add(DBFactory.Helper.FormatParameter("UpdateUserId", DbType.String, model.UpdateUserId, 20));
            param.Add(DBFactory.Helper.FormatParameter("UpdateTime", DbType.DateTime, model.UpdateTime, 8));
            param.Add(DBFactory.Helper.FormatParameter("UpdateDeptId", DbType.String, model.UpdateDeptId, 20));
            return DBFactory.Helper.ExecuteNonQuery(script.Modt_SystemNo, param.ToArray()) > 0 ? true : false;
        }
        /// <summary>
        ///刪除單據
        /// <summary>
        public bool Delt_SystemNo(string autoId, Trans t)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("AutoID", DbType.String, autoId));
            return DBFactory.Helper.ExecuteNonQuery(script.Delt_SystemNo, param.ToArray(), t) > 0 ? true : false;
        }
        /// <summary>
        /// 判斷是否存在
        /// <summary>
        public bool IsExitt_SystemNo(t_SystemNo model)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("CompanyId", DbType.String, model.CompanyId, 20));
            param.Add(DBFactory.Helper.FormatParameter("ModuleType", DbType.String, model.ModuleType, 20));
            param.Add(DBFactory.Helper.FormatParameter("ModularType", DbType.String, model.ModularType, 20));
            param.Add(DBFactory.Helper.FormatParameter("Category", DbType.String, model.Category, 20));
            param.Add(DBFactory.Helper.FormatParameter("ReceiptType", DbType.String, model.ReceiptType, 20));
            param.Add(DBFactory.Helper.FormatParameter("keyword", DbType.String, model.keyword, 20));
            return Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.IsExitt_SystemNo, param.ToArray())) > 0 ? true : false;
        }

        /// <summary>
        /// 驗證規則是否重複(按模塊、模組、類別、單據別、關鍵字、日期、流水碼)進行判斷
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool IsRepeatRule(t_SystemNo model)
        {
            List<DbParameter> param = new List<DbParameter>();
            //update by  jeven 2015/6/2 新增公司别参数，验证单据时必须卡公司别
            param.Add(DBFactory.Helper.FormatParameter("CompanyId", DbType.String, model.CompanyId, 20));
            param.Add(DBFactory.Helper.FormatParameter("AutoId", DbType.Int32, model.AutoID));
            param.Add(DBFactory.Helper.FormatParameter("ModuleType", DbType.String, model.ModuleType, 20));
            param.Add(DBFactory.Helper.FormatParameter("ModularType", DbType.String, model.ModularType, 20));
            param.Add(DBFactory.Helper.FormatParameter("Category", DbType.String, model.Category, 20));
            param.Add(DBFactory.Helper.FormatParameter("ReceiptType", DbType.String, model.ReceiptType, 20));
            param.Add(DBFactory.Helper.FormatParameter("keyword", DbType.String, model.keyword, 20));
            param.Add(DBFactory.Helper.FormatParameter("DateType", DbType.String, model.DateType, 20));
            param.Add(DBFactory.Helper.FormatParameter("CodeLen", DbType.Int32, model.CodeLen == null ? 0 : model.CodeLen.Value));

            return Convert.ToInt32(DBFactory.Helper.ExecuteScalar(script.IsRepeatRule, param.ToArray())) > 0 ? true : false;
        }
        /// <summary>
        /// 根據查詢條件獲取查詢結果
        /// <summary>
        public DataTable GetListt_SystemNo(string strWhere)
        {
            return DBFactory.Helper.ExecuteDataSet(script.GetListt_SystemNo + strWhere, null).Tables[0];
        }
        /// <summary>
        /// 不區分公司別的單號
        /// </summary>
        /// <param name="ReceiptType"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string GetGlobelNo(string ReceiptType, string keyword, string SplitStr = "-")
        {
            string result = string.Empty;
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("ReceiptType", DbType.String, ReceiptType, 20));
            param.Add(DBFactory.Helper.FormatParameter("keyword", DbType.String, keyword, 20));

            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.GlobelNo, param.ToArray()).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                bool isDay = false;
                //檢查是否存在日期
                if (!Convert.IsDBNull(dr["GeneratTime"]))
                {
                    DateTime dateTime = Convert.ToDateTime(dr["GeneratTime"]);
                    string dtype = dr["DateType"].ToString();
                    switch (dtype.Trim().ToLower())
                    {
                        case "yyyymmdd"://按天生成序號
                            if (dateTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yymmdd"://按天生成序號
                            if (dateTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yyyymm"://按月生成序號
                            if (dateTime.ToString("yyyy-MM") == DateTime.Now.ToString("yyyy-MM"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yymm"://按月生成序號
                            if (dateTime.ToString("yyyy-MM") == DateTime.Now.ToString("yyyy-MM"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yyyy"://按年生成序號
                            if (dateTime.ToString("yyyy") == DateTime.Now.ToString("yyyy"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yy"://按年生成序號
                            if (dateTime.ToString("yyyy") == DateTime.Now.ToString("yyyy"))
                            {
                                isDay = true;
                            }
                            break;
                    }
                }
                //設置初始值
                int code = 1;
                if (isDay)
                {
                    code = Convert.ToInt32(dr["CurrCode"]) + 1;
                }
                string fommat = dr["DateType"].ToString();
                string No = code.ToString();
                int CodeLen = Convert.ToInt32(dr["CodeLen"]);
                //檢查日期格式
                result = ReceiptType + SplitStr + DateTime.Now.ToString(fommat) + SplitStr + No.PadLeft(CodeLen, '0');
                //更新數據庫
                List<DbParameter> param1 = new List<DbParameter>();
                param1.Add(DBFactory.Helper.FormatParameter("AutoId", DbType.String, Convert.ToInt32(dr["AutoId"])));
                param1.Add(DBFactory.Helper.FormatParameter("CurrCode", DbType.Int32, code));
                DBFactory.Helper.ExecuteNonQuery(script.UpdateCode, param1.ToArray());
            }
            return result;
        }
        /// <summary>
        /// 根据类别，生成单据号
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="Category"></param>
        /// <param name="Keyword"></param>
        /// <param name="SplitStr"></param>
        /// <returns></returns>
        public string GetCategoryNo(string CompanyId, string Category, string Keyword, string SplitStr = "-")
        {
            string result = string.Empty;
            string ReceiptType = "";
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("CompanyId", DbType.String, CompanyId, 20));
            param.Add(DBFactory.Helper.FormatParameter("Category", DbType.String, Category, 20));
            param.Add(DBFactory.Helper.FormatParameter("keyword", DbType.String, Keyword, 20));

            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.GetCategoryNo, param.ToArray()).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ReceiptType = dr["ReceiptType"].ToString();
                bool isDay = false;
                //檢查是否存在日期
                if (!Convert.IsDBNull(dr["GeneratTime"]))
                {
                    DateTime dateTime = Convert.ToDateTime(dr["GeneratTime"]);
                    string dtype = dr["DateType"].ToString();
                    switch (dtype.Trim().ToLower())
                    {
                        case "yyyymmdd"://按天生成序號
                            if (dateTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yymmdd"://按天生成序號
                            if (dateTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yyyymm"://按月生成序號
                            if (dateTime.ToString("yyyy-MM") == DateTime.Now.ToString("yyyy-MM"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yymm"://按月生成序號
                            if (dateTime.ToString("yyyy-MM") == DateTime.Now.ToString("yyyy-MM"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yyyy"://按年生成序號
                            if (dateTime.ToString("yyyy") == DateTime.Now.ToString("yyyy"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yy"://按年生成序號
                            if (dateTime.ToString("yyyy") == DateTime.Now.ToString("yyyy"))
                            {
                                isDay = true;
                            }
                            break;
                    }
                }
                //設置初始值
                int code = 1;
                if (isDay)
                {
                    code = Convert.ToInt32(dr["CurrCode"]) + 1;
                }
                string fommat = dr["DateType"].ToString();
                string No = code.ToString();
                int CodeLen = Convert.ToInt32(dr["CodeLen"]);
                //檢查日期格式
                result = ReceiptType + SplitStr + DateTime.Now.ToString(fommat) + SplitStr + No.PadLeft(CodeLen, '0');
                //更新數據庫
                List<DbParameter> param1 = new List<DbParameter>();
                param1.Add(DBFactory.Helper.FormatParameter("AutoId", DbType.String, Convert.ToInt32(dr["AutoId"])));
                param1.Add(DBFactory.Helper.FormatParameter("CurrCode", DbType.Int32, code));
                DBFactory.Helper.ExecuteNonQuery(script.UpdateCode, param1.ToArray());
            }
            return result;
        }

        #region 多线程安全产生单号，500单号/S
        /*
         * 1.查询出单号规则，首次进行加载，后续不再加载
         */
        //格式
        public class CategoryRule
        {
            public string ReceiptType { get; set; }
            public string DateFormat { get; set; }
            public int CodeLen { get; set; }
            public int AutoId { get; set; }
            public DateTime GenDateTime { get; set; }
        }

        public class OrderNumber
        {
            private static object lockObject = new object();
            private int MaxNumber = 1;
            public OrderNumber(int initNumber)
            {
                MaxNumber = initNumber;
            }
            public int GetNumber()
            {
                int result = 0;
                lock (lockObject)
                {
                    result = ++MaxNumber;
                }
                return result;
            }
        }
        public static Dictionary<string, CategoryRule> dicRule = new Dictionary<string, CategoryRule>();
        //格式的最大流水码
        public static Dictionary<string, OrderNumber> dicFormatMaxNumber = new Dictionary<string, OrderNumber>();
        //排队生成对象
        public static object lockObject = new object();

        public string GetCategoryNoByThread(string CompanyId, string Category, string Keyword, string SplitStr = "-")
        {
            string result = string.Empty;
            //1.缓存规则
            string key = string.Format("{0}-{1}-{2}", CompanyId, Category, Keyword);
            if (!dicRule.ContainsKey(key))
            {
                //不存在则生成规则的格式，则查询出来
                lock (lockObject)
                {
                    if (!dicRule.ContainsKey(key))
                    {
                        //1.加载格式定义
                        List<DbParameter> param = new List<DbParameter>();
                        param.Add(DBFactory.Helper.FormatParameter("CompanyId", DbType.String, CompanyId, 20));
                        param.Add(DBFactory.Helper.FormatParameter("Category", DbType.String, Category, 20));
                        param.Add(DBFactory.Helper.FormatParameter("keyword", DbType.String, Keyword, 20));
                        DataTable dtRule = DBFactory.Helper.ExecuteDataSet(script.GetCategoryNo, param.ToArray()).Tables[0];

                        //加载最后版本的内容
                        if (dtRule != null && dtRule.Rows.Count > 0)
                        {
                            CategoryRule cr = new CategoryRule();
                            DataRow dr = dtRule.Rows[0];
                            cr.ReceiptType = dr["ReceiptType"].ToString();
                            DateTime dateTime = DateTime.Now;
                            if (dr["GeneratTime"] != null && !Convert.IsDBNull(dr["GeneratTime"]))
                            {
                                dateTime = Convert.ToDateTime(dr["GeneratTime"]);
                            }
                            cr.DateFormat = dr["DateType"].ToString();
                            cr.CodeLen = Convert.ToInt32(dr["CodeLen"]);
                            cr.AutoId = Convert.ToInt32(dr["AutoId"]);
                            cr.GenDateTime = dateTime;
                            int code = 0;
                            if (dr["CurrCode"] != null && !Convert.IsDBNull(dr["CurrCode"]))
                            {
                                code = Convert.ToInt32(dr["CurrCode"]);
                            }
                            string keyFormat = string.Format("{1}{0}{2}{0}", SplitStr, cr.ReceiptType, dateTime.ToString(cr.DateFormat)) + "{0}";
                            dicFormatMaxNumber.Add(keyFormat, new OrderNumber(code));
                            dicRule.Add(key, cr);
                        }
                    }
                }
            }
            //产生格式 
            CategoryRule drRow = dicRule[key];
            if (drRow != null)
            {
                //DataRow dr = dt.Rows[0];
                //string ReceiptType = drRow["ReceiptType"].ToString();
                //string fommat = drRow["DateType"].ToString();
                string keyFormat = string.Format("{1}{0}{2}{0}", SplitStr, drRow.ReceiptType, DateTime.Now.ToString(drRow.DateFormat)) + "{0}";
                if (!dicFormatMaxNumber.ContainsKey(keyFormat))
                {
                    lock (lockObject)
                    {
                        if (!dicFormatMaxNumber.ContainsKey(keyFormat))
                        {
                            string keyPerFormat = string.Format("{1}{0}{2}{0}", SplitStr, drRow.ReceiptType, DateTime.Now.AddDays(-1).ToString(drRow.DateFormat)) + "{0}";
                            if (keyPerFormat != keyFormat)
                            {
                                dicFormatMaxNumber.Remove(keyPerFormat);
                            }
                            if (DateTime.Now.ToString(drRow.DateFormat) != drRow.GenDateTime.ToString(drRow.DateFormat))
                            {
                                drRow.GenDateTime = DateTime.Now;
                                //初始化为0;
                                //更新數據庫
                                List<DbParameter> param1 = new List<DbParameter>();
                                param1.Add(DBFactory.Helper.FormatParameter("AutoId", DbType.String, drRow.AutoId));
                                param1.Add(DBFactory.Helper.FormatParameter("CurrCode", DbType.Int32, 0));
                                DBFactory.Helper.ExecuteNonQuery(script.UpdateCode, param1.ToArray());
                                //判断内容值是否为0，设置初始
                                dicFormatMaxNumber.Add(keyFormat, new OrderNumber(0));
                            }
                        }
                    }
                }
                //string uuid = Guid.NewGuid().ToString();
                //dicFormatMaxNumber[keyFormat].Add(uuid);
                int code = dicFormatMaxNumber[keyFormat].GetNumber();
                //string No = code.ToString();
                //檢查日期格式
                result = string.Format(keyFormat, code.ToString().PadLeft(drRow.CodeLen, '0'));//drRow.ReceiptType + SplitStr + DateTime.Now.ToString(drRow.DateFormat) + SplitStr + No.PadLeft(CodeLen, '0');
                //更新數據庫
                List<DbParameter> param2 = new List<DbParameter>();
                param2.Add(DBFactory.Helper.FormatParameter("AutoId", DbType.String, drRow.AutoId));
                DBFactory.Helper.ExecuteNonQuery(script.StepAddCode, param2.ToArray());
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 根据类别，生成单据号
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="Category"></param>
        /// <param name="Keyword"></param>
        /// <param name="SplitStr"></param>
        /// <returns></returns>
        public string GetCategoryNo(string CompanyId, string Category, string Keyword, Trans t, string SplitStr = "-")
        {
            string result = string.Empty;
            string ReceiptType = "";
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("CompanyId", DbType.String, CompanyId, 20));
            param.Add(DBFactory.Helper.FormatParameter("Category", DbType.String, Category, 20));
            param.Add(DBFactory.Helper.FormatParameter("keyword", DbType.String, Keyword, 20));

            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.GetCategoryNo, param.ToArray(), t).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ReceiptType = dr["ReceiptType"].ToString();
                bool isDay = false;
                //檢查是否存在日期
                if (!Convert.IsDBNull(dr["GeneratTime"]))
                {
                    DateTime dateTime = Convert.ToDateTime(dr["GeneratTime"]);
                    string dtype = dr["DateType"].ToString();
                    switch (dtype.Trim().ToLower())
                    {
                        case "yyyymmdd"://按天生成序號
                            if (dateTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yymmdd"://按天生成序號
                            if (dateTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yyyymm"://按月生成序號
                            if (dateTime.ToString("yyyy-MM") == DateTime.Now.ToString("yyyy-MM"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yymm"://按月生成序號
                            if (dateTime.ToString("yyyy-MM") == DateTime.Now.ToString("yyyy-MM"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yyyy"://按年生成序號
                            if (dateTime.ToString("yyyy") == DateTime.Now.ToString("yyyy"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yy"://按年生成序號
                            if (dateTime.ToString("yyyy") == DateTime.Now.ToString("yyyy"))
                            {
                                isDay = true;
                            }
                            break;
                    }
                }
                //設置初始值
                int code = 1;
                if (isDay)
                {
                    code = Convert.ToInt32(dr["CurrCode"]) + 1;
                }
                string fommat = dr["DateType"].ToString();
                string No = code.ToString();
                int CodeLen = Convert.ToInt32(dr["CodeLen"]);
                //檢查日期格式
                result = ReceiptType + SplitStr + DateTime.Now.ToString(fommat) + SplitStr + No.PadLeft(CodeLen, '0');
                //更新數據庫
                List<DbParameter> param1 = new List<DbParameter>();
                param1.Add(DBFactory.Helper.FormatParameter("AutoId", DbType.String, Convert.ToInt32(dr["AutoId"])));
                param1.Add(DBFactory.Helper.FormatParameter("CurrCode", DbType.Int32, code));
                DBFactory.Helper.ExecuteNonQuery(script.UpdateCode, param1.ToArray(), t);
            }
            return result;
        }

        /// <summary>
        /// 根據單據別和關鍵字，產生單號
        /// </summary>
        /// <param name="ReceiptType"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string GetSysNoNew(string CompanyId, string keyword)
        {
            string result = string.Empty;
            string ReceiptType = "";
            List<DbParameter> param = new List<DbParameter>();
            //param.Add(DBFactory.Helper.FormatParameter("ReceiptType", DbType.String, ReceiptType, 20));
            param.Add(DBFactory.Helper.FormatParameter("keyword", DbType.String, keyword, 20));
            param.Add(DBFactory.Helper.FormatParameter("CompanyId", DbType.String, CompanyId, 20));

            DataTable dt = DBFactory.Helper.ExecuteDataSet(script.GeneratSystemNo, param.ToArray()).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ReceiptType = dr["ReceiptType"].ToString();
                bool isDay = false;
                //檢查是否存在日期
                if (!Convert.IsDBNull(dr["GeneratTime"]))
                {
                    DateTime dateTime = Convert.ToDateTime(dr["GeneratTime"]);
                    string dtype = dr["DateType"].ToString();
                    switch (dtype.Trim().ToLower())
                    {
                        case "yyyymmdd"://按天生成序號
                            if (dateTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yyyymm"://按月生成序號
                            if (dateTime.ToString("yyyy-MM") == DateTime.Now.ToString("yyyy-MM"))
                            {
                                isDay = true;
                            }
                            break;
                        case "yyyy"://按年生成序號
                            if (dateTime.ToString("yyyy") == DateTime.Now.ToString("yyyy"))
                            {
                                isDay = true;
                            }
                            break;
                    }
                }
                //設置初始值
                int code = 1;
                if (isDay)
                {
                    code = Convert.ToInt32(dr["CurrCode"]) + 1;
                }
                string fommat = dr["DateType"].ToString();
                string No = code.ToString();
                int CodeLen = Convert.ToInt32(dr["CodeLen"]);
                //檢查日期格式
                result = ReceiptType + "-" + DateTime.Now.ToString(fommat) + "-" + No.PadLeft(CodeLen, '0');
                //更新數據庫
                List<DbParameter> param1 = new List<DbParameter>();
                param1.Add(DBFactory.Helper.FormatParameter("AutoId", DbType.String, Convert.ToInt32(dr["AutoId"])));
                param1.Add(DBFactory.Helper.FormatParameter("CurrCode", DbType.Int32, code));
                DBFactory.Helper.ExecuteNonQuery(script.UpdateCode, param1.ToArray());
            }
            return result;
        }

        /// <summary>
        /// 獲取單據別類表
        /// </summary>
        /// <param name="ProgramId"></param>
        /// <returns></returns>
        public DataTable GetReceiptList(string ProgramId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("keyword", DbType.String, ProgramId, 20));
            return DBFactory.Helper.ExecuteDataSet(script.GetReceiptList, param.ToArray()).Tables[0];
        }
        /// <summary>
        /// 根據單據別和關鍵字，產生單流水單號
        /// </summary>
        /// <param name="ReceiptType"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string GetSysNo(string CompanyId, string keyword)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DBFactory.Helper.FormatParameter("pro", DbType.String, keyword));
            param.Add(DBFactory.Helper.FormatParameter("Commpany ", DbType.String, CompanyId));
            DataTable dt = DBFactory.Helper.ExecuteProcDataSet("CreateSysNum", param.ToArray()).Tables[0];
            string strSysNo = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                string[] colum = dt.Rows[0]["Expression"].ToString().Split('+');
                foreach (string p in colum)
                {
                    if (p == "Datatype")
                    {
                        strSysNo += DateTime.Parse(dt.Rows[0]["Serverdate"].ToString()).ToString(dt.Rows[0][p].ToString());
                        continue;
                    }
                    if (p == "NumSite")
                    {
                        strSysNo += dt.Rows[0]["MaxNum"].ToString().PadLeft(int.Parse(dt.Rows[0][p].ToString()), '0');
                        continue;
                    }
                    strSysNo += dt.Rows[0][p].ToString();
                }
                return strSysNo;
            }
            else
            {
                return "0";
            }
        }
    }
}
