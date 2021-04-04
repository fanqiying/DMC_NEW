<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dialogdivRose.ascx.cs"
    Inherits="UIControl_Permission_PerTypeManage_dialogdivRose" %>
<script>
    function addRose() {
        $('#divRose').dialog('open').dialog('setTitle', '<%=Language("AddRoseTitle")%>');
        $('#fm').form('clear');
        $('#RoseIdInfo').removeAttr("readonly");
        $('#drpUsy').val('Y');
        $('#drpSystemType').val('01');
        url = '../../ASHX/Permission/RoseManage.ashx?M=add';
    }
    function editRose(index) {
        var row = $('#tbRose').datagrid('getData').rows[index];
        if (row) {
            $('#divRose').dialog('open').dialog('setTitle', '<%=Language("EditRoseTitle")%>');
            $('#fm').form('load', row);
            $('#RoseIdInfo').attr("readonly", "readonly");
            url = '../../ASHX/Permission/RoseManage.ashx?M=update';
        }
    }

    function saveRose() {
        var error = '';
        var errorid = '';
        var pattern = new RegExp("[`~!$^{}\\[\\]/?\"\'‘']");
        if ($('#RoseIdInfo').val().match(/^[\d\w]+$/) == null) {
            error = '<%=Language("permnoformaterr")%>'; //permnoformaterr 請輸入權限類別編號，且只能為英文和數字！
            errorid = 'RoseIdInfo';
        }
        else if ($.trim($('#RoseNameInfo').val()) == '') {
            error = '<%=Language("pernameisempty")%>'; //pernameisempty 權限類別名稱不能為空！
            errorid = 'RoseNameInfo';
        }
        else if ($('#RoseNameInfo').val().match(pattern) != null) {
            error = '<%=Language("pernamehavestr")%>'; //pernamehavestr 權限類別名稱不能包含特殊字符！
            errorid = 'RoseNameInfo';
        }

        if (error != '') {
            $.messager.alert('<%=Language("errTips")%>', error, 'warning', function () { $("#" + errorid).focus(); });
            return false;
        }

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
                        $('#divRose').dialog('close'); 	// close the dialog
                        $('#tbRose').datagrid('reload'); // reload the user data
                    } else {
                        $.messager.alert('<%=Language("errTips")%>', result.msg, 'warning',
                            function () {
                                if (result.msg.indexOf('<%=Language("Exists")%>') > -1) {//Exists
                                    $("#RoseIdInfo").focus();
                                } else {
                                    $("#RoseNameInfo").focus();
                                }
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
<div id="divRose" class="easyui-dialog" data-options="closed:true,modal:true" style="width: 450px;
    height: 220px; padding: 5px;" buttons="#dlg-buttons" maximizable="true">
    <form id="fm" method="post" novalidate>
    <table align="center" cellpadding="0" cellspacing="0" class="addCoyTB" width="90%">
        <tr>
            <td style="width: 30%;">
                <%=Language("RoseId")%>:
            </td>
            <td style="width: 70%;">
                <input style="width: 150px" id="RoseIdInfo" name="RoseId" onkeyup="javascript:this.value=this.value.toLocaleUpperCase();">
            </td>
        </tr>
        <tr>
            <td>
                <%=Language("RoseName")%>:
            </td>
            <td>
                <input style="width: 150px" id="RoseNameInfo" name="RoseName">
            </td>
        </tr>
        <tr>
            <td>
                <%=Language("SystemType")%>:
            </td>
            <td>
                <select style="width: 150px" id="drpSystemType" name="SystemType">
                    <option value="01">
                        <%=Language("systemuser")%></option>
                    <option value="02">
                        <%=Language("supplyid")%></option>
                    <option value="03">
                        <%=Language("customername")%></option>
                </select>
            </td>
        </tr>
        <tr>
            <td>
                <%=Language("Usy")%>:
            </td>
            <td>
                <select style="width: 150px" id="drpUsy" name="Usy">
                    <option value="Y">Y</option>
                    <option value="N">N</option>
                </select>
            </td>
        </tr>
    </table>
    </form>
</div>
