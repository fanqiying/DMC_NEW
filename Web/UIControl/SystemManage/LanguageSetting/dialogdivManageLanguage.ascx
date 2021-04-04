<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dialogdivManageLanguage.ascx.cs" Inherits="UIControl_SystemManage_LanguageSetting_dialogdivManageLanguage" %>
<script>
    function Generate() {
        $.post('../../ASHX/SystemManage/LanguageManage.ashx?M=generatebit', null,
            function (result) {
                try {
                    if (result.success) {
                        $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("LangPackageSuscess")%>');//SuccessTips 成功提示LangPackageSuscess 語言包生成成功
                        $('#divManageLanguage').dialog('close')
                    } else {
                        $.messager.alert('<%=Language("errTips")%>', result.msg);//errTips 錯誤提示
                    }
                }
                catch (e) {
                    $.messager.alert({
                        title: '<%=Language("ExceptionTips")%>',//ExceptionTips 異常提示
                        msg: '<%=Language("SubmitDataException")%>'//SubmitDataException 提交數據異常，請確認網絡連接狀況
                    });
                }
            }, 'json');
        }
        function OpenLanguageType() {
            $('#divManageLanguage').dialog('open').dialog('setTitle', '<%=Language("LanguageNewTitle")%>');
            LoadLanguageType();
        }
        function SaveLanguageType() {
            var results = '';
            var trList = $('tr[name="languagetypetr"]');
            for (var i = 0; i < trList.length; i++) {
                var no = trList[i].id.match(/[\d]+/);
                results += 'LanguageId:' + $('input[id="languageid' + no + '"]').val();
                results += ',LanguageName:' + $('input[id="languagename' + no + '"]').val();
                var chks = $('input[name="languageusy"]');
                for (var j = 0; j < chks.length; j++) {
                    if (chks[j].id == "languageusy" + no) {
                        results += ',Usy:' + chks[j].checked + ';';
                        break;
                    }
                }
            }
            $.post('../../ASHX/SystemManage/LanguageManage.ashx?M=savetype', { Languages: results },
            function (result) {
                try {
                    if (result.success) {
                        $.messager.defaults = { ok: '<%=Language("comform")%>' };//comform 確定
                        $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("langTypeStupSucess")%>');//成功提示 langTypeStupSucess語言類別已設置成功
                        $('#divManageLanguage').dialog('close')
                    } else {
                        $.messager.alert('<%=Language("errTips")%>', result.msg);//errTips 錯誤提示
                    }
                }
                catch (e) {
                    $.messager.alert({
                        title: '<%=Language("ExceptionTips")%>',//ExceptionTips 異常提示
                        msg: '<%=Language("SubmitDataException")%>'//SubmitDataException 提交數據異常，請確認網絡連接狀況
                    });
                }
            }, 'json');

        }
</script>
<div id="divManageLanguage" class="easyui-dialog" data-options="closed:true,modal:true"
        style="width: 600px; height: 270px; padding: 5px;" buttons="#dlg-buttons1" maximizable="true">
        <table style="width: 90%; border-collapse: collapse;" cellspacing="0" cellpadding="0"
            id="openLanguageType">
            <tr style="height: 24px" id="th">
                <td style="border-bottom: #aaccee 1px solid; border-left: #aaccee 1px solid; background: #f3faff;
                    border-top: #aaccee 1px solid; border-right: #aaccee 1px solid; text-align: center;"
                    width="30%">
                    <%=Language("LanguageId")%>
                </td>
                <td style="border-bottom: #aaccee 1px solid; border-left: #aaccee 1px solid; background: #f3faff;
                    border-top: #aaccee 1px solid; border-right: #aaccee 1px solid; text-align: center;"
                    width="30%">
                    <%=Language("LanguageName")%>
                </td>
                <td style="border-bottom: #aaccee 1px solid; border-left: #aaccee 1px solid; background: #f3faff;
                    border-top: #aaccee 1px solid; border-right: #aaccee 1px solid; text-align: center;"
                    width="20%">
                    <%=Language("Usy")%>
                </td>
                <td width="10%">
                    <a onclick="AddLanguageType()">
                        <img src="../../images/add.gif" /><%=Language("Adjunction")%></a>
                </td>
            </tr>
        </table>
    </div>