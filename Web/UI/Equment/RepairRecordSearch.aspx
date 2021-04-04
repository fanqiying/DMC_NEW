<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepairRecordSearch.aspx.cs" Inherits="Web.UI.Equment.RepairRecordSearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>工时报表</title>
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="../../easyUI15/locale/easyui-lang-zh_TW.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            //綁定datagrid
            $('#tbEqManage').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: '../../ASHX/DMC/RepairRecord.ashx?M=Search&SearchType=ByAdvanced&RepairStatus=100',
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
                pageSize: 20,
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
                                        default:
                                            text = row.repairstatus+"维修完成";
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
                     { field: 'positiontext1', title: '故障位置1', width: 70, align: 'left' },
                     { field: 'phenomenatext1', title: '故障现象1', width: 70, align: 'left' },
                     { field: 'applyuserid', title: '申请人', width: 50, align: 'left' }
                ]]
            });
            $(window).resize(function () {
                $('#tbEqManage').datagrid('resize');
            });
            //設置分页控件屬性 
            var p = $('#tbEqManage').datagrid('getPager');
            $(p).pagination({
                pageSize: 20,
                pageList: [10, 15, 20, 25, 30],
                beforePageText: '第',
                afterPageText: '页 共{pages}页',
                displayMsg: '当前显示 {from} - {to} 条记录 共 {total} 条记录'
            });
        });
        //Grid初使加載的數據
        function Search(SearchType) {
            var queryParams = $('#tbEqManage').datagrid('options').queryParams;
            //請輸入關鍵字            
            queryParams.KeyWord = $('#txtKeyword').textbox("getValue");
            //queryParams.SearchType = SearchType;
            queryParams.EqumentId = $("#qequmentid").textbox("getValue");
            queryParams.RepairFormNO = $('#qrepairformno').textbox("getValue");
            queryParams.RepairmanId = $("#repairmanid").textbox("getValue");
            queryParams.RepairmanName = $('#repairmanname').textbox("getValue");
            queryParams.YearMonth = $('#qyearmonth').datebox("getValue");
            
            $('#tbEqManage').datagrid('reload');
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

        /**
         * 时间格式化
         * @param value
         * @returns {string}
        */
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
            </div>
        </div>
        <div style="clear: both">
        </div>
        <div class="squery" id="divSearch">
            <div class="sinquery">
                <table class="addCoyTB" style="height: 30px; border: 0px;">
                    <tr>
                        <td style="width: 70px;">报修单号:</td>
                        <td style="width: 165px;">
                            <input class="easyui-textbox" id="qrepairformno" name="repairformno" data-options="prompt:'请输入报修单号'" style="width: 150px;" />
                        </td>
                        <td style="width: 70px;">设备编号:</td>
                        <td style="width: 120px;">
                            <input class="easyui-textbox" id="qequmentid" name="equmentid" data-options="prompt:'请输入设备编号'" style="width: 120px;" />
                        </td>
                        <td style="width: 90px;">维修员工号:</td>
                        <td style="width: 120px;">
                            <input class="easyui-textbox" id="repairmanid" name="repairmanid" data-options="prompt:'请输入维修员工号'" style="width: 120px;" />
                        </td>
                        <td style="width: 90px;">维修员姓名:</td>
                        <td style="width: 120px;">
                            <input class="easyui-textbox" id="repairmanname" name="repairmanname" data-options="prompt:'请输入维修员姓名'" style="width: 120px;" />
                        </td>
                        <td style="width: 90px;">按年月查询:</td>
                        <td style="width: 150px;">
                            <input data-options="formatter: dateFormatter, parser: dateParser" class="easyui-datebox" name="yearmonth" id="qyearmonth" style="width: 150px;" />
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
        <table id="tbEqManage" data-options="fit:false">
        </table>
    </div>
</body>
</html>