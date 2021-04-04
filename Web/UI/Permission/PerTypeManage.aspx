<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerTypeManage.aspx.cs" Inherits="UI_Permission_PerTypeManage" %>

<%@ Register Src="../../UIControl/Permission/PerTypeManage/dialogdivRose.ascx" TagName="dialogdivRose"
    TagPrefix="uc1" %>
<%@ Register Src="../../UIControl/Permission/PerTypeManage/dialogdivRoseDetail.ascx"
    TagName="dialogdivRoseDetail" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title></title>
    <style type="text/css">
        #fm {
            margin: 0;
        }

        .fitem {
            margin-bottom: 5px;
            text-align: left;
        }

            .fitem label {
                display: inline-block;
                text-align: left;
            }
    </style>
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
        $(document).ready(function () {
            dialogTransfer("divRose");
            dialogTransfer("divRoseDetail");
            //綁定datagrid
            $('#tbRose').datagrid({
                //是否折叠
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: '../../ASHX/Permission/RoseManage.ashx?M=Search',
                //數據在一行顯示 
                nowrap: true,
                fitColumns: true,
                //行條紋化
                striped: true,
                //固定序號
                rownumbers: true,
                //是否可以多選
                singleSelect: true,
                sortName: 'RoseId',
                sortOrder: 'desc',
                remoteSort: false,
                idField: 'RoseId',
                loadMsg: '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: true,
                //固定列  
                frozenColumns: [[{ field: 'ck', checkbox: true }]],
                //可動列
                columns: [[{ field: 'RoseId', title: '<%=Language("RoseId")%>', width: $(this).width() * 0.2, align: 'left' },
                           { field: 'RoseName', title: '<%=Language("RoseName")%>', width: $(this).width() * 0.3, align: 'left' },
                           { field: 'SystemType', title: '<%=Language("SystemType")%>', width: $(this).width() * 0.1, align: 'left', formatter: function (value, row, index) { return ReadType(row.SystemType); } },
                           { field: 'Usy', title: '<%=Language("Usy")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'Opt', title: '<%=Language("Opt")%>', width: $(this).width() * 0.2, align: 'left', formatter: function (value, row, index) { return '<a href="#" <%=IsUsy("Delete") %> onclick="<%=IsCanClick("Delete") %>delRose(\'' + index + '\')"><%=Language("Delete")%></a> | <a href="#" <%=IsUsy("View") %> onclick="<%=IsCanClick("View") %>viewRose(\'' + index + '\')"><%=Language("View")%></a> | <a href="#" <%=IsUsy("Edit") %> onclick="<%=IsCanClick("Edit") %>editRose(' + index + ');"><%=Language("Edit")%></a>'; } }
                ]]
            });
            $(window).resize(function(){
                $('#tbRose').datagrid('resize');
            });
            //設置分页控件屬性 
            var p = $('#tbRose').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: <%= Language("PageList")%>,
                beforePageText: '<%= Language("BeforePageText")%>',
                afterPageText: '<%= Language("AfterPageText")%>',
                displayMsg: '<%= Language("DisplayMsg")%>'
            });

            //綁定datagrid
            $('#tbUser').datagrid({
                //是否折叠
                collapsible: true,
                url: '../../ASHX/Permission/RoseManage.ashx?M=detail',
                //數據在一行顯示 
                nowrap: true,
                //行條紋化
                striped: true,
                //固定序號
                rownumbers: true,
                //是否可以多選
                singleSelect: false,
                sortName: 'UserId',
                sortOrder: 'desc',
                remoteSort: false,
                idField: 'UserId',
                loadMsg:  '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: false,
                //可動列
                columns: [[{ field: 'UserId', title: '<%=Language("UserId")%>', width: 200, align: 'center' },
                           { field: 'UserName', title: '<%=Language("UserName")%>', width: 200, align: 'center' },
                           { field: 'UserDept', title: '<%=Language("UserDept")%>', width: 100, align: 'center' }
                ]]
            });
        });
        var url = "";
        function ReadType(SystemType) {
            if (SystemType == "01")
                return '<%=Language("systemuser")%>';//systemuser 系統用戶
            else if (SystemType == "02")
                return '<%=Language("supplyid")%>';//supplyid 供應商
            else
                return '<%=Language("customername")%>';//customername 客戶
    }
    function delRose(index) {
        var ids = [];
        if (index) {
            var row = $('#tbRose').datagrid('getData').rows[index];
            ids.push(row.RoseId);
        }
        else {
            var rows = $('#tbRose').datagrid('getChecked');
            for (var i = 0; i < rows.length; i++) {
                //每行ID放入數組中
                ids.push(rows[i].RoseId);
            }
        }
        if (ids.length > 0) {
            //必須為string類型，否則傳輸不過去 
            var aa = ids.toString();
            $.messager.defaults = { ok: '<%=Language("yes")%>', cancel: '<%=Language("mesageNo")%>'};//yes 是 mesageNo 否 
            $.messager.confirm('<%=Language("DeleteConfirm")%>', '<%=Language("delPerConfirm")%>', function (r) {//DeleteConfirm 刪除確認 delPerConfirm 你確定需要刪除權限類別?
                if (r) {
                    $.post('../../ASHX/Permission/RoseManage.ashx?M=delete', { RoseIdList: aa },
                    function (result) {
                        try {
                            if (result.success) {
                                $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("deletesuccess")%>');//SuccessTips 成功提示 deletesuccess 删除成功 
                                $('#tbRose').datagrid('reload');
                            } else {
                                $.messager.alert('<%=Language("errTips")%>', result.msg);
                            }
                        }
                        catch (e) {
                            $.messager.alert({
                                title: '<%=Language("ExceptionTips")%>', //ExceptionTips 異常提示
                                    msg: '<%=Language("SubmitDataException")%>'//SubmitDataException 提交數據異常，請確認網絡連接狀況
                                });
                            }
                    }, 'json');
                    }
            });
            }
            else {
                $.messager.alert('<%=Language("msgDel")%>', '<%=Language("DelTips")%>');//msgDel 刪除數據 DelTips 請選擇需要刪除的數據
        }
    }

      

    function Search(type) {
        try {
            var queryParams = $('#tbRose').datagrid('options').queryParams;
            queryParams.SearchType = type;
            queryParams.KeyWord = $('#txtKeyword').val().replace('<%=Language("InputDefaultKey")%>','');
            queryParams.RoseId = $('#sRoseId').val();
            queryParams.RoseName = $('#sRoseName').val();
            queryParams.SystemType = $('#sSystemType').val();
            queryParams.Usy = $('#sUsy').val();
            $('#tbRose').datagrid('load');
        }
        catch (e) {
            $.messager.alert({
                title: '<%=Language("ExceptionTips")%>', //ExceptionTips 異常提示
                    msg: '<%=Language("SubmitDataException")%>'//SubmitDataException 提交數據異常，請確認網絡連接狀況
                });
            }
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
                            href="javascript:void(0);" <%=IsUsy("Search") %> onclick="<%=IsCanClick("Search") %>Search('ByKey')"><%=Language("Search")%></a></span><span><a
                                href="javascript:void(0);" <%=IsUsy("AdvancedSearch") %> onclick="<%=IsCanClick("AdvancedSearch") %>openSearch('divSearch');"><%=Language("AdvancedSearch")%></a></span>
            </div>
            <div class="r rightSearch">
                <img src="../../images/add.gif" />
                <a href="javascript:void(0)" <%=IsUsy("Add") %> onclick="<%=IsCanClick("Add") %>addRose()">
                    <%=Language("Add")%></a> &nbsp;
                <img src="../../images/del.gif" />
                <a href="javascript:void(0)" <%=IsUsy("BatchDelete") %> onclick="<%=IsCanClick("BatchDelete") %>delRose()">
                    <%=Language("BatchDelete")%></a> &nbsp;&nbsp;
            </div>
        </div>
        <div style="clear: both">
        </div>
        <div class="squery" id="divSearch">
            <div class="sinquery">
                <table cellpadding="0" cellspacing="0" class="addCoyTB">
                    <tr>
                        <td>
                            <%=Language("RoseId")%>:
                        </td>
                        <td>
                            <input type="text" id="sRoseId" />
                        </td>
                        <td>
                            <%=Language("RoseName")%>:
                        </td>
                        <td>
                            <input type="text" id="sRoseName" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Language("SystemType")%>:
                        </td>
                        <td>
                            <select style="width: 90px" id="sSystemType">
                                <option value="">ALL</option>
                                <option value="01">
                                    <%=Language("systemuser")%></option>
                                <option value="02">
                                    <%=Language("supplyid")%></option>
                                <option value="03">
                                    <%=Language("customername")%></option>
                            </select>
                        </td>
                        <td>
                            <%=Language("Usy")%>:
                        </td>
                        <td>
                            <select style="width: 65px" id="sUsy">
                                <option value="">ALL</option>
                                <option value="Y">Y</option>
                                <option value="N">N</option>
                            </select>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="clear: both">
            </div>
            <div style="text-align: right; padding: 5px 0; border: 0px; height: 21px; font-size: 12px;">
                <a href="javascript:void(0)" onclick="Search('ByAdvanced')">
                    <%=Language("Search")%></a>&nbsp;&nbsp;&nbsp;&nbsp;
            </div>
            <div style="clear: both">
            </div>
        </div>
        <div style="clear: both">
        </div>
        <table id="tbRose" width="100%" fit="false">
        </table>
    </div>
    <uc1:dialogdivRose ID="dialogdivRose1" runat="server" />
    <div id="dlg-buttons">
        <a class="easyui-linkbutton" href="javascript:void(0)" onclick="saveRose()">
            <%=Language("Save")%></a> <a class="easyui-linkbutton" href="javascript:void(0)"
                onclick="closeAddWindow('divRose')">
                <%=Language("Cancel")%></a>
    </div>
    <div style="clear: both">
    </div>
    <uc2:dialogdivRoseDetail ID="dialogdivRoseDetail1" runat="server" />
</body>
</html>
