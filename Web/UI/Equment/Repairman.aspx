<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Repairman.aspx.cs" Inherits="Web.UI.Equment.Repairman" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>维修人员设置</title>
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="../../easyUI15/locale/easyui-lang-zh_TW.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#arepairmanid').combogrid({
                //是否折叠
                required: true,
                panelWidth: 430,
                toolbar: '#toolbarleader',
                url: '../../ASHX/Organize/EmpManage.ashx?M=SearchGroup',
                idField: 'empid',
                textField: 'disaplytext',
                pagination: true,//是否分页
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
                onSelect: function (rowIndex, rowData) {
                    $("#arepairmanname").textbox("setValue", rowData.empname);
                }
            });
            //綁定datagrid
            $('#tbRepairman').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: '../../ASHX/DMC/Repairman.ashx?M=Search',
                //數據在一行顯示 
                nowrap: false,
                fitColumns: true,
                //行條紋化
                striped: true,
                pageSize: 20,
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
                           { field: 'yearmonth', title: '所属年月', width: 30, align: 'left' },
                    { field: 'repairmanid', title: '维修员编号', width: 30, align: 'left', sortable:"true"},
                    { field: 'repairmanname', title: '维修员姓名', width: 30, align: 'left' },
                    { field: 'workrangetimebegin',title: '上班时间', width: 40, align: 'left' },
                    { field: 'workrangetimeend', title: '下班时间', width: 40, align: 'left' },
                    { field: 'worknum', title: '上班时长/时', width: 20, align: 'left' },
                    { field: 'workdate', title: '上班日期', width: 30, align: 'left' },
                           {
                               field: 'isleader', title: '员工类别', width: 20, align: 'left',
                               formatter: function (value, row, index) {
                                   if (row.isleader == "0") {
                                       return "员工";
                                   } else {
                                       return "组长";
                                   }
                               }
                           },
                           {
                               field: 'isworking', title: '值班状态', width: 20, align: 'left',
                               formatter: function (value, row, index) {
                                   if (row.isworking == "0") {
                                       return "休假";
                                   } else {
                                       return "值班";
                                   }
                               }
                           },
                           {
                               field: 'classtype', title: '班别', width: 20, align: 'left',
                               formatter: function (value, row, index) {
                                   if (row.classtype == "0") {
                                       return "晚班";
                                   } else {
                                       return "早班";
                                   }
                               }
                           },
                           {
                               field: 'opt', title: '操作', width: 70, align: 'left',
                               formatter: function (value, row, index) {
                                   var url = '<a href="#" onclick="Edit(\'' + index + '\')">编辑</a>';
                                   if (row.isworking == "1") {
                                       url += ' | <a href="#" onclick="SetWorking(0,\'' + index + '\')">休假</a>';
                                   } else {
                                       url += ' | <a href="#" onclick="SetWorking(1,\'' + index + '\')">值班</a>';
                                   }
                                   url += ' | <a href="#" onclick="Delete(\'' + index + '\')">删除</a>';
                                   return url;
                               }
                           }
                ]]
            });
            $(window).resize(function () {
                $('#tbRepairman').datagrid('resize');
            });
            //設置分页控件屬性 
            var p = $('#tbRepairman').datagrid('getPager');
            $(p).pagination({
                pageSize: 20,
                pageList: [10, 15, 20, 25, 30],
                beforePageText: '第',
                afterPageText: '页 共{pages}页',
                displayMsg: '当前显示 {from} - {to} 条记录 共 {total} 条记录'
            });
        });
        //负责人搜索
        function searchUser() {
            var queryParams = $('#arepairmanid').combogrid('grid').datagrid('options').queryParams;
            //請輸入關鍵字            
            queryParams.KeyWord = $('#txtleader').textbox("getValue");
            queryParams.SearchType = 'ByKey';
            $('#arepairmanid').combogrid('grid').datagrid('reload');
        }
        //Grid初使加載的數據
        function Search(SearchType) {
            var queryParams = $('#tbRepairman').datagrid('options').queryParams;
            //請輸入關鍵字            
            queryParams.KeyWord = $('#txtKeyword').textbox("getValue");
            queryParams.SearchType = SearchType;
            queryParams.Repairman = $("#qrepairman").textbox("getValue");
            queryParams.IsLeader = $('#qisleader').combobox("getValue");
            queryParams.YearMonth = $('#qyearmonth').datebox("getValue");
            $('#tbRepairman').datagrid('reload');
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

        var url = "../../ASHX/DMC/Repairman.ashx?M=NewRepairman";
        //查看
        function Edit(index) {
            url = "../../ASHX/DMC/Repairman.ashx?M=UpdateRepairman";
            $('#fm').form('clear');
            var row = $('#tbRepairman').datagrid('getData').rows[index];
            $('#fm').form('load', row);
            $("#aisleader").switchbutton("uncheck");
            $("#aisworking").switchbutton("uncheck");
            $("#aclasstype").switchbutton("uncheck");
            if (row.classtype == 1) {
                $("#aclasstype").switchbutton("check");
            }
            if (row.isleader == 1) {
                $("#aisleader").switchbutton("check");
            }
            if (row.isworking == 1) {
                $("#aisworking").switchbutton("check");
            }
            $('#divNew').dialog('open').dialog('setTitle', '信息查看');
        }
        //删除
        function Delete(index) {
            var row = $('#tbRepairman').datagrid('getData').rows[index];
            $.messager.confirm("删除确认", "确定需要删除维修员【" + row.repairmanid + "】吗？", function (r) {
                if (r) {
                    $.post("../../ASHX/DMC/Repairman.ashx?M=DeleteRepairman",
                        {
                            RepairmanId: row.repairmanid,
                            WorkDate: row.workdate
                        },
                        function (result) {
                            if (result.success) {
                                $('#tbRepairman').datagrid('reload');
                                $.messager.alert({ title: '成功提示', msg: '数据已删除成功' });
                                $('#divNew').dialog('close');
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
        //状态更改
        function SetWorking(isworking, index) {
            url = "../../ASHX/DMC/Repairman.ashx?M=SetWorking";
            var row = $('#tbRepairman').datagrid('getData').rows[index];
            $.messager.defaults = { ok: '确认', cancel: '取消', width: 300 };
            var tip = "";
            var msg = "";
            if (isworking == 0) {
                tip = "休假确认";
                msg = "该员工确认需要休假么?";
            } else {
                tip = "值班确认";
                msg = "该员工确认需要值班么?";
            }
            $.messager.confirm(tip, msg, function (r) {
                if (r) {
                    $.post(url, {
                        RepairmanId: row.repairmanid,
                        WorkDate: row.workdate,
                        IsWorking: isworking
                    },
                    function (result) {
                        if (result.success) {
                            $('#tbRepairman').datagrid('reload');
                            $.messager.alert({ title: '成功提示', msg: '值班状态已更新' });
                            $('#divNew').dialog('close');
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

        function Save() {
            var classtype = "0";
            if ($("#aclasstype").switchbutton("options").checked) {
                classtype = "1";
            }
            var isleader = "0";
            if ($("#aisleader").switchbutton("options").checked) {
                isleader = "1";
            }
            var isworking = "0";
            if ($("#aisworking").switchbutton("options").checked) {
                isworking = "1";
            }
            var msg = "";
            if ($("#arepairmanid").combogrid("getValue") != "") {
                //数据提交
                $.post(url, {
                    RepairmanId: $("#arepairmanid").combogrid("getValue"),
                    RepairmanName: $("#arepairmanname").textbox("getValue"),
                    ClassType: classtype,
                    IsLeader: isleader,
                    IsWorking: isworking,
                    PhotoUrl: '',
                    WorkRangeTime: $("#aworkrangetime").numberbox("getValue"),
                    YearMonth: $('#ayearmonth').datebox("getValue"),
                    WorkRangeTimeBegin: $("#aworkrangetimebegin").datetimebox("getValue"),
                    WorkRangeTimeEnd: $("#aworkrangetimeend").datetimebox("getValue"),
                    WorkDate: $("#aworkdate").datebox("getValue"),
                    WorkNum: $("#aworknum").textbox("getValue")
                },
                function (result) {
                    if (result.success) {
                        $('#tbRepairman').datagrid('reload');
                        $.messager.alert({ title: '成功提示', msg: '数据已保存成功' });
                        $('#divNew').dialog('close');
                    } else {
                        $.messager.alert({
                            title: '错误提示',
                            msg: result.msg
                        });
                    }
                },
                'json');
            } else {
                msg = "必输项未设置完整";
                $.messager.alert({ title: '错误提示', msg: msg });
            }
        }

        function add() {
            url = "../../ASHX/DMC/Repairman.ashx?M=NewRepairman";
            $('#fm').form('clear');
            $("#aisleader").switchbutton("uncheck");
            $("#aisworking").switchbutton("check");
            $("#aclasstype").switchbutton("check");
            $('#ayearmonth').datebox("setValue", '<%= DateTime.Now.ToString("yyyy-MM-dd")%>');
            $('#divNew').dialog('open').dialog('setTitle', '新增维修员');
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
    <!--文件上传相关方法-->
    <script type="text/javascript">
        //新增上傳
        function NewUpload() {
            try {
                $('#fmUpload').form('clear');
                $('#adduploadfile').dialog({ title: '导入维修员信息' }); //新增上傳 
                $('#adduploadfile').dialog('open');
            }
            catch (e) {
                $.messager.show({
                    title: '异常提示', //異常提示
                    msg: '数据异常，请确认网络连接状况'//數據異常，請確認網絡連接狀況
                });
            }
        }

        //生成一個隨機數
        function GetUuid() {
            var s = [];
            var hexDigits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            for (var i = 0; i < 36; i++) {
                s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
            }
            s[14] = "4";
            s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1);
            s[8] = s[13] = s[18] = s[23] = "-";

            var uuid = s.join("");
            return uuid;
        }
        function Download() {
            window.location.target = "_blank";
            window.location.href = "../../Template/RepairmanTemp.xls";
        }

        //上传文件操作
        function UploadFile() {
            var value = $("#arepairfile").filebox('getValue');
            var files = $("#arepairfile").next().find('input[type=file]')[0].files;
            var guid = GetUuid();
            if (value && files[0]) {
                //构建一个FormData存储复杂对象
                var formData = new FormData();
                formData.append("guid", guid);
                formData.append('Filedata', files[0]);//默认的文件数据名为“Filedata”

                $.ajax({
                    url: '../../ASHX/DMC/Repairman.ashx?M=SaveUpload', //单文件上传
                    type: 'POST',
                    dataType: "json",
                    processData: false,
                    contentType: false,
                    data: formData,
                    success: function (result) {
                        if (result.success) {
                            $('#tbRepairman').datagrid('reload');
                            $('#adduploadfile').dialog('close');
                            $.messager.alert({ title: '成功提示', msg: "数据导入成功！" });
                        } else {
                            $.messager.show({
                                title: '错误提示',
                                msg: result.msg
                            });
                        }
                    },
                    error: function (xhr, status, error) {
                        $.messager.alert("提示", "操作失败");
                    }
                });
            } else {
                $.messager.alert("提示", "请选择文件");
            }
        }
        var isDay = false;
        function dateFormatter(value) {
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
            month = month < 10 ? '0' + month : month;
            day = day < 10 ? '0' + day : day;
            if (isDay)
                return year + "-" + month + "-" + day;
            else
                return year + "-" + month;
        }

        function dateParser(s) {
            if (s != "") {
                var dt = s.split(" ");
                var arr = dt[0].split("-");
                var y = parseInt(arr[0], 10);
                var m = parseInt(arr[1], 10) - 1;
                var d = null;
                if (isDay) {
                    d = parseInt(arr[2], 10);
                } else {
                    d = 1;
                }
                return new Date(y, m, d);
            } else {
                return new Date();
            }
        }

        function datedayFormatter(value) {
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
            month = month < 10 ? '0' + month : month;
            day = day < 10 ? '0' + day : day;
            return year + "-" + month + "-" + day;
        }

        function datedayParser(s) {
            if (s != "") {
                var dt = s.split(" ");
                var arr = dt[0].split("-");
                var y = parseInt(arr[0], 10);
                var m = parseInt(arr[1], 10) - 1;
                var d = null;
                d = parseInt(arr[2], 10);
                return new Date(y, m, d);
            } else {
                return new Date();
            }
        }
    </script>
</head>
<body>
    <div id="funMain" style="background-color: transparent; margin-left: 5px; margin-right: 0px;">
        <div id="divOperation" class="Search">
            <div class="l leftSearch">
                <input class="easyui-textbox" data-options="prompt:'请输入关键字'" id="txtKeyword" style="width: 200px;" />
                <a class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true" onclick="Search('ByKey')">搜索</a>
                <a id="btnMore" class="easyui-linkbutton" data-options="iconCls:'icon-expand',plain:true" onclick="openSearch('divSearch');">高级搜索</a>
            </div>
            <div class="r rightSearch">
                <a class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" href="javascript:void(0)" onclick='add()'>新增</a> &nbsp;&nbsp;
                <a class="easyui-linkbutton" data-options="iconCls:'icon-excel',plain:true" href="javascript:void(0)" onclick='NewUpload()'>导入维修</a>&nbsp;&nbsp;
                <a class="easyui-linkbutton" data-options="iconCls:'icon-tempsave',plain:true" href="javascript:void(0)" onclick='Download()'>模板下载</a>&nbsp;&nbsp;
            </div>
        </div>
        <div style="clear: both">
        </div>
        <div class="squery" id="divSearch">
            <div class="sinquery">
                <table class="addCoyTB" style="height: 30px; border: 0px;">
                    <tr>
                        <td style="width: 70px;">员工信息:</td>
                        <td style="width: 195px;">
                            <input class="easyui-textbox" id="qrepairman" name="repairman" data-options="prompt:'请输入员工信息'" style="width: 150px;" />
                        </td>
                        <td style="width: 70px;">员工类别:</td>
                        <td>
                            <select class="easyui-combobox" name="isleader" id="qisleader" style="width: 150px;" data-options="editable:false">
                                <option value="" selected="selected">All</option>
                                <option value="0">员工</option>
                                <option value="1">组长</option>
                            </select>
                        </td>
                        <td style="width: 70px;">所属年月:</td>
                        <td style="width: 195px;">
                            <input data-options="formatter: dateFormatter, parser: dateParser" class="easyui-datebox" name="yearmonth" id="qyearmonth" style="width: 200px;" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="text-align: right; padding: 3px 0; padding-right: 8px; height: 26px; font-size: 12px;">
                <a class="easyui-linkbutton" data-options="iconCls:'icon-enlarge',plain:true" onclick="Search('ByAdvanced')">高級搜索</a>
            </div>
        </div>
        <div style="clear: both">
        </div>
        <table id="tbRepairman" data-options="fit:false">
        </table>
    </div>

    <!--新增-->
    <div id="divNew" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:true,buttons:'#dlg-buttons'"
        style="width: 700px; height: 300px; padding: 5px;">
        <form id="fm" method="post" novalidate="novalidate">
            <table style="height: 100%; vertical-align: middle;">
                <colgroup>
                    <col span="1" style="width: 95px;" />
                    <col span="1" style="width: 220px" />
                    <col span="1" style="width: 95px;" />
                    <col span="1" style="width: 220px;" />
                </colgroup>
                <tbody>
                    <tr>
                        <td style="height: 25px;">所属年月:</td>
                        <td>
                            <input data-options="formatter: dateFormatter, parser: dateParser" class="easyui-datebox" name="yearmonth" id="ayearmonth" style="width: 200px;" />
                        </td>
                        <td>工作日期:</td>
                        <td>
                            <input data-options="formatter: datedayFormatter, parser: datedayParser" class="easyui-datebox" name="workdate" id="aworkdate" style="width: 200px;" /></td>
                    </tr>
                    <tr>
                        <td style="height: 25px;">上班时间:</td>
                        <td>
                            <input data-options="formatter: datetimeFormatter, parser: datetimeParser" class="easyui-datetimebox" name="workrangetimebegin" id="aworkrangetimebegin" style="width: 200px;" /></td>
                        <td>下班时间:</td>
                        <td>
                            <input data-options="formatter: datetimeFormatter, parser: datetimeParser" class="easyui-datetimebox" name="workrangetimeend" id="aworkrangetimeend" style="width: 200px;" /></td>
                    </tr>
                    <tr>
                        <td style="height: 25px;">维修员编号:</td>
                        <td>
                            <input data-options="prompt:'请输入员工编号'" class="easyui-combogrid" name="repairmanid" id="arepairmanid" style="width: 200px;" />
                        </td>
                        <td>维修员姓名:</td>
                        <td>
                            <input data-options="prompt:'请输入员工姓名'" class="easyui-textbox" name="repairmanname" id="arepairmanname" style="width: 200px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 25px;">员工类别:</td>
                        <td>
                            <input class="easyui-switchbutton" id="aisleader" name="isleader" data-options="onText:'组长',offText:'员工'" />
                        </td>
                        <td>值班状态:</td>
                        <td>
                            <input class="easyui-switchbutton" id="aisworking" name="isworking" data-options="onText:'值班',offText:'休假'" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 25px;">员工班别:</td>
                        <td>
                            <input class="easyui-switchbutton" id="aclasstype" name="classtype" data-options="onText:'早班',offText:'晚班'" />
                        </td>
                        <td>工作时长:</td>
                        <td>
                            <input data-options="prompt:'请输入工作时长(小时)'" class="easyui-textbox" name="worknum" id="aworknum" style="width: 200px;" /></td>
                    </tr>                    
                    <tr>
                        <td style="height: 25px;">班别时间:</td>
                        <td colspan="3">
                            <input name="workrangetime" id="aworkrangetime" data-options="prompt:'早班:07:15-19:30;晚班19:15-*07:30'" class="easyui-textbox" style="width: 400px;" /></td>
                    </tr>
                </tbody>
            </table>
        </form>
    </div>
    <div id="dlg-buttons">
        <a id="btnSave" class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="Save()">保存</a>
        <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel',plain:true" onclick="closeWindow('divNew')">关闭</a>
    </div>
    <div style="clear: both">
    </div>
    <div id="toolbarleader" style="height: auto">
        <input class="easyui-textbox" data-options="prompt:'请输入关键字'" id="txtleader" style="width: 200px;" /><a
            href="#" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true" onclick="searchUser()">查询</a>
    </div>
    <div style="clear: both">
    </div>
    <div id="adduploadfile" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:false,buttons:'#btn_uploadfile'"
        style="width: 550px; height: 130px; padding: 5px;">
        <form id="fmUpload" method="post">
            <input class="easyui-filebox" style="width: 100%;" id="arepairfile" name="repairfile" data-options="prompt:'请点击【文件选择】选择需要导入的数据',buttonText:'文件选择', buttonAlign: 'right',multiple:false,accept:'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel'" />
        </form>
    </div>
    <div id="btn_uploadfile" style="display: none;">
        <a class="easyui-linkbutton" data-options="text:'確認'" href="javascript:void(0)" onclick="UploadFile()">导入</a>
        <a class="easyui-linkbutton" data-options="text:'取消'" href="javascript:void(0)" onclick="$('#adduploadfile').dialog('close')">取消</a>
    </div>
</body>
</html>
