<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>首页</title>
    <link href="css/base.css" rel="stylesheet" />
    <link href="css/platform.css?v=7" rel="stylesheet" />
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <%--<link href="../../easyUI15/themes/default/easyui.css?v=1" rel="stylesheet" type="text/css" />--%>
    <link href="easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="JSPage/echarts.min.js"></script>

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
            $(".easyui-tree").tree('expandAll'); //collapseAll
           <%-- var DefaultRole = '<%=defaultRole%>';
            if (DefaultRole == "操作员" ) {
               // addTab("生产报修(eqwi001)", "UI/Equment/RepairForm.aspx");
                //$("#homepage").src = "UI/Equment/RepairForm.aspx";
                $('#homepage').attr('src', 'UI/Equment/RepairForm.aspx');
                //$('#divhomepage').attr('title', '生产报修');
               
            }--%>
        }
        
        );
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
        function appointTab(title, url) {
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
        var lastTimeId = "";
        $(function () {//间隔60s自动加载一次   
            getData(); //首次立即加载 
            if (!!lastTimeId) {
                window.clearInterval(lastTimeId);
            }
            lastTimeId=window.setInterval(getData, 60 * 1000); //循环执行！！   
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
<body runat="server" style="height: 100%;">
    <div class="container">
        <div id="pf-hd">
            <div class="pf-logo">
                <img src="images/cbigammlogo.png" style="width: 102px; height: 50px; margin-top: -10px;" />
            </div>
            <div class="pf-logo" style="margin-left: -127px;">
                <div style="white-space: nowrap; font-size: 25px; color: white; text-align: left; width: 100%; position: absolute; margin-top: -10px; float: left;">
                    DMC
                </div>
                <div style="white-space: nowrap; font-size: 15px; color: white; text-align: left; width: 100%; height: 25px; position: absolute; margin-top: 10px; float: left;">
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
                <h1 class="pf-user-name ellipsis"><a href="#" onclick="window.open('UI/Equment/Kanban.aspx');">数据屏</a></h1>
                <%--<input class="easyui-combobox" id="SComanyType" style="height: 30px; width: 120px;" />--%>
            </div>
            <div class="pf-user">
                
                <h1 class="pf-user-name ellipsis"><a href="#" onclick="window.open('UI/Equment/TingjiFenxi.aspx');">停机分析</a></h1>
                <%--<input class="easyui-combobox" id="SComanyType" style="height: 30px; width: 120px;" />--%>
            </div>
        </div>
        <div id="pf-bd" style="height: calc(100% - 77px);">
            <div id="pf-sider">
                <div data-options="region:'north',border:false" style="height: 33px;">
                    <h2 class="pf-model-name">
                        <span class="iconfont">&#xe64a;</span>
                        <span class="pf-name">系统功能导航</span>
                        <span class="toggle-icon"></span>
                    </h2>
                </div>
                <div data-options="region:'center',border:true" style="height: calc(100% - 33px);">
                    <%= InitRight()%>
                </div>
            </div>
            <div id="pf-page">
                <div class="easyui-tabs" style="width: 100%; height: 100%; z-index: -1;" id="tabs">
                    <div title=<%=getDefaultTitle()%> id="divhomepage" style="width: calc(100% - 1px); height: calc(100% - 1px); position:relative;" data-options="fit:true">
                        <iframe scrolling='no' id="homepage" frameborder="0" src=<%=getDefaultPage() %> style="width: calc(100% - 1px); height: calc(100% - 1px);position:absolute;left:0px;top:0px;"></iframe>
                    </div>
                </div>
            </div>
        </div>
        <div id="pf-ft">
            <div class="system-name">
                <%--<i class="iconfont">&#xe6fe;</i>--%>
                <span>排单应用系统&nbsp;v1.0</span>
            </div>
            <div class="copyright-name">
                <span>CopyRight&nbsp;2020&nbsp;&nbsp;Advanced Assembly Materials International Limited Co.,Ltd.&nbsp;All rights reserved</span>
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
