<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendLog.aspx.cs" Inherits="Web.UI.Simulator.SendLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>SOP推送记录</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.json-2.4.js"></script>
    <style type="text/css">
        /* 旋转，渐变色 */
        .yellowbox {
            width: 20px;
            height: 20px;
            border-radius: 50%;
            background-image: -webkit-radial-gradient(20px 20px, circle cover, yellow, yellow);
            animation-name: spin;
            animation-duration: 3s; /* 3 seconds */
            animation-iteration-count: infinite;
            animation-timing-function: linear;
        }

        .redbox {
            width: 20px;
            height: 20px;
            border-radius: 50%;
            background-image: -webkit-radial-gradient(20px 20px, circle cover, red, red);
            animation-name: spin;
            animation-duration: 3s; /* 3 seconds */
            animation-iteration-count: infinite;
            animation-timing-function: linear;
        }

        .greenbox {
            width: 20px;
            height: 20px;
            border-radius: 50%;
            background-image: -webkit-radial-gradient(20px 20px, circle cover, green, green);
            animation-name: spin;
            animation-duration: 3s; /* 3 seconds */
            animation-iteration-count: infinite;
            animation-timing-function: linear;
        }

        .greybox {
            width: 20px;
            height: 20px;
            border-radius: 50%;
            background-image: -webkit-radial-gradient(20px 20px, circle cover, grey, grey);
            animation-name: spin;
            animation-duration: 3s; /* 3 seconds */
            animation-iteration-count: infinite;
            animation-timing-function: linear;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            //$("#sline").combobox({
            //    url: '../../ASHX/ESOP/SOPSendLog.ashx?M=getline',
            //    required: true,
            //    missingMessage: '请选择线别',
            //    editable: false,
            //    valueField: 'linetypeno',
            //    //textField: 'displaytext',
            //    panelHeight: '150px',
            //    hasDownArrow: true,
            //    filter: function (value, row) {
            //        return row.linetypeno.toLowerCase().match(value.toLowerCase()) != null;
            //    },
            //    formatter: function (row) {
            //        return row.linetypeno + "-" + row.linetypename
            //    }
            //});

            //$("#spartid").combogrid({
            //    panelWidth: 430,
            //    mode: 'local',
            //    url: '../../ASHX/ESOP/SOPSendLog.ashx?M=GetPartSOP&partId=',
            //    required: true,
            //    idField: 'testitemno',
            //    textField: 'displaytext',
            //    pagination: false,//是否分页
            //    rownumbers: true,//序号 
            //    method: 'post',
            //    fitColumns: true,
            //    columns: [[
            //         { field: 'testitemno', title: '测试项编号', width: 90, align: 'left' },
            //         { field: 'testitemname', title: '测试项名称', width: 70, align: 'left' },
            //         { field: 'ver', title: '版本号', width: 70, align: 'left' },
            //         { field: 'custominfo', title: '客户信息', width: 70, align: 'left' }
            //    ]],
            //    onSelect: function (rowIndex, rowData) {
            //        if (!isView) {
            //            $("#aver").val(rowData.ver);
            //            SetFields(rowData.testitemno, rowData.ver, "", "");
            //        }
            //    }
            //}); 

            //綁定datagrid
            $('#tbSendLog').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: '../../ASHX/ESOP/SendLog.ashx?M=Search',
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
                           {
                               field: 'messagetype', title: '消息类别', width: 50, align: 'left', formatter: function (value, row, index) {
                                   var desc = value;
                                   switch (row.messagetype) {
                                       case "1": desc = "1-SOP";
                                           break;
                                       case "2": desc = "2-通知";
                                           break;
                                       case "3": desc = "3-报警";
                                           break;
                                   }
                                   return desc;
                               }
                           },
                           { field: 'intime', title: '通知时间', width: 70, align: 'left' },
                           { field: 'usingqty', title: '应发数量', width: 50, align: 'left' },
                           { field: 'usedqty', title: '已发数量', width: 50, align: 'left' },
                           { field: 'isqueued', title: '发送状态', width: 50, align: 'left' },
                           { field: 'partid', title: '料件编号', width: 70, align: 'left' },
                           { field: 'partprocessno', title: '料号途程', width: 70, align: 'left' },
                           { field: 'processversion', title: '途程版本', width: 50, align: 'left' },
                           { field: 'linetypename', title: '线别', width: 50, align: 'left' },
                           {
                               field: 'opt', title: '操作', width: 50, align: 'left',
                               formatter: function (value, row, index) {
                                   return '<a href="#" onclick="view(' + index + ')">推送状况</a>';
                                   //return '<a href="#" onclick="view(' + index + ')">查看</a> | <a href="#" onclick="runflow(' + index + ')">改善对策</a>';
                               }
                           }
                ]]
            });
            $(window).resize(function () {
                $('#tbSendLog').datagrid('resize');
            });

            //設置分页控件屬性 
            var p = $('#tbSendLog').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: [10, 15, 20, 25, 30],
                beforePageText: '第',
                afterPageText: '页 共{pages}页',
                displayMsg: '当前显示 {from} - {to} 条记录 共 {total} 条记录'
            });

            //綁定datagrid
            $('#dtDetail').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                //url: '../../ASHX/ESOP/SendLog.ashx?M=SearchDetail',
                //數據在一行顯示 
                nowrap: false,
                fitColumns: true,
                //行條紋化
                striped: true,
                //固定序號
                rownumbers: false,
                //是否可以多選
                singleSelect: true,
                remoteSort: false,
                idField: 'autoid',
                loadMsg: '數據加載中...',//數據加載中
                //是否顯示分页
                pagination: false,
                //可動列
                columns: [[
                           {
                               field: 'autoid', title: '', width: 15, align: 'center', formatter: function (value, row, index) {
                                   var div = "";
                                   if (row.result == "1")
                                       div = "<div class='greenbox'></div>";
                                   else if (row.result == "2")
                                       div = "<div class='redbox'></div>";
                                   else if (row.result == "0" && row.messagetype == "0")
                                       div = "<div class='greybox'></div>";
                                   else
                                       div = "<div class='yellowbox'></div>";
                                   return div;
                               }
                           },
                           { field: 'equipmentid', title: '设备编号', width: 40, align: 'left' },
                           {
                               field: 'isonline', title: '设备状态', width: 40, align: 'left', formatter: function (value, row, index) {
                                   var desc = value;
                                   switch (row.isonline) {
                                       case "1": desc = "1-在线";
                                           break;
                                       case "0": desc = "0-离线";
                                           break;
                                       default: desc = "未激活";
                                           break;
                                   }
                                   return desc;
                               }
                           },
                           { field: 'macaddress', title: 'MAC地址', width: 70, align: 'left' },
                           { field: 'intime', title: '入队时间', width: 70, align: 'left' },
                           {
                               field: 'result', title: '推送结果', width: 40, align: 'left', formatter: function (value, row, index) {
                                   var desc = value;
                                   switch (row.result) {
                                       case "0": desc = "0-排队中";
                                           break;
                                       case "1": desc = "1-已发送";
                                           break;
                                       case "2": desc = "2-已失败";
                                           break;
                                   }
                                   return desc;
                               }
                           },
                           {
                               field: 'reasoncode', title: '原因码', width: 70, align: 'left', formatter: function (value, row, index) {
                                   var desc = value;
                                   //原因码：1 - 发送成功; 2 - 发送失败；3 - 过期未发送；4 - 更换SOP未发送；5 - 发送未回馈；6 - 设备未在线；7 - 消息压缩无内容; 8 - 消息不完整
                                   switch (row.reasoncode) {
                                       case "1": desc = "1-已送达";
                                           break;
                                       case "2": desc = "2-发送失败";
                                           break;
                                       case "3": desc = "3-过期未发送";
                                           break;
                                       case "4": desc = "4-更换SOP未发送";
                                           break;
                                       case "5": desc = "5-发送未回馈";
                                           break;
                                       case "6": desc = "6-设备异常离线";
                                           break;
                                       case "7": desc = "7-消息压缩失败";
                                           break;
                                       case "8": desc = "8-消息不完整";
                                           break;
                                   }
                                   return desc;
                               }
                           },
                           {
                               field: 'opt', title: '操作', width: 40, align: 'left',
                               formatter: function (value, row, index) {
                                   //'<a href="javascript:void window.open("https://www.sina.com.cn/","XX","left=0,top=0,width='+ (screen.availWidth - 10) +',height='+ (screen.availHeight-50) +',scrollbars,resizable=yes,toolbar=no')">Open</a>'
                                   return '<a href="#" class="start" onclick="openSM(\'' + row.messageid + '\')">预览</a>';
                               }
                           }
                ]],
                onLoadSuccess: function (data) {
                    $('.start').linkbutton({ text: '预览', plain: true, iconCls: 'icon-start' });
                }
            });
        });

        //Grid初使加載的數據
        function Search(SearchType) {
            var queryParams = $('#tbSendLog').datagrid('options').queryParams;
            //請輸入關鍵字            
            queryParams.KeyWord = $('#txtKeyword').val().replace('請輸入關鍵字', '');
            queryParams.SearchType = SearchType;
            queryParams.messagetype = $('#smessagetype').combobox("getValue");
            queryParams.partid = $('#spartid').textbox("getValue");
            queryParams.line = $('#sline').textbox("getValue");
            queryParams.isqueued = $('#sisqueued').combobox("getValue");
            queryParams.starttime = $("#txtStartTime").datebox('getValue');
            queryParams.endtime = $("#txtEndTime").datebox('getValue');
            $('#tbSendLog').datagrid('reload');
        }

        /**
      * 时间格式化
      * @param value
      * @returns {string}
      */
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
        /**
         * 解析时间
         */
        function dateParser(s) {
            if (s != "") {
                var dt = s.split(" ");
                var d = dt[0].split("-");
                var y = parseInt(d[0], 10);
                var m = parseInt(d[1], 10) - 1;
                var d = parseInt(d[2], 10);
                return new Date(y, m, d);
            } else {
                return new Date();
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

        function view(index) {
            var row = $('#tbSendLog').datagrid('getData').rows[index];
            $("#vintime").textbox("setValue", row.intime);
            $("#vintime").textbox("disable");
            if (row.usingqty <= 0) {
                $("#vusedprogress").progressbar("setValue", 100);
                $("#vusedprogress").progressbar("setText", "0/0");
            } else {
                $("#vusedprogress").progressbar("setValue", (row.usedqty / row.usingqty) * 100);
                $("#vusedprogress").progressbar("setText", row.usedqty + "/" + row.usingqty);
            }

            //加载明细
            $.post('../../ASHX/ESOP/SendLog.ashx?M=SearchDetail',
               {
                   ticketid: row.ticketid
               },
               function (result) {
                   $("#dtDetail").datagrid("loadData", result);
               },
               'json');
            $('#divView').dialog('open').dialog('setTitle', '推送明细');
        }
        function openSM(messageid) {
            window.open('QMSClient.aspx?messageid=' + messageid, '_blank', 'left=0,top=0,height=' + screen.height + ',width =' + screen.width + ',scrollbars,resizable=yes,toolbar=no,location=no,menubar=no');
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
                <table class="addCoyTB" style="height: 40px; border: 0;">
                    <tr style="height: 25px;">
                        <td style="width: 70px;">推送类别：</td>
                        <td style="width: 155px;">
                            <select class="easyui-combobox" style="width: 150px;" id="smessagetype" name="messagetype">
                                <option selected="selected" value="">ALL</option>
                                <option value="1">1-SOP</option>
                                <option value="2">2-通知</option>
                                <option value="3">3-报警</option>
                            </select>
                        </td>
                        <td style="width: 70px;">料件编号：</td>
                        <td style="width: 155px;">
                            <input class="easyui-textbox" style="width: 150px;" id="spartid" name="partid" />
                        </td>
                        <td style="width: 70px;">线别编号：</td>
                        <td style="width: 155px;">
                            <input class="easyui-textbox" name="line" id="sline" style="width: 150px;" />

                        </td>
                    </tr>
                    <tr style="height: 25px;">
                        <td style="width: 70px;">推送状态：</td>
                        <td style="width: 155px;">
                            <select class="easyui-combobox" style="width: 150px;" id="sisqueued" name="isqueued">
                                <option selected="selected" value="">ALL</option>
                                <option value="0">0-已建立</option>
                                <option value="1">1-已入队</option>
                                <option value="2">2-已推送</option>
                            </select>
                        </td>
                        <td style="width: 70px;">推送时间：
                        </td>
                        <td colspan="3">
                            <input type="text" id="txtStartTime" class="easyui-datebox" style="width: 150px;" data-options="formatter: dateFormatter, parser: dateParser" />
                            -
                            <input type="text" id="txtEndTime" class="easyui-datebox" style="width: 150px;" data-options="formatter: dateFormatter, parser: dateParser" />
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

        <table id="tbSendLog" data-options="fit:false">
        </table>
    </div>

    <div id="divView" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:true"
        style="width: 850px; height: 450px; padding: 5px;">
        <div class="easyui-layout" style="width: 100%; height: 100%;">
            <div data-options="region:'north',border:true" style="height: 50px; padding: 1px;">
                <table style="height: 100%; vertical-align: middle;">
                    <colgroup>
                        <col span="1" style="width: 95px;" />
                        <col span="1" style="width: 270px" />
                        <col span="1" style="width: 95px;" />
                        <col span="1" style="width: 270px;" />
                    </colgroup>
                    <tbody>
                        <tr>
                            <td style="height: 25px; text-align: right;">通知时间：
                            </td>
                            <td style="height: 25px;">
                                <input id="vintime" class="easyui-textbox" data-options="disable:true" />
                            </td>
                            <td style="height: 25px; text-align: right;">通知进度:</td>
                            <td>
                                <div id="vusedprogress" class="easyui-progressbar" style="width: 200px;"></div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div data-options="region:'center',border:false,fit:false" style="padding-top: 2px; padding-bottom: 5px;">
                <table id="dtDetail" data-options="fit:true,border:true"></table>
            </div>
        </div>
    </div>
</body>
</html>
