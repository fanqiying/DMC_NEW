using System;
using System.Collections.Generic;
using DMC.Model;
using System.Data;
using DMC.DAL;
using Utility.HelpClass;
namespace DMC.BLL
{
    /// <summary>
    /// 菜單目錄業務邏輯操作類
    /// code by jeven_xiao
    /// 2013-6-9
    /// </summary>
    public class MenusService
    {
        private MenusDAL mdal = new MenusDAL();
        private PageManage pageView = new PageManage();

        /// <summary>
        /// 新建菜单目录
        /// </summary>
        /// <param name="menu">菜单实体</param>
        /// <returns>bool</returns>
        public string AddMenu(t_Menu menu)
        {
            if (string.IsNullOrEmpty(menu.menuID))
                return "EB0032";
            if (string.IsNullOrEmpty(menu.menuName))
                return "EB0033";
            if (IsExitMenu(menu.menuID))
                return "EB0035";

            if (mdal.AddMenu(menu) == true)
            {
                return "SB0018";
            }
            else
            {
                return "UB0002";
            }

        }
        /// <summary>
        /// 修改菜单目录
        /// </summary>
        /// <param name="menu">實體</param>
        /// <returns>string</returns>

        public string ModMenu(t_Menu menu)
        {
            if (string.IsNullOrEmpty(menu.menuID))
                return "EB0032";
            if (string.IsNullOrEmpty(menu.menuName))
                return "EB0033";
            if (IsExitMenu(menu.menuID))
            {
                if (mdal.ModMenu(menu) == true)
                {
                    return "SB0019";
                }

                else
                {
                    return "UB0003";
                }
            }
            else
            {
                return "EB0035";
            }


        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="idList">菜单目录集合</param>
        /// <returns>string</returns>

        public string DelMenu(List<string> idList)
        {
            if (idList.Count < 1 || idList == null)
                return "EB0036";
            if (mdal.DelMenu(idList) == true)
                return "SB0020";
            else
                return "UB0004";
        }

        /// <summary>
        /// 验证菜单目录是否存在
        /// </summary>
        /// <param name="menuID">菜單編號</param>
        /// <returns>bool</returns>
        public bool IsExitMenu(string menuID)
        {
            if (string.IsNullOrEmpty(menuID))
                return false;
            return mdal.IsExitMenu(menuID);

        }

        /// <summary>
        /// 取得所有菜单列表
        /// </summary>
        /// 按條件查詢所有的日誌記錄
        /// <param name="pageSize">每页條數</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageCount">總页數</param>
        /// <param name="total">總記錄數</param>
        /// <param name="Where">查詢條件</param>
        /// <returns>DataTable</returns>
        public DataTable GetAllMenu(int pageSize, int pageIndex, out int pageCount, out int total, string Where = "")
        {
            return pageView.PageView("t_Menu", "AutoId", pageIndex, pageSize, "*", "AutoId ASC", Where, out total, out pageCount);
        }

        /// <summary>
        /// 根据菜單ID的关键字取得菜單的列表
        /// </summary>
        /// <param name="key">關鍵字</param>
        /// <returns>List</returns>
        public static List<t_Menu> GetMenuListByKey(string key)
        {
            List<t_Menu> listT = new List<t_Menu>();
            List<t_Menu> listInfo = new List<t_Menu>();
            if (!Utility.HelpClass.Cache.Exists("Cache_MenuInfo"))
            {
                listT = ReturnMenuCCache();
            }
            else
            {
                listT = Utility.HelpClass.Cache.GetCache("Cache_MenuInfo") as List<t_Menu>;
            }

            //检索字母

            listInfo = listT.FindAll(r => r.menuID.ToLower().IndexOf(key.ToLower()) > -1 ? true : false);

            return listInfo;
        }
        /// <summary>
        /// 獲取菜單集合，并設置數據緩存
        /// </summary>
        /// <returns>List</returns>
        public static List<t_Menu> ReturnMenuCCache()
        {
            List<t_Menu> listT = new List<t_Menu>();
            if (!Utility.HelpClass.Cache.Exists("Cache_MenuInfo"))
            {
                try
                {


                    DataTable dtt = GetAllMenuToDT();
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        t_Menu tinfo = new t_Menu();
                        tinfo.menuID = dtt.Rows[i]["menuID"].To_String();
                        tinfo.menuName = dtt.Rows[i]["menuName"].To_String();
                        listT.Add(tinfo);
                    }

                    if (listT.Count > 0)
                        Cache.SetCache("Cache_MenuInfo", listT, 60 * 60 * 8);
                }
                catch
                {
                }

            }
            else
            {
                listT = Cache.GetCache("Cache_MenuInfo") as List<t_Menu>;
            }

            return listT;
        }
        /// <summary>
        /// 獲取搜有的菜單數據列表
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetAllMenuToDT()
        {
            return new MenusDAL().GetAllMenuToDT();
        }

        /// <summary>
        /// 取最大菜单编号
        /// </summary>
        /// <returns>最大的菜单编号</returns>
        public string GetMaxMenuID()
        {
            return new MenusDAL().GetMaxMenuID();
        }


        /// <summary>
        /// 取得某一菜单下的所有子菜单的最大值
        /// </summary>
        /// <param name="fatherID">该菜单的父级菜单编号</param>
        /// <returns>string
        /// </returns>
        public string GetNodeMaxMenuID(string fatherID)
        {
            return new MenusDAL().GetNodeMaxMenuID(fatherID);


        }
        /// <summary>
        /// 根据菜单编号动态生成有规则的菜单号
        /// </summary>
        /// <param name="BaseNumber">原来的菜单编号</param>
        /// <returns>系统的菜单编号</returns>
        public string NextNumber(string BaseNumber)
        {
            string NewNumber = "";//新值
            int InNumber = 1;//进位
            int PlaceValue;//位值
            char[] No = BaseNumber.ToCharArray();

            for (int i = BaseNumber.Length - 1; i >= 0; i--)
            {
                if (No[i] == '9' && InNumber == 1)
                {
                    InNumber = 1;
                    NewNumber = "0" + NewNumber;
                }
                else
                    if (InNumber == 1 && No[i] >= '0' && No[i] < '9')
                    {
                        PlaceValue = Int32.Parse(No[i].ToString());
                        PlaceValue = (InNumber + PlaceValue);
                        InNumber = 0;
                        NewNumber = PlaceValue.ToString() + NewNumber;
                    }
                    else
                    {
                        InNumber = 0;
                        NewNumber = No[i] + NewNumber;
                    }
            }
            if (BaseNumber == NewNumber)
                NewNumber = "0000000001";
            return NewNumber;
        }
    }
}
