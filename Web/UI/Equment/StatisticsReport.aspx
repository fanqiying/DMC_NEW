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
            var p = $('#datetime1').datebox('panel'), // 日期选择对象    
            tds = false, // 日期选择对象中月份    
            span = p.find('span.calendar-text'); // 显示月份层的触发控件 
            $("#divdoing").parent().css("margin-top", "2px");
            $("#divwait").parent().css("float", "left");
            $("#divdo").parent().css("float", "right");
        });

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
    </script>
</head>
<body style="width: 100%; height: 100%;">
    <div style="padding: 1px 1px 1px 1px; height: calc(100% - 2px); width: 100%; background-color: white;" data-options="fit:true">
        <div class="easyui-panel" data-options="title:'过滤条件'" style="width: 100%; height: 70px; padding: 2px 2px 2px 2px; background-color: white;">
            <table>
                <tr>
                    <td style="width: 200px;">
                        <input data-options="prompt:'请选择维修员',label:'维修员：'" class="easyui-combogrid" name="applyuserid" id="aapplyuserid" style="width: 200px;" /></td>
                    <td style="width: 200px;">
                        <input data-options="prompt:'请选择月份',label:'月份：',formatter: dateFormatter, parser: dateParser" class="easyui-datebox" name="applyuserid" id="datetime1" style="width: 200px;" /></td>
                    <td style="width: 200px;">
                        <input data-options="prompt:'请填入故障类型',label:'故障类型：'" class="easyui-textbox" id="adeviceid" name="deviceid" style="width: 200px;" /></td>
                </tr>
            </table>

        </div>
        <div class="easyui-panel" style="width: 50%; height: calc(50% - 30px); padding: 2px 2px 2px 2px; background-color: #F6F6F6;" id="divwait">
            <div style="width: calc(100% - 4px); height: calc(100% - 4px); float: left;" class="chart-chart" id="chart1"></div>
        </div>
        <div class="easyui-panel" style="width: 50%; height: calc(50% - 30px); padding: 2px 2px 2px 2px; background-color: #F6F6F6;" id="divdo">
            <div style="width: calc(100% - 4px); height: calc(100% - 4px); float: left;" class="chart-chart" id="chart3"></div>
        </div>
        <div class="easyui-panel" style="width: 100%; height: calc(50% - 40px); padding: 2px 2px 2px 2px; background-color: #F6F6F6;" id="divdoing">
            <div style="width: calc(100% - 4px); height: calc(100% - 4px); float: left;" class="chart-chart" id="chart2"></div>
        </div>
    </div>
</body>
</html>
<script type="text/javascript">
    //id:'chart0'
    function loadChart(id) {
        var myChart = echarts.init(document.getElementById(id));
        // 指定图表的配置项和数据
        var option;

        var app = {};
        option = {
            title: {
                text: '维修员工单统计',
                x: 'center',
                textStyle: {
                    color: 'black',
                    fontSize: 25
                }
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
            legend: {
                x: 'left',
                data: ['维修单数量', '总得分']
            },
            xAxis: [
                {
                    type: 'category',
                    data: ['张应辉', '陈智平', '柏建军', '周秀慧', '王诗伍', '罗静', '蒋玉朝', '李土源', '吴富盛', '姚煌智', '龙晓彬', '麦康瑞'],
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
                    max: 250,
                    interval: 50,
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
                    show:false,
                    axisTick: { //y轴刻度线
                        show: false
                    },
                    axisLine: { //y轴
                        show: false
                    },
                    max: 90,
                    interval: 10,
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
                    data: [135, 130, 128, 125, 110, 108, 100, 98, 88, 86, 85, 80]
                    
                   
                },
                {
                    name: '维修单数量',
                    type: 'line',
                    label: {
                        show: true
                    },
                    yAxisIndex:1,
                    data: [80, 57, 80, 48, 78, 80, 81, 85, 68, 62, 69, 76]
                }
            ]
        };
        myChart.setOption(option);
    }
    loadChart("chart1");

    //id:'chart0'
    function loadChart3(id) {
        var myChart = echarts.init(document.getElementById(id));
        // 指定图表的配置项和数据
        var option = {
            title: {
                text: '停机时长',
                x: 'center',
                textStyle: {
                    color: 'black',
                    fontSize: 25
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
                top: 40,
                itemWidth: 70,
                itemHeight: 30,
                formatter: '{name}',
                textStyle: {
                    color: 'black'
                },
                data: [
                    { name: 'C10', icon: 'rect' },
                    { name: 'C11', icon: 'rect' },
                    { name: 'C12', icon: 'rect' },
                    { name: 'C13', icon: 'rect' },
                    { name: 'C14', icon: 'rect' },
                    { name: '其他设备', icon: 'rect' },
                ]
            },
            calculable: true,
            color: ['HotPink', 'DeepSkyBlue', 'LawnGreen', 'Gold', 'Red', 'Green'],

            series: [
                {
                    name: '停机时长',
                    type: 'pie',    // 设置图表类型为饼图
                    radius: '80%',  // 饼图的半径，外半径为可视区尺寸（容器高宽中较小一项）的 55% 长度。
                    center: ['60%', '60%'],//饼图的位置 
                    label: {            //饼图图形上的文本标签
                        normal: {
                            show: true,
                            position: 'inner', //标签的位置
                            textStyle: {
                                fontWeight: 300,
                                fontSize: 12    //文字的字体大小
                            }


                        }
                    },
                    data: [          // 数据数组，name 为数据项名称，value 为数据项值
                        { value: 335, name: 'C10' },
                        { value: 294, name: 'C11' },
                        { value: 320, name: 'C12' },
                        { value: 305, name: 'C13' },
                        { value: 335, name: 'C14' },
                        { value: 455, name: '其他设备' }
                    ]
                }
            ]
        };
        myChart.setOption(option);
    }
    loadChart3("chart3");

    function myChart() {
        var alarmTime = "2021-01-19 14:56:31";

        var option;

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
                }
            },
            toolbox: {
                feature: {

                    saveAsImage: { show: false }
                }
            },
            legend: {
                x: 'left',
                data: ['转模->深打凹/反打凹/复杂打凹/加隔纸的转模', '模具->PI深度/耳朵倾斜/偏移', '转模->深打凹/反打凹/复杂打凹/不加隔纸的转模', '模具->矽座倾斜/矽座平面度/中心偏移', '模具->切脚/倒角披锋/歪脚仔/高低脚', '其他故障']
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            xAxis: {
                type: 'value'
            },
            yAxis: {
                type: 'category',

                data: ['张应辉', '陈智平', '柏建军', '周秀慧', '王诗伍', '罗静', '蒋玉朝', '李土源', '吴富盛', '姚煌智']
            },
            series: [
                {
                    name: '转模->深打凹/反打凹/复杂打凹/加隔纸的转模',
                    type: 'bar',
                    stack: 'total',
                    label: {
                        show: true
                    },
                    emphasis: {
                        focus: 'series'
                    },
                    data: [302, 301, 334, 390, 330, 320, 340, 350, 400, 450]
                },
                {
                    name: '模具->PI深度/耳朵倾斜/偏移',
                    type: 'bar',
                    stack: 'total',
                    label: {
                        show: true
                    },
                    emphasis: {
                        focus: 'series'
                    },
                    data: [292, 101, 134, 90, 230, 210, 350, 300, 400, 500]
                },
                {
                    name: '转模->深打凹/反打凹/复杂打凹/不加隔纸的转模',
                    type: 'bar',
                    stack: 'total',
                    label: {
                        show: true
                    },
                    emphasis: {
                        focus: 'series'
                    },
                    data: [182, 191, 234, 290, 330, 310, 360, 380, 390, 509]
                },
                {
                    name: '模具->矽座倾斜/矽座平面度/中心偏移',
                    type: 'bar',
                    stack: 'total',
                    label: {
                        show: true
                    },
                    emphasis: {
                        focus: 'series'
                    },
                    data: [282, 201, 154, 190, 330, 410, 400, 403, 410, 500]
                },
                {
                    name: '模具->切脚/倒角披锋/歪脚仔/高低脚',
                    type: 'bar',
                    stack: 'total',
                    label: {
                        show: true
                    },
                    emphasis: {
                        focus: 'series'
                    },
                    data: [272, 901, 934, 1290, 1330, 1320, 1320, 1400, 1348, 1300]
                },
                {
                    name: '其他故障',
                    type: 'bar',
                    stack: 'total',
                    label: {
                        show: true
                    },
                    emphasis: {
                        focus: 'series'
                    },
                    data: [262, 901, 934, 1290, 1330, 1320, 1320, 1400, 1348, 1300]
                }

            ]
        };

        var chart = echarts.init(document.getElementById('chart2'));
        // 使用刚指定的配置项和数据显示图表。
        chart.setOption(option);
    }
    myChart();
</script>
