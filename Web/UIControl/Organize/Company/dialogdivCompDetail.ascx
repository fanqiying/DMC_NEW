<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dialogdivCompDetail.ascx.cs"
    Inherits="UIControl_Organize_Company_dialogdivCompDetail" %>
<%@ Register Src="~/UIControl/RevisionInfo.ascx" TagName="RevisionInfo" TagPrefix="vri" %>
<script>
    function viewComp(index) {
        var row = $('#tbComp').datagrid('getData').rows[index];
        if (row) {
            try {
                $('#fmView').form('load', row);
                var data = $('#compLanguageid').combobox('getData');
                for (var k = 0; k < data.length; k++)   //給查看語言賦值
                {
                    if (data[k].languageid == row.complanguage) {
                        $("input[name='complanguage']").val(data[k].languagename);
                        break;
                    }
                }

                $("#vcompcategory").val(Compcategory(row.compcategory));
                LoadTelInfo();
                $('#divCompDetail').dialog('open');
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
<div id="divCompDetail" class="easyui-window" data-options="closed:true,modal:true,title:'<%=Language("comp_viewInfo")%>'"
    style="width: 710px; height: 400px; padding: 3px;" maximizable="true">
    <div class="easyui-layout" fit="true">
        <form id="fmView" method="get">
        <div region="north" border="false" style="height: 1px;" class="p-search">
        </div>
        <div region="center" border="false">
            <div class="easyui-tabs" style="margin: auto; margin-top: 5px; height:auto;" border="true" fit="true">
                <div title="<%=Language("BasicInfoTab")%>" style="overflow: auto; padding: 3px; text-align: left;">
                    <table id="Table1" cellpadding="0" cellspacing="0" border="0" class="tbView2">
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("comp_id")%>:
                            </td>
                            <td>
                                <input readonly name="companyid" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("comp_languageType")%>:
                            </td>
                            <td>
                                <input readonly name="complanguage" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("comp_type")%>:
                            </td>
                            <td>
                                <input readonly id="vcompcategory" name="compcategory" />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("comp_intername")%>:
                            </td>
                            <td colspan="3">
                                <input readonly name="intername" style="width: 340px;" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("comp_sinpleName")%>:
                            </td>
                            <td>
                                <input readonly name="simplename" />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("comp_outName")%>:
                            </td>
                            <td colspan="3">
                                <input readonly name="outername" style="width: 340px;" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("comp_no")%>:
                            </td>
                            <td>
                                <input readonly name="companyno" />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("comp_addrone")%>:
                            </td>
                            <td colspan="5">
                                <textarea readonly name="addrone" style="width: 300px"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("comp_addrtwo")%>:
                            </td>
                            <td colspan="5">
                                <textarea readonly name="addrtwo" style="width: 300px"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("comp_tel")%>:
                            </td>
                            <td>
                                <input readonly name="comptel" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("comp_fax")%>:
                            </td>
                            <td>
                                <input readonly name="compfax" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("comp_regno")%>:
                            </td>
                            <td>
                                <input readonly name="compregno" />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("comp_remark")%>:
                            </td>
                            <td colspan="3">
                                <input readonly name="remark" style="width: 300px;" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("Usy")%>:
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
