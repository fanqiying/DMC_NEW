<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dialogdivUser.ascx.cs"
    Inherits="UIControl_Permission_UserBaseInfo_dialogdivUser" %>
<%@ Register Src="~/UIControl/RevisionInfo.ascx" TagName="RevisionInfo" TagPrefix="vri" %>
<script type="text/javascript">

    function ClearUserNo() {
        $('#userNoInfo').val('');
        $('#userNameInfo').val('');
        $('#userMailInfo').val('');
    }

    function AddUser() {
        ResetAll();
        falg = false;
        $('#fmBase').form('clear');
        tab_option = $('#usertabs').tabs('getTab', '<%=Language("ReviseInfoTab")%>').panel('options').tab;
        tab_option.hide();
        $('#divUser').dialog('open').dialog('setTitle', '<%=Language("AddUser")%>');
        $('#drpUsy').val('Y');
        $('#drpUserType').val('01');
        LoadRose();
        LoadProgram();
        LoadCompany();
        //SupplySearch();
    }

    function EditUser(index) {
        falg = false;
        var row = $('#tbUser').datagrid('getData').rows[index];
        if (row) {
            ResetAll();
            tab_option = $('#usertabs').tabs('getTab', '<%=Language("ReviseInfoTab")%>').panel('options').tab;
            tab_option.hide();
            $('#divUser').dialog('open').dialog('setTitle', '<%=Language("EditUser")%>');
            $('#fmBase').form('load', row);
            SetEdit()
            LoadRose();
            LoadProgram();
            LoadCompany();
            //SupplySearch();
        }
    }

    function saveRose() {
        var roseList = [];
        var companyList = [];
        var supplyList = [];
        var roses = $('#tbUserRose').datagrid('getChecked');
        for (var i = 0; i < roses.length; i++) {
            //每行ID放入數組中
            roseList.push(roses[i].RoseId);
        }
        var companys = $('#tbCompany').datagrid('getChecked');
        for (var i = 0; i < companys.length; i++) {
            //每行ID放入數組中
            companyList.push(companys[i].companyID);
        }
        //var supplys = $('#tbSupply').datagrid('getChecked');
        //for (var i = 0; i < supplys.length; i++) {
        //    //每行ID放入數組中
        //    supplyList.push(supplys[i].suppNumber);
        //}
        if ($('#userIdInfo').attr("readonly"))
            url = "../../../ASHX/Permission/UserRightManage.ashx?M=mod&RoseList=" + roseList.toString() + "&CompanyList=" + companyList.toString() + "&SupplyList=" + supplyList.toString();
        else
            url = "../../../ASHX/Permission/UserRightManage.ashx?M=add&RoseList=" + roseList.toString() + "&CompanyList=" + companyList.toString() + "&SupplyList=" + supplyList.toString();
        $('#fmBase').form('submit', {
            url: url,
            onSubmit: function () {
                var error = '';
                if ($('#userIdInfo').val().match(/^[\d\w]+$/) == null) {
                    error = '<%=Language("userformaterr")%>'; //userformaterr 請輸入使用者編號，且只能為英文和數字！
                }
                else if ($.trim($('#userNoInfo').val()) == '') {
                    error = '<%=Language("usernameisempty")%>'; //usernameisempty 使用者名称不能為空
                }

                if (error != '') {
                    $.messager.alert('<%=Language("errTips")%>', error);
                    return false;
                }
                return true;
            },
            success: function (result) {
                var result = eval('(' + result + ')');
                if (result.success) {
                    $.messager.defaults = { ok: '<%=Language("yes")%>' };
                    $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("DataSaveSuccess")%>'); //SuccessTips 成功提示 DataSaveSuccess 數據已保存成功
                    $('#divUser').dialog('close'); 	// close the dialog
                    $('#tbUser').datagrid('reload'); // reload the user data
                } else {
                    $.messager.alert('<%=Language("errTips")%>', result.msg);
                }
            }
        });
    }
    function ViewUser(index) {
        falg = true;
        ResetAll();
        var row = $('#tbUser').datagrid('getData').rows[index];
        if (row) {
            tab_option = $('#usertabs').tabs('getTab', '<%=Language("ReviseInfoTab")%>').panel('options').tab;
            tab_option.show();
            $('#divUser').dialog('open').dialog('setTitle', '<%=Language("UserDetail")%>');
            $('#fmBase').form('load', row);
            $('#frmRevise').form('load', row);
            SetRead();
            LoadRose();
            LoadProgram();
            LoadCompany();
            //SupplySearch();
            LoadTelInfo();
        }
    }
</script>
<div id="divUser" class="easyui-dialog" data-options="closed:true,modal:true" style="width: 760px;
    height: 400px; padding: 5px;" buttons="#dlg-buttons" maximizable="true">
    <div class="easyui-layout" fit="true">
        <div region="north" border="false" style="height: 80px;" class="p-search">
            <form id="fmBase" method="post" novalidate>
            <table style="width: 98%">
                <tr>
                    <td style="vertical-align: top; width: 10%;">
                        <%=Language("SystemType")%>:
                    </td>
                    <td style="width: 20%;">
                        <select style="width: 90px" id="drpUserType" name="userType" onchange="ClearUserNo();">
                            <option value="01">
                                <%=Language("systemuser")%></option>
                            <option value="02">
                                <%=Language("Supply")%></option>
                            <option value="03">
                                <%=Language("customername")%></option>
                        </select>
                    </td>
                    <td style="width: 10%;">
                        <%=Language("UserId")%>:
                    </td>
                    <td style="width: 20%;">
                        <input style="width: 90px" id="userIdInfo" name="userID" />
                    </td>
                    <td style="width: 10%;">
                        <%=Language("UserNo")%>:
                    </td>
                    <td style="width: 20%;">
                        <input style="width: 90px" id="userNoInfo" name="userNo" onkeyup="JointESC(this);" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 10%;">
                        <%=Language("UserEmail")%>:
                    </td>
                    <td style="width: 20%;">
                        <input style="width: 90px" id="userMailInfo" name="userMail" />
                    </td>
                    <td>
                        <%=Language("UserName")%>:
                    </td>
                    <td>
                        <input style="width: 90px" id="userNameInfo" name="userName" />
                    </td>
                    <td>
                        <%=Language("emp_dept")%>:
                    </td>
                    <td>
                        <input style="width: 90px" name="userDept" id="userDept" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Language("AreaAccount")%>:
                    </td>
                    <td>
                        <input style="width: 90px" id="domainIDInfo" name="domainID" />
                    </td>
                    <td>
                        <%=Language("HostIP")%>:
                    </td>
                    <td>
                        <input style="width: 90px" id="domainAddrInfo" name="domainAddr" />
                    </td>
                    <td>
                        <%=Language("Usy")%>:
                    </td>
                    <td>
                        <select style="width: 90px" id="drpUsy" name="usy">
                            <option value="Y">Y</option>
                            <option value="N">N</option>
                        </select>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <%=Language("defaultRole")%>:
                    </td>
                    <td>
                        <input style="width: 90px" id="defaultRole" name="defaultRole" />
                    </td>
                    <td>
                        
                    </td>
                    <td>
                         
                    </td>
                    <td>
                         
                    </td>
                    <td>
                         
                    </td>
                </tr>
               
            </table>
            </form>
        </div>
        <div region="center" border="false">
            <div id="usertabs" class="easyui-tabs" style="margin: auto; height: 220px;" border="true"
                fit="true">
                <div title="<%=Language("CompanyRight")%>" data-options="closable:false" style="overflow: auto;
                    padding: 5px;">
                    <table id="tbCompany" width="98%" fit="true">
                    </table>
                </div>
                <div title="<%=Language("RoseTypeSetting")%>" style="padding: 5px;">
                    <div class="easyui-layout" fit="true">
                        <div region="north" border="false" style="height: 30px;" class="p-search">
                            <div style="float: left;">
                                <input type="text" id="txtTypeKeyWord" style="width: 200px;" value='<%=Language("CompanyKey")%>'
                                    onfocus="this.value=''" onblur="if(!value){value=defaultValue;}" />
                                <a href="javascript:void(0);" onclick="LoadRose()">
                                    <%=Language("Search")%></a>
                            </div>
                        </div>
                        <div region="center" border="false">
                            <table id="tbUserRose" width="98%" fit="true">
                            </table>
                        </div>
                    </div>
                </div>
                <div title="<%=Language("ProgramSetting")%>" data-options="closable:false" style="padding: 5px;">
                    <div class="easyui-layout" fit="true">
                        <div region="north" border="false" style="height: 30px;" class="p-search">
                            <div style="float: left;">
                                <input type="text" id="txtProgramKeyWord" style="width: 200px;" value='<%=Language("ProgramKey")%>'
                                    onfocus="this.value=''" onblur="if(!value){value=defaultValue;}" />
                                <a href="javascript:void(0);" onclick="LoadProgram()">
                                    <%=Language("Search")%></a>
                            </div>
                            <div style="float: right;">
                                <input type="button" value="<%=Language("SetProgram")%>" onclick="OpenProgramSetting()"
                                    id="bnSetProg" />
                            </div>
                        </div>
                        <div region="center" border="false">
                            <table id="tbProgram" width="98%" fit="true">
                            </table>
                        </div>
                    </div>
                </div>
               <%-- <div title="<%=Language("SupplySetting")%>" data-options="closable:false" style="overflow: auto;
                    padding: 5px;">
                    <table id="tbSupply" width="98%" fit="true">
                    </table>
                </div>--%>
                <div title="<%=Language("ReviseInfoTab")%>" data-options="closable:false" style="overflow: auto;
                    padding: 5px; text-align: left;" id="ReviseInfoTab">
                    <form id="frmRevise" method="post" novalidate>
                    <vri:RevisionInfo ID="vri1" runat="server" />
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>
<div id="dlg-buttons">
    <a class="easyui-linkbutton" href="javascript:void(0)" onclick="saveRose()">
        <%=Language("Save")%></a> <a class="easyui-linkbutton" href="javascript:void(0)"
            onclick="closeAddWindow('divUser')">
            <%=Language("Cancel")%></a>
</div>
