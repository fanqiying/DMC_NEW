<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DialogAddDiv.ascx.cs"
    Inherits="UIControl_Organize_Dept_DialogAddDiv" %>
<div id="addNewDiv" class="easyui-dialog" data-options="closed:true,modal:true,title:'<%=Language("dept_addTitle")%>'"
    style="width: 530px; height: 240px; padding: 5px;" buttons="#dlg-buttons" maximizable="true">
    <form id="fm" method="post">
        <table align="center" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td>
                    <%=Language("dept_id")%>：
                </td>
                <td>
                    <input name="deptid" id="deptid" class="easyui-textbox" style="width: 150px;" data-option="required:true" maxlength="20" onchange="GetFlseID();">
                </td>
                <td>
                    <%=Language("falsedept_id")%>：
                </td>
                <td>
                    <input name="falsedeptid" id="falseDeptID" style="width: 150px;" class="easyui-textbox" maxlength="20">
                </td>
            </tr>
            <tr>
                <td>
                    <%=Language("dept_sname")%>：
                </td>
                <td>
                    <input name="simplename" id="simplename" style="width: 150px;" class="easyui-textbox" data-option="required:true">
                </td>
                <td>
                    <%=Language("dept_fname")%>：
                </td>
                <td>
                    <input type="text" class="easyui-textbox" style="width: 150px;" name="fullname" />
                </td>
            </tr>
            <tr>
                <td>
                    <%=Language("dept_sex")%>：
                </td>
                <td>
                    <input id="dept_nature" name="deptnatureid" style="width: 150px;" value="0" />
                    <input type="hidden" id="hideGroup" />
                    <input type="hidden" id="hideSelectGroup" />
                </td>
                <td>
                    <%=Language("dept_dead")%>：
                </td>
                <td>
                    <input type="text" class="easyui-textbox" style="width: 150px;" id="deptheader" name="deptheader" onkeyup="GetUserList(this)" />
                </td>
            </tr>
            <tr>
                <td>
                    <%=Language("dept_group")%>：
                </td>
                <td>
                    <input type="text" class="easyui-textbox" style="width: 150px;" name="deptgroup" id="dept_group" onkeyup="GetDeptIDListByKey(this);" />
                </td>
                <td>
                    <%=Language("Usy")%>：
                </td>
                <td>
                    <select class="easyui-combobox" style="width: 150px;" name="usy" id="dept_usy">
                        <option value="Y">Y</option>
                        <option value="N">N</option>
                    </select>
                </td>
            </tr>
        </table>
    </form>
</div>
