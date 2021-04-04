
namespace DMC.DAL.Script
{
    public interface IProgramRight
    {
        /// <summary>
        /// 權限類別添加程式權限
        /// </summary>
        string AddProgramRightByRose { get; }
        /// <summary>
        /// 使用者添加程式權限
        /// </summary>
        string AddProgramRightByUser { get; }
        /// <summary>
        /// 刪除權限類別的程式權限
        /// </summary>
        string DeleteProgramRightByRose { get; }
        /// <summary>
        /// 刪除使用者的程式權限
        /// </summary>
        string DeleteProgramRightByUser { get; }
        /// <summary>
        /// 搜索權限類別的程式權限
        /// </summary>
        string SearchProgramRightByRose { get; }
        /// <summary>
        /// 搜索使用者的程式權限
        /// </summary>
        string SearchProgramRightByUser { get; }
        /// <summary>
        /// 检查程式是否存在
        /// </summary>
        string ExistsProgramId { get; }

        string DeleteAllProgramRightByRose { get; }

        string DeleteAllProgramRightByUser { get; }
    }
}
