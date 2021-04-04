using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UI_Program_ProgramSet : PageBase
{
    private string _programId = string.Empty;
    public override string ProgramId
    {
        get { return "sbmi004"; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}