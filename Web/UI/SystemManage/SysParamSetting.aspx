<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysParamSetting.aspx.cs"
    Inherits="UI_SystemManage_SysParamSetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系統參數設置</title>
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
    <script src="../../js/datagrid-detailview.js" type="text/javascript"></script>
    <script src="../../js/keyUp.js" type="text/javascript"></script>
    <script type="text/javascript">


         var res =
       {
           FailureTips: '<%=Language("FailureTips")%>',
    IntenetException: '<%=Language("IntenetException")%>',
    ExceptionTips: '<%=Language("ExceptionTips")%>',
    SubmitDataException: '<%=Language("SubmitDataException")%>'

        };


        var url = "../../ASHX/SystemManage/SysParaSetting.ashx?M=add";
        //初始化綁定
        $(document).ready(function () {
            //綁定datagrid
            $('#tbSysParamSetting').datagrid({
                //是否折叠
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: '../../ASHX/SystemManage/SysParaSetting.ashx?M=search',
                //數據在一行顯示 
                nowrap: true,
                fitColumns: true,
                //行條紋化
                striped: true,
                //固定序號
                rownumbers: true,
                //是否可以多選
                singleSelect: true,
                //sortName: 'autoid',
                //sortOrder: 'desc',
                remoteSort: false,
                idField: 'autoid',
                loadMsg: 'WaitData',
                //是否顯示分页
                pagination: true,
                //可動列
                columns: [[{field: 'companyid', title: '<%#Language("CompanyId")%>', width: 80, align: 'left' }, //公司別
                           {field: 'parakey', title: '<%#Language("parakey") %>', width: 80, align: 'left' }, //參數編號
                           {field: 'paraname', title: '<%#Language("paraname") %>', width: 80, align: 'left' }, //參數名稱
                           {field: 'paracontent', title: '<%#Language("paracontent") %>', width: 150, align: 'left' }, //參數內容 
                           {field: 'paradesc', title: '<%#Language("paradesc") %>', width: 150, align: 'left' }, //參數描述 
                           {field: 'usey', title: '<%#Language("Usy") %>', width: 80, align: 'left' }, //有效否 
                           {field: 'Opt', title:'<%=Language("Opt")%>', width: 80, align: 'left',formatter: function (value, row, index) { return '<a href="#"  onclick="Delete(\'' + index + '\')"><%=Language("Delete")%></a> | <a href="#" onclick="Edit(' + index + ');"><%=Language("Edit")%></a> '; } }
                        ]]
            });
            //設置分页控件屬性 
            var p = $('#tbSysParamSetting').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList:<%= Language("PageList")%>,
                beforePageText:'<%= Language("BeforePageText")%>',
                afterPageText: '<%= Language("AfterPageText")%>',
                displayMsg:'<%= Language("DisplayMsg")%>'
            });

            //設置窗體自適應
            $(window).resize(function () {
                $('#tbSysParamSetting').datagrid('resize');
            });

            //加載公司別信息
            $('#aCompanyId').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=scompanytype&ll=<%# LanguageId %>',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight: '150',
                panelWidth:'150',
                editable: false,
                multiple: false
            }); 
            //加載有效否
            $('#aUsey').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=selIsTurn',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight: '150',
                panelWidth:'150',
                editable: false,
                multiple: false
            }); 
        }); 
       
        //新增
        function Add() {
            url = "../../ASHX/SystemManage/SysParaSetting.ashx?M=add";
            $('#fm').form('clear');
            $('#aCompanyId').combobox("enable");
            $("#aCompanyId").combobox("setValue", "All");
            $("#aUsey").combobox("setValue", "Y");
            $('#aParaKey').removeAttr("readonly");
            $('#AddSysParam').dialog({
                title: '<%=Language("Add")%>'//新增HR信息
            });
            $('#AddSysParam').dialog('open');
        }
        //修改
        function Edit(index) {
            url = "../../ASHX/SystemManage/SysParaSetting.ashx?M=mod";
            var row = $('#tbSysParamSetting').datagrid('getData').rows[index];
            if (row) {
                $('#fm').form('clear');
                $('#fm').form('load', row);
                $('#aCompanyId').combobox("disable");
                $('#aParaKey').attr("readonly", "readonly");
                $('#AddSysParam').dialog({
                    title: '<%=Language("mod")%>'//新增HR信息
                });
                $('#AddSysParam').dialog('open');
            }
        }
                 
        //保存
        function Save() {
            $.ajax({
                type: "POST",
                url: url,
                data: {
                    CompanyID: $('#aCompanyId').combobox("getValue"),
                    ParaContent: $('#aParaContent').val(),
                    ParaDesc: $('#aParaDesc').val(),
                    ParaKey: $('#aParaKey').val(),
                    ParaName: $('#aParaName').val(),
                    Usey: $('#aUsey').combobox("getValue")
                },
                success: function (data) {
                    try {
                        if (data.success) {
                            $('#AddSysParam').dialog('close');
                            $('#tbSysParamSetting').datagrid('reload'); // reload the user data
                            $.messager.alert({	// show error message
                                title: '<%# Language("SuccessTips")%>', //成功提示
                                msg: data.msg
                            });
                        } else {
                            $.messager.alert({	// show error message
                                title: '<%# Language("FailureTips")%>', //失敗提示
                                msg: data.msg
                            });
                        }
                    }
                    catch (e) {
                        $.messager.alert({ title: '<%# Language("ExceptionTips")%>', //異常提示
                            msg: '<%# Language("DataException")%>'//數據異常，請確認網絡連接狀況
                        });
                    }
                },
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    parent.ajaxLoadEnd();
                    $.messager.alert({
                        title: res.FailureTips,
                        msg: res.IntenetException
                    });
                }
            });
        } 
         //刪除
        function Delete(index) {
            var row = $('#tbSysParamSetting').datagrid('getData').rows[index];
            if (row) {
                $.messager.defaults = { ok: '<%=Language("yes")%>', cancel: '<%=Language("mesageNo")%>' };
                $.messager.confirm('<%=Language("comformMsg")%>', '<%=Language("comformDelMsg")%>', function (data) {
                    if (data) {
                        $.ajax({
                            type: "POST",
                            url: "../../ASHX/SystemManage/SysParaSetting.ashx?M=del",
                            data: {
                                CompanyID: row.companyid,
                                ParaKey:row.parakey
                            },
                            success: function (data) {
                                try {
                                    if (data.success) {
                                        $('#tbSysParamSetting').datagrid('reload'); // reload the user data
                                        $.messager.alert({	// show error message
                                            title: '<%= Language("SuccessTips")%>', //成功提示
                                            msg: data.msg
                                        });
                                    } else {
                                        $.messager.alert({	// show error message
                                            title: '<%= Language("FailureTips")%>', //失敗提示
                                            msg: data.msg
                                        });
                                    }
                                }
                                catch (e) {
                                    $.messager.alert({ title: '<%= Language("ExceptionTips")%>', //異常提示
                                        msg: '<%= Language("DataException")%>'//數據異常，請確認網絡連接狀況
                                    });
                                }
                            },
                            dataType: 'json',
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                parent.ajaxLoadEnd();
                                $.messager.alert({
                                    title: res.FailureTips,
                                    msg: res.IntenetException
                                });
                            }
                        });
                    }
                });
            }
        }

        //搜索
        function Search(type) {
            var queryParams = $('#tbSysParamSetting').datagrid('options').queryParams;
            queryParams.SearchType = type;
            queryParams.KeyWord = $('#txtKeyword').val().replace('<%#Language("InputDefaultKey")%>', '');
            $('#tbSysParamSetting').datagrid('reload');
        }
    </script>
</head>
<body>
    <div style="width: 98%; background-color: transparent;">
        <div id="divOperation" class="Search">
            <div class="l leftSearch">
                <span>
                    <input type="text" id="txtKeyword" style="width: 200px;" value="<%=Language("InputDefaultKey")%>"
                        onfocus="this.value=''" onblur="if(!value){value=defaultValue;}" /></span><span><a
                            href="javascript:void(0);" onclick="Search('ByKey')"><%=Language("Search")%></a></span>
            </div>
            <div class="r rightSearch">
                <img src="../../images/add.gif" /><a href="javascript:void(0)" onclick="Add()">
                    <%=Language("Add")%></a> &nbsp;&nbsp;
            </div>
        </div>
        <div style="clear: both">
        </div>
        <table id="tbSysParamSetting" width="98%" fit="false">
        </table>
    </div>
    <div style="clear: both">
    </div>
    <div id="AddSysParam" class="easyui-window" data-options="closed:true,modal:true"
        style="width: 510px; height: 280px; display: block; padding-top: 10px;" buttons="#btn_AddSysParam">
        <form id="fm" method="post">
        <table style="width: 98%; padding-top: 3px; padding-bottom: 3px;">
            <tr>
                <td style="width: 20%; padding-left: 20px;" align="left">
                    <%=Language("comp_name")%>：
                </td>
                <td style="width: 30%;" align="left">
                    <input id="aCompanyId" name="companyid" style="width:125px;"/>
                </td>
                <td align="left">
                    <%=Language("Usy") %>：
                </td>
                <td align="left">
                    <input id="aUsey" name="usey" style="width:125px;"/>
                </td>
            </tr>
            <tr>
                <td style="width: 20%; padding-left: 20px;">
                    <%=Language("parakey")%>：
                </td>
                <td align="left">
                    <input id="aParaKey" name="parakey" style="width: 125px;" />
                </td>
                <td align="left">
                    <%=Language("paraname")%>：
                </td>
                <td align="left">
                    <input id="aParaName" name="paraname" style="width: 125px;" />
                </td>
            </tr>
            <tr>
                <td style="width: 20%; padding-left: 20px;">
                    <%=Language("paracontent")%>：
                </td>
                <td colspan="3" align="left">
                    <textarea rows="3" style="width: 345px;" id="aParaContent" name="paracontent"></textarea>
                </td>
            </tr>
            <tr>
                <td style="width: 20%; padding-left: 20px;">
                    <%=Language("paradesc")%>：
                </td>
                <td colspan="3" align="left">
                    <textarea rows="2" style="width: 345px;" id="aParaDesc" name="paradesc"></textarea>
                </td>
            </tr>
        </table>
        </form>
    </div>
    <div id="btn_AddSysParam" style="display: none; padding-right: 20px;">
        <a class="easyui-linkbutton" data-options="text:'<%=Language("Save")%>'" href="javascript:void(0)"
            onclick="Save()">
            <%=Language("Save")%></a> <a class="easyui-linkbutton" data-options="text:'<%=Language("Cancel")%>'"
                href="javascript:void(0)" onclick="closeAddWindow('AddSysParam')">
                <%=Language("Cancel")%></a>
    </div>
</body>
</html>
