<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviceManage.aspx.cs" Inherits="Web.UI.Equment.DeviceManage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>设备管理</title>
    <%--<script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>--%>
    <script src="../../js/jquery-3.1.1.js" type="text/javascript"></script>
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.json-2.4.js"></script>
    <script src="../../easyUI15/locale/easyui-lang-zh_TW.js"></script>
    <script src="../../js/qrcode.min.js"></script>
    <script src="../../js/jquery.jqprint-0.3.js"></script>
    <script src="../../js/jquery-migrate-1.2.1.min.js"></script>
     
    <script type="text/javascript">
        $(document).ready(function () {
            //綁定datagrid
            $('#tbDevice').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: '../../ASHX/DMC/DeviceManage.ashx?M=Search',
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
                           { field: 'deviceid', title: '设备编号', width: 70, align: 'left' },
                           { field: 'devicename', title: '设备名称', width: 50, align: 'left' },
                           { field: 'categorytext', title: '设备类别', width: 150, align: 'left' },
                           { field: 'placement', title: '设备位置', width: 50, align: 'left' },
                           { field: 'keepuserid', title: '保养人', width: 70, align: 'left' },
                           {
                               field: 'opt', title: '操作', width: 70, align: 'left',
                               formatter: function (value, row, index) {
                                   return '<a href="#" onclick="View(\'' + index + '\')">查看</a> | <a href="#" onclick="Edit(\'' + index + '\')">编辑</a>  | <a href="#" onclick="Delete(\'' + index + '\')">删除</a> | <a href="#" onclick="QCCode(\'' + index + '\')">二维码</a>';
                               }
                           }
                ]]
            });
            $(window).resize(function () {
                $('#tbDevice').datagrid('resize');
            });
            //設置分页控件屬性 
            var p = $('#tbDevice').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,
                pageList: [10, 15, 20, 25, 30],
                beforePageText: '第',
                afterPageText: '页 共{pages}页',
                displayMsg: '当前显示 {from} - {to} 条记录 共 {total} 条记录'
            });
        });
        //Grid初使加載的數據
        function Search(SearchType) {
            var queryParams = $('#tbDevice').datagrid('options').queryParams;
            //請輸入關鍵字            
            queryParams.KeyWord = $('#txtKeyword').textbox("getValue");
            queryParams.SearchType = SearchType;
            queryParams.DeviceId = $("#qdeviceid").textbox("getValue");
            queryParams.Usey = $('#qusey').combobox("getValue");
            $('#tbDevice').datagrid('reload');
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

        //取消(關閉dialog)
        function closeWindow(id) {
            $('#' + id).dialog('close')
        }

        var url = "../../ASHX/DMC/DeviceManage.ashx?M=NewDevice";
        //编辑
        function Edit(index) {
            url = "../../ASHX/DMC/DeviceManage.ashx?M=UpdateDevice";
            $('#fm').form('clear');
            var row = $('#tbDevice').datagrid('getData').rows[index];
            $('#fm').form('load', row);
            //$('#acategoryid').combotree("setValue", row.categoryid);
            $("#adeviceid").textbox("disable");
            $('#divNew').dialog('open').dialog('setTitle', '修改设备');
        }

        //查看
        function View(index) {
            $('#fmView').form('clear');
            var row = $('#tbDevice').datagrid('getData').rows[index];
            $('#fmView').form('load', row);
            //$('#vcategoryid').combotree("setValue", row.categoryid);
            $('#divView').dialog('open').dialog('setTitle', '设备详情');
        }
        function Delete(index) {
            var row = $('#tbDevice').datagrid('getData').rows[index];
            $.messager.confirm("删除确认", "确定需要删除设备【" + row.deviceid + "】吗？", function (r) {
                if (r) {
                    $.post("../../ASHX/DMC/DeviceManage.ashx?M=DeleteDevice",
                        {
                            CategoryId: row.deviceid,
                        },
                        function (result) {
                            if (result.success) {
                                $('#tbDevice').datagrid('reload');
                                $.messager.alert({ title: '成功提示', msg: '数据已删除成功' });
                                $('#divNew').dialog('close');
                            } else {
                                $.messager.alert({
                                    title: '错误提示',
                                    msg: result.msg
                                });
                            }
                        },
                        'json');
                }
            });
        }
        function Save() {
            var msg = "";
            //获取当前combotree的tree对象
            var tree = $('#acategoryid').combotree('tree');
            //获取当前选中的节点
            var node = tree.tree('getSelected');
            //必输项检查
            //1.检查编号，分类和保养人是否设置
            if ($("#adeviceid").textbox("getValue") == "") {
                msg = "请输入设备编号";
                $.messager.alert({ title: '错误提示', msg: msg });
                $("#adeviceid").textbox("textbox").focus();
                return;
            } else if ($("#akeepuserid").textbox("getValue") == "") {
                msg = "请设置保养人信息";
                $.messager.alert({ title: '错误提示', msg: msg });
                $("#akeepuserid").textbox("textbox").focus();
                return;
            } else if (node == null) {
                msg = "请选择设备编号";
                $.messager.alert({ title: '错误提示', msg: msg });
                $("#acategoryid").combotree('tree').focus();
                return;
            }


            if (msg == "") {
                //数据提交
                $.post(url, {
                    DeviceId: $("#adeviceid").textbox("getValue"),
                    DeviceName: node.text,
                    CategoryId: node.id,
                    CategoryText: node.attributes.categorytext,
                    KeepUserId: $("#akeepuserid").textbox("getValue"),
                    Placement: $("#aplacement").textbox("getValue"),
                    Remark: $("#aremark").textbox("getValue"),
                    Usey: $("#ausey").combobox("getValue")
                },
                function (result) {
                    if (result.success) {
                        $('#tbDevice').datagrid('reload');
                        $.messager.alert({ title: '成功提示', msg: '數據已保存成功' });
                        $('#divNew').dialog('close');
                    } else {
                        $.messager.alert({
                            title: '错误提示',
                            msg: result.msg
                        });
                    }
                },
                'json');
            } else {
                msg = "必输项未设置完整";
                $.messager.alert({ title: '错误提示', msg: msg });
            }
        }

        function add() {
            url = "../../ASHX/DMC/DeviceManage.ashx?M=NewDevice";
            $('#fm').form('clear');
            $("#ausy").combobox("setValue", "Y");
            $("#adeviceid").textbox("enable");
            $('#divNew').dialog('open').dialog('setTitle', '新增设备');
        }

        var qrcode = null;
        function QCCode(index) {
            if (qrcode == null) {
                qrcode = new QRCode(document.getElementById("qrcode"), {
                    width: 200,
                    height: 200
                });
            }
            var row = $('#tbDevice').datagrid('getData').rows[index];
            qrcode.makeCode(row.deviceid);
            $('#divQCCode').dialog('open').dialog('setTitle', '设备二维码');
        }

        function qccodeprint() {
            $("#qrcode").find("img").jqprint({
                debug: false,
                importCSS: true,
                printContainer: true,
                operaSupport: false
            });
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
                <a class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" href="javascript:void(0)" onclick='add()'>新增</a> &nbsp;&nbsp;                
            </div>
        </div>
        <div style="clear: both">
        </div>
        <div class="squery" id="divSearch">
            <div class="sinquery">
                <table class="addCoyTB" style="height: 30px; border: 0px;">
                    <tr>
                        <td style="width: 70px;">设备信息:</td>
                        <td style="width: 195px;">
                            <input class="easyui-textbox" id="qdeviceid" name="deviceid" data-options="prompt:'请输入设备信息编号'" style="width: 150px;" />
                        </td>
                        <td style="width: 70px;">有效否:</td>
                        <td style="width: 195px;">
                            <select class="easyui-combobox" name="usey" id="qusey" style="width: 150px;" data-options="editable:false">
                                <option value="" selected="selected">All</option>
                                <option value="Y">Y-有效</option>
                                <option value="N">N-失效</option>
                            </select>
                        </td>
                        <td style="width: 70px;">设备状态:</td>
                        <td style="width: 195px;">
                            <select class="easyui-combobox" name="status" id="astatus" style="width: 150px;" data-options="editable:false">
                                <option value="" selected="selected">All</option>
                                <option value="0">0-停机</option>
                                <option value="1">1-运行</option>
                            </select>
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
        <table id="tbDevice" data-options="fit:false">
        </table>
    </div>

    <!--归属设置-->
    <div id="divNew" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:false,buttons:'#dlg-buttons'"
        style="width: 700px; height: 250px; padding: 5px;">
        <form id="fm" method="post" novalidate="novalidate">
            <table style="height: 100%; vertical-align: middle;">
                <colgroup>
                    <col span="1" style="width: 95px;" />
                    <col span="1" style="width: 220px" />
                    <col span="1" style="width: 95px;" />
                    <col span="1" style="width: 220px;" />
                </colgroup>
                <tbody>
                    <tr>
                        <td style="height: 25px;">设备编号:</td>
                        <td>
                            <input data-options="prompt:'请维护设备编号'" class="easyui-textbox" name="deviceid" id="adeviceid" style="width: 150px;" /><span style="color: red;">*</span>
                        </td>
                        <td>有效否:</td>
                        <td>
                            <select class="easyui-combobox" name="usey" id="ausey" style="width: 150px;" data-options="editable:false,pannlHeight:50">
                                <option value="Y" selected="selected">Y-有效</option>
                                <option value="N">N-无效</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 25px;">设备类别:</td>
                        <td>
                            <select id="acategoryid" name="categoryid" class="easyui-combotree" style="width: 150px;" data-options="url:'../../ASHX/DMC/DeviceCategory.ashx?M=GetDeviceTree',required:true">
                            </select><span style="color: red;">*</span>
                        </td>
                        <td style="height: 25px;">保养人:</td>
                        <td>
                            <input data-options="prompt:'请维护保养人'" class="easyui-textbox" name="keepuserid" id="akeepuserid" style="width: 150px;" /><span style="color: red;">*</span>
                        </td>
                    </tr>
                    <%--<tr>
                        <td style="height: 25px;">保养人：</td>
                        <td colspan="3">
                            <input data-options="multiline:true,prompt:'请输入保养人'" class="easyui-textbox" name="keepuser" id="akeepuser" style="width: 530px;" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td style="height: 25px;">摆放位置:</td>
                        <td colspan="3">
                            <input data-options="multiline:true,prompt:'请输入摆放位置'" class="easyui-textbox" name="placement" id="aplacement" style="width: 530px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 25px;">设备备注:</td>
                        <td colspan="3">
                            <input data-options="multiline:true,prompt:'如需备注,请填写'" class="easyui-textbox" id="aremark" name="remark" style="width: 530px;" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </form>
    </div>
    <div id="dlg-buttons">
        <a id="btnSave" class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="Save()">保存</a>
        <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel',plain:true" onclick="closeWindow('divNew')">关闭</a>
    </div>
    <div style="clear: both">
    </div>

    <!--归属设置-->
    <div id="divView" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:false,buttons:'#dlg-view'"
        style="width: 700px; height: 250px; padding: 5px;">
        <form id="fmView" method="post" novalidate="novalidate">
            <table style="height: 100%; vertical-align: middle;">
                <colgroup>
                    <col span="1" style="width: 95px;" />
                    <col span="1" style="width: 220px" />
                    <col span="1" style="width: 95px;" />
                    <col span="1" style="width: 220px;" />
                </colgroup>
                <tbody>
                    <tr>
                        <td style="height: 25px;">设备编号:</td>
                        <td>
                            <input disabled="disabled" data-options="prompt:'请维护设备编号'" class="easyui-textbox" name="deviceid" style="width: 150px;" /><span style="color: red;">*</span>
                        </td>
                        <td>有效否:</td>
                        <td>
                            <select disabled="disabled" class="easyui-combobox" name="usey" style="width: 150px;" data-options="editable:false,pannlHeight:50">
                                <option value="Y" selected="selected">Y-有效</option>
                                <option value="N">N-无效</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 25px;">设备类别:</td>
                        <td>
                            <select disabled="disabled" id="vcategoryid" name="categoryid" class="easyui-combotree" style="width: 150px;" data-options="url:'../../ASHX/DMC/DeviceCategory.ashx?M=GetDeviceTree',required:true">
                            </select><span style="color: red;">*</span>
                        </td>
                        <td style="height: 25px;">保养人:</td>
                        <td>
                            <input disabled="disabled" data-options="prompt:'请维护保养人'" class="easyui-textbox" name="keepuserid" style="width: 150px;" /><span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 25px;">摆放位置:</td>
                        <td colspan="3">
                            <input disabled="disabled" data-options="multiline:true,prompt:'请输入摆放位置'" class="easyui-textbox" name="placement" style="width: 530px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 25px;">设备备注:</td>
                        <td colspan="3">
                            <input disabled="disabled" data-options="multiline:true,prompt:'如需备注,请填写'" class="easyui-textbox" name="remark" style="width: 530px;" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </form>
    </div>
    <div id="dlg-view">
        <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel',plain:true" onclick="closeWindow('divView')">关闭</a>
    </div>
    <div style="clear: both">
    </div>

    <div id="divQCCode" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:false,buttons:'#dlg-QCCode'"
        style="width: 220px; height: 290px; padding: 2px;">
        <div id="qrcode" style="width: 200px; height: 200px;"></div>
    </div>
    <div id="dlg-QCCode">
        <a class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="qccodeprint()">打印</a>
        <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel',plain:true" onclick="closeWindow('divQCCode')">关闭</a>
    </div>
</body>
</html>
