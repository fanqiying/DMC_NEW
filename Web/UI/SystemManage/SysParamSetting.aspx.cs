using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UI_SystemManage_SysParamSetting : PageBase
{
    public override string ProgramId
    {
        get { return "sbmi022"; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Header.DataBind();
    }
}