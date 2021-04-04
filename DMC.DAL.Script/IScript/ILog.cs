
namespace DMC.DAL.Script
{
    public  interface ILog
    {
        /// <summary>
        /// 增加系统日志
        /// </summary>
        string WriteLog { get; }
        /// <summary>

        /// <summary>
        /// 删除日志资料
        /// </summary>
        string DelSysLog { get; }
         
    }
}
