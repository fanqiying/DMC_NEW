<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dialogAddNew.ascx.cs" Inherits="UIControl_SystemManage_LanguageSetting_dialogAddNew" %>
   <script>
       function addLanguage() {
           LoadLanguageValue('');
           $('#addNew').dialog('open').dialog('setTitle', '<%=Language("LanguageNewTitle")%>');
           $('#fmNew').form('clear');
           $('#ResourceIdInfo').removeAttr("readonly");
           $('#UsyInfo').val('Y');
           $('#ResourceTypeInfo').val('01');
           url = '../../ASHX/SystemManage/LanguageManage.ashx?M=add';
       }
       function editLanguage(index) {
           var row = $('#tbLanguage').datagrid('getData').rows[index];
           if (row) {
               LoadLanguageValue(row.ResourceId);
               $('#addNew').dialog('open').dialog('setTitle', '<%=Language("LanguageEditTitle")%>');
               $('#fmNew').form('load', row);
               $('#ResourceIdInfo').attr("readonly", "readonly");
               url = '../../ASHX/SystemManage/LanguageManage.ashx?M=update';
           }
       }
       function saveLanguage() {
           var error = '';
           var errorid = '';
           var pattern = new RegExp("[`~!$^{}\\[\\]/?\"\'‘']");
           var results = '';
           var trList = $('tr[name="languagevaluetr"]');
           for (var i = 0; i < trList.length; i++) {
               var no = trList[i].id.match(/[\d]+/);
               results += 'LanguageId:' + $('input[id="hideValue' + no + '"]').val();
               results += ',LanguageValue:' + $('input[id="langvalue' + no + '"]').val() + ';';
               if ($('input[id="langvalue' + no + '"]').val().match(pattern) != null) {
                   error = '<%=Language("ErrMsg")%>';//ErrMsg國際化中的內容不能包含特殊字符
                   errorid = 'langvalue' + no;
               }
           }
           if (error == '') {
               if ($('#ResourceIdInfo').val().match(/^[\d\w]+$/) == null) {
                   error = '<%=Language("InputRule")%>';//InputRule請輸入資源編號，且只能為英文和數字！
                   errorid = 'ResourceIdInfo';
               }
               else if ($.trim($('#DefaultValueInfo').val()) == '') {
                   error = '<%=Language("DefaultValueInfo")%>';//DefaultValueInfo 默認顯示值不能為空！
                   errorid = 'DefaultValueInfo';
               }
               else if ($('#DefaultValueInfo').val().match(pattern) != null) {
                   error = '<%=Language("DefaultValueInfoSpeCial")%>';//DefaultValueInfoSpeCial 默認顯示值中不能包含特殊字符
                   errorid = 'DefaultValueInfo';
               }
               else if ($('#AResourceType').combobox("getValue") == '') {
                   error = '<%=Language("AResourceType")%>';//AResourceType 請設置資源類別
                   errorid = 'AResourceType';
               }
               else if ($('#GroupKeyInfo').val().match(pattern) != null) {
                   error = '<%=Language("GroupKeyInfo")%>';//組Id不能包含特殊字符
                   errorid = 'GroupKeyInfo';
               }
               else if ($('#GroupValueInfo').val().match(pattern) != null) {
                   error = '<%=Language("GroupValueInfo")%>';//GroupValueInfo組內容不能包含特殊字符！
                   errorid = 'GroupValueInfo';
               }
           }

           if (error != '') {
               $.messager.alert('<%=Language("errTips")%>', error, 'warning', function () { $("#" + errorid).focus(); });//errTips 錯誤提示
               return;
           }

           $('#fmNew').form('submit', {
               url: url + '&LanguageValues=' + results,
               onSubmit: function () {
                   return $(this).form('validate');
               },
               success: function (result) {
                   try {
                       var result = eval('(' + result + ')');
                       if (result.success) {
                           $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("DataSaveSuccess")%>');//SuccessTips 成功提示 DataSaveSuccess 數據已保存成功
                           $('#addNew').dialog('close'); 	// close the dialog
                           $('#tbLanguage').datagrid('reload'); // reload the user data
                       } else {
                           $.messager.alert('<%=Language("errTips")%>', result.msg, 'warning',//errTips 錯誤提示
                            function () {
                                if (result.msg.indexOf('<%=Language("Exists")%>') > -1) {//Exists 存在
                                    $("#ResourceIdInfo").focus();
                                } else {
                                    $("#DefaultValueInfo").focus();
                                }
                            });
                       }
                   }
                   catch (e) {
                       $.messager.alert({
                           title: '<%=Language("ExceptionTips")%>',//ExceptionTips 異常提示
                           msg: '<%=Language("SubmitDataException")%>'//SubmitDataException 提交數據異常，請確認網絡連接狀況
                       });
                   }
               }
           });
       }
   </script>
   <div id="addNew" class="easyui-dialog" data-options="closed:true,modal:true" style="width: 600px;
        height: 270px; padding: 5px;" buttons="#dlg-buttons" maximizable="true">
        <form id="fmNew" method="post" novalidate>
        <table cellpadding="0" cellspacing="0" class="addCoyTB " id="addNewTable" width="100%">
            <tr>
                <td>
                    <%=Language("ResourceId")%>:
                </td>
                <td>
                    <input type="text" id="ResourceIdInfo" name="resourceid" />
                </td>
                <td>
                    <%=Language("ResourceType")%>:
                </td>
                <td>
                    <input id="AResourceType" name="resourcetype" style="width:150px;"/>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Language("GroupKey")%>:
                </td>
                <td>
                    <input type="text" id="GroupKeyInfo" name="groupkey" />
                </td>
                <td>
                    <%=Language("GroupValue")%>:
                </td>
                <td>
                    <input type="text" id="GroupValueInfo" name="groupvalue" />
                </td>
            </tr>
            <tr>
                <td>
                    <%=Language("DefaultValue")%>:
                </td>
                <td>
                    <input type="text" id="DefaultValueInfo" name="defaultvalue" />
                </td>
                <td>
                    <%=Language("Usy")%>:
                </td>
                <td>
                    <select style="width: 65px" id="UsyInfo" name="usy">
                        <option value="Y">Y</option>
                        <option value="N">N</option>
                    </select>
                </td>
            </tr>
        </table>
        </form>
    </div>