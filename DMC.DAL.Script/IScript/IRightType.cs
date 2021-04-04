
namespace DMC.DAL.Script
{
    public interface IRightType
    {
        /// <summary>
        /// 判斷權限類別編號是否存在
        /// </summary>
        string ExistsRoseId { get; }
        /// <summary>
        /// 創建權限類別
        /// </summary>
        string CreateRoseType { get; }
        /// <summary>
        /// 刪除權限類別
        /// </summary>
        string DeleteRoseType { get; }
        /// <summary>
        /// 修改權限類別
        /// </summary>
        string ModifyRose { get; }
        /// <summary>
        /// 权限类别所对应的用户列表
        /// </summary>
        string RoseUserList { get; }
        /// <summary>
        /// 删除权限类别的程式权限
        /// </summary>
        string DeleteRightRProgram { get; }
        /// <summary>
        /// 删除权限类别的程式操作权限
        /// </summary>
        string DeleteRightRPAction { get; }
        /// <summary>
        /// 删除资料权限
        /// </summary>
        string DeleteRightRData { get; }
        /// <summary>
        /// 删除供应商权限
        /// </summary>
        string DeleteRightRsupply { get; }
        /// <summary>
        /// 驗證權限類別是否存在授權
        /// </summary>
        string RoseExitsRight { get; }
        /// <summary>
        /// 複製程式權限
        /// </summary>
        string CopyRightRProgram { get; }
        /// <summary>
        /// 複製程式操作權限
        /// </summary>
        string CopyRightRPAcion { get; }
        /// <summary>
        /// 複製資料權限
        /// </summary>
        string CopyRightRData { get; }
        /// <summary>
        /// 複製供應商權限
        /// </summary>
        string CopyRightRsupply { get; }
        /// <summary>
        /// 權限編號是否處於使用中
        /// </summary>
        string RightIsUsing { get; }
        /// <summary>
        /// 權限編號是否處於使用中,并返回id
        /// </summary>
        string RightIsUsingId { get; }
        /// <summary>
        /// 檢查權限類別名稱是否存在
        /// </summary>
        string ExistsRoseName { get; }
        /// <summary>
        /// 獲取所有權限類型
        /// </summary>
        string GetAllType { get; }
    }
}
