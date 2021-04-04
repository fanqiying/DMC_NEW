<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DialogDetailDiv.ascx.cs"
    Inherits="UIControl_Organize_Dept_DialogDetailDiv" %>
<%@ Register Src="~/UIControl/RevisionInfo.ascx" TagName="RevisionInfo" TagPrefix="vri" %>
<div id="divDeptDetail" class="easyui-window" data-options="closed:true,modal:true,title:'<%=Language("dept_viewDetail")%>'"
    style="width: 600px; height: 250px; padding: 3px;" maximizable="true">
    <div class="easyui-layout" fit="true">
        <form id="fmView" method="get">
        <div region="north" border="false" style="height: 1px;" class="p-search">
        </div>
        <div region="center" border="false">
            <div class="easyui-tabs" border="true" fit="true" style="margin: auto; margin-top: 5px;
                height: auto;">
                <div title="<%=Language("BasicInfoTab")%>" style="overflow: auto; padding: 3px; text-align: left;">
                    <table style="width: 100%" class="tbView">
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("dept_id")%>:
                            </td>
                            <td>
                                <input readonly name="deptid" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("dept_sname")%>:
                            </td>
                            <td>
                                <input readonly name="simplename" />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("dept_fname")%>:
                            </td>
                            <td>
                                <input readonly name="fullname" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("dept_dead")%>:
                            </td>
                            <td>
                                <input readonly name="deptheader" />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("dept_sex")%>:
                            </td>
                            <td>
                                <input readonly id="vdeptnature" name="deptnature" />
                            </td>
                            <td bgcolor="#e8f5ff">
                                <%=Language("dept_group")%>:
                            </td>
                            <td>
                                <input readonly name="deptgroup" />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#e8f5ff">
                                <%=Language("Usy")%>:
                            </td>
                            <td>
                                <input readonly name="usy" />
                            </td>
                            <td>
                            </td>
                            <td>
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
