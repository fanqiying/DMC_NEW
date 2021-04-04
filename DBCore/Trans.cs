using System;
using System.Data.Common;

/// <summary>
/// 事物操作類
/// </summary>
public class Trans : IDisposable
{
	/// <summary>
	/// 變量定義
	/// </summary>
    private DbConnection conn;
    private DbTransaction dbTrans;
	/// <summary>
	/// db連接對象
	/// </summary>
    public DbConnection DbConnection
    {
        get { return this.conn; }
    }
	/// <summary>
	/// db事物對象
	/// </summary>
    public DbTransaction DbTrans
    {
        get { return this.dbTrans; }
    }
	/// <summary>
	/// 事物構造函數
	/// </summary>
    public Trans()
    {
        conn = DBFactory.CreateConnection();
        conn.Open();
        dbTrans = conn.BeginTransaction();
    }
	/// <summary>
	/// 建立事物對象
	/// </summary>
	/// <param name="connectionString"></param>
    public Trans(string connectionString)
    {
        conn = DBFactory.CreateConnection(connectionString);
        conn.Open();
        dbTrans = conn.BeginTransaction();
    }
	/// <summary>
	/// 事物提交方法
	/// </summary>
    public void Commit()
    {
        dbTrans.Commit();
        this.Colse();
    }
	/// <summary>
	/// 事物回滾方法
	/// </summary>
    public void RollBack()
    {
        dbTrans.Rollback();
        this.Colse();
    }
	/// <summary>
	/// 事物釋放方法
	/// </summary>
    public void Dispose()
    {
        this.Colse();
    }
	/// <summary>
	/// 關閉事物
	/// </summary>
    public void Colse()
    {
        if (conn.State == System.Data.ConnectionState.Open)
        {
            conn.Close();
        }
    }
}
