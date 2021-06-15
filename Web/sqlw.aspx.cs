using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMC.Model;

namespace Web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(textSQL.Text);
            
             
            string result = Convert.ToString(DBFactory.Helper.ExecuteScalar(sbSql.ToString(),null));
            
            Response.Write("<script>alert('ok');</script>");

        }
    }
}