using System.Web;
using System.Text;
using System.Data;
using System.Web.SessionState;
using DMC.BLL;

namespace Web.ASHX
{
    /// <summary>
    /// 樹節點操作類
    /// code by klint
    /// </summary>
    public class TreeJson : IHttpHandler, IRequiresSessionState
    {
        //變量定義以及相關類的實例化
        private UserManageService userManage = new UserManageService();
        private PermissionServices permission = new PermissionServices();
        private string LangId = "0002";
        private string CompanyId = "avccn";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Method = context.Request.Params["M"];
            switch (Method.ToLower())
            {
                case "mytree":
                    GetMyMenu(context);//獲取我的操作菜單
                    break;
                case "myprogramsetting":
                    GetMyProgramSetting(context);//獲取我的程式設置菜單
                    break;
                case "maintree":
                    GetMainMenu(context);//返回程式設置關聯菜單
                    break;
            }
        }

        /// <summary>
        /// 獲取我的操作菜單
        /// </summary>
        /// <param name="context"></param>
        private void GetMyMenu(HttpContext context)
        {
            string tree = string.Empty;
            string userId = context.Request.Params["UserId"];
            string langId = LangId;// context.Request.Params["LangId"];
            DataTable dt = new DataTable();
            permission.GetMyMenu(userId, langId, CompanyId, ref dt);
            tree = GetMyMenuTreeNode("", dt);
            context.Response.Write(string.IsNullOrEmpty(tree) ? "[]" : tree);
        }

        /// <summary>
        /// 獲取我的程式設置菜單
        /// </summary>
        private void GetMyProgramSetting(HttpContext context)
        {
            string tree = string.Empty;
            string userId = context.Request.Params["UB"];
            string roseId = context.Request.Params["RoseId"];
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(userId))
                userManage.GetSettingMenuTree("1", userId, LangId, ref dt);
            else if (!string.IsNullOrEmpty(roseId))
                userManage.GetSettingMenuTree("2", roseId, LangId, ref dt);
            tree = GetTreeNodeJson("", dt);
            context.Response.Write(string.IsNullOrEmpty(tree) ? "[]" : tree);
        }

        /// <summary>
        /// 返回程式設置關聯菜單
        /// </summary>
        /// <param name="context"></param>
        private void GetMainMenu(HttpContext context)
        {
            string tree = string.Empty;
            DataTable dt = new DataTable();
            userManage.GetMainMenu(LangId, ref dt);
            tree = GetMainMenuJson("", dt);
            context.Response.Write(string.IsNullOrEmpty(tree) ? "[]" : tree);
        }
        /// <summary>
        /// 獲取菜單數據轉成JSON數據
        /// </summary>
        /// <param name="parentId">是否有父級菜單</param>
        /// <param name="dt">菜單數據實體</param>
        /// <returns>JSON字符串</returns>
        protected string GetMyMenuTreeNode(string parentId, DataTable dt)
        {
            StringBuilder content = new StringBuilder();
            DataRow[] drList = dt.Select("ISNULL(ParentId,'')='" + parentId + "'");
            foreach (DataRow dr in drList)
            {
                if (content.Length > 0)
                    content.Append(",");
                content.Append("{");
                content.AppendFormat("\"id\":\"{0}\"", dr["ID"].ToString());
                //只有為目錄時，才有展開和關閉操作
                if (dr["IsProgram"].ToString() == "N")
                    content.AppendFormat(",\"state\":\"{0}\"", "close");

                content.AppendFormat(",\"text\":\"{0}\"", dr["DisplayText"].ToString());
                if (dr["IsProgram"].ToString() == "Y")
                {
                    content.AppendFormat(",\"attributes\":{0}", "{\"url\":\"" + dr["Url"].ToString() + "\"}");
                }
                string childrenStr = GetMyMenuTreeNode(dr["ID"].ToString(), dt);
                if (!string.IsNullOrEmpty(childrenStr))
                {
                    content.AppendFormat(",\"children\":{0}", childrenStr);
                }
                content.Append("}");
            }
            if (content.Length > 0)
                return "[" + content.ToString() + "]";
            return string.Empty;
        }
        /// <summary>
        /// 獲取菜單節點組織成節點樹
        /// </summary>
        /// <param name="parentId">是否是父級菜單</param>
        /// <param name="dt">數據集</param>
        /// <returns>JSON</returns>
        protected string GetTreeNodeJson(string parentId, DataTable dt)
        {
            StringBuilder content = new StringBuilder();
            DataRow[] drList = dt.Select("ISNULL(ParentId,'')='" + parentId + "'");
            foreach (DataRow dr in drList)
            {
                if (content.Length > 0)
                    content.Append(",");
                content.Append("{");
                content.AppendFormat("\"id\":\"{0}\"", dr["ID"].ToString());
                //只有為目錄時，才有展開和關閉操作
                if (dr["IsProgram"].ToString() == "N")
                    content.AppendFormat(",\"state\":\"{0}\"", "close");

                content.AppendFormat(",\"checked\":{0}", dr["IsUse"].ToString() == "Y" ? "true" : "false");
                content.AppendFormat(",\"text\":\"{0}\"", dr["DisplayText"].ToString());
                if (dr["IsProgram"].ToString() == "Y")
                {
                    content.AppendFormat(",\"attributes\":{0}", "{\"url\":\"" + dr["Url"].ToString() + "\"}");
                }
                string childrenStr = GetTreeNodeJson(dr["ID"].ToString(), dt);
                if (!string.IsNullOrEmpty(childrenStr))
                {
                    content.AppendFormat(",\"children\":{0}", childrenStr);
                }
                content.Append("}");
            }
            if (content.Length > 0)
                return "[" + content.ToString() + "]";
            return string.Empty;
        }
        /// <summary>
        /// 返回程式設置關聯菜單組織成JSON字符串，生成樹形結構
        /// </summary>
        /// <param name="parentId">是否是父級菜單</param>
        /// <param name="dt">數據集</param>
        /// <returns>JSON</returns>
        private string GetMainMenuJson(string parentId, DataTable dt)
        {
            StringBuilder content = new StringBuilder();
            DataRow[] drList = dt.Select("ISNULL(fatherID,'')='" + parentId + "'");
            foreach (DataRow dr in drList)
            {
                if (content.Length > 0)
                    content.Append(",");
                content.Append("{");
                content.AppendFormat("\"id\":\"{0}\"", dr["menuID"].ToString());
                content.AppendFormat(",\"state\":\"{0}\"", "close");
                content.AppendFormat(",\"text\":\"{0}\"", dr["DisplayValue"].ToString());
                string childrenStr = GetMainMenuJson(dr["menuID"].ToString(), dt);
                if (!string.IsNullOrEmpty(childrenStr))
                {
                    content.AppendFormat(",\"children\":{0}", childrenStr);
                }
                content.Append("}");
            }
            if (content.Length > 0)
                return "[" + content.ToString() + "]";
            return string.Empty;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}