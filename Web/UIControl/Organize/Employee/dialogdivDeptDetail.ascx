<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dialogdivDeptDetail.ascx.cs"
    Inherits="UIControl_Organize_Employee_dialogdivDeptDetail" %>
<%@ Register Src="~/UIControl/RevisionInfo.ascx" TagName="RevisionInfo" TagPrefix="vri" %>
<script>
    function viewDept(index) {
        var row = $('#tbEmp').datagrid('getData').rows[index];
        if (row) {
            try {
                $('#fmView').form('load', row);
                LoadTelInfo();
                $('#divDeptDetail').dialog('open');
            }
            catch (e) {
                $.messager.alert({
                    title: '<%=Language("ExceptionTips")%>', //ExceptionTips 異常提示
                    msg: '<%=Language("SubmitDataException")%>'//SubmitDataException 提交數據異常，請確認網絡連接狀況
                });
            }
        }
    }
</script>
<div id="divDeptDetail" class="easyui-window" data-options="closed:true,modal:true,title:'<%=Language("emp_viewDetail")%>'"
    style="width: 610px; height: 290px; padding: 3px;">
    <form id="fmView" method="get">
        <div class="easyui-tabs" style="margin: auto; margin-top: 5px; height: 180px;" fit="false">
            <div title="<%=Language("BasicInfoTab")%>" style="overflow: auto; padding: 5px; text-align: left;">
                <table cellpadding="0" cellspacing="0" class="tbView">
                    <tr>
                        <td bgcolor="#e8f5ff" style="width: 100px;">
                            <%=Language("emp_id")%>：
                        </td>
                        <td>
                            <input readonly name="empid" style="width: 100px;" />
                        </td>
                        <td bgcolor="#e8f5ff" style="width: 100px;">
                            <%=Language("emp_name")%>：
                        </td>
                        <td>
                            <input readonly name="empname" style="width: 100px;" />
                        </td>
                        <td bgcolor="#e8f5ff" style="width: 100px;">
                            <%=Language("emp_dept")%>：
                        </td>
                        <td>
                            <input readonly name="empdept" style="width: 100px;" />
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#e8f5ff" style="width: 100px;">
                            <%=Language("emp_extTel")%>：
                        </td>
                        <td>
                            <input readonly name="exttelno" style="width: 100px;" />
                        </td>
                        <td bgcolor="#e8f5ff" style="width: 100px;">
                            <%=Language("emp_mail")%>：
                        </td>
                        <td>
                            <input readonly name="empmail" style="width: 100px;" />
                        </td>
                        <td bgcolor="#e8f5ff" style="width: 100px;">
                            <%=Language("emp_sigerID")%>：
                        </td>
                        <td>
                            <input readonly name="signerid" style="width: 100px;" />
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#e8f5ff" style="width: 100px;">
                            <%=Language("Usy")%>：
                        </td>
                        <td colspan="5">
                            <input readonly name="usy" style="width: 100px;" />
                        </td>
                    </tr>
                </table>
            </div>
            <div title="<%=Language("ReviseInfoTab")%>" data-options="closable:false" style="overflow: auto; padding: 5px; text-align: left;">
                <vri:RevisionInfo ID="vri1" runat="server" />
            </div>
        </div>
    </form>
</div>
