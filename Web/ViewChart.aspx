<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewChart.aspx.cs" Inherits="Web.ViewChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <title>實時產能狀況</title>
    <link rel="stylesheet" type="text/css" href="easyUI15/themes/metro/easyui.css" />
    <link rel="stylesheet" type="text/css" href="easyUI15/themes/mobile.css" />
    <link rel="stylesheet" type="text/css" href="easyUI15/themes/icon.css" />
    <script src="easyUI15/jquery.min.js" type="text/javascript"></script>
    <script src="easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="easyUI15/jquery.easyui.mobile.js" type="text/javascript"></script>
    <script src="JSPage/echarts.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //綁定datagrid
            $('#tbRepairAssign').datagrid({
                fitColumns: true,
                border: true,
                //固定序號
                rownumbers: false,
                scrollbarSize: 2,
                //是否可以多選
                singleSelect: true,
                remoteSort: false,
                idField: 'autoid',
                loadMsg: '數據加載中...',//數據加載中
                //是否顯示分页
                pagination: false,
                //可動列
                columns: [[
                           { field: 'classtype', title: '班别', width: 50, align: 'left' },
                           { field: 'runtime', title: '启动时间', width: 100, align: 'left' },
                           {
                               field: 'autoid', title: '操作', width: 30, align: 'center', formatter: function (value, row, index) {
                                   return '<a href="#" onclick="QCConfirm(\'' + index + '\')">进入</a>';
                               }
                           }
                ]]
            });

            //設置分页控件屬性 
            //var p = $('#tbRepairAssign').datagrid('getPager');
            //$(p).pagination({
            //    pageSize: 10,
            //    pageList: [10, 15, 20, 25, 30],
            //    beforePageText: '第',
            //    afterPageText: '页 共{pages}页',
            //    displayMsg: '当前显示 {from} - {to} 条记录 共 {total} 条记录'
            //});
            var json = [
                { classtype: "A3 -月薪A班", runtime: "2021-01-29 08:09:35" },
                { classtype: "A2 -月薪B班", runtime: "2021-01-29 08:09:35" }
            ];
            $('#tbRepairAssign').datagrid("loadData", json);
            //綁定datagrid
            $('#tbList').datagrid({
                fitColumns: true,
                border: true,
                //固定序號
                rownumbers: false,
                scrollbarSize: 2,
                //是否可以多選
                singleSelect: true,
                remoteSort: false,
                idField: 'autoid',
                loadMsg: '數據加載中...',//數據加載中
                //是否顯示分页
                pagination: false,
                //可動列
                columns: [[
                           { field: 'partid', title: '料号', width: 100, align: 'left' },
                           { field: 'plannum', title: '計劃產量', width: 80, align: 'left' },
                           { field: 'tragetnum', title: '目標產量', width: 80, align: 'left' },
                           { field: 'nownum', title: '實際產量', width: 80, align: 'left' }
                ]]
            });
            var json1 = [
               { partid: "DBPB0428B2UP055-1", plannum: "3500" ,tragetnum:'1799',nownum:'1497'},
               { partid: "DBTA0328B2LP030-1", plannum: "1100",tragetnum:'400',nownum:'13' },
               { partid: "DBTA0428B2SP211-1", plannum: "800",tragetnum:'1350',nownum:'876' },
               { partid: "DV05028B12UP008", plannum: "704",tragetnum:'685',nownum:'668' }
            ];
            $('#tbList').datagrid("loadData", json1);
        });

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
            return year + "-" + month + "-" + day;
        }

        function dateParser(s) {
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

        function QCConfirm(index) {
            $('#dlgBasicInfo').dialog('close');
        }
    </script>
</head>
<body style="height: 100%; margin: 0">
    <div class="easyui-navpanel" style="left: 2px; right: 2px; top: 2px; bottom: 2px;">
        <header>
            <div class="m-toolbar">
                <div class="m-left">
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-large-chart',plain:true,outline:true" onclick="$('#dlgBasicInfo').dialog('open').dialog('center')">条件设置</a>
                </div>
                <div class="m-title">
                    <span style="color: #27A9E3;">實時產能狀況</span>
                </div>
                <div class="m-right">
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-reload',plain:true,outline:true"></a>
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-excel',plain:true,outline:true">导出档案</a>
                </div>
            </div>
        </header>
        <div class="easyui-accordion" style="margin-top: 2px; margin-bottom: 2px;" data-options="fit:false,border:true">
            <div title="明细信息" style="height: 280px; padding: 2px;" data-option="selected:false">
                <div style="margin-bottom: 2px">
                    <input disabled="disabled" class="easyui-textbox" value="TFD30" data-options="label:'部門'" style="width: 49%;" />
                    <input disabled="disabled" class="easyui-textbox" value="FHA01B" data-options="label:'線別'" style="width: 49%;" />
                </div>
                <div style="margin-bottom: 2px">
                    <input disabled="disabled" class="easyui-textbox" value="B1" data-options="label:'班別'" style="width: 49%;" />
                    <input disabled="disabled" class="easyui-textbox" value="sz62783-文雪姣" data-options="label:'組長'" style="width: 49%;" />
                </div>
                <div style="margin-bottom: 2px">
                    <input disabled="disabled" class="easyui-textbox" value="2021-02-03 04:09:10 - 2021-02-03 14:20:35" data-options="label:'運行時間'" style="width: 98%;" />
                </div>
                <div style="margin-bottom: 2px">
                    <input disabled="disabled" class="easyui-textbox" value="2021-02-03 08:00:00 - 2021-02-03 21:00:00" data-options="label:'工作時間'" style="width: 98%;" />
                </div>
                <div style="margin-bottom: 2px">
                    <table id="tbList" data-options="fit:false" style="height: 100px; width: 98%;">
                    </table>
                </div>
            </div>
        </div>
        <div id="chart1"></div>
    </div>

    <div id="dlgBasicInfo" class="easyui-dialog" style="padding: 20px 6px; width: 90%;" data-options="inline:true,modal:true,closed:true,title:'条件设置'">
        <div style="margin-bottom: 10px">
            <input class="easyui-combobox" value="TFC70" data-options="prompt:'请选择部門',label:'部門'" style="width: 100%; height: 30px" />
        </div>
        <div style="margin-bottom: 10px">
            <input class="easyui-combobox" value="FTB03B" data-options="prompt:'请选择線別',label:'線別'" style="width: 100%; height: 30px" />
        </div>
        <div style="margin-bottom: 10px">
            <input class="easyui-datebox" value="2021-01-29" data-options="prompt:'请选择日期',label:'日期',formatter: dateFormatter, parser: dateParser" style="width: 100%; height: 30px" />
        </div>
        <div style="margin-bottom: 10px">
            <table id="tbRepairAssign" data-options="fit:false" style="height: 120px;">
            </table>
        </div>
        <%-- <div style="margin-bottom: 10px">
            <div style="width: 50%; float: left;"><a href="javascript:void(0)" class="easyui-linkbutton" style="width: 100%; height: 35px" onclick="$('#dlgBasicInfo').dialog('close')">确定</a></div>
            <div style="width: 50%; float: right;"><a href="javascript:void(0)" class="easyui-linkbutton" style="width: 100%; height: 35px" onclick="$('#dlgBasicInfo').dialog('close')">取消</a></div>
        </div>--%>
    </div>
</body>
</html>
<script type="text/javascript">
    Date.prototype.Format = function (fmt) { //author: meizz 
        var o = {
            "M+": this.getMonth() + 1, //月份 
            "d+": this.getDate(), //日 
            "h+": this.getHours(), //小时 
            "m+": this.getMinutes(), //分 
            "s+": this.getSeconds(), //秒 
            "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
            "S": this.getMilliseconds() //毫秒 
        };
        if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    }
</script>
<script type="text/javascript">
    function loadChart(id) {
        var myChart = echarts.init(document.getElementById(id));
        // 指定图表的配置项和数据
        var option = {
            title: {
                text: '產能負荷'
            },
            legend: {
            },
            tooltip: {
                trigger: 'item',
                formatter: function (params) {
                    if (params.componentType == "markArea") {
                        return null;
                    }
                    return params.value[1].Format("MM-dd hh:mm") + '<br />' +
                    '<div style="font-size:23px;float:left;color:' + params.color + '">●</div>' + '&thinsp;' +
                    params.seriesName + '<br />' +
                    '實際產量：' + params.value[0] + '<br />' +
                    '標準產能：' + (params.value[5].toFixed(2) / params.value[3]) + '<br />' +
                    '工作人數：' + params.value[6]
                }
            },
            //toolbox: {
            //    show: true,
            //    feature: {
            //        restore: {},
            //        saveAsImage: {}
            //    }
            //},
            xAxis: [{
                type: 'time',
                name: '时间',
                interval: 3600000,
                axisLabel: {
                    formatter: function (value, index) {
                        var date = new Date(value);
                        return date.Format("hh:mm");
                    }
                }
            }],
            yAxis: [{
                type: 'value',
                name: '實際產量',
                scale: true,
                min: 0,
                boundaryGap: [0.2, 0.2]
            }]
        };
        myChart.setOption(option);
        myChart.resize();
    }
    //loadChart("chart1");

    var data = {"result":true,"legends":["DBPB0428B2UP055-1","DBTA0328B2LP030-1","DBTA0428B2SP211-1","DV05028B12UP008"],"param":[],"entity":{"aliasName":"B1  ","classId":"AVC2TFD30B1","company":"AVC2","dept":"TFD30","endDate":"2021-02-03 14:20:35.22","parts":[{"endTime":"","partId":"DBPB0428B2UP055-1","planOutput":"3500","prod":"1799.28","prodTarget":16.65999984741211,"production":"1497","rate":"0.0%","ratesType":"1","scrap":"0.0%","startTime":"","testStation":"","usePeopNum":27},{"endTime":"","partId":"DBTA0328B2LP030-1","planOutput":"1100","prod":"399.96","prodTarget":22.219999313354492,"production":"1","rate":"0.0%","ratesType":"1","scrap":"0.0%","startTime":"","testStation":"","usePeopNum":18},{"endTime":"","partId":"DBTA0428B2SP211-1","planOutput":"800","prod":"1350.36","prodTarget":20.459999084472656,"production":"876","rate":"0.0%","ratesType":"1","scrap":"0.0%","startTime":"","testStation":"","usePeopNum":22},{"endTime":"","partId":"DV05028B12UP008","planOutput":"704","prod":"685.4","prodTarget":14.899999618530273,"production":"668","rate":"0.0%","ratesType":"1","scrap":"0.0%","startTime":"","testStation":"","usePeopNum":23}],"prodLine":"FHA01B","queryEndDate":"2021-02-03 21:00:00","queryStartDate":"2021-02-03 08:00:00","refRate":120,"restTimes":"","rests":[],"runDate":"2021-02-03 04:07:02","smtSignSetId":"AVC220210203083411","teamLeader":"sz62783-文雪姣","times":[{"endTime":"2021-02-03 09:00:00","list":[{"endTime":"","partId":"DBPB0428B2UP055-1","planOutput":"","prod":"","prodTarget":16.65999984741211,"production":"245","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":27},{"endTime":"","partId":"DBTA0428B2SP211-1","planOutput":"","prod":"","prodTarget":20.459999084472656,"production":"288","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":22}],"startTime":"2021-02-03 08:00:00"},{"endTime":"2021-02-03 10:00:00","list":[{"endTime":"","partId":"DBPB0428B2UP055-1","planOutput":"","prod":"","prodTarget":16.65999984741211,"production":"248","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":27},{"endTime":"","partId":"DBTA0428B2SP211-1","planOutput":"","prod":"","prodTarget":20.459999084472656,"production":"30","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":22},{"endTime":"","partId":"DV05028B12UP008","planOutput":"","prod":"","prodTarget":14.899999618530273,"production":"1","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":23}],"startTime":"2021-02-03 09:00:00"},{"endTime":"2021-02-03 11:00:00","list":[{"endTime":"","partId":"DBPB0428B2UP055-1","planOutput":"","prod":"","prodTarget":16.65999984741211,"production":"238","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":27},{"endTime":"","partId":"DBTA0428B2SP211-1","planOutput":"","prod":"","prodTarget":20.459999084472656,"production":"46","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":22}],"startTime":"2021-02-03 10:00:00"},{"endTime":"2021-02-03 12:00:00","list":[{"endTime":"","partId":"DBPB0428B2UP055-1","planOutput":"","prod":"","prodTarget":16.65999984741211,"production":"36","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":27},{"endTime":"","partId":"DBTA0428B2SP211-1","planOutput":"","prod":"","prodTarget":20.459999084472656,"production":"269","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":22}],"startTime":"2021-02-03 11:00:00"},{"endTime":"2021-02-03 14:00:00","list":[{"endTime":"","partId":"DBPB0428B2UP055-1","planOutput":"","prod":"","prodTarget":16.65999984741211,"production":"30","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":27},{"endTime":"","partId":"DBTA0428B2SP211-1","planOutput":"","prod":"","prodTarget":20.459999084472656,"production":"237","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":22},{"endTime":"","partId":"DV05028B12UP008","planOutput":"","prod":"","prodTarget":14.899999618530273,"production":"51","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":23}],"startTime":"2021-02-03 13:00:00"},{"endTime":"2021-02-03 15:00:00","list":[{"endTime":"","partId":"DBPB0428B2UP055-1","planOutput":"","prod":"","prodTarget":16.65999984741211,"production":"381","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":27},{"endTime":"","partId":"DV05028B12UP008","planOutput":"","prod":"","prodTarget":14.899999618530273,"production":"520","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":23}],"startTime":"2021-02-03 14:00:00"},{"endTime":"2021-02-03 16:00:00","list":[{"endTime":"","partId":"DBPB0428B2UP055-1","planOutput":"","prod":"","prodTarget":16.65999984741211,"production":"308","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":27},{"endTime":"","partId":"DBTA0328B2LP030-1","planOutput":"","prod":"","prodTarget":22.219999313354492,"production":"1","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":18},{"endTime":"","partId":"DBTA0428B2SP211-1","planOutput":"","prod":"","prodTarget":20.459999084472656,"production":"6","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":22},{"endTime":"","partId":"DV05028B12UP008","planOutput":"","prod":"","prodTarget":14.899999618530273,"production":"96","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":23}],"startTime":"2021-02-03 15:00:00"},{"endTime":"2021-02-03 17:00:00","list":[{"endTime":"","partId":"DBPB0428B2UP055-1","planOutput":"","prod":"","prodTarget":16.65999984741211,"production":"11","rate":"-","ratesType":"","scrap":"-","startTime":"","testStation":"","usePeopNum":27}],"startTime":"2021-02-03 16:00:00"}],"type":"1","vestingDate":"2021-02-03"}};

    // 獲取坐標軸數據
    function getSeriesDate(json) {
        var resultList = [];
        var times = json.entity.times;
        times.sort(function(a, b){
            return a.startTime > b.startTime ? 1 : -1;
        });
        legends = json.legends;
        //legends = ["料號A", "料號B", "料號C"];
        for(let i = 0; i < legends.length; ++i) {
            var customer = {
            name: legends[i],
            type: 'custom',
            renderItem: function (params, api) {
            //这里使用 api.value(0) 取出当前 dataItem 中第一个维度的数值。
                    var categoryIndex = api.value(0);
            // 这里使用 api.coord(...) 将数值在当前坐标系中转换成为屏幕上的点的像素值。
                    var startPoint = api.coord([api.value(1), categoryIndex]); //柱狀左上角坐標
                    var endPoint = api.coord([api.value(2), categoryIndex]); // 柱狀右上角坐標
                    var height = api.size([0, 1])[1];// 
                
                    return {
            type: 'group',
            children: [
                            {
            type: 'rect',// 表示这个图形元素是矩形。还可以是 'circle', 'sector', 'polygon' 等等。
            shape: echarts.graphic.clipRectByRect({ 
            // 矩形的位置和大小。
            x: startPoint[0] + 8 + api.value(4) * ((endPoint[0] - startPoint[0]) / api.value(3)),
            //x: startPoint[0] + 8,
            y: startPoint[1] - height / 2,
            width: (endPoint[0] - startPoint[0]) / api.value(3) - 16,
            height: params.coordSys.height
        }, {
            // 当前坐标系的包围盒。
            x: params.coordSys.x,
            y: params.coordSys.y,
            width: params.coordSys.width,
            height: params.coordSys.height
        }),
            style: api.style()
        },
                            {
            type: 'text',
            style: {
            text: (categoryIndex / api.value(5) * api.value(3) * 100).toFixed(1) + '%',
            textAlign: 'center',
            // fill: '#FFFFFF',
            x: (2 * (startPoint[0] + api.value(4) * ((endPoint[0] - startPoint[0]) / api.value(3))) + (endPoint[0] - startPoint[0]) / api.value(3)) / 2,
            y: startPoint[1] - 15
        }
        }
        ]
        };
        },
            encode: {
            x: [1, 2], // data 中『维度1』和『维度2』对应到 X 轴
            y: 0// data 中『维度0』对应到 Y 轴
        },
    data: []
    }
    resultList.push(customer)
    }
    
    for(let j = 0; j < resultList.length; ++j) {
        for(let i = 0; i < times.length; ++i) {
        	for(let k = 0; k < times[i].list.length; ++k) {
        		if(resultList[j].name == times[i].list[k].partId) {
            		resultList[j].data.push({value: [
	  	         	    times[i].list[k].production,
	  	 				new Date(times[i].startTime), 
	  	 				new Date(times[i].endTime),
	  	 				times[i].list.length,
	  	 				k,
	  	 				times[i].list[k].usePeopNum * times[i].list[k].prodTarget.toFixed(2),
	  	 				times[i].list[k].usePeopNum
    ]})
    }
    }
    }
    }
    
    resultList.push({
        name: '標準產能',
        type: 'line',
        step: 'start',
        lineStyle: {
        color: '#0F5E03',
        width: 1.5
    },
        itemStyle: {
        color: '#0F5E03'
    },
        showSymbol: false,
        markArea: {
        data: (function() {
            	let res = [];
            	let restTimes = json.entity.rests
            	for(let time of restTimes) {
            		res.push([{name:'休息時間', xAxis: time.startTime}, {xAxis: time.endTime}])
    }
                return res;
    })()
    },
    data: (function (){
        let res = [];
        res.push({value: [new Date(times[0].startTime).getTime(), times[0].list[0].usePeopNum * times[0].list[0].prodTarget]})
        for(let t of times) {
            if(t.list.length > 1) {
            for(let i = 0; i < t.list.length; i++) {
                res.push({value: [new Date(t.startTime).getTime() + 3600000/t.list.length * (i + 1) , t.list[i].usePeopNum * t.list[i].prodTarget / t.list.length]})
            }
        } else {
                res.push({value: [new Date(t.endTime).getTime(), t.list[0].usePeopNum * t.list[0].prodTarget]})
    }
    }
            return res;
    })()
    })
    legends.push({
        name: '標準產能',
        icon: 'image://themes/images/line.png'
    })

    return resultList;
    }

    // 基于准备好的dom，初始化echarts实例
    var loadChart = echarts.init(document.getElementById('chart1'), 'light');

    // 定義模板
    loadoption = {
        title: {
            //text: '產能負荷',
            // textStyle: {
            //     color: '#FFFFFF'
            // }
        },
        legend: {
            // textStyle: {
            //     color: '#FFFFFF'
            // }
        },
        tooltip: {
            trigger: 'item',
            formatter: function(params) {
                if(params.componentType == "markArea") {
                    return null;
                }
                return params.value[1].Format("MM-dd hh:mm") + '<br />' + 
                '<div style="font-size:23px;float:left;color:' + params.color + '">●</div>' + '&thinsp;' +
                params.seriesName + '<br />' + 
                '實際產量：' + params.value[0] + '<br />' + 
                '標準產能：' + (params.value[5].toFixed(2) / params.value[3]) + '<br />' + 
                '工作人數：' + params.value[6]
            }
        },
        //toolbox: {
        //    show: true,
        //    feature: {
        //        restore: {},
        //        saveAsImage: {}
        //    }
        //},
        xAxis: [{
            type: 'time',
            name: '时间',
            interval: 3600000,
            axisLabel: {
                formatter: function (value, index) {
                    var date = new Date(value);
                    return date.Format("hh:mm");
                },
                //color: '#FFFFFF'
            },
            // axisTick:{
            //     lineStyle: {
            //         color: '#FFFFFF'
            //     }
            // },
            // axisLine: {
            //     lineStyle: {
            //         color: '#FFFFFF'
            //     }
            // },
            // nameTextStyle: {
            //     color: '#FFFFFF'
            // },
        }],
        yAxis: [{
            type: 'value',
            name: '產量',
            nameTextStyle: {
                padding: [0, 0, 0, -80]    // 四个数字分别为上右下左与原位置距离
            },
            scale: true,
            min: 0,
            // axisLabel: {
            //     color: '#FFFFFF'
            // },
            // axisTick:{
            //     lineStyle: {
            //         color: '#FFFFFF'
            //     }
            // },
            // axisLine: {
            //     lineStyle: {
            //         color: '#FFFFFF'
            //     }
            // },
            // nameTextStyle: {
            //     color: '#FFFFFF'
            // },
            boundaryGap: [0.2, 0.2]
        }]
    };

    loadChart.setOption(loadoption);
    loadChart.getDom().style.height = (document.body.clientHeight-90)+"px";
    loadChart.resize();
    //window.onresize = loadChart.resize;
    //window.addEventListener('resize', function () {
    //    loadChart.resize()
    //});
    var seriesData = getSeriesDate(data);// 獲取坐標軸數據

    // 載入數據
    loadChart.setOption({
        legend: {
            data: legends
        },
        xAxis: {
            min: data.entity.queryStartDate,
            max: data.entity.queryEndDate
        },
        yAxis: {
        },
        series: seriesData
    });
</script>
