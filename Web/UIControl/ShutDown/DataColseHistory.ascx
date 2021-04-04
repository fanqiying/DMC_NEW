<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataColseHistory.ascx.cs" Inherits="UIControl_ShutDown_DataColseHistory" %>
<div id="DataColseHistory" class="easyui-window" data-options="closed:true,modal:true,title:'<%= Language("hisInfo")%>',iconCls:'icon-save'"
        style="width: 600px; height: 220px; padding: 5px;">
        <div data-options="region:'center',border:false" style="padding: 10px; background: #fff;
            border: 1px solid #ccc;">
            <table width="100%" id="tbDataColse">
            </table>
        </div>
    </div>