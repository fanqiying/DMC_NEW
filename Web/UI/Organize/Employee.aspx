<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Employee.aspx.cs" Inherits="UI_Organize_Employee" %>

<%@ Register Src="../../UIControl/Organize/Employee/dialogaddNewDiv.ascx" TagName="dialogaddNewDiv" TagPrefix="uc1" %>
<%@ Register Src="../../UIControl/Organize/Employee/dialogdivDeptDetail.ascx" TagName="dialogdivDeptDetail" TagPrefix="uc2" %>

<!DOCTYPE >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    <script type="text/javascript" src="../../js/autotips_user.js"></script>
    <script type="text/javascript" src="../../js/autotips_dept.js"></script>
    <script src="../../js/keyUp.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            dialogTransfer("addNewDiv");
            dialogTransfer("divDeptDetail");
            //綁定datagrid
            $('#tbEmp').datagrid({
                //是否折叠
                collapsible: true,
                url: '../../ASHX/Organize/EmpManage.ashx?M=Search',
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
                columns: [[{ field: 'empid', title: '<%=Language("emp_id")%>', width: $(this).width() * 0.1, align: 'left' },
                       { field: 'empname', title: '<%=Language("emp_name")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'empdept', title: '<%=Language("emp_dept")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'exttelno', title: '<%=Language("emp_extTel")%>', width: $(this).width() * 0.2, align: 'left' },
                           { field: 'empmail', title: '<%=Language("emp_mail")%>', width: $(this).width() * 0.2, align: 'left' },
                           { field: 'signerid', title: '<%=Language("emp_sigerID")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'usy', title: '<%=Language("Usy")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'Opt', title: '<%=Language("Opt")%>', width: $(this).width() * 0.2, align: 'left',  formatter: function (value, row, index) { return '<a href="#" <%=IsUsy("Delete") %> onclick="<%=IsCanClick("Delete") %>delMuti(\'' + index + '\')"><%=Language("Delete")%></a> | <a href="#" <%=IsUsy("View") %> onclick="<%=IsCanClick("View") %>viewDept(\'' + index + '\')"><%=Language("View")%></a> | <a href="#" <%=IsUsy("Edit") %> onclick="<%=IsCanClick("Edit") %>editEmp(' + index + ');"><%=Language("Edit")%></a>'; } }
                ]]
            });
            //設置分页控件屬性 
            var p = $('#tbEmp').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: <%=Language("PageList")%>,
                beforePageText: '<%= Language("BeforePageText")%>',
                afterPageText: '<%= Language("AfterPageText")%>',
                displayMsg: '<%= Language("DisplayMsg")%>'
            });
            $(window).resize(function(){
                $('#tbEmp').datagrid('resize');
            });

        });
        var url = "";
        var delStatStr = '';
        function delMuti(index) {
            var ids = [];

            if (index) {
                var row = $('#tbEmp').datagrid('getData').rows[index];
                ids.push(row.empid);

            }
            else {
                var rows = $('#tbEmp').datagrid('getChecked');
                for (var i = 0; i < rows.length; i++) {
                    //每行ID放入數組中
                    ids.push(rows[i].empid);

                }

            }
            if (ids.length > 0) {
                //必須為string類型，否則傳輸不過去 
                var idlist = ids.toString();

                $.messager.defaults = { ok: '<%=Language("yes")%>', cancel: '<%=Language("mesageNo")%>'};//yes 是 mesageNo 否 

                checkDelemp(idlist);


            }
            else {
                $.messager.alert('<%=Language("msgDel")%>', '<%=Language("DelTips")%>');//msgDel 刪除數據 DelTips 請選擇需要刪除的數據
            }
        }
        function checkDelemp(idlist) {
            $.post('../../ASHX/Organize/DelCheck.ashx', { empID: idlist },
                                 function (result) {
                                     try {
                                         if (result == "True") {
                                             delStatStr = '<%=Language("exitsemployact")%>';//exitsemployact 員工中包含帳號數據，確定要刪除嗎？
                                             delEmpByIDstr(idlist);
                                         } else {
                                             delStatStr = '<%=Language("comformDelMsg")%>';
                                             delEmpByIDstr(idlist);
                                         }
                                     }
                                     catch (e) {
                                         $.messager.alert({
                                             title: '<%=Language("ExceptionTips")%>', //ExceptionTips 異常提示
                                             msg: '<%=Language("SubmitDataException")%>'//SubmitDataException 提交數據異常，請確認網絡連接狀況
                                         });

                                     }
                                 }, 'text');
                             }

                             function delEmpByIDstr(idlist) {
                                 $.messager.confirm('<%=Language("DeleteConfirm")%>', delStatStr, function (data) {
                                     if (data) {
                                         $.post('../../ASHX/Organize/EmpManage.ashx?M=delete', { empIDstr: idlist },
                                                     function (result) {
                                                         try {
                                                             if (result.success) {

                                                                 $('#tbEmp').datagrid('reload'); // reload the user data
                                                             } else {
                                                                 $.messager.alert({	// show error message
                                                                     title: 'Error',
                                                                     msg: result.msg
                                                                 });
                                                             }
                                                         }
                                                         catch (e) {
                                                             $.messager.alert({
                                                                 title: '<%=Language("ExceptionTips")%>',//ExceptionTips 異常提示
                                                                   msg: '<%=Language("SubmitDataException")%>'//SubmitDataException 提交數據異常，請確認網絡連接狀況
                                                               });

                                                           }
                                                       }, 'json');
                                                   }
                                 });
                                           }
                                           function GetDeptIDListByKey(op) {

                                               Tips({ input: op, keyword: op.value });
                                           }

                                           function GetUserList(op) {

                                               autoTips({ input: op, keywords: op.value });
                                           }
                                           //截取是否6位。
                                           function RexValue(Control) {
                                               if (Control.value.length > 6) {
                                                   Control.value = Control.value.substring(0, 6);
                                                   alert('<%=Language("extnotexits")%>');//extnotexits 分機不能超過6位
                                             }
                                         }
                                         function Search(type) {
                                             var queryParams = $('#tbEmp').datagrid('options').queryParams;
                                             queryParams.SearchType = type;
                                             queryParams.KeyWord = $('#txtKeyword').val().replace('<%=Language("InputDefaultKey")%>', '');
                                             queryParams.empid = $('#txtEmpID').val();
                                             queryParams.empname = $('#txtEmpName').val();
                                             queryParams.empdept = $('#txtEmpDeptID').val();
                                             queryParams.emptitle = $('#txtTitle').val();
                                             queryParams.exttelno = $('#txtTel').val();
                                             queryParams.empmail = $('#txtEmpMail').val();
                                             queryParams.signerid = $('#txtSignerID').val();
                                             queryParams.usy = $('#txtEmpUsy').val();
                                             $('#tbEmp').datagrid('reload');
                                         }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="Search" id="divOperation">
            <div class="l leftSearch">
                <span>
                    <input type="text" id="txtKeyword" style="width: 200px;" value="<%=Language("InputDefaultKey")%>"
                        onfocus="this.value=''" onblur="if(!value){value=defaultValue;}" /></span><span><a
                            href="javascript:void(0);" <%=IsUsy("Search") %> onclick="<%=IsCanClick("Search") %>Search('ByKey')"><%=Language("Search")%></a></span><span><a
                                href="javascript:void(0);" <%=IsUsy("AdvancedSearch") %> onclick="<%=IsCanClick("AdvancedSearch") %>openSearch('divSearch');"><%=Language("AdvancedSearch")%></a></span>
            </div>
            <div class="r rightSearch">
                <img src="../../images/add.gif" />
                <a href="javascript:void(0)" <%=IsUsy("Add") %> onclick="<%=IsCanClick("Add") %>addEmp()">
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
                <table align="center" cellpadding="0" cellspacing="0" class="addCoyTB">
                    <tr>
                        <td align="right">
                            <%=Language("emp_id")%>：
                        </td>
                        <td>
                            <input class="easyui-textbox" id="txtEmpID" />
                        </td>
                        <td align="right">
                            <%=Language("emp_name")%>：
                        </td>
                        <td>
                            <input class="easyui-textbox" id="txtEmpName" />
                        </td>
                        <td align="right">
                            <%=Language("emp_dept")%>：
                        </td>
                        <td>
                            <input class="easyui-textbox" id="txtEmpDeptID" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%=Language("emp_extTel")%>：
                        </td>
                        <td>
                            <input class="easyui-textbox" id="txtTel" />
                        </td>
                        <td align="right">
                            <%=Language("emp_mail")%>：
                        </td>
                        <td>
                            <input class="easyui-textbox" id="txtEmpMail" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%=Language("emp_sigerID")%>：
                        </td>
                        <td>
                            <input class="easyui-textbox" id="txtSignerID" />
                        </td>
                        <td align="right">
                            <%=Language("Usy")%>：
                        </td>
                        <td>
                            <select class="easyui-combobox" style="width: 120px; font-size: 12px;" id="txtEmpUsy">
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
        <table id="tbEmp" width="99%" fit="false">
        </table>
        <div style="clear: both">
        </div>
    </form>
    <uc1:dialogaddNewDiv ID="dialogaddNewDiv1" runat="server" />
    <uc2:dialogdivDeptDetail ID="dialogdivDeptDetail1" runat="server" />
</body>
</html>
