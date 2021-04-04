<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Company.aspx.cs" Inherits="UI_Organize_Company" %>

<%@ Register Src="../../UIControl/Organize/Company/dialogaddNewDiv.ascx" TagName="dialogaddNewDiv"
    TagPrefix="uc1" %>
<%@ Register Src="../../UIControl/Organize/Company/dialogdivCompDetail.ascx" TagName="dialogdivCompDetail"
    TagPrefix="uc2" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="../../css/public.css" rel="stylesheet" />
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
            dialogTransfer("addNewDiv");
            dialogTransfer("divCompDetail");
            Combox();
            //綁定datagrid
            $('#tbComp').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: '../../ASHX/Organize/CompManage.ashx?M=search',
                //數據在一行顯示 
                nowrap: false,
                fitColumns: true,
                //行條紋化
                striped: true,
                //固定序號
                rownumbers: true,
                //是否可以多選
                singleSelect: true,
                remoteSort: false,
                idField: 'autoid',
                loadMsg: '數據加載中...',//數據加載中
                //是否顯示分页
                pagination: true,
                //固定列  
                frozenColumns: [[{ field: 'ck', checkbox: true }]],
                //可動列
                columns: [[{ field: 'companyid', title: '<%=Language("comp_id")%>', width: 100, align: 'left' },
                           {
                               field: 'compcategory', title: '<%=Language("comp_type")%>', width: 100, align: 'left',
                               formatter: function (value, row, index) {
                                   if (row.compcategory == "1") {
                                       return '<%=Language("CompanyGroup")%>';//CompanyGroup 集團
                                   }
                                   else {
                                       return '<%=Language("company")%>';//company 公司
                                   }
                               }
                           },
                           { field: 'simplename', title: '<%=Language("comp_name")%>', width: 100, align: 'left' },
                           { field: 'addrone', title: '<%=Language("comp_addrone")%>', width: 100, align: 'left' },
                           { field: 'comptel', title: '<%=Language("comp_tel")%>', width: 100, align: 'left' },
                           { field: 'usy', title: '<%=Language("Usy")%>', width: 100, align: 'left' },
                           {
                               field: 'Opt', title: '<%=Language("Opt")%>', width: 100, align: 'left', 
                               formatter: function (value, row, index) { return '<a href="#" <%=IsUsy("Delete") %> onclick="<%=IsCanClick("Delete") %>delMuti(\'' + index + '\')"><%=Language("Delete")%></a> | <a href="#" <%=IsUsy("View") %> onclick="<%=IsCanClick("View") %>viewComp(\'' + index + '\')"><%=Language("View")%></a> | <a href="#" <%=IsUsy("Edit") %> onclick="<%=IsCanClick("Edit") %>editComp(' + index + ');"><%=Language("Edit")%></a>'; }
                           }
                ]]
            });
            $(window).resize(function () {
                $('#tbComp').datagrid('resize');
            });
            //設置分页控件屬性 
            var p = $('#tbComp').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: [10, 15, 20, 25, 30],
                beforePageText: '第',
                afterPageText: '页 共{pages}页',
                displayMsg: '当前显示 {from} - {to} 条记录 共 {total} 条记录'
            });
        });

        function Search(type) {
            var queryParams = $('#tbComp').datagrid('options').queryParams;
            queryParams.SearchType = type;
            queryParams.KeyWord = $('#txtKeyword').val().replace('<%=Language("InputDefaultKey")%>', '');
            queryParams.companyid = $('#txtComyID').val();
            queryParams.compcategory = $("#txtType").val();
            queryParams.simplename = $('#txtCompName').val();
            queryParams.addrone = $('#txtCompAddr').val();
            queryParams.comptel = $('#txtCompTel').val();
            queryParams.usy = $('#txtcompUsy').val();
            $('#tbComp').datagrid('load');
        }

        function Combox() {
            //加載下拉框的多語言
            $('#compLanguageid').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=LanguageType',
                valueField: 'languageid',
                textField: 'languagename',
                panelHeight: '150',
                editable: false,
                multiple: false
            });
        }

        var url = "";
        function delMuti(index) {
            var ids = [];

            if (index) {
                var row = $('#tbComp').datagrid('getData').rows[index];
                ids.push(row.autoid);
            }
            else {
                var rows = $('#tbComp').datagrid('getChecked');
                for (var i = 0; i < rows.length; i++) {
                    //每行ID放入數組中
                    ids.push(rows[i].autoid);
                }
            }
            if (ids.length > 0) {
                //必須為string類型，否則傳輸不過去 
                var idlist = ids.toString();
                $.messager.defaults = { ok: '<%=Language("yes")%>', cancel: '<%=Language("mesageNo")%>' };//yes 是 mesageNo 否 
                $.messager.confirm('<%=Language("DeleteConfirm")%>', '<%=Language("comformDelMsg")%>', function (r) {//DeleteConfirm 刪除確認 comformDelMsg 你確定需要刪除吗?
                    if (r) {
                        $.post('../../ASHX/Organize/CompManage.ashx?M=delete', { empIDstr: idlist },
                                function (result) {
                                    if (result.success) {

                                        $('#tbComp').datagrid('reload'); // reload the user data
                                    } else {
                                        $.messager.alert({	// show error message
                                            title: '<%=Language("errTips")%>',
                                            msg: result.msg
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
        function Compcategory(id) {
            if (id == "1") {
                return '<%=Language("CompanyGroup")%>';//CompanyGroup 集團
            }
            else {
                return '<%=Language("company")%>';//company 公司
            }
        }
    </script>
</head>
<body>
    <div id="funMain" style="background-color: transparent; margin-left: 5px; margin-right: 0px;">
        <div class="Search" id="divOperation">
            <div class="l leftSearch">
                <span>
                    <input type="text" id="txtKeyword" style="width: 200px;" value="<%=Language("InputDefaultKey")%>"
                        onfocus="this.value=''" onblur="if(!value){value=defaultValue;}" /></span><span><a
                            href="javascript:void(0);" <%=IsUsy("Search") %> onclick="<%=IsCanClick("Search") %>Search('ByKey')"><%=Language("Search")%></a></span><span><a
                                href="javascript:void(0);" <%=IsUsy("AdvancedSearch") %> onclick="<%=IsCanClick("AdvancedSearch") %>openSearch('divSearch')"><%=Language("AdvancedSearch")%></a></span>
            </div>
            <div class="r rightSearch">
                <img src="../../images/add.gif" />
                <a href="javascript:void(0)" <%=IsUsy("Add") %> onclick="<%=IsCanClick("Add") %>addComp()">
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
                <table id="tbCoy" cellpadding="0" cellspacing="0" border="0" class="addCoyTB">
                    <tr>
                        <td>
                            <label id="lb_sbmcs13">
                                <%=Language("comp_id")%>:</label>
                        </td>
                        <td>
                            <input id="txtComyID" type="text" style="width: 120px;" />
                        </td>
                        <td>
                            <label id="lb_sbmcs08">
                                <%=Language("comp_type")%>:</label>
                        </td>
                        <td>
                            <select id="txtType" name="" style="width: 120px; font-size: 12px;">
                                <option value="">ALL</option>
                                <option value="1">
                                    <%=Language("CompanyGroup")%></option>
                                <option value="2">
                                    <%=Language("company")%></option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Language("comp_name")%>:
                        </td>
                        <td>
                            <input id="txtCompName" name="" type="text" />
                        </td>
                        <td>
                            <%=Language("comp_addr")%>:
                        </td>
                        <td>
                            <input id="txtCompAddr" name="" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Language("comp_tel")%>:
                        </td>
                        <td>
                            <input id="txtCompTel" name="" type="text" />
                        </td>
                        <td>
                            <%=Language("Usy")%>:
                        </td>
                        <td>
                            <select style="width: 120px; font-size: 12px;" id="txtcompUsy" name="">
                                <option value="" checked>ALL</option>
                                <option value="Y">Y</option>
                                <option value="N">N</option>
                            </select>
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
        <table id="tbComp" data-options="fit:false">
        </table>
        <div style="clear: both">
        </div>
    </div>
    <div style="clear: both">
    </div>
    <uc2:dialogdivCompDetail ID="dialogdivCompDetail1" runat="server" />
    <uc1:dialogaddNewDiv ID="dialogaddNewDiv1" runat="server" />
</body>
</html>
