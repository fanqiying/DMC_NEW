using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UI_SystemManage_LanguageSetting :  PageBase
{
    private string _programId = string.Empty;
    public override string ProgramId
    {
        get { return "sbmi006"; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}