<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dialogaddNewDiv.ascx.cs"
    Inherits="UIControl_Organize_dialogaddNewDiv" %>
<script>
    function editComp(index) {
        $("#comp_id").attr("readonly", "readonly");
        $("#comp_id").css("background-color", "#e1e1e1");
        var row = $('#tbComp').datagrid('getData').rows[index];
        if (row) {
            try {
                $('#fm').form('load', row);
                $('#addNewDiv').dialog({
                    title: '<%=Language("sbmi001")%>'//sbmi001  公司基本資料維護作業
                });
                $('#addNewDiv').dialog('open');
                url = '../../ASHX/Organize/CompManage.ashx?M=update';
            }
            catch (e) {
                $.messager.alert({
                    title: '<%=Language("ExceptionTips")%>', //ExceptionTips 異常提示
                    msg: '<%=Language("SubmitDataException")%>'//SubmitDataException 提交數據異常，請確認網絡連接狀況
                });
            }


        }
    }
    function addComp() {
        try {
            $('#addNewDiv').dialog({
                title: '<%=Language("sbmi001")%>'//sbmi001  公司基本資料維護作業
            });

            $('#addNewDiv').dialog('open');
            $('#fm').form('clear');
            var data = $('#compLanguageid').combobox('getData');
            if (data.length > 0) {
                $("#compLanguageid").combobox("setValue", data[0].languageid);
            }
            $("#comp_id").removeAttr("readonly");
            $('#comp_id').removeAttr("style");
            $("#comp_usy").val('Y');
            $("#compcategoryid").val('2');
            $("#compLanguageid").val('tw');
            url = '../../ASHX/Organize/CompManage.ashx?M=add';
        }
        catch (e) {
            $.messager.alert({
                title: '<%=Language("ExceptionTips")%>', //ExceptionTips 異常提示
                msg: '<%=Language("SubmitDataException")%>'//SubmitDataException 提交數據異常，請確認網絡連接狀況
            });
        }
    }

    function saveComp() {
        $('#fm').form('submit', {
            url: url,
            onSubmit: function () {
                return $(this).form('validate');
            },
            success: function (result) {
                try {
                    var result = eval('(' + result + ')');
                    if (result.success) {
                        $.messager.defaults = { ok: '<%=Language("yes")%>' };
                        $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("DataSaveSuccess")%>'); //SuccessTips 成功提示 DataSaveSuccess 數據已保存成功
                        $('#addNewDiv').dialog('close'); 	// close the dialog                  
                        $('#tbComp').datagrid('reload'); // reload the user data
                    } else {
                        $.messager.alert({
                            title: '<%=Language("errTips")%>',
                            msg: result.msg
                        });
                    }
                }
                catch (e) {
                    $.messager.alert({
                        title: '<%=Language("ExceptionTips")%>', //ExceptionTips 異常提示
                        msg: '<%=Language("SubmitDataException")%>'//SubmitDataException 提交數據異常，請確認網絡連接狀況
                    });
                }
            }
        });
    }
</script>
<div id="addNewDiv" class="easyui-window" data-options="closed:true,modal:true,title:'<%=Language("comp_addTitle")%>',iconCls:'icon-save'"
    style="width: 730px; height: 455px; padding: 3px;" maximizable="true" buttons="#dlg-buttons-add">
    <form id="fm" method="post" novalidate>
    <table border="0" align="center" cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" class="tbADD2">
                    <tr>
                        <td>
                            <%=Language("comp_id")%>:
                        </td>
                        <td>
                            <input id="comp_id" name="companyid" type="text" class="easyui-validatebox" required="true"
                                style="width: 120px;" />
                        </td>
                        <td>
                            <%=Language("comp_languageType")%>:
                        </td>
                        <td>
                            <select id="compLanguageid" name="complanguage" style="width: 100px;" font-size="12px;">
                            </select>
                        </td>
                        <td>
                            <%=Language("comp_type")%>:
                        </td>
                        <td>
                            <select name="compcategory" id="compcategoryid" style="width: 100px; font-size: 12px;">
                                <option value="1">
                                    <%=Language("CompanyGroup")%></option>
                                <option value="2">
                                    <%=Language("company")%></option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Language("comp_intername")%>:
                        </td>
                        <td colspan="3">
                            <input id="Text3" name="intername" type="text" class="easyui-validatebox" required="true"
                                style="width: 350px;" />
                        </td>
                        <td>
                            <%=Language("comp_sinpleName")%>:
                        </td>
                        <td>
                            <input id="Text5" name="simplename" type="text" class="easyui-validatebox" required="true"
                                style="width: 120px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Language("comp_outName")%>:
                        </td>
                        <td colspan="3">
                            <input id="Text7" name="outername" type="text" style="width: 350px;" class="easyui-validatebox"
                                required="true" />
                        </td>
                        <td>
                            <%=Language("comp_no")%>:
                        </td>
                        <td>
                            <input id="Text8" name="companyno" type="text" style="width: 120px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Language("comp_addrone")%>:
                        </td>
                        <td colspan="5">
                            <input id="Text9" name="addrone" type="text" style="width: 560px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Language("comp_addrtwo")%>:
                        </td>
                        <td colspan="5">
                            <input id="Text10" name="addrtwo" type="text" style="width: 560px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Language("comp_tel")%>:
                        </td>
                        <td>
                            <input id="Text12" name="comptel" type="text" style="width: 120px;" />
                        </td>
                        <td>
                            <%=Language("comp_fax")%>:
                        </td>
                        <td>
                            <input id="Text13" name="compfax" type="text" style="width: 120px;" />
                        </td>
                        <td>
                            <%=Language("comp_regno")%>:
                        </td>
                        <td>
                            <input id="Text14" name="compregno" type="text" style="width: 120px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Language("comp_remark")%>:
                        </td>
                        <td colspan="3">
                            <input id="Text15" name="remark" type="text" style="width: 345px;" />
                        </td>
                        <td>
                            <%=Language("Usy")%>
                        </td>
                        <td>
                            <select style="width: 120px; font-size: 12px;" id="comp_usy" name="usy">
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
<div id="dlg-buttons-add">
    <a class="easyui-linkbutton" data-options="iconCls:'icon-ok'" href="javascript:void(0)"
        onclick="saveComp()">
        <%=Language("Save")%></a> <a class="easyui-linkbutton" data-options="text:'cancel'"
            href="javascript:void(0)" onclick="closeAddWindow('addNewDiv')">
            <%=Language("Cancel")%></a>
</div>
