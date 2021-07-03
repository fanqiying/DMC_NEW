<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepairRecord.aspx.cs" Inherits="Web.UI.Equment.RepairRecord" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>维修管理</title>
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.json-2.4.js"></script>
    <script src="../../easyUI15/locale/easyui-lang-zh_TW.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
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
                           },
                           'json');
                    }
                }
            });
            //加載下拉框的故障分类
            $('#aphenomenaid').combobox({
                //url: '../../ASHX/DMC/FaultPosition.ashx?M=GetFaultPositionMain',
                valueField: 'positionid',
                textField: 'positionname',
                panelHeight: '150',
                editable: false,
                multiple: false
            });
            //綁定datagrid
            $('#tbRepairRecord').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: '../../ASHX/DMC/RepairRecord.ashx?M=Search&SearchType=ByAdvanced&repairstatus=1',
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
                           { field: 'repairformno', title: '维修单号', width: 60, align: 'left' },
                    { field: 'repairmanid', title: '维修员', width: 60, align: 'left', hidden: true },
                    { field: 'repairmanname', title: '维修员', width: 60, align: 'left' },
                           {
                               field: 'repairstatus', title: '状态', width: 50, align: 'left',
                               formatter: function (value, row, index) {
                                   var text = "N/A";
                                   switch (row.repairstatus) {
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
                                   }
                                   return text;
                               }
                           },
                           { field: 'deviceid', title: '设备编号', width: 60, align: 'left' },
                           { field: 'positiontext', title: '故障位置', width: 70, align: 'left' },
                           { field: 'phenomenatext', title: '故障现象', width: 70, align: 'left' },
                           { field: 'repairstime', title: '指派时间', width: 80, align: 'left' },
                    { field: 'repairetime', title: '完成时间', width: 80, align: 'left' },
                    { field: 'ipqcnumber', title: 'IPQC', width: 80, align: 'left' },
                    { field: 'mouldid', title: '模具编号', width: 70, align: 'left' },
                    { field: 'newmouldid', title: '新模编号', width: 70, align: 'left' },
                    { field: 'positiontext1', title: '故障位置1', width: 70, align: 'left' },
                    { field: 'phenomenatext1', title: '故障现象1', width: 70, align: 'left' },
                     { field: 'rebackreason', title: '返修原因', width: 70, align: 'left' },

                           {
                               field: 'opt', title: '操作', width: 120, align: 'left',
                               formatter: function (value, row, index) {
                                   return '<a href="#" onclick="Confirm(\'30\',\'' + index + '\')">提交生产员</a> | <a href="#" onclick="Confirm(\'40\',\'' + index + '\')">提交QC</a> | <a href="#" onclick="Reject(\'' + index + '\')">挂单</a> | <a href="#" onclick="View(\'' + index + '\')">查看</a>';
                               }
                           }
                ]]
            });
            $(window).resize(function () {
                $('#tbRepairRecord').datagrid('resize');
            });
            //設置分页控件屬性 
            var p = $('#tbRepairRecord').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: [10, 15, 20, 25, 30],
                beforePageText: '第',
                afterPageText: '页 共{pages}页',
                displayMsg: '当前显示 {from} - {to} 条记录 共 {total} 条记录'
            });
        });
        //Grid初使加載的數據
        function Search(SearchType) {
            debugger;
            var queryParams = $('#tbRepairRecord').datagrid('options').queryParams;
            //請輸入關鍵字            
            queryParams.KeyWord = $('#txtKeyword').textbox("getValue");
            //queryParams.SearchType = SearchType;
            queryParams.RepairFormNO = $("#qRepairFormNO").textbox("getValue");
            queryParams.DeviceId = $("#qDeviceId").textbox("getValue");
            //queryParams.RepairStatus = $('#qRepairStatus').combobox("getValue");
            $('#tbRepairRecord').datagrid('reload');
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
            var row = $('#tbRepairRecord').datagrid('getData').rows[index];
            $('#fmView').form('clear');
            $('#fmView').form('load', row);
            $('#divView').dialog('open').dialog('setTitle', '信息查看');
        }
        //挂单
        var repairstatus = "";
        function Reject(index) {
            var row = $('#tbRepairRecord').datagrid('getData').rows[index];
            if (row.repairstatus == "20" || row.repairstatus == "23" || row.repairstatus == "24" || row.repairstatus == "25") {
                $('#fmReback').form('clear');
                $('#fmReback').form('load', row);
                repairstatus = row.repairstatus;
                $('#divReback').dialog('open').dialog('setTitle', '挂单');
            } else {
                $.messager.alert({ title: '错误提示', msg: '只能挂单待维修和返修的单号！' });
            }
        }

        function SaveReback() {
            $.messager.defaults = { ok: '是', cancel: '否', width: 300 };

            if ($("#auserid").textbox("getValue") == "") {
                $.messager.alert({ title: '消息提示', msg: '请设置操作员' });
                return;
            }
            //检查是否有操作权限
            $.post("../../ASHX/Permission/UserRightManage.ashx?M=programright",
                   { opuser: $("#auserid").textbox("getValue"), ProgramId: "eqwi002" },
                   function (result) {
                       if (result.success) {
                           //有权限则提交
                           $.messager.confirm('挂单确认', '你確定需要挂单吗?', function (r) {
                               if (r) {
                                   $.post("../../ASHX/DMC/RepairRecord.ashx?M=Reject",
                                          {
                                              RepairFormNO: $("#rrepairformno").val(),
                                              AutoId: $("#rautoid").val(),
                                              RebackType: $("#rebacktype").combobox("getValue"),
                                              RebackReason: $("#rebackreason").textbox("getValue"),
                                              OldRepairStatus: repairstatus
                                          },
                                           function (result) {
                                               if (result.success) {
                                                   $('#tbRepairRecord').datagrid('reload');
                                                   closeWindow('divReback');
                                                   $.messager.alert({ title: '成功提示', msg: '挂单已确认！' });
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

                       } else {
                           $.messager.alert({
                               title: '错误提示',
                               msg: result.msg
                           });
                       }
                   },
                   'json');
        }

        var curConfirmState = "20";
        //提交
        function Confirm(nextStatus, index) {
            var row = $('#tbRepairRecord').datagrid('getData').rows[index];
            if (row.repairstatus == "20" || row.repairstatus == "23" || row.repairstatus == "24" || row.repairstatus == "25") {
                $('#fm').form('clear');
                curConfirmState = nextStatus;
                repairstatus = row.repairstatus;
                $('#fm').form('load', row);
                $('#btnSave').show();
                if (nextStatus == "30")
                    $('#divNew').dialog('open').dialog('setTitle', '提交生产员');
                else
                    $('#divNew').dialog('open').dialog('setTitle', '提交IQC');

                $('#afaultanalysis').textbox().next('span').find('input').focus();
            }
            else {
                $.messager.alert({ title: '错误提示', msg: '只能提交待维修和返修的单号！' });
            }
        }

        //保存
        function Save() {
            var msg = "";
            if ($("#auserid").textbox("getValue") == "") {
                $.messager.alert({ title: '消息提示', msg: '请设置操作员' });
                return;
            }
            //检查是否有操作权限
            $.post("../../ASHX/Permission/UserRightManage.ashx?M=programright",
                   { opuser: $("#auserid").textbox("getValue"), ProgramId: "eqwi002" },
                   function (result) {
                       if (result.success) {
                           //数据提交
                           $.post("../../ASHX/DMC/RepairRecord.ashx?M=Confirm", {
                               RepairFormNO: $("#arepairformno").val(),
                               FaultReason: $("#afaultreason").textbox("getValue"),
                               //FaultStatus: $("#afaultstatus").combobox("getValue"),
                               FaultStatus: "",
                               FaultCode: $("#afaultcode").textbox("getValue"),
                               PositionText: $('#apositionid').combobox('getText'),
                               PositionId: $('#apositionid').combobox('getValue'),
                               PhenomenaId: $('#aphenomenaid').combobox('getValue'),
                               PhenomenaText: $('#aphenomenaid').combobox('getText'),
                               FaultAnalysis: $("#afaultanalysis").textbox("getValue"),
                               RepairStatus: curConfirmState,
                               OldRepairStatus: repairstatus,
                               AutoId: $("#aautoid").val()
                           },
                           function (result) {
                               if (result.success) {
                                   $('#fm').form('clear');
                                   $('#tbRepairRecord').datagrid('reload');
                                   closeWindow('divNew');
                                   $.messager.alert({ title: '成功提示', msg: '數據已保存成功' });
                               } else {
                                   $.messager.alert({
                                       title: '错误提示',
                                       msg: result.msg
                                   });
                               }
                           },
                           'json');
                       } else {
                           $.messager.alert({
                               title: '错误提示',
                               msg: result.msg
                           });
                       }
                   },
                   'json');
        }

        function Export() {
            var queryParams = $('#tbRepairRecord').datagrid('options').queryParams;
            var url = "../../ASHX/DMC/RepairRecord.ashx?M=download&repairstatus=1&fileName=维修记录.csv";
            if (queryParams.KeyWord)
            {
                url = url + "&KeyWord=" + queryParams.KeyWord;
            }
            if (queryParams.RepairFormNO)
            {
                url = url + "&RepairFormNO=" + queryParams.RepairFormNO;
            }
            if (queryParams.DeviceId)
            {
                url = url + "&DeviceId=" + queryParams.DeviceId;
            }
            if ($("#downloadForm").length <= 0) {
                $("body").append("<form id='downloadForm' method='post' target='iframe'></form>");
                $("body").append("<iframe id='ifm' name='iframe' style='display:none;'></iframe>");
            }
            $("#downloadForm").attr('action', url);
            $("#downloadForm").submit();
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
</head>
<body>
    <div id="funMain" style="background-color: transparent; margin-left: 5px; margin-right: 0px;">
        <div id="divOperation" class="Search">
            <div class="l leftSearch">
                <input class="easyui-textbox" data-options="prompt:'请输入关键字'" id="txtKeyword" style="width: 200px;" />
                <a class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true" onclick="Search('ByKey')">搜索</a>
                <a id="btnMore" class="easyui-linkbutton" data-options="iconCls:'icon-expand',plain:true" onclick="openSearch('divSearch');">高级搜索</a>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;操作人：<input data-options="prompt:'请输入操作人工号',required: true" class="easyui-textbox" id="auserid" style="width: 150px;" />
            </div>
            <div class="r rightSearch">
               </div>
        </div>
        <div style="clear: both">
        </div>
        <div class="squery" id="divSearch">
            <div class="sinquery">
                <table class="addCoyTB" style="height: 30px; border: 0px;">
                    <tr>
                        <td style="width: 70px;">报修单号:</td>
                        <td style="width: 195px;">
                            <input class="easyui-textbox" id="qRepairFormNO" data-options="prompt:'请输入报修单号'" style="width: 150px;" />
                        </td>
                        <td style="width: 70px;">设备编号:</td>
                        <td style="width: 195px;">
                            <input class="easyui-textbox" id="qDeviceId" data-options="prompt:'请输入设备编号'" style="width: 150px;" />
                        </td>
                        <%--<td style="width: 70px;">单据状态:</td>
                        <td>
                            <select class="easyui-combobox" id="qRepairStatus" style="width: 150px;" data-options="editable:false">
                                <option value="" selected="selected">All</option>
                                <option value="-1">-1-返修</option>
                                <option value="0">0-待指派</option>
                                <option value="1">1-待维修</option>
                                <option value="2">2-待生产确认</option>
                                <option value="3">3-待QC确认</option>
                                <option value="4">4-待生产员确认</option>
                                <option value="5">5-待生产组长确认</option>
                                <option value="6">6-已确认</option>
                            </select>
                        </td>--%>
                    </tr>
                </table>
            </div>
            <div style="text-align: right; padding: 3px 0; padding-right: 8px; height: 26px; font-size: 12px;">
                <a class="easyui-linkbutton" data-options="iconCls:'icon-enlarge',plain:true" onclick="Search('ByAdvanced')">高級搜索</a>
            </div>
        </div>
        <div style="clear: both">
        </div>
        <table id="tbRepairRecord" data-options="fit:false">
        </table>
    </div>

    <!--归属设置-->
    <div id="divNew" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:false,buttons:'#dlg-buttons'"
        style="width: 700px; height: 320px; padding: 5px;">
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
                        <td style="height: 25px;">故障设备:</td>
                        <td>
                            <input disabled="disabled" class="easyui-textbox" name="deviceid" id="adeviceid" style="width: 200px;" />
                            <input type="hidden" id="arepairformno" name="repairformno" />
                            <input type="hidden" id="aautoid" name="autoid" />
                        </td>
                        <td>故障时间:</td>
                        <td>
                            <input disabled="disabled" class="easyui-datetimebox" name="faulttime" id="afaulttime" style="width: 200px;" data-options="formatter: datetimeFormatter, parser: datetimeParser" />
                        </td>
                    </tr>
                    <%--<tr>
                        <td style="height: 25px;">故障设备:</td>
                        <td colspan="3">
                            
                        </td>
                         <td>故障历史:</td>
                        <td>
                            <input disabled="disabled" data-options="prompt:'如需参考历史故障，请选择'" class="easyui-combogrid" style="width: 200px;" name="hiserror" /></td>
                    </tr>--%>
                    <tr>
                        <td style="height: 25px;">故障位置:</td>
                        <td>
                            <input disabled="disabled" data-options="prompt:'请选择故障位置'" class="easyui-combobox" style="width: 150px;" id="apositionid" name="positionid" />
                        </td>
                        <td>故障分类:</td>
                        <td>
                            <input disabled="disabled" data-options="prompt:'请选择故障分类'" class="easyui-combobox" style="width: 150px;" id="aphenomenaid" name="phenomenaid" />
                        </td>
                    </tr>
                    <tr>
                        <%-- <td>故障状态:</td>
                        <td>
                            <select class="easyui-combobox" name="faultstatus" id="afaultstatus" style="width: 150px;" data-options="editable:false">
                                <option value="1" selected="selected">1-停机</option>
                                <option value="2">2-其他</option>
                            </select>
                        </td>--%>
                        <td>瑕疵代码:</td>
                        <td colspan="3">
                            <input disabled="disabled" data-options="prompt:'请输入瑕疵代码'" id="afaultcode" name="faultcode" class="easyui-textbox" style="width: 150px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 25px;">故障原因:</td>
                        <td colspan="3">
                            <input disabled="disabled" data-options="multiline:true,prompt:'请输入故障原因'" class="easyui-textbox" name="faultreason" id="afaultreason" style="width: 530px; height: 60px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 50px;">故障分析:</td>
                        <td colspan="3">
                            <input data-options="multiline:true,prompt:'请填写故障分析'" class="easyui-textbox" id="afaultanalysis" name="faultanalysis" style="width: 530px; height: 60px;" />
                        </td>
                    </tr>
                    <%--<tr>
                        <td style="height: 25px;">返修原因:</td>
                        <td colspan="3">
                            <input data-options="multiline:true,prompt:'如需返修原因,请填写'" class="easyui-textbox" style="width: 530px;" />
                        </td>
                    </tr>--%>
                </tbody>
            </table>
        </form>
    </div>
    <div id="dlg-buttons">
        <a id="btnSave" class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="Save()">确认</a>
        <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel',plain:true" onclick="closeWindow('divNew')">取消</a>
    </div>
    <div style="clear: both">
    </div>

    <!--归属设置-->
    <div id="divReback" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:false,buttons:'#dlg-Reback'"
        style="width: 700px; height: 300px; padding: 5px;">
        <form id="fmReback" method="post" novalidate="novalidate">
            <table style="height: 100%; vertical-align: middle;">
                <colgroup>
                    <col span="1" style="width: 95px;" />
                    <col span="1" style="width: 220px" />
                    <col span="1" style="width: 95px;" />
                    <col span="1" style="width: 220px;" />
                </colgroup>
                <tbody>
                    <tr>
                        <td style="height: 25px;">挂单分类:</td>
                        <td colspan="3">
                            <input type="hidden" id="rrepairformno" name="repairformno" />
                            <input type="hidden" id="rautoid" name="autoid" />
                            <select class="easyui-combobox" id="rebacktype" name="rebacktype" style="width: 150px;" data-options="editable:false">
                                <option value="" selected="selected">请选择</option>
                                <option value="0">0-拒收</option>
                                <option value="1">1-做不了</option>
                                <option value="2">2-交接班</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 25px;">挂单原因:</td>
                        <td colspan="3">
                            <input data-options="multiline:true,prompt:'请输入挂单原因'" class="easyui-textbox" id="rebackreason" name="rebackreason" style="width: 530px; height: 60px;" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </form>
    </div>
    <div id="dlg-Reback">
        <a class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="SaveReback()">确认</a>
        <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel',plain:true" onclick="closeWindow('divReback')">取消</a>
    </div>
    <div style="clear: both">
    </div>
    <!--归属设置-->
    <div id="divView" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:false,buttons:'#dlg-view'"
        style="width: 700px; height: 320px; padding: 5px;">
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
                        <td style="height: 25px;">故障设备:</td>
                        <td>
                            <input disabled="disabled" class="easyui-textbox" name="deviceid" style="width: 200px;" />
                            <input type="hidden" id="Hidden1" name="repairformno" />
                            <input type="hidden" id="Hidden2" name="autoid" />
                        </td>
                        <td>故障时间:</td>
                        <td>
                            <input disabled="disabled" class="easyui-datetimebox" name="faulttime" style="width: 200px;" data-options="formatter: datetimeFormatter, parser: datetimeParser" />
                        </td>
                    </tr>
                    <%--<tr>
                        <td style="height: 25px;">故障设备:</td>
                        <td colspan="3">
                            
                        </td>
                         <td>故障历史:</td>
                        <td>
                            <input disabled="disabled" data-options="prompt:'如需参考历史故障，请选择'" class="easyui-combogrid" style="width: 200px;" name="hiserror" /></td>
                    </tr>--%>
                    <tr>
                        <td style="height: 25px;">故障位置:</td>
                        <td>
                            <input disabled="disabled" data-options="prompt:'请选择故障位置'" class="easyui-combobox" style="width: 150px;" name="positionid" />
                        </td>
                        <td>故障分类:</td>
                        <td>
                            <input disabled="disabled" data-options="prompt:'请选择故障分类'" class="easyui-combobox" style="width: 150px;" name="phenomenaid" />
                        </td>
                    </tr>
                    <tr>
                        <%-- <td>故障状态:</td>
                        <td>
                            <select class="easyui-combobox" name="faultstatus" id="afaultstatus" style="width: 150px;" data-options="editable:false">
                                <option value="1" selected="selected">1-停机</option>
                                <option value="2">2-其他</option>
                            </select>
                        </td>--%>
                        <td>瑕疵代码:</td>
                        <td colspan="3">
                            <input disabled="disabled" data-options="prompt:'请输入瑕疵代码'" name="faultcode" class="easyui-textbox" style="width: 150px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 25px;">故障原因:</td>
                        <td colspan="3">
                            <input disabled="disabled" data-options="multiline:true,prompt:'请输入故障原因'" class="easyui-textbox" name="faultreason" style="width: 530px; height: 60px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 50px;">故障分析:</td>
                        <td colspan="3">
                            <input disabled="disabled" data-options="multiline:true,prompt:'请填写故障分析'" class="easyui-textbox" name="faultanalysis" style="width: 530px; height: 60px;" />
                        </td>
                    </tr>
                    <%--<tr>
                        <td style="height: 25px;">返修原因:</td>
                        <td colspan="3">
                            <input data-options="multiline:true,prompt:'如需返修原因,请填写'" class="easyui-textbox" style="width: 530px;" />
                        </td>
                    </tr>--%>
                </tbody>
            </table>
        </form>
    </div>
    <div id="dlg-view">
        <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel',plain:true" onclick="closeWindow('divView')">取消</a>
    </div>
    <div style="clear: both">
    </div>
</body>
</html>
