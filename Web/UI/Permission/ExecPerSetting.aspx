<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExecPerSetting.aspx.cs" Inherits="UI_Permission_ExecPerSetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link rel="stylesheet" type="text/css" href="../../css/public.css" />
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../easyUI15/jquery.min.js"></script>
    <script type="text/javascript" src="../../easyUI15/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../js/jquery.json-2.4.js"></script>
    <script type="text/javascript" src="../../easyUI15/locale/easyui-lang-zh_TW.js"></script>
    <script type="text/javascript" src="../../js/common.js"></script>
    <script src="../../js/keyUp.js" type="text/javascript"></script>
    <!--主页內容處理-->
    <script type="text/javascript">
        $(document).ready(function () {
            dialogTransfer("ProgramSetting");
            dialogTransfer("CopyPermission");
            dialogTransfer("RightSetting");
            //綁定datagrid
            $('#tbRose').datagrid({
                //是否折叠
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: '../../ASHX/Permission/RoseManage.ashx?M=Search',
                fitColumns: true,
                //數據在一行顯示 
                nowrap: true,
                //行條紋化
                striped: true,
                //固定序號
                rownumbers: true,
                //是否可以多選
                singleSelect: true,
                //sortName: 'RoseId',
                //sortOrder: 'desc',
                remoteSort: false,
                idField: 'RoseId',
                loadMsg: '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: true,
                onDblClickRow: function (Index, rowData) { RightSetting(Index) },
                //可動列
                columns: [[{ field: 'RoseId', title: '<%=Language("RoseId")%>', width: $(this).width() * 0.2, align: 'left' },
                           { field: 'RoseName', title: '<%=Language("RoseName")%>', width: $(this).width() * 0.3, align: 'left' },
                           { field: 'SystemType', title: '<%=Language("SystemType")%>', width: $(this).width() * 0.1, align: 'left', formatter: function (value, row, index) { return ReadType(row.SystemType); } },
                           { field: 'Usy', title: '<%=Language("Usy")%>', width: $(this).width() * 0.1, align: 'left' },
                           { field: 'Opt', title: '<%=Language("Opt")%>', width: $(this).width() * 0.2, align: 'left', formatter: function (value, row, index) { return '<a href="#" <%=IsUsy("RightCopy") %> onclick="<%=IsCanClick("RightCopy") %>CopyRight(\'' + index + '\')"><%=Language("RightCopy")%></a> | <a href="#" <%=IsUsy("RightSetting") %> onclick="<%=IsCanClick("RightSetting") %>RightSetting(\'' + index + '\')"><%=Language("RightSetting")%></a>'; } }
                ]]
            });
            //設置分页控件屬性 
            var p = $('#tbRose').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: <%= Language("PageList")%>,
                beforePageText: '<%= Language("BeforePageText")%>',
                afterPageText: '<%= Language("AfterPageText")%>',
                displayMsg: '<%= Language("DisplayMsg")%>'
            });

            //加載下拉框的多語言
            $('#SocureRoseId').combobox({
                url: '../../ASHX/Basic/ComboxManage.ashx?ComboxType=rosetype',
                valueField: 'roseid',
                textField: 'roseid',
                panelHeight:'150px',
                editable:true ,
                hasDownArrow:false,
                filter:function(value,row)
                {
                    return row.roseid.toLowerCase().match(value.toLowerCase())!=null;
                }
            });
        });
        $(window).resize(function(){
            $('#tbRose').datagrid('resize');
        });
        function ReadType(SystemType) {
            if (SystemType == "01")
                return '<%=Language("systemuser")%>';//systemuser 系統用戶
            else if (SystemType == "02")
                return '<%=Language("supplyid")%>';//supplyid 供應商
            else
                return '<%=Language("customername")%>';//customername 客戶
    }

    var url = "";
    //複製權限
    function CopyRight(index) {
        $('#SocureRoseId').combobox('reload');
        var row = $('#tbRose').datagrid('getData').rows[index];
        if (row) {
            if (row.Usy != "N") {
                $('#fmCopy').form('clear');
                $('#CopyPermission').dialog('open').dialog('setTitle', '<%=Language("RightCopy")%>');
                $('#fmCopy').form('load', row);
                url = '../../ASHX/Permission/RoseManage.ashx?M=copy';
            }
            else {
                alert('<%=Language("notuseycopy")%>');//notuseycopy 已失效的權限類別不可以被復制!!
            }
        }
    }
    function Search(type) {
        var queryParams = $('#tbRose').datagrid('options').queryParams;
        queryParams.SearchType = type;
        queryParams.KeyWord = $('#txtKeyword').val().replace('<%=Language("InputDefaultKey")%>','');
            queryParams.RoseId = $('#sRoseId').val();
            queryParams.RoseName = $('#sRoseName').val();
            queryParams.SystemType = $('#sSystemType').val();
            queryParams.Usy = $('#sUsy').val();
            $('#tbRose').datagrid('load');
        }
    </script>
    <!--程式權限-->
    <script type="text/javascript">
        //綁定程式權限
        $(document).ready(function () {
            $('#tbProgram').datagrid({
                //是否折叠
                collapsible: true,
                //數據在一行顯示 
                nowrap: true,
                checkOnSelect: false,
                selectOnCheck: false,
                //行條紋化
                striped: true,
                //是否可以多選
                singleSelect: true,
                //sortName: 'ProgramId',
                //sortOrder: 'desc',
                remoteSort: false,
                idField: 'ProgramId',
                loadMsg: '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: false,
                //可動列
                columns: [[{ field: 'ProgramId', title: '<%=Language("ProgramId")%>', width: 100, align: 'left' },
                           { field: 'DisplayValue', title: '<%=Language("ProgramName")%>', width: 150, align: 'left' },
                           { field: 'ActionIdNames', title: '<%=Language("ActionRight")%>', width: 200, align: 'left' },
                           { field: 'Opt', title: '<%=Language("Opt")%>', width: 60, align: 'left', formatter: function (value, row, index) { return '<a href="#" onclick="delProgram(\'' + index + '\')"><%=Language("Delete")%></a> '; } }
                ]]
            });
        });

        //程式權限搜索
        function ProgramSearch() {
            $('#tbProgram').datagrid('options').url = "../../ASHX/Permission/RoseRightSetting.ashx?M=Search&T=program";
            var queryParams = $('#tbProgram').datagrid('options').queryParams;
            queryParams.RoseId = $('#hideRoseId').val();
            queryParams.KeyWord = $('#txtProgramKeyWord').val().replace('<%=Language("ProgramKey")%>', '');
            $('#tbProgram').datagrid('load');
        }
        //刪除程式權限
        function delProgram(index) {
            //$('#tbProgram').datagrid('selectRow', index);
            var row = $('#tbProgram').datagrid('getData').rows[index];
            if (row) {
                $.post('../../ASHX/Permission/RoseRightSetting.ashx?M=delProgram',
                   { RoseId: $('#hideRoseId').val(), ProgramId: row.ProgramId },
                   function (result) {
                       try {
                           if (result.success) {
                               $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("deletesuccess")%>'); // SuccessTips 成功提示 deletesuccess 數據已刪除成功
                               $('#tbProgram').datagrid('reload'); // reload the user data
                           } else {
                               $.messager.alert('<%=Language("errTips")%>', result.msg); //errTips 錯誤提示
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
    <!--資料權限-->
    <script type="text/javascript">
        //綁定資料權限
        $(document).ready(function () {
            $('#tbData').datagrid({
                //是否折叠
                collapsible: true,
                //數據在一行顯示 
                nowrap: true,
                checkOnSelect: false,
                selectOnCheck: false,
                //行條紋化
                striped: true,
                //是否可以多選
                singleSelect: true,
                //sortName: 'deptID',
                //sortOrder: 'desc',
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
                        if (value == 'Y') {
                            return '<input onclick="chkDataUsy(this,' + rowIndex + ')" id="IsDataUsy' + row.deptID + '" name="IsDataUsy" type="checkbox" checked/>';
                        }
                        else {
                            return '<input onclick="chkDataUsy(this,' + rowIndex + ')" id="IsDataUsy' + row.deptID + '" name="IsDataUsy" type="checkbox"/>';
                        }
                    }
                },
                           { field: 'fullName', title: '<%=Language("DeptName")%>', width: 200, align: 'left' },
                           { field: 'IsAll',
                               title: '<%=Language("ActionRight")%>',
                           width: 100,
                           formatter: function (value, row, rowIndex) {
                               if (value == 'Y')
                                   return '<input id="IsAll' + row.deptID + '" name="IsAll" type="checkbox" checked/><label for="IsAll' + row.deptID + '"><%=Language("All")%></label>';

                                   else
                                       return '<input id="IsAll' + row.deptID + '" name="IsAll" type="checkbox"/><label for="IsAll' + row.deptID + '"><%=Language("All")%></label>';

                               }
                           }
                ]]
            });
            //$(".datagrid-header-check").hide(); 
        });

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

                   //資料權限搜索
                   function DataSearch() {
                       $('#tbData').datagrid('options').url = '../../ASHX/Permission/RoseRightSetting.ashx?M=Search&T=data';
                       var queryParams = $('#tbData').datagrid('options').queryParams;
                       queryParams.RoseId = $('#hideRoseId').val();
                       $('#tbData').datagrid('load');
                   }
    </script>
    <!--供應商權限-->
  <%--  <script type="text/javascript">
        //綁定供應商權限
        $(document).ready(function () {           
            $('#tbSupply').datagrid({
                //是否折叠
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                //數據在一行顯示 
                nowrap: true,
                //固定序號
                rownumbers: true,
                //行條紋化
                striped: true,
                //是否可以多選
                singleSelect: true,
                //sortName: 'suppNumber',
                //sortOrder: 'desc',
                remoteSort: false,
                idField: 'suppNumber',
                loadMsg: '<%=Language("WaitData")%>',
                //是否顯示分页
                pagination: false,
                //固定列  
                frozenColumns: [[{ field: 'supplyck', checkbox: true}]],
                //可動列
                columns: [[{ field: 'suppNumber', title: '<%=Language("SupplyId")%>', width: 200, align: 'left' },
                           { field: 'suppName', title: '<%=Language("SupplyName")%>', width: 200, align: 'left' }
                ]],
                onLoadSuccess: function (data) {
                    if (data) {
                        $.each(data.rows, function (index, item) {
                            if (item.Usy == 'Y') {
                                $('#tbSupply').datagrid('checkRow', index);
                            }
                        });
                    }
                }
            });
        });
        //供應商權限
        function SupplySearch() {
            $('#tbSupply').datagrid('options').url = '../../ASHX/Permission/RoseRightSetting.ashx?M=Search&T=supply';
            var queryParams = $('#tbSupply').datagrid('options').queryParams;
            queryParams.RoseId = $('#hideRoseId').val();
            $('#tbSupply').datagrid('load');
        }
    </script>--%>

    <!--彈出程式設定處理-->
    <script type="text/javascript">
        //{"ProgramId":p01,"ActionIds":xxx}
        var PAList = [];
        var tempList = [];
        var selectNode = null;


        function LoadAction(ProgramId) {
            $.post('../../ASHX/Permission/RoseRightSetting.ashx?M=Search&T=programaction',
                   { RoseId: $('#hideRoseId').val(), ProgramId: ProgramId },
                   function (result) {
                       SetAction(result);
                       InitTempResult(ProgramId);
                   }, 'json');
        }

        function InitTempResult(programId) {
            for (var i = 0; i < PAList.length; i++) {
                if (PAList[i].indexOf(programId) > -1) {
                    var chk = $('input[name="actionchk"]');
                    var selectChk = PAList[i].toString().split("|");
                    var action = selectChk[1];
                    var actionList = action.split(";");
                    for (var j = 0; j < chk.length; j++) {
                        chk[j].checked = false;
                        for (var h = 0; h < actionList.length; h++) {
                            if (actionList[h] == chk[j].id) {
                                chk[j].checked = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        //動態生成Action表格
        function SetAction(actionList) {
            $("#tbAction tr").attr("align", "center");
            var _len = $("#tbAction tr").length;
            //alert(_len);
            for (var i = 2; i <= _len; i++) {
                $("tr[id='actiontr" + i + "']").remove(); //删除行
            }
            if (actionList && actionList.rows) {
                var rowMod = actionList.rows.length % 3;
                var rowLen = parseInt(actionList.rows.length / 3);
                if (rowMod > 0) rowLen += 1;
                for (var i = 2; i <= rowLen + 1; i++) {
                    var trcontent = "<tr id='actiontr" + i + "' align='center' style='height: 26px;'>";
                    trcontent += "<td style='width:33%'>";
                    if (actionList.rows[(i - 2) * 3 + 0]) {
                        trcontent += "<input type='checkbox' name='actionchk' onclick='UpdatePAList()' ";
                        if (actionList.rows[(i - 2) * 3 + 0].Usy == "Y")
                            trcontent += " checked ";
                        trcontent += "id='" + actionList.rows[(i - 2) * 3 + 0].ActionId + "'>";
                        trcontent += actionList.rows[(i - 2) * 3 + 0].ActionName;
                        trcontent += "</input>";
                    }
                    trcontent += "</td>";
                    trcontent += "<td style='width:33%'>";
                    if (actionList.rows[(i - 2) * 3 + 1]) {
                        trcontent += "<input type='checkbox' name='actionchk' onclick='UpdatePAList()' ";
                        if (actionList.rows[(i - 2) * 3 + 1].Usy == "Y")
                            trcontent += " checked ";
                        trcontent += "id='" + actionList.rows[(i - 2) * 3 + 1].ActionId + "'>";
                        trcontent += actionList.rows[(i - 2) * 3 + 1].ActionName;
                        trcontent += "</input>";
                    }
                    trcontent += "</td>";
                    trcontent += "<td style='width:34%'>";
                    if (actionList.rows[(i - 2) * 3 + 2]) {
                        trcontent += "<input type='checkbox' name='actionchk' onclick='UpdatePAList()' ";
                        if (actionList.rows[(i - 2) * 3 + 2].Usy == "Y")
                            trcontent += " checked ";
                        trcontent += "id='" + actionList.rows[(i - 2) * 3 + 2].ActionId + "'>";
                        trcontent += actionList.rows[(i - 2) * 3 + 2].ActionName;
                        trcontent += "</input>";
                    }
                    trcontent += "</td>";
                    trcontent += "</tr>";
                    $("#tbAction").append(trcontent);
                }
            }
        }

        function CheckAllAction(all) {
            var chk = $('input[name="actionchk"]');
            for (var i = 0; i < chk.length; i++)
                chk[i].checked = all.checked;
            UpdatePAList();
        }
        //獲取表格中的CheckBox中的值
        function GetActionIds() {
            var actionIdList = [];
            var chk = $('input[name="actionchk"]');
            for (var i = 0; i < chk.length; i++) {
                if (chk[i].checked)
                    actionIdList.push(chk[i].id);
            }
            return actionIdList.toString().replace(/,/g, ";");
        }

        function UpdatePAList() {
            var node = $('#ProgramStatus').tree("getSelected");
            if (node && node.attributes) {
                $.each(PAList, function (index, value) {
                    if (value.toString().indexOf(node.id.toString()) < 0)
                        tempList.push(value);
                });
                tempList.push(node.id + "|" + GetActionIds());
                PAList = tempList;
                tempList = [];
            }
        }

        function CopySave() {
            $('#fmCopy').form('submit', {
                url: url,
                onSubmit: function () {
                    return $(this).form('validate');
                },
                success: function (result) {
                    try {
                        var result = eval('(' + result + ')');
                        if (result.success) {
                            $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("DataSaveSuccess")%>'); //SuccessTips 成功提示 DataSaveSuccess 數據已保存成功
                            $('#CopyPermission').dialog('close'); 	// close the dialog
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
                }
            });
        }


        function OpenProgramSetting() {
            PAList = [];
            $("#tbAction").hide();
            $('#ProgramStatus').tree({
                url: '../../ASHX/Permission/RoseRightSetting.ashx?M=gettree&RoseId=' + $('#hideRoseId').val(),
                onSelect: function (node) {
                    selectNode = null;
                    if (node.attributes) {
                        $("#tbAction").show();
                        LoadAction(node.id);
                        selectNode = node;
                    }
                    else {
                        $("#tbAction").hide();
                    }
                },
                onCheck: function (node, isChecked) {
                    if (selectNode != null && selectNode.id == node.id) {
                        var chk = $('input[name="actionchk"]');
                        for (var j = 0; j < chk.length; j++) {
                            chk[j].checked = isChecked;
                        }
                        UpdatePAList();
                    }
                }
            });
            $('#ProgramStatus').tree("reload");
            $('#ProgramSetting').dialog('open').dialog('setTitle', '<%=Language("ProgramSetting")%>');
        }
        function SaveProgramSetting() {
            var list = PAList.toString();
            var programId = [];
            var node = $('#ProgramStatus').tree("getChecked");
            for (var i = 0; i < node.length; i++) {
                if (node[i] && node[i].attributes) {
                    programId.push(node[i].id);
                }
            }
            var programs = programId.toString();
            $.post('../../ASHX/Permission/RoseRightSetting.ashx?M=saveprogram',
                   { RoseId: $('#hideRoseId').val(), PAList: list, Programs: programs },
                   function (result) {
                       try {
                           if (result.success) {
                               $('#tbProgram').datagrid('load');
                               $('#ProgramSetting').dialog('close');
                               $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("DataSaveSuccess")%>'); //SuccessTips 成功提示 DataSaveSuccess 數據已保存成功
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

               function RightSetting(index) {
                   var row = $('#tbRose').datagrid('getData').rows[index];
                   if (row) {
                       $('#hideRoseId').val(row.RoseId);
                       $('#RightSetting').dialog('open').dialog('setTitle', '<%=Language("RightDetail")%>');
                ProgramSearch();
                DataSearch();
                //SupplySearch();
            }
        }
        function saveRight() {
            var dataList = [];
            var supplyList = [];
            //var rows = $('#tbSupply').datagrid('getChecked');
            //for (var i = 0; i < rows.length; i++) {
            //    //每行ID放入數組中
            //    supplyList.push(rows[i].suppNumber);
            //}
            var supplys = supplyList.toString();

            var chks = $('input[name="IsDataUsy"]');
            var drow = $('#tbData').datagrid('getData').rows;
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].checked) {
                    var deptId = chks[i].id.replace("IsDataUsy", "");
                    var IsAll = 'N';
                    if ($('input[id="IsAll' + deptId + '"]')[0].checked) {
                        IsAll = 'Y';
                    }
                    dataList.push(deptId + '|' + IsAll);
                    if (drow[i].deptID != drow[i].falseDeptID) {
                        dataList.push(drow[i].falseDeptID + '|' + IsAll);
                    }
                }
            }
            var datas = dataList.toString();

            $.post('../../ASHX/Permission/RoseRightSetting.ashx?M=rightsetting',
                   { RoseId: $('#hideRoseId').val(), SupplyIdList: supplys, DataList: datas },
                   function (result) {
                       try {
                           if (result.success) {
                               $.messager.alert('<%=Language("SuccessTips")%>', '<%=Language("DataSaveSuccess")%>'); //SuccessTips 成功提示 DataSaveSuccess 數據已保存成功
                               $('#RightSetting').dialog('close'); 	// close the dialog
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
    </script>
</head>
<body>
    <form runat="server" id="form1">
        <div style="width: 98%; background-color: transparent;">
            <div id="divOperation" class="Search">
                <div class="l leftSearch">
                    <span>
                        <input type="text" id="txtKeyword" style="width: 200px;" value="<%=Language("InputDefaultKey")%>"
                            onfocus="this.value=''" onblur="if(!value){value=defaultValue;}" /></span><span><a
                                href="javascript:void(0);" <%=IsUsy("Search") %> onclick="<%=IsCanClick("Search") %>Search('ByKey')"><%=Language("Search")%></a></span><span><a
                                    href="javascript:void(0);" <%=IsUsy("AdvancedSearch") %> onclick="<%=IsCanClick("AdvancedSearch") %>openSearch('divSearch');"><%=Language("AdvancedSearch")%></a></span>
                </div>
            </div>
            <div style="clear: both">
            </div>
            <div class="squery" id="divSearch">
                <div class="sinquery">
                    <table cellpadding="0" cellspacing="0" class="addCoyTB">
                        <tr>
                            <td>
                                <%=Language("RoseId")%>:
                            </td>
                            <td>
                                <input type="text" id="sRoseId" />
                            </td>
                            <td>
                                <%=Language("RoseName")%>:
                            </td>
                            <td>
                                <input type="text" id="sRoseName" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=Language("SystemType")%>:
                            </td>
                            <td>
                                <select style="width: 90px" id="sSystemType">
                                    <option value="">ALL</option>
                                    <option value="01"><%=Language("systemuser")%></option>
                                    <option value="02"><%=Language("supplyid")%></option>
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
                <div style="text-align: right; padding: 5px 0; border: false; height: 21px; font-size: 12px;">
                    <a href="javascript:void(0)" onclick="Search('ByAdvanced')">
                        <%=Language("Search")%></a>&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
            </div>
            <table id="tbRose" width="99%" fit="false">
            </table>
        </div>
    </form>
    <div id="CopyPermission" class="easyui-dialog" data-options="closed:true,modal:true"
        style="width: 400px; height: 200px; padding: 5px;" maximizable="true" buttons="#dlg-buttonscopy">
        <form id="fmCopy" method="post" novalidate>
            <table class="tbStyle" width="100%">
                <tr>
                    <td style="width: 30%;" height="20" bgcolor="#e8f5ff">
                        <%=Language("SocureRoseId")%>：
                    </td>
                    <td style="width: 70%;">
                        <input name="RoseId" id="AimRoseId" type="text" width="150px" readonly />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#e8f5ff">
                        <%=Language("AimRoseId")%>：
                    </td>
                    <td>
                        <input name="SocureRoseId" id="SocureRoseId" />
                    </td>
                </tr>
            </table>
        </form>
    </div>
    <div id="dlg-buttonscopy">
        <a class="easyui-linkbutton" href="javascript:void(0)" onclick="CopySave()">
            <%=Language("Save")%></a> <a class="easyui-linkbutton" href="javascript:void(0)"
                onclick="closeCopyPermission()">
                <%=Language("Cancel")%></a>
    </div>
    <div style="clear: both">
    </div>
    <div id="RightSetting" class="easyui-dialog" data-options="closed:true,modal:false"
        style="width: 600px; height: 400px; padding: 5px;" buttons="#dlg-buttons" maximizable="true">
        <input type="hidden" id="hideRoseId" name="RoseId" />
        <div class="easyui-tabs" style="margin: auto; margin-top: 5px; height: 220px;" border="true"
            fit="true">
            <div title='<%=Language("ProgramSetting")%>' style="overflow: auto; padding: 5px; text-align: left;">
                <div class="easyui-layout" fit="true">
                    <div region="north" border="false" style="height: 30px;" class="p-search">
                        <div style="float: left;">
                            <input type="text" id="txtProgramKeyWord" style="width: 200px;" value="<%=Language("ProgramKey")%>"
                                onfocus="this.value=''" onblur="if(!value){value=defaultValue;}" />
                            <a href="javascript:void(0);" onclick="ProgramSearch()">
                                <%=Language("Search")%></a>
                        </div>
                        <div style="float: right;" fit="true">
                            <input type="button" value='<%=Language("SetProgram")%>' onclick="OpenProgramSetting()" />
                        </div>
                    </div>
                    <div region="center" border="false">
                        <table id="tbProgram" width="98%" fit="true">
                        </table>
                    </div>
                </div>
            </div>
            <div title='<%=Language("DataSetting")%>' style="overflow: auto; padding: 5px; text-align: left;">
                <table id="tbData" width="98%" fit="true">
                </table>
            </div>
            <%--<div title='<%=Language("SupplySetting")%>' style="overflow: auto; padding: 5px; text-align: left;display:none;">
                <table id="tbSupply" width="98%" fit="true">
                </table>
            </div>--%>
        </div>
    </div>
    <div style="clear: both">
    </div>
    <div id="dlg-buttons">
        <a class="easyui-linkbutton" href="javascript:void(0)" onclick="saveRight()">
            <%=Language("Save")%></a> <a class="easyui-linkbutton" href="javascript:void(0)"
                onclick="closeAddWindow('RightSetting')">
                <%=Language("Cancel")%></a>
    </div>
    <div style="clear: both">
    </div>
    <div id="ProgramSetting" class="easyui-dialog" data-options="closed:true,modal:true"
        style="width: 600px; height: 400px; padding: 5px;" maximizable="true">
        <div class="easyui-layout" data-options="fit:true">
            <div data-options="region:'center',border:false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                <div id="divTree" style="background-color: #fff; padding: 1px;">
                    <table border="0" cellpadding="0" cellspacing="1" bgcolor="#a8c7ce" style="width: 99%; height: 99%;">
                        <tr>
                            <td width="50%" height="20" bgcolor="#E0ECFF" style="text-align: center;">
                                <%=Language("ProgramRight")%>
                            </td>
                            <td width="50%" height="20" bgcolor="#E0ECFF" style="text-align: center;">
                                <%=Language("ActionRight")%>
                            </td>
                        </tr>
                        <tr>
                            <td style="background: #fff; vertical-align: top; height: 200px;">
                                <ul id="ProgramStatus" data-options="animate:true,dnd:false,checkbox:true">
                                </ul>
                            </td>
                            <td style="background: #fff; padding: 5px; vertical-align: top;">
                                <table id="tbAction" class="tbStyle" width="100%">
                                    <tr style="height: 26px;" id="actiontr1">
                                        <td colspan="3" bgcolor="#E0ECFF">
                                            <input type="checkbox" id="actionchkall" onclick="CheckAllAction(this)"><%=Language("CheckAll")%></input>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div data-options="region:'south',border:false" style="text-align: right; padding: 5px 0;">
                <a class="easyui-linkbutton" href="javascript:void(0)" onclick="SaveProgramSetting()">
                    <%=Language("Save")%></a> <a class="easyui-linkbutton" href="javascript:void(0)"
                        onclick="closeProgramSetting()">
                        <%=Language("Cancel")%></a>
            </div>
        </div>
    </div>
</body>
</html>
