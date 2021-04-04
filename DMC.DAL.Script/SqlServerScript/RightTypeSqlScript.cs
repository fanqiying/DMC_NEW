using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.DAL.Script.SqlServer
{
    public class RightTypeSqlScript : IRightType
    {
        /// <summary>
        /// 检查权限类别编号是否存在
        /// </summary>
        private string _existsRoseId = @" SELECT COUNT(*) FROM t_Right_Rose WHERE RoseId=@RoseId; ";
        /// <summary>
        /// 检查权限类别编号是否存在
        /// </summary>
        public string ExistsRoseId
        {
            get { return _existsRoseId; }
        }

        private string _existsRoseName = @" SELECT COUNT(*) FROM t_Right_Rose WHERE RoseName=@RoseName and RoseId<>@RoseId; ";
        /// <summary>
        /// 檢查權限名稱是否存在
        /// </summary>
        public string ExistsRoseName
        {
            get { return _existsRoseName; }
        }
        /// <summary>
        /// 创建权限类别
        /// </summary>
        private string _createRoseType = @" INSERT INTO t_Right_Rose(RoseId,RoseName,Usy,SystemType,CreateDeptId,CreateUserId,CreateTime) " +
                                          " VALUES(@RoseId,@RoseName,@Usy,@SystemType,@CreateDeptId,@CreateUserId,@CreateTime); ";
        /// <summary>
        /// 创建权限类别
        /// </summary>
        public string CreateRoseType
        {
            get { return _createRoseType; }
        }
        /// <summary>
        /// 刪除權限類別
        /// </summary>
        private string _deleteRoseType = @" DELETE t_Right_Rose WHERE RoseId=@RoseId; ";
        /// <summary>
        /// 刪除權限類別
        /// </summary>
        public string DeleteRoseType
        {
            get { return _deleteRoseType; }
        }
        /// <summary>
        /// 刪除權限類別的程式權限
        /// </summary>
        private string _deleteRightRProgram = @" DELETE t_Right_RProgram WHERE RoseId=@RoseId; ";
        /// <summary>
        /// 刪除權限類別的程式權限
        /// </summary>
        public string DeleteRightRProgram
        {
            get { return _deleteRightRProgram; }
        }
        /// <summary>
        /// 刪除權限類別的程式操作權限
        /// </summary>
        private string _deleteRightRPAction = @" DELETE t_Right_RPAction WHERE RoseId=@RoseId; ";
        /// <summary>
        /// 刪除權限類別的程式操作權限
        /// </summary>
        public string DeleteRightRPAction
        {
            get { return _deleteRightRPAction; }
        }
        /// <summary>
        /// 刪除權限類別資料權限
        /// </summary>
        private string _deleteRightRData = @" DELETE t_Right_RData WHERE RoseId=@RoseId;";
        /// <summary>
        /// 刪除權限類別資料權限
        /// </summary>
        public string DeleteRightRData
        {
            get { return _deleteRightRData; }
        }
        /// <summary>
        /// 刪除權限類別供應商權限
        /// </summary>
        private string _deleteRightRsupply = @" DELETE t_Right_Rsupply WHERE RoseId=@RoseId; ";
        /// <summary>
        /// 刪除權限類別供應商權限
        /// </summary>
        public string DeleteRightRsupply
        {
            get { return _deleteRightRsupply; }
        }
        /// <summary>
        /// 修改角色類別
        /// </summary>
        private string _modifyRose = @" UPDATE t_Right_Rose " +
                                         " SET RoseName=@RoseName," +
                                             " Usy=@Usy," +
                                             " SystemType=@SystemType," +
                                             " UpdateUserId=@UpdateUserId," +
                                             " UpdateDeptId=@UpdateDeptId," +
                                             " UpdateTime=@UpdateTime " +
                                       " WHERE RoseId=@RoseId;";
        /// <summary>
        /// 修改角色類別
        /// </summary>
        public string ModifyRose
        {
            get { return _modifyRose; }
        }
        /// <summary>
        /// 權限類別用戶列表
        /// </summary>
        private string _roseUserList = @" SELECT A.UserId,B.UserName,B.UserDept FROM t_Right_URole A LEFT JOIN t_User B ON A.UserId=B.userID WHERE A.RoseId=@RoseId;";
        /// <summary>
        /// 權限類別用戶列表
        /// </summary>
        public string RoseUserList
        {
            get { return _roseUserList; }
        }
        /// <summary>
        /// 檢查角色是否存在授權
        /// </summary>
        private string _roseExitsRight = @" SELECT SUM(Number) total FROM (" +
                                          " SELECT COUNT(RoseId) Number FROM t_Right_RProgram WHERE RoseId=@RoseId UNION ALL " +
                                          " SELECT COUNT(RoseId) Number FROM t_Right_RPAction WHERE RoseId=@RoseId UNION ALL " +
                                          " SELECT COUNT(RoseId) Number FROM t_Right_RData WHERE RoseId=@RoseId UNION ALL " +
                                          " SELECT COUNT(RoseId) Number FROM t_Right_Rsupply WHERE RoseId=@RoseId) AS A;";
        /// <summary>
        /// 檢查角色是否存在授權
        /// </summary>
        public string RoseExitsRight
        {
            get { return _roseExitsRight; }
        }
        /// <summary>
        /// 複製程式權限
        /// </summary>
        private string _copyRightRProgram = @" INSERT INTO t_Right_RProgram(ProgramId,RoseId) SELECT A.ProgramId,@RoseIdAim FROM t_Right_RProgram A WHERE A.RoseId=@RoseIdSocure; ";
        /// <summary>
        /// 複製程式權限
        /// </summary>
        public string CopyRightRProgram
        {
            get { return _copyRightRProgram; }
        }
        /// <summary>
        /// 複製程式操作權限
        /// </summary>
        private string _copyRightRPAction = @" INSERT INTO t_Right_RPAction(ActionId,ProgramId,RoseId) SELECT ActionId,ProgramId,@RoseIdAim FROM t_Right_RPAction WHERE RoseId=@RoseIdSocure; ";
        /// <summary>
        /// 複製程式操作權限
        /// </summary>
        public string CopyRightRPAcion
        {
            get { return _copyRightRPAction; }
        }
        /// <summary>
        /// 複製資料權限
        /// </summary>
        private string _copyRightRData = @" INSERT INTO t_Right_RData(DeptId,IsAll,RoseId) SELECT A.DeptId,A.IsAll,@RoseIdAim FROM t_Right_RData A WHERE A.RoseId=@RoseIdSocure; ";
        /// <summary>
        /// 複製資料權限
        /// </summary>
        public string CopyRightRData
        {
            get { return _copyRightRData; }
        }
        /// <summary>
        /// 複製供應商權限
        /// </summary>
        private string _copyRightRsupply = @" INSERT INTO t_Right_Rsupply(SupplyId,RoseId) SELECT SupplyId,@RoseIdAim FROM t_Right_Rsupply WHERE RoseId=@RoseIdSocure; ";
        /// <summary>
        /// 複製供應商權限
        /// </summary>
        public string CopyRightRsupply
        {
            get { return _copyRightRsupply; }
        }

        private string _rightIsUsing = @" SELECT COUNT(RoseId) Number FROM t_Right_URole WHERE RoseId in (SELECT SplitValue AS RoseId FROM f_SplitConvert(@RoseIdList,','));";
        /// <summary>
        /// 判斷權限是否處於使用中
        /// </summary>
        public string RightIsUsing
        {
            get { return _rightIsUsing; }
        }

        private string _rightIsUsingId = @" SELECT  distinct  RoseId FROM t_Right_URole WHERE RoseId in (SELECT SplitValue AS RoseId FROM f_SplitConvert(@RoseIdList,','));";
        /// <summary>
        /// 判斷權限是否處於使用中
        /// </summary>
        public string RightIsUsingId
        {
            get { return _rightIsUsingId; }
        }



        private string _getAllType = @" SELECT RoseId , RoseName From t_Right_Rose where Usy='Y';";
        public string GetAllType
        {
            get { return _getAllType; }
        }
    }
}
