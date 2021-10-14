<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Kanban.aspx.cs" Inherits="Web.UI.Equment.Kanban" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>电子看板</title>
    <%--<link href="../../css/base.css" rel="stylesheet" />
    <link href="../../css/platform.css?v=7" rel="stylesheet" />--%>
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="../../easyUI15/locale/easyui-lang-zh_TW.js" type="text/javascript"></script>
    <script src="../../JSPage/echarts.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //綁定datagrid
            $('#tbEqManage').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: '../../ASHX/DMC/RepairRecord.ashx?M=KanBan',
                //數據在一行顯示 
                nowrap: false,
                fitColumns: true,
                //行條紋化
                striped: true,
                //固定序號

                rownumbers: true,
                pageSize: 10,
                //是否可以多選
                singleSelect: true,
                remoteSort: false,
                idField: 'autoid',
                loadMsg: '數據加載中...',//數據加載中
                //是否顯示分页
                pagination: true,
                //可動列
                columns: [[
                    {
                        field: 'repairformno', title: '维修单号', width: 70, align: 'left', sortable: true,
                        formatter: function (value, row, index) {
                            return "<span style='font-weight:bold;font-size:18px;'>" + value.substring(3, value.length) + "</span>";
                        }

                    },
                    {
                        field: 'repairstatus', title: '状态', width: 100, align: 'left', sortable: true,

                        formatter: function (value, row, index) {
                            var text = "N/A";
                            switch (row.formstatus) {
                                case "10":
                                    text = "待指派";
                                    break;
                                case "0":
                                    text = "待指派";
                                    break;
                                case "12":
                                    text = "待指派(挂单)";
                                    break;
                                case "24":
                                    text = "待维修(IPQC返修)";
                                    break;
                                case "25":
                                    text = "待维修(组长返修)";
                                    break;
                                case "20":
                                    text = "维修中";
                                    break;
                                case "23":
                                    text = "待维修(返修)";
                                    break;
                                case "30":
                                    text = "待生产员确认";
                                    break;
                                case "40":
                                    text = "待IPQC确认";
                                    break;
                                case "50":
                                    text = "待组长确认";
                                    break;
                                case "61":
                                    text = "生产员返修";
                                    break;
                                case "62":
                                    text = "挂单完结";
                                    break;
                                case "64":
                                    text = "生产员返修";
                                    break;
                                case "63":
                                    text = "维修完成";
                                    break;
                                case "60":
                                    text = "维修完成";
                                    break;
                                case "65":
                                    text = "生产组长返修";
                                    break;

                                default:
                                    text = row.formstatus + "维修完成";
                                    break;
                            }
                            return "<span style='font-weight:bold;font-size:18px;'>" + text + "</span>";

                        }
                    },
                    {
                        field: 'deviceid', title: '设备信息', width: 40, align: 'left', sortable: true,
                        formatter: function (value, row, index) {
                            return "<span style='font-weight:bold;font-size:18px;'>" + value + "</span>";
                        }
                    },
                    //{ field: 'applyuserid', title: '申请生产员', width: 50, align: 'left', sortable: true },
                    {
                        field: 'bfaulttime', title: '故障时间', width: 100, align: 'left', sortable: true,
                        formatter: function (value, row, index) {
                            return "<span style='font-weight:bold;font-size:18px;'>" + value + "</span>";
                        }
                    },
                    {
                        field: 'repairetime', title: '完成时间', width: 100, align: 'left', sortable: "true",
                        formatter: function (value, row, index) {
                            return "<span style='font-weight:bold;font-size:18px;'>" + value + "</span>";
                        }
                    },
                    {
                        field: 'positiontext', title: '故障位置', width: 40, align: 'left', sortable: true,
                        formatter: function (value, row, index) {
                            return "<span style='font-weight:bold;font-size:18px;'>" + value + "</span>";
                        }
                    },
                  //  { field: 'phenomenatext', title: '故障现象', width: 70, align: 'left', sortable: true },
                    {
                        field: 'repairmanname', title: '维修员', width: 40, align: 'left', sortable: true,
                        formatter: function (value, row, index) {
                            return "<span style='font-weight:bold;font-size:18px;'>" + value + "</span>";
                        }
                    },
                    {
                        field: 'mouldid', title: '模具编号1', width: 70, align: 'left',
                        formatter: function (value, row, index) {
                            return "<span style='font-weight:bold;font-size:18px;'>" + value + "</span>";
                        }
                    },
                           {
                               field: 'mouldid1', title: '模具编号2', width: 70, align: 'left',
                               formatter: function (value, row, index) {
                                   return "<span style='font-weight:bold;font-size:18px;'>" + value + "</span>";
                               }
                           },
                           {
                               field: 'newmouldid', title: '新模编号1', width: 70, align: 'left',
                               formatter: function (value, row, index) {
                                   return "<span style='font-weight:bold;font-size:18px;'>" + value + "</span>";
                               },
                               styler: function (value, row, index) {
                                   if (row.newmouldid != '') {
                                       return 'background-color:#4cae4c;color: #fff;border: 0px'
                                   }

                               }
                           },
                           {
                               field: 'newmouldid1', title: '新模编号2', width: 70, align: 'left',
                               formatter: function (value, row, index) {
                                   return "<span style='font-weight:bold;font-size:18px;'>" + value + "</span>";
                               },
                               styler: function (value, row, index) {
                                   if (row.newmouldid1 != '') {
                                       return 'background-color:#4cae4c;color: #fff;border: 0px'
                                   }

                               }
                           },
                  //  { field: 'positiontext1', title: '故障位置1', width: 70, align: 'left' },
                   // { field: 'phenomenatext1', title: '故障现象1', width: 70, align: 'left' },
                    {
                        field: 'manhoure', title: '停机时长<br />(分钟)', width: 40, align: 'left',
                        formatter: function (value, row, index) {
                            return "<span style='font-weight:bold;font-size:18px;'>" + value + "</span>";
                        }
                    },
                    {
                        field: 'gradetime', title: '标准工时<br />(分钟)', width: 40, align: 'left',
                        formatter: function (value, row, index) {
                            return "<span style='font-weight:bold;font-size:18px;'>" + value + "</span>";
                        }
                    },
                   // { field: 'rowcolor', title: '颜色', width: 70, align: 'left', hidden: true }

                ]],
                rowStyler: function (index, row) {
                    console.log(parseInt(row.manhoure), (parseInt(row.gradetime) + 60), row.formstatus);
                    if (parseInt(row.manhoure) > (parseInt(row.gradetime) + 60) && (row.formstatus != "30" && row.formstatus != "40" && row.formstatus != "50")) {
                        return 'background-color:red;color:blue;font-weight:bold;';
                    } else {
                        if (row.formstatus == "10" && parseInt(row.rowcolor) > 20) {
                            return 'background-color:#e1c311;color:blue;font-weight:bold;';
                        } else if (row.formstatus == "40" && parseInt(row.rowcolor) > 20) {
                            return 'background-color:#db861e;color:blue;font-weight:bold;';
                        }
                        else if (row.formstatus == "30" && parseInt(row.rowcolor) > 20) {
                            return 'background-color:LightGreen;color:blue;font-weight:bold;';
                        }
                        else if (row.formstatus == "50" && parseInt(row.rowcolor) > 20) {
                            return 'background-color:LightGreen;color:blue;font-weight:bold;';
                        }
                        else { return "" }
                    }

                },
                onLoadSuccess: function (data) {
                     $($("td")[0]).css({ "background": "#4cae4c", "color": "#fff", "font-weight": "Blob" });

                }
            });

            //設置分页控件屬性 
            var p1 = $('#tbEqManage').datagrid('getPager');
            $(p1).pagination({
                pageSize: 10,
                pageList: [10, 15, 20, 25, 30],
                beforePageText: '第',
                afterPageText: '页 共{pages}页',
                displayMsg: '当前显示 {from} - {to} 条记录 共 {total} 条记录'
            });
            $(".datagrid-header-row td div span").each(function (i, th) {
                var val = $(th).text();
                $(th).html("<label style='font-weight:bolder;font-size:15px;'>" + val + "</label>");
            });
        });

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
        /*::-webkit-scrollbar { width:4px;}
        ::-webkit-scrollbar-thumb{ display:block; width:4px; margin:0 auto;  background:#ccc;}*/
    </style>
</head>
<body style="width: 100%; height: 100%;">
    <div id="repair" style="padding: 2px 2px 2px 2px; margin-left: 5px; margin-right: 0px; height: calc(100% - 50px); width: calc(100% - 10px); float: left;">
        <table id="tbEqManage" data-options="fit:true">
        </table>
    </div>
    <div id="divOperation" style="width: 100%; height: 50px; padding: 2px 2px 2px 2px; margin-left: 5px; margin-top: 2px; float: left;">
        <div style="width: 100%; height: 100%; padding: 2px 2px 2px 2px; float: left;">
            <div id="dWaitQty" style="width: calc(20% - 2px); height: 45px; line-height: 45px; float: left; margin-left: 2px; background-color: cornflowerblue">
                <a style="margin-left: 5px; color: white; font-size: 18px; font-weight: bold; display: inline-block; text-align: center;" data-options="toggle:true,group:'g1',selected:true">待指派：(&nbsp;&nbsp;&nbsp;<span style="font-size: 25px;" id="WaitQty">0</span>&nbsp;&nbsp;&nbsp;)</a>
            </div>
            <div id="dWorkQty" style="width: calc(20% - 2px); height: 45px; line-height: 45px; float: left; margin-left: 2px; background-color: cornflowerblue">
                <a style="margin-left: 5px; color: white; font-size: 18px; font-weight: bold; flex-item-align: center;" data-options="toggle:true,group:'g1',selected:true">维修中：(&nbsp;&nbsp;&nbsp;<span style="font-size: 25px;" id="WorkQty">0</span>&nbsp;&nbsp;&nbsp;)</a>
            </div>
            <div id="dCHAOSHIQty" style="width: calc(20% - 2px); height: 45px; line-height: 45px; float: left; margin-left: 2px; background-color: cornflowerblue">
                <a style="margin-left: 5px; color: white; font-size: 18px; font-weight: bold; flex-item-align: center;" data-options="toggle:true,group:'g1',selected:true">维修超时：(&nbsp;&nbsp;&nbsp;<span style="font-size: 25px;" id="CHAOSHIQty">0</span>&nbsp;&nbsp;&nbsp;)</a>
            </div>
            <div id="dQCQty" style="width: calc(20% - 2px); height: 45px; line-height: 45px; float: left; margin-left: 2px; background-color: cornflowerblue">
                <a style="margin-left: 5px; color: white; font-size: 18px; font-weight: bold; flex-item-align: center;" data-options="toggle:true,group:'g1',selected:true">待QC确认：(&nbsp;&nbsp;&nbsp;<span style="font-size: 25px;" id="QCQty">0</span>&nbsp;&nbsp;&nbsp;)</a>
            </div>
            <div id="dSCQty" style="width: calc(20% - 2px); height: 45px; line-height: 45px; float: left; margin-left: 2px; background-color: cornflowerblue">
                <a style="margin-left: 5px; color: white; font-size: 18px; font-weight: bold; flex-item-align: center;" data-options="toggle:true,group:'g1',selected:true">待生产确认：(&nbsp;&nbsp;&nbsp;<span style="font-size: 25px;" id="SCQty">0</span>&nbsp;&nbsp;&nbsp;)</a>
            </div>
        </div>
        <%-- <div id="user" style="width: calc(100% - 400px); height: 100%; float: right; padding: 2px 2px 2px 2px;">
            <% int i = 0;
               foreach (System.Data.DataRow dr in RepairmanList.Rows)
               { %>
            <div style="float: left; height: 120px; width: 100px; margin-left: <%= (i==0?0:2) %>px; margin-top: 2px; background-color: #F6F6F6;">
                <div style="width: 100%; height: 80px;">
                    <div style="width: 43%; height: 80px; float: left;">
                        <img style="width: 100%; height: 80px;" src="../../login_files/workman.jpg" alt="头像" />
                    </div>
                    <div style="width: 57%; height: 80px; float: left;" class="chart-chart" id="chart<%= i %>"></div>
                </div>
                <div style="width: 100%; height: 30px;">
                    <div id="pworking<%= i %>" style="font-size: 16px; color: red; text-align: center; font-family: arial;">忙碌 02:30:00</div>
                    <div id="preset<%= i %>" style="font-size: 16px; color: green; text-align: center; font-family: arial;">空闲 05:30:00</div>
                </div>
            </div>
            <% i = i + 1;
               } %>
        </div>--%>
    </div>
    <%-- <div id="user" style="width: calc(100% - 200px); height: 200px; padding: 2px 2px 2px 2px; margin-top: 2px; float: right; overflow-y: scroll;">
        <% int i = 0;
            foreach (System.Data.DataRow dr in RepairmanList.Rows)
            { %>
        <div style="float: left; height: 120px; width: 7.5%; margin-left: 2px; margin-top: 2px; background-color: #F6F6F6;">
            <div style="width: 100%; height: 80px;">
                <div style="width: 43%; height: 80px; float: left;">
                    <img style="width: 100%; height: 80px;" src="../../login_files/workman.jpg" alt="头像" />
                </div>
                <div style="width: 57%; height: 80px; float: left;" class="chart-chart" id="chart<%= i %>"></div>
            </div>
            <div style="width: 100%; height: 30px;">
                <div id="pworking<%= i %>" style="font-size: 16px; color: red; text-align: center; font-family: arial;">忙碌 02:30:00</div>
                <div id="preset<%= i %>" style="font-size: 16px; color: green; text-align: center; font-family: arial;">空闲 05:30:00</div>
            </div>
        </div>
        <% i = i + 1;
            } %>
    </div>--%>
</body>
</html>

<script type="text/javascript">
    function left(mainStr, lngLen) {
        if (lngLen > 0) {
            return mainStr.substring(mainStr.length - lngLen)
        }
        else { return null }
    }
    //id:'chart0'
    function loadChart(id, row) {
        var wh = parseInt(row.workingtime / 60);
        var wm = row.workingtime % 60;
        var rh = parseInt(row.resttime / 60);
        var rm = row.resttime % 60;
        $("#pworking" + id).html("忙碌 " + left("000" + wh, 2) + ":" + left("000" + wm, 2) + ":00");
        $("#preset" + id).html("空闲 " + left("000" + rh, 2) + ":" + left("000" + rm, 2) + ":00");
        var myChart = echarts.init(document.getElementById("chart" + id));
        // 指定图表的配置项和数据
        var option = {
            title: {//标题组件
                text: row.repairmanname,
                left: '2px',//标题的位置 默认是left，其余还有center、right属性
                textStyle: {
                    color: "#436EEE",
                    fontSize: 12,
                }
            },
            color: ['green', 'red', 'blue'],  //手动设置每个图例的颜色
            series: [ //系列列表
                {
                    name: '比例',  //系列名称
                    type: 'pie',   //类型 pie表示饼图
                    center: ['50%', '60%'], //设置饼的原心坐标 不设置就会默认在中心的位置
                    radius: ['50%', '70%'],  //饼图的半径,第一项是内半径,第二项是外半径,内半径为0就是真的饼,不是环形
                    itemStyle: {  //图形样式
                        normal: { //normal 是图形在默认状态下的样式；emphasis 是图形在高亮状态下的样式，比如在鼠标悬浮或者图例联动高亮时。
                            label: {  //饼图图形上的文本标签
                                position: 'inner',
                                show: true  //平常不显示
                            },
                            labelLine: {     //标签的视觉引导线样式
                                show: false  //平常不显示
                            }
                        },
                        emphasis: {   //normal 是图形在默认状态下的样式；emphasis 是图形在高亮状态下的样式，比如在鼠标悬浮或者图例联动高亮时。
                            label: {  //饼图图形上的文本标签
                                show: true,
                                position: 'center',
                                textStyle: {
                                    fontSize: '10',
                                    fontWeight: 'bold'
                                }
                            }
                        }
                    },
                    data: [
                        { value: row.resttime, name: '空闲' },
                        { value: row.workingtime, name: '忙碌' },
                        { value: row.surplustime, name: '剩余' }
                    ]
                }
            ]
        };
        myChart.setOption(option, true);
    }

    function InitRefersh() {
        //人员状态刷新
        $.post("../../ASHX/DMC/Repairman.ashx?M=GetRepairmWorking",
            {},
            function (data) {
                //for (var i = 0; i < data.ReportData.length; i++) {
                //    loadChart(i, data.ReportData[i]);
                //}
                RefershQTY(data.RefershQTY);
            },
            'json');

    }

    function RefershQTY(data) {
        $("#WaitQty").html(data.waitqty);
        $("#WorkQty").html(data.workqty);
        $("#CHAOSHIQty").html(data.chaoshiqty);
        $("#QCQty").html(data.qcqty);
        $("#SCQty").html(data.scqty);
    }

    //根据参数的进行翻页(负数为下一页，)
    function pageTurring(e) {
        flag = false;
        var grid = $('#tbEqManage');
        var options = grid.datagrid('getPager').data("pagination").options;
        var pageNum = options.pageNumber;//当前页数  
        var total = options.total;
        var max = Math.ceil(total / options.pageSize);

        if (e < 0) {
            $('#tbEqManage').datagrid('gotoPage', {
                page: pageNum + 1 > max ? 1 : pageNum + 1,
                callback: function (page) {
                    // console.log(page); 
                }
            });
        }
    }

    function Refersh() {
        //分页显示
        pageTurring(-1);

        //人员状态刷新
        $.post("../../ASHX/DMC/Repairman.ashx?M=GetRepairmWorking",
            {},
            function (data) {
                //for (var i = 0; i < data.ReportData.length; i++) {
                //    RefreshChartData(i, data.ReportData[i]);
                //}
                RefershQTY(data.RefershQTY);
            },
            'json');
    }

    function RefreshChartData(id, row) {
        var wh = parseInt(row.workingtime / 60);
        var wm = row.workingtime % 60;
        var rh = parseInt(row.resttime / 60);
        var rm = row.resttime % 60;
        $("#pworking" + id).html("忙碌 " + left("000" + wh, 2) + ":" + left("000" + wm, 2) + ":00");
        $("#preset" + id).html("空闲 " + left("000" + rh, 2) + ":" + left("000" + rm, 2) + ":00");
        var myChart = echarts.init(document.getElementById("chart" + id));
        var data = [
            { value: row.resttime, name: '空闲' },
            { value: row.workingtime, name: '忙碌' },
            { value: row.surplustime, name: '剩余' }
        ];
        //刷新数据
        var option = myChart.getOption();
        option.series[0].data = data;
        myChart.setOption(option);
    }

    var lastTimeId = "";
    $(function () {//间隔60s自动加载一次   
        InitRefersh(); //首次立即加载   
        if (!!lastTimeId) {
            window.clearInterval(lastTimeId);
        }
        lastTimeId = window.setInterval(Refersh, 180 * 1000); //循环执行！！   
    });
</script>
