namespace DMC.DAL.Script
{
    public interface IDTSX
    {
        /// <summary>
        /// 添加
        /// <summary>
        string Addt_sDTSX { get; }
        /// <summary>
        /// 修改
        /// <summary>
        string Modt_sDTSX { get; }
        /// <summary>
        /// ?除
        /// <summary>
        string Delt_sDTSX { get; }
        string DelTaskIDList { get; }
        /// <summary>
        /// ??是否存在
        /// <summary>
        string IsExitt_sDTSX { get; }
        /// <summary>
        /// 根据查?返回?据集
        /// <summary>
        string GetListt_sDTSX { get; }
        /// <summary>
        /// 取得所有?料列表
        /// <summary>
        string GetAllt_sDTSX { get; }
        /// <summary>
        /// 獲取DTSX對應的步驟
        /// </summary>
        string GetDtsxSteps { get; }
    }
}
