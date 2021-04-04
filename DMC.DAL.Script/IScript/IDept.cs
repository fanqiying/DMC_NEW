
namespace DMC.DAL.Script
{
    public interface IDept
    {
        /// <summary>
        /// 增加新部门资料
        /// </summary>
        string AddDept { get; }
        /// <summary>
        /// 删除部门资料
        /// </summary>
        string DelDept{get;}
        /// <summary>
        /// 更新部门资料
        /// </summary>
        string ModDept{get;}
        /// <summary>
        /// 判断部门资料是否存在
        /// </summary>
        string IsExitDept { get; } 

        /// <summary>
        /// 取得所有部门的DATATABLE信息
        /// </summary>
        string GetAllDeptToDT { get; }
        /// <summary>
        /// 根据部门ID取得部门主管
        /// </summary>
        string GetDeptHeaderByID { get; }
        /// <summary>
        /// 取得部门的性质
        /// </summary>
        string GetDeptNature { get; }
    }
}
