<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RevisionInfo.ascx.cs"
    Inherits="UIControl_RevisionInfo" %>
<script type="text/javascript">
    function LoadTelInfo() {
        $.post('../../ASHX/Organize/EmpManage.ashx?M=loademptel', { 'Create': $("#vcreateuserid").val(), 'Update': $("#vupdateuserid").val() },
                function (result) {
                    //加載所有公司别的简称
                    for (var i = 0; i < result.length; i++) {
                        if (result[i].userid == $("#vcreateuserid").val()) {
                            $("#vcreateTel").val(result[i].tel)
                        }
                        if (result[i].userid == $("#vupdateuserid").val()) {
                            $("#vlastmodTel").val(result[i].tel)
                        }
                    }
                },
                'json');
    }
</script>
<table style="width: 100%" class="tbView">
    <tr>
        <td bgcolor="#e8f5ff">
            <%=Language("CreateUser")%>：
        </td>
        <td>
            <input readonly name="createrid" id="vcreateuserid" />
        </td>
        <td bgcolor="#e8f5ff">
            <%=Language("LastUser")%>：
        </td>
        <td>
            <input readonly name="updaterid" id="vupdateuserid" />
        </td>
    </tr>
    <tr>
        <td bgcolor="#e8f5ff">
            <%=Language("CreaterDept")%>：
        </td>
        <td>
            <input readonly name="cdeptid" />
        </td>
        <td bgcolor="#e8f5ff">
            <%=Language("LastDept")%>：
        </td>
        <td>
            <input readonly name="udeptid" />
        </td>
    </tr>
    <tr>
        <td bgcolor="#e8f5ff">
            <%=Language("CreateTime")%>：
        </td>
        <td>
            <input readonly name="createtime" />
        </td>
        <td bgcolor="#e8f5ff">
            <%=Language("LastTime")%>：
        </td>
        <td>
            <input readonly name="lastmodtime" />
        </td>
    </tr>
    <tr>
        <td bgcolor="#e8f5ff">
            <%=Language("createTel")%>：
        </td>
        <td>
            <input readonly name="createTel" id="vcreateTel" />
        </td>
        <td bgcolor="#e8f5ff">
            <%=Language("lastmodTel")%>：
        </td>
        <td>
            <input readonly name="lastmodTel" id="vlastmodTel"/>
        </td>
    </tr>
</table>
