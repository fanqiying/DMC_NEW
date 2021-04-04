<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dialogdivProgDetail.ascx.cs"
    Inherits="UIControl_Program_dialogdivProgDetail" %>
<%@ Register Src="~/UIControl/RevisionInfo.ascx" TagName="RevisionInfo" TagPrefix="vri" %>
<script>

    function viewProgram(index) {

        var row = $('#tbProgram').datagrid('getData').rows[index];
        if (row) {
            try {
                $('#fmView').form('load', row);
                LoadTelInfo();
                $('#divProgDetail').dialog('open');
            }
            catch (e) {
                $.messager.alert({
                    title: '<%=Language("ExceptionTips")%>', //ExceptionTips 異常提示
                    msg: '<%=Language("IntenetException")%>'//IntenetException 網絡異常，請確認網絡連接狀況
                });
            }
        }
    }

</script>
<div id="divProgDetail" class="easyui-window" data-options="closed:true,modal:true,title:'<%=Language("program_viewInfo")%>'"
    style="width: 628px; height: 260px; padding: 3px;" maximizable="true">
    <div class="easyui-layout" fit="true">
        <form id="fmView" method="get">
        <div region="north" border="false" style="height: 1px;" class="p-search">
        </div>
        <div region="center" border="false">
            <div class="easyui-tabs" border="true" style="margin: auto; margin-top: 5px; height: auto;"
                fit="true">
                <div title="<%=Language("BasicInfoTab")%>" style="overflow: auto; padding: 3px; text-align: left;">
                    <table cellpadding="0" cellspacing="0" class="tbView">
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("ProgramId")%>：
                            </td>
                            <td>
                                <input readonly name="programid" />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("ProgramName")%>：
                            </td>
                            <td>
                                <input readonly name="programname" />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("ProgramUrl")%>:
                            </td>
                            <td>
                                <input readonly name="menuurl" />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("program_func")%>：
                            </td>
                            <td>
                                <textarea name="functionstr" readonly style="width: 100%; border: 0">
                           </textarea>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=Language("orderid")%>:
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
