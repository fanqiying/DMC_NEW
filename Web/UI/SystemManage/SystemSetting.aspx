<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemSetting.aspx.cs" Inherits="UI_SystemManage_SystemSetting" %>

<%@ Register Src="../../UIControl/ShutDown/DialogStopSys.ascx" TagName="DialogStopSys"
    TagPrefix="uc1" %>
<%@ Register Src="../../UIControl/ShutDown/divDataClose.ascx" TagName="divDataClose"
    TagPrefix="uc2" %>
<%@ Register Src="../../UIControl/ShutDown/divSysHistory.ascx" TagName="divSysHistory"
    TagPrefix="uc3" %>
<%@ Register Src="../../UIControl/ShutDown/DataColseHistory.ascx" TagName="DataColseHistory"
    TagPrefix="uc4" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link rel="stylesheet" type="text/css" href="../../css/public.css" />
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../easyUI15/jquery.min.js"></script>
    <script type="text/javascript" src="../../easyUI15/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../js/jquery.json-2.4.js"></script>
    <script type="text/javascript" src="../../easyUI15/locale/easyui-lang-zh_TW.js"></script>
    <script type="text/javascript" src="../../js/common.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            dialogTransfer("divStop");
            dialogTransfer("divDataClose");
            dialogTransfer("divSysHistory");
            dialogTransfer("DataColseHistory");
            //綁定datagrid
            $('#tbStopList').datagrid({
                //是否折叠
                collapsible: true,
                url: '../../ASHX/SystemManage/ShutDownList.ashx?M=Load',
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
                //sortName: 'switchid',
                //sortOrder: 'desc',
                remoteSort: false,
                idField: 'switchid',
                loadMsg: '<%=Language("WaitData")%>',
                //可動列
                columns: [[{ field: 'reasons', title: '<%=Language("reasons")%>', width: $(this).width() * 0.2, align: 'left' },
                               { field: 'startime', title: '<%=Language("str_time")%>', width: $(this).width() * 0.1, align: 'left' },
                               { field: 'endtime', title: '<%=Language("end_time")%>', width: $(this).width() * 0.1, align: 'left' },
                               { field: 'operatetype', title: '<%=Language("shuptdown")%>', width: $(this).width() * 0.1, align: 'left' },
                               { field: 'operaterid', title: '<%=Language("operatePeople")%>', width: $(this).width() * 0.2, align: 'left' },
                               { field: 'operatedeptid', title: '<%=Language("operatePeopleDept")%>', width: $(this).width() * 0.1, align: 'left' }
                ]]
            });

            $(window).resize(function () {
                $('#tbStopList').datagrid('resize');
            });


            //綁定datagrid
            $('#tbDataColse').datagrid({
                //是否折叠
                collapsible: true,
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
                //sortName: 'switchid',
                //sortOrder: 'desc',
                remoteSort: false,
                idField: 'switchid',
                loadMsg: '<%=Language("WaitData")%>',
                //可動列
                columns: [[{ field: 'reasons', title: '<%=Language("reasons")%>', width: $(this).width() * 0.2, align: 'left' },
                               { field: 'startime', title: '<%=Language("str_time")%>', width: $(this).width() * 0.1, align: 'left' },
                               { field: 'endtime', title: '<%=Language("end_time")%>', width: $(this).width() * 0.1, align: 'left' },
                               { field: 'operatetype', title: '<%=Language("shuptdown")%>', width: $(this).width() * 0.1, align: 'left' },
                               { field: 'operaterid', title: '<%=Language("operatePeople")%>', width: $(this).width() * 0.2, align: 'left' },
                               { field: 'operatedeptid', title: '<%=Language("operatePeopleDept")%>', width: $(this).width() * 0.1, align: 'left' }
                ]]
            });

            $(window).resize(function () {
                $('#tbDataColse').datagrid('resize');
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server" style="padding: 5px;">
        <div style="padding: 5px; background-color: transparent;">
            <input type="hidden" id="compyid" />


            <table border="0" cellpadding="0" cellspacing="1" bgcolor="#a8c7ce" width="100%"
                id="tbOne">
                <asp:Repeater ID="rpList" runat="server">
                    <HeaderTemplate>
                        <tr style="text-align: center;">
                            <td width="15%" height="20" bgcolor="#E0ECFF">
                                <%=Language("comp_sinpleName")%>
                            </td>
                            <td width="35%" height="20" bgcolor="#E0ECFF">
                                <%=Language("shuptdown")%>
                            </td>
                            <td width="35%" height="20" bgcolor="#E0ECFF">
                                <%=Language("stopData")%>
                            </td>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="content<%#Eval("displayid") %>">
                            <td bgcolor="#FFFFFF">
                                <%#Eval("displayText")%>
                            </td>
                            <% if (rowId == 0)
                               {
                                   rowId += 1;%>
                            <td id="tdChkCloseSystem" bgcolor="#FFFFFF" align="center">
                                <a class="easyui-linkbutton" href="javascript:void(0)" onclick="$('#divStop').dialog('open').dialog('setTitle','<%= Language("shuptdown")%>');">
                                    <%=Language("shuptdown")%></a>&nbsp; &nbsp;<a class="easyui-linkbutton" href="javascript:void(0)"
                                        onclick="openAddWindow('divSysHistory');">
                                        <%= Language("hisInfo")%></a>
                            </td>
                            <% } %>
                            <td bgcolor="#FFFFFF" align="center">
                                <a class="easyui-linkbutton" href="javascript:void(0)" onclick="$('#divDataClose').dialog('open').dialog('setTitle','<%= Language("stopData")%>');passCompanyID('<%#Eval("displayid") %>')">
                                    <%= Language("stopData")%></a> <a class="easyui-linkbutton" href="javascript:void(0)"
                                        onclick="openAddWindow('DataColseHistory');passCompanyID('<%#Eval("displayid") %>');test();">
                                        <%= Language("hisInfo")%></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <script type="text/javascript">
                function onload() {
                    var tb = document.getElementById("tbOne");
                    var tr = document.getElementById("tdChkCloseSystem");
                    if (tr != null) {
                        tr.rowSpan = tb.rows.length;
                    }
                }
                onload();
            </script>
        </div>
        <uc1:DialogStopSys ID="DialogStopSys1" runat="server" />
        <uc2:divDataClose ID="divDataClose1" runat="server" />
        <uc3:divSysHistory ID="divSysHistory1" runat="server" />
        <uc4:DataColseHistory ID="DataColseHistory1" runat="server" />
    </form>
</body>
</html>
