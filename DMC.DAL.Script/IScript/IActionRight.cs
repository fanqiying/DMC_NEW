
namespace DMC.DAL.Script
{
    public interface IActionRight
    {
        /// <summary>
        /// 添加使用者程式的所有操作
        /// </summary>
        string AddAllProgramActionByUser { get; }
        /// <summary>
        /// 添加權限類別程式的所有操作
        /// </summary>
        string AddAllProgramActionByRose { get; }
        /// <summary>
        /// 添加使用者程式的操作權限
        /// </summary>
        string AddProgramActionByUser { get; }
        /// <summary>
        /// 添加權限類別程式的操作權限
        /// </summary>
        string AddProgramActionByRose { get; }
        /// <summary>
        /// 刪除使用者的程式操作權限
        /// </summary>
        string DeleteProgramActionByUser { get; }
        /// <summary>
        /// 刪除權限類別的程式操作權限
        /// </summary>
        string DeleteProgramActionByRose { get; }
        /// <summary>
        /// 獲取使用者程式的操作權限
        /// </summary>
        string ReadProgramActionByUser { get; }
        /// <summary>
        /// 獲取權限類別程式的操作權限
        /// </summary>
        string ReadProgramActionByRose { get; }

    }
}
