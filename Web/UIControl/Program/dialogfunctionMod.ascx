<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dialogfunctionMod.ascx.cs"
    Inherits="UIControl_Program_dialogfunctionMod" %>
<script>
    function addFuntion() {
        initFunctionList = [];
        $('#functionMod').dialog('open');
        var rows = $('#progFutionList').datagrid('getChecked');
        for (var i = 0; i < rows.length; i++) {
            //每行ID放入數組中
            initFunctionList.push(rows[i].ActionId);
        }
    }
    function saveFuntion() {
        actionNames = GetCheckName();
        if (actionNames.toString() != '') {

            $("#txtProgramNameStr").val(actionNames.toString());
            actionIds = GetCheckId();
            actionNames = GetCheckName();
            $('#functionMod').dialog('close');
        }
        else {
            alert('<%=Language("noFunc_msg")%>');
        }
    }
    function cancelFunction() {
        $('#functionMod').dialog('close');
        var rows = $('#progFutionList').datagrid('getData').rows;

        for (var i = 0; i < rows.length; i++)
            $('#progFutionList').datagrid('uncheckRow', i)

        if (initFunctionList != null && initFunctionList.length > 0) {
            for (var j = 0; j < initFunctionList.length; j++) {
                var index = $('#progFutionList').datagrid('getRowIndex', initFunctionList[j]);
                $('#progFutionList').datagrid('checkRow', index)
            }
        }
    }
</script>
<div id="functionMod" class="easyui-dialog" data-options="closed:true,modal:true,title:'<%=Language("prog_funcTtle")%>'"
    style="width: 600px; height: 260px; padding: 5px;" buttons="#data-buttons">
    <table id="progFutionList" width="550px" fit="false">
    </table>
</div>
<div id="data-buttons">
    <a class="easyui-linkbutton" data-options="text:'<%=Language("Save")%>'" href="javascript:void(0)" onclick="saveFuntion()">
        <%=Language("Save")%></a> <a class="easyui-linkbutton" data-options="text:'<%=Language("Cancel")%>'" href="javascript:void(0)"
            onclick="cancelFunction()">
            <%=Language("Cancel")%></a>
</div>
