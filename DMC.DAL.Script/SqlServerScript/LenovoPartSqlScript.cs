using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.DAL.Script.SqlServer
{
    public class LenovoPartSqlScript:ILenovoPart
    {
        private string _Add = @"INSERT INTO t_LenovoPart(CompanyId,PartId,CusPartId,CommCode,PartDesc,Usey,IsUpload,IsPost,Remark,InUser,InTime)
VALUES(@CompanyId,@PartId,@CusPartId,@CommCode,@PartDesc,@Usey,@IsUpload,'N',@Remark,@InUser,GetDate())";
        /// <summary>
        /// 新增
        /// </summary>
        public string Add
        {
            get { return _Add; }
        }

        private string _Edit = @"UPDATE t_LenovoPart 
SET CusPartId=@CusPartId,CommCode=@CommCode,PartDesc=@PartDesc,Usey=@Usey,
    IsUpload=@IsUpload,Remark=@Remark,UpUser=@UpUser,UpTime=GETDATE()
WHERE CompanyId=@CompanyId AND PartId=@PartId ";
        /// <summary>
        /// 修改
        /// </summary>
        public string Edit
        {
            get { return _Edit; }
        }

        private string _Delete = "DELETE FROM t_LenovoPart WHERE CompanyId=@CompanyId AND PartId=@PartId;";
        /// <summary>
        /// 刪除
        /// </summary>
        public string Delete
        {
            get { return _Delete; }
        }

        public string _Exists = "SELECT * FROM t_LenovoPart WHERE CompanyId=@CompanyId AND PartId=@PartId";
        /// <summary>
        /// 驗證是否存在
        /// </summary>
        public string Exists
        {
            get { return _Exists; }
        }

    }
}
