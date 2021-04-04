using DMC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.UI.PageTemplate
{
    public partial class PageBasic : System.Web.UI.MasterPage
    {
        UserManageService m_UserService = new UserManageService();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected string GetLanguageScript() 
        {
            string strLanguageId = m_UserService.GetUserMain().LanguageId.ToString();
            string js = string.Empty;
            switch (strLanguageId)
            {
                case "0001":
                    js = Page.ResolveClientUrl("../../easyUI15/locale/easyui-lang-zh_TW.js");
                    break;
                case "0002":
                    js = Page.ResolveClientUrl("../../easyUI15/locale/easyui-lang-zh_CN.js");
                    break;
                case "0003":
                    js = Page.ResolveClientUrl("../../easyUI15/locale/easyui-lang-en.js");
                    break;
                default:
                    js = Page.ResolveClientUrl("../../easyUI15/locale/easyui-lang-zh_TW.js");
                    break;
            }
            return string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>",js);
        }
    }
}