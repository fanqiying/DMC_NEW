using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMC.DAL.Script;

namespace DMC.DAL.Script.SqlServer
{
    public class MenuSqlScript:IMenus
    {

        /// <summary>
        /// 增加新菜单资料
        /// </summary>
        private string _addMenu = "insert into t_Menu (menuID,fatherID,menuName,usy,"+
            "createrID,cDeptID,createTime,orderid)values(@menuID,@fatherID,@menuName,@usy,@createrID,"+
            "@cDeptID,@createTime,@orderid)";
        /// <summary>
        /// 删除菜单资料
        /// </summary>
        private string _delMenu = "delete from t_Menu where menuID =@menuID";
        /// <summary>
        /// 更新菜单资料
        /// </summary>
        private string _modMenu = "update t_Menu SET fatherID=@fatherID, menuName=@menuName," +
            "usy=@usy,updaterID=@updaterID,uDeptID=@uDept,lastModTime=@modTime,orderid=@orderid where menuID=" +
            "@menuID";
        /// <summary>
        /// 判断菜单资料是否存在
        /// </summary>
        private string _isExitMenu = "select COUNT (*) from t_Menu where menuID=@menuID";
     
        private string _getAllMenuToDT = "select * from t_Menu";


		/// <summary>
		/// 取最大的菜单编号并返回
		/// </summary>
		private string _getMaxMenuID = "select  menuID from t_Menu where autoID =(select MAX (autoID) from t_Menu where fatherID ='' )";
		/// <summary>
		/// 取得某一菜单下的所有子菜单的最大值
		/// </summary>
		private string _getNodeMaxMenuID = "select  menuID from t_Menu where autoID =(select MAX (autoID) from t_Menu  where fatherID =@fatherID )";
        string IMenus.AddMenu
        {
            get { return _addMenu; }
        }

        string IMenus.DelMenu
        {
            get { return _delMenu; }
        }

        string IMenus.ModMenu
        {
            get { return _modMenu; }
        }

        string IMenus.IsExitMenu
        {
            get { return _isExitMenu; }
        } 
        string IMenus.GetAllMenuToDT
        {
            get { return _getAllMenuToDT; }
        }


		string GetMaxMenuID {
			get { return _getMaxMenuID; }
		}


		string IMenus.GetMaxMenuID {
			get { return _getMaxMenuID; }
		}


		string IMenus.GetNodeMaxMenuID {
			get { return _getNodeMaxMenuID; }
		}
	}
}
