<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QCCheck.aspx.cs" Inherits="Web.UI.Equment.QCCheck" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>QC确认</title>
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
            $('#tbQCCheck').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: '../../ASHX/DMC/RepairRecord.ashx?M=Search&SearchType=ByAdvanced&repairstatus=4',
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
                    { field: 'mouldid', title: '模具编号1', width: 70, align: 'left' },
                           { field: 'mouldid1', title: '模具编号2', width: 70, align: 'left' },
                           {
                               field: 'newmouldid', title: '新模编号1', width: 70, align: 'left',
                               styler: function (value, row, index) {
                                   if (row.newmouldid != '') {
                                       return 'background-color:#4cae4c;color: #fff;border: 0px'
                                   }

                               }
                           },
                           {
                               field: 'newmouldid1', title: '新模编号2', width: 70, align: 'left',
                               styler: function (value, row, index) {
                                   if (row.newmouldid1 != '') {
                                       return 'background-color:#4cae4c;color: #fff;border: 0px'
                                   }

                               }
                           },
                           { field: 'rebackreason', title: '返修原因', width: 70, align: 'left' },
                    { field: 'positiontext1', title: '故障位置1', width: 70, align: 'left' },
                    { field: 'phenomenatext1', title: '故障现象1', width: 70, align: 'left' },
                           {
                               field: 'opt', title: '操作', width: 120, align: 'left',
                               formatter: function (value, row, index) {
                                   return '<a href="#" onclick="View(\'' + index + '\')">查看</a>';
                               }
                           }
                ]]
            });
            $(window).resize(function () {
                $('#tbQCCheck').datagrid('resize');
            });
            //設置分页控件屬性 
            var p = $('#tbQCCheck').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: [10, 15, 20, 25, 30],
                beforePageText: '第',
                afterPageText: '页 共{pages}页',
                displayMsg: '当前显示 {from} - {to} 条记录 共 {total} 条记录'
            });
            $("#aipqcnumber").textbox('textbox').bind('keyup', function (e) {
                $("#aipqcnumber").textbox('setValue', $(this).val().replace(/\D/g, ''));
                if ($(this).val().length > 8) {
                    $("#aipqcnumber").textbox('setValue', $(this).val().substring(0, 8));
                }
            });

            $("#aipqcnumberConfirm").textbox('textbox').bind('keyup', function (e) {
                $("#aipqcnumberConfirm").textbox('setValue', $(this).val().replace(/\D/g, ''));
                if ($(this).val().length > 8) {
                    $("#aipqcnumberConfirm").textbox('setValue', $(this).val().substring(0, 8));
                }
            });
        });
        //Grid初使加載的數據
        function Search(SearchType) {
            var queryParams = $('#tbQCCheck').datagrid('options').queryParams;
            //請輸入關鍵字            
            queryParams.KeyWord = $('#txtKeyword').textbox("getValue");
            //queryParams.SearchType = SearchType;
            queryParams.RepairFormNO = $("#qRepairFormNO").textbox("getValue");
            queryParams.DeviceId = $("#qDeviceId").textbox("getValue");
            //queryParams.RepairStatus = $('#qRepairStatus').combobox("getValue");
            $('#tbQCCheck').datagrid('reload');
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
            row = $('#tbQCCheck').datagrid('getData').rows[index];
            $('#fm').form('clear');
            $('#fm').form('load', row);
            $('#divNew').dialog('open').dialog('setTitle', '信息查看');
        }
        //QC确认
        function QCConfirm(index) {
            var row = $('#tbQCCheck').datagrid('getData').rows[index];
            if (row.repairstatus == "40") {
                $.messager.defaults = { ok: '是', cancel: '否', width: 300 };
                $.messager.confirm('QC确认', '你確定需要确认吗?', function (r) {
                    if (r) {
                        $.post("../../ASHX/DMC/RepairRecord.ashx?M=QCConfirm",
                               { RepairFormNO: row.repairformno, AutoId: row.autoid },
                                function (result) {
                                    if (result.success) {
                                        $('#tbQCCheck').datagrid('reload');
                                        $("#auserid").textbox("setValue", "");
                                        $.messager.alert({ title: '成功提示', msg: '维修单已提交给生产员！' });
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
                $.messager.alert({ title: '错误提示', msg: '只能确认待QC确认的单号！' });
            }
        }
        var row = null;
        function Reback() {
            //var row = $('#tbQCCheck').datagrid('getData').rows[index];
            if (row.repairstatus == "40") {
                $('#fm').form('clear');
                $('#fm').form('load', row);
                $('#divIPQC').dialog('open').dialog('setTitle', '返修处理');
            } else {
                $.messager.alert({ title: '错误提示', msg: '只能返修待QC确认的单号！' });
            }
        }
        //确认
        function QCConfirmNew() {
            //var row = $('#tbQCCheck').datagrid('getData').rows[index];
            if (row.repairstatus == "40") {
                $('#fm').form('clear');
                $('#fm').form('load', row);
                $('#divIPConfirm').dialog('open').dialog('setTitle', '确认');
            } else {
                $.messager.alert({ title: '错误提示', msg: '只能返修待QC确认的单号！' });
            }
        }
        function Confirm() {
            if (row.repairstatus == "40") {
                $.messager.defaults = { ok: '是', cancel: '否', width: 300 };
                $.messager.confirm('QC确认', '你確定需要确认吗?', function (r) {
                    if (r) {
                        $.post("../../ASHX/DMC/RepairRecord.ashx?M=QCConfirm",
                            {
                                RepairFormNO: row.repairformno, AutoId: row.autoid,
                                IPQCNumber: $("#aipqcnumberConfirm").textbox("getValue")
                            },
                                function (result) {
                                    if (result.success) {
                                        $('#tbQCCheck').datagrid('reload');
                                        closeWindow('divNew');                                        
                                        closeWindow('divIPConfirm');
                                        $("#auserid").textbox("setValue", "");
                                        $.messager.alert({ title: '成功提示', msg: '维修单已提交给生产员！' });
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
                $.messager.alert({ title: '错误提示', msg: '只能确认待QC确认的单号！' });
            }
        }

        //提交
        function Save() {
            if ($("#arebackreason").textbox("getValue") != "") {
                //数据提交
                $.post("../../ASHX/DMC/RepairRecord.ashx?M=QCRConfirm", {
                    RepairFormNO: $("#arepairformno").val(),
                    RebackReason: $("#arebackreason").textbox("getValue"),
                    AutoId: $("#aautoid").val(),
                    IPQCNumber: $("#aipqcnumber").textbox("getValue")
                },
                function (result) {
                    if (result.success) {
                        $('#fm').form('clear');
                        $('#tbQCCheck').datagrid('reload');
                        closeWindow('divNew');
                        closeWindow('divIPQC');
                        $("#auserid").textbox("setValue", "");
                        $.messager.alert({ title: '成功提示', msg: '返修成功' });
                    } else {
                        $.messager.alert({
                            title: '错误提示',
                            msg: result.msg
                        });
                    }
                },
                'json');
            } else {
                $.messager.alert({ title: '成功提示', msg: '请填写返修原因' });
            }
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

        function Refersh() {
            //重新加载数据
            $('#tbQCCheck').datagrid('reload');
        }

        $(function () {//间隔60s自动加载一次   
            Refersh(); //首次立即加载   
            window.setInterval(Refersh, 60 * 1000); //循环执行！！   
        });
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
                        <td style="width: 70px;">单据状态:</td>
                        <%--<td>
                            <select class="easyui-combobox" id="qRepairStatus" data-options="editable:false" style="width: 150px;">
                                <option value="" selected="selected">All</option>
                                <option value="-1">-1-返修</option>
                                <option value="0">0-待指派</option>
                                <option value="1">1-待维修</option>
                                <option value="2">2-待生产确认</option>
                                <option value="3">3-待QC确认</option>
                                <option value="4">4-待生产组长确认</option>
                                <option value="5">5-待评价</option>
                                <option value="6">6-已评价</option>
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
        <table id="tbQCCheck" data-options="fit:false">
        </table>
    </div>

    <!--归属设置-->
    <div id="divNew" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:true,buttons:'#dlg-buttons'"
        style="width: 700px; height: 370px; padding: 5px;">
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
                    <tr>
                        <td style="height: 25px;">故障位置:</td>
                        <td>
                            <input disabled="disabled" class="easyui-combotree" style="width: 200px;" id="apositionid" name="positionid" />
                        </td>
                        <td>故障分类:</td>
                        <td>
                            <input disabled="disabled" class="easyui-combotree" style="width: 200px;" id="aphenomenaid" name="phenomenaid" />
                        </td>
                    </tr>
                    <tr>
                        <%--<td>故障状态:</td>
                        <td>
                            <select disabled="disabled" class="easyui-combobox" name="faultstatus" style="width: 150px;" data-options="editable:false">
                                <option value="1" selected="selected">1-停机</option>
                                <option value="2">2-其他</option>
                            </select>
                        </td>--%>
                        <td>瑕疵代码:</td>
                        <td colspan="3">
                            <input disabled="disabled" name="faultcode" class="easyui-textbox" style="width: 200px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 30px;">故障原因:</td>
                        <td colspan="3">
                            <input disabled="disabled" data-options="multiline:true" class="easyui-textbox" name="faultreason" style="width: 530px; height: 30px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 30px;">故障分析:</td>
                        <td colspan="3">
                            <input disabled="disabled" data-options="multiline:true" class="easyui-textbox" name="faultanalysis" style="width: 530px; height: 30px;" />
                        </td>
                    </tr>
                    <%--<tr>
                        <td style="height: 50px;">返修原因:</td>
                        <td colspan="3">
                            <input data-options="multiline:true,prompt:'请填写返修原因'" class="easyui-textbox" id="arebackreason" name="rebackreason" style="width: 530px; height: 50px;" />
                        </td>
                    </tr>--%>
                    <%-- <tr>
                        <td style="height: 25px;">IPQC工号:</td>
                        <td colspan="3">
                            <input data-options="multiline:true,prompt:'请填写IPQC工号'" class="easyui-textbox" id="aipqcnumber" name="ipqcnumber" style="width: 530px; height: 50px;" />
                        </td>
                    </tr>--%>
                </tbody>
            </table>
        </form>
    </div>
    <div id="dlg-buttons">
        <a class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="QCConfirmNew()">确认</a>
        <a class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="Reback()">返修</a>
        <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel',plain:true" onclick="closeWindow('divNew')">取消</a>
    </div>
    <div style="clear: both">
    </div>

    <div id="divIPQC" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:true,buttons:'#dlgipqc'"
        style="width: 700px; height: 370px; padding: 5px;">
        <input data-options="multiline:true,prompt:'请填写返修原因',label:'返修原因'" class="easyui-textbox" id="arebackreason" name="rebackreason" style="width: 100%; height: 50px;" />
        <input data-options="prompt:'请填写IPQC工号',label:'IPQC工号'" class="easyui-textbox" id="aipqcnumber" name="ipqcnumber" style="width: 100%; height: 50px;" />
    </div>
    <div id="divIPConfirm" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:true,buttons:'#dlgipqcconfirm'"
        style="width: 700px; height: 370px; padding: 5px;">
        <input data-options="prompt:'请填写IPQC工号',label:'IPQC工号'" class="easyui-textbox" id="aipqcnumberConfirm" name="ipqcnumber" style="width: 100%; height: 50px;" />
    </div>
    <div id="dlgipqc">
        <a class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="Save()">返修</a>
        <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel',plain:true" onclick="closeWindow('divIPQC')">取消</a>
    </div>
    <div id="dlgipqcconfirm">
        <a class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="Confirm()">确认</a> 
    </div>
</body>
</html>