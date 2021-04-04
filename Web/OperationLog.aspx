<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OperationLog.aspx.cs" Inherits="OperationLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="../../css/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../easyUI15/jquery.min.js"></script>
    <script type="text/javascript" src="../../easyUI15/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../js/jquery.json-2.4.js"></script>
    <script type="text/javascript" src="../../easyUI15/locale/easyui-lang-zh_TW.js"></script>
    <script type="text/javascript" src="js/common.js"></script>
    <script src="common/keyUp.js" type="text/javascript"></script>
    <!--初始化-->
    <script type="text/javascript">
        $(document).ready(function () {
            dialogTransfer("OpenLogSetting");
            dialogTransfer("divDeptDetail");
            //綁定datagrid
            $('#tbLog').datagrid({
                //是否折叠
                collapsible: true,
                //自適應寬度
                fitColumns: true,
                url: 'ASHX/LogManage.ashx?M=Search',
                //數據在一行顯示 
                nowrap: false,
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
                columns: [[{ field: 'operatorid', title: '<%=Language("Log_userID")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'refremark', title: '<%=Language("Log_remark")%>', width:$(this).width() * 0.3, align: 'left' },
                           { field: 'refprogram', title: '<%=Language("Log_program")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'refclass', title: '<%=Language("Log_class")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'refmethod', title: '<%=Language("Log_method")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'refip', title: '<%=Language("Log_ip")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'refsql', title: '<%=Language("Log_sql")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'Opt', title: '<%=Language("Opt")%>', width: $(this).width() * 0.1, align: 'left',formatter: function (value, row, index) { return '<a href="#" onclick="viewLog(\'' + index + '\')">詳細描述</a>  '; } }
                ]]
            });
            //設置分页控件屬性 
            var p = $('#tbLog').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: <%= Language("PageList")%>,
                beforePageText: '<%= Language("BeforePageText")%>',
                afterPageText: '<%= Language("AfterPageText")%>',
                displayMsg: '<%= Language("DisplayMsg")%>'
            });
            $(window).resize(function(){
                $('#tbLog').datagrid('resize');
            });

            $('#tbOpenLogSetting').datagrid({
                //是否折叠
                collapsible: true,
                //自適應寬度
                fitColumns: true,
                //數據在一行顯示 
                nowrap: false,
                //行條紋化
                striped: true,
                //固定序號
                rownumbers: false,
                //是否可以多選
                singleSelect: true,
                checkOnSelect: false,
                selectOnCheck: false,
                sortName: 'ClassName',
                sortOrder: 'desc',
                remoteSort: false,
                idField: 'ClassName',
                loadMsg: '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: false,
                //固定列  
                frozenColumns: [[{ field: 'ck', checkbox: true}]],
                //可動列
                columns: [[{ field: 'ClassName', title: '<%=Language("classname")%>', width: 200, align: 'left' },
                           { field: 'ClassDesc', title: '<%=Language("typeClassDesc")%>', width:200, align: 'left' }
            ]],
                onLoadSuccess: function (data) {
                    if (data) {
                        $.each(data.rows, function (index, item) {
                            if (item.Usy == 'Y') {
                                $('#tbOpenLogSetting').datagrid('checkRow', index);
                            }
                        });
                    }
                }
            });
        });
    </script>
    <script type="text/javascript">
        function viewLog(index) {
            var row = $('#tbLog').datagrid('getData').rows[index];
            if (row) {
                try {
                    $('#fmView').form('load', row);
                    $('#divDeptDetail').dialog({
                        title: '<%=Language("details")%>'
                    });
                $('#divDeptDetail').dialog('open');
            }
            catch (e) {
                $.messager.alert({
                    title: '<%=Language("ExceptionTips")%>',
                    msg: '<%=Language("IntenetException")%>'
                });
            }
        }
    }

    function delMuti(index) {
        var ids = [];
        if (index) {
            var row = $('#tbLog').datagrid('getData').rows[index];
            ids.push(row.autoid);
        }
        else {
            var rows = $('#tbLog').datagrid('getChecked');
            for (var i = 0; i < rows.length; i++) {
                //每行ID放入數組中
                ids.push(rows[i].autoid);
            }

        }
        if (ids.length > 0) {
            //必須為string類型，否則傳輸不過去 
            var idlist = ids.toString();
            $.messager.defaults = { ok: '<%=Language("yes")%>', cancel: '<%=Language("mesageNo")%>' };   
            $.messager.confirm('<%=Language("DeleteConfirm")%>', '<%=Language("comformDelMsg")%>', function (data) {
                if (data) {
                    $.post('ASHX/LogManage.ashx?M=delete', { IDstr: idlist },
                     function (result) {
                         try {
                             if (result.success) {
                                 $('#tbLog').datagrid('reload'); // reload the user data
                             } else {
                                 $.messager.alert({	// show error message
                                     title: 'Error',
                                     msg: result.msg
                                 });
                             }
                         }
                         catch (e) {
                             $.messager.alert({
                                 title: '<%=Language("ExceptionTips")%>',
                                     msg: '<%=Language("SubmitDataException")%>'
                                 });

                             }
                         }, 'json').error(function() {  
                             $.messager.alert({
                                 title: '<%=Language("ExceptionTips")%>',
                                 msg: '<%=Language("SubmitDataException")%>'
                             }); });
                     }
                });
             }
             else {
                 $.messager.alert('<%=Language("msgDel")%>', '<%=Language("DelTips")%>');
        }
    }
    function Search(type) {
        var queryParams = $('#tbLog').datagrid('options').queryParams;
        queryParams.SearchType = type;
        queryParams.KeyWord = $('#txtKeyword').val();
        queryParams.operatorid = $('#log_operaterID').val();
        queryParams.refprogram = $('#log_programeName').val();
        queryParams.refclass = $('#log_className').val();
        queryParams.refip = $('#log_ip').val();
        queryParams.startTime = $('#log_startTime').datebox('getValue');
        queryParams.endTime = $('#log_endTime').datebox('getValue');
        queryParams.refremark = $('#log_remark').val();
        $('#tbLog').datagrid('reload');
    }

    function OpenLogSetting() {
        $('#OpenLogSetting').dialog('open').dialog("setTitle", '<%=Language("log010")%>');
        }

        function CloseLogSetting() {
            $('#OpenLogSetting').dialog('close');
        }

        function CheckExists(rows, classname) {
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].ClassName == classname) {
                    return true;
                }
            }
            return false;
        }

        function SaveOpenLogSetting() {
            var rows = $('#tbOpenLogSetting').datagrid('getChecked');
            var str = '';
            for (var i = 0; i < rows.length; i++) {
                if (str != '')
                    str += ';';
                str += "Usy:Y,";
                str += "ClassName:" + rows[i].ClassName + ",";
                str += "ClassDesc:" + rows[i].ClassDesc + ",";
                str += "AutoId:" + rows[i].AutoId;
            }
            var rowsall = $('#tbOpenLogSetting').datagrid('getData').rows;
            for (var i = 0; i < rowsall.length; i++) {
                var c = rowsall[i];
                if (!CheckExists(rows, c.ClassName)) {
                    if (str != '')
                        str += ';';
                    str += "Usy:N,";
                    str += "ClassName:" + c.ClassName + ",";
                    str += "ClassDesc:" + c.ClassDesc + ",";
                    str += "AutoId:" + c.AutoId;
                }
            }

            $.post('ASHX/LogManage.ashx?M=saveopensetting', { LogList: str, AssemblyName: "WPMS.Services" }, function (result) {
                try {
                    if (result.success) {
                        $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("datasetsucess")%>');
                        $('#OpenLogSetting').dialog('close');
                    } else {
                        $.messager.alert('<%=Language("errTips")%>', result.msg);
                    }
                }
                catch (e) {
                    $.messager.alert({
                        title: '<%=Language("ExceptionTips")%>',
                        msg: '<%=Language("SubmitDataException")%>'
                    });
                }
            }, 'json').error(function() {  
                $.messager.alert({
                    title: '<%=Language("ExceptionTips")%>',
                    msg: '<%=Language("SubmitDataException")%>'
                }); });
        }

        function ChangeAss() {
            $('#tbOpenLogSetting').datagrid('options').url = "ASHX/LogManage.ashx?M=loadopensetting";
            var queryParams = $('#tbOpenLogSetting').datagrid('options').queryParams;
            queryParams.AssemblyName = $("#selAss").val();
            $('#tbOpenLogSetting').datagrid('load');
        }
    </script>
</head>
<body>
    <div class="Search" id="divOperation">
        <div class="l leftSearch">
            <span>
                <input type="text" id="txtKeyword" style="width: 200px;" value="<%=Language("InputDefaultKey")%>"
                    onfocus="this.value=''" onblur="if(!value){value=defaultValue;}" /></span><span><a
                        href="javascript:void(0);" onclick="Search('ByKey')"><%=Language("Search")%></a></span><span><a
                            href="javascript:openSearch('divSearch');"><%=Language("AdvancedSearch")%></a></span>
        </div>
        <div class="r rightSearch">
            <a href="javascript:void(0);" onclick="OpenLogSetting();"><%=Language("log011")%></a>
            <img src="images/del.gif" alt="" />
            <a href="javascript:void(0);" onclick="delMuti()">
                <%=Language("BatchDelete")%></a> &nbsp;&nbsp;
        </div>
    </div>
    <div style="clear: both">
    </div>
    <div class="squery" id="divSearch">
        <div class="sinquery">
            <table align="center" cellpadding="0" cellspacing="0" class="addCoyTB">
                <tr>
                    <td>
                        <%=Language("Log_userID")%>:
                    </td>
                    <td>
                        <input type="text" name="name" id="log_operaterID" />
                    </td>
                    <td>
                        <%=Language("Log_program")%>:
                    </td>
                    <td>
                        <input type="text" name="programeName" id="log_programeName" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Language("Log_class")%>:
                    </td>
                    <td>
                        <input type="text" name="className" id="log_className" />
                    </td>
                    <td>
                        <%=Language("Log_ip")%>:
                    </td>
                    <td>
                        <input type="text" name="logip" id="log_ip" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Language("Log_startTime")%>:
                    </td>
                    <td>
                        <%--<input type="text" name="startTime" id="log_startTime" />--%>
                        <input id="log_startTime" name="startTime" type="text" class="easyui-datebox" />
                    </td>
                    <td>
                        <%=Language("Log_endTime")%>:
                    </td>
                    <td>
                        <%--  <input type="text" name="endTime" id="log_endTime" />--%>
                        <input id="log_endTime" name="endTime" type="text" class="easyui-datebox" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Language("Log_remark")%>:
                    </td>
                    <td>
                        <input type="text" name="remark" id="log_remark" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="clear: both">
        </div>
        <div style="text-align: right; padding: 5px 0; border: false; height: 21px; font-size: 12px;">
            <a href="javascript:void(0)" onclick="Search('ByAdvanced')">
                <%=Language("Search")%></a>&nbsp;&nbsp;&nbsp;&nbsp;
        </div>
    </div>
    <div style="clear: both">
    </div>
    <table id="tbLog" width="99%" fit="false">
    </table>
    <div style="clear: both">
    </div>
    <div id="divDeptDetail" class="easyui-window" data-options="closed:true,modal:true"
        style="width: 600px; padding: 3px;">
        <form id="fmView" method="get">
            <table cellpadding="0" cellspacing="0" border="0" class="tbView" style="text-align: left; width: 100%;">
                <tr>
                    <td bgcolor="#e8f5ff" style="width: 15%;">
                        <%=Language("Log_userID")%>:
                    </td>
                    <td>
                        <input readonly name="operatorid" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#e8f5ff">
                        <%=Language("Log_remark")%>:
                    </td>
                    <td>
                        <input readonly name="refremark" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#e8f5ff">
                        <%=Language("Log_program")%>:
                    </td>
                    <td>
                        <input readonly name="refprogram" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#e8f5ff">
                        <%=Language("Log_class")%>:
                    </td>
                    <td>
                        <input readonly name="refclass" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#e8f5ff">
                        <%=Language("Log_method")%>:
                    </td>
                    <td>
                        <input readonly name="refmethod" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#e8f5ff">
                        <%=Language("Log_ip")%>:
                    </td>
                    <td>
                        <input readonly name="refip" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#e8f5ff">
                        <%=Language("Log_sql")%>:
                    </td>
                    <td>
                        <input readonly name="refsql" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#e8f5ff">
                        <%=Language("Log_time")%>:
                    </td>
                    <td>
                        <input readonly name="reftime" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#e8f5ff">
                        <%=Language("Log_event")%>:
                    </td>
                    <td>
                        <textarea name="refevent" style="width: 100%"></textarea>
                    </td>
                </tr>
            </table>
        </form>
    </div>
    <div id="OpenLogSetting" class="easyui-window" data-options="closed:true,modal:true"
        style="width: 600px; height: 300px; padding: 3px;" buttons="#data-buttons">
        <select onchange="ChangeAss()" id="selAss">
            <option value="WPMS.Services"><%=Language("buslog")%></option>
            <option value="WPMS.DAL"><%=Language("databaseLog")%></option>
        </select>
        <table id="tbOpenLogSetting" width="99%" style="height: 200px;" fit="false">
        </table>
        <a class="easyui-linkbutton" href="javascript:void(0)" onclick="SaveOpenLogSetting()">
            <%=Language("Save")%></a> <a class="easyui-linkbutton" href="javascript:void(0)"
                onclick="CloseLogSetting()">
                <%=Language("Cancel")%></a>
    </div>
</body>
</html>
