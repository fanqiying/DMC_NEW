<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepairForm.aspx.cs" Inherits="Web.UI.Equment.RepairForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>生产报修</title>
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="../../easyUI15/locale/easyui-lang-zh_TW.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#aapplyuserid').combogrid({
                //是否折叠
                required: false,
                panelWidth: 430,
                //toolbar: '#toolbarleader',
                url: '../../ASHX/Organize/EmpManage.ashx?M=SearchGroup&SearchType=ByKey&rows=10000&page=1',
                idField: 'empid',
                textField: 'disaplytext',
                pagination: false,//是否分页
                rownumbers: true,//序号 
                method: 'post',
                fitColumns: true,
                fit: true,
                columns: [[
                    {
                        field: 'empid', title: '用户信息', width: 70, align: 'left', formatter: function (value, row, index) {
                            return row.empname + "(" + row.empid + "/" + row.empmail + ")";
                        }
                    }
                ]],
                keyHandler: {
                    enter: function () {
                        var pClosed = $("#aapplyuserid").combogrid("panel").panel("options").closed;
                        if (!pClosed) {
                            $("#aapplyuserid").combogrid("hidePanel");
                        }
                        var grid = $('#aapplyuserid').combogrid("grid");
                        var record = grid.datagrid("getSelected");
                        if (record == null || record == undefined) {
                            //如果未选中，则匹配当前回车选择的第一个
                            var rows = grid.datagrid("getRows");
                            if (rows.length > 0) {
                                //record = rows[0];
                                var rowIndex = grid.datagrid("getRowIndex", rows[0]);
                                //setPatient(record);
                                grid.datagrid("selectRow", rowIndex);
                            }
                            return;
                        }
                        else {
                            var rowIndex = grid.datagrid("getRowIndex", record);
                            grid.datagrid("selectRow", rowIndex);
                            //setPatient(record);
                        }
                    },
                    up: function () {
                        var pClosed = $("#aapplyuserid").combogrid("panel").panel("options").closed;
                        if (pClosed) {
                            $("#aapplyuserid").combogrid("showPanel");
                        }
                        var grid = $('#aapplyuserid').combogrid("grid");
                        var rowSelected = grid.datagrid("getSelected");
                        if (rowSelected != null) {
                            var rowIndex = grid.datagrid("getRowIndex", rowSelected);
                            if (rowIndex > 0) {
                                rowIndex = rowIndex - 1;
                                grid.datagrid("selectRow", rowIndex);
                            }
                        } else if (grid.datagrid("getRows").length > 0) {
                            grid.datagrid("selectRow", 0);
                        }
                    },
                    down: function () {
                        var pClosed = $("#aapplyuserid").combogrid("panel").panel("options").closed;
                        if (pClosed) {
                            $("#aapplyuserid").combogrid("showPanel");
                        }
                        var grid = $('#aapplyuserid').combogrid("grid");
                        var rowSelected = grid.datagrid("getSelected");
                        if (rowSelected != null) {
                            var totalRow = grid.datagrid("getRows").length;
                            var rowIndex = grid.datagrid("getRowIndex", rowSelected);
                            if (rowIndex < totalRow - 1) {
                                rowIndex = rowIndex + 1;
                                grid.datagrid("selectRow", rowIndex);
                            }
                        } else if (grid.datagrid("getRows").length > 0) {
                            grid.datagrid("selectRow", 0);
                        }
                    },
                    query: function (q) {
                        //$('#adeviceid').combogrid("setValue", null);
                        $('#aapplyuserid').combogrid("grid").datagrid("clearSelections");
                        $('#aapplyuserid').combogrid("grid").datagrid("reload", {
                            'KeyWord': q,
                            'sid2': Math.round(Math.random() * 1000)
                        });
                        $('#aapplyuserid').combogrid("grid").datagrid({
                            onLoadSuccess: function (data) {
                                $('#aapplyuserid').combogrid("setText", q);
                            }
                        });
                    }
                }
            });
            $('#adeviceid').combogrid({
                //是否折叠
                required: false,
                panelWidth: 430,
                //toolbar: '#toolbardeviceid',
                url: '../../ASHX/DMC/DeviceManage.ashx?M=Search&SearchType=ByKey&rows=10000&page=1',
                idField: 'deviceid',
                textField: 'deviceid',
                mode: 'remote',
                pagination: false,//是否分页
                rownumbers: true,//序号 
                method: 'post',
                fitColumns: true,
                fit: true,
                columns: [[
                    {
                        field: 'deviceid', title: '设备编号', width: 70, align: 'left', formatter: function (value, row, index) {
                            return row.deviceid + "(" + row.devicename + ")";
                        }
                    }
                ]],
                onSelect: function (rowIndex, rowData) {
                    LoadHisRecord();
                },
                keyHandler: {
                    enter: function () {
                        var pClosed = $("#adeviceid").combogrid("panel").panel("options").closed;
                        if (!pClosed) {
                            $("#adeviceid").combogrid("hidePanel");
                        }
                        var grid = $('#adeviceid').combogrid("grid");
                        var record = grid.datagrid("getSelected");
                        if (record == null || record == undefined) {
                            //如果未选中，则匹配当前回车选择的第一个
                            var rows = grid.datagrid("getRows");
                            if (rows.length > 0) {
                                //record = rows[0];
                                var rowIndex = grid.datagrid("getRowIndex", rows[0]);
                                //setPatient(record);
                                grid.datagrid("selectRow", rowIndex);
                            }
                            return;
                        }
                        else {
                            var rowIndex = grid.datagrid("getRowIndex", record);
                            grid.datagrid("selectRow", rowIndex);
                            //setPatient(record);
                        }
                    },
                    up: function () {
                        var pClosed = $("#adeviceid").combogrid("panel").panel("options").closed;
                        if (pClosed) {
                            $("#adeviceid").combogrid("showPanel");
                        }
                        var grid = $('#adeviceid').combogrid("grid");
                        var rowSelected = grid.datagrid("getSelected");
                        if (rowSelected != null) {
                            var rowIndex = grid.datagrid("getRowIndex", rowSelected);
                            if (rowIndex > 0) {
                                rowIndex = rowIndex - 1;
                                grid.datagrid("selectRow", rowIndex);
                            }
                        } else if (grid.datagrid("getRows").length > 0) {
                            grid.datagrid("selectRow", 0);
                        }
                    },
                    down: function () {
                        var pClosed = $("#adeviceid").combogrid("panel").panel("options").closed;
                        if (pClosed) {
                            $("#adeviceid").combogrid("showPanel");
                        }
                        var grid = $('#adeviceid').combogrid("grid");
                        var rowSelected = grid.datagrid("getSelected");
                        if (rowSelected != null) {
                            var totalRow = grid.datagrid("getRows").length;
                            var rowIndex = grid.datagrid("getRowIndex", rowSelected);
                            if (rowIndex < totalRow - 1) {
                                rowIndex = rowIndex + 1;
                                grid.datagrid("selectRow", rowIndex);
                            }
                        } else if (grid.datagrid("getRows").length > 0) {
                            grid.datagrid("selectRow", 0);
                        }
                    },
                    query: function (q) {
                        //$('#adeviceid').combogrid("setValue", null);
                        $('#adeviceid').combogrid("grid").datagrid("clearSelections");
                        $('#adeviceid').combogrid("grid").datagrid("reload", {
                            'KeyWord': q,
                            'sid2': Math.round(Math.random() * 1000)
                        });
                        $('#adeviceid').combogrid("grid").datagrid({
                            onLoadSuccess: function (data) {
                                $('#adeviceid').combogrid("setText", q);
                            }
                        });
                    }
                }
            });
            //加載下拉框的故障位置
            $('#apositionid').combobox({
                url: '../../ASHX/DMC/FaultPosition.ashx?M=GetFaultPositionMain',
                valueField: 'positionid',
                textField: 'positionname',
                panelHeight: '150',
                editable: false,
                multiple: false,
                onChange: function (newValue, oldValue) {
                    $('#aphenomenaid').combobox("loadData", []);
                    if (newValue != "") {
                        $.post("../../ASHX/DMC/FaultPosition.ashx?M=GetFaultPositionNode", {
                            PPositionId: newValue
                        },
                           function (result) {
                               $('#aphenomenaid').combobox("loadData", result);
                               if (result.length == 0) {
                                   $('#aphenomenaid').combobox('select', "");
                               }
                               else {
                                   $('#aphenomenaid').combobox('select', result[0].positionid);
                               }
                           },
                           'json');
                    }
                }
            });
            //加載下拉框的故障现象
            $('#aphenomenaid').combobox({
                //url: '../../ASHX/DMC/FaultPosition.ashx?M=GetFaultPositionMain',
                valueField: 'positionid',
                textField: 'positionname',
                panelHeight: '150',
                editable: false,
                multiple: false,
                onChange: function (newValue, oldValue) {
                    LoadHisRecord();
                }
            });
            //加載下拉框的故障位置
            $('#apositionid1').combobox({
                url: '../../ASHX/DMC/FaultPosition.ashx?M=GetFaultPositionMain',
                valueField: 'positionid',
                textField: 'positionname',
                panelHeight: '150',
                editable: false,
                multiple: false,
                onChange: function (newValue, oldValue) {
                    $('#aphenomenaid1').combobox("loadData", []);
                    if (newValue != "") {
                        $.post("../../ASHX/DMC/FaultPosition.ashx?M=GetFaultPositionNode", {
                            PPositionId: newValue
                        },
                           function (result) {
                               $('#aphenomenaid1').combobox("loadData", result);
                               if (result.length == 0) {
                                   $('#aphenomenaid1').combobox('select', "");
                               }
                               else {
                                   $('#aphenomenaid1').combobox('select', result[0].positionid);
                               }
                           },
                           'json');
                    }
                }
            });
            //加載下拉框的故障现象
            $('#aphenomenaid1').combobox({
                //url: '../../ASHX/DMC/FaultPosition.ashx?M=GetFaultPositionMain',
                valueField: 'positionid',
                textField: 'positionname',
                panelHeight: '150',
                editable: false,
                multiple: false
            });
            //綁定datagrid
            $('#tbRepairForm').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: '../../ASHX/DMC/RepairForm.ashx?M=Search&SearchType=ByKey&NoFormStatus=6',
                //數據在一行顯示 
                nowrap: false,
                fitColumns: true,
                //行條紋化
                striped: true,
                //固定序號
                rownumbers: true,
                //是否可以多選
                singleSelect: true,
                remoteSort: false,
                idField: 'autoid',
                loadMsg: '數據加載中...',//數據加載中
                //是否顯示分页
                pagination: true,
                //可動列
                columns: [[
                           { field: 'repairformno', title: '维修单号', width: 70, align: 'left' },
                           { field: 'applyuserid', title: '申请人', width: 50, align: 'left' },
                           {
                               field: 'formstatus', title: '状态', width: 50, align: 'left',
                               formatter: function (value, row, index) {
                                   var text = "N/A";
                                   switch (row.formstatus) {
                                       case "10":
                                           text = "10-待指派";
                                           break;
                                       case "12":
                                           text = "12-待指派(挂单)";
                                           break;
                                       case "24":
                                           text = "24-待维修(IPQC返修)";
                                           break;
                                       case "25":
                                           text = "25-待维修(组长返修)";
                                           break;
                                       case "20":
                                           text = "20-待维修";
                                           break;
                                       case "23":
                                           text = "23-待维修(返修)";
                                           break;
                                       case "30":
                                           text = "30-待生产员确认";
                                           break;
                                       case "40":
                                           text = "40-待IPQC确认";
                                           break;
                                       case "50":
                                           text = "50-待组长确认";
                                           break;
                                       //case "60":
                                       //    text = "60-已完成";
                                       //    break;
                                   }
                                   return text;
                               }
                           },
                           { field: 'deviceid', title: '设备编号', width: 70, align: 'left' },
                           { field: 'positiontext', title: '故障位置', width: 70, align: 'left' },
                           { field: 'phenomenatext', title: '故障现象', width: 70, align: 'left' },
                           { field: 'faulttime', title: '故障时间', width: 70, align: 'left' },
                           { field: 'intime', title: '报修时间', width: 70, align: 'left' },
                           { field: 'positiontext1', title: '故障位置1', width: 70, align: 'left' },
                           { field: 'phenomenatext1', title: '故障现象1', width: 70, align: 'left' },
                           //{ field: 'assigntime', title: '指派时间', width: 70, align: 'left' },
                           //{ field: 'assignuser', title: '维修员', width: 70, align: 'left' },
                           //{ field: 'repairtime', title: '维修时间', width: 70, align: 'left' },
                           //{ field: 'repairedtime', title: '维修完成时间', width: 70, align: 'left' },
                           { field: 'confirmtime', title: '确认时间', width: 70, align: 'left' },
                           {
                               field: 'opt', title: '操作', width: 70, align: 'left',
                               formatter: function (value, row, index) {
                                   return '<a href="#" onclick="View(\'' + index + '\')">查看</a>';
                               }
                           }
                ]]
            });
            $(window).resize(function () {
                $('#tbRepairForm').datagrid('resize');
            });
            //設置分页控件屬性 
            var p = $('#tbRepairForm').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: [10, 15, 20, 25, 30],
                beforePageText: '第',
                afterPageText: '页 共{pages}页',
                displayMsg: '当前显示 {from} - {to} 条记录 共 {total} 条记录'
            });

            //綁定datagrid
            $('#tbRepairInfo').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                //url: '../../ASHX/DMC/RepairForm.ashx?M=Search',
                //數據在一行顯示 
                nowrap: false,
                fitColumns: true,
                //行條紋化
                striped: true,
                //固定序號
                rownumbers: true,
                //是否可以多選
                singleSelect: true,
                remoteSort: false,
                idField: 'autoid',
                loadMsg: '數據加載中...',//數據加載中
                //是否顯示分页
                pagination: true,
                //可動列
                columns: [[{ field: 'assignuser', title: '维修员', width: 70, align: 'left' },
                           { field: 'assigntime', title: '指派时间', width: 70, align: 'left' },
                           { field: 'repairtime', title: '完成时间', width: 70, align: 'left' },
                           { field: 'repairedtime', title: '维修工时', width: 70, align: 'left' }
                ]]
            });
            //設置分页控件屬性 
            var p1 = $('#tbRepairInfo').datagrid('getPager');
            $(p1).pagination({
                pageSize: 10,
                pageList: [10, 15, 20, 25, 30],
                beforePageText: '第',
                afterPageText: '页 共{pages}页',
                displayMsg: '当前显示 {from} - {to} 条记录 共 {total} 条记录'
            });
            $('#aapplyuserid').combogrid("setValue", '<%= UserId%>');
            $('#afaulttime').datetimebox("setValue", datetimeFormatter(new Date()));
            $("#afaultcode").textbox('textbox').bind('keyup', function (e) {
                $("#afaultcode").textbox('setValue', $(this).val().replace(/\D/g, ''));
                if ($(this).val().length > 3) {
                    $("#afaultcode").textbox('setValue', $(this).val().substring(0, 3));
                }
            });
        });
        function LoadHisRecord() {
            //设备
            var deviceid = $("#adeviceid").combobox("getValue");
            //位置
            var positionid = $('#apositionid').combobox("getValue");
            //现象
            var phenomenaid = $('#aphenomenaid').combobox("getValue");
            if (deviceid != "" && positionid != "" && phenomenaid != "") {
                $.post("../../ASHX/DMC/RepairForm.ashx?M=HisRecord",
                       { DeviceId: deviceid, PositionId: positionid, PhenomenaId: phenomenaid },
                       function (data) {
                           var a = [];
                           if (data.length > 0 && data[0].faultanalysis != "") {
                               $("#afaultreason").textbox("setValue", data[0].faultanalysis);
                               $("#afaultreason").textbox("disable");
                           } else {
                               $("#afaultreason").textbox("enable");
                           }
                       },
                       'json');
            } else {
                $("#afaultreason").textbox("enable");
            }
        }
        //负责人搜索
        function searchUser() {
            var queryParams = $('#aapplyuserid').combogrid('grid').datagrid('options').queryParams;
            //請輸入關鍵字            
            queryParams.KeyWord = $('#txtleader').textbox("getValue");
            queryParams.SearchType = 'ByKey';
            $('#aapplyuserid').combogrid('grid').datagrid('reload');
        }
        //设备搜索
        function searchDeviceid() {
            var queryParams = $('#adeviceid').combogrid('grid').datagrid('options').queryParams;
            //請輸入關鍵字            
            queryParams.KeyWord = $('#txtdeviceid').textbox("getValue");
            //queryParams.SearchType = 'ByKey';
            $('#adeviceid').combogrid('grid').datagrid('reload');
        }
        //Grid初使加載的數據
        function Search(SearchType) {
            var queryParams = $('#tbRepairForm').datagrid('options').queryParams;
            //請輸入關鍵字            
            queryParams.KeyWord = $('#txtKeyword').textbox("getValue");
            //queryParams.SearchType = SearchType;
            //queryParams.EqumentId = $("#qequmentid").textbox("getValue");
            //queryParams.OnOff = $('#qonoff').combobox("getValue");
            $('#tbRepairForm').datagrid('reload');
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

        //查看
        function View(index) {
            $('#fmView').form('clear');
            var row = $('#tbRepairForm').datagrid('getData').rows[index];
            $('#fmView').form('load', row);
            $('#divEvaluate').dialog('open').dialog('setTitle', '报修明细');
        }

        function Save() {
            var msg = "";
            if ($("#aapplyuserid").combogrid("getValue") == "") {
                $.messager.alert({ title: '错误提示', msg: "请设置申请人" });
                return;
            }
            if ($("#adeviceid").textbox("getValue") == "") {
                $.messager.alert({ title: '错误提示', msg: "请设置故障设备" });
                return;
            }
            if ($("#afaulttime").datetimebox("getValue") == "") {
                $.messager.alert({ title: '错误提示', msg: "请设置故障时间" });
                return;
            }
            /*
            //获取当前combotree的tree对象
            var treeposition = $('#apositionid').combotree('tree');
            //获取当前选中的节点
            var nodeposition = treeposition.tree('getSelected');
            var PositionId = "";
            var PositionText = "";
            if (nodeposition != null) {
                PositionId = nodeposition.id;
                PositionText = nodeposition.attributes.positiontext;
            }
            //获取当前combotree的tree对象
            var treephenomena = $('#aphenomenaid').combotree('tree');
            //获取当前选中的节点
            var nodephenomena = treephenomena.tree('getSelected');
            var PhenomenaId = "";
            var PhenomenaText = "";
            if (nodephenomena != null) {
                PhenomenaId = nodephenomena.id;
                PhenomenaText = nodephenomena.attributes.categorytext;
            }
            */
            //故障代码由两部分组成
            //数据提交
            $.post("../../ASHX/DMC/RepairForm.ashx?M=NewRepairForm", {
                ApplyUserId: $("#aapplyuserid").combogrid("getValue"),
                DeviceId: $("#adeviceid").combogrid("getValue"),
                FaultTime: $("#afaulttime").datetimebox("getValue"),
                FaultCode: $("#afaulttype").combobox("getValue") + $("#afaultcode").textbox("getValue"),
                FaultReason: $("#afaultreason").textbox("getValue"),
                //FaultStatus: $("#afaultstatus").combobox("getValue"),
                FaultStatus: "",
                PositionText: $('#apositionid').combobox('getText'),
                PositionId: $('#apositionid').combobox('getValue'),
                PhenomenaId: $('#aphenomenaid').combobox('getValue'),
                PhenomenaText: $('#aphenomenaid').combobox('getText'),
                PositionId1: $('#apositionid1').combobox('getValue'),
                PhenomenaId1: $('#aphenomenaid1').combobox('getValue'),
                PositionText1: $('#apositionid1').combobox('getText'),
                PhenomenaText1: $('#aphenomenaid1').combobox('getText')
            },
            function (result) {
                if (result.success) {
                    $('#fm').form('clear');
                    $('#tbRepairForm').datagrid('reload');
                    $.messager.alert({ title: '成功提示', msg: '维修单生成成功' });
                    $('#aapplyuserid').combogrid("setValue", '<%= UserId%>');
                    $('#afaulttime').datetimebox("setValue", datetimeFormatter(new Date()));
                    $('#afaulttype').combobox("setValue", "NA");
                } else {
                    $.messager.alert({
                        title: '错误提示',
                        msg: result.msg
                    });
                }
            },
            'json');
        }
        /**
         * 时间格式化
         * @param value
         * @returns {string}
        */
        function datetimeFormatter(value) {
            var date;
            if (value == "") {
                date = new Date();
            }
            else {
                date = new Date(value);
            }
            var year = date.getFullYear().toString();
            var month = date.getMonth() + 1;
            var day = date.getDate();
            var hour = date.getHours();
            var minutes = date.getMinutes();
            var seconds = date.getSeconds();
            month = month < 10 ? '0' + month : month;
            day = day < 10 ? '0' + day : day;
            hour = hour < 10 ? '0' + hour : hour;
            minutes = minutes < 10 ? '0' + minutes : minutes;
            seconds = seconds < 10 ? '0' + seconds : seconds;
            return year + "-" + month + "-" + day + " " + hour + ":" + minutes + ":" + seconds;
        }
        /**
         * 解析时间
         */
        function datetimeParser(s) {
            if (s != "") {
                var dt = s.split(" ");
                var d = dt[0].split("-");
                var t = dt[1].split(":");
                var y = parseInt(d[0], 10);
                var m = parseInt(d[1], 10) - 1;
                var d = parseInt(d[2], 10);
                var hh = parseInt(t[0], 10);
                var mm = parseInt(t[1], 10);
                var ss = parseInt(t[2], 10);
                return new Date(y, m, d, hh, mm, ss);
            } else {
                return new Date();
            }
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 100%;
        }
    </style>
</head>
<body style="width: 100%; height: 100%;">
    <div id="funMain" style="margin-left: 5px; margin-right: 0px; " class="auto-style1">
        <div id="divOperation" class="Search" style="display: none;">
            <div class="l leftSearch">
                <input class="easyui-textbox" data-options="prompt:'请输入关键字'" id="txtKeyword" style="width: 200px;" />
                <a class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true" onclick="Search('ByKey')">搜索</a>
                <a id="btnMore" class="easyui-linkbutton" data-options="iconCls:'icon-expand',plain:true" onclick="openSearch('divSearch');">高级搜索</a>
            </div>
            <div class="r rightSearch">
                &nbsp;&nbsp;
                <%--<a class="easyui-linkbutton" data-options="iconCls:'icon-scan',plain:true" href="javascript:void(0)" onclick='add()'>扫码报修</a>--%> &nbsp;&nbsp;                
            </div>
        </div>
        <div style="clear: both">
        </div>
        <div class="squery" id="divSearch" style="display: block;">
            <div class="sinquery">
                <form id="fm" method="post" novalidate="novalidate">
                    <table class="addCoyTB" style="height: 30px; border: 0px;">
                        <colgroup>
                            <col span="1" style="width: 70px;" />
                            <col span="1" style="width: 195px" />
                            <col span="1" style="width: 70px;" />
                            <col span="1" style="width: 195px" />
                            <col span="1" style="width: 70px;" />
                            <col span="1" style="width: 195px" />
                            <col span="1" style="width: 70px;" />
                            <col span="1" style="width: 195px" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <td style="height: 25px;">申请人:</td>
                                <td>
                                    <input data-options="prompt:'请填写申请人'" class="easyui-combogrid" name="applyuserid" id="aapplyuserid" style="width: 100px;" />
                                </td>
                                <td style="height: 25px;">故障设备:</td>
                                <td>
                                    <input data-options="prompt:'请填入设备编号'" class="easyui-textbox" id="adeviceid" name="deviceid" style="width: 100px;" />
                                </td>
                                <td>故障时间:</td>
                                <td>
                                    <input class="easyui-datetimebox" id="afaulttime" name="faulttime" style="width: 160px;" data-options="formatter: datetimeFormatter, parser: datetimeParser" />
                                </td>
                                <%-- <td>故障历史:</td>
                                <td>
                                    <input data-options="prompt:'如需参考历史故障，请选择'" class="easyui-combogrid" style="width: 150px;" name="hisrepairform" /></td>--%>
                            </tr>
                            <tr>
                                <td style="height: 25px;">故障位置:</td>
                                <td>
                                    <input data-options="prompt:'请选择故障位置'" class="easyui-combobox" style="width: 100%;" id="apositionid" name="positionid" />
                                </td>
                                <td>故障现象:</td>
                                <td>
                                    <input data-options="prompt:'请选择故障现象'" class="easyui-combobox" style="width: 100%;" id="aphenomenaid" name="phenomenaid" />
                                </td>
                                <%--<td>故障状态:</td>
                                <td>
                                    <select class="easyui-combobox" name="faultstatus" id="afaultstatus" style="width: 150px;" data-options="editable:false">
                                        <option value="1" selected="selected">1-停机</option>
                                        <option value="2">2-其他</option>
                                    </select>
                                </td>--%>
                                <td>瑕疵代码:</td>
                                <td>
                                    <select class="easyui-combobox" id="afaulttype" name="faulttype" style="width: 60px;" data-options="editable:false,onChange: function (newValue, oldValue) {
                                            if(newValue=='NA'){
                                                $('#afaultcode').textbox('setValue','');
                                                $('#afaultcode').textbox('disable');
                                            }else{
                                                $('#afaultcode').textbox('enable');
                                            }
                                        }">
                                        <option value="NA" selected="selected">NA</option>
                                        <option value="C">C</option>
                                        <option value="P">P</option>
                                    </select>
                                    <input disabled="disabled" data-options="prompt:'请输入瑕疵代码'" id="afaultcode" name="faultcode" class="easyui-textbox" style="width: 100px;" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px;">故障位置1:</td>
                                <td>
                                    <input data-options="prompt:'请选择故障位置1'" class="easyui-combobox" style="width: 100%;" id="apositionid1" name="positionid1" />
                                </td>
                                <td>故障现象1:</td>
                                <td>
                                    <input data-options="prompt:'请选择故障现象1'" class="easyui-combobox" style="width: 100%;" id="aphenomenaid1" name="phenomenaid1" />
                                </td>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td>历史故障原因:</td>
                                <td colspan="5">
                                    <input data-options="multiline:true,prompt:'请输入故障原因'" class="easyui-textbox" readonly="true" name="faultreason" id="afaultreason" style="width: 90%; height: 40px;" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <a class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" href="javascript:void(0)" onclick='Save()'>创建维修单</a>
                                    <a class="easyui-linkbutton" data-options="iconCls:'icon-reload',plain:true" onclick="Search('ByKey')">刷新</a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </form>
            </div>
        </div>
        <div style="clear: both">
        </div>
        <table id="tbRepairForm" data-options="fit:false">
        </table>
    </div>

    <div style="clear: both">
    </div>

    <!--评价-->
    <div id="divEvaluate" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:true,buttons:'#dlg-evaluate'"
        style="width: 700px; height: 290px; padding: 5px;">
        <div class="easyui-tabs" style="width: 100%; height: 100%; z-index: -1;">
            <div title="报修信息" style="padding: 1px 1px 1px 1px; width: 100%;" data-options="fit:true">
                <form id="fmView" method="post" novalidate="novalidate">
                    <table style="height: 100%; vertical-align: middle;">
                        <colgroup>
                            <col span="1" style="width: 95px;" />
                            <col span="1" style="width: 220px" />
                            <col span="1" style="width: 95px;" />
                            <col span="1" style="width: 220px;" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <td style="height: 25px;">申请人:</td>
                                <td>
                                    <!--维修单号-->
                                    <input type="hidden" id="" name="repairid" />
                                    <input disabled="disabled" class="easyui-textbox" name="applyuserid" style="width: 200px;" />
                                </td>
                                <td>故障时间:</td>
                                <td>
                                    <input disabled="disabled" class="easyui-datetimebox" name="faulttime" style="width: 200px;" data-options="formatter: datetimeFormatter, parser: datetimeParser" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px;">故障设备:</td>
                                <td>
                                    <input disabled="disabled" class="easyui-textbox" name="deviceid" style="width: 200px;" />
                                </td>
                                <td>瑕疵代码:</td>
                                <td>
                                    <input disabled="disabled" class="easyui-textbox" name="faultcode" style="width: 200px;" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px;">故障位置:</td>
                                <td>
                                    <input disabled="disabled" class="easyui-textbox" style="width: 200px;" name="positiontext" />
                                </td>
                                <td>故障分类:</td>
                                <td>
                                    <input disabled="disabled" class="easyui-textbox" style="width: 200px;" name="phenomenatext" /></td>
                            </tr>
                            <%--<tr>
                                <td>故障状态:</td>
                                <td>
                                    <select disabled="disabled" class="easyui-combobox" name="faultstatus" style="width: 200px;" data-options="editable:false">
                                        <option value="1" selected="selected">1-停机</option>
                                        <option value="2">2-其他</option>
                                    </select>
                                </td>
                                
                            </tr>--%>
                            <tr>
                                <td style="height: 25px;">故障原因:</td>
                                <td colspan="3">
                                    <input disabled="disabled" class="easyui-textbox" name="faultreason" style="width: 530px; height: 52px;" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </form>
            </div>
            <div title="维修信息" style="padding: 1px 1px 1px 1px; width: 100%;" data-options="fit:true">
                <table id="tbRepairInfo" data-options="fit:true">
                </table>
            </div>
        </div>
    </div>
    <div id="dlg-evaluate">
        <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel',plain:true" onclick="closeWindow('divEvaluate')">取消</a>
    </div>
    <div style="clear: both">
    </div>
    <div id="toolbarleader" style="height: auto; display: none;">
        <input class="easyui-textbox" data-options="prompt:'请输入关键字'" id="txtleader" style="width: 200px;" /><a
            href="#" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true" onclick="searchUser()">查询</a>
    </div>
    <div style="clear: both">
    </div>
    <div id="toolbardeviceid" style="height: auto; display: none;">
        <input class="easyui-textbox" data-options="prompt:'请输入关键字'" id="txtdeviceid" style="width: 200px;" /><a
            href="#" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true" onclick="searchDeviceid()">查询</a>
    </div>
    <div style="clear: both">
    </div>
</body>
</html>
