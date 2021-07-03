<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepairAssign.aspx.cs" Inherits="Web.UI.Equment.RepairAssign" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>组长排单</title>
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.json-2.4.js"></script>
    <script src="../../easyUI15/locale/easyui-lang-zh_TW.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //綁定datagrid
            $('#tbRepairAssign').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                onClickCell: onClickCell,    //点击单元格触发的事件（重要）  
                onAfterEdit: onAfterEdit,     //编辑单元格之后触发的事件（重要） 
                //url: '../../ASHX/DMC/RepairForm.ashx?M=Search&SearchType=ByKey',
                //數據在一行顯示 
                nowrap: false,
                fitColumns: false,
                //行條紋化
                striped: true,
                //固定序號
                rownumbers: true,
                //是否可以多選
                singleSelect: true,
                remoteSort: false,
                idField: 'autoid',
                loadMsg: '数据加载中...',//數據加載中
                //是否顯示分页
                pagination: true,
                //冻结列
                frozenColumns: [[{
                    field: 'repairmanid', title: '指派维修员', width: 120, align: 'left',
                    editor: {
                        type: 'combobox',
                        options: {
                            valueField: 'repairmanid',
                            textField: 'repairmanname',
                            method: 'get',
                            url: '../../ASHX/DMC/Repairman.ashx?M=GetOnDuty',
                            //required: true,
                            onSelect: function (rec) {
                                var queryParams = $('#tbRepairAssign').datagrid('options').queryParams;
                                if (queryParams.DType != "1") {
                                    $.messager.defaults = { ok: '确认', cancel: '取消', width: 300 };
                                    if ($("#auserid").textbox("getValue") == "") {
                                        Refersh();
                                        $.messager.alert({ title: '消息提示', msg: '请设置操作员' });
                                        return;
                                    }
                                    //检查是否有操作权限
                                    $.post("../../ASHX/Permission/UserRightManage.ashx?M=programright",
                                           { opuser: $("#auserid").textbox("getValue"), ProgramId: "eqwi004" },
                                           function (result) {
                                               if (result.success) {
                                                   //有权限则提交
                                                   $.messager.confirm('指派确认', '你確定需要指派给' + rec.repairmanname + '吗?', function (r) {
                                                       if (r) {
                                                           $.post("../../ASHX/DMC/RepairForm.ashx?M=RepairAssign",
                                                               { RepairFormNO: repairformno, AssignUser: rec.repairmanid, AssignUserFullname: rec.repairmanname, FormStatus: formstatus, opuser: $("#auserid").textbox("getValue") },
                                                                    function (result) {
                                                                        if (result.success) {
                                                                            repairformno = "";
                                                                            formstatus = "";
                                                                            editIndex = undefined;
                                                                            Refersh();
                                                                            $.messager.alert({ title: '成功提示', msg: '指派成功' });
                                                                        } else {
                                                                            $.messager.alert({
                                                                                title: '错误提示',
                                                                                msg: result.msg
                                                                            });
                                                                            Refersh();
                                                                        }
                                                                    },
                                                                    'json');
                                                       }
                                                   });
                                               } else {
                                                   $.messager.alert({
                                                       title: '错误提示',
                                                       msg: result.msg
                                                   });
                                                   Refersh();
                                               }
                                           },
                                           'json');
                                }
                            }
                        }
                    }
                }
                ]],
                //可動列
                columns: [[
                           { field: 'deviceid', title: '设备编号', width: 100, align: 'left' },
                           { field: 'positiontext', title: '故障位置', width: 100, align: 'left' },
                           { field: 'phenomenatext', title: '故障现象', width: 100, align: 'left' },
                           { field: 'rebackreason', title: '重排原因', width: 150, align: 'left' },
                           { field: 'faulttime', title: '故障时间', width: 150, align: 'left' },
                           { field: 'ipqcnumber', title: 'IPQC工号', width: 100, align: 'left' },
                           { field: 'mouldid', title: '模具编号', width: 70, align: 'left' },
                           { field: 'newmouldid', title: '新模编号', width: 70, align: 'left' },
                           { field: 'rebackreason', title: '返修原因', width: 70, align: 'left' },
                           { field: 'positiontext1', title: '故障位置1', width: 100, align: 'left' },
                    { field: 'phenomenatext1', title: '故障现象1', width: 100, align: 'left' },
                    { field: 'repairformno', title: '维修单号', width: 120, align: 'left' }
                ]]
            });
            $(window).resize(function () {
                $('#tbRepairAssign').datagrid('resize');
            });
            //設置分页控件屬性 
            var p = $('#tbRepairAssign').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: [10, 15, 20, 25, 30],
                beforePageText: '第',
                afterPageText: '页 共{pages}页',
                displayMsg: '当前显示 {from} - {to} 条记录 共 {total} 条记录'
            });
        });
        var editIndex = undefined;
        var repairformno = "";
        var formstatus = "";
        //判断是否编辑结束  
        function endEditing() {
            var queryParams = $('#tbRepairAssign').datagrid('options').queryParams;
            if (queryParams.DType != "1") {
                if (editIndex == undefined) { return true }
                if ($('#tbRepairAssign').datagrid('validateRow', editIndex)) {
                    $('#tbRepairAssign').datagrid('endEdit', editIndex);
                    editIndex = undefined;
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }

        //点击单元格触发的事件  
        function onClickCell(index, field) {

            if (endEditing()) {
                $('#tbRepairAssign').datagrid('selectRow', index).datagrid('beginEdit', index);
                var row = $('#tbRepairAssign').datagrid('getData').rows[index];
                repairformno = row.repairformno;
                formstatus = row.formstatus;
                //var assignuser = $('#tbRepairAssign').datagrid('getEditor', { index: index, field: 'assignuser' });
                //绑定下拉框<a href="../../ASHX/DMC/Repairman.ashx">../../ASHX/DMC/Repairman.ashx</a>
                //$(assignuser.target).combobox({
                //    panelWidth: 430,
                //    mode: 'remote',
                //    url: '../../ASHX/DMC/Repairman.ashx?M=GetOnDuty',
                //    idField: 'repairmanid',
                //    textField: 'repairmanname',
                //    required: true,

                //});
                editIndex = index;
            }
        }

        //编辑完单元格之后触发的事件  
        function onAfterEdit(index, row, changes) {
            $('#tbRepairAssign').datagrid('updateRow', { index: $('#tbEqManage').datagrid('getRowIndex', row) });
        }

        //單擊高級查詢按鈕
        function openSearch(obj) {
            var obj = $("#" + obj + "");
            if (obj.css('display') == "block") {
                obj.hide();
                $("#btnMore").linkbutton({
                    iconCls: 'icon-expand'
                });
            }
            else {
                obj.show();
                $("#btnMore").linkbutton({
                    iconCls: 'icon-collapse'
                });
            }
        }

        //取消(關閉dialog)
        function closeWindow(id) {
            $('#' + id).dialog('close')
        }

        //Grid初使加載的數據
        var type = 0;
        function Search(DType) {
            var queryParams = $('#tbRepairAssign').datagrid('options').queryParams;
            //請輸入關鍵字            
            //queryParams.KeyWord = $('#txtKeyword').textbox("getValue");
            //queryParams.SearchType = SearchType;            
            $('#tbRepairAssign').datagrid('options').url = '../../ASHX/DMC/RepairForm.ashx?M=Search&SearchType=ByKey';
            queryParams.DType = DType;
            type = DType;
            $('#tbRepairAssign').datagrid('reload');
        }

        function Refersh() {
            $.post("../../ASHX/DMC/RepairForm.ashx?M=GetAssignQty",
                {},
                function (data) {
                    if (data.length > 0) {
                        $("#Wait").html(data[0].waitqty);
                        $("#Reback").html(data[0].rebackqty);
                        $("#Change").html(data[0].changeqty);
                    } else {
                        $("#Wait").html(0);
                        $("#Reback").html(0);
                        $("#Change").html(0);
                    }

                    //   Search(type);
                    if (type == 0) {
                        $("#Wait").click();
                    }
                    else if (type == 1) {
                        $("#Reback").click();
                    }
                    else {
                        $("#Change").click();
                    }
                },
                'json');
        }

        $(function () {//间隔60s自动加载一次   
            Refersh(); //首次立即加载   
            window.setInterval(Refersh, 1 * 60 * 1000); //循环执行！！   
        }
            );
    </script>
    <style type="text/css">
        #opter .l-btn {
            color: red;
            color: #fff;
            border-color: rgb(230, 126, 34);
            background: rgb(230, 126, 34);
            background: -webkit-linear-gradient(top,rgb(230, 126, 34) 0,rgb(230, 126, 34) 100%);
            background: -moz-linear-gradient(top,rgb(230, 126, 34) 0,rgb(230, 126, 34) 100%);
            background: -o-linear-gradient(top,rgb(230, 126, 34) 0,rgb(230, 126, 34) 100%);
            background: linear-gradient(to bottom,rgb(230, 126, 34) 0,rgb(230, 126, 34) 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=rgb(230, 126, 34),endColorstr=rgb(230, 126, 34),GradientType=0);
        }

        #opter .l-btn-selected, .l-btn-selected:hover {
            background: #4cae4c;
            color: #fff;
            border: 0px;
        }
    </style>
</head>
<body>
    <div id="funMain" style="background-color: transparent; margin-left: 5px; margin-right: 0px;">
        <div id="divOperation" class="Search">
            <div class="l leftSearch" id="opter">
                <a class="easyui-linkbutton" data-options="toggle:true,group:'g1',selected:true" onclick="Search(0)">待排单(<span id="Wait">0</span>)</a>&nbsp;&nbsp;
               
                <a class="easyui-linkbutton" data-options="toggle:true,group:'g1'" onclick="Search(1)">返修排单(<span id="Reback">0</span>)</a>&nbsp;&nbsp;
               
                <a class="easyui-linkbutton" data-options="toggle:true,group:'g1'" onclick="Search(2)">交接排单(<span id="Change">0</span>)</a>
                操作人：<input data-options="prompt:'请输入操作人工号',required: true" class="easyui-textbox" id="auserid" style="width: 150px;" />
                <%-- <input class="easyui-textbox" data-options="prompt:'请输入关键字'" id="txtKeyword" style="width: 200px;" />
                <a class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true" onclick="Search()">搜索</a>
                <a id="btnMore" class="easyui-linkbutton" data-options="iconCls:'icon-expand',plain:true" onclick="openSearch('divSearch');">高级搜索</a>--%>
            </div>
            <div class="r rightSearch">
                <%--<a class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true" href="javascript:void(0)" onclick='RepairAssign()'>指派</a> &nbsp;&nbsp;--%>
            </div>
        </div>
        <div style="clear: both">
        </div>
        <div style="clear: both">
        </div>
        <table id="tbRepairAssign" data-options="fit:false">
        </table>
    </div>
    <div style="clear: both">
    </div>
</body>
</html>
