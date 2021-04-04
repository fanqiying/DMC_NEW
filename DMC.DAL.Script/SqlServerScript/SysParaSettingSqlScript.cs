using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.DAL.Script.SqlServer
{
    /// <summary>
    /// 系統參數設置
    /// </summary>
    public class SysParaSettingSqlScript : ISysParaSetting
    {
        private string _Add = "INSERT INTO t_SysParaSetting(CompanyId,ParaKey,ParaName,ParaContent,ParaDesc,Usey,InUser,InTime)VALUES(@CompanyId,@ParaKey,@ParaName,@ParaContent,@ParaDesc,@Usey,@InUser,GetDate());";
        /// <summary>
        /// 新增系統參數
        /// </summary>
        public string Add
        {
            get { return _Add; }
        }

        private string _Edit = "UPDATE t_SysParaSetting SET ParaName=@ParaName,ParaContent=@ParaContent,ParaDesc=@ParaDesc,Usey=@Usey,InUser=@InUser,InTime=GetDate() WHERE CompanyId=@CompanyId AND ParaKey=@ParaKey;";
        /// <summary>
        /// 修改系統參數
        /// </summary>
        public string Edit
        {
            get { return _Edit; }
        }

        private string _Delete = "DELETE FROM t_SysParaSetting WHERE CompanyId=@CompanyId AND ParaKey=@ParaKey;";
        /// <summary>
        /// 刪除系統參數
        /// </summary>
        public string Delete
        {
            get { return _Delete; }
        }

        public string _Exists = "SELECT CompanyId,ParaKey,ParaContent FROM t_SysParaSetting WHERE CompanyId='ALL' AND ParaKey=@ParaKey UNION  SELECT CompanyId,ParaKey,ParaContent FROM t_SysParaSetting WHERE CompanyId=@CompanyId AND ParaKey=@ParaKey;";
        /// <summary>
        /// 驗證系統參數是否存在(如果公司別為ALL，則只判斷關鍵字是否存在，如果公司別不為ALL，則驗證公司+關鍵字是否重複)
        /// </summary>
        public string Exists
        {
            get { return _Exists; }
        }
    }
}
