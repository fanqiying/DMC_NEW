<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FaultPhenomena.aspx.cs" Inherits="Web.UI.Equment.FaultPhenomena" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>故障现象维护</title>
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.json-2.4.js"></script>
    <script src="../../easyUI15/locale/easyui-lang-zh_TW.js"></script>
    <script type="text/javascript">
        var PCategoryId = null;
        var PCategoryText = null;
        var editfatherid = null;
        $(document).ready(function () {
            //实例化树形菜单
            $("#tree").tree({
                lines: true,
                onSelect: function (node) {
                    //增加判断，不能设置父级为自己及其子节点
                    debugger;
                    if (node.id != "" && node.id == $("#acategoryid").textbox("getValue") ||
                        (node.id != "" && node.id != $("#acategoryid").textbox("getValue") && node.attributes.categorytext.indexOf('->' + $("#acategoryname").textbox("getValue") + '->') > -1)) {
                        if (editfatherid != null) {
                            var node = $('#tree').tree('find', editfatherid);
                            $('#tree').tree('select', node.target);
                        }
                        $.messager.alert({ title: '选择错误', msg: '请不要选择自己及其子节点' });
                        return;
                    } else {
                        PCategoryId = node.id;
                        PCategoryText = node.attributes.categorytext;
                    }
                },
                onLoadSuccess: function (node, data) {
                    if (editfatherid != null) {
                        var node = $('#tree').tree('find', editfatherid);
                        PCategoryId = node.id;
                        PCategoryText = node.attributes.categorytext;
                        $('#tree').tree('select', node.target);
                    }
                }
            });
            //綁定datagrid
            $('#tbFaultPhenomena').datagrid({
                //是否折?
                collapsible: true,
                checkOnSelect: false,
                selectOnCheck: false,
                url: '../../ASHX/DMC/FaultPhenomena.ashx?M=Search',
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
                           { field: 'categoryid', title: '现象编号', width: 50, align: 'left' },
                           { field: 'categoryname', title: '现象名称', width: 50, align: 'left' },
                           { field: 'orderid', title: '排序序号', width: 50, align: 'left' },
                           { field: 'pcategoryid', title: '父级现象', width: 50, align: 'left' },
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
                $('#tbFaultPhenomena').datagrid('resize');
            });
            //設置分页控件屬性 
            var p = $('#tbFaultPhenomena').datagrid('getPager');
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
            var queryParams = $('#tbFaultPhenomena').datagrid('options').queryParams;
            //請輸入關鍵字            
            queryParams.KeyWord = $('#txtKeyword').textbox("getValue");
            queryParams.SearchType = SearchType;
            queryParams.CategoryName = $("#qCategoryName").textbox("getValue");
            queryParams.Usey = $('#qUsey').combobox("getValue");
            $('#tbFaultPhenomena').datagrid('reload');
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

        var url = "../../ASHX/DMC/FaultPhenomena.ashx?M=NewFaultPhenomena";
        //归属设置
        function Edit(index) {
            $("#btnSave").show();
            url = "../../ASHX/DMC/FaultPhenomena.ashx?M=UpdateFaultPhenomena";
            $('#fm').form('clear');
            var row = $('#tbFaultPhenomena').datagrid('getData').rows[index];
            $('#fm').form('load', row);
            PCategoryId = null;
            PCategoryText = null;
            editfatherid = row.pcategoryid;
            LoadTree();
            $('#divNew').dialog('open').dialog('setTitle', '修改现象');
        }
        //设备详情
        function View(index) {
            $("#btnSave").hide();
            $('#fm').form('clear');
            var row = $('#tbFaultPhenomena').datagrid('getData').rows[index];
            $('#fm').form('load', row);
            PCategoryId = null;
            PCategoryText = null;
            editfatherid = row.pcategoryid;
            LoadTree();
            $('#divNew').dialog('open').dialog('setTitle', '现象详情');
        }

        function Save() {
            var isValid = $("#fm").form('validate') && $("#fmplan").form('validate');
            var msg = "";
            if (isValid) {
                //数据提交
                $.post(url, {
                    CategoryId: $("#acategoryid").textbox("getValue"),
                    CategoryName: $("#acategoryname").textbox("getValue"),
                    OrderId: $("#aorderid").numberbox("getValue"),
                    Usey: $("#ausey").combobox("getValue"),
                    PCategoryId: PCategoryId,
                    PCategoryText: PCategoryText
                },
                function (result) {
                    if (result.success) {
                        $('#tbFaultPhenomena').datagrid('reload');
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
            $('#tree').tree('options').url = "../../ASHX/DMC/FaultPhenomena.ashx?M=GetFaultPhenomenaTree";
            $('#tree').tree('reload');
        }
        function add() {
            $("#btnSave").show();
            url = "../../ASHX/DMC/FaultPhenomena.ashx?M=NewFaultPhenomena";
            $('#fm').form('clear');
            PCategoryId = null;
            PCategoryText = null;
            editfatherid = null;
            LoadTree();
            $("#ausey").combobox("setValue", "Y");
            $('#divNew').dialog('open').dialog('setTitle', '新增现象');
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
                        <td style="width: 70px;">现象名称:</td>
                        <td style="width: 195px;">
                            <input class="easyui-textbox" id="qCategoryName" data-options="prompt:'请输入现象名称'" style="width: 150px;" />
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
        <table id="tbFaultPhenomena" data-options="fit:false">
        </table>
    </div>

    <div id="divNew" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:false,buttons:'#dlg-button'"
        style="width: 620px; height: 310px; padding: 3px;">
        <div class="easyui-layout" data-options="fit:true,border:false">
            <div data-options="region:'west',split:true,title:'父级设置',collapsible:false" style="width: 30%;">
                <ul id="tree" class="easyui-tree" data-options="animate:true,dnd:false,fit:true">
                </ul>
            </div>
            <div data-options="region:'center',split:true,title:'现象明细维护',collapsible:false" style="width: 70%;">
                <form id="fm">
                    <table class="tbStyle" style="width: 100%; height: 90%;">
                        <tr>
                            <td>现象编号：
                            </td>
                            <td>
                                <input type="text" id="acategoryid" name="categoryid" class="easyui-textbox" data-option="required:true" /><span style="color: red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td>现象名称：
                            </td>
                            <td>
                                <input type="text" id="acategoryname" name="categoryname" class="easyui-textbox" data-option="required:true" /><span style="color: red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td>排序序号：
                            </td>
                            <td>
                                <input type="text" name="orderid" class="easyui-numberbox" data-option="required:true" id="aorderid" />
                            </td>
                        </tr>
                        <tr>
                            <td>有效否：
                            </td>
                            <td>
                                <select class="easyui-combobox" style="width: 100px; font-size: 12px;" name="usey" id="ausey">
                                    <option value="Y" selected="selected">Y</option>
                                    <option value="N">N</option>
                                </select>
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
