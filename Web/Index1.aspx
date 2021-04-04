<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index1.aspx.cs" Inherits="Web.Index1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>DMC控制台</title>
    <link href="css/base.css" rel="stylesheet" />
    <link href="css/platform.css?v=7" rel="stylesheet" />
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <%--<link href="../../easyUI15/themes/default/easyui.css?v=1" rel="stylesheet" type="text/css" />--%>
    <link href="easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css"/>
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <style>
        .work-inner .work-item i {
            left: -2px;
        }

        .work-inner {
            padding-right: 6px;
            width: 16%;
            float: left;
        }

            .work-inner .work-item {
                position: relative;
                height: 111px;
                color: #fff;
            }

                .work-inner .work-item i {
                    position: absolute;
                    font-size: 60px;
                    left: 11px;
                    top: 22px;
                }

                .work-inner .work-item span.num {
                    position: absolute;
                    left: 80px;
                    top: 30px;
                    font-size: 32px;
                }

                    .work-inner .work-item span.num span {
                        font-size: 14px;
                    }

                .work-inner .work-item label {
                    position: absolute;
                    top: 70px;
                    font-size: 14px;
                    left: 80px;
                }

                .work-inner .work-item.blue {
                    background-color: #11a9e2;
                }

                .work-inner .work-item.green {
                    background-color: #1da02b;
                }

                .work-inner .work-item.red {
                    background-color: #f45438;
                }

                .work-inner .work-item.purple {
                    background-color: #9b59b6;
                }

                .work-inner .work-item.gray {
                    background-color: #4f5c65;
                }

                .work-inner .work-item.yellow {
                    background-color: gold;
                    margin-bottom: 10px;
                }

        .center-items.chart0 {
            height: 310px;
        }

            .center-items.chart0 .chart0-item {
                float: left;
                width: 50%;
            }

                .center-items.chart0 .chart0-item .item-inner {
                    padding-right: 10px;
                }

                    .center-items.chart0 .chart0-item .item-inner .item-content {
                        height: 300px;
                        border: 1px solid #95B8E7;
                    }

                        .center-items.chart0 .chart0-item .item-inner .item-content .content-hd {
                            padding: 0px 10px;
                            height: 35px;
                            line-height: 35px;
                            border-bottom: 1px solid #95B8E7;
                            background-color: #E0ECFF;
                            font-weight: bold;
                        }

                        .center-items.chart0 .chart0-item .item-inner .item-content .chart-chart {
                            padding: 2px;
                            height: 260px;
                        }

        .center-items.chart1 {
            padding-right: 10px;
            height: 310px;
            border: 0;
            margin-bottom: 0;
        }

            .center-items.chart1 .chart1-inner {
                height: 300px;
                border: 1px solid #bfbfbf;
            }

                .center-items.chart1 .chart1-inner .chart1-chart {
                    padding: 2px;
                    height: 260px;
                }
    </style>
    <style type="text/css">
        @charset "UTF-8";

        .f-sort {
            float: right;
            margin-right: -5px;
        }

            .f-sort .arrow, .filter .f-sort .arrow-bottom, .filter .f-sort .arrow-top {
                float: left;
                width: 7px;
                overflow: hidden;
                background: url(images/main/sprite-arrow.png) no-repeat 0 -100px;
            }

            .f-sort .arrow {
                height: 11px;
                margin-top: 6px;
            }

            .f-sort .arrow-top {
                height: 4px;
                margin-top: 6px;
                background-position: -10px -100px;
            }

            .f-sort .arrow-bottom {
                height: 4px;
                margin-top: 3px;
                background-position: -10px -110px;
            }

            .f-sort .fs-down, .filter .f-sort .fs-up {
                display: inline-block;
                width: 7px;
                margin-left: 5px;
                vertical-align: top;
                *cursor: pointer;
            }

            .f-sort .fs-tit {
                display: inline-block;
                vertical-align: top;
                *cursor: pointer;
            }

            .f-sort a {
                float: left;
                padding: 0 9px;
                height: 23px;
                border: 1px solid #CCC;
                line-height: 23px;
                margin-right: -1px;
                background: #FFF;
                color: #333;
            }

                .f-sort a:hover {
                    position: relative;
                    text-decoration: none;
                    border-color: #e4393c;
                    color: #e4393c;
                }

                    .f-sort a:hover .fs-down .arrow {
                        background-position: 0 -140px;
                    }

                .f-sort a.curr {
                    border-color: #e4393c;
                    background: #e4393c;
                    color: #FFF;
                }

                    .f-sort a.curr .arrow-top {
                        background-position: -10px -120px;
                    }

                    .f-sort a.curr .arrow-bottom {
                        background-position: -10px -130px;
                    }

                    .f-sort a.curr .fs-down .arrow {
                        background-position: 0 -120px;
                    }

                    .f-sort a.curr .fs-down .arrow-top, .filter .f-sort a.curr .fs-up .arrow-bottom {
                        filter: alpha(opacity=50);
                        -moz-opacity: .5;
                        opacity: .5;
                    }

                    .f-sort a.curr:hover {
                        color: #fff;
                    }
    </style>
    <script src="JSPage/echarts.min.js"></script>
    <%--<script src="JSPage/echarts-all.js"></script>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#SComanyType').combobox({
                url: 'ASHX/Basic/ComboxManage.ashx?ComboxType=MyCompany',
                valueField: 'compid',
                textField: 'simplename',
                panelHeight: '100',
                editable: false,
                multiple: false,
                onLoadSuccess: function () {
                    $("#SComanyType").combobox("setText", '<%=CompanyName()%>');
                },
                onChange: function (newValue, oldValue) {
                    location.href = "Index.aspx?cb=" + newValue;
                }
            });

            $(".easyui-tree").tree('collapseAll');
        });
    </script>
    <script type="text/javascript">
        function addTab(title, url) {
            if ($('#tabs').tabs('exists', title)) {
                $('#tabs').tabs('select', title); //选中并刷新
            } else {
                var content = createFrame(url);
                $('#tabs').tabs('add', {
                    title: title,
                    content: content,
                    closable: true
                });
            }
            tabClose();
        }
        function createFrame(url) {

            var s = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
            return s;
        }

        function tabClose() {
            /*双击关闭TAB选项卡*/
            $(".tabs-inner").dblclick(function () {
                var subtitle = $(this).children(".tabs-closable").text();
                $('#tabs').tabs('close', subtitle);
            })
            /*为选项卡绑定右键*/
            $(".tabs-inner").bind('contextmenu', function (e) {
                $('#mm').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });

                var subtitle = $(this).children(".tabs-closable").text();

                $('#mm').data("currtab", subtitle);
                $('#tabs').tabs('select', subtitle);
                return false;
            });
        }
        // divUpdatePwd
        function openPwd() {
            $('#divUpdatePwd').window('open');
        }
        function closePwd() {
            $('#divUpdatePwd').window('close');
        }

        function closeAccount() {
            $('#divAccount').window('close');
        }
        //divSelectAccount
        function openSelectAccount() {
            $('#divSelectAccount').window('open');
        }
        function closeSelectAccount() {
            $('#divSelectAccount').window('close');
        }

        //绑定右键菜单事件
        function tabCloseEven() {
            //刷新
            $('#mm-tabupdate').click(function () {
                var currTab = $('#tabs').tabs('getSelected');
                var url = $(currTab.panel('options').content).attr('src');
                if (url != undefined && currTab.panel('options').title != 'Home') {
                    $('#tabs').tabs('update', {
                        tab: currTab,
                        options: {
                            content: createFrame(url)
                        }
                    })
                }
            })
            //关闭当前
            $('#mm-tabclose').click(function () {
                var currtab_title = $('#mm').data("currtab");
                $('#tabs').tabs('close', currtab_title);
            })

        }

        $(function () {
            tabCloseEven();
            $('.cs-navi-tab').click(function () {
                var $this = $(this);
                var href = $this.attr('src');
                var programid = $this.attr('programid');
                var title = $this.text() + '(' + programid + ')';
                addTab(title, href);
            });

            //左侧菜单收起
            $(document).on('click', '.toggle-icon', function () {
                $(this).closest("#pf-bd").toggleClass("toggle");
                setTimeout(function () {
                    $(window).resize();
                }, 300)
            });
            $(".tabs-first").css("margin-left", "20px");
        });

        function showCompany(a, b) {
            var obj = document.getElementById("divCompany");
            var aList = obj.getElementsByTagName("a");
            if (aList.length > 0) {
                if (obj.style.display == "block") {
                    obj.style.display = "none";
                }
                else {
                    obj.style.display = "block";
                }
                var text = document.getElementById("spanCompany");
                if (a != "") {
                    text.innerHTML = a;
                    location.href = "Index.aspx?cb=" + b;
                }
            }
        }

        function showUserInfo() {
            var divobj = document.getElementById("divUserInfo");
            if (divobj.style.display == "block") {
                divobj.style.display = "none";
            }
            else {
                divobj.style.display = "block";
            }
        }

        //保持後端連接的時長
        function getData() {
            var temp = "";
            $.get("ASHX/Basic/ComboxManage.ashx?ComboxType=languagetype", { time: new Date() }, function (result) {//利用ajax返回
                //定時獲取信息，不處理響應
            })
        };

        $(function () {//间隔60s自动加载一次   
            getData(); //首次立即加载   
            window.setInterval(getData, 60 * 1000); //循环执行！！   
        });

        //修改用户密码
        function UpdatePwd() {
            var oldpwd = $("#txtOldUserPwd").val();
            var newpwd = $("#txtNewPwd").val();
            var pwdtwo = $("#txtAgainNewPwd").val();

            if (oldpwd == "") {
                alert('<%=Language("oldpwd_msg")%>');
                $("#txtOldUserPwd").focus();
                return;
            }
            else if (newpwd == "") {
                alert('<%=Language("newpwd_msg")%>');
                $("#txtNewPwd").focus();
                return;
            }
            else if (!newpwd.match(/[0-9 | A-Z | a-z]{6,30}/)) {
                alert('<%=Language("pwdlimitlength")%>'); // pwdlimitlength 密码至少为6位
                return;
            }
            else if (newpwd.match(/[A-Za-z]/g) == null || newpwd.match(/[A-Za-z]/g).length < 2) {
                alert('<%=Language("pwdlimit2e")%>'); //pwdlimit2e 密码至少需要保护两位英文
                return;
            }

            else if (pwdtwo == "") {
                alert('<%=Language("pwdtwo_msg")%>');
                $("#txtAgainNewPwd").focus();
                return;
            }
            else if (newpwd != pwdtwo) {
                alert('<%=Language("pwd_no")%>');
                    $("#txtAgainNewPwd").focus();
                    return;
                }

    if (window.navigator.onLine != true) {
        $.messager.alert({
            title: '<%=Language("ExceptionTips")%>', //ExceptionTips 異常提示
            msg: '<%=Language("SubmitDataException")%>'//SubmitDataException 提交數據異常，請確認網絡連接狀況
        });
        return;
    }

    $.post("ASHX/Permission/ModPwd.ashx", { oldpwd: oldpwd, pwd: newpwd, pwdtwo: pwdtwo, time: new Date() }, function (result) {//利用ajax返回
        if (result.indexOf("OK") > -1) {//返回有数据   

            alert('<%=Language("pwdmodok")%>');
            $('#divUpdatePwd').dialog('close');
            $('#divUpdatePwd').form('clear');
        }
        else if (result.indexOf("ERR") > -1) {//返回有数据   
            alert('<%=Language("pwdoldpwdisno")%>');
                $("#txtOldUserPwd").focus();
            }
    })
}
    </script>
</head>
<body runat="server">
    <div class="container">
        <div id="pf-hd">
            <div class="pf-logo">
                <div style="white-space: nowrap; font-size: 35px; color: white; text-align: left; width: 100%; position: absolute; margin-top: -10px; float: left;">
                    DMC
                </div>
                <div style="white-space: nowrap; font-size: 18px; color: white; text-align: left; width: 100%; height: 35px; position: absolute; margin-top: 20px; float: left;">
                    Device Management System
                </div>

            </div>
            <%--<div class="pf-nav-wrap">
            </div>--%>
            <div id="divuser" class="pf-user">
                <div class="pf-user-photo">
                    <img src="images/main/user.png" alt="头像" />
                </div>
                <h4 class="pf-user-name ellipsis"><%=u.userName %></h4>
                <i class="iconfont xiala">&#xe607;</i>
                <div class="pf-user-panel">
                    <ul class="pf-user-opt">
                        <li>
                            <a href="javascript:showUserInfo();">
                                <i class="iconfont">&#xe60d;</i>
                                <span class="pf-opt-name">用户信息</span>
                            </a>
                        </li>
                        <li class="pf-modify-pwd">
                            <a href="javascript:openPwd();">
                                <i class="iconfont">&#xe634;</i>
                                <span class="pf-opt-name">修改密码</span>
                            </a>
                        </li>
                        <li class="pf-modify-help">
                            <a href="#">
                                <i class="iconfont">&#xe62f;</i>
                                <span class="pf-opt-name">帮助信息</span>
                            </a>
                        </li>
                        <li class="pf-logout">
                            <a href="login.html">
                                <i class="iconfont">&#xe60e;</i>
                                <span class="pf-opt-name">退出</span>
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="pf-user">
                <h1 class="pf-user-name ellipsis">数据屏</h1>
                <%--<input class="easyui-combobox" id="SComanyType" style="height: 30px; width: 120px;" />--%>
            </div>
        </div>
        <div id="pf-bd">
            <div id="pf-sider">
                <div data-options="region:'north',border:false" style="height: 31px;">
                    <h2 class="pf-model-name">
                        <span class="iconfont">&#xe64a;</span>
                        <span class="pf-name">系统功能导航</span>
                        <span class="toggle-icon"></span>
                    </h2>
                </div>
                <div data-options="region:'center',border:false" style="height: 90%;">
                    <%= InitRight()%>
                </div>
            </div>
            <div id="pf-page">
                <div class="easyui-tabs" style="width: 100%; height: 100%; z-index: -1;" id="tabs">
                    <div title="首页" style="padding: 10px 5px 5px 10px;">
                        <div class="container">
                            <div class="work-inner">
                                <div class="work-item gray">
                                    <i class="iconfont">&#xe620;</i>
                                    <span class="num">10&nbsp;</span>
                                    <label>待分配</label>
                                </div>
                            </div>
                            <div class="work-inner">
                                <div class="work-item purple">
                                    <i class="iconfont">&#xe61f;</i>
                                    <span class="num">8&nbsp;</span>
                                    <label>&nbsp;&nbsp;处理中</label>
                                </div>
                            </div>
                            <div class="work-inner">
                                <div class="work-item yellow">
                                    <i class="iconfont">&#xe61e;</i>
                                    <span class="num">4&nbsp;</span>
                                    <label>&nbsp;&nbsp;待确认</label>
                                </div>
                            </div>
                            <div class="work-inner">
                                <div class="work-item red">
                                    <i class="iconfont">&#xe622;</i>
                                    <span class="num">15&nbsp;</span>
                                    <label>未接单</label>
                                </div>
                            </div>
                            <div class="work-inner">
                                <div class="work-item green">
                                    <i class="iconfont">&#xe62f;</i>
                                    <span class="num">48&nbsp;</span>
                                    <label>已完成</label>
                                </div>
                            </div>
                            <div class="work-inner">
                                <div class="work-item blue">
                                    <i class="iconfont">&#xe64a;</i>
                                    <span class="num">15&nbsp;</span>
                                    <label>总数</label>
                                </div>
                            </div>
                            <div style="clear: both">
                            </div>
                            <div class="center-items chart0 clearfix">
                                <div class="chart0-item">
                                    <div class="item-inner">
                                        <div class="item-content">
                                            <div class="content-hd">
                                                <div style="float: left;">报修分析统计</div>
                                                <div class="f-sort">
                                                    <a href="javascript:;" class="curr" id="partbyd" onclick="Show('dd')"><span class="fs-tit">当日</span><em class="fs-down"><i class="arrow"></i></em></a>
                                                    <a href="javascript:;" class="" id="partbyw" onclick="Show('ww')"><span class="fs-tit">当周</span><em class="fs-down"><i class="arrow"></i></em></a>
                                                    <a href="javascript:;" class="" id="partbym" onclick="Show('mm')"><span class="fs-tit">当月</span><em class="fs-down"><i class="arrow"></i></em></a>
                                                    <a href="javascript:;" class="" id="partbyy" onclick="Show('yy')"><span class="fs-tit">当年</span><em class="fs-down"><i class="arrow"></i></em></a>
                                                </div>
                                            </div>
                                            <div class="chart-chart" id="chart0"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="chart0-item">
                                    <div class="item-inner">
                                        <div class="item-content">
                                            <div class="content-hd">
                                                <div style="float: left;" id="divTitle">报修趋势分析</div>
                                                <div class="f-sort">
                                                    <a href="javascript:;" class="curr" id="lined" onclick="ShowLine('dd')"><span class="fs-tit">每日</span><em class="fs-down"><i class="arrow"></i></em></a>
                                                    <a href="javascript:;" class="" id="linew" onclick="ShowLine('ww')"><span class="fs-tit">每周</span><em class="fs-down"><i class="arrow"></i></em></a>
                                                    <a href="javascript:;" class="" id="linem" onclick="ShowLine('mm')"><span class="fs-tit">每月</span><em class="fs-down"><i class="arrow"></i></em></a>
                                                </div>
                                            </div>
                                            <div class="chart-chart" id="chart1"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="pf-ft">
            <div class="system-name">
                <%--<i class="iconfont">&#xe6fe;</i>--%>
                <span>智能数据应用系统&nbsp;v1.0</span>
            </div>
            <div class="copyright-name">
                <span>CopyRight&nbsp;2020&nbsp;&nbsp;Asia Vital Components Co.,Ltd.&nbsp;All rights reserved</span>
                <%--<i class="iconfont">&#xe6ff;</i>--%>
            </div>
        </div>
    </div>

    <div id="divUserInfo" style="top: 40px; right: 150px; position: absolute; z-index: 4; display: none; background-color: #e0ecff;">
        <div title='<%=Language("EmpBasicInfo")%>' style="overflow: auto; padding: 5px; text-align: left;">
            <table cellpadding="0" cellspacing="0" class="tbStyle">
                <tr>
                    <td bgcolor="#e8f5ff">
                        <%=Language("UserId")%>：
                    </td>
                    <td>
                        <%=u.userID%>
                    </td>
                    <td bgcolor="#e8f5ff">
                        <%=Language("emp_name")%>：
                    </td>
                    <td>
                        <%=u.userName%>
                    </td>
                    <td bgcolor="#e8f5ff">
                        <%=Language("emp_dept")%>：
                    </td>
                    <td>
                        <%=u.userDept%>
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#e8f5ff">
                        <%=Language("emp_mail")%>：
                    </td>
                    <td>
                        <%=u.userMail%>
                    </td>
                    <td bgcolor="#e8f5ff">
                        <%=Language("LoginLastTime")%>：
                    </td>
                    <td>
                        <%=u.lastLoginTime%>
                    </td>
                    <td bgcolor="#e8f5ff">
                        <%=Language("LastLoginIP")%>：
                    </td>
                    <td colspan="3">
                        <%=u.lastLoginIP%>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div id="divUpdatePwd" class="easyui-dialog" data-options="closed:true,modal:true,buttons:'#dlg-buttons',title:'<%=Language("ModPwd")%>'"
        style="width: 350px; height: 245px; padding: 5px;">
        <table style="width: 95%; height: 100%;">
            <tr>
                <td style="width: 35%; height: 25px; text-align: right;">
                    <%=Language("UserId")%>:&nbsp;&nbsp;
                </td>
                <td style="width: 65%; text-align: left;">
                    <%=userID%>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; height: 25px;">
                    <%=Language("UserName")%>:&nbsp;&nbsp;
                </td>
                <td style="text-align: left;">
                    <%=u.userName %>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; height: 25px;">
                    <%=Language("oldpwd")%>:&nbsp;&nbsp;
                </td>
                <td style="text-align: left;">
                    <input type="password" id="txtOldUserPwd" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right; height: 25px;">
                    <%=Language("newpwd")%>:&nbsp;&nbsp;
                </td>
                <td style="text-align: left;">
                    <input type="password" id="txtNewPwd" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right; height: 25px;">
                    <%=Language("comformPwd")%>:&nbsp;&nbsp;
                </td>
                <td style="text-align: left;">
                    <input type="password" id="txtAgainNewPwd" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dlg-buttons">
        <a class="easyui-linkbutton" id="btnSave" data-options="iconCls:'icon-save',plain:true" onclick="UpdatePwd()"><%=Language("Save")%></a>
        <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel',plain:true" onclick="closePwd()"><%=Language("Cancel")%></a>
    </div>
</body>
</html>

<script type="text/javascript">
    var myChart0 = echarts.init(document.getElementById('chart0'));
    var Show = function (s) {
        $("#partbyd").removeClass("curr")
        $("#partbyw").removeClass("curr")
        $("#partbym").removeClass("curr")
        $("#partbyy").removeClass("curr")
        switch (s) {
            case "dd":
                $("#partbyd").addClass("curr");
                break;
            case "ww":
                $("#partbyw").addClass("curr");
                break;
            case "mm":
                $("#partbym").addClass("curr");
                break;
            case "yy":
                $("#partbyy").addClass("curr");
                break;
        }

        $.post("../../ASHX/Data/DataSetting.ashx?M=gettop5part",
          {
              datetype: s
          },
          function (result) {
              //数据查询
              var parts = [];
              var partPer = [];
              for (var i = 0; i < result.length; i++) {
                  parts.push(result[i].partid);
                  partPer.push({ value: result[i].warnnumber, name: result[i].partid });
              }
              myChart0.clear();
              myChart0.hideLoading();
              myChart0.setOption({
                  tooltip: {
                      trigger: 'item',
                      formatter: "{a} <br/>料号:{b} <br/>次数:{c} ({d}%)"
                  },
                  legend: {
                      orient: 'vertical',
                      x: 'left',
                      data: parts,
                      show: false
                  },
                  toolbox: {
                      show: false,
                      feature: {
                          mark: { show: true },
                          dataView: { show: true, readOnly: false },
                          magicType: {
                              show: true,
                              type: ['pie', 'funnel'],
                              option: {
                                  funnel: {
                                      x: '25%',
                                      width: '50%',
                                      funnelAlign: 'center',
                                      max: 1548
                                  }
                              }
                          },
                          restore: { show: true },
                          saveAsImage: { show: true }
                      }
                  },
                  calculable: true,
                  series: [{
                      name: '料号状况',
                      type: 'pie',
                      radius: ['70%', '90%'],
                      itemStyle: {
                          normal: {
                              label: {
                                  show: false
                              },
                              labelLine: {
                                  show: false
                              }
                          },
                          emphasis: {
                              label: {
                                  show: true,
                                  position: 'center',
                                  textStyle: {
                                      fontSize: '30',
                                      fontWeight: 'bold'
                                  }
                              }
                          }
                      }, data: partPer
                  }]
              }, true);
          },
          'json');
    }



    //chart1 
    var myChart1 = echarts.init(document.getElementById('chart1'));
    var ShowLine = function (s) {
        $("#lined").removeClass("curr");
        $("#linew").removeClass("curr");
        $("#linem").removeClass("curr");
        //switch (s) {
        //    case "dd":
        //        $("#lined").addClass("curr");
        //        $("#divTitle").html("7日预警状况");
        //        break;
        //    case "ww":
        //        $("#linew").addClass("curr");
        //        $("#divTitle").html("7周预警状况");
        //        break;
        //    case "mm":
        //        $("#linem").addClass("curr");
        //        $("#divTitle").html("7月预警状况");
        //        break;
        //}

        $.post("../../ASHX/Data/DataSetting.ashx?M=gettop7line",
         {
             datetype: s
         },
         function (result) {
             //数据填充
             myChart1.clear();
             myChart1.hideLoading();
             myChart1.setOption({
                 tooltip: {
                     trigger: 'axis'
                 },
                 legend: {
                     data: ['一级预警', '二级预警', '三级预警'],
                     show: false
                 },
                 toolbox: {
                     show: false,
                     feature: {
                         mark: { show: true },
                         dataView: { show: true, readOnly: false },
                         magicType: { show: true, type: ['line', 'bar', 'stack', 'tiled'] },
                         restore: { show: true },
                         saveAsImage: { show: true }
                     }
                 },
                 calculable: true,
                 xAxis: [
                     {
                         type: 'category',
                         boundaryGap: false,
                         data: result.DateTypes
                     }
                 ],
                 yAxis: [
                     {
                         type: 'value'
                     }
                 ],
                 series: [
                     {
                         name: '一级预警',
                         type: 'line',
                         stack: '总量',
                         data: result.Level1
                     },
                     {
                         name: '二级预警',
                         type: 'line',
                         stack: '总量',
                         data: result.Level2
                     },
                     {
                         name: '三级预警',
                         type: 'line',
                         stack: '总量',
                         data: result.Level3
                     }
                 ]
             }, true);
         },
         'json');
    }

    $(document).ready(function () {
        Show("dd");
        ShowLine("dd");
    });

</script>
