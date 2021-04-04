namespace DMC.DAL.Script
{
    public interface IProgramTask
    {
        /// <summary>
        /// 添加
        /// <summary>
        string Addt_sProgramTask { get; }
        /// <summary>
        /// 修改
        /// <summary>
        string Modt_sProgramTask { get; }
        /// <summary>
        /// ?除
        /// <summary>
        string Delt_sProgramTask { get; }
        string DelTaskIDList { get; }
        /// <summary>
        /// ??是否存在
        /// <summary>
        string IsExitt_sProgramTask { get; }
        /// <summary>
        /// 根据查?返回?据集
        /// <summary>
        string GetListt_sProgramTask { get; }
        /// <summary>
        /// 取得所有?料列表
        /// <summary>
        string GetAllt_sProgramTask { get; }
        /// <summary>
        /// 獲取工作對應的執行程序步驟
        /// </summary>
        string GetProgramSteps { get; }
    }
}
