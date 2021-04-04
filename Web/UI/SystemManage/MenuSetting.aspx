<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuSetting.aspx.cs" Inherits="SystemManage_MenuSetting" %>

<%@ Register Src="../../UIControl/Menu/DialogSetMenu.ascx" TagName="DialogSetMenu"
    TagPrefix="uc1" %>
<%@ Register Src="../../UIControl/Menu/DialogMenuDetail.ascx" TagName="DialogMenuDetail"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7" />
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../css/public.css" />
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../easyUI15/jquery.min.js"></script>
    <script type="text/javascript" src="../../easyUI15/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../js/jquery.json-2.4.js"></script>
    <script type="text/javascript" src="../../easyUI15/locale/easyui-lang-zh_TW.js"></script>
    <script type="text/javascript" src="../../js/common.js"></script>
    <script type="text/javascript" src="../../js/autotips_menu.js"></script>
    <script src="../../js/keyUp.js" type="text/javascript"></script>
    <script type="text/javascript">
        var editfatherid = '';
        var isEdit = false;
        $(document).ready(function () {
            dialogTransfer("divMenuDetail");
            dialogTransfer("setMenuDiv");
            //实例化树形菜单
            $("#tree").tree({
                lines: true,
                onSelect: function (node) {
                    var currentMenuId = $("#menu_id").val();

                    if (currentMenuId != '' && !isEdit) {
                        var isResult = true;
                        if (currentMenuId == node.id) {
                            alert('<%=Language("notselectself")%>'); //notselectself
                            isResult = false;
                        }
                        else {
                            var currentNode = $("#tree").tree("find", currentMenuId);

                            $.post('../../ASHX/SystemManage/MenuManage.ashx?M=GetNode', { selectedNode: node.id },
                         function (result) {
                             try {
                                 if (result.split('|')[0].indexOf("OK") > -1) {

                                     $("#menu_id").val(result.split('|')[1]);
                                 } else {

                                 }
                             }
                             catch (e) {
                                 $.messager.alert({
                                     title: '<%=Language("ExceptionTips")%>', //ExceptionTips 異常提示
                                     msg: '<%=Language("SubmitDataException")%>'//SubmitDataException 提交數據異常，請確認網絡連接狀況
                                 });

                             }
                         }, 'text');

                            if (currentNode != null) {
                                var childrenNodes = $("#tree").tree("getChildren", currentNode.target);
                                for (var i = 0; i < childrenNodes.length; i++) {
                                    if (childrenNodes[i].id == node.id) {
                                        lert('<%=Language("notselectself")%>'); //notselectself
                                        isResult = false
                                        break;
                                    }
                                }
                            }
                        }
                        if (!isResult)
                            $("#tree").tree("select", '');
                    }
                },
                onLoadSuccess: function (node, data) {
                    if (editfatherid != '') {
                        var node = $('#tree').tree('find', editfatherid);
                        $('#tree').tree('select', node.target);
                    }
                }
            });
        });
        function LoadTree() {
            $('#tree').tree('options').url = "../../ASHX/Permission/TreeJson.ashx?M=maintree";
            $('#tree').tree('reload');
        } 
    </script>
    <script type="text/javascript">
        var newMenuNo;
        $(document).ready(function () {
            //綁定datagrid
            $('#tbMenu').datagrid({
                //是否折叠
                collapsible: true,
                url: '../../ASHX/SystemManage/MenuManage.ashx?M=Load',
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
                columns: [[{ field: 'menuid', title: '<%=Language("menu_id")%>', width:$(this).width() * 0.1, align: 'left' },
                           { field: 'menuname', title: '<%=Language("menu_name")%>', width:$(this).width() * 0.1, align: 'left' },
                           { field: 'orderid', title: '<%=Language("orderid")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'usy', title: '<%=Language("Usy")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'Opt', title: '<%=Language("Opt")%>', width: $(this).width() * 0.2, align: 'left', formatter: function (value, row, index) { return '<a href="#" <%=IsUsy("Delete") %> onclick="<%=IsCanClick("Delete") %>delMuti(\'' + index + '\')"><%=Language("Delete")%></a> | <a href="#" <%=IsUsy("View") %> onclick="<%=IsCanClick("View") %>viewMenu(\'' + index + '\')"><%=Language("View")%></a> | <a href="#" <%=IsUsy("Edit") %> onclick="<%=IsCanClick("Edit") %>editMenu(' + index + ');"><%=Language("Edit")%></a>'; } }
                        ]]
            });
             //設置分页控件屬性 
            var p = $('#tbMenu').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: <%= Language("PageList")%>,
                beforePageText: '<%= Language("BeforePageText")%>',
                afterPageText: '<%= Language("AfterPageText")%>',
                displayMsg: '<%= Language("DisplayMsg")%>'
            });
            $(window).resize(function(){
                $('#tbMenu').datagrid('resize');
            });
            
            newMenuNo='<%=newMenuNo%>';

        });
    </script>
</head>
<body>
    <div class="Search" id="divOperation">
        <div class="r rightSearch">
            <img src="../../images/add.gif" />
            <a href="javascript:void(0)" <%=IsUsy("Add") %> onclick="<%=IsCanClick("Add") %>addMenu()">
                <%=Language("Add")%></a> &nbsp;
            <img src="../../images/del.gif" />
            <a href="javascript:void(0)" <%=IsUsy("BatchDelete") %> onclick="<%=IsCanClick("BatchDelete") %>delMuti()">
                <%=Language("BatchDelete")%></a> &nbsp;&nbsp;
        </div>
    </div>
    <input type="hidden" runat="server" id="newnodeid" />
    <div style="clear: both">
    </div>
    <table id="tbMenu" width="99%" fit="false">
    </table>
    <div style="clear: both">
    </div>
    <uc1:DialogSetMenu ID="DialogSetMenu1" runat="server" />
    <uc2:DialogMenuDetail ID="DialogMenuDetail1" runat="server" />
</body>
</html>
