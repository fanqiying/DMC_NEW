<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DialogStopSys.ascx.cs"
    Inherits="UIControl_ShutDown_DialogStopSys" %>
<script type="text/javascript">

    function shutDowm() {
        var startTime = $("#txtStopStart").datebox('getValue');
        var endTime = $("#txtStopEnd").datebox('getValue');
        var soms = document.getElementById("<%=txtStopResons.ClientID  %>").value;

        if (startTime == '') {
            alert('<%=Language("closeStartime")%>');
            $("#txtStopStart").focus();
            return;
        }
        else if (endTime == '') {
            alert('<%=Language("closeEndTime")%>');
            $("#txtStopEnd").focus();
            return;
        }

        else if (soms == '') {
            alert('<%=Language("msg_resons")%>');
            $("#txtStopResons").focus();
            return;
        }
        else if (startTime > endTime) {
            alert('<%=Language("errorTime")%>');
            return;
        }

        $.post('../../ASHX/SystemManage/StopSysManege.ashx', { M: "SysColse", st: startTime, et: endTime, reson: soms },
                                 function (result) {
                                     try {
                                         if (result.success) {
                                             $.messager.defaults = { ok: '<%=Language("comform")%>' };
                                             $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("DataSaveSuccess")%>');
                                             $('#divStop').dialog('close');
                                             $('#divStop').form('clear');
                                             $('#tbStopList').datagrid('reload');
                                         } else {
                                             $.messager.alert({	// show error message
                                                 title: '<%=Language("FailureTips")%>',
                                                 msg: result.msg
                                             });
                                         }
                                     }
                                     catch (e) {
                                         $.messager.alert({
                                             title: '<%=Language("ExceptionTips")%>',
                                             msg: '<%=Language("SubmitDataException")%>'
                                         });

                                     }
                                 }, 'json').error(function () {
                                     $.messager.alert({
                                         title: '<%=Language("ExceptionTips")%>',
                                         msg: '<%=Language("SubmitDataException")%>'
                                     });
                                 });
    }

</script>
<div id="divStop" class="easyui-window" data-options="closed:true,modal:true" style="width: 470px;
    height: 240px; padding: 5px;">
    <div data-options="region:'center',border:false" style="padding: 10px; background: #fff;
        border: 1px solid #ccc;">
        <table cellpadding="0" cellspacing="0" class="addCoyTB ">
            <tr>
                <td>
                    <%=Language("shuptdownTime")%>:
                </td>
                <td>
                    <input type="text" id="txtStopStart" class="easyui-datetimebox" />~<input type="text"
                        id="txtStopEnd" class="easyui-datetimebox" />
                </td>
            </tr>
            <tr>
                <td>
                    <%=Language("stopResons")%>:
                </td>
                <td>
                    <asp:TextBox ID="txtStopResons" runat="server" TextMode="MultiLine" Height="50px"
                        Width="210px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div data-options="region:'south',border:false" style="text-align: right; padding: 5px;">
        <a class="easyui-linkbutton" data-options="text:'<%=Language("comform")%>'" href="javascript:void(0)"
            onclick="shutDowm();">
            <%=Language("Save")%></a> <a class="easyui-linkbutton" data-options="text:'<%=Language("Cancel")%>'"
                href="javascript:void(0)" onclick="closeAddWindow('divStop')">
                <%=Language("Cancel")%></a>
    </div>
</div>
