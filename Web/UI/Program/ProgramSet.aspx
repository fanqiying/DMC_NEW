<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProgramSet.aspx.cs" Inherits="UI_Program_ProgramSet" %>

<%@ Register Src="../../UIControl/Program/dialogaddMenuDiv.ascx" TagName="dialogaddMenuDiv"
    TagPrefix="uc1" %>
<%@ Register Src="../../UIControl/Program/dialogaddNewDiv.ascx" TagName="dialogaddNewDiv"
    TagPrefix="uc2" %>
<%@ Register Src="../../UIControl/Program/dialogdivProgDetail.ascx" TagName="dialogdivProgDetail"
    TagPrefix="uc3" %>
<%@ Register Src="../../UIControl/Program/dialogfunctionMod.ascx" TagName="dialogfunctionMod"
    TagPrefix="uc4" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>程式管理作業</title>
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
    <script src="../../js/keyUp.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            dialogTransfer("addNewDiv");
            dialogTransfer("divProgDetail");
            dialogTransfer("functionMod");
            dialogTransfer("addMenuDiv");
            //綁定datagrid
            $('#tbProgram').datagrid({
                //是否折叠
                collapsible: true,
                url: '../../ASHX/Permission/ProgManage.ashx?M=Search',
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
                columns: [[{ field: 'programid', title: '<%=Language("ProgramId")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'programname', title: '<%=Language("ProgramName")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'functionstr', title: '<%=Language("program_func")%>', width: $(this).width() * 0.3, align: 'left' },                 
                           { field: 'orderid', title: '<%=Language("orderid")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'usy', title: '<%=Language("Usy")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'Opt', title: '<%=Language("Opt")%>', width: $(this).width() * 0.2, align: 'left',  
                           formatter: function (value, row, index) 
                           { 
                               var str='<a href="#" <%=IsUsy("View") %> onclick="<%=IsCanClick("View") %> viewProgram(\'' + index + '\')"><%=Language("View")%></a> | ';
                               str+='<a href="#" <%=IsUsy("Edit") %> onclick="<%=IsCanClick("Edit") %>editProgram(' + index + ')" ><%=Language("Edit")%></a>';
                               return str; 
                           } 
                           }
                        ]]
            });
            
           //設置分页控件屬性 
            var p = $('#tbProgram').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: <%= Language("PageList")%>,
                beforePageText: '<%= Language("BeforePageText")%>',
                afterPageText: '<%= Language("AfterPageText")%>',
                displayMsg: '<%= Language("DisplayMsg")%>'
            });
             $(window).resize(function(){
                $('#tbProgram').datagrid('resize');
            });

            $('#MainMenu').tree({
                url: '../../ASHX/Permission/TreeJson.ashx?M=maintree',
                onSelect:function (node) {
                    debugger;
                    if(node)
                    {
                        $("#menuProg_id").val(node.id);
                        $("#program_menuid").val(node.id);
                        if (node.attributes) {

                        }
                        else {

                        }
                    }
                }
            }); 
        }); 
        
        function delMuti(index) {
           var ids = [];
            if (index) {
                var row = $('#tbProgram').datagrid('getData').rows[index];
                ids.push(row.programid);
            }
            else {
                var rows = $('#tbProgram').datagrid('getChecked');
                for (var i = 0; i < rows.length; i++) {
                    //每行ID放入數組中
                    ids.push(rows[i].programid);
                }
            }       
            if (ids.length > 0) {
                //必須為string類型，否則傳輸不過去 
                var idlist = ids.toString();  
                 $.messager.defaults =  { ok: '<%=Language("yes")%>', cancel: '<%=Language("mesageNo")%>'};//yes 是 mesageNo 否
                $.messager.confirm('<%=Language("DeleteConfirm")%>', '<%=Language("DeleteConfirm")%>', function (r) {
                    if (r) {
                        $.post('../../ASHX/Permission/ProgManage.ashx?M=delete', { programIDstr: idlist },
                         function (result) {
                             try {
                                 if (result.success) {
                                     $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("deletesuccess")%>');//SuccessTips 成功提示 deletesuccess 删除成功 
                                     $('#tbProgram').datagrid('reload'); // reload the user data
                                 } else {
                                     $.messager.alert({	// show error message
                                         title: '<%=Language("errTips")%>',
                                         msg: result.msg
                                     });
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
        var url = "";
        function Search(type) {
            var queryParams = $('#tbProgram').datagrid('options').queryParams;
            queryParams.SearchType = type;
            queryParams.KeyWord = $('#txtKeyword').val().replace('<%=Language("InputDefaultKey")%>', '');
            queryParams.programid = $('#txtProgID').val();
            queryParams.programname = $('#txtProgName').val();
            queryParams.menuid=$("#txtsMenuId").combotree("getValue");
            queryParams.usy = $('#txtProgUsy').val();
            $('#tbProgram').datagrid('load');
        } 
        var historymenuId=''; 
        var actionIds='';
        var actionNames='';
        function GetCheckId()
        {
            var functions=[];
            var rows = $('#progFutionList').datagrid('getChecked');
            for (var i = 0; i < rows.length; i++) {
                //每行ID放入數組中
                functions.push(rows[i].ActionId);
            }
            return functions.toString();
        }

        function GetCheckName()
        {
            var functionnames=[];
            var rows = $('#progFutionList').datagrid('getChecked');
            for (var i = 0; i < rows.length; i++) {
                //每行ID放入數組中
                functionnames.push(rows[i].ActionName);
            }
           return functionnames.toString();
        } 
    </script>
    <!--程式基本功能-->
    <script type="text/javascript">
        $(document).ready(function () {
            //綁定权限类别tbUserRose
            $('#progFutionList').datagrid({
                //是否折叠
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                //數據在一行顯示 
                nowrap: true,
                //固定序號
                rownumbers: true,
                //行條紋化
                striped: true,
                //是否可以多選
                singleSelect: false,
                sortName: 'ActionId',
                sortOrder: 'desc',
                remoteSort: false,
                idField: 'ActionId',
                loadMsg: '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: false,
                //固定列  
                frozenColumns: [[{ field: 'userrosechk', checkbox: true}]],
                //可動列
                columns: [[{ field: 'ActionId', title: '<%=Language("pfunc_id")%>', width: 200, align: 'center' },
                          { field: 'ActionName', title: '<%=Language("pfunc_name")%>', width: 200, align: 'center' }
                        ]],
                onLoadSuccess: function (data) {
                    if (data) {
                        $.each(data.rows, function (index, item) {
                            if (item.IsUsy == 'Y' && (actionIds == '' || actionIds.indexOf(item.ActionId) > -1)) {
                                $('#progFutionList').datagrid('checkRow', index);
                            }
                        });
                    }
                }
            });
        });

        var initFunctionList = [];
        //加載程式的基本功能列表带多语言
        function LoadFuncList() {
            $('#progFutionList').datagrid('options').url = "../../ashx/Permission/GetFunctionList.ashx";
            var queryParams = $('#progFutionList').datagrid('options').queryParams;
            queryParams.ProgramId = $('#program_id').val();
            $('#progFutionList').datagrid('load');
        }
    </script>
</head>
<body>
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
            <a href="javascript:void(0)" <%=IsUsy("Add") %> onclick="<%=IsCanClick("Add") %>addProgram()">
                <%=Language("Add")%></a> &nbsp; &nbsp;&nbsp;
        </div>
    </div>
    <div style="clear: both">
    </div>
    <div class="squery" id="divSearch">
        <div class="sinquery">
            <table cellpadding="0" cellspacing="0" class="addCoyTB">
                <tr>
                    <td>
                        <label>
                            <%=Language("ProgramId")%>:</label>
                    </td>
                    <td>
                        <input id="txtProgID" name="txtProgID" type="text" style="width: 200px;" />
                    </td>
                    <td>
                        <label>
                            <%=Language("ProgramName")%>:</label>
                    </td>
                    <td>
                        <input id="txtProgName" name="txtProgName" type="text" style="width: 350px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <%=Language("menu_name")%>:</label>
                    </td>
                    <td>
                        <input id="txtsMenuId" name="txtsMenuId" class="easyui-combotree" data-options="url:'../../ASHX/Permission/TreeJson.ashx?M=maintree',method:'get'"
                            style="width: 200px;" />
                    </td>
                    <td>
                        <label>
                            <%=Language("Usy")%>:&nbsp;&nbsp;&nbsp;&nbsp;</label>
                    </td>
                    <td>
                        <select style="width: 120px; font-size: 12px;" id="txtProgUsy" name="sbmcsEntity.acti">
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
    <table id="tbProgram" width="99%" fit="false">
    </table>
    <div style="clear: both">
    </div>
    <uc1:dialogaddMenuDiv ID="dialogaddMenuDiv1" runat="server" />
    <uc2:dialogaddNewDiv ID="dialogaddNewDiv1" runat="server" />
    <uc3:dialogdivProgDetail ID="dialogdivProgDetail1" runat="server" />
    <uc4:dialogfunctionMod ID="dialogfunctionMod1" runat="server" />
</body>
</html>
