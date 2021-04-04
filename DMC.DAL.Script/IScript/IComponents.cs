namespace DMC.DAL.Script
{
    /// <summary>
    /// 組建信息維護
    /// </summary>
    public interface IComponents
    {
        /// <summary>
        /// 添加
        /// <summary>
        string Addt_sComponents { get; }
        /// <summary>
        /// 修改
        /// <summary>
        string Modt_sComponents { get; }
        /// <summary>
        /// 刪除
        /// <summary>
        string Delt_sComponents { get; }
        /// <summary>
        /// 刪除任務列表
        /// </summary>
        string DelTaskIDList { get; }
        /// <summary>
        /// 判斷是否存在
        /// <summary>
        string IsExitt_sComponents { get; }
        /// <summary>
        /// 獲取步驟列表
        /// <summary>
        string GetListt_sComponents { get; }
        /// <summary>
        /// 獲取所有資料列表
        /// <summary>
        string GetAllt_sComponents { get; }
        /// <summary>
        /// 獲取工作對應的步驟
        /// </summary>
        string GetJobComponentStep { get; }
    }
}
