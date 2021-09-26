<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StatisticsReport.aspx.cs" Inherits="Web.UI.Equment.StatisticsReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>统计分析</title>
    <link href="../../css/base.css" rel="stylesheet" />
    <link href="../../css/platform.css?v=7" rel="stylesheet" />
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../JSPage/echarts.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //$('#aapplyuserid').combogrid({
            //    //是否折叠
            //    required: false,
            //    panelWidth: 430,
            //    //toolbar: '#toolbarleader',
            //    url: '../../ASHX/Organize/EmpManage.ashx?M=SearchGroup&SearchType=ByKey&rows=10000&page=1',
            //    idField: 'empid',
            //    textField: 'disaplytext',
            //    pagination: false,//是否分页
            //    rownumbers: true,//序号 
            //    method: 'post',
            //    fitColumns: true,
            //    fit: true,
            //    columns: [[
            //        {
            //            field: 'empid', title: '用户信息', width: 70, align: 'left', formatter: function (value, row, index) {
            //                return row.empname + "(" + row.empid + "/" + row.empmail + ")";
            //            }
            //        }
            //    ]],
            //    keyHandler: {
            //        enter: function () {
            //            var pClosed = $("#aapplyuserid").combogrid("panel").panel("options").closed;
            //            if (!pClosed) {
            //                $("#aapplyuserid").combogrid("hidePanel");
            //            }
            //            var grid = $('#aapplyuserid').combogrid("grid");
            //            var record = grid.datagrid("getSelected");
            //            if (record == null || record == undefined) {
            //                //如果未选中，则匹配当前回车选择的第一个
            //                var rows = grid.datagrid("getRows");
            //                if (rows.length > 0) {
            //                    //record = rows[0];
            //                    var rowIndex = grid.datagrid("getRowIndex", rows[0]);
            //                    //setPatient(record);
            //                    grid.datagrid("selectRow", rowIndex);
            //                }
            //                return;
            //            }
            //            else {
            //                var rowIndex = grid.datagrid("getRowIndex", record);
            //                grid.datagrid("selectRow", rowIndex);
            //                //setPatient(record);
            //            }
            //        },
            //        up: function () {
            //            var pClosed = $("#aapplyuserid").combogrid("panel").panel("options").closed;
            //            if (pClosed) {
            //                $("#aapplyuserid").combogrid("showPanel");
            //            }
            //            var grid = $('#aapplyuserid').combogrid("grid");
            //            var rowSelected = grid.datagrid("getSelected");
            //            if (rowSelected != null) {
            //                var rowIndex = grid.datagrid("getRowIndex", rowSelected);
            //                if (rowIndex > 0) {
            //                    rowIndex = rowIndex - 1;
            //                    grid.datagrid("selectRow", rowIndex);
            //                }
            //            } else if (grid.datagrid("getRows").length > 0) {
            //                grid.datagrid("selectRow", 0);
            //            }
            //        },
            //        down: function () {
            //            var pClosed = $("#aapplyuserid").combogrid("panel").panel("options").closed;
            //            if (pClosed) {
            //                $("#aapplyuserid").combogrid("showPanel");
            //            }
            //            var grid = $('#aapplyuserid').combogrid("grid");
            //            var rowSelected = grid.datagrid("getSelected");
            //            if (rowSelected != null) {
            //                var totalRow = grid.datagrid("getRows").length;
            //                var rowIndex = grid.datagrid("getRowIndex", rowSelected);
            //                if (rowIndex < totalRow - 1) {
            //                    rowIndex = rowIndex + 1;
            //                    grid.datagrid("selectRow", rowIndex);
            //                }
            //            } else if (grid.datagrid("getRows").length > 0) {
            //                grid.datagrid("selectRow", 0);
            //            }
            //        },
            //        query: function (q) {
            //            //$('#adeviceid').combogrid("setValue", null);
            //            $('#aapplyuserid').combogrid("grid").datagrid("clearSelections");
            //            $('#aapplyuserid').combogrid("grid").datagrid("reload", {
            //                'KeyWord': q,
            //                'sid2': Math.round(Math.random() * 1000)
            //            });
            //            $('#aapplyuserid').combogrid("grid").datagrid({
            //                onLoadSuccess: function (data) {
            //                    $('#aapplyuserid').combogrid("setText", q);
            //                }
            //            });
            //        }
            //    }
            //});
            $('#datetime1').datebox({
                onShowPanel: function () {// 显示日趋选择对象后再触发弹出月份层的事件，初始化时没有生成月份层    
                    span.trigger('click'); // 触发click事件弹出月份层    
                    if (!tds)
                        setTimeout(function () {// 延时触发获取月份对象，因为上面的事件触发和对象生成有时间间隔    
                            tds = p.find('div.calendar-menu-month-inner td');
                            tds.click(function (e) {
                                e.stopPropagation(); // 禁止冒泡执行easyui给月份绑定的事件    
                                var year = /\d{4}/.exec(span.html())[0]// 得到年份    
                                , month = parseInt($(this).attr('abbr'), 10) + 1; // 月份    
                                $('#datetime1').datebox('hidePanel')// 隐藏日期对象    
                                .datebox('setValue', year + '-' + month); // 设置日期的值    
                            });
                        }, 0);
                },
                parser: function (s) {// 配置parser，返回选择的日期    
                    if (!s)
                        return new Date();
                    var arr = s.split('-');
                    return new Date(parseInt(arr[0], 10), parseInt(arr[1], 10) - 1, 1);
                },
                formatter: function (d) {
                    var month = d.getMonth();
                    if (month <= 10) {
                        month = "0" + month;
                    }
                    if (d.getMonth() == 0) {
                        return d.getFullYear() - 1 + '-' + 12;
                    } else {
                        return d.getFullYear() + '-' + month;
                    }
                }// 配置formatter，只返回年月    
            });
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
                    if (newValue.indexOf("M01") > -1) {
                        $('#trnewmould').show();
                    } else {
                        $('#trnewmould').hide();
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
            //var p = $('#datetime1').datebox('panel'), // 日期选择对象    
            //tds = false, // 日期选择对象中月份    
            //span = p.find('span.calendar-text'); // 显示月份层的触发控件 
            $("#divdoing").parent().css("float", "left");
            $("#divwait").parent().css("float", "left");
            $("#divdo").parent().css("float", "left");
            $("#div4").parent().css("float", "left");
        });

        var isDay = false;
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
<body style="width: 100%; height: 100%;">
    <div style="padding: 1px 1px 1px 1px; height: calc(100% - 2px); width: 100%; background-color: white;" data-options="fit:true">
        <div class="easyui-panel" data-options="title:'过滤条件'" style="width: 100%; height: 70px; padding: 2px 2px 2px 2px; background-color: white;">
            <table>
                <tr>
                    <td>维修员:</td>
                    <%--<input data-options="prompt:'请选择维修员'" class="easyui-combogrid" name="applyuserid" id="aapplyuserid" style="width: 100%;" />--%>
                    <td>
                        <input class="easyui-textbox" id="repairmanid" name="repairmanid" data-options="prompt:'请输入维修员工号'" style="width: 120px;" /></td>
                    <td style="width: 90px;">按报修时间查询:</td>
                    <td style="width: 150px;">
                        <input data-options="formatter: datetimeFormatter, parser: datetimeParser" class="easyui-datetimebox" name="yearmonth" id="qyearmonth" style="width: 150px;" />
                    </td>
                    <td style="width: 150px;">
                        <input data-options="formatter: datetimeFormatter, parser: datetimeParser" class="easyui-datetimebox" name="eyearmonth" id="eqyearmonth" style="width: 150px;" />
                    </td>
                    <td class="auto-style2">故障位置:
                    </td>
                    <td>
                        <input data-options="prompt:'请选择故障位置'" class="easyui-combobox" style="width: 100%;" id="apositionid" name="positionid" />
                    </td>
                    <td>故障现象:</td>
                    <td>
                        <input data-options="prompt:'请选择故障现象'" class="easyui-combobox" style="width: 100%;" id="aphenomenaid" name="phenomenaid" />
                    </td>
                    <td style="width: 200px;">
                        <a class="easyui-linkbutton" data-options="toggle:true,group:'g1'" onclick="Search()">查询</a></td>
                </tr>
            </table>
        </div>
        <div class="easyui-panel" style="width: 50%; height: calc(50% - 30px); padding: 2px 2px 2px 2px; background-color: #F6F6F6;" id="divwait">
            <div style="width: calc(100% - 4px); height: calc(100% - 4px); float: left;" class="chart-chart" id="chart1"></div>
        </div>
        <div class="easyui-panel" style="width: 50%; height: calc(50% - 30px); padding: 2px 2px 2px 2px; background-color: #F6F6F6;" id="divdo">
            <div style="width: calc(100% - 4px); height: calc(100% - 4px); float: left;" class="chart-chart" id="chart2"></div>
        </div>
        <div class="easyui-panel" style="width: 60%; height: calc(50% - 30px); padding: 2px 2px 2px 2px; background-color: #F6F6F6;" id="divdoing">
            <div style="width: calc(100% - 4px); height: calc(100% - 4px); float: left;" class="chart-chart" id="chart3"></div>
        </div>
        <div class="easyui-panel" style="width: 40%; height: calc(50% - 30px); padding: 2px 2px 2px 2px; background-color: #F6F6F6;" id="divd4">
            <div style="width: calc(100% - 4px); height: calc(100% - 4px); float: left;" class="chart-chart" id="chart4"></div>
        </div>
    </div>
</body>
</html>
<script type="text/javascript">
    //id:'chart0'
    //加載下拉框的故障位置

    function loadChart1(id, RepairmanId, phenomenaid, YearMonth, EYearMonth) {
        var myChart = echarts.init(document.getElementById(id));
        // 指定图表的配置项和数据
        var option;
        var name = [];
        var totaltime = [];
        var workedtime = [];
        var app = {};
        var url = "../../ASHX/DMC/RepairRecord.ashx?M=SearchReport&SearchType=chart1";
        if (RepairmanId) {
            url = url + "&RepairmanId=" + RepairmanId;
        }
        if (phenomenaid) {
            url = url + "&phenomenaid=" + phenomenaid;
        }
        if (YearMonth && EYearMonth) {
            url = url + "&YearMonth=" + YearMonth;
            url = url + "&EYearMonth=" + EYearMonth;
        }
        var nmax = 0;
        var interval = 0;
        var xnmax = 0;
        var xinterval = 0;
        $.post(url,
            {},
            function (data) {
                for (var i = 0; i < data.length; i++) {
                    name[i] = data[i].username;
                    workedtime[i] = data[i].standgrade;
                    totaltime[i] = data[i].rfnum;
                    nmax = nmax + data[i].standgrade;
                    xnmax = xnmax + data[i].rfnum;

                }
                interval = Math.ceil(nmax / data.length);
                nmax = Math.max.apply(null, workedtime) + interval;

                xinterval = Math.ceil(xnmax / data.length);
                xnmax = Math.max.apply(null, totaltime) + xinterval;

                option = {
                    title: {
                        text: '维修员工单统计',
                        x: 'center',
                        textStyle: {
                            color: 'black',
                            fontSize: 25
                        }
                    },
                    grid: {
                        top: 50,
                        left: 30,
                        right: 20,
                        bottom: 20
                    },
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {
                            type: 'cross',
                            crossStyle: {
                                color: '#999'
                            }
                        }
                    },
                    toolbox: {
                        feature: {
                            saveAsImage: { show: false }
                        }
                    },
                    color: ['HotPink', 'DeepSkyBlue', 'LawnGreen', 'Gold', 'Red', 'Green'],
                    legend: {
                        x: 'left',
                        data: ['维修单数量', '总得分']
                    },
                    xAxis: [
                        {
                            type: 'category',
                            data: name,
                            axisPointer: {
                                type: 'shadow'
                            }
                        }
                    ],
                    yAxis: [
                        {
                            type: 'value',
                            name: '总得分',
                            min: 0,
                            max: nmax,
                            interval: interval,
                            splitLine: { show: false },
                            axisLabel: {
                                formatter: '{value}'
                            }
                        },
                        {
                            type: 'value',
                            name: '维修单数量',
                            min: 0,
                            splitLine: { show: false },
                            show: false,
                            axisTick: { //y轴刻度线
                                show: false
                            },
                            axisLine: { //y轴
                                show: false
                            },
                            max: xnmax,
                            interval: xinterval,
                            axisLabel: {
                                formatter: '{value}'
                            }
                        }
                    ],

                    series: [
                        {
                            name: '总得分',
                            type: 'bar',
                            label: {
                                show: true
                            },
                            data: workedtime
                        },
                        {
                            name: '维修单数量',
                            type: 'line',
                            label: {
                                show: true
                            },
                            yAxisIndex: 1,
                            data: totaltime
                        }
                    ]
                };
                myChart.setOption(option, true);
            },
            'json');
    }


    //id:'chart0'
    function loadChart2(id, RepairmanId, phenomenaid, YearMonth, EYearMonth) {
        var myChart = echarts.init(document.getElementById(id));
        var url = "../../ASHX/DMC/RepairRecord.ashx?M=SearchReport&SearchType=chart2";
        if (RepairmanId) {
            url = url + "&RepairmanId=" + RepairmanId;
        }
        if (phenomenaid) {
            url = url + "&phenomenaid=" + phenomenaid;
        }
        if (YearMonth && EYearMonth) {
            url = url + "&YearMonth=" + YearMonth;
            url = url + "&EYearMonth=" + EYearMonth;
        }
        var name = [];
        var manhoure = [];
        var nmax = 0;
        var interval = 0;
        var xnmax = 0;
        var xinterval = 0;
        $.post(url,
            {},
            function (data) {
                // 指定图表的配置项和数据
                for (var i = 0; i < data.length; i++) {
                    name[i] = data[i].deviceid;
                    manhoure[i] = data[i].manhoure;
                    nmax = nmax + data[i].manhoure;

                }
                interval = Math.ceil(nmax / data.length);
                nmax = Math.max.apply(null, manhoure) + interval;
                console.log([name, manhoure, interval, nmax]);
                var option = {
                    title: {
                        text: '停机时长',
                        x: 'center',
                        textStyle: {
                            color: 'black',
                            fontSize: 20
                        }
                    },
                    grid: {
                        top: 50,
                        left: 60,
                        right: 40,
                        bottom: 20
                    },
                    color: ['HotPink', 'DeepSkyBlue', 'LawnGreen', 'Gold', 'Red', 'Green'],
                    tooltip: {
                        trigger: 'item',
                        formatter: "{a} <br/>{b} : {c} 分"
                    },
                    toolbox: {
                        feature: {
                            saveAsImage: { show: false }
                        }
                    },
                    //legend: {
                    //    data: name
                    //},
                    calculable: true,
                    xAxis: [
                        {
                            type: 'category',
                            data: name,
                            axisPointer: {
                                type: 'shadow'
                            },
                            axisTick: {
                                interval: 0
                            }
                        }
                    ],
                    yAxis: [
                        {
                            type: 'value',
                            name: '总时长',
                            min: 0,
                            max: nmax,
                            interval: interval,
                            splitLine: { show: false },
                            axisLabel: {
                                formatter: '{value}'
                            }
                        },
                        {
                            type: 'value',
                            name: '停机时长',
                            min: 0,
                            splitLine: { show: false },
                            show: false,
                            axisTick: { //y轴刻度线
                                show: false
                            },
                            axisLine: { //y轴
                                show: false
                            },
                            max: xnmax,
                            interval: xinterval,
                            axisLabel: {
                                formatter: '{value}'
                            }
                        }
                    ],
                    series: [
                        {
                            name: '停机时长',
                            type: 'bar',
                            barWidth: '50%',
                            label: {
                                show: true
                            },
                            data: manhoure
                        }
                    ]
                };
                myChart.setOption(option, true);
            },
            'json');

    }


    function loadChart3(id, RepairmanId, phenomenaid, YearMonth, EYearMonth) {
        var alarmTime = "2021-01-19 14:56:31";
        var myChart = echarts.init(document.getElementById(id));
        var option;
        var url = "../../ASHX/DMC/RepairRecord.ashx?M=SearchReport&SearchType=chart3";
        if (RepairmanId) {
            url = url + "&RepairmanId=" + RepairmanId;
        }
        if (phenomenaid) {
            url = url + "&phenomenaid=" + phenomenaid;
        }
        if (YearMonth && EYearMonth) {
            url = url + "&YearMonth=" + YearMonth;
            url = url + "&EYearMonth=" + EYearMonth;
        }

        var positiontext = [];
        var name = [];
        var manhoure = [];
        var nmax = 0;
        var interval = 0;
        var xnmax = 0;
        var xinterval = 0;
        $.post(url,
            {},
            function (data) {
                // 指定图表的配置项和数据
                option = {
                    title: {
                        x: 'right',
                        textStyle: {
                            color: 'black',
                            fontSize: 25
                        }
                    },
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {            // Use axis to trigger tooltip
                            type: 'shadow'        // 'shadow' as default; can also be 'line' or 'shadow'
                        },
                        position: function (point, params, dom, rect, size) {
                            dom.style.transform = 'translateZ(0)'
                        },
                        confine: true
                    },
                    toolbox: {
                        feature: {
                            saveAsImage: { show: false }
                        }
                    },
                    color: ['HotPink', 'DeepSkyBlue', 'LawnGreen', 'Gold', 'Red', 'Green', 'LightPink', 'Orchid', 'Magenta', 'SlateBlue'],
                    legend: {
                        show: true,
                        textStyle:{colr:'#fff',fontSize:10},
                        x: 'left',
                        data: data.PositionText
                    },
                    grid: {
                        top: 40,
                        left: 60,
                        right: 20,
                        bottom: 20
                    },
                    xAxis: {
                        type: 'value'
                    },
                    yAxis: {
                        type: 'category',
                        data: data.UserName
                    },
                    series: []
                };
                for (var i = 0; i < data.Series.length; i++) {
                    option.series.push({
                        name: data.Series[i].PositionText,
                        type: 'bar',
                        stack: 'total',
                        barWidth: '70%',
                        label: {
                            show: true
                        },
                        emphasis: {
                            focus: 'series'
                        },
                        data: data.Series[i].Datas
                    });
                }
                // 使用刚指定的配置项和数据显示图表。
                myChart.setOption(option, true);
            },
            'json');

    }

    function loadChart4(id, RepairmanId, phenomenaid, YearMonth, EYearMonth) {
        var myChart = echarts.init(document.getElementById(id));
        // 指定图表的配置项和数据
        var url = "../../ASHX/DMC/RepairRecord.ashx?M=SearchReport&SearchType=chart4";
        if (RepairmanId) {
            url = url + "&RepairmanId=" + RepairmanId;
        }
        if (phenomenaid) {
            url = url + "&phenomenaid=" + phenomenaid;
        }
        if (YearMonth && EYearMonth) {
            url = url + "&YearMonth=" + YearMonth;
            url = url + "&EYearMonth=" + EYearMonth;
        }
        var name = [];
        var manhoure = [];
        var nmax = 0;
        var interval = 0;
        var xnmax = 0;
        var xinterval = 0;
        $.post(url,
            {},
            function (data) {
                // 指定图表的配置项和数据
                for (var i = 0; i < data.length; i++) {
                    name[i] = data[i].positiontext;
                    manhoure[i] = { value: data[i].manhoure, name: data[i].positiontext };
                    nmax = nmax + data[i].manhoure;
                }
                var option = {
                    title: {
                        text: '维修时长',
                        x: 'center',
                        textStyle: {
                            color: 'black',
                            fontSize: 20
                        }
                    },
                    tooltip: {
                        trigger: 'item',
                        formatter: "{a} <br/>{b} : {c} 分"
                    },
                    toolbox: {
                        feature: {
                            saveAsImage: { show: false }
                        }
                    },
                    legend: {
                        orient: 'vertical',
                        x: 'left',
                        y: "top",
                        top: 30,
                        textStyle: {
                            
                            fontSize: 10
                        },
                        //itemWidth: 70,
                        //itemHeight: 30,
                        //formatter: '{name}',
                        //textStyle: {
                        //    color: 'black'
                        //},
                        data: name
                    },
                    calculable: true,
                    color: ['HotPink', 'DeepSkyBlue', 'LawnGreen', 'Gold', 'Red', 'Green', 'LightPink', 'Orchid', 'Magenta', 'SlateBlue'],
                    series: [
                        {
                            name: '停机时长',
                            type: 'pie',    // 设置图表类型为饼图
                            radius: '80%',  // 饼图的半径，外半径为可视区尺寸（容器高宽中较小一项）的 55% 长度。
                            center: ['70%', '60%'],//饼图的位置 
                            //label: {            //饼图图形上的文本标签
                            //    normal: {
                            //        show: true,
                            //        position: 'inner', //标签的位置
                            //        textStyle: {
                            //            fontWeight: 300,
                            //            fontSize: 12    //文字的字体大小
                            //        }
                            //    }
                            //},
                            data: manhoure,
                            itemStyle: {
                                normal: {
                                    label: {
                                        color: "black",
                                        fontsize: "10px",
                                        show: true,
                                        position: "outside",
                                        formatter: "{d}%"
                                    }
                                }
                            }
                        }
                    ]
                };
                myChart.setOption(option, true);
            },
            'json');

    }
    var myDate = new Date();
    var year = myDate.getFullYear();
    var month = myDate.getMonth();
    var YearMonth = year + "-" + month.toString();
    var EYearMonth = year + "-" + (month + 1).toString();
    //var EYearMonth = YearMonth.setMonth(YearMonth.getMonth() + 1);

    YearMonth = datetimeFormatter(YearMonth);
    EYearMonth = datetimeFormatter(EYearMonth);
    var RepairmanId = "";
    var phenomenaid = "";
    console.log([RepairmanId, phenomenaid, YearMonth, EYearMonth]);

    loadChart1("chart1", RepairmanId, phenomenaid, YearMonth, EYearMonth);
    loadChart2("chart2", RepairmanId, phenomenaid, YearMonth, EYearMonth);
    loadChart3("chart3", RepairmanId, phenomenaid, YearMonth, EYearMonth);
    loadChart4("chart4", RepairmanId, phenomenaid, YearMonth, EYearMonth);
    function Search() {
        var RepairmanId = $('#repairmanid').textbox("getValue");
        var phenomenaid = $('#aphenomenaid').combobox("getValue");
        var YearMonth = $('#qyearmonth').datebox("getValue");
        var EYearMonth = $('#eqyearmonth').datebox("getValue");

        if (YearMonth && EYearMonth) {
            console.log([RepairmanId, phenomenaid, YearMonth, EYearMonth]);
            loadChart1("chart1", RepairmanId, phenomenaid, YearMonth, EYearMonth);
            loadChart2("chart2", RepairmanId, phenomenaid, YearMonth, EYearMonth);
            loadChart3("chart3", RepairmanId, phenomenaid, YearMonth, EYearMonth);
            loadChart4("chart4", RepairmanId, phenomenaid, YearMonth, EYearMonth);
        }
        else {
            $.messager.alert({ title: '错误提示', msg: "请录入报修时间查询:" });
            return;
        }


    }
</script>
