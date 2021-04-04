using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.DAL.Script.SqlServer
{
    public class SupplyRightSqlScript : ISupplyRight
    {
        private string _addSupplyRightByRose = @" INSERT INTO t_Right_Rsupply(RoseId,SupplyId) VALUES(@RoseId,@SupplyId); ";
        /// <summary>
        /// 權限類別添加供應商權限
        /// </summary>
        public string AddSupplyRightByRose
        {
            get { return _addSupplyRightByRose; }
        }

        private string _addSupplyRightByUser = @" INSERT INTO t_Right_Usupply(UserId,SupplyId) VALUES(@UserId,@SupplyId); ";
        /// <summary>
        /// 使用者添加供應商權限
        /// </summary>
        public string AddSupplyRightByUser
        {
            get { return _addSupplyRightByUser; }
        }

        private string _deleteSupplyRightByRose = @" DELETE t_Right_Rsupply WHERE RoseId=@RoseId AND SupplyId=@SupplyId ";
        /// <summary>
        /// 刪除權限類別的供應商權限
        /// </summary>
        public string DeleteSupplyRightByRose
        {
            get { return _deleteSupplyRightByRose; }
        }

        private string _deleteSupplyRightByUser = @" DELETE t_Right_Usupply WHERE UserId=@UserId ";
        /// <summary>
        /// 刪除使用者的供應商權限
        /// </summary>
        public string DeleteSupplyRightByUser
        {
            get { return _deleteSupplyRightByUser; }
        }

        private string _readSupplyRightByRose = @" SELECT A.suppNumber,A.suppName,ISNULL(B.RoseId,'') RoseId, " +
                                                        " (CASE ISNULL(B.RoseId,'') WHEN '' THEN 'N' ELSE 'Y' END) Usy " +
                                                   " FROM t_Supply A LEFT JOIN t_Right_Rsupply B ON A.suppNumber=B.SupplyId AND B.RoseId= @RoseId ";
        //" WHERE (@CompanyId='' OR A.companyID=@CompanyId) ";
        /// <summary>
        /// 獲取權限類別的供應商權限
        /// </summary>
        public string ReadSupplyRightByRose
        {
            get { return _readSupplyRightByRose; }
        }

        private string _readSupplyRightByUser = @" SELECT A.suppNumber,A.suppName,ISNULL(B.UserId,'') UserId, " +
                                                        " (CASE ISNULL(B.UserId,'') WHEN '' THEN 'N' ELSE 'Y' END) Usy " +
                                                   " FROM t_Supply A LEFT JOIN t_Right_Usupply B ON A.suppNumber=B.SupplyId AND B.UserId= @UserId ";
        //" WHERE (@CompanyId='' OR A.companyID=@CompanyId) ";
        /// <summary>
        /// 獲取使用者的供應商權限
        /// </summary>
        public string ReadSupplyRightByUser
        {
            get { return _readSupplyRightByUser; }
        }

        private string _existsSupplyId = @" SELECT COUNT(0) FROM t_Supply WHERE suppNumber=@SupplyId;";
        public string ExistsSupplyId
        {
            get { return _existsSupplyId; }
        }
    }
}
