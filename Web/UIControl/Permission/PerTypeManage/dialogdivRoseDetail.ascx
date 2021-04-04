<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dialogdivRoseDetail.ascx.cs"
    Inherits="UIControl_Permission_PerTypeManage_dialogdivRoseDetail" %>
<%@ Register Src="~/UIControl/RevisionInfo.ascx" TagName="RevisionInfo" TagPrefix="vri" %>
<script>
    function viewRose(index) {
        var row = $('#tbRose').datagrid('getData').rows[index];
        if (row) {
            try {
                $('#divRoseDetail').dialog('open').dialog('setTitle', '<%=Language("RoseDetailTitle")%>');
                $('#fmView').form('load', row);
                LoadTelInfo();
                $('#vSystemType').val(ReadType(row.SystemType));
                var queryParams = $('#tbUser').datagrid('options').queryParams;
                queryParams.RoseId = row.RoseId;
                $('#tbUser').datagrid('reload');
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
<div id="divRoseDetail" class="easyui-dialog" data-options="closed:true,modal:true"
    style="width: 600px; height: 350px; padding: 5px;" maximizable="true">
    <div class="easyui-layout" fit="true">
        <form id="fmView" method="get">
        <div region="north" border="false" style="height: 1px;" class="p-search">
        </div>
        <div region="center" border="false">
            <div class="easyui-tabs" style="margin: auto; margin-top: 5px; height: 220px;" border="true"
                fit="true">
                <div title="<%=Language("BasicInfoTab")%>" style="overflow: auto; padding: 5px; text-align: left;">
                    <table style="width: 100%" class="tbView">
                        <tr>
                            <td style="width: 20%;" height="20" bgcolor="#e8f5ff">
                                <%=Language("RoseId")%>:
                            </td>
                            <td style="width: 25%;">
                                <div align="left">
                                    <input name="RoseId" readonly />
                                </div>
                            </td>
                            <td style="width: 20%;" bgcolor="#e8f5ff">
                                <%=Language("RoseName")%>:
                            </td>
                            <td style="width: 30%;">
                                <div align="left">
                                    <input name="RoseName" readonly />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; width: 10%;" bgcolor="#e8f5ff">
                                <%=Language("SystemType")%>:
                            </td>
                            <td>
                                <div align="left">
                                    <input id="vSystemType" name="SystemType" readonly />
                                </div>
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("Usy")%>:
                            </td>
                            <td>
                                <div align="left">
                                    <input name="Usy" readonly />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div title="<%=Language("WarrantUserTab")%>" style="overflow: auto; padding: 5px;
                    text-align: left;">
                    <table width="98%" id="tbUser" fit="true">
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
