using DMC.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.UI.Equment
{
    public partial class TingjiFenxi : System.Web.UI.Page
    {
        public DataTable RepairmanList = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            RepairmanService rs = new RepairmanService();
            RepairmanList = rs.GetRepairmWorking();
        }
        public string getDefaultTitle(int i)
        {
            string title = DateTime.Now.AddDays(i).ToString("yyyy-MM-dd");
            return title;
        }
    }
}