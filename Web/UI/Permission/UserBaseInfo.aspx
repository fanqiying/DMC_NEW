<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserBaseInfo.aspx.cs" Inherits="UI_Permission_UserBaseInfo" %>

<%@ Register Src="../../UIControl/Permission/UserBaseInfo/dialogdivUser.ascx" TagName="dialogdivUser"
    TagPrefix="uc1" %>
<%@ Register Src="../../UIControl/Permission/UserBaseInfo/dialogdivUpdatePwd.ascx"
    TagName="dialogdivUpdatePwd" TagPrefix="uc2" %>
<%@ Register Src="../../UIControl/Permission/UserBaseInfo/dialogProgramSetting.ascx"
    TagName="dialogProgramSetting" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    <!--用戶基本資料-->
    <script type="text/javascript">

        var res =
     {
         FailureTips: '<%=Language("FailureTips")%>',
         IntenetException: '<%=Language("IntenetException")%>',
         ExceptionTips: '<%=Language("ExceptionTips")%>',
         SubmitDataException: '<%=Language("SubmitDataException")%>'

     };



        $(document).ready(function () {
            dialogTransfer("divUser");
            dialogTransfer("divUpdatePwd");
            dialogTransfer("ProgramSetting");
            //綁定datagrid
            $('#tbUser').datagrid({
                //是否折叠
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: '../../ASHX/Permission/UserRightManage.ashx?M=Search',
                fitColumns: true,
                //數據在一行顯示 
                nowrap: false,
                //行條紋化
                striped: true,
                //固定序號
                rownumbers: true,
                //是否可以多選
                singleSelect: true,
                //sortName: 'userID',
                //sortOrder: 'desc',
                remoteSort: false,
                idField: 'userID',
                loadMsg: '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: true,
                //固定列  
                frozenColumns: [[{ field: 'ck', checkbox: true }, { field: 'userID', title: '<%=Language("UserId")%>', width: 100, sortable: true, align: 'left'}]],
                //可動列
                columns: [[{ field: 'userName', title: '<%=Language("UserName")%>', width:  $(this).width() * 0.2, align: 'left' },
                           { field: 'userDept', title: '<%=Language("DeptName")%>', width:  $(this).width() * 0.2, align: 'left' },
                           { field: 'userType', title: '<%=Language("SystemType")%>', width:  $(this).width() * 0.1, align: 'left', formatter: function (value, row, index) { return ReadType(row.userType);; }},
                           { field: 'usy', title:  '<%=Language("Usy")%>', width:  $(this).width() * 0.1, align: 'left' },
                           { field: 'Opt', title: '<%=Language("Opt")%>', width:  $(this).width() * 0.3, align: 'left', formatter: function (value, row, index) { return '<a href="#" <%=IsUsy("Delete") %> onclick="<%=IsCanClick("Delete") %>DelUser(\'' + index + '\')"><%=Language("Delete")%></a> | <a href="#" <%=IsUsy("View") %> onclick="<%=IsCanClick("View") %>ViewUser(\'' + index + '\')"><%=Language("View")%></a> | <a href="#" <%=IsUsy("Edit") %> onclick="<%=IsCanClick("Edit") %>EditUser(' + index + ');"><%=Language("Edit")%></a> | <a href="#" <%=IsUsy("ResetPwd") %> onclick="<%=IsCanClick("ResetPwd") %>ResetPwd(' + index + ');"><%=Language("ResetPwd")%></a> | <a href="#" <%=IsUsy("Copy") %> onclick="<%=IsCanClick("Copy") %>CopyUser(' + index + ');"><%=Language("Copy")%></a>'; } }
                ]]
            });
            //設置分页控件屬性 
            var p = $('#tbUser').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: <%= Language("PageList")%> ,
                beforePageText: '<%= Language("BeforePageText")%>',
                afterPageText: '<%= Language("AfterPageText")%>',
                displayMsg: '<%= Language("DisplayMsg")%>'
            });

            //加載下拉框的多語言
            $('#userDept').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=dept',
                valueField: 'deptID',
                textField: 'deptID',
                panelHeight:'150px',
                editable:true ,
                hasDownArrow:false,
                filter:function(value,row)
                {
                    return row.deptID.toLowerCase().match(value.toLowerCase())!=null;
                }
            });

            //加載下拉框的多語言
            $('#domainAddrInfo').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=domainserver',
                valueField: 'hostname',
                textField: 'hostname',
                panelHeight:'150px',
                editable:false 
            });
        });

        $(window).resize(function(){
            $('#tbUser').datagrid('resize');
        });
        var url = "";
        function ReadType(SystemType) {
            if (SystemType == "01")
                return '<%=Language("sysuser")%>';
            else if (SystemType == "02")
                return '<%=Language("Supply")%>';
              else if (SystemType == "03")
                  return '<%=Language("customername")%>';
} 

function Search(type) {
    var queryParams = $('#tbUser').datagrid('options').queryParams;
    queryParams.SearchType = type;
    queryParams.KeyWord = $('#txtKeyword').val().replace('<%=Language("InputDefaultKey")%>','');
    queryParams.UD = $('#sUserId').val();
    queryParams.UserNo = $('#sUserNo').val();
    queryParams.UserName = $('#sUserName').val();
    queryParams.UserType = $('#sUserType').val();
    queryParams.Usy = $('#sUsy').val();
    queryParams.UserMail = $('#sUserEmail').val();
    $('#tbUser').datagrid('reload');
}
        
function CopyUser(index) {
    var row = $('#tbUser').datagrid('getData').rows[index];
    if (row) {
        tab_option = $('#usertabs').tabs('getTab', '<%=Language("ReviseInfoTab")%>').panel('options').tab;
        tab_option.hide();
        $('#dlg-buttons').show();
        $('#divUser').dialog('open').dialog('setTitle', '<%=Language("CopyUserDetail")%>');
        $('#fmBase').form('load', row);
        LoadRose();
        LoadCompany();
        SupplySearch();
        $('#userIdInfo').removeAttr("readonly");
        $('#userIdInfo').val('');
        $('#userNoInfo').val('');
        $('#userMailInfo').val('');
        $('#userNameInfo').val('');
        $('#domainIDInfo').val('');
        LoadProgram();
    }
}

function JointESC(op)
{
    RightTips({input:op,keyword:op.value},$('#drpUserType').val());
}

function DelUser(index) {
    var ids = [];
    if (index) {
        var row = $('#tbUser').datagrid('getData').rows[index];
        ids.push(row.userID);
    }
    else {
        var rows = $('#tbUser').datagrid('getChecked');
        for (var i = 0; i < rows.length; i++) {
            //每行ID放入數組中
            ids.push(rows[i].userID);
        }
    }
    if (ids.length > 0) {
        //必須為string類型，否則傳輸不過去 
        var aa = ids.toString();
        $.messager.defaults = { ok: '<%=Language("yes")%>', cancel: '<%=Language("mesageNo")%>' };
        $.messager.confirm('<%=Language("DeleteConfirm")%>', '<%=Language("comformDelMsg")%>', function (r) {
            if (r) {
                $.post('../../ASHX/Permission/UserRightManage.ashx?M=delete', { UserIdList: aa }, function (result) {
                    try {
                        if (result.success) {
                            $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("delsucess")%>');
                                    $('#tbUser').datagrid('reload'); // reload the user data
                                } else {
                                    $.messager.alert('<%=Language("errTips")%>', result.msg);
                                }
                            }
                            catch (e) {
                                $.messager.alert({
                                    title: '<%=Language("ExceptionTips")%>',
                                msg: '<%=Language("SubmitDataException")%>'
                            });
                        }
                        }, 'json').error(function () {
                            $.messager.alert({
                                title: '<%=Language("ExceptionTips")%>',
                            msg: '<%=Language("SubmitDataException")%>'
                        });
                    });
                }
                });
        }
        else {
            $.messager.alert('<%=Language("msgDel")%>', '<%=Language("DelTips")%>');
    }
}


var falg=false;//判斷狀態是否為查看，新增修改==
    </script>
    <!--角色權限-->
    <script type="text/javascript">
        $(document).ready(function () {
            //綁定权限类别tbUserRose
            $('#tbUserRose').datagrid({
                //是否折叠
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                //數據在一行顯示 
                nowrap: false,
                //固定序號
                rownumbers: true,
                //行條紋化
                striped: true,
                //是否可以多選
                singleSelect: false,
                sortName: 'RoseId',
                sortOrder: 'desc',
                remoteSort: false,
                idField: 'RoseId',
                loadMsg: '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: false,
                //固定列  
                frozenColumns: [[{ field: 'userrosechk', checkbox: true}]],
                //可動列
                columns: [[{ field: 'RoseId', title: '<%=Language("RoseId")%>', width: 200, align: 'center' },
                           { field: 'RoseName', title: '<%=Language("RoseName")%>', width: 200, align: 'center' }
                ]],
                onLoadSuccess: function (data) {
                    if (data) {
                        $.each(data.rows, function (index, item) {
                            if (item.Usy == 'Y') {
                                $('#tbUserRose').datagrid('checkRow', index);
                            }
                        });
                    }
                    if (falg == true) {//判斷是否是查看
                        $('#tbUserRose').parent().find("div .datagrid-header-check").children("input[type='checkbox']").each(function (i, value) {
                            $(value).attr("disabled", "disabled");
                        });
                        $('input[type="checkbox"][name="userrosechk"]').each(function (i, value) {
                            $(value).attr("disabled", "disabled");
                        });
                    }
                    else {
                        $('#tbUserRose').parent().find("div .datagrid-header-check").children("input[type='checkbox']").each(function (i, value) {
                            $(value).removeAttr("disabled");
                        });
                    }

                }
            });
        });

        //加載權限
        function LoadRose() {
            $('#tbUserRose').datagrid('options').url = "../../ASHX/Permission/UserRightManage.ashx?M=getright&RightType=RoseType";
            var queryParams = $('#tbUserRose').datagrid('options').queryParams;
            queryParams.UB = $('#userIdInfo').val();
            queryParams.UserType = $('#drpUserType').val();
            queryParams.Keyword = $('#txtTypeKeyWord').val().replace('<%=Language("CompanyKey")%>', '');
            $('#tbUserRose').datagrid('load');
        }
    </script>
    <!--程式權限-->
    <script type="text/javascript">
        $(document).ready(function () {
            //綁定程式權限
            $('#tbProgram').datagrid({
                //是否折叠
                collapsible: true,
                //數據在一行顯示 
                nowrap: false,
                checkOnSelect: false,
                selectOnCheck: false,
                //行條紋化
                striped: true,
                //是否可以多選
                singleSelect: false,
                sortName: 'ProgramId',
                sortOrder: 'desc',
                remoteSort: false,
                idField: 'ProgramId',
                loadMsg: '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: false,
                //可動列
                columns: [[{ field: 'ProgramId', title: '<%=Language("ProgramId")%>', width: 100, align: 'center' },
                           { field: 'DisplayValue', title: '<%=Language("ProgramName")%>', width: 150, align: 'center' },
                           { field: 'ActionIdNames', title: '<%=Language("ActionRight")%>', width: 200, align: 'center' },
                           { field: 'Opt', title: '<%=Language("Opt")%>', width: 150, align: 'center', formatter: function (value, row, index) { return '<a href="#" name="delProgname" onclick="delProgram(\'' + index + '\')"><%=Language("Delete")%></a>'; } }
                ]],
                //add by jeven_xiao
                onLoadSuccess: function (data) {
                    if (falg == true) {//判斷是否是查看
                        $('#bnSetProg').attr("disabled", "disabled");

                        $('a[name="delProgname"]').each(function (i, value) {
                            $(value).attr("disabled", "disabled");
                        });
                    }
                    else {
                        $('#bnSetProg').removeAttr("disabled");
                        $('a[name="delProgname"]').each(function (i, value) {
                            $(value).removeAttr("disabled");
                        });
                    }
                }
            });
        });

        //加載程式
        function LoadProgram() {
            $('#tbProgram').datagrid('options').url = "../../ASHX/Permission/UserRightManage.ashx?M=getright&RightType=program";
            var queryParams = $('#tbProgram').datagrid('options').queryParams;
            queryParams.UB = $('#userIdInfo').val();
            queryParams.KeyWord = $('#txtProgramKeyWord').val().replace('<%=Language("ProgramKey")%>', '');
            $('#tbProgram').datagrid('reload');
        }

        function delProgram(index) {
            var row = $('#tbProgram').datagrid('getData').rows[index];
            if (row) {
                $.post('../../ASHX/Permission/UserRightManage.ashx?M=delProgram',
                   { UB: $('#userIdInfo').val(), ProgramId: row.ProgramId },
                   function (result) {
                       try {
                           if (result.success) {
                               $.messager.defaults = { ok: '<%=Language("comform")%>' };
                               $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("delsucess")%>');
                               $('#tbProgram').datagrid('reload'); // reload the user data
                           } else {
                               $.messager.alert('<%=Language("errTips")%>', result.msg);
                           }
                       } catch (e) {
                           $.messager.alert({
                               title: '<%=Language("ExceptionTips")%>',
                               msg: '<%=Language("SubmitDataException")%>'
                           });
                       }
                   }, 'json');
               }
           }
    </script>
    <!--公司權限-->
    <script type="text/javascript">
        $(document).ready(function () {
            //綁定公司權限
            $('#tbCompany').datagrid({
                //是否折叠
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                //數據在一行顯示 
                nowrap: false,
                //固定序號
                rownumbers: true,
                //行條紋化
                striped: true,
                //是否可以多選
                singleSelect: false,
                sortName: 'companyID',
                sortOrder: 'desc',
                remoteSort: false,
                idField: 'companyID',
                loadMsg: '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: false,
                //固定列  
                frozenColumns: [[{ field: 'companychk', checkbox: true}]],
                //可動列
                columns: [[{ field: 'companyID', title: '<%=Language("comp_id")%>', width: 200, align: 'center' },
                           { field: 'simpleName', title: '<%=Language("comp_sinpleName")%>', width: 200, align: 'center' }
                ]],
                onLoadSuccess: function (data) {
                    if (data) {
                        $.each(data.rows, function (index, item) {
                            if (item.Usy == 'Y') {
                                $('#tbCompany').datagrid('checkRow', index);
                            }
                        });
                    }
                    if (falg == true) {//判斷是否是查看
                        $('#tbCompany').parent().find("div .datagrid-header-check").children("input[type='checkbox']").each(function (i, value) {
                            $(value).attr("disabled", "disabled");
                        });
                        $('input[type="checkbox"][name="companychk"]').each(function (i, value) {
                            $(value).attr("disabled", "disabled");
                        });

                    }
                    else {
                        $('#tbCompany').parent().find("div .datagrid-header-check").children("input[type='checkbox']").each(function (i, value) {
                            $(value).removeAttr("disabled");
                        });
                    }
                }
            });
        });
        //加載公司
        function LoadCompany() {
            $('#tbCompany').datagrid('options').url = "../../ASHX/Permission/UserRightManage.ashx?M=getright&RightType=company";
            var queryParams = $('#tbCompany').datagrid('options').queryParams;
            queryParams.UB = $('#userIdInfo').val();
            $('#tbCompany').datagrid('reload');
        }
    </script>
    <!--供應商權限-->
    <%--<script type="text/javascript">
        $(document).ready(function () {
            //綁定供應商權限
            $('#tbSupply').datagrid({
                //是否折叠
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                //數據在一行顯示 
                nowrap: false,
                //固定序號
                rownumbers: true,
                //行條紋化
                striped: true,
                //是否可以多選
                singleSelect: false,
                sortName: 'suppNumber',
                sortOrder: 'desc',
                remoteSort: false,
                idField: 'suppNumber',
                loadMsg: '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: false,
                //固定列  
                frozenColumns: [[{ field: 'supplyck', checkbox: true}]],
                //可動列
                columns: [[{ field: 'suppNumber', title: '<%=Language("SupplyId")%>', width: 200, align: 'center' },
                           { field: 'suppName', title: '<%=Language("SupplyName")%>', width: 200, align: 'center' }
                ]],
                onLoadSuccess: function (data) {
                    if (data) {
                        $.each(data.rows, function (index, item) {
                            if (item.Usy == 'Y') {
                                $('#tbSupply').datagrid('checkRow', index);
                            }
                        });
                    }
                    if (falg == true) {//判斷是否是查看
                        $('#tbSupply').parent().find("div .datagrid-header-check").children("input[type='checkbox']").each(function (i, value) {
                            $(value).attr("disabled", "disabled");
                        });
                        //add by jeven_xiao
                        $("#usuallyInfo :input").each(function () {
                            $(this).attr("readonly", "readonly");
                        });

                        $('input[type="checkbox"][name="supplyck"]').each(function (i, value) {
                            $(value).attr("disabled", "disabled");
                        });
                    }
                    else {
                        $('#tbSupply').parent().find("div .datagrid-header-check").children("input[type='checkbox']").each(function (i, value) {
                            $(value).removeAttr("disabled");
                        });
                        if ($("#userIdInfo").attr("readonly") != 'readonly') {
                            $("#usuallyInfo :input").each(function () {
                                $(this).removeAttr("readonly");
                            });
                        }
                    }
                }
            });
        });

        //供應商權限
        function SupplySearch() {
            $('#tbSupply').datagrid('options').url = "../../ASHX/Permission/UserRightManage.ashx?M=getright&RightType=supply";
            var queryParams = $('#tbSupply').datagrid('options').queryParams;
            queryParams.UB = $('#userIdInfo').val();
            $('#tbSupply').datagrid('reload');
        }
    </script>--%>
    <!--程式資料操作、公司、資料權限-->
    <script type="text/javascript">
        var currentPData = null;
        $(document).ready(function () {
            //綁定程式資料權限
            $('#tbData').datagrid({
                //是否折叠
                collapsible: true,
                checkOnSelect: true,
                //url: '../ASHX/UserRightManage.ashx?M=getright&RightType=data',
                //數據在一行顯示 
                nowrap: false,
                checkOnSelect: false,
                selectOnCheck: false,
                //行條紋化
                striped: true,
                //是否可以多選
                singleSelect: true,
                sortName: 'deptID',
                sortOrder: 'desc',
                remoteSort: false,
                idField: 'deptID',
                loadMsg: '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: false,
                //可動列
                columns: [[{ field: 'Usy',
                    tatle: '<%=Language("Usy")%>',
                    width: 40,
                    formatter: function (value, row, rowIndex) {
                        value = ExistsDept(row.deptID, value);
                        if (value == 'Y') {
                            return '<input onclick="chkDataUsy(this,' + rowIndex + ')" id="IsDataUsy' + row.deptID + '" name="IsDataUsy" type="checkbox" checked/>';
                        }
                        else {
                            return '<input onclick="chkDataUsy(this,' + rowIndex + ')" id="IsDataUsy' + row.deptID + '" name="IsDataUsy" type="checkbox"/>';
                        }
                    }
                },
                            { field: 'fullName', title: '<%=Language("DeptName")%>', width: 140, align: 'left' },
                           { field: 'IsAll',
                               title: '<%=Language("ActionRight")%>',
                           width: 80,
                           align: 'left',
                           formatter: function (value, row, rowIndex) {
                               value = DeptStatus(row.deptID, value);
                               if (value == 'Y')
                                   return '<input id="IsAll' + row.deptID + '" name="IsAll" type="checkbox" checked/><label for="IsAll' + row.deptID + '"><%=Language("All")%></label>';

                               else
                                   return '<input id="IsAll' + row.deptID + '" name="IsAll" text="所有" type="checkbox"/><label for="IsAll' + row.deptID + '"><%=Language("All")%></label>';
                           }
                           }
                ]]
            });
        });

               function ExistsDept(deptid, value) {
                   if (currentPData == null)
                       return value;
                   else {
                       var ttp = [];
                       for (var i = 0; i < currentPData.length; i++) {
                           ttp = currentPData[i].split("|")
                           if (ttp[0] == deptid) {
                               return "Y";
                           }
                       }
                       return "N";
                   }
               }

               function DeptStatus(deptid, value) {
                   if (currentPData == null)
                       return value;
                   else {
                       var ttp = [];
                       for (var i = 0; i < currentPData.length; i++) {
                           ttp = currentPData[i].split("|")
                           if (ttp[0] == deptid) {
                               return ttp[1];
                           }
                       }
                       return "N";
                   }
               }

               function chkDataUsy(chk, index) {
                   if (chk.id == "IsDataUsy0") {
                       if (chk.checked) {
                           var chks = $('input[name="IsDataUsy"]');
                           for (var i = 0; i < chks.length; i++) {
                               if (chks[i].id != chk.id) {
                                   chks[i].checked = !chk.checked;
                               }
                           }
                       }
                   }
                   else {
                       if (chk.checked) {
                           $('input[id="IsDataUsy0"]')[0].checked = false;
                       }
                   }
               }
               function PDataSearch(program) {
                   $('#tbData').datagrid('options').url = "../../ASHX/Permission/UserRightManage.ashx?M=getright&RightType=data";
                   var queryParams = $('#tbData').datagrid('options').queryParams;
                   queryParams.UB = $('#userIdInfo').val();
                   queryParams.ProgramId = program;
                   $('#tbData').datagrid('load');
               }
    </script>
    <script type="text/javascript">
        function SetRead() {
            $('#dlg-buttons').hide();
            if (!$('#usuallyInfo').hasClass("tbView2")) {
                $('#usuallyInfo').addClass("tbView2");
            }
            $('#fmBase').attr("readonly", "readonly");
            $('#usuallyInfo').addClass("tbView2");
            $("#drpUserType").attr("disabled", '');
            $("#drpUsy").attr("disabled", '');
            $('#domainAddrInfo').combobox("disable");
            $('#userDept').combobox("disable");
        }

        function SetEdit() {
            $('#dlg-buttons').show();
            if ($('#usuallyInfo').hasClass("tbView2")) {
                $('#usuallyInfo').removeClass("tbView2");
            }
            $('#userIdInfo').attr("readonly", "readonly");
            $('#domainAddrInfo').combobox("enable");
            $('#userDept').combobox("enable");
            $("#drpUserType").removeAttr("disabled");
            $("#drpUsy").removeAttr("disabled");
        }

        function ResetAll() {
            $('#dlg-buttons').show();
            if ($('#usuallyInfo').hasClass("tbView2")) {
                $('#usuallyInfo').removeClass("tbView2");
            }
            $('#fmBase').removeAttr("readonly");
            $('#userIdInfo').removeAttr("readonly");
            $("#drpUserType").removeAttr("disabled");
            $("#drpUsy").removeAttr("disabled");
            $('#domainAddrInfo').combobox("enable");
            $('#userDept').combobox("enable");
        }
    </script>
</head>
<body>
    <div style="width: 98%; background-color: transparent;">
        <div id="divOperation" class="Search">
            <div class="l leftSearch">
                <span>
                    <input type="text" id="txtKeyword" style="width: 200px;" value="<%=Language("InputDefaultKey")%>"
                        onfocus="this.value=''" onblur="if(!value){value=defaultValue;}" /></span><span><a
                            href="javascript:void(0);" <%=IsUsy("Search") %> onclick="<%=IsCanClick("Search") %>Search('ByKey')"><%=Language("Search")%></a></span><span><a
                                href="javascript:void(0);" <%=IsUsy("AdvancedSearch") %> onclick="<%=IsCanClick("AdvancedSearch") %>openSearch('divSearch');"><%=Language("AdvancedSearch")%></a></span>
            </div>
            <div class="r rightSearch">
                <img src="../../images/add.gif" />
                <a href="javascript:void(0)" <%=IsUsy("Add") %> onclick="<%=IsCanClick("Add") %>AddUser()">
                    <%=Language("Add")%></a> &nbsp;
            <img src="../../images/del.gif" />
                <a href="javascript:void(0)" <%=IsUsy("BatchDelete") %> onclick="<%=IsCanClick("BatchDelete") %>DelUser()">
                    <%=Language("BatchDelete")%></a> &nbsp;&nbsp;
            </div>
        </div>
        <div style="clear: both">
        </div>
        <div class="squery" id="divSearch">
            <div class="sinquery">
                <table cellpadding="0" cellspacing="0" class="addCoyTB">
                    <tr>
                        <td style="width: 10%;">
                            <%=Language("UserId")%>:
                        </td>
                        <td style="width: 20%;">
                            <input type="text" id="sUserId" name="UserId" />
                        </td>
                        <td style="width: 10%;">
                            <%=Language("UserNo")%>:
                        </td>
                        <td style="width: 20%;">
                            <input type="text" id="sUserNo" name="UserNo" />
                        </td>
                        <td style="width: 10%;">
                            <%=Language("UserEmail")%>:
                        </td>
                        <td style="width: 30%;">
                            <input type="text" id="sUserEmail" name="UserEmail" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Language("UserName")%>:
                        </td>
                        <td>
                            <input type="text" id="sUserName" name="UserName" />
                        </td>
                        <td>
                            <%=Language("SystemType")%>:
                        </td>
                        <td>
                            <select style="width: 90px" id="sUserType">
                                <option value="">ALL</option>
                                <option value="01"><%=Language("SystemType")%></option>
                                <option value="02"><%=Language("Supply")%></option>
                                <option value="03"><%=Language("customername")%></option>
                            </select>
                        </td>
                        <td>
                            <%=Language("Usy")%>:
                        </td>
                        <td>
                            <select style="width: 65px" id="sUsy">
                                <option value="">ALL</option>
                                <option value="Y">Y</option>
                                <option value="N">N</option>
                            </select>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="clear: both">
            </div>
            <div style="text-align: right; padding: 5px 0; border: 0px; height: 21px; font-size: 12px;">
                <span><a href="javascript:void(0)" onclick="Search('ByAdvanced')">
                    <%=Language("Search")%></a></span>
            </div>
        </div>
        <div style="clear: both">
        </div>
        <table id="tbUser" width="100%" fit="false">
        </table>
    </div>
    <script type="text/javascript">
        function UseMulticom(chk, tr) {
            var multi = document.getElementById(chk).checked;
            var companyList = document.getElementById(tr);
            if (multi) {
                companyList.style.display = "block";
            }
            else {
                companyList.style.display = "none";
            }
        }
    </script>
    <!--根据-->
    <script type="text/javascript">
    </script>
    <uc1:dialogdivUser ID="dialogdivUser1" runat="server" />
    <div style="clear: both">
    </div>
    <uc2:dialogdivUpdatePwd ID="dialogdivUpdatePwd1" runat="server" />
    <!--彈出程式設定處理-->
    <uc3:dialogProgramSetting ID="dialogProgramSetting1" runat="server" />
</body>
</html>
