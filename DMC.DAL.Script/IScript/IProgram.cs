
namespace DMC.DAL.Script
{
    public interface IProgram
    {
        /// <summary>
        /// 增加新的程式
        /// </summary>
        string AddProgram { get; }
        /// <summary>
        /// 删除程式资料
        /// </summary>
        string DelProgram { get; }
        /// <summary>
        /// 修改程式资料
        /// </summary>
        string ModProgram { get; }
        /// <summary>
        /// 判断程式是否存在
        /// </summary>
        string IsExitProg { get; } 
        /// <summary>
        /// 设置程式的基本功能
        /// </summary>
        string SetProgFunc { get; }
        /// <summary>
        /// 设置程式与菜单的关联
        /// </summary>
        string ModProgMenu { get; }

        /// <summary>
        /// 增加新的程式到菜单
        /// </summary>
        string AddProgMenu { get; }
        /// <summary>
        /// 删除程式的基本功能
        /// </summary>
        string DelProgFunc { get; }
        /// <summary>
        /// 是否存在程式与菜单的关联
        /// </summary>
        string IsExitProgVsMenu { get; }

        /// <summary>
        /// 程式的基本功能是否存在
        /// </summary>
        string IsExitProgFunc { get; }

        /// <summary>
        /// 取得格式化的程式列表
        /// </summary>
        string Programdt { get; }

        /// <summary>
        /// 取得程式的基本功能列表
        /// </summary>
        string ProgramFuncList { get; }
        /// <summary>
        /// 獲取程式編號列表
        /// </summary>
        string ProgramIdList { get; }
    }
}
