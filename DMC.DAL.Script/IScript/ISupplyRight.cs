
namespace DMC.DAL.Script
{
    public interface ISupplyRight
    {
        /// <summary>
        /// 權限類別添加供應商權限
        /// </summary>
        string AddSupplyRightByRose { get; }
        /// <summary>
        /// 使用者添加供應商權限
        /// </summary>
        string AddSupplyRightByUser { get; }
        /// <summary>
        /// 刪除權限類別的供應商權限
        /// </summary>
        string DeleteSupplyRightByRose { get; }
        /// <summary>
        /// 刪除使用者的供應商權限
        /// </summary>
        string DeleteSupplyRightByUser { get; }
        /// <summary>
        /// 獲取權限類別的供應商權限
        /// </summary>
        string ReadSupplyRightByRose { get; }
        /// <summary>
        /// 獲取使用者的供應商權限
        /// </summary>
        string ReadSupplyRightByUser { get; }
        /// <summary>
        /// 检查供应商是否存在
        /// </summary>
        string ExistsSupplyId { get; }
    }
}
