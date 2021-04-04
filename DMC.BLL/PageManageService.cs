using System.Data;
using DMC.DAL;

namespace DMC.BLL
{
    /**********************************************************************
     * Developer      : klint_kong
     * Create Time    : 2016/1/21 16:54:37
     * Description    : 
     * 
     * Update History :
     * No. | Developer   |   Update Time   |     Description
     * 
     * 
     * ********************************************************************/
    public class PageManageService
    {
        PageManage pm = new PageManage();
        public DataTable PageView(string TableName, string KeyField, int PageCurrent, int PageSize, string ShowField, string OrderField, string Condition, out int Total, out int PageCount, string GroupField = "")
        {
            return pm.PageView(TableName, KeyField, PageCurrent, PageSize, ShowField, OrderField, Condition, out Total, out PageCount, GroupField);
        }
    }
}
