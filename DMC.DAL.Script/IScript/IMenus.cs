
namespace DMC.DAL.Script
{
    public interface IMenus
    {
        /// <summary>
        /// 增加新菜单资料
        /// </summary>
        string AddMenu { get; }
        /// <summary>
        /// 删除菜单资料
        /// </summary>
        string DelMenu { get; }
        /// <summary>
        /// 更新菜单资料
        /// </summary>
        string ModMenu { get; }
        /// <summary>
        /// 判断菜单资料是否存在
        /// </summary>
        string IsExitMenu { get; } 

        /// <summary>
        /// 取得菜单目录的datatble
        /// </summary>
        string GetAllMenuToDT { get; }

		/// <summary>
		/// 取得当前系统中存在的最大菜单编号
		/// </summary>
		string GetMaxMenuID { get; }

		/// <summary>
		/// 取得某一菜单下的所有子菜单的最大值
		/// </summary>

		string GetNodeMaxMenuID { get; }
    }
}
