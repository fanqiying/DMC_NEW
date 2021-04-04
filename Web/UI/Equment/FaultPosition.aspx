<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FaultPosition.aspx.cs" Inherits="Web.UI.Equment.FaultPosition" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>故障位置维护</title>
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.json-2.4.js"></script>
    <script src="../../easyUI15/locale/easyui-lang-zh_TW.js"></script>
    <script type="text/javascript">
        var PPositionId = null;
        var PPositionText = null;
        var editfatherid = null;
        $(document).ready(function () {
            //实例化树形菜单
            $("#tree").tree({
                lines: true,
                onSelect: function (node) {
                    //增加判断，不能设置父级为自己及其子节点
                    debugger;
                    if (node.id != "" && node.id == $("#apositionid").textbox("getValue") ||
                        (node.id != "" && node.id != $("#apositionid").textbox("getValue") && node.attributes.positiontext.indexOf('->' + $("#apositionname").textbox("getValue") + '->') > -1)) {
                        if (editfatherid != null) {
                            var node = $('#tree').tree('find', editfatherid);
                            $('#tree').tree('select', node.target);
                        }
                        $.messager.alert({ title: '选择错误', msg: '请不要选择自己及其子节点' });
                        return;
                    } else {
                        PPositionId = node.id;
                        PPositionText = node.attributes.positiontext;
                    }
                },
                onLoadSuccess: function (node, data) {
                    if (editfatherid != null) {
                        var node = $('#tree').tree('find', editfatherid);
                        PPositionId = node.id;
                        PPositionText = node.attributes.Positiontext;
                        $('#tree').tree('select', node.target);
                    }
                }
            });
            //綁定datagrid
            $('#tbFaultPosition').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: '../../ASHX/DMC/FaultPosition.ashx?M=Search',
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
                           { field: 'positionid', title: '位置编号', width: 50, align: 'left' },
                           { field: 'positionname', title: '位置名称', width: 50, align: 'left' },
                           { field: 'gradetime', title: '评分时间(分钟)', width: 60, align: 'left' },
                           { field: 'grade', title: '评分分数(分)', width: 60, align: 'left' },
                           { field: 'orderid', title: '排序序号', width: 50, align: 'left' },
                           { field: 'ppositionid', title: '父级位置', width: 50, align: 'left' },
                           { field: 'usey', title: '有效否', width: 50, align: 'left' },
                           {
                               field: 'opt', title: '操作', width: 50, align: 'left',
                               formatter: function (value, row, index) {
                                   return '<a href="#" onclick="View(\'' + index + '\')">查看</a> | <a href="#" onclick="Edit(\'' + index + '\')">编辑</a>';
                               }
                           }
                ]]
            });
            $(window).resize(function () {
                $('#tbFaultPosition').datagrid('resize');
            });
            //設置分页控件屬性 
            var p = $('#tbFaultPosition').datagrid('getPager');
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
            var queryParams = $('#tbFaultPosition').datagrid('options').queryParams;
            //請輸入關鍵字            
            queryParams.KeyWord = $('#txtKeyword').textbox("getValue");
            queryParams.SearchType = SearchType;
            queryParams.PositionName = $("#qPositionName").textbox("getValue");
            queryParams.Usey = $('#qUsey').combobox("getValue");
            $('#tbFaultPosition').datagrid('reload');
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

        var url = "../../ASHX/DMC/FaultPosition.ashx?M=NewFaultPosition";
        //归属设置
        function Edit(index) {
            $("#btnSave").show();
            url = "../../ASHX/DMC/FaultPosition.ashx?M=UpdateFaultPosition";
            $('#fm').form('clear');
            var row = $('#tbFaultPosition').datagrid('getData').rows[index];
            $('#fm').form('load', row);
            PPositionId = null;
            PPositionText = null;
            editfatherid = row.ppositionid;
            LoadTree();
            $('#divNew').dialog('open').dialog('setTitle', '修改故障位置');
        }
        //设备详情
        function View(index) {
            $("#btnSave").hide();
            $('#fm').form('clear');
            var row = $('#tbFaultPosition').datagrid('getData').rows[index];
            $('#fm').form('load', row);
            PPositionId = null;
            PPositionText = null;
            editfatherid = row.pPositionid;
            LoadTree();
            $('#divNew').dialog('open').dialog('setTitle', '故障位置详情');
        }

        function Save() {
            var isValid = $("#fm").form('validate') && $("#fmplan").form('validate');
            var msg = "";
            if (isValid) {
                //数据提交
                $.post(url, {
                    PositionId: $("#apositionid").textbox("getValue"),
                    PositionName: $("#apositionname").textbox("getValue"),
                    OrderId: $("#aorderid").numberbox("getValue"),
                    Usey: $("#ausey").combobox("getValue"),
                    PPositionId: PPositionId,
                    PPositionText: PPositionText,
                    GradeTime: $("#agradetime").numberbox("getValue"),
                    Grade: $("#agrade").numberbox("getValue")
                },
                function (result) {
                    if (result.success) {
                        $('#tbFaultPosition').datagrid('reload');
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
        function LoadTree() {
            $('#tree').tree('options').url = "../../ASHX/DMC/FaultPosition.ashx?M=GetFaultPositionTree";
            $('#tree').tree('reload');
        }
        function add() {
            $("#btnSave").show();
            url = "../../ASHX/DMC/FaultPosition.ashx?M=NewFaultPosition";
            $('#fm').form('clear');
            PPositionId = null;
            PPositionText = null;
            editfatherid = null;
            LoadTree();
            $("#ausey").combobox("setValue", "Y");
            $('#divNew').dialog('open').dialog('setTitle', '新增故障位置');
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
                        <td style="width: 70px;">位置名称:</td>
                        <td style="width: 195px;">
                            <input class="easyui-textbox" id="qPositionName" data-options="prompt:'请输入位置名称'" style="width: 150px;" />
                        </td>
                        <td style="width: 70px;">有效否:</td>
                        <td>
                            <select class="easyui-combobox" id="qUsey" style="width: 150px; height: 40px;" data-options="editable:false">
                                <option value="" selected="selected">All</option>
                                <option value="Y">Y-有效</option>
                                <option value="N">N-失效</option>
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
        <table id="tbFaultPosition" data-options="fit:false">
        </table>
    </div>

    <div id="divNew" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:false,buttons:'#dlg-button'"
        style="width: 620px; height: 310px; padding: 3px;">
        <div class="easyui-layout" data-options="fit:true,border:false">
            <div data-options="region:'west',split:true,title:'父级设置',collapsible:false" style="width: 30%;">
                <ul id="tree" class="easyui-tree" data-options="animate:true,dnd:false,fit:true">
                </ul>
            </div>
            <div data-options="region:'center',split:true,title:'故障位置维护',collapsible:false" style="width: 70%;">
                <form id="fm">
                    <table style="width: 100%; height: 90%;">
                        <tr>
                            <td style="height: 25px; text-align: right;">位置编号：
                            </td>
                            <td>
                                <input type="text" id="apositionid" name="positionid" class="easyui-textbox" data-option="required:true" /><span style="color: red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 25px; text-align: right;">位置名称：
                            </td>
                            <td>
                                <input type="text" id="apositionname" name="positionname" class="easyui-textbox" data-option="required:true" /><span style="color: red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 25px; text-align: right;">排序序号：
                            </td>
                            <td>
                                <input type="text" name="orderid" class="easyui-numberbox" data-option="required:true" id="aorderid" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 25px; text-align: right;">有效否：
                            </td>
                            <td>
                                <select class="easyui-combobox" style="width: 100px; font-size: 12px;" name="usey" id="ausey">
                                    <option value="Y" selected="selected">Y</option>
                                    <option value="N">N</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 25px; text-align: right;">评分时间：
                            </td>
                            <td>
                                <input type="text" name="gradetime" id="agradetime" class="easyui-numberbox" data-option="required:true" />分钟
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 25px; text-align: right;">评分分数：
                            </td>
                            <td>
                                <input type="text" name="grade" id="agrade" class="easyui-numberbox" data-option="required:true" />分
                            </td>
                        </tr>
                    </table>
                </form>
            </div>
        </div>
    </div>
    <div id="dlg-button">
        <a id="btnSave" class="easyui-linkbutton" href="javascript:void(0)" onclick="Save()">保存</a>
        <a class="easyui-linkbutton" href="javascript:void(0)" onclick="closeWindow('divNew');">关闭</a>
    </div>
</body>
</html>
