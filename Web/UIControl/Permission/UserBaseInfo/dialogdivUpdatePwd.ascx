<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dialogdivUpdatePwd.ascx.cs"
    Inherits="UIControl_Permission_UserBaseInfo_dialogdivUpdatePwd" %>
<script type="text/javascript">
    function ResetPwd(index) {
        var row = $('#tbUser').datagrid('getData').rows[index];
        if (row) {
            $.post('../../ASHX/Permission/UserRightManage.ashx?M=Reset',
                   { UB: row.userID },
                   function (result) {
                       try {
                           if (result.success) {
                               $("#newPwd").val(result.pwd);
                               $("#newError").val(result.msg);
                               $('#divUpdatePwd').dialog('open').dialog('setTitle', '<%=Language("ResetPwd")%>');
                           } else {
                               $.messager.alert('<%=Language("errTips")%>', result.msg);
                           }
                       }
                       catch (e) {
                           $.messager.alert({
                               title: '<%=Language("ExceptionTips")%>', //ExceptionTips 異常提示
                               msg: '<%=Language("SubmitDataException")%>'//SubmitDataException 提交數據異常，請確認網絡連接狀況
                           });
                       }
                   }, 'json');
        }
    }
</script>
<div id="divUpdatePwd" class="easyui-window" data-options="closed:true,modal:true,title:'<%=Language("ResetPwd")%>',iconCls:'icon-save'"
    style="width: 350px; height: 200px; padding: 5px;" buttons="#data-buttons3">
    <div data-options="region:'center',border:false" style="padding: 10px; background: #fff;
        border: 1px solid #ccc;">
        <%=Language("Reseted")%>：<input id="newPwd" readonly style="border: 0px; width: 150px;" />
        <br />
        <input id="newError" readonly style="color: Red; border: 0px; width: 100%; height: 30px;" />
    </div>
</div>
<div id="data-buttons3">
    <a class="easyui-linkbutton" href="javascript:void(0)" onclick="closePwd()">
        <%=Language("Close")%></a>
</div>
