<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysNo.aspx.cs" Inherits="UI_Basicdata_SysNo" %>

<%@ Register Src="~/UIControl/Basicdata/SysNo/DialogSetSysNo.ascx" TagName="dialogSysNo"
    TagPrefix="uc1" %>
<%@ Register Src="~/UIControl/Basicdata/SysNo/DialogSysNoDetail.ascx" TagName="dialogSysNoDetail"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>
        <%=Language("sbmi014")%></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="../../css/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../easyUI15/jquery.min.js"></script>
    <script type="text/javascript" src="../../easyUI15/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../js/jquery.json-2.4.js"></script>
    <script type="text/javascript" src="../../easyUI15/locale/easyui-lang-zh_TW.js"></script>
    <script type="text/javascript" src="../../js/common.js"></script>
    <script src="../../js/keyUp.js" type="text/javascript"></script>
    <link href="../../easyvalidator/css/validate.css" rel="stylesheet" type="text/css" />
    <script src="../../easyvalidator/js/easy_validator.pack.js" type="text/javascript"></script>
    <script src="../../easyvalidator/js/jquery.bgiframe.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var res =
{
    FailureTips:'<%=Language("FailureTips")%>',
    IntenetException: '<%=Language("IntenetException")%>',
    ExceptionTips: '<%=Language("ExceptionTips")%>',
    SubmitDataException: '<%=Language("SubmitDataException")%>'

};
        $(document).ready(function () {
            //Auth.showAuthDig();
            //Auth.init();
            tbSysNoBind();
            BindSelect();
            dialogTransfer("addSystemNo");
            dialogTransfer("ViewSysNo");
        });
        //--公司信息開始--// 
        function tbSysNoBind() {
            //綁定datagrid
            $('#tbSysNo').datagrid({
                //是否折叠
                collapsible: true,
                url: '../../ASHX/Basic/SysNoManage.ashx?M=search',
                //數據在一行顯示 
                nowrap: true,
                //自適應寬度
                fitColumns: true,
                //行條紋化
                striped: true,
                //固定序號
                rownumbers: false,
                //是否可以多選
                singleSelect: true,
                checkOnSelect: false,
                selectOnCheck: false,
                //sortName: 'autoid',
                //sortOrder: 'desc',
                remoteSort: false,
                idField: 'autoid',
                loadMsg: '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: true,
                //固定列  
                frozenColumns: [[{ field: 'ck', checkbox: true}]],
                //可動列
                columns: [[{ field: 'companyid', title: '<%=Language("CompanyId")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'moduletype', title: '<%=Language("ModuleType")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'modulartype', title: '<%=Language("ModularType")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'category', title: '<%=Language("comp_type")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'receipttype', title: '<%=Language("receipttype")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'keyword', title: '<%=Language("keyword")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'datetype', title: '<%=Language("datetype")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'codelen', title: '<%=Language("codelen")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'usy', title: '<%=Language("Usy")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'opt', title: '<%=Language("Opt")%>', width: $(this).width() * 0.1, align: 'left',  formatter: function (value, row, index) { return '<a href="#" <%=IsUsy("Delete") %> onclick="<%=IsCanClick("Delete") %>delSystemNo(\'' + index + '\')"><%=Language("Delete")%></a> | <a href="#" <%=IsUsy("View") %> onclick="<%=IsCanClick("View") %>view(\'' + index + '\')"><%=Language("View")%></a> | <a href="#" <%=IsUsy("Edit") %> onclick="<%=IsCanClick("Edit") %>modSystemNo(' + index + ');"><%=Language("Edit")%></a>';  } }
                ]]
            });
            //設置分页控件屬性 
            var p = $('#tbSysNo').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: <%= Language("PageList")%>,
                beforePageText: '<%= Language("BeforePageText")%>',
                afterPageText: '<%= Language("AfterPageText")%>',
                displayMsg: '<%= Language("DisplayMsg")%>'
            });
            $(window).resize(function () {
                $('#tbSysNo').datagrid('resize');
            });
        }

        function BindSelect()
        {
            $('#sCompanyId').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=companyclass',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight: '150',
                editable: false,
                multiple: false
            });
            $('#addcompany').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=companyclass',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight: '150',
                editable: false,
                multiple: false
            });
            $('#sModuleType').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=moduletype',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight: '150',
                editable: false,
                multiple: false
            });
            $('#addmoduletype').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=moduletype',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight: '150',
                editable: false,
                multiple: false
            });
            $('#sModularType').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=modulartype',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight: '150',
                editable: false,
                multiple: false
            });
            $('#addmodulartype').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=modulartype',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight: '150',
                editable: false,
                multiple: false
            });
            $('#sCategory').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=category',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight: '150',
                editable: false,
                multiple: false
            });
            $('#addcategory').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=category',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight: '150',
                editable: false,
                multiple: false
            });
            //            $('#sReceiptType').combobox({
            //                    url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=receipttype',
            //                    valueField: 'displayid',
            //                    textField: 'displaytext',
            //                    panelHeight: '150',
            //                    editable: false,
            //                    multiple: false
            //            });
            //            $('#addreceipttype').combobox({
            //                    url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=receipttype',
            //                    valueField: 'displayid',
            //                    textField: 'displaytext',
            //                    panelHeight: '150',
            //                    editable: false,
            //                    multiple: false
            //            });
            $('#adddatetype').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=datatype',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight: '150',
                editable: false,
                multiple: false
            });
            $('#sKeyword').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=programids',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight: '150',
                editable: false,
                multiple: false
            });
            $('#addkeyword').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=programids',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight: '150',
                editable: false,
                multiple: false
            });
        }

        //刪除
        function delSystemNo(index){
            var ids = [];
            if (index) {
                var row = $('#tbSysNo').datagrid('getData').rows[index];
                ids.push(row.autoid);
            }
            else {
                var rows = $('#tbSysNo').datagrid('getChecked');
                for (var i = 0; i < rows.length; i++) {
                    //每行ID放入數組中
                    ids.push(rows[i].autoid);
                }
            }
            if (ids.length > 0) {
                //必須為string類型，否則傳輸不過去 
                var aa = ids.toString();
                $.messager.defaults = { ok: '<%=Language("yes")%>', cancel: '<%=Language("mesageNo")%>' };
                $.messager.confirm('<%=Language("DeleteConfirm")%>', '<%=Language("comformDelMsg")%>', function (r) {
                    if (r) {
                        $.post('../../ASHX/Basic/SysNoManage.ashx?M=delete', { IDs: aa },
                        function (result) {
                            try {
                                if (result.success) {
                                    $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("delsucess")%>');
                                    $('#tbSysNo').datagrid('reload');
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
                        }, 'json').error(function () {
                            $.messager.alert({
                                title: '<%=Language("ExceptionTips")%>',
                                msg: '<%=Language("SubmitDataException")%>'
                            });
                        });

                    }
                 });
            }
            else {
                $.messager.alert('<%=Language("msgDel")%>', '<%=Language("DelTips")%>');
            }
        }
     
        function Search(type) {
            try {
                var queryParams = $('#tbSysNo').datagrid('options').queryParams;
                queryParams.SearchType = type;
                queryParams.KeyWord = $('#txtKeyword').val().replace('<%=Language("InputDefaultKey")%>','');
                queryParams.CompanyId = $('#sCompanyId').combobox("getValue");
                queryParams.ModuleType = $('#sModuleType').combobox("getValue");
                queryParams.ModularType = $('#sModularType').combobox("getValue");
                queryParams.Category = $('#sCategory').combobox("getValue");
                queryParams.ReceiptType = $('#sReceiptType').val();
                queryParams.Word = $('#sKeyword').combobox("getValue");
                queryParams.Usy = $('#sUsy').val();
                $('#tbSysNo').datagrid('load');
            }
            catch (e) {
                $.messager.alert({
                    title: '<%=Language("ExceptionTips")%>',
                    msg: '<%=Language("SubmitDataException")%>'
                });
            }
        } 
    </script>
</head>
<body>
    <div class="Search" id="divOperation">
        <div class="l leftSearch">
            <span>
                <input type="text" id="txtKeyword" style="width: 200px;" value="<%=Language("InputDefaultKey")%>"
                    onfocus="this.value=''" onblur="if(!value){value=defaultValue;}" /></span><span><a
                        href="javascript:void(0);" href="javascript:void(0);" <%=IsUsy("Search") %> onclick="<%=IsCanClick("Search") %>Search('ByKey')"><%=Language("Search")%></a></span><span><a
                            href="javascript:void(0);" <%=IsUsy("AdvancedSearch") %> onclick="<%=IsCanClick("AdvancedSearch") %>openSearch('divSearch');"><%=Language("AdvancedSearch")%></a></span>
        </div>
        <div class="r rightSearch">
            <img src="../../images/add.gif" /><a href="javascript:void(0)" <%=IsUsy("Add") %>
                onclick="<%=IsCanClick("Add") %>addSystemNo()">
                <%=Language("Add")%></a> &nbsp;
            <img src="../../images/del.gif" />
            <a href="javascript:void(0)" <%=IsUsy("BatchDelete") %> onclick="<%=IsCanClick("BatchDelete") %>delSystemNo()">
                <%=Language("BatchDelete")%></a> &nbsp;&nbsp;
        </div>
    </div>
    <div style="clear: both">
    </div>
    <div class="squery" id="divSearch">
        <div class="sinquery">
            <table id="tbCoy" cellpadding="0" cellspacing="0" border="0" class="addCoyTB">
                <tr>
                    <td>
                        <%=Language("CompanyId")%>:
                    </td>
                    <td>
                        <input style="width: 120px; font-size: 12px;" id="sCompanyId" />
                    </td>
                    <td>
                        <%=Language("ModuleType")%>:
                    </td>
                    <td>
                        <input style="width: 120px; font-size: 12px;" id="sModuleType" />
                    </td>
                    <td>
                        <%=Language("ModularType")%>:
                    </td>
                    <td>
                        <input style="width: 120px; font-size: 12px;" id="sModularType" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Language("comp_type")%>:
                    </td>
                    <td>
                        <input style="width: 120px; font-size: 12px;" id="sCategory" />
                    </td>
                    <td style="width: 14%;">
                        <%=Language("receipttype")%>:
                    </td>
                    <td style="width: 20%;">
                        <input style="width: 120px; font-size: 12px;" id="sReceiptType" />
                    </td>
                    <td style="width: 13%;">
                        <%=Language("keyword")%>:
                    </td>
                    <td style="width: 20%;">
                        <input style="width: 90px" id="sKeyword" />
                    </td>
                </tr>
                <tr>
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
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
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
    <div style="float: left; position: absolute; width: 100%">
        <table id="tbSysNo" width="99%" fit="false">
        </table>
    </div>
    <div style="clear: both">
    </div>
    <uc1:dialogSysNo ID="dialogSysNo1" runat="server" />
    <div style="clear: both">
    </div>
    <uc2:dialogSysNoDetail ID="dialogSysNoDetail1" runat="server" />
</body>
</html>
