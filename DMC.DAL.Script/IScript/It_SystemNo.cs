namespace DMC.DAL.Script
{
    public interface It_SystemNo
    {
        /// <summary>
        /// 添加
        /// <summary>
        string Addt_SystemNo { get; }
        /// <summary>
        /// 修改
        /// <summary>
        string Modt_SystemNo { get; }
        /// <summary>
        /// 刪除
        /// <summary>
        string Delt_SystemNo { get; }
        /// <summary>
        /// 判斷是否存在
        /// <summary>
        string IsExitt_SystemNo { get; }
        /// <summary>
        /// 根據查詢條件，返回結果集
        /// <summary>
        string GetListt_SystemNo { get; }
        /// <summary>
        /// 取得所有數據
        /// <summary>
        string GetAllt_SystemNo { get; }
        /// <summary>
        /// 獲取對應規則
        /// </summary>
        string GeneratSystemNo { get; }
        /// <summary>
        /// 更新流水號
        /// </summary>
        string UpdateCode { get; }
        /// <summary>
        /// 獲取程式對應的單據別列表
        /// </summary>
        string GetReceiptList { get; }
        /// <summary>
        /// 判斷產生單據的規則是否重複
        /// </summary>
        string IsRepeatRule { get; }
        /// <summary>
        /// 無區分公司別的單據
        /// </summary>
        string GlobelNo { get; }
        /// <summary>
        /// 根据类别生成单据
        /// </summary>
        string GetCategoryNo { get; }
        /// <summary>
        /// 固定增长步长1
        /// </summary>
        string StepAddCode { get; }
    }
}
