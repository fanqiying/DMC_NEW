
namespace DMC.DAL.Script
{
    public interface IDataRight
    {
        /// <summary>
        /// 添加程式的數據權限
        /// </summary>
        string AddProgramDataRight { get; }
        /// <summary>
        /// 獲取程式的數據權限
        /// </summary>
        string ReadProgramDataRight { get; }
        /// <summary>
        /// 添加權限類別的數據權限
        /// </summary>
        string AddRoseDataRight { get; }
        /// <summary>
        /// 修改權限類別的數據權限
        /// </summary>
        string UpdateRoseDataRight { get; }
        /// <summary>
        /// 獲取權限類別的數據權限
        /// </summary>
        string ReadRoseDataRight { get; }
        /// <summary>
        /// 判斷部門是否存在
        /// </summary>
        string ExistsDeptId { get; }
        /// <summary>
        /// 刪除權限類別的數據權限
        /// </summary>
        string DeleteRoseDataRight { get; }
        /// <summary>
        /// 刪除程式的數據權限
        /// </summary>
        string DeleteProgramDataRight { get; }

        string UpdateProgramDataRight { get; }

        string ReadRightDataList { get; }

        string ReadProgramDataList { get; }
    }
}
