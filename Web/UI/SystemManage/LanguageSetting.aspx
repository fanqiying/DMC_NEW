<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LanguageSetting.aspx.cs" Inherits="UI_SystemManage_LanguageSetting" %>

<%@ Register Src="../../UIControl/SystemManage/LanguageSetting/dialogAddNew.ascx" TagName="dialogAddNew" TagPrefix="uc1" %>

<%@ Register Src="../../UIControl/SystemManage/LanguageSetting/dialogdivManageLanguage.ascx" TagName="dialogdivManageLanguage" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link rel="stylesheet" type="text/css" href="../../css/public.css" />
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../easyUI15/jquery.min.js"></script>
    <script type="text/javascript" src="../../easyUI15/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../js/jquery.json-2.4.js"></script>
    <script type="text/javascript" src="../../easyUI15/locale/easyui-lang-zh_TW.js"></script>
    <script type="text/javascript" src="../../js/common.js"></script> 
    <script src="../../js/keyUp.js" type="text/javascript"></script>
    <!--初始化控件加載-->
    <script type="text/javascript" id="InitUI">
        var url = "";
        $(document).ready(function () {
            dialogTransfer("addNew");
            dialogTransfer("divManageLanguage");
            //綁定datagrid
            $('#tbLanguage').datagrid({
                //是否折叠
                checkOnSelect: false,
                selectOnCheck: false,
                collapsible: true,
                fitColumns: true,
                url:'../../ASHX/SystemManage/LanguageManage.ashx?M=Search',
                //數據在一行顯示 
                nowrap: true,
                //行條紋化
                striped: true,
                //固定序號
                rownumbers: true,
                //是否可以多選
                singleSelect: true,
                //sortName: 'resourceid',
                //sortOrder: 'desc',
                remoteSort: false,
                idField: 'ResourceId',
                loadMsg: '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: true,
                //固定列  
                frozenColumns: [[{ field: 'ck', checkbox: true}]],
                //可動列
                columns: [[{ field: 'resourceid', title: '<%=Language("ResourceId")%>', width: $(this).width() * 0.3, align: 'left' },
                           { field: 'defaultvalue', title: '<%=Language("DefaultValue")%>', width: $(this).width() * 0.3, align: 'left' },
                           { field: 'resourcetype', title: '<%=Language("ResourceType")%>', width: $(this).width() * 0.1, align: 'left', formatter: function (value, row, index) { return ReadType(row.resourcetype); } },
                           { field: 'usy', title: '<%=Language("Usy")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'opt', title: '<%=Language("Opt")%>', width: $(this).width() * 0.1, align: 'left', 
                               formatter: function (value, row, index) 
                               { 
                                   var str="";
                                   str+='<a href="#" <%=IsUsy("Delete") %> onclick="<%=IsCanClick("Delete") %>delLanguage(\'' + index + '\')"><%=Language("Delete")%></a> | ';                                
                                str+='<a href="#" <%=IsUsy("Edit") %> onclick="<%=IsCanClick("Edit") %>editLanguage(' + index + ');"><%=Language("Edit")%></a>';
                                
                               return str;
                           } }
                ]]
            });
            //設置分页控件屬性 
            var p = $('#tbLanguage').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: <%= Language("PageList")%>,
                beforePageText: '<%= Language("BeforePageText")%>',
                afterPageText: '<%= Language("AfterPageText")%>',
                displayMsg: '<%= Language("DisplayMsg")%>'
            });
            //加載下拉框的多語言
            $('#SResourceType').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=ResourceType',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight:'150px',
                editable:false ,
                filter:function(value,row)
                {
                    return row.displaytext.toLowerCase().match(value.toLowerCase())!=null;
                }
            });
            //加載下拉框的多語言
            $('#AResourceType').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=ResourceType',
                valueField: 'displayid',
                textField: 'displaytext',
                panelHeight:'150px',
                editable:false ,
                filter:function(value,row)
                {
                    return row.displaytext.toLowerCase().match(value.toLowerCase())!=null;
                }
            });

            $(window).resize(function(){
                $('#tbLanguage').datagrid('resize');
            });
        });

        function ReadType(ResourceType) {
            if (ResourceType == "01")
                return "Label";
            else if (ResourceType == "02")
                return "Message";
            else if (ResourceType == "03")
                return "Operation";
            else if (ResourceType == "04")
                return "Select";
            else if (ResourceType == "05")
                return "Menu";
            else if (ResourceType == "06")
                return "Program";
            else
                "Other";
        }
       
    </script>
    <!--資源管理管理-->
    <script type="text/javascript">
        function delLanguage(index) {
            var ids = [];
            if (index) {
                var row = $('#tbLanguage').datagrid('getData').rows[index];
                ids.push(row.resourceid);
            }
            else {
                var rows = $('#tbLanguage').datagrid('getChecked');
                for (var i = 0; i < rows.length; i++) {
                    //每行ID放入數組中
                    ids.push(rows[i].resourceid);
                }
            }
            if (ids.length > 0) {
                //必須為string類型，否則傳輸不過去 
                var aa = ids.toString();
                $.messager.defaults = { ok: '<%=Language("yes")%>', cancel: '<%=Language("mesageNo")%>' }; //yes 是 mesageNo 否
                $.messager.confirm('<%=Language("DeleteConfirm")%>', '<%=Language("delPerConfirm")%>', function (r) {//delPerConfirm
                    if (r) {
                        $.post('../../ASHX/SystemManage/LanguageManage.ashx?M=delete', { LanguageList: aa },
                        function (result) {
                            try {
                                if (result.success) {
                                    $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("deletesuccess")%>'); //SuccessTips 成功提示 deletesuccess 删除成功 
                                    $('#tbLanguage').datagrid('load');
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
                });
            }
            else {
                $.messager.alert('<%=Language("msgDel")%>', '<%=Language("DelTips")%>'); //msgDel 刪除數據 DelTips 請選擇需要刪除的數據
            }
        }

        function Search(type) {
            try {
                var queryParams = $('#tbLanguage').datagrid('options').queryParams;
                queryParams.SearchType = type;
                queryParams.KeyWord = $('#txtKeyword').val().replace('<%=Language("InputDefaultKey")%>', '');
                queryParams.ResourceId = $('#sResourceId').val();
                queryParams.ResourceType = $('#SResourceType').combobox("getValue");
                queryParams.DefaultValue = $('#sDefaultValue').val();
                queryParams.Usy = $('#sUsy').val();
                $('#tbLanguage').datagrid('load');
            }
            catch (e) {
                $.messager.alert({
                    title: '<%=Language("ExceptionTips")%>', //ExceptionTips 異常提示
                    msg: '<%=Language("SubmitDataException")%>'//SubmitDataException 提交數據異常，請確認網絡連接狀況
                });
            }
        }

     

        function LoadLanguageValue(value) {
            $.post('../../ASHX/SystemManage/LanguageManage.ashx?M=LoadResValue&ResourceId=' + value,
                   { RoseId: '' },
                   function (result) {
                       InitLanguageValue(result);
                   }, 'json');
        }

        function InitLanguageValue(list) {
            $("#addNewTable tr").attr("align", "center");
            var trList = $('tr[name="languagevaluetr"]');
            for (var i = 0; i < trList.length; i++)
                $("tr[id='" + trList[i].id + "']").remove(); //刪除行
            if (list) {
                for (var i = 0; i < list.length; i++) {
                    var contenttr = '<tr id="languagevaluetr' + i + '" name="languagevaluetr">';
                    contenttr += '<td>' + list[i].languagename + ':</td>';
                    contenttr += '<td colspan="3"><input type="hidden" id="hideValue' + i + '" value="' + list[i].languageid + '" />';
                    contenttr += '<input type="text" id="langvalue' + i + '" style="width: 90%;" name="langvalue" value="' + list[i].displayvalue + '" /></td>';
                    contenttr += '</tr>';
                    $("#addNewTable").append(contenttr);
                }
            }
        }

     
    </script>
    <!--語言類別管理-->
    <script type="text/javascript">
    

        function LoadLanguageType() {
            $.post('../../ASHX/Basic/ComboxManage.ashx?ComboxType=languagetype',
                   { RoseId: '' },
                   function (result) {
                       InitLanguageType(result);
                   }, 'json');
        }
        var max = 0;
        //加載已有數據
        function InitLanguageType(list) {
            max = 0;
            $("#openLanguageType tr").attr("align", "center");
            var trList = $('tr[name="languagetypetr"]');
            for (var i = 0; i < trList.length; i++)
                $("tr[id='" + trList[i].id + "']").remove(); //刪除行

            if (list) {
                for (var i = 0; i < list.length; i++) {
                    var contenttr = '<tr style="height: 24px;" id="languagetypetr' + i + '" name="languagetypetr">';
                    contenttr += '<td class="td" style="text-align: center; vertical-align: middle;">';
                    contenttr += '<input style="width: 120px;border:0px;" type="text" id="languageid' + i + '" value="' + list[i].languageid + '" readonly /></td>';
                    contenttr += '<td class="td" style="text-align: center; vertical-align: middle;">';
                    contenttr += '<input style="width: 120px;border:0px;" type="text" id="languagename' + i + '" value="' + list[i].languagename + '" readonly /></td>';
                    if (list[i].usy == 'Y')
                        contenttr += '<td><input type="checkbox" checked id="languageusy' + i + '" name="languageusy" /><label for="languageusy' + i + '">有效</label></td>';
                    else
                        contenttr += '<td><input type="checkbox" id="languageusy' + i + '" name="languageusy" /><label for="languageusy' + i + '">有效</label></td>';
                    contenttr += '<td><a id="A3"><img style="cursor: pointer" src="../../images/del.gif" onclick="DeleteLanguageType(' + i + ')"></img>刪除</a></td>';
                    contenttr += '</tr>';
                    $("#openLanguageType").append(contenttr);
                    max++;
                }
            }
        }
        //添加行
        function DeleteLanguageType(index) {
            $("tr[id='languagetypetr" + index + "']").remove(); //删除行
        }
        //刪除行
        function AddLanguageType() {
            var contenttr = '<tr style="height: 24px;" id="languagetypetr' + max + '" name="languagetypetr">';
            contenttr += '<td class="td" style="text-align: center; vertical-align: middle;">';
            contenttr += '<input style="width: 120px;" type="text" id="languageid' + max + '" /></td>';
            contenttr += '<td class="td" style="text-align: center; vertical-align: middle;">';
            contenttr += '<input style="width: 120px;" type="text" id="languagename' + max + '" /></td>';
            contenttr += '<td><input type="checkbox" checked id="languageusy' + max + '" name="languageusy" /><label for="languageusy' + max + '">有效</label></td>';
            contenttr += '<td><a id="A3"  onclick="DeleteLanguageType(' + max + ')"><img style="cursor: pointer" src="../../images/del.gif"></img>刪除</a></td>';
            contenttr += '</tr>';
            $("#openLanguageType").append(contenttr);
            max++;
        }

     
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="divOperation" class="Search">
            <div class="l leftSearch">
                <span>
                    <input type="text" id="txtKeyword" style="width: 200px;" value='<%=Language("InputDefaultKey")%>'
                        onfocus="this.value=''" onblur="if(!value){value=defaultValue;}" /></span><span><a
                            href="javascript:void(0);" <%=IsUsy("Search") %> onclick="<%=IsCanClick("Search") %>Search('ByKey')"><%=Language("Search")%></a></span><span><a
                                href="javascript:void(0);" <%=IsUsy("AdvancedSearch") %> onclick="<%=IsCanClick("AdvancedSearch") %>openSearch('divSearch');"><%=Language("AdvancedSearch")%></a></span>
            </div>
            <div class="r rightSearch">
                <a href="javascript:void(0)" <%=IsUsy("GenerateBit") %> onclick="<%=IsCanClick("GenerateBit") %>Generate()">
                    <%=Language("GenerateBit")%></a> &nbsp; <a href="javascript:void(0)" <%=IsUsy("LangTypeSetting") %>
                        onclick="<%=IsCanClick("LangTypeSetting") %>OpenLanguageType()">
                        <%=Language("LangTypeSetting")%></a> &nbsp;
            <img src="../../images/add.gif" width="10" height="10" />
                <a href="javascript:void(0)" <%=IsUsy("Add") %> onclick="<%=IsCanClick("Add") %>addLanguage()">
                    <%=Language("Add")%></a> &nbsp;
            <img src="../../images/del.gif" width="10" height="10" />
                <a href="javascript:void(0)" <%=IsUsy("BatchDelete") %> onclick="<%=IsCanClick("BatchDelete") %>delLanguage()">
                    <%=Language("BatchDelete")%></a> &nbsp;&nbsp;
            </div>
        </div>
        <div style="clear: both">
        </div>
        <div class="squery" id="divSearch">
            <div class="sinquery">
                <table class="addCoyTB">
                    <tr>
                        <td>
                            <%=Language("ResourceId")%>:
                        </td>
                        <td>
                            <input type="text" id="sResourceId" name="ResourceId" />
                        </td>
                        <td>
                            <%=Language("DefaultValue")%>:
                        </td>
                        <td>
                            <input type="text" id="sDefaultValue" name="DefaultValue" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Language("ResourceType")%>:
                        </td>
                        <td>
                            <input id="SResourceType" name="ResourceType" style="width:150px;" />
                        </td>
                        <td>
                            <%=Language("Usy")%>:
                        </td>
                        <td>
                            <select style="width: 65px" id="sUsy" name="Usy">
                                <option value="">ALL</option>
                                <option value="Y">Y</option>
                                <option value="N">N</option>
                            </select>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="text-align: right; padding: 5px 0; border: 0; height: 21px; font-size: 12px;">
                <a href="javascript:void(0)" onclick="Search('ByAdvanced')">
                    <%=Language("Search")%></a>&nbsp;&nbsp;&nbsp;&nbsp;
            </div>
        </div>
        <div style="clear: both">
        </div>
        <div style="float: left; position: absolute; width: 100%">
            <table id="tbLanguage" width="98%" fit="false">
            </table>
        </div>
        <div style="clear: both">
        </div>
    </form>
    <uc1:dialogAddNew ID="dialogAddNew1" runat="server" />

    <div style="clear: both">
    </div>
    <div id="dlg-buttons">
        <a class="easyui-linkbutton" href="javascript:void(0)" onclick="saveLanguage()">
            <%=Language("Save")%></a> <a class="easyui-linkbutton" href="javascript:void(0)"
                onclick="closeAddWindow('addNew')">
                <%=Language("Cancel")%></a>
    </div>
    <div style="clear: both">
    </div>
    <uc2:dialogdivManageLanguage ID="dialogdivManageLanguage1"
        runat="server" />

    <div id="dlg-buttons1">
        <a class="easyui-linkbutton" href="javascript:void(0)" onclick="SaveLanguageType()">
            <%=Language("Save")%></a> <a class="easyui-linkbutton" href="javascript:void(0)"
                onclick="closeAddWindow('divManageLanguage')">
                <%=Language("Cancel")%></a>
    </div>
</body>
</html>
