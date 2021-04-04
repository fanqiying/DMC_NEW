<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DialogMenuDetail.ascx.cs"
    Inherits="UIControl_Menu_DialogMenuDetail" %>
<%@ Register Src="~/UIControl/RevisionInfo.ascx" TagName="RevisionInfo" TagPrefix="vri" %>
<script type="text/javascript">
    var url = "";
    function delMuti(index) {
        var ids = [];
        if (index) {
            var row = $('#tbMenu').datagrid('getData').rows[index];
            ids.push(row.menuid);
        }
        else {
            var rows = $('#tbMenu').datagrid('getChecked');
            for (var i = 0; i < rows.length; i++) {
                //每行ID放入數組中
                ids.push(rows[i].menuid);
            }
        }
        if (ids.length > 0) {
            //必須為string類型，否則傳輸不過去 
            var idlist = ids.toString();
            $.messager.defaults = { ok: "是", cancel: "否" };
            $.messager.confirm('刪除確認', '你確定需要刪除吗?', function (r) {
                if (r) {
                    $.post('../../ASHX/SystemManage/MenuManage.ashx?M=delete', { menuIDstr: idlist },
                        function (result) {
                            if (result.success) {

                                $('#tbMenu').datagrid('reload'); // reload the user data
                            } else {
                                $.messager.alert({	// show error message
                                    title: 'Error',
                                    msg: result.msg
                                });
                            }
                        }, 'json');
                }
            });
        }
        else {
            $.messager.alert('刪除數據', '請選擇需要刪除的數據');
        }
    }
    function viewMenu(index) {
        var row = $('#tbMenu').datagrid('getData').rows[index];
        if (row) {
            try {
                $('#fmView').form('load', row);
                LoadTree();
                $('#divMenuDetail').dialog({
                    title: '查看菜單目錄維護'
                });
                LoadTelInfo();
                $('#divMenuDetail').dialog('open');
            }
            catch (e) {
                $.messager.alert({
                    title: '異常提示',
                    msg: '網絡異常，請確認網絡連接狀況'
                });
            }
        }
    }

</script>
<div id="divMenuDetail" class="easyui-window" data-options="closed:true,modal:true,title:'<%=Language("comp_viewInfo")%>'"
    style="width: 620px; height:250px; padding: 5px;" maximizable="true">
    <div class="easyui-layout" fit="true">
        <form id="fmView" method="get">
        <div region="north" border="false" style="height: 1px;" class="p-search">
        </div>
        <div region="center" border="false">
            <div class="easyui-tabs"  style="margin: auto; margin-top: 5px; height: 220px;" border="true"
                fit="true">
                <div title="<%=Language("BasicInfoTab")%>" style="overflow: auto; padding: 5px; text-align: left;">
                    <table cellpadding="0" cellspacing="0" class="tbView">
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("menu_id")%>：
                            </td>
                            <td>
                                <input readonly name="menuid" />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("menu_name")%>：
                            </td>
                            <td>
                                <input readonly name="menuname" />
                            </td>
                        </tr>
                         <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("orderid")%>：
                            </td>
                            <td>
                                <input readonly name="orderid" />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("Usy")%>：
                            </td>
                            <td>
                                <input readonly name="usy" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div title="<%=Language("ReviseInfoTab")%>" data-options="closable:false" style="overflow: auto;
                    padding: 3px; text-align: left;">
                    <vri:RevisionInfo ID="vri1" runat="server" />
                </div>
            </div>
        </div>
        </form>
    </div>
</div>
