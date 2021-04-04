<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DefaultHome.aspx.cs" Inherits="Web.DefaultHome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%; width: auto;">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>默认首页</title>
    <link href="css/base.css" rel="stylesheet" />
    <%--<link href="css/platform.css?v=7" rel="stylesheet" />--%>
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <%--<link href="../../easyUI15/themes/default/easyui.css?v=1" rel="stylesheet" type="text/css" />--%>
    <link href="easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="JSPage/echarts.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //綁定datagrid
            $('#tbWait').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: 'ASHX/DMC/RepairForm.ashx?M=Search&SearchType=ByAdvanced&FormStatus=1',
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
                loadMsg: '数据加载中...',//數據加載中
                //是否顯示分页
                pagination: true,
                //可動列
                columns: [[
                           { field: 'repairformno', title: '维修单号', width: 50, align: 'left' },
                           { field: 'applyuserid', title: '申请人', width: 50, align: 'left' },
                           { field: 'deviceid', title: '设备编号', width: 50, align: 'left' },
                           { field: 'positiontext', title: '故障位置', width: 50, align: 'left' },
                           { field: 'phenomenatext', title: '故障现象', width: 50, align: 'left' },
                           { field: 'faulttime', title: '故障时间', width: 120, align: 'left' }
                ]]
            });
            $(window).resize(function () {
                $('#tbWait').datagrid('resize');
            });
            //設置分页控件屬性 
            var p = $('#tbWait').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: [10, 15, 20, 25, 30],
                beforePageText: '第',
                afterPageText: '页 共{pages}页',
                displayMsg: '当前显示 {from} - {to} 条记录 共 {total} 条记录'
            });

            //綁定datagrid
            $('#tbDo').datagrid({
                //是否折行
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: 'ASHX/DMC/RepairRecord.ashx?M=Search&SearchType=ByAdvanced&RepairStatus=1',
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
                loadMsg: '数据加载中...',//數據加載中
                //是否顯示分页
                pagination: true,
                //可動列
                columns: [[
                           { field: 'repairformno', title: '维修单号', width: 50, align: 'left' },
                           { field: 'applyuserid', title: '申请人', width: 50, align: 'left' },
                           { field: 'deviceid', title: '设备编号', width: 50, align: 'left' },
                           { field: 'positiontext', title: '故障位置', width: 50, align: 'left' },
                           { field: 'phenomenatext', title: '故障现象', width: 50, align: 'left' },
                           { field: 'faulttime', title: '故障时间', width: 120, align: 'left' }
                ]]
            });
            $(window).resize(function () {
                $('#tbDo').datagrid('resize');
            });
            //設置分页控件屬性 
            var p1 = $('#tbDo').datagrid('getPager');
            $(p1).pagination({
                pageSize: 10,
                pageList: [10, 15, 20, 25, 30],
                beforePageText: '第',
                afterPageText: '页 共{pages}页',
                displayMsg: '当前显示 {from} - {to} 条记录 共 {total} 条记录'
            });

            $(".easyui-tree").tree('expandAll'); //collapseAll

            $("#divwait").parent().css("float", "left");
            $("#divwait").parent().css("margin-top", "2px");

            $("#divdo").parent().css("float", "right");
            $("#divdo").parent().css("margin-top", "2px");
            //$("#divdo").parent().css("margin-right", "2px");
            //$("#divdo").parent().css("margin-bottom", "5px");
            //$("#divdo").parent().css("margin-top", "5px");
            //$("#divdo").parent().css("float", "left");
            $("#divdoing").parent().css("float", "left");
            $("#divdoing").parent().css("margin-bottom", "2px");
            $("#divdoing").parent().css("margin-top", "2px");
        });
    </script>
</head>
<body style="background-color: white; height: 100%;">
    <div style="height: 100%; width: 100%;">
        <div class="easyui-panel" title="待指派" data-options="collapsible:true,selected:true" style="width: 50%; height: 50%; padding: 2px 2px 2px 2px;" id="divwait">
            <table id="tbWait" data-options="fit:true">
            </table>
        </div>
        <div class="easyui-panel" title="处理中" data-options="collapsible:true,selected:false" style="width: 50%; height: 50%; padding: 2px 2px 2px 2px;" id="divdo">
            <table id="tbDo" data-options="fit:true">
            </table>
        </div>
        <div class="easyui-panel" style="width: 100%; height: 50%; border: 1px solid #ccc; overflow-y: scroll; padding: 2px 2px 2px 2px;" id="divdoing">
            <% int i = 0;
               foreach (System.Data.DataRow dr in RepairmanList.Rows)
               { %>
            <div style="float: left; height: 142px; width: 140px; padding: 1px 1px 1px 1px;">
                <div style="background-color: #F6F6F6;">
                    <div style="width: 100%; height: 100px;">
                        <div style="width: 61px; height: 100px; float: left;">
                            <img id="headman<%= i %>" style="width: 61px; height: 100px; border-radius: 10px;" src="login_files/workman.jpg" alt="头像" />
                        </div>
                        <div style="width: 77px; height: 100px; float: left;" class="chart-chart" id="chart<%= i %>"></div>
                    </div>
                    <div style="width: 100%; height: 40px;">
                        <p id="pworking<%= i %>" style="font-size: 18px; color: red; text-align: center; font-family: arial;">忙碌 02:30:00</p>
                        <p id="preset<%= i %>" style="font-size: 18px; color: green; text-align: center; font-family: arial; margin-top: 2px;">空闲 05:30:00</p>
                    </div>
                </div>
            </div>
            <% i = i + 1;
               } %>
        </div>
    </div>
</body>
</html>


<script type="text/javascript">
    //id:'chart0'
    function left(mainStr, lngLen) {
        if (lngLen > 0) {
            return mainStr.substring(mainStr.length - lngLen)
        }
        else { return null }
    }
    function loadChart(id, row) {
        //设置工作时间和空闲时间 
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
                text: row.repairmanid,
                left: '10px',//标题的位置 默认是left，其余还有center、right属性
                textStyle: {
                    color: "#436EEE",
                    fontSize: 12,
                }
            },
            //tooltip: { //提示框组件
            //    trigger: 'item', //触发类型(饼状图片就是用这个)
            //    formatter: "{a} <br/>{b} : {c} ({d}%)" //提示框浮层内容格式器
            //},
            color: ['green', 'red', 'blue'],  //手动设置每个图例的颜色
            //legend: {  //图例组件
            //    //right:100,  //图例组件离右边的距离
            //    orient: 'horizontal',  //布局  纵向布局 图例标记居文字的左边 vertical则反之
            //    width: 40,      //图行例组件的宽度,默认自适应
            //    x: 'right',   //图例显示在右边
            //    y: 'center',   //图例在垂直方向上面显示居中
            //    itemWidth: 10,  //图例标记的图形宽度
            //    itemHeight: 10, //图例标记的图形高度
            //    data: ['空闲', '忙碌'],
            //    textStyle: {    //图例文字的样式
            //        color: '#333',  //文字颜色
            //        fontSize: 12    //文字大小
            //    }
            //},
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
        myChart.setOption(option);
    }
    function Refersh() {
        $.post("ASHX/DMC/Repairman.ashx?M=GetRepairmWorking",
            {},
            function (data) {
                for (var i = 0; i < data.length; i++) {
                    loadChart(i, data[i]);
                }
            },
            'json');
    }

    $(function () {//间隔60s自动加载一次   
        Refersh(); //首次立即加载   
        window.setInterval(Refersh, 1 * 60 * 1000); //循环执行！！   
    });

    //for (var i = 0; i <= 7; i++) {
    //    loadChart(i);
    //}
</script>
