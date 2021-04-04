using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Utility.HelpClass;
using DMC.BLL;
using DMC.Model;
using System.Text;
using System.IO.Compression;
/// <summary>
///Page 基類
/// </summary>
public class PageBase : System.Web.UI.Page
{
    #region 變量定義以及義務方法實例化
    public PermissionServices pServices = new PermissionServices();
    public LanguageManageService languageManage = new LanguageManageService();
    public UserManageService uservice = new UserManageService();
    public string UserId = string.Empty;
    public string LanguageId = string.Empty;
    public string CompanyId = string.Empty;
    private List<ProgramActionInfo> _paInfoList = new List<ProgramActionInfo>();
    protected List<ProgramActionInfo> paInfoList { get { return _paInfoList; } }
    public SwitchService serviceManage = new SwitchService();
    /// <summary>
    /// 程序編號
    /// </summary>
    public virtual string ProgramId
    {
        get { return "PageBase"; }
    }
    //國際化資源集合
    protected List<string> ResourceIdList = new List<string>();
    /// <summary>
    /// 页面資源管理
    /// </summary>
    protected List<LanguageKeyValue> _currentPage = new List<LanguageKeyValue>();
    protected List<LanguageKeyValue> CurrentPage
    {
        get
        {
            return _currentPage;
        }
        set { _currentPage = value; }
    }
    #endregion
    public PageBase()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    } 

    /// <summary>
    /// 驗證是否有页面權限    
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        string Page = "login.html";
        //string Page = Utility.HelpClass.GetData.getCookieStr("LoginPage");
        //if (string.IsNullOrEmpty(Page))
        //    Page = "login.html";

        if (string.IsNullOrEmpty(uservice.GetUserMain().userID))
        {
            string url = GetData.GetPageUrl();
            if (url.IndexOf("Login") <= 0)
            {
                Response.Redirect(@"~\" + Page);
            }
        }

        UserId = uservice.GetUserMain().userID.To_String();

        LanguageId = uservice.GetUserMain().LanguageId.To_String();
        CompanyId = uservice.GetUserMain().Company.companyID.To_String();
        base.OnInit(e);
        if (Utility.HelpClass.Cache.Exists(ProgramId))
        {
            _currentPage = (List<LanguageKeyValue>)Utility.HelpClass.Cache.GetCache(ProgramId);
        }
        //檢查程式是否存在權限
        if (!string.IsNullOrEmpty(ProgramId) && ProgramId != "PageBase")
        {
            //1、檢查是否有程式權限
            if (!pServices.IsExistsPgm(UserId, ProgramId, LanguageId, CompanyId))
                Response.Redirect(@"~\" + Page + "?" + DMC.BLL.common.Base64.EncodingForString(HttpContext.Current.Request.Url.OriginalString));
        }
    }

    /// <summary>
    /// 設置页面的按鈕狀態及按鈕的多語言
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoadComplete(EventArgs e)
    {
        base.OnLoadComplete(e);
        //1.設定操作按鈕
        ActionSetting();
        if (!Utility.HelpClass.Cache.Exists(ProgramId))
        {
            //2.設置緩存
            Utility.HelpClass.Cache.SetCache(ProgramId, CurrentPage, 24 * 60 * 60);
        }
    }

    protected virtual void SetPageResource()
    {

    }
    /// <summary>
    /// 獲取國際化資源
    /// </summary>
    protected void LoadPageResource()
    {
        _currentPage = languageManage.GetResourceByKey(LanguageId, this.ResourceIdList);
    }
    /// <summary>
    /// 操作按鈕設定
    /// </summary>
    private void ActionSetting()
    {
        //檢查程式是否存在權限
        if (!string.IsNullOrEmpty(ProgramId))
        {
            //1、設置按鈕的操作權限
            pServices.GetMyPgmOpt(UserId, ProgramId, LanguageId, ref _paInfoList);
            if (serviceManage.IsDataClose(uservice.GetUserMain().Company.companyID))
            {
                foreach (ProgramActionInfo item in _paInfoList)
                {
                    if (item.ActionId != "View" && item.ActionId.IndexOf("Search") < 0)
                    {
                        item.IsUse = "N";
                    }
                }
            }
            //foreach (ProgramActionInfo action in paInfoList)
            //{
            //    Button ctl = this.FindControl(action.ActionId) as Button;
            //    ctl.Enabled = action.IsUse == "Y";
            //    ctl.Text = action.ActionName;
            //}
        }
    }
    /// <summary>
    /// 檢查按鈕是否可以點擊
    /// </summary>
    /// <param name="controlId">按鈕對象</param>
    /// <returns></returns>
    protected string IsCanClick(string controlId)
    {
        if (_paInfoList == null || _paInfoList.Count == 0)
        {
            //1.設定操作按鈕
            ActionSetting();
        }
        ProgramActionInfo action = _paInfoList.Find(o => o.ActionId == controlId);
        if (action == null)
            return "javascript:return;";
        if (action.IsUse == "Y")
        {
            return "";
        }
        else
        {
            return "javascript:return;";
        }
    }
    /// <summary>
    /// 驗證操作按鈕是否可用
    /// </summary>
    /// <param name="controlId">按鈕對象</param>
    /// <returns></returns>
    protected string IsUsy(string controlId)
    {
        if (_paInfoList == null || _paInfoList.Count == 0)
        {
            //1.設定操作按鈕
            ActionSetting();
        }
        ProgramActionInfo action = _paInfoList.Find(o => o.ActionId == controlId);
        if (action == null)
            return " disabled style=\"color:Gray;\"";
        if (action.IsUse == "Y")
            return "";
        else
            return " disabled style=\"color:Gray;\"";
        //return "disabled";
    }
    /// <summary>
    /// 獲取用戶的用戶名稱
    /// </summary>
    /// <param name="userid"></param>
    /// <returns></returns>
    protected string GetName(string userid)
    {
        return uservice.GetUserInfoByID(userid).userName;
    }
    /// <summary>
    /// 綁定多語言消息和Label
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string Language(string key)
    {
        //1.設定匹配條件
        var res = CurrentPage.Find(o => o.Key == key && o.LanguageId == LanguageId);
        if (res == null)
        {
            //2.獲取匹配資源
            List<LanguageKeyValue> resultList = languageManage.GetResourceByKey(LanguageId, new List<string> { key });
            if (resultList != null && resultList.Count > 0)
            {
                CurrentPage.Add(resultList[0]);
                return resultList[0].Value;
            }
            else
            {
                Response.Write(key);
            }
            return key;
        }
        else
            return res.Value;
    }

}

public class myUserControl : UserControl
{
    public LanguageManageService languageManage = new LanguageManageService();
    public UserManageService uservice = new UserManageService();
    public string LanguageId = string.Empty;
    public virtual string ProgramId
    {
        get { return "PageBase"; }
    }
    /// <summary>
    /// 页面資源管理
    /// </summary>
    protected List<LanguageKeyValue> _currentPage = new List<LanguageKeyValue>();
    protected List<LanguageKeyValue> CurrentPage
    {
        get
        {
            return _currentPage;
        }
        set { _currentPage = value; }
    }

    /// <summary>
    /// 驗證是否有页面權限
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        LanguageId = uservice.GetUserMain().LanguageId.To_String();
        base.OnInit(e);
        if (Utility.HelpClass.Cache.Exists(ProgramId))
        {
            _currentPage = (List<LanguageKeyValue>)Utility.HelpClass.Cache.GetCache(ProgramId);
        }
    }

    /// <summary>
    /// 綁定多語言消息和Label
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected string Language(string key)
    {
        var res = CurrentPage.Find(o => o.Key == key && o.LanguageId == LanguageId);
        if (res == null)
        {
            List<LanguageKeyValue> resultList = languageManage.GetResourceByKey(LanguageId, new List<string> { key });
            if (resultList != null && resultList.Count > 0)
            {
                CurrentPage.Add(resultList[0]);
                return resultList[0].Value;
            }
            else
            {
                Response.Write(key);
            }
            return key;
        }
        else
            return res.Value;
    }
}