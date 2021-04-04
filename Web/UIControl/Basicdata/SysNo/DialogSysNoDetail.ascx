<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DialogSysNoDetail.ascx.cs"
    Inherits="UIControl_Basicdata_SysNo_DialogSysNoDetail" %>
<%@ Register Src="~/UIControl/RevisionInfo.ascx" TagName="RevisionInfo" TagPrefix="vri" %>
<script type="text/javascript">
    //查看
    function view(index) {
        var row = $('#tbSysNo').datagrid('getData').rows[index];
        if (row) {
            try {
                $('#fmview').form('load', row);
                LoadTelInfo();
                $('#ViewSysNo').dialog('open').dialog('setTitle', '<%=Language("RoseDetailTitle")%>');
            }
            catch (e) {
                $.messager.alert({
                    //異常提示
                    title: '<%=Language("ExceptionTips")%>',
                    //提交數據異常，請確認網絡連接狀況
                    msg: '<%=Language("SubmitDataException")%>'
                });
            }
        }
    }
</script>
<div id="ViewSysNo" class="easyui-dialog" data-options="closed:true,modal:true,"
    style="width: 770px; height: 300px; padding: 2px;" maximizable="true">
    <div class="easyui-layout" fit="true">
        <form id="fmview" method="post">
        <div region="north" border="false" style="height: 1px;" class="p-search">
        </div>
        <div region="center" border="false">
            <div class="easyui-tabs" style="margin: auto; margin-top: 5px; height: 220px;" border="true"
                fit="true">
                <div title="<%=Language("BasicInfoTab")%>" data-options="closable:false" style="overflow: auto;
                    padding: 5px;">
                    <table style="width: 100%" class="tbView">
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("CompanyId")%>:
                            </td>
                            <td>
                                <input readonly name="companyid" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("ModuleType")%>:
                            </td>
                            <td>
                                <input readonly name="moduletype" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("ModularType")%>:
                            </td>
                            <td>
                                <input readonly name="modulartype" />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("comp_type")%>:
                            </td>
                            <td>
                                <input readonly name="category" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("receipttype")%>:
                            </td>
                            <td>
                                <input readonly name="receipttype" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("keyword")%>:
                            </td>
                            <td>
                                <input readonly name="keyword" />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("datetype")%>:
                            </td>
                            <td>
                                <input readonly name="datetype" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("codelen")%>:
                            </td>
                            <td>
                                <input readonly name="codelen" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("Usy")%>:
                            </td>
                            <td>
                                <input name="Usy" readonly />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("Mark")%>:
                            </td>
                            <td colspan="5">
                                <input readonly name="mark" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div title="<%=Language("ReviseInfoTab")%>" data-options="closable:false" style="overflow: auto;
                    padding: 5px; text-align: left;">
                    <vri:RevisionInfo ID="vri1" runat="server" />
                </div>
            </div>
        </div>
        </form>
    </div>
</div>
