
namespace DMC.DAL.Script
{
    public interface ILanguage
    {
        /// <summary>
        /// 創建數據
        /// </summary>
        string CreateLanguageResources { get; }
        /// <summary>
        /// 保存多語言的值
        /// </summary>
        string CreateLanguageValue { get; }
        /// <summary>
        /// 清空多語言的值
        /// </summary>
        string ClearLanguageValue { get; }
        /// <summary>
        /// 驗證Key是否已存在
        /// </summary>
        string ExistKey { get; }
        /// <summary>
        /// 修改數據
        /// </summary>
        string ModifyLanguage { get; }
        /// <summary>
        /// 通過Key刪除數據
        /// </summary>
        string DeleteLanguageByKey { get; }
        /// <summary>
        /// 通過AutoID刪除數據
        /// </summary>
        string DeleteLanguageByAutoId { get; } 
        /// <summary>
        /// 添加語言類別
        /// </summary>
        string AddLanguageType { get; }
        /// <summary>
        /// 刪除語言類別
        /// </summary>
        string DeleteLanguageType { get; }
        /// <summary>
        /// 更新語言類別
        /// </summary>
        string UpdateLanguageType { get; }
        /// <summary>
        /// 獲取語言類別
        /// </summary>
        string GetAllLanguageType { get; }
        /// <summary>
        /// 語言類別是否存在
        /// </summary>
        string ExistsLanguageType { get; }
        /// <summary>
        /// 獲取對應資源的文本內容
        /// </summary>
        string GetResourceText { get; }
        /// <summary>
        /// 獲取下拉框對應的選項
        /// </summary>
        string GetResourceSelect { get; }
        /// <summary>
        /// 獲取多語言項對應的值
        /// </summary>
        string GetResourceValues { get; }
        /// <summary>
        /// 獲取對應語言的值
        /// </summary>
        string GetResourceByType { get; }
        /// <summary>
        /// 檢查默認值是否存在
        /// </summary>
        string ExistsDefaultValue { get; }
    }
}
