<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DialogSetSysNo.ascx.cs"
    Inherits="UIControl_Basicdata_SysNo_DialogSetSysNo" %>
<script type="text/javascript">
    var suburl;
    //新增
    function addSystemNo() {
        try {
            $('#addSystemNo').dialog({
                //新增單據信息作業
                title: '<%=Language("addInvoiceInfoWork")%>'
            });
            $('#addcompany').combobox("enable");
            $('#addmoduletype').combobox("enable");
            $('#addmodulartype').combobox("enable");
            $('#addcategory').combobox("enable");
            $('#addreceipttype').removeAttr("readonly");
            $('#addkeyword').combobox("enable");
            $('#addSystemNo').dialog('open');
            $('#fmSystemNo').form('clear');
            $('#addusy').val('Y');
            suburl = '../../ASHX/Basic/SysNoManage.ashx?M=create';
        }
        catch (e) {
            $.messager.alert({
                //異常提示
                //提交數據異常，請確認網絡連接狀況
                title: '<%=Language("ExceptionTips")%>',
                msg: '<%=Language("SubmitDataException")%>'
            });
        }
    }
    //修改
    function modSystemNo(index) {
        var row = $('#tbSysNo').datagrid('getData').rows[index];
        try {
            $('#addSystemNo').dialog({
                //修改單據信息作業
                title: '<%=Language("editInvoiceInfoWork")%>'
            });
            $('#fmSystemNo').form('load', row);
            $('#addSystemNo').dialog('open');
            $('#addcompany').combobox("disable");
            $('#addmoduletype').combobox("disable");
            $('#addmodulartype').combobox("disable");
            $('#addcategory').combobox("disable");
            $('#addreceipttype').attr("readonly","readonly");
            $('#addkeyword').combobox("disable");
            var parakey = "&category=" + row.category;
            parakey += "&companyid=" + row.companyid;
            parakey += "&keyword=" + row.keyword;
            parakey += "&modulartype=" + row.modulartype;
            parakey += "&moduletype=" + row.moduletype;
            parakey += "&receipttype=" + row.receipttype;
            suburl = '../../ASHX/Basic/SysNoManage.ashx?M=update' + parakey;
        }
        catch (e) {
            $.messager.alert({
                //異常提示
                //提交數據異常，請確認網絡連接狀況
                title: '<%=Language("ExceptionTips")%>',
                msg: '<%=Language("SubmitDataException")%>'
            });
        }
    }

    //新增保存
    function SaveSystemNo() {
        var CompanyId = $('#addcompany').combobox("getValue");
        var ModuleType = $('#addmoduletype').combobox("getValue");
        var ModularType = $('#addmodulartype').combobox("getValue");
        var Category = $('#addcategory').combobox("getValue");
        var ReceiptType = $('#addreceipttype').val();
        var Word = $('#addkeyword').combobox("getValue");
        var DateType = $('#adddatetype').combobox("getValue");
        $('#fmSystemNo').form('submit', {
            url: suburl,
            onSubmit: function () {
                var isSubmit = true; 
                return isSubmit;
            },
            success: function (result) {
                try {
                    var result = eval('(' + result + ')');
                    if (result.success) {
                        $.messager.defaults = { ok: "<%=Language("comform")%>" };
                        //確定
                        //成功提示 數據已保存成功
                        $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("DataSaveSuccess")%>');
                        $('#addSystemNo').dialog('close'); 	// close the dialog
                        $('#tbSysNo').datagrid('reload'); // reload the user data
                    } else {
                        //錯誤提示
                        $.messager.alert('<%=Language("errTips")%>', result.msg, 'warning',
                                function () {
                                });
                    }
                }
                catch (e) {
                    $.messager.alert({
                        //異常提示
                        //提交數據異常，請確認網絡連接狀況
                        title: '<%=Language("ExceptionTips")%>',
                        msg: '<%=Language("SubmitDataException")%>'
                    });
                }
            }
        });
    }
</script>
<div id="addSystemNo" class="easyui-dialog" data-options="closed:true,modal:true"
    maximizable="true" style="width: 700px; height: 200px; padding: 2px;" buttons="#dlg-buttons" maximizable="true">
    <form id="fmSystemNo" method="post">
    <table style="width: 100%;">
        <tr>
            <td>
                <input type="hidden" name="autoid" />
                <%=Language("CompanyId")%>:
            </td>
            <td>
                <%--請選擇公司別--%>
                <input style="width: 120px; font-size: 12px;" id="addcompany" name="companyid" reg="^.+$"
                    tip="<%=Language("ESN001")%>" />
            </td>
            <td>
                <%=Language("ModuleType")%>:
            </td>
            <td>
                <%--請選擇模塊別--%>
                <input style="width: 120px; font-size: 12px;" id="addmoduletype" name="moduletype"
                    reg="^.+$" tip="<%=Language("selectModel")%>" />
            </td>
            <td>
                <%=Language("ModularType")%>:
            </td>
            <td>
                <%--請選擇模組別--%>
                <input style="width: 120px; font-size: 12px;" id="addmodulartype" name="modulartype"
                    reg="^.+$" tip="<%=Language("selectModelGroup")%>" />
            </td>
        </tr>
        <tr>
            <td>
                <!--category-->
                <%=Language("comp_type")%>:
            </td>
            <td>
                <%--請選擇類別--%>
                <input style="width: 120px; font-size: 12px;" id="addcategory" name="category" reg="^.+$"
                    tip="<%=Language("ESN004")%>" />
            </td>
            <td>
                <%=Language("receipttype")%>:
            </td>
            <td>
                <%--請選擇單據別--%>
                <input style="width: 120px; font-size: 12px;" id="addreceipttype" name="receipttype"
                    reg="^.+$" tip="<%=Language("ESN005")%>" />
            </td>
            <td>
                <%=Language("keyword")%>:
            </td>
            <td>
                <%--請選擇關鍵字--%>
                <input style="width: 120px" id="addkeyword" name="keyword" reg="^.+$" tip="<%=Language("ESN006")%>"" />
            </td>
        </tr>
        <tr>
            <td>
                <%=Language("datetype")%>:
            </td>
            <td>
                <input style="width: 120px; font-size: 12px;" id="adddatetype" name="datetype" reg="^.+$"
                    <%--請選擇--%>
                    tip="<%=Language("releaselect")%>"+<%=Language("datetype")%>" />
            </td>
            <td>
                <%=Language("codelen")%>:
            </td>
            <td>
                <%--請輸入流水碼長度，且須為數字--%>                
                <input style="width: 115px" id="addcodelen" name="codelen" reg="^[\d]+$" tip="<%=Language("warderCodeIsData")%>"!" />
            </td>
            <td>
                <%=Language("Usy")%>:
            </td>
            <td>
                <select style="width: 120px; font-size: 12px;" id="addusy" name="usy">
                    <option value="Y">Y</option>
                    <option value="N">N</option>
                </select>
            </td>
        </tr>
        <tr>
            <td>
                <%=Language("Mark")%>:
            </td>
            <td colspan="5">
                <textarea style="width: 90%; height: 30px;" id="addmark" name="mark"></textarea>
            </td>
        </tr>
    </table>
    </form>
</div>
<div id="dlg-buttons">
    <a class="easyui-linkbutton" href="javascript:void(0)" onclick="SaveSystemNo()">
        <%=Language("Save")%></a> <a class="easyui-linkbutton" href="javascript:void(0)"
            onclick="closeAddWindow('addSystemNo')">
            <%=Language("Cancel")%></a>
</div>
