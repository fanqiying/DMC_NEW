<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dept.aspx.cs" Inherits="UI_Organize_Dept" %>

<%@ Register Src="../../UIControl/Organize/Dept/DialogAddDiv.ascx" TagName="DialogAddDiv"
    TagPrefix="uc1" %>
<%@ Register Src="../../UIControl/Organize/Dept/DialogDetailDiv.ascx" TagName="DialogDetailDiv"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=7" />
    <link rel="stylesheet" type="text/css" href="../../css/public.css" />
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../easyUI15/jquery.min.js"></script>
    <script type="text/javascript" src="../../easyUI15/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../js/jquery.json-2.4.js"></script>
    <script type="text/javascript" src="../../easyUI15/locale/easyui-lang-zh_TW.js"></script>
    <script type="text/javascript" src="../../js/common.js"></script>
    <script src="../../js/datagrid-detailview.js" type="text/javascript"></script>
    <script src="../../js/dept.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            dialogTransfer("addNewDiv");
            dialogTransfer("divDeptDetail");
            //綁定datagrid
            $('#tbDept').datagrid({
                //是否折叠
                collapsible: true,
                url: '../../ASHX/Organize/DeptManage.ashx?M=Search',
                //自適應寬度
                fitColumns: true,
                //數據在一行顯示 
                nowrap: true,
                //行條紋化
                striped: true,
                //固定序號
                rownumbers: false,
                //是否可以多選
                singleSelect: true,
                checkOnSelect: false,
                selectOnCheck: false,
                //sortName: 'autoID',
                //sortOrder: 'desc',
                remoteSort: false,
                idField: 'autoID',
                loadMsg: '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: true,
                //固定列  
                frozenColumns: [[{ field: 'ck', checkbox: true}]],
                //可動列
                columns: [[{ field: 'deptid', title: '<%=Language("dept_id")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'simplename', title: '<%=Language("dept_sname")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'fullname', title: '<%=Language("dept_fname")%>', width: $(this).width() * 0.2, align: 'left' },
                           { field: 'deptnature', title: '<%=Language("dept_sex")%>', width: $(this).width() * 0.1, align: 'left' , formatter: function (value, row, index) { return ReadText(row.deptnature); }},
                           { field: 'deptgroup', title: '<%=Language("dept_group")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'deptheader', title: '<%=Language("dept_dead")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'usy', title: '<%=Language("Usy")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'Opt', title: '<%=Language("Opt")%>', width: $(this).width() * 0.2, align: 'left',  formatter: function (value, row, index) { return '<a href="#" <%=IsUsy("Delete") %> onclick="<%=IsCanClick("Delete") %>delMuti(\'' + index + '\')"><%=Language("Delete")%></a> | <a href="#" <%=IsUsy("View") %> onclick="<%=IsCanClick("View") %>viewDept(\'' + index + '\')"><%=Language("View")%></a> | <a href="#" <%=IsUsy("Edit") %> onclick="<%=IsCanClick("Edit") %>editDept(' + index + ');"><%=Language("Edit")%></a>'; } }
                ]]
            });
            
            //設置分页控件屬性 
            var p = $('#tbDept').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: <%= Language("PageList")%>,
                beforePageText: '<%= Language("BeforePageText")%>',
                afterPageText: '<%= Language("AfterPageText")%>',
                displayMsg: '<%= Language("DisplayMsg")%>'
            });
            $(window).resize(function(){
                $('#tbDept').datagrid('resize');
            });

          

            //加載下拉框的多語言
            $('#dept_nature').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=DeptNature',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight:'150',
                width:150,
                editable:false
            });
            //加載下拉框的多語言
            $('#txtDeptNature').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=DeptNature',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight:'150', 
                width:150,
                editable:false
            });
        });

       
        var url = "";
        var delStatStr='';
        function Search(type) {
            var queryParams = $('#tbDept').datagrid('options').queryParams;
            queryParams.SearchType = type;
            queryParams.KeyWord = $('#txtKeyword').val();
            queryParams.deptid = $('#txtDeptID').val();
            queryParams.deptname = $('#txtDeptName').val();
            queryParams.header = $('#txtDeptHeader').val();
            queryParams.natrue = $('#txtDeptNature').combobox('getValue');
            queryParams.group = $('#txtDeptGroup').val();
            queryParams.usy = $('#txtDeptUsy').val();
            $('#tbDept').datagrid('load');
        }
    </script>
</head>
<body>
    <div class="Search" id="divOperation">
        <div class="l leftSearch">
            <span>
                <input class="easyui-textbox" type="text" id="txtKeyword" style="width: 200px;" data-options="prompt:'<%=Language("InputDefaultKey")%>'" /></span><span><a
                    href="javascript:void(0);" <%=IsUsy("Search") %> onclick="<%=IsCanClick("Search") %>Search('ByKey')"><%=Language("Search")%></a></span><span><a
                        href="javascript:void(0);" <%=IsUsy("AdvancedSearch") %> onclick="<%=IsCanClick("AdvancedSearch") %>openSearch('divSearch');"><%=Language("AdvancedSearch")%></a>
                    </span>
        </div>
        <div class="r rightSearch">
            <img src="../../images/add.gif" />
            <a href="javascript:void(0)" <%=IsUsy("Add") %> onclick="<%=IsCanClick("Add") %>addDept()">
                <%=Language("Add")%></a> &nbsp;
            <img src="../../images/del.gif" />
            <a href="javascript:void(0)" <%=IsUsy("BatchDelete") %> onclick="<%=IsCanClick("BatchDelete") %>delMuti()">
                <%=Language("BatchDelete")%></a> &nbsp;&nbsp;
        </div>
    </div>
    <div style="clear: both">
    </div>
    <div class="squery" id="divSearch">
        <div class="sinquery">
            <table cellpadding="0" cellspacing="0" class="addCoyTB" id="tbSearch">
                <tr>
                    <td>
                        <%=Language("dept_id")%>：
                    </td>
                    <td>
                        <input type="text" class="easyui-textbox" id="txtDeptID" name="txtDeptID" style="width: 150px;" />
                    </td>
                    <td>
                        <%=Language("dept_sname")%>：
                    </td>
                    <td>
                        <input type="text" class="easyui-textbox" id="txtDeptName" name="txtDeptName" style="width: 150px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Language("dept_dead")%>：
                    </td>
                    <td>
                        <input type="text" class="easyui-textbox" id="txtDeptHeader" name="txtDeptHeader" style="width: 150px;" />
                    </td>
                    <td>
                        <%=Language("dept_sex")%>：
                    </td>
                    <td>
                        <input id="txtDeptNature" name="txtDeptNature" style="width: 150px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Language("dept_group")%>：
                    </td>
                    <td>
                        <input type="text" class="easyui-textbox" id="txtDeptGroup" name="txtDeptGroup" style="width: 150px;" />
                    </td>
                    <td>
                        <%=Language("Usy")%>：
                    </td>
                    <td>
                        <select class="easyui-combobox" style="width: 150px;" id="txtDeptUsy" name="txtDeptUsy"
                            runat="server">
                            <option value="" selected="selected">ALL</option>
                            <option value="Y">Y</option>
                            <option value="N">N</option>
                        </select>
                    </td>
                </tr>
            </table>
        </div>
        <div style="clear: both">
        </div>
        <div style="text-align: right; padding: 5px 0; height: 21px; font-size: 12px;">
            <a href="javascript:void(0)" onclick="Search('ByAdvanced')">
                <%=Language("Search")%></a>&nbsp;&nbsp;&nbsp;&nbsp;
        </div>
    </div>
    <div style="clear: both">
    </div>
    <table id="tbDept" width="99%" fit="false">
    </table>
    <div id="dlg-buttons">
        <a class="easyui-linkbutton" data-options="text:'<%=Language("Save")%>'" href="javascript:void(0)" onclick="saveDept()">
            <%=Language("Save")%></a> <a class="easyui-linkbutton" data-options="text:'<%=Language("Cancel")%>'" href="javascript:void(0)"
                onclick="closeAddWindow('addNewDiv')">
                <%=Language("Cancel")%></a>
    </div>
    <div style="clear: both">
    </div>
    <uc1:DialogAddDiv ID="DialogAddDiv1" runat="server" />
    <uc2:DialogDetailDiv ID="DialogDetailDiv2" runat="server" />
</body>
</html>
