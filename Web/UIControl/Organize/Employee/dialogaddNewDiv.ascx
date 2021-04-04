<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dialogaddNewDiv.ascx.cs"
    Inherits="UIControl_Organize_Employee_dialogaddNewDiv" %>
<script>
    function editEmp(index) {
        url = '../../ASHX/Organize/EmpManage.ashx?M=update';
        var row = $('#tbEmp').datagrid('getData').rows[index];
        $("#emp_id").attr("readOnly", "readOnly");
        //$("#emp_id").css("background-color", "#e1e1e1");
        if (row) {
            $('#fm').form('load', row);
            $('#addNewDiv').dialog('open');
        }
    }
    function addEmp() {
        $('#addNewDiv').dialog({ title: '<%=Language("sbmi003")%>' });
        $('#addNewDiv').dialog('open');
        $('#fm').form('clear');
        $("#txtDeptUsy").val('Y');
        $("#emp_id").removeAttr("readOnly");
        //$('#emp_id').removeAttr("style");
        url = '../../ASHX/Organize/EmpManage.ashx?M=add';
    }
    function saveEmp() {
        $('#fm').form('submit', {
            url: url,
            onSubmit: function () {
                var error = '';
                if ($.trim($('#emp_id').val()) == '') {
                    error = '<%=Language("empnoisempty")%>'; //empnoisempty 員工編號不能為空
                    $("#emp_id").focus();
                }
                else if (!$('#emp_id').val().match(/^\w+$/)) {
                    error = '<%=Language("empformaterr")%>'; //empformaterr 員工編號只能是英文，數字或者下劃綫
                    $("#emp_id").focus();
                }
                else if ($.trim($('#emp_name').val()) == '') {
                    error = '<%=Language("empnameisempty")%>'; //empnameisempty 員工姓名不能為空
                        $("#emp_name").focus();
                    }
                if (error != '') {
                    $.messager.alert('<%=Language("errTips")%>', error);
                    return false;
                }
                return $(this).form('validate');
            },
            success: function (result) {
                try {
                    var result = eval('(' + result + ')');
                    if (result.success) {
                        $.messager.defaults = { ok: '<%=Language("yes")%>' };
                        $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("DataSaveSuccess")%>'); //SuccessTips 成功提示 DataSaveSuccess 數據已保存成功
                        $('#addNewDiv').dialog('close'); 	// close the dialog
                        $('#tbEmp').datagrid('reload'); // reload the user data
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
<div id="addNewDiv" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:false,buttons:'#dlg-buttons'"
    style="width: 610px; padding: 3px; height: 230px;">
    <form id="fm" method="post" novalidate>
        <table border="0" style="width: 100%;">
            <tr>
                <td style="text-align: right;">
                    <%=Language("emp_id")%>：
                </td>
                <td>
                    <input id="emp_id" name="empid" style="width: 150px;" class="easyui-textbox" data-options="required:true" />
                </td>
                <td style="text-align: right;">
                    <%=Language("emp_name")%>：
                </td>
                <td>
                    <input id="emp_name" name="empname" style="width: 150px;" class="easyui-textbox" data-options="required:true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <%=Language("emp_dept")%>：
                </td>
                <td>
                    <input class="easyui-textbox" style="width: 150px;" name="empdept" />
                </td>
                <td style="text-align: right;">
                    <%=Language("emp_extTel")%>：
                </td>
                <td>
                    <input class="easyui-textbox" style="width: 150px;" name="exttelno" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <%=Language("emp_sigerID")%>：
                </td>
                <td>
                    <input class="easyui-textbox" style="width: 150px;" name="signerid" />
                </td>
                <td style="text-align: right;">
                    <%=Language("emp_mail")%>：
                </td>
                <td>
                    <input class="easyui-textbox" style="width: 150px;" name="empmail" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <%=Language("Usy")%>：
                </td>
                <td colspan="3">
                    <select class="easyui-combobox" style="width: 150px; font-size: 12px;" name="usy">
                        <option value="Y">Y</option>
                        <option value="N">N</option>
                    </select>
                </td>
            </tr>
        </table>
    </form>
    <div id="dlg-buttons">
        <a class="easyui-linkbutton" data-options="text:'保存'" href="javascript:void(0)" onclick="saveEmp()">
            <%=Language("Save")%></a> <a class="easyui-linkbutton" data-options="text:'取消'" href="javascript:void(0)"
                onclick="closeAddWindow('addNewDiv')">
                <%=Language("Cancel")%></a>
    </div>
</div>
