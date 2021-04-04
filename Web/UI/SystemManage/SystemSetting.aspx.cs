using System;
using System.Data;
using DMC.BLL;

public partial class UI_SystemManage_SystemSetting : PageBase
{
    private string _programId = string.Empty;
    public override string ProgramId
    {
        get { return "sbmi007"; }
    }

    public int rowId = 0;
    protected CompService cs = new CompService();
    protected void Page_Load(object sender, EventArgs e)
    {
        this.rpList.DataSource = GetAllCompanyList();
        this.rpList.DataBind();
    }

    private DataTable CreateTable()
    {
        DataTable dt = GetAllCompanyList();

        for (int i = 0; i <= dt.Rows.Count; i++)
        {
            DataRow dr = dt.NewRow();
            dr["CompanyId"] = dt.Rows[i]["companyID"].ToString();
            dr["CompanyName"] = dt.Rows[i]["simpleName"].ToString();
            dt.Rows.Add(dr);
        }
        return dt;
    }

    public DataTable GetAllCompanyList()
    {
        return cs.GetCompanyList(LanguageId);
    }
    
}