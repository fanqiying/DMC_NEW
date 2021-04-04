namespace DMC.DAL.Script
{
    public interface IFtpTask
    {
        /// <summary>
        /// 添加
        /// <summary>
        string Addt_sFtpTask { get; }
        /// <summary>
        /// 修改
        /// <summary>
        string Modt_sFtpTask { get; }
        /// <summary>
        /// ?除
        /// <summary>
        string Delt_sFtpTask { get; }
        string DelTaskIDList { get; }
        /// <summary>
        /// ??是否存在
        /// <summary>
        string IsExitt_sFtpTask { get; }
        /// <summary>
        /// 根据查?返回?据集
        /// <summary>
        string GetListt_sFtpTask { get; }
        /// <summary>
        /// 取得所有?料列表
        /// <summary>
        string GetAllt_sFtpTask { get; }
        /// <summary>
        /// 獲取工作對應的FTP步驟
        /// </summary>
        string GetFtpSteps { get; }
    }
}
