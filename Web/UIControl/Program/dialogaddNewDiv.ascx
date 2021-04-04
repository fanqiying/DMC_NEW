<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dialogaddNewDiv.ascx.cs"
    Inherits="UIControl_Program_dialogaddNewDiv" %>
<script type="text/javascript">
    function addProgram(index) {
        try {
            actionIds = '';
            historymenuId = '';
            $('#addNewDiv').dialog({
                title: '<%=Language("AddProgrameTask")%>'//AddProgrameTask 新增程式資料建立作業
            });
            $('#addNewDiv').dialog('open');
            $('#fm').form('clear');
            LoadFuncList();
            $('#program_id').removeAttr("readonly");
            $('#program_id').removeAttr("style");
            $("#prog_usy").val('Y');
            url = '../../ASHX/Permission/ProgManage.ashx?M=add';
        }
        catch (e) {
            $.messager.alert({
                title: '<%=Language("ExceptionTips")%>', //ExceptionTips 異常提示
                msg: '<%=Language("DataException")%>'//DataException 數據異常，請確認網絡連接狀況

            });
        }
    }
    function editProgram(index) {
        actionIds = '';
        historymenuId = '';
        var row = $('#tbProgram').datagrid('getData').rows[index];
        $("#program_id").attr("readOnly", "readOnly");
        $("#program_id").css("background-color", "#e1e1e1");
        if (row) {
            try {
                $('#tbProgram').attr("readonly", "readonly");
                $('#fm').form('load', row);
                LoadFuncList();
                historymenuId = row.menuid;
                $("#program_menuid").val(row.menuid);
                $('#addNewDiv').dialog({
                    title: '<%=Language("EditProgramTask")%>'//EditProgramTask 編輯程式資料建立作業
                });
                $('#addNewDiv').dialog('open');
                url = '../../ASHX/Permission/ProgManage.ashx?M=update';
            }
            catch (e) {
                $.messager.alert({
                    title: '<%=Language("ExceptionTips")%>', //ExceptionTips 異常提示
                    msg: '<%=Language("IntenetException")%>'//IntenetException 網絡異常，請確認網絡連接狀況
                });
            }
        }
    }
    function saveProgram() {
        try {
            var s = document.getElementById("program_menuid").value;
            actionIds = GetCheckId();
            actionNames = GetCheckName();

            $('#fm').form('submit', {
                url: url + '&ids=' + actionIds + '&names=' + actionNames + '&menu_id=' + s,
                onSubmit: function () {
                    var error = '';
                    if ($.trim($('#program_id').val()) == '') {
                        error = '<%=Language("IsEmptyProgramID")%>'; //IsEmptyProgramID 程式編號不能為空
                        $("#program_id").focus();
                    }
                    else if (!$('#program_id').val().match(/^\w+$/)) {
                        error = '<%=Language("ProgramIDRule")%>'; //ProgramIDRule 程式編號只能是英文，數字或者下劃綫！
                        $("#program_id").focus();

                    }
                    else if ($.trim($('#program_name').val()) == '') {
                        error = '<%=Language("IsEmptyProgramName")%>'; //IsEmptyProgramName 程式名稱不能為空
                        $("#program_name").focus();
                    }
                    else if ($.trim($('#txtProgramNameStr').val()) == '') {
                        error = '<%=Language("IsEmptyProgramFuncti")%>'; //IsEmptyProgramFuncti 程式的基本功能不能為空
                    }
                    else if ($("#ismobile").is(":checked") && $.trim($('#mobileurl').val()) == '') {
                        //判斷是否勾選支持移動化，支持則移動化地址不能為空
                        error = "請輸入移動化url地址";
                    }

                    if (error != '') {
                        $.messager.alert('<%=Language("Tips")%>', error); //Tips 溫馨提示
                        return false;
                    }

                    return $(this).form('validate');
                },
                success: function (result) {
                    try {
                        var result = eval('(' + result + ')');
                        if (result.success) {
                            $.messager.defaults = { ok: '<%=Language("comform")%>' }; //comform 確定
                            $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("DataSaveSuccess")%>'); //SuccessTips 成功提示 DataSaveSuccess 數據已保存成功
                            $('#addNewDiv').dialog('close'); 	// close the dialog
                            $('#tbProgram').datagrid('reload'); // reload the user data
                        } else {
                            $.messager.alert({
                                title: 'Error',
                                msg: result.msg
                            });
                        }
                    }
                    catch (e) {
                        debugger;
                        $.messager.alert({
                            title: '<%=Language("ExceptionTips")%>',
                            msg: '<%=Language("DataException")%>'
                        });
                    }
                }
            });
        }
        catch (e) {
            $.messager.alert({
                title: '<%=Language("ExceptionTips")%>',
                msg: '<%=Language("IntenetException")%>'

            });
        }
    }
</script>
<div id="addNewDiv" class="easyui-window" data-options="closed:true,modal:true,title:'<%=Language("addnewrecord")%>',iconCls:'icon-save'"
    style="width: 600px; height: 310px; padding: 3px;" maximizable="true">
    <div class="easyui-layout" data-options="fit:true">
        <form id="fm" method="post" novalidate>
        <div data-options="region:'center',border:false" style="background: #fff;">
            <table class="addCoyTB" width="100%">
                <tr>
                    <td>
                        <%=Language("ProgramId")%>:
                    </td>
                    <td>
                        <input id="program_id" name="programid" type="text" /><input type="checkbox" name="ismobile"
                            id="ismobile" value="Y" /><label for="ismobile"><%=Language("ismobile")%></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Language("ProgramName")%>:
                    </td>
                    <td>
                        <input id="program_name" name="programname" type="text" style="width: 350px;" />
                        <input type="hidden" id="program_menuid" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Language("ProgramUrl")%>:
                    </td>
                    <td>
                        <input id="menuurl" name="menuurl" type="text" style="width: 350px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Language("menumurl")%>:
                    </td>
                    <td>
                        <input id="mobileurl" name="mobileurl" type="text" style="width: 350px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Language("program_func")%>:
                    </td>
                    <td>
                        <input type="text" readonly="readonly" id="txtProgramNameStr" name="functionstr"
                            style="width: 350px;" />
                        <label>
                            <img src="../../images/orgAdd.gif" /></label>
                        <label>
                            <a href="javascript:void(0)" onclick="addFuntion();">
                                <%=Language("Add")%></a></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Language("orderid")%>:
                    </td>
                    <td>
                        <input id="orderid" name="orderid" type="text" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Language("Usy")%>:
                    </td>
                    <td>
                        <select style="width: 100px; font-size: 12px;" name="usy" id="prog_usy">
                            <option value="Y">Y</option>
                            <option value="N">N</option>
                        </select>
                        <label>
                            <img src="../../images/orgAdd.gif" /></label>
                        <label>
                            <a href="javascript:void(0)" onclick="addToMenu();">
                                <%=Language("program_tomenu")%></a></label>
                    </td>
                </tr>
            </table>
        </div>
        <div data-options="region:'south',border:false" style="text-align: right; padding: 5px 0;">
            <a class="easyui-linkbutton" data-options="iconCls:'icon-ok'" href="javascript:void(0)"
                onclick="saveProgram()">
                <%=Language("Save")%></a> <a class="easyui-linkbutton" data-options="text:'cancel'"
                    href="javascript:void(0)" onclick="closeAddWindow('addNewDiv')">
                    <%=Language("Cancel")%></a>
        </div>
        </form>
    </div>
</div>
