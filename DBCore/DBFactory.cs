using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using MySql.Data.MySqlClient;
using System.Data.OracleClient;
using System.Linq;

public class DBFactory
{
	//實例化DB工廠
    public static DBFactory Helper = new DBFactory();

    #region 基礎變量
    private static string _paraKey = "@";
    public static string ParaKey
    {
        get
        {
            if (string.IsNullOrEmpty(DBType))
                DBType = ConfigurationManager.AppSettings["DBType"];

            switch (DBType.ToLower())
            {
                case "sqlserver":
                    _paraKey = "@";
                    break;
                case "oracel":
                    _paraKey = ":";
                    break;
                case "mysql":
                    _paraKey = "?";
                    break;
            }
            return _paraKey;
        }
    }
    /// <summary>
    /// 使用的數據庫類型
    /// </summary>
    private static string DBType = ConfigurationManager.AppSettings["DBType"];
    /// <summary>
    /// 連接驅動類型
    /// </summary>
    private static string dbProviderName
    {
        get
        {
            if (string.IsNullOrEmpty(DBType))
                DBType = ConfigurationManager.AppSettings["DBType"];
            return ConfigurationManager.ConnectionStrings[DBType].ProviderName;
        }
    }
    /// <summary>
    /// 連接字符串
    /// </summary>
    private static string dbConnectionString
    {
        get
        {
            if (string.IsNullOrEmpty(DBType))
                DBType = ConfigurationManager.AppSettings["DBType"];
            return ConfigurationManager.ConnectionStrings[DBType].ConnectionString;
        }
    }
    /// <summary>
    /// 數據庫連接
    /// </summary>
    private DbConnection connection;
    #endregion

    #region 構造函數
    public DBFactory()
    {
        this.connection = CreateConnection(DBFactory.dbConnectionString);
    }

    public DBFactory(string connectionString)
    {
        this.connection = CreateConnection(connectionString);
    }
    #endregion

    #region 創建命令和連接
    public static DbConnection CreateConnection()
    {
        DbProviderFactory dbfactory = GetFactory(DBFactory.dbProviderName);
        DbConnection dbconn = dbfactory.CreateConnection();
        dbconn.ConnectionString = DBFactory.dbConnectionString;
        return dbconn;
    }
    public static DbConnection CreateConnection(string connectionString)
    {
        DbProviderFactory dbfactory = GetFactory(DBFactory.dbProviderName);
        DbConnection dbconn = dbfactory.CreateConnection();
        dbconn.ConnectionString = connectionString;
        return dbconn;
    }

    public static DbProviderFactory GetFactory(string providerName)
    {
        if (providerName == null)
            throw new ArgumentNullException("providerName");
        switch (providerName)
        {
            case "MySql.Data.MySqlClient":
                return new MySqlClientFactory();
            default: return DbProviderFactories.GetFactory(providerName);
        }
    }

    public DbCommand GetStoredProcCommond(string storedProcedure, DbConnection conn = null)
    {
        if (conn == null)
            conn = CreateConnection(DBFactory.dbConnectionString);
        DbCommand dbCommand = conn.CreateCommand();
        dbCommand.CommandText = storedProcedure;
        dbCommand.CommandType = CommandType.StoredProcedure;
        return dbCommand;
    }
    public DbCommand GetSqlStringCommond(string sqlQuery, DbConnection conn = null)
    {
        if (conn == null)
            conn = CreateConnection(DBFactory.dbConnectionString);
        DbCommand dbCommand = conn.CreateCommand();
        dbCommand.CommandText = sqlQuery;
        dbCommand.CommandType = CommandType.Text;
        return dbCommand;
    }
    #endregion

    #region 參數
    public void AddParameterCollection(DbCommand cmd, DbParameterCollection dbParameterCollection)
    {
        foreach (DbParameter dbParameter in dbParameterCollection)
        {
            cmd.Parameters.Add(dbParameter);
        }
    }

	/// <summary>
	/// 增加輸出參數
	/// </summary>
	/// <param name="cmd">DbCommand 對象</param>
	/// <param name="parameterName">參數名稱</param>
	/// <param name="dbType">數據類型</param>
	/// <param name="size">字段大小</param>
    public void AddOutParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
    {
        DbParameter dbParameter = cmd.CreateParameter();
        dbParameter.DbType = dbType;
        dbParameter.ParameterName = parameterName;
        dbParameter.Size = size;
        dbParameter.Direction = ParameterDirection.Output;
        cmd.Parameters.Add(dbParameter);
    }
	/// <summary>
	/// 增加輸出類型
	/// </summary>
	/// <param name="cmd">DbCommand 對象</param>
	/// <param name="parameterName">參數名稱</param>
	/// <param name="dbType">數據類型</param>
	/// <param name="value">字段大小</param>
    public void AddInParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
    {
        DbParameter dbParameter = cmd.CreateParameter();
        dbParameter.DbType = dbType;
        dbParameter.ParameterName = parameterName;
        dbParameter.Value = value;
        dbParameter.Direction = ParameterDirection.Input;
        cmd.Parameters.Add(dbParameter);
    }
	/// <summary>
	/// 格式化參數
	/// </summary>
	/// <param name="parameterName">參數名稱</param>
	/// <param name="dbType">數據類型</param>
	/// <param name="value">參數值</param>
	/// <param name="size">參數大小</param>
	/// <param name="direction"></param>
	/// <returns></returns>
    public DbParameter FormatParameter(string parameterName, DbType dbType,
        object value = null,
        int size = 0,
        ParameterDirection direction = ParameterDirection.Input)
    {
        DbParameter para = null;
        switch (DBType.ToLower())
        {
            case "sqlserver":
                para = new System.Data.SqlClient.SqlParameter(parameterName, value);
                break;
            case "mysql":
                para = new MySql.Data.MySqlClient.MySqlParameter(parameterName, value);
                break;
            case "oracle":
                para = new System.Data.OracleClient.OracleParameter(parameterName, value);
                break;
        }
        if (size > 0)
            para.Size = size;
        para.DbType = dbType;
        para.Direction = direction;
        return para;
    }
	/// <summary>
	/// 增加參數集合
	/// </summary>
	/// <param name="cmd">DbCommand 對象</param>
	/// <param name="param">參數數組</param>
    public void AttachParameters(DbCommand cmd, DbParameter[] param)
    {
        if (param == null || param.Length == 0)
            return;

        foreach (DbParameter p in param)
        {
            //check for derived output value with no value assigned
            if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
            {
                p.Value = DBNull.Value;
            }
            cmd.Parameters.Add(p);
        }
    }
	/// <summary>
	/// 增加返回參數
	/// </summary>
	/// <param name="cmd">DbCommand 對象</param>
	/// <param name="parameterName">參數名</param>
	/// <param name="dbType">參數類型</param>
    public void AddReturnParameter(DbCommand cmd, string parameterName, DbType dbType)
    {
        DbParameter dbParameter = cmd.CreateParameter();
        dbParameter.DbType = dbType;
        dbParameter.ParameterName = parameterName;
        dbParameter.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(dbParameter);
    }
	/// <summary>
	/// 獲取參數
	/// </summary>
	/// <param name="cmd">DbCommand對象</param>
	/// <param name="parameterName">參數名稱</param>
	/// <returns>參數</returns>
    public DbParameter GetParameter(DbCommand cmd, string parameterName)
    {
        return cmd.Parameters[parameterName];
    }

    #endregion

    #region 普通執行
    public DataSet ExecuteDataSet(DbCommand cmd)
    {
        using (cmd)
        {
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            DbProviderFactory dbfactory = GetFactory(DBFactory.dbProviderName);
            DataSet ds = new DataSet();
            using (DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter())
            {
                dbDataAdapter.SelectCommand = cmd;
                dbDataAdapter.Fill(ds);
            }
            cmd.Connection.Close();
            return ds;
        }
    }
    /// <summary>
    /// 執行Sql語句，返回
    /// </summary>
    /// <param name="sql">需要執行的SQL語句</param>
    /// <param name="param">執行參數</param>
    /// <returns></returns>
    public DataSet ExecuteDataSet(string sql, DbParameter[] param)
    {
        DbProviderFactory dbfactory = GetFactory(DBFactory.dbProviderName);
        DataSet ds = new DataSet();
        using (DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter())
        {
            using (DbCommand cmd = GetSqlStringCommond(sql))
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                AttachParameters(cmd, param);
                dbDataAdapter.SelectCommand = cmd;
                dbDataAdapter.Fill(ds);
                cmd.Connection.Close();
            }
        }
        return ds;
    }
    /// <summary>
    /// 執行存儲過程返回DataSet
    /// </summary>
    /// <param name="procName">存儲過程名稱</param>
    /// <param name="param">輸入輸出參數</param>
    /// <returns></returns>
    public DataSet ExecuteProcDataSet(string procName, DbParameter[] param)
    {
        DbProviderFactory dbfactory = GetFactory(DBFactory.dbProviderName);        
        using (DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter())
        {
            using (DbCommand cmd = GetStoredProcCommond(procName))
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                AttachParameters(cmd, param);
                dbDataAdapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                dbDataAdapter.Fill(ds);
                cmd.Connection.Close();
                return ds;
            }
        }
    }

    /// <summary> 
    ///  执行既带返回值又带Dataset（可带可不带参数）的存储过程 ExecuteProDatasetArray
    ///  add by jeven_xiao
    /// </summary>
    /// <param name="procName">存储过程名</param>
    /// <param name="spin">输入参数数组（可选）</param>
    /// <param name="spout">输出参数数组</param>
    public ArrayList ExecuteProDatasetArray(string procName, DbParameter[] spout, DbParameter[] spin = null)
    {
        DbProviderFactory dbfactory = GetFactory(DBFactory.dbProviderName);
        using (DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter())
        {
            using (DbCommand cmd = GetStoredProcCommond(procName))
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                if (spin != null)
                {
                    AttachParameters(cmd, spin);
                }
                if (spout != null)
                {
                    AttachParameters(cmd, spout);
                }
                dbDataAdapter.SelectCommand = cmd;
                ArrayList al = new ArrayList();
                DataSet ds = new DataSet();
                dbDataAdapter.Fill(ds);
                al.Add(spout);
                al.Add(ds);
                cmd.Connection.Close();
                return al;
            }
        }

    }
	/// <summary>
	/// 對象轉換類
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class ConvertTo<T> where T : new()
    {

        #region dataset转实体类
        public static IList<T> FillModel(DataTable dt)
        {
            List<T> l = new List<T>();
            T model = default(T);

            if (dt.Columns[0].ColumnName == "rowId")
            {
                dt.Columns.Remove("rowId");
            }

            foreach (DataRow dr in dt.Rows)
            {
                model = Activator.CreateInstance<T>();
                List<PropertyInfo> pList = model.GetType().GetProperties().ToList();
                foreach (DataColumn dc in dr.Table.Columns)
                {
                    PropertyInfo pi = pList.Find(o => o.Name.ToLower() == dc.ColumnName.ToLower());
                    if (dr[dc.ColumnName] != null && dr[dc.ColumnName] != DBNull.Value)
                    {
                        try
                        {
                            Type t = dr[dc.ColumnName].GetType();
                            if ((t.FullName == "System.Int32") || (t.FullName == "System.Decimal") || (t.FullName == "System.Byte") || (t.FullName == "System.Double") || (t.FullName == "System.Int16"))
                            {
                                pi.SetValue(model, Convert.ToInt32(dr[dc.ColumnName].ToString()), null);
                            }
                            else
                            {
                                pi.SetValue(model, dr[dc.ColumnName], null);
                            }
                        }
                        catch
                        {
                            // System.Web.HttpContext.Current.Response.Write(dc.ColumnName);
                        }
                    }
                    else
                        pi.SetValue(model, null, null);

                }
                l.Add(model);
            }

            return l;
        }

        /// <summary>
        /// 将datatable的第一行给实体类中对应的属性赋值

        /// </summary>
        /// <param name="model"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T FillModel(T model, DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                List<PropertyInfo> pList = model.GetType().GetProperties().ToList();
                foreach (DataColumn dc in dr.Table.Columns)
                {
                    PropertyInfo pi = pList.Find(o => o.Name.ToLower() == dc.ColumnName.ToLower());
                    if (pi != null)
                    {
                        if (dr[dc.ColumnName] != DBNull.Value)
                        {
                            Type t = dr[dc.ColumnName].GetType();
                            if ((t.FullName == "System.Int32") || (t.FullName == "System.Decimal") || (t.FullName == "System.Byte") || (t.FullName == "System.Double") || (t.FullName == "System.Int16"))
                            {
                                pi.SetValue(model, Convert.ToInt32(dr[dc.ColumnName].ToString()), null);
                            }
                            else
                            {
                                pi.SetValue(model, dr[dc.ColumnName], null);
                            }
                        }
                        else
                            pi.SetValue(model, null, null);
                    }
                }

            }
            return model;

        }


        //dataset转实体类 
        public static IList<T> FillModel(DataSet ds)
        {
            return FillModel(ds.Tables[0]);
        }
        #endregion

    }


	/// <summary>
	/// 返回datatable數據集
	/// </summary>
	/// <param name="cmd"></param>
	/// <returns></returns>
    public DataTable ExecuteDataTable(DbCommand cmd)
    {
        using (cmd)
        {
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            DbProviderFactory dbfactory = GetFactory(DBFactory.dbProviderName);
            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataTable dataTable = new DataTable();
            dbDataAdapter.Fill(dataTable);
            cmd.Connection.Close();
            return dataTable;
        }
    }

    public DbDataReader ExecuteReader(DbCommand cmd)
    {
        throw new Exception("not using");
        //using (cmd)
        //{
        //    if (cmd.Connection.State == ConnectionState.Closed)
        //        cmd.Connection.Open();
        //    DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //    return reader;
        //}
    }
	/// <summary>
	/// 執行SQL返回受影響的記錄
	/// </summary>
	/// <param name="cmd"></param>
	/// <returns></returns>
    public int ExecuteNonQuery(DbCommand cmd)
    {
        using (cmd)
        {
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            int ret = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return ret;
        }
    }

    /// <summary>
    /// 執行Sql語句
    /// </summary>
    /// <param name="sql">sql語句</param>
    /// <param name="param">參數</param>
    /// <returns></returns>
    public int ExecuteNonQuery(string sql, DbParameter[] param)
    {
        DbProviderFactory dbfactory = GetFactory(DBFactory.dbProviderName);
        using (DbCommand cmd = GetSqlStringCommond(sql))
        {
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            AttachParameters(cmd, param);
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
            int ret = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return ret;
        }
    }
    /// <summary>
    /// 執行存儲過程
    /// </summary>
    /// <param name="procName">存儲過程名稱</param>
    /// <param name="param">輸入輸出參數</param>
    /// <returns></returns>
    public int ExecuteProcNonQuery(string procName, DbParameter[] param)
    {
        DbProviderFactory dbfactory = GetFactory(DBFactory.dbProviderName);
        using (DbCommand cmd = GetStoredProcCommond(procName))
        {
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            AttachParameters(cmd, param);
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
            int ret = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return ret;
        }
    }
	/// <summary>
	/// 執行SQL查詢，返回記錄
	/// </summary>
	/// <param name="cmd"></param>
	/// <returns></returns>
    public object ExecuteScalar(DbCommand cmd)
    {
        using (cmd)
        {
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            object ret = cmd.ExecuteScalar();
            cmd.Connection.Close();
            return ret;
        }
    }
    /// <summary>
    /// 執行sql語句
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public object ExecuteScalar(string sql, DbParameter[] param)
    {
        DbProviderFactory dbfactory = GetFactory(DBFactory.dbProviderName);
        using (DbCommand cmd = GetSqlStringCommond(sql))
        {
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            AttachParameters(cmd, param);
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
            object ret = cmd.ExecuteScalar();
            cmd.Connection.Close();
            return ret;
        }
    }
    /// <summary>
    /// 執行存儲過程，返回受影響的行數
    /// </summary>
    /// <param name="procName"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public object ExecuteProcScalar(string procName, DbParameter[] param)
    {
        DbProviderFactory dbfactory = GetFactory(DBFactory.dbProviderName);
        using (DbCommand cmd = GetStoredProcCommond(procName))
        {
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            AttachParameters(cmd, param);
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
            object ret = cmd.ExecuteScalar();
            cmd.Connection.Close();
            return ret;
        }
    }
    #endregion

    #region 事物
	/// <summary>
	/// 返回數據集dataset
	/// </summary>
	/// <param name="cmd"></param>
	/// <param name="t"></param>
	/// <returns></returns>
    public DataSet ExecuteDataSet(DbCommand cmd, Trans t)
    {
        cmd.Connection = t.DbConnection;
        cmd.Transaction = t.DbTrans;
        if (cmd.Connection.State == ConnectionState.Closed)
            cmd.Connection.Open();
        DbProviderFactory dbfactory = GetFactory(DBFactory.dbProviderName);
        DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
        dbDataAdapter.SelectCommand = cmd;
        DataSet ds = new DataSet();
        dbDataAdapter.Fill(ds);
        return ds;
    }

    /// <summary>
    /// 返回帶參數集合，帶事物的數據集
    /// </summary>
    /// <param name="sql">執行的SQL語句</param>
    /// <param name="param">參數集合</param>
    /// <param name="t">事物對象</param>
    /// <returns></returns>
    public DataSet ExecuteDataSet(string sql, DbParameter[] param, Trans t)
    {
        DbCommand cmd = GetSqlStringCommond(sql, t.DbConnection);
        AttachParameters(cmd, param);
        cmd.Transaction = t.DbTrans;
        if (cmd.Connection.State == ConnectionState.Closed)
            cmd.Connection.Open();
        DbProviderFactory dbfactory = GetFactory(DBFactory.dbProviderName);
        DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
        dbDataAdapter.SelectCommand = cmd;
        DataSet ds = new DataSet();
        dbDataAdapter.Fill(ds);
        return ds;
    }
	/// <summary>
	/// 返回帶事物執行的datatable
	/// </summary>
	/// <param name="cmd">DbCommand 對象</param>
	/// <param name="t">事物對象</param>
	/// <returns>DataTable</returns>
    public DataTable ExecuteDataTable(DbCommand cmd, Trans t)
    {
        cmd.Connection = t.DbConnection;
        cmd.Transaction = t.DbTrans;
        if (cmd.Connection.State == ConnectionState.Closed)
            cmd.Connection.Open();
        DbProviderFactory dbfactory = GetFactory(DBFactory.dbProviderName);
        DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
        dbDataAdapter.SelectCommand = cmd;
        DataTable dataTable = new DataTable();
        dbDataAdapter.Fill(dataTable);
        return dataTable;
    }
	/// <summary>
	/// 返回datareader
	/// </summary>
	/// <param name="cmd">DbCommand對象</param>
	/// <param name="t">事物對象</param>
	/// <returns>DbDataReader</returns>
    public DbDataReader ExecuteReader(DbCommand cmd, Trans t)
    {
        cmd.Connection = t.DbConnection;
        cmd.Transaction = t.DbTrans;
        if (cmd.Connection.State == ConnectionState.Closed)
            cmd.Connection.Open();
        DbDataReader reader = cmd.ExecuteReader();
        DataTable dt = new DataTable();
        return reader;
    }
	/// <summary>
	/// 執行SQL 返回受影響的行數
	/// </summary>
	/// <param name="cmd">DbCommand</param>
	/// <param name="t">事物</param>
	/// <returns></returns>
    public int ExecuteNonQuery(DbCommand cmd, Trans t)
    {
        cmd.Connection = t.DbConnection;
        cmd.Transaction = t.DbTrans;
        int ret = cmd.ExecuteNonQuery();
        return ret;
    }
	/// <summary>
	/// 參數化執行SQL
	/// </summary>
	/// <param name="sql">sql語句</param>
	/// <param name="param">參數</param>
	/// <param name="t">事物對象</param>
	/// <returns></returns>
    public int ExecuteNonQuery(string sql, DbParameter[] param, Trans t)
    {
        DbCommand cmd = GetSqlStringCommond(sql, t.DbConnection);
        AttachParameters(cmd, param);
        cmd.Transaction = t.DbTrans;
        if (cmd.Connection.State == ConnectionState.Closed)
            cmd.Connection.Open();
        int ret = cmd.ExecuteNonQuery();
        return ret;
    }
	/// <summary>
	/// 事物執行SQL 返回記錄
	/// </summary>
	/// <param name="cmd"></param>
	/// <param name="t"></param>
	/// <returns></returns>
    public object ExecuteScalar(DbCommand cmd, Trans t)
    {
        cmd.Connection = t.DbConnection;
        cmd.Transaction = t.DbTrans;
        if (cmd.Connection.State == ConnectionState.Closed)
            cmd.Connection.Open();
        object ret = cmd.ExecuteScalar();
        return ret;
    }
	/// <summary>
	/// 執行參數化帶事物的SQL語句
	/// </summary>
	/// <param name="sql">SQL語句</param>
	/// <param name="param">參數集合</param>
	/// <param name="t">事物對象</param>
	/// <returns>object</returns>
    public object ExecuteScalar(string sql, DbParameter[] param, Trans t)
    {
        DbCommand cmd = GetSqlStringCommond(sql, t.DbConnection);
        AttachParameters(cmd, param);
        cmd.Transaction = t.DbTrans;
        if (cmd.Connection.State == ConnectionState.Closed)
            cmd.Connection.Open();
        object ret = cmd.ExecuteScalar();
        return ret;
    }
    #endregion
}