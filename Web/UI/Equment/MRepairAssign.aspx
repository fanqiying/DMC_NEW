<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MRepairAssign.aspx.cs" Inherits="Web.UI.Equment.MRepairAssign" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="viewport" content="initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <title>组长排单</title>
    <link rel="stylesheet" type="text/css" href="../../easyUI15/themes/metro/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../../easyUI15/themes/mobile.css" />
    <link rel="stylesheet" type="text/css" href="../../easyUI15/themes/icon.css" />
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../easyUI15/jquery.easyui.mobile.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //綁定datagrid
            $('#tbRepairAssign').datagrid({
                header: '#hh',
                onClickCell: onClickCell,    //点击单元格触发的事件（重要）  
                onAfterEdit: onAfterEdit,     //编辑单元格之后触发的事件（重要） 
                //url: '../../ASHX/DMC/RepairForm.ashx?M=Search&SearchType=ByKey&FormStatus=0',
                fitColumns: false,
                border: true,
                //固定序號
                rownumbers: false,
                scrollbarSize: 2,
                //是否可以多選
                singleSelect: true,
                remoteSort: false,
                idField: 'autoid',
                loadMsg: '数据加载中...',//數據加載中
                //是否顯示分页
                pagination: true,
                //冻结列
                frozenColumns: [[{
                    field: 'repairmanid', title: '指派维修员', width: 100, align: 'left',
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
                                    $.messager.confirm('指派确认', '你確定需要指派给' + rec.repairmanname + '吗?', function (r) {
                                        if (r) {
                                            $.post("../../ASHX/DMC/RepairForm.ashx?M=RepairAssign",
                                                   { RepairFormNO: repairformno, AssignUser: rec.repairmanid },
                                                     function (result) {
                                                         if (result.success) {
                                                             repairformno = "";
                                                             editIndex = undefined;
                                                             Refersh();
                                                             $.messager.alert({ title: '成功提示', msg: '指派成功' });
                                                         } else {
                                                             $.messager.alert({
                                                                 title: '错误提示',
                                                                 msg: result.msg
                                                             });
                                                         }
                                                     },
                                                     'json');
                                        }
                                    });
                                }
                            }
                        }
                    }
                }]],
                //可動列
                columns: [[
                           { field: 'deviceid', title: '设备编号', width: 100, align: 'left' },
                           { field: 'positiontext', title: '故障位置', width: 100, align: 'left' },
                           { field: 'phenomenatext', title: '故障现象', width: 100, align: 'left' },
                           { field: 'positiontext1', title: '故障位置1', width: 100, align: 'left' },
                           { field: 'phenomenatext1', title: '故障现象1', width: 100, align: 'left' },
                           { field: 'rebackreason', title: '重排原因', width: 150, align: 'left' },
                           { field: 'faulttime', title: '故障时间', width: 150, align: 'left' },
                           { field: 'ipqcnumber', title: 'IPQC工号', width: 100, align: 'left' },
                           


                ]]
            });
            //$(window).resize(function () {
            //    $('#tbRepairAssign').datagrid('resize');
            //});
            ////設置分页控件屬性 
            var p = $('#tbRepairAssign').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: [10, 15, 20, 25, 30],
                beforePageText: '第',
                afterPageText: '页 共{pages}页',
                displayMsg: '当前显示 {from} - {to} 条记录 共 {total} 条记录'
            });

            $('#txtKeyword').textbox({
                icons: [{
                    iconCls: 'icon-search',
                    handler: function (e) {
                        Search();
                    }
                }]
            })
        });
        var editIndex = undefined;
        var repairformno = "";
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
        //Grid初使加載的數據
        function Search(DType) {
            var queryParams = $('#tbRepairAssign').datagrid('options').queryParams;
            $('#tbRepairAssign').datagrid('options').url = '../../ASHX/DMC/RepairForm.ashx?M=Search&SearchType=ByKey';
            //請輸入關鍵字            
            //queryParams.KeyWord = $('#txtKeyword').textbox("getValue");
            queryParams.DType = DType;
            //queryParams.SearchType = SearchType;
            //queryParams.FormStatus = 0;
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
                   // Search(0);
                    $("#Wait").click();
                },
                'json');
        }
        var lastTimeId = "";
        $(function () {//间隔300s自动加载一次   
            Refersh(); //首次立即加载   
            if (!!lastTimeId) {
                window.clearInterval(lastTimeId);
            }
            lastTimeId = window.setInterval(Refersh, 1 * 300 * 1000); //循环执行！！   
        });
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
    <table id="tbRepairAssign" data-options="fit:true">
    </table>
    <div id="hh">
        <div class="m-toolbar" id="opter">
            <div class="m-title">组长排单</div>
            <a class="easyui-linkbutton" data-options="toggle:true,group:'g1',selected:true" onclick="Search(0)">待排单(<span id="Wait">0</span>)</a>&nbsp;&nbsp;
            <a class="easyui-linkbutton" data-options="toggle:true,group:'g1'" onclick="Search(1)">返修排单(<span id="Reback">0</span>)</a>&nbsp;&nbsp;
            <a class="easyui-linkbutton" data-options="toggle:true,group:'g1'" onclick="Search(2)">交接排单(<span id="Change">0</span>)</a>
            <%--<a class="easyui-linkbutton" style="width: 20%;" data-options="iconCls:'icon-search',plain:true" onclick="Search()">搜索</a>--%>
        </div>
    </div>
</body>
</html>
