<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TingjiFenxi.aspx.cs" Inherits="Web.UI.Equment.TingjiFenxi" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>停机分析</title>
    <link href="../../css/base.css" rel="stylesheet" />
    <link href="../../css/platform.css?v=7" rel="stylesheet" />
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="../../easyUI15/locale/easyui-lang-zh_TW.js" type="text/javascript"></script>
    <script src="../../JSPage/echarts.min.js"></script>
    <script type="text/javascript">
        var lastTimeId = "";
        $(document).ready(function () {
            $("#qyearmonth").datebox("setValue", "<%= DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd")%>");
            $("#eqyearmonth").datebox("setValue", "<%= DateTime.Now.ToString("yyyy-MM-dd")%>");
            QueryData();
            if (!!lastTimeId) {
                window.clearInterval(lastTimeId);
            }
            lastTimeId = window.setInterval(QueryData, 1 * 3600 * 1000); //循环执行！！   
        });
        function setGridColumns(Columns) {
            $('#tbEqManage').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                //url: '../../ASHX/DMC/RepairRecord.ashx?M=TingjiFenxi',
                //數據在一行顯示 
                nowrap: false,
                //fitColumns: true,
                //行條紋化
                striped: true,
                //固定序號
                rownumbers: false,
                fit: true,
                //是否可以多選
                singleSelect: true,
                remoteSort: false,
                idField: 'autoid',
                loadMsg: '數據加載中...',//數據加載中
                //是否顯示分页
                pagination: false,
                //可動列
                columns: [Columns],
                rowStyler: function (index, row) {
                    var modNum = index % 4;
                    //switch (modNum)
                    //{
                    //    case 0:
                    //        return 'background-color:blue;color:white;font-weight:bolder;font-size:18px;';
                    //        break;
                    //    case 1:
                    //        return 'background-color:red;color:white;font-weight:bolder;font-size:18px;';
                    //        break;
                    //    case 2:
                    //        return 'background-color:green;color:white;font-weight:bolder;font-size:18px;';
                    //        break;
                    //    case 3:
                    //        return 'background-color:black;color:white;font-weight:bolder;font-size:18px;';
                    //        break;
                    //}

                    switch (modNum) {
                        case 0:
                            return 'background-color:#fffac09e;color:blue;font-weight:bolder;font-size:18px;';
                            break;
                        case 1:
                            return 'background-color:#fffac09e;color:red;font-weight:bolder;font-size:18px;';
                            break;
                        case 2:
                            return 'background-color:#fffac09e;color:green;font-weight:bolder;font-size:18px;';
                            break;
                        case 3:
                            return 'background-color:#fffac09e;color:black;font-weight:bolder;font-size:18px;';
                            break;
                    }
                },
                onLoadSuccess: function (data) {
                    setGridHead();
                    loadChart1("chart1");
                    var merges = [{
                        index: 0,
                        rowspan: 4
                    },
                    {
                        index: 4,
                        rowspan: 4
                    }
                    ,
                    {
                        index: 8,
                        rowspan: 4
                    },

                    {
                        index: 12,
                        rowspan: 4
                    },
                    {
                        index: 16,
                        rowspan: 5
                    }];
                    for (var i = 0; i < merges.length; i++)
                        $('#tbEqManage').datagrid('mergeCells', {
                            index: merges[i].index,
                            field: 'type',
                            rowspan: merges[i].rowspan
                        });

                    var merges = [{
                        index: 21,
                        rowspan: 2
                    }, {
                        index: -1,
                        rowspan: 2
                    }];
                    for (var i = 0; i < merges.length; i++)
                        $('#tbEqManage').datagrid('mergeCells', {
                            index: merges[i].index,
                            field: 'type',
                            colspan: merges[i].rowspan
                        });
                }
            }).datagrid("reload");            
        }

        function QueryData() {
            if ($("#qyearmonth").datebox("getValue") != "" && $("#eqyearmonth").datebox("getValue") != "") {
                $.post("../../ASHX/DMC/RepairRecord.ashx?M=TingjiFenxiNew",
                {
                    startDate: $("#qyearmonth").datebox("getValue"),
                    endDate: $("#eqyearmonth").datebox("getValue")
                },
                function (data) {
                    setGridColumns(data.Columns);
                    $('#tbEqManage').datagrid("loadData", data.Rows);
                },
                'json');
            }
        }

        function setGridHead() {
            $(".datagrid-header-row td div span").each(function (i, th) {
                var val = $(th).text();
                $(th).html("<label style='font-weight:bolder;font-size:18px;'>" + val + "</label>");

                //if (i == 0) {
                //    var val = $(th).text();
                //    $(th).html("<label style='font-weight:bolder;font-size:18px;'>" + val + "</label>");
                //} else if (i == 1) {
                //    $(th).remove();
                //}
                //else {
                //    var val = $(th).text();
                //    $(th).html("<label style='font-weight:bolder;font-size:18px;'>" + val + "</label>");
                //}
            });
            $(".datagrid-header-row td").each(function (i, th) {
                if (i == 0) {
                    $(th).css("width", $(th).width() * 2 + 2);
                } else if (i == 1) {
                    $(th).remove();
                }
                $(th).css("background-color", "#fffac09e");
            });
            $(".datagrid-header-row td div").each(function (i, th) {
                if (i == 0) {
                    $(th).css("width", "100%");
                }
            });
            $('.datagrid-cell').css('font-size', '20px');
        }

        function loadChart1(id) {
            var myChart = echarts.init(document.getElementById(id));
            // 指定图表的配置项和数据
            var option;
            var rows = $('#tbEqManage').datagrid('getRows')
            var name = $('#tbEqManage').datagrid('getData').rows[rows.length - 1];

            var yName = [];
            var xName = [];

            var fields = $('#tbEqManage').datagrid('getColumnFields');
            for (var i = 2; i < fields.length; i++)
            {
                var col = $('#tbEqManage').datagrid("getColumnOption", fields[i]);
                xName.push(col.title);
            }           
            var allName = Object.values(name);            
            for (var j = 0; j < allName.length;j++) {
                if (allName[j] != "总维修时间") {
                    yName.push(allName[j]);
                }
            }

            option = {
                backgroundColor: '#fffac09e',
                xAxis: {
                    type: 'category',
                    data: xName
                },
                yAxis: {
                    type: 'value',
                    name: '总时长(h)',

                },
                tooltip: {
                    trigger: 'item',
                    formatter: "{b} : {c}h"
                },
                series: [
                  {
                      data: yName,
                      type: 'line',
                      showBackground: true,
                      backgroundStyle: {
                          color: 'white'
                      }
                  }
                ],
                grid: {
                    top: 30,
                    left: 60,
                    right: 20,
                    bottom: 20,
                    backgroundColor: '#fff'
                }
            };
            myChart.setOption(option, true);
        }

        /**
        * 时间格式化
        * @param value
        * @returns {string}
       */
        var isDay = true;
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
    <style type="text/css">
        /*::-webkit-scrollbar { width:4px;}
        ::-webkit-scrollbar-thumb{ display:block; width:4px; margin:0 auto;  background:#ccc;}*/
    </style>
</head>
<body style="width: 100%; height: 100%;">
    <div style="padding: 1px 1px 1px 1px; height: calc(100% - 2px); width: 100%; background-color: white;" data-options="fit:true">
        <div class="easyui-panel" style="width: 100%; height: 80%; padding: 2px 2px 2px 2px; background-color: #F6F6F6;" id="divd1">
            <table id="tbEqManage" data-options="fit:false" toolbar="#tb" style="width:98%;">
            </table>
        </div>
        <div class="easyui-panel" style="width: 100%; height: 20%; padding: 2px 2px 2px 2px; background-color: #F6F6F6;" id="divd4">
            <div style="width: 100%; height: 100%; float: left;" class="chart-chart" id="chart1"></div>
        </div>
    </div>

    <div id="tb">
        <label style="padding-left: 20px;" for="qyearmonth">报修时间:</label>
        <input data-options="formatter: dateFormatter, parser: dateParser" class="easyui-datebox" name="yearmonth" id="qyearmonth" style="width: 120px;" />
        <label for="eyearmonth">~</label>
        <input data-options="formatter: dateFormatter, parser: dateParser" class="easyui-datebox" name="eyearmonth" id="eqyearmonth" style="width: 120px;" />
        <a class="easyui-linkbutton" data-options="iconCls:'icon-reload',plain:true" onclick="QueryData()">手动刷新</a>
    </div>
</body>
</html>
