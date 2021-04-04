<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dialogaddMenuDiv.ascx.cs"
    Inherits="UIControl_Program_dialogaddMenuDiv" %>
<script type="text/javascript">
    function addToMenu() {
        if (historymenuId != '') {
            var node = $('#MainMenu').tree("find", historymenuId);
            if (node != null) {
                $('#MainMenu').tree('select', node.target);
            }
        }
        else {
            $('#MainMenu').tree('select', '');
        }
        $('#addMenuDiv').dialog('open');
        $("#pmenu_usy").val('Y');

    }
    function saveMenuID() {

        $('#addMenuDiv').dialog('close');
    } 
</script>
<div id="addMenuDiv" class="easyui-window" data-options="closed:true,modal:true,title:'<%=Language("addnewrecord")%>',iconCls:'icon-save'"
    style="width: 600px; height: 260px; padding: 5px;" buttons="#cxc_dlg-buttons">
    <form id="fm_MenuDiv" method="post" novalidate>
    <table border="0" cellpadding="0" cellspacing="1" bgcolor="#a8c7ce" style="width: 100%;
        height: 100%">
        <tr>
            <td width="50%" height="20" bgcolor="#E0ECFF" style="text-align: center;">
                <%=Language("MenuList")%> <%--MenuList 菜單列表--%>
            </td>
            <td width="50%" height="20" bgcolor="#E0ECFF" style="text-align: center;">
            </td>
        </tr>
        <tr>
            <td rowspan="3" style="background: #fff;">
                <ul id="MainMenu" class="easyui-tree" fit="true" data-options="animate:true,dnd:false">
                </ul>
            </td>
        </tr>
        <tr>
            <td style="background: #fff; padding: 5px; vertical-align: top;">
                <table cellpadding="0" cellspacing="0" class="tbStyle">
                    <tr>
                        <td bgcolor="#e8f5ff">
                            <%=Language("menu_id")%>： <%--menu_id 菜單編號--%>
                        </td>
                        <td>
                            <input name="programid" id="menuProg_id" />
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#e8f5ff">
                            <%=Language("Usy")%>：
                        </td>
                        <td>
                            <select style="width: 100px; font-size: 12px;" name="usy" id="pmenu_usy">
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
<div id="cxc_dlg-buttons">
    <a class="easyui-linkbutton" data-options="text:'<%=Language("Save")%>'" href="javascript:void(0)" onclick="saveMenuID()">
        <%=Language("Save")%></a> <a class="easyui-linkbutton" data-options="text:'<%=Language("Cancel")%>'" href="javascript:void(0)"
            onclick="closeAddWindow('addMenuDiv')">
            <%=Language("Cancel")%></a>
</div>
