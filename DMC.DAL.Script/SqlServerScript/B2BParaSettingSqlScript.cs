using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.DAL.Script.SqlServer
{
    public class B2BParaSettingSqlScript : IB2BParaSetting
    {
        private string _Add = "INSERT INTO t_B2BParaSetting(CompanyId,ParaKey,ParaName,ParaContent,ParaDesc,Usey,InUser,InTime)VALUES(@CompanyId,@ParaKey,@ParaName,@ParaContent,@ParaDesc,@Usey,@InUser,GetDate());";
        /// <summary>
        /// 新增系統參數
        /// </summary>
        public string Add
        {
            get { return _Add; }
        }

        private string _Edit = "UPDATE t_B2BParaSetting SET ParaName=@ParaName,ParaContent=@ParaContent,ParaDesc=@ParaDesc,Usey=@Usey,InUser=@InUser,InTime=GetDate() WHERE CompanyId=@CompanyId AND ParaKey=@ParaKey;";
        /// <summary>
        /// 修改系統參數
        /// </summary>
        public string Edit
        {
            get { return _Edit; }
        }

        private string _Delete = "DELETE FROM t_B2BParaSetting WHERE CompanyId=@CompanyId AND ParaKey=@ParaKey;";
        /// <summary>
        /// 刪除系統參數
        /// </summary>
        public string Delete
        {
            get { return _Delete; }
        }

        public string _Exists = "SELECT CompanyId,ParaKey,ParaContent FROM t_B2BParaSetting WHERE CompanyId='ALL' AND ParaKey=@ParaKey UNION  SELECT CompanyId,ParaKey,ParaContent FROM t_B2BParaSetting WHERE CompanyId=@CompanyId AND ParaKey=@ParaKey;";
        /// <summary>
        /// 驗證系統參數是否存在(如果公司別為ALL，則只判斷關鍵字是否存在，如果公司別不為ALL，則驗證公司+關鍵字是否重複)
        /// </summary>
        public string Exists
        {
            get { return _Exists; }
        }


        public string _VolidPara = "SELECT CompanyId,ParaKey,ParaContent,usey FROM t_B2BParaSetting WHERE CompanyId='ALL' AND ParaKey=@ParaKeys UNION  SELECT CompanyId,ParaKey,ParaContent,usey FROM t_B2BParaSetting WHERE CompanyId=@CompanyIds AND ParaKey=@ParaKeys;";
        /// <summary>
        /// 驗證出貨invoice參數設置參數是否為Y 2016/09/01)
        /// </summary>
        public string VolidPara
        {
            get { return _VolidPara; }
        }
    }
}
