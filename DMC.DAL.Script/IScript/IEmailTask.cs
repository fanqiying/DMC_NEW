namespace DMC.DAL.Script
{
    public interface IEmailTask
    {
        /// <summary>
        /// 添加
        /// <summary>
        string Addt_sEmailTask { get; }
        /// <summary>
        /// 修改
        /// <summary>
        string Modt_sEmailTask { get; }
        /// <summary>
        /// ?除
        /// <summary>
        string Delt_sEmailTask { get; }
        string DelTaskIDList { get; }
        /// <summary>
        /// ??是否存在
        /// <summary>
        string IsExitt_sEmailTask { get; }
        /// <summary>
        /// 根据查?返回?据集
        /// <summary>
        string GetListt_sEmailTask { get; }
        /// <summary>
        /// 取得所有?料列表
        /// <summary>
        string GetAllt_sEmailTask { get; }

        /// <summary>
        /// 獲取工作對應的郵件步驟
        /// </summary>
        string GetEmailSteps { get; }
    }
}
