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
                fitColumns: false,
                 
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
                            { field: 'repairformno', title: '维修单号', width: 60, align: 'left', sortable: "true" },
                     { field: 'repairmanid', title: '维修员', width: 60, align: 'left', hidden: true, sortable: "true" },
                     { field: 'repairmanname', title: '维修员', width: 60, align: 'left', sortable: "true" },
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
                                        case "61":
                                            text = "61-生产员返修";
                                            break;
                                        case "62":
                                            text = "62 -挂单完结";
                                            break;
                                        case "64":
                                            text = "64-QC返修";
                                            break;
                                        case "63":
                                            text = "63-生产员返修";
                                            break;
                                        case "60":
                                            text = "维修完成";
                                            break;
                                        case "65":
                                            text = "65-生产组长返修";
                                            break;
                                       
                                        default:
                                            text = row.repairstatus + "维修完成";
                                            break;
                                    }
                                    return text;
                                }
                            },
                            { field: 'deviceid', title: '设备编号', width: 60, align: 'left', sortable: "true" },
                            { field: 'positiontext', title: '故障位置', width: 70, align: 'left', sortable: "true" },
                            { field: 'phenomenatext', title: '故障现象', width: 70, align: 'left', sortable: "true" },
                            { field: 'faultanalysis', title: '故障分析', width: 70, align: 'left', sortable: "true" },                            
                            { field: 'positiontext1', title: '故障位置1', width: 70, align: 'left', sortable: "true" },
                     { field: 'phenomenatext1', title: '故障现象1', width: 70, align: 'left', sortable: "true" },
                            { field: 'faulttime', title: '故障时间', width: 70, align: 'left', sortable: "true" },
                            { field: 'repairstime', title: '指派时间', width: 80, align: 'left', sortable: "true" },
                     { field: 'repairetime', title: '完成时间', width: 80, align: 'left', sortable: "true" },
                     {
                         field: 'qcconfirmtime', title: 'IPQC确认<br />时间', width: 80, align: 'left',sortable:"true"
                          
                     },
                        { field: 'confirmtime', title: '生产<br />确认时间', width: 70, align: 'left', sortable: "true" },
                     { field: 'ipqcnumber', title: 'IPQC确认', width: 80, align: 'left', sortable: "true" },
                        { field: 'confirmuser', title: '生产<br />确认工号', width: 70, align: 'left', sortable: "true" },

                          { field: 'mouldid', title: '模具编号1', width: 90, align: 'left', sortable: "true" },
                           { field: 'mouldid1', title: '模具编号2', width: 90, align: 'left', sortable: "true" },
                           {
                               field: 'newmouldid', title: '新模编号1', width: 90, align: 'left',
                               styler: function (value, row, index) {
                                   if (row.newmouldid != '') {
                                       return 'background-color:#4cae4c;color: #fff;border: 0px'
                                   }

                               }
                           },
                           {
                               field: 'newmouldid1', title: '新模编号2', width: 90, align: 'left',
                               styler: function (value, row, index) {
                                   if (row.newmouldid1 != '') {
                                       return 'background-color:#4cae4c;color: #fff;border: 0px'
                                   }

                               }
                           },
                     { field: 'rebackreason', title: '返修原因', width: 70, align: 'left' },
                     { field: 'applyuserid', title: '申请人', width: 70, align: 'left' }
                ]],
                onLoadSuccess: function (data) {
                    $($("td[field='newmouldid']")[0]).css({ "background": "#4cae4c", "color": "#fff" });
                    $($("td[field='newmouldid1']")[0]).css({ "background": "#4cae4c", "color": "#fff" });
                }
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

        function Export() {

             
            var queryParams = $('#tbEqManage').datagrid('options').queryParams;
            //請輸入關鍵字            
            queryParams.KeyWord = $('#txtKeyword').textbox("getValue");
            //queryParams.SearchType = SearchType;
            queryParams.DeviceId = $("#qequmentid").textbox("getValue");
            queryParams.RepairFormNO = $('#qrepairformno').textbox("getValue");
            queryParams.RepairmanId = $("#repairmanid").textbox("getValue");
            queryParams.RepairmanName = $('#repairmanname').textbox("getValue");
            queryParams.YearMonth = $('#qyearmonth').datebox("getValue");
            queryParams.EYearMonth = $('#eqyearmonth').datebox("getValue");
            if (queryParams.YearMonth && queryParams.EYearMonth) {

                var url = "../../ASHX/DMC/RepairRecord.ashx?M=download&SearchType=ByAdvanced&RepairStatus=100&fileName=维修记录.csv";
                if (queryParams.KeyWord) {
                    url = url + "&KeyWord=" + queryParams.KeyWord;
                }
                if (queryParams.RepairFormNO) {
                    url = url + "&RepairFormNO=" + queryParams.RepairFormNO;
                }
                if (queryParams.EqumentId) {
                    url = url + "&EqumentId=" + queryParams.EqumentId;
                }
                if (queryParams.RepairmanId) {
                    url = url + "&RepairmanId=" + queryParams.RepairmanId;
                }
                if (queryParams.RepairmanName) {
                    url = url + "&RepairmanName=" + queryParams.RepairmanName;
                }
                if (queryParams.YearMonth) {
                    url = url + "&YearMonth=" + queryParams.YearMonth;
                }
                if (queryParams.EYearMonth) {
                    url = url + "&EYearMonth=" + queryParams.EYearMonth;
                }
                if ($("#downloadForm").length <= 0) {
                    $("body").append("<form id='downloadForm' method='post' target='iframe'></form>");
                    $("body").append("<iframe id='ifm' name='iframe' style='display:none;'></iframe>");
                }
                $("#downloadForm").attr('action', url);
                $("#downloadForm").submit();
            }
            else
            {
                $.messager.alert({ title: '错误提示', msg: "请录入报修时间查询:" });
                return;
            }
            
        }

        //Grid初使加載的數據
        function Search(SearchType) {
            var queryParams = $('#tbEqManage').datagrid('options').queryParams;
            //請輸入關鍵字            
            queryParams.KeyWord = $('#txtKeyword').textbox("getValue");
            //queryParams.SearchType = SearchType;
            queryParams.DeviceId = $("#qequmentid").textbox("getValue");
            //queryParams.DeviceId = $("#qequmentid").textbox("getValue");
            queryParams.RepairFormNO = $('#qrepairformno').textbox("getValue");
            queryParams.RepairmanId = $("#repairmanid").textbox("getValue");
            queryParams.RepairmanName = $('#repairmanname').textbox("getValue");
            queryParams.YearMonth = $('#qyearmonth').datebox("getValue");
            queryParams.EYearMonth = $('#eqyearmonth').datebox("getValue");
            console.log(SearchType);
            if (SearchType == "ByAdvanced") {
                if (queryParams.YearMonth && queryParams.EYearMonth) {
                    console.log(queryParams);
                    $('#tbEqManage').datagrid('reload');
                }
                else {
                    $.messager.alert({ title: '错误提示', msg: "请录入报修时间查询:" });
                    return;
                }
            }
            else {
                $('#tbEqManage').datagrid('reload');
            }
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
            </div>
            <div class="r rightSearch">
            </div>
        </div>
        <div style="clear: both" >
        </div>
        <div class="squery" id="divSearch">
            <div class="sinquery">
                <table class="addCoyTB" style="height: 30px; border: 0px;">
                    <tr>
                        <td style="width: 70px;" >报修单号:</td>
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
                        <td style="width: 90px;">按报修时间查询:</td>
                        <td style="width: 150px;">
                            <input data-options="formatter: datetimeFormatter, parser: datetimeParser" class="easyui-datetimebox" name="yearmonth" id="qyearmonth" style="width: 150px;" />
                        </td>
                        <td style="width: 150px;">
                            <input data-options="formatter: datetimeFormatter, parser: datetimeParser" class="easyui-datetimebox" name="eyearmonth" id="eqyearmonth" style="width: 150px;" />
                        </td>
                        <td style="width: 150px;">
                            <a class="easyui-linkbutton" data-options="iconCls:'icon-excel',plain:true" href="javascript:void(0)" onclick='Export()'>导出</a>
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
