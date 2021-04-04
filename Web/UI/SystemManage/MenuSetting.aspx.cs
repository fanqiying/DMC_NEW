using System;
using DMC.BLL;
public partial class SystemManage_MenuSetting : PageBase
{
    private string _programId = string.Empty;
    protected string newMenuNo = string.Empty;
    protected string newfatherID = string.Empty;

    private MenusService mss = new MenusService();

    public override string ProgramId
    {
        get { return "sbmi005"; }
    }
    MenusService mserv = new MenusService();
    protected void Page_Load(object sender, EventArgs e)
    {
        string nowMenuID = mserv.GetMaxMenuID();
        newMenuNo = mss.NextNumber(nowMenuID);


    }


}