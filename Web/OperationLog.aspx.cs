using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

public partial class OperationLog : PageBase
{

    private string _programId = string.Empty;
    public override string ProgramId
    {
        get { return "sbmi013"; }
    } 

    protected void Page_Load(object sender, EventArgs e)
    {


    }


}
