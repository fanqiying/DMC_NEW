<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="divDataClose.ascx.cs"
    Inherits="UIControl_ShutDown_divDataClose" %>
<script type="text/javascript">

    function DataColse() {
        var startTime = $("#txtDataColseStart").datebox('getValue');
        var endTime = $("#txtDataColseEnd").datebox('getValue');
        var soms = $("#txtDataCloseCause").val();
        var compid = $("#compyid").val();

        if (startTime == '') {

            alert('<%=Language("closeDataStime")%>');
            $("#txtDataColseStart").focus();
            return;
        }
        else if (endTime == '') {
            alert('<%=Language("closeDataEndTime")%>');
            $("#txtDataColseEnd").focus();
            return;
        }

        else if (soms == '') {
            alert('<%=Language("closeDataCause")%>');
            $("#txtDataCloseCause").focus();
            return;
        }


        $.post('../../ASHX/SystemManage/StopSysManege.ashx', { M: "DataColse", st: startTime, et: endTime, reson: soms, cid: compid },
                                 function (result) {
                                     try {
                                         if (result.success) {
                                             $.messager.defaults = { ok: '<%=Language("comform")%>' };
                                             $('#divDataClose').dialog('close');
                                             $('#divDataClose').form('clear');
                                             $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("DataSaveSuccess")%>');
                                             test();
                                         } else {
                                             $.messager.alert({	// show error message
                                                 title: 'Error',
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

    function passCompanyID(pid) {
        $("#compyid").val(pid);

    }

    function test() {
        $('#tbDataColse').datagrid('options').url = "../../ASHX/SystemManage/StopSysManege.ashx?M=LoadData";
        var queryParams = $('#tbDataColse').datagrid('options').queryParams;
        queryParams.cpid = $("#compyid").val(),
            $('#tbDataColse').datagrid('load');
    }
</script>
<div id="divDataClose" class="easyui-window" data-options="closed:true,modal:true"
    style="width: 450px; height: 240px; padding: 5px;">
    <div data-options="region:'center',border:false" style="padding: 10px; background: #fff;
        border: 1px solid #ccc;">
        <table cellpadding="0" cellspacing="0" class="addCoyTB ">
            <tr>
                <td>
                    <%=Language("stopTime")%>:
                </td>
                <td>
                    <input type="text" id="txtDataColseStart" class="easyui-datetimebox" />~<input type="text"
                        id="txtDataColseEnd" class="easyui-datetimebox" />
                </td>
            </tr>
            <tr>
                <td>
                    <%=Language("reasons")%>:
                </td>
                <td>
                    <asp:TextBox ID="txtDataCloseCause" required="true" runat="server" TextMode="MultiLine"
                        Height="50px" Width="210px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div data-options="region:'south',border:false" style="text-align: right; padding: 5px;">
        <a class="easyui-linkbutton" href="javascript:void(0)" onclick="DataColse()">
            <%=Language("Save")%></a> <a class="easyui-linkbutton" href="javascript:void(0)"
                onclick="closeAddWindow('divDataClose')">
                <%=Language("Cancel")%></a>
    </div>
</div>
