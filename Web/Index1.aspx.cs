using DMC.BLL;
using DMC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class Index1 : PageBase
    {
        protected string userID = string.Empty; 
        public UserInfo u = new UserInfo();
        UserManageService uc = new UserManageService();
        private PermissionServices permission = new PermissionServices();

        protected override void InitializeCulture()
        {
            base.InitializeCulture();          
            try
            {
                userID = uc.GetUserMain().userID.ToString();
                u = uc.GetUserMain();
                if (Request.Params["cb"] != null)
                {
                    u.Company.companyID = Request.Params["cb"].ToString();
                    System.Web.HttpContext.Current.Session["UserMain"] = u;
                }
            }
            catch
            { }
            try
            {
                if (Request.Params["UB"] != null)
                {
                    string userId = Request.Params["UB"].ToString();
                    if (!string.IsNullOrEmpty(userId))
                    {
                        UserInfo cu = uc.GetUserInfoByID(userId);
                        cu.LanguageId = u.LanguageId;
                        cu.Company = u.Company;
                        System.Web.HttpContext.Current.Session["UserMain"] = cu;
                        u = cu;
                    }
                }
                if (Request.Params["p"] != null && !string.IsNullOrEmpty(Request.Params["p"]))
                {
                    string url = DMC.BLL.common.Base64.DecodingForString(Request.Params["p"]);
                    Response.Redirect(url);
                }
            }
            catch
            {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.OnInit(e);
        }

        public string CompanyName()
        {
            DataTable dt = new DataTable();
            permission.GetMyCompany(u.userID, u.LanguageId, ref dt);
            DataRow[] drList = dt.Select("ISNULL(CompId,'')='" + u.Company.companyID + "'");
            if (drList != null && drList.Length > 0)
                return drList[0]["simpleName"].ToString();
            return "";
        }

        public string InitRight()
        {
            StringBuilder treeNode = new StringBuilder();
            DataTable dt = new DataTable();
            permission.GetMyMenu(u.userID, u.LanguageId, u.Company.companyID, ref dt);
            treeNode.Append(" <div class='easyui-accordion' fit='true' border='false' closed='true' id='menu_page'> ");
            DataRow[] drList = dt.Select("ISNULL(ParentId,'')=''", "orderid asc");
            foreach (DataRow dr in drList)
            {
                //生成一級菜單
                treeNode.Append(" <div title=\"" + dr["DisplayText"].ToString() + "\" data-options=\"selected:false\" > ");
                if (checkLevel(dr["ID"].ToString(), dt))
                {
                    treeNode.Append(InitLevel1(dr["ID"].ToString(), dt));
                }
                else
                {
                    treeNode.Append(InitLevels(dr["ID"].ToString(), dt));
                }
                treeNode.Append(" </div> ");
            }
            treeNode.Append(" </div> ");
            return treeNode.ToString();
        }

        public bool checkLevel(string ParentId, DataTable dt)
        {
            DataRow[] drList = dt.Select("ISNULL(ParentId,'')='" + ParentId + "'", "orderid asc");
            foreach (DataRow dr in drList)
            {
                DataRow[] list = dt.Select("ISNULL(ParentId,'')='" + dr["ID"].ToString() + "'");
                if (list != null && list.Length > 0)
                {
                    return false;
                }
            }
            return true;
        }

        public string InitLevel1(string ParentId, DataTable dt)
        {
            StringBuilder sbList = new StringBuilder();
            DataRow[] drList = dt.Select("ISNULL(ParentId,'')='" + ParentId + "'", "orderid asc");
            foreach (DataRow dr in drList)
            {
                sbList.Append("<p style='padding-top: 5px;padding-left:3px;'  onmouseover=\"this.style.background='#d0e5f6'\" onmouseout=\"this.style.background='#ffffff'\"><img src='" + dr["PathImg"].ToString() + "'  style=' width:16px; height:16px' />");
                sbList.Append("<a href='javascript:void(0);' src='" + dr["Url"].ToString() + "' class='cs-navi-tab' programid='" + dr["ID"].ToString() + "'>" + dr["DisplayText"].ToString() + "</a>");
                sbList.Append("</p>");
            }
            return sbList.ToString();
        }

        public string InitLevels(string ParentId, DataTable dt)
        {
            StringBuilder sbList = new StringBuilder();
            DataRow[] drList = dt.Select("ISNULL(ParentId,'')='" + ParentId + "'", "orderid asc");
            sbList.Append("<ul class='easyui-tree'>");
            foreach (DataRow dr in drList)
            {
                if (dr["IsProgram"].ToString() == "Y")
                {
                    sbList.Append("<li><span><a href='#' src='" + dr["Url"].ToString() + "' class='cs-navi-tab' programid='" + dr["ID"].ToString() + "'>" + dr["DisplayText"].ToString() + "</a></span> </li>");
                }
                else
                {
                    sbList.Append(NodeInfo(dr["ID"].ToString(), dr["DisplayText"].ToString(), dt));
                }
            }
            sbList.Append("</ul>");
            return sbList.ToString();
        }

        public string NodeInfo(string ParentId, string DisplayText, DataTable dt)
        {
            StringBuilder sbList = new StringBuilder();
            sbList.Append("<li><span>" + DisplayText + "</span><ul>");
            DataRow[] drList = dt.Select("ISNULL(ParentId,'')='" + ParentId + "'", "orderid asc");
            foreach (DataRow dr in drList)
            {
                if (dr["IsProgram"].ToString() == "Y")
                {
                    sbList.Append("<li><span><a href='#' src='" + dr["Url"].ToString() + "' class='cs-navi-tab' programid='" + dr["ID"].ToString() + "'>" + dr["DisplayText"].ToString() + "</a></span> </li>");
                }
                else
                {
                    sbList.Append(NodeInfo(dr["ID"].ToString(), dr["DisplayText"].ToString(), dt));
                }
            }
            sbList.Append("</ul></li>");
            return sbList.ToString();
        }
    }
}