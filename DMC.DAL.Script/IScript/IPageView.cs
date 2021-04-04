
namespace DMC.DAL.Script
{
    public interface IPageView
    {
        /// <summary>
        /// 獲取記錄總數
        /// </summary>
        string GetTotalData { get; }
        /// <summary>
        /// 查詢數據
        /// </summary>
        string GetData { get; }
        /// <summary>
        /// 根據行號進行分页處理
        /// </summary>
        string GetDataByIntId { get; }
    }
}
