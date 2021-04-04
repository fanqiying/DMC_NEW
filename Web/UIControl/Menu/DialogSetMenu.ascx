<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DialogSetMenu.ascx.cs"
    Inherits="UIControl_Menu_DialogSetMenu" %>
<script type="text/javascript">
    var editfatherid = '';
    function addMenu() {
        try {
            isEdit = false;
            LoadTree();
            $('#setMenuDiv').dialog('open').dialog('setTitle', '<%= Language("menu_addTitle")%>');
            $('#fm').form('clear');
            $("#menu_id").removeAttr("style");
            $("#menu_usy").val('Y');
            $("#menu_id").attr("readonly", "readonly");
            $("#menu_id").css("background-color", "#e1e1e1");
            $("#menu_id").val(newMenuNo);
            url = '../../ASHX/SystemManage/MenuManage.ashx?M=add';
        }
        catch (e) {
            $.messager.alert({
                title: '異常提示',
                msg: '數據異常，請確認網絡連接狀況'

            });
        }
    }
    function editMenu(index) {
        isEdit = true;
        fatherid = '';
        $('#setMenuDiv').dialog('open').dialog('setTitle', '<%= Language("menu_modTitle")%>');
        $("#menu_id").attr("readonly", "readonly");
        $("#menu_id").css("background-color", "#e1e1e1");
        var row = $('#tbMenu').datagrid('getData').rows[index];
        if (row) {
            editfatherid = row.fatherid;
            //                $("#menu_id").val(row.menuid);
            LoadTree();
            try {
                $('#fm').form('load', row);
                $('#setMenuDiv').dialog('open');
                url = '../../ASHX/SystemManage/MenuManage.ashx?M=update';
            }
            catch (e) {
                $.messager.alert({
                    title: '異常提示',
                    msg: '網絡異常，請確認網絡連接狀況'
                });
            }
        }
    }
    function saveMenu() {
        var node = $('#tree').tree('getSelected');
        var fatherId = '';
        if (node != null)
            fatherId = node.id;
        $('#fm').form('submit', {
            url: url + '&fatherID=' + fatherId,
            onSubmit: function () {
                return $(this).form('validate');
            },
            success: function (result) {
                try {
                    var result = eval('(' + result + ')');
                    if (result.success) {
                        $.messager.defaults = { ok: "確定" };
                        $.messager.alert('成功提示', '數據已保存成功');
                        $('#setMenuDiv').dialog('close'); 	// close the dialog                  
                        $('#tbMenu').datagrid('reload'); // reload the user data
                        $('#tree').tree("reload");
                    } else {
                        $.messager.alert({
                            title: 'Error',
                            msg: result.msg
                        });
                    }
                }
                catch (e) {
                    $.messager.alert({
                        title: '異常提示',
                        msg: '提交數據異常，請確認網絡連接狀況'
                    });
                }
            }
        });
    } 
    

    function GetMenuListByKey(op) {
        auTips({ input: op, keywords: op.value });
    }



</script>
<div id="setMenuDiv" class="easyui-window" data-options="closed:true,modal:true"
    style="width: 620px; height: 310px; padding: 3px;" maximizable="true" buttons="#dlg-buttons">
    <form id="fm" method="post" novalidate>
    <table border="0" cellpadding="0" cellspacing="1" bgcolor="#a8c7ce" style="width: 100%;
        height: 90%;">
        <tr>
            <td width="50%" height="20px" bgcolor="#E0ECFF" style="text-align: center;">
                <%=Language("setMenuTitle")%>
            </td>
            <td width="50%" bgcolor="#E0ECFF" style="text-align: center;">
                填寫菜單信息
            </td>
        </tr>
        <tr>
            <td style="background: #fff; padding: 5px; vertical-align: top; height: 80%;">
                <div style="height: 100%; overflow:auto;">
                    <ul id="tree" class="easyui-tree" fit="true" data-options="animate:true,dnd:false">
                    </ul>
                </div>
            </td>
            <td style="background: #fff; padding: 5px; vertical-align: top;">
                <table cellpadding="0" cellspacing="0" class="tbStyle"  style="width: 100%;
        height: 90%;">
                    <tr>
                        <td bgcolor="#e8f5ff">
                            <%=Language("menu_id")%>：
                        </td>
                        <td>
                            <input type="text" name="menuid" class="easyui-validatebox" required="true" id="menu_id"
                                style="border: 0px;" />
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#e8f5ff">
                            <%=Language("menu_name")%>：
                        </td>
                        <td>
                            <input type="text" value="" name="menuname" class="easyui-validatebox" required="true"
                                id="menuname" />
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#e8f5ff">
                            <%=Language("orderid")%>：
                        </td>
                        <td>
                            <input type="text" value="" name="orderid" class="easyui-validatebox" required="true"
                                id="orderid" />
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#e8f5ff">
                            <%=Language("Usy")%>：
                        </td>
                        <td>
                            <select style="width: 100px; font-size: 12px;" name="usy" id="menu_usy">
                                <option value="Y">Y</option>
                                <option value="N">N</option>
                            </select>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</div>
<div id="dlg-buttons">
    <a class="easyui-linkbutton" href="javascript:void(0)" onclick="saveMenu()">
        <%=Language("Save")%></a> <a class="easyui-linkbutton" href="javascript:void(0)"
            onclick="closeAddWindow('setMenuDiv');">
            <%=Language("Cancel")%></a>
</div>
