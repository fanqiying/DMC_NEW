<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dialogProgramSetting.ascx.cs"
    Inherits="UIControl_Permission_UserBaseInfo_dialogProgramSetting" %>
<script type="text/javascript">
    var PAList = [];
    var PDList = [];
    var tempList = [];
    var pernode = null;
    var selectNode = null;
    function OpenProgramSetting() {
        if ($('#usuallyInfo').hasClass("tbView2")) {
            $.messager.alert('<%=Language("errTips")%>', '<%=Language("userpernotsave")%>'); //userpernotsave 用户的个人程式权限需要先保存资料
            return;
        }
        PAList = [];
        PDList = [];
        pernode = null;
        $("#tbAction").hide();
        $('#divData').hide();
        $('#ProgramStatus').tree({
            url: '../../ASHX/Permission/TreeJson.ashx?M=myprogramsetting&IsUser=Y&UB=' + $('#userIdInfo').val(),
            onBeforeSelect: function (node) {
                //記錄資料權限
                if (pernode && pernode.attributes) {
                    var tempDataList = [];
                    var rowDataList = [];
                    $.each(PDList, function (index, value) {
                        if (value.toString().indexOf(pernode.id.toString()) < 0)
                            tempDataList.push(value);
                    });

                    var chks = $('input[name="IsDataUsy"]');
                    var drow = $('#tbData').datagrid('getData').rows;
                    if (chks != null && chks.length > 0) {
                        for (var i = 0; i < chks.length; i++) {
                            if (chks[i].checked) {
                                var deptId = chks[i].id.replace("IsDataUsy", "");
                                var IsAll = 'N';
                                if ($('input[id="IsAll' + deptId + '"]')[0].checked) {
                                    IsAll = 'Y';
                                }
                                rowDataList.push(deptId + '|' + IsAll);
                                if (drow[i].deptID != drow[i].falseDeptID) {
                                    rowDataList.push(drow[i].falseDeptID + '|' + IsAll);
                                }
                            }
                        }
                        tempDataList.push(pernode.id + ':' + rowDataList.toString().replace(/,/g, ';'));
                    }
                    else {
                        tempDataList.push(pernode.id + ':');
                    }
                    PDList = tempDataList;
                    tempDataList = [];
                }
            },
            onSelect: function (node) {
                selectNode = null;
                pernode = node;
                if (node.attributes) {
                    $("#tbAction").show();
                    LoadAction(node.id);
                    InitData(node.id);
                    PDataSearch(node.id);
                    $('#divData').show();
                    //DataSearch(node.id);
                    selectNode = node;
                }
                else {
                    $("#tbAction").hide();
                    $('#divData').hide();
                }
            },
            onCheck: function (node, isChecked) {
                if (selectNode != null && selectNode.id == node.id) {
                    var chk = $('input[name="actionchk"]');
                    for (var j = 0; j < chk.length; j++) {
                        chk[j].checked = isChecked;
                    }
                    UpdatePAList();
                }
            }
        });
        $('#ProgramStatus').tree("reload");
        $('#ProgramSetting').dialog('open').dialog('setTitle', '<%=Language("programperset")%>'); //programperset 程式權限設定
    }

    function InitData(ProgramId) {
        currentPData = null;
        var tempStr = '';
        for (var i = 0; i < PDList.length; i++) {
            if (PDList[i].indexOf(ProgramId) > -1) {
                tempStr = PDList[i];
                currentPData = [];
                break;
            }
        }
        var tt = tempStr.split(":");
        if (tt.length == 2) {
            currentPData = tt[1].split(";");
        }
    }


    function LoadAction(ProgramId) {
        $.post('../../ASHX/Permission/UserRightManage.ashx?M=getright&RightType=programaction',
                   { UB: $('#userIdInfo').val(), ProgramId: ProgramId },
                   function (result) {
                       SetAction(result);
                       InitTempResult(ProgramId);
                   }, 'json');
    }

    function InitTempResult(programId) {
        for (var i = 0; i < PAList.length; i++) {
            if (PAList[i].indexOf(programId) > -1) {
                var chk = $('input[name="actionchk"]');
                var selectChk = PAList[i].toString().split("|");
                var action = selectChk[1];
                var actionList = action.split(";");
                for (var j = 0; j < chk.length; j++) {
                    chk[j].checked = false;
                    for (var h = 0; h < actionList.length; h++) {
                        if (actionList[h] == chk[j].id) {
                            chk[j].checked = true;
                            break;
                        }
                    }
                }
            }
        }
    }
    //動態生成Action表格
    function SetAction(actionList) {
        $("#tbAction tr").attr("align", "center");
        var _len = $("#tbAction tr").length;
        //alert(_len);
        for (var i = 2; i <= _len; i++) {
            $("tr[id='actiontr" + i + "']").remove(); //删除行
        }
        if (actionList && actionList.rows) {
            var rowMod = actionList.rows.length % 2;
            var rowLen = parseInt(actionList.rows.length / 2);
            if (rowMod > 0) rowLen += 1;
            for (var i = 2; i <= rowLen + 1; i++) {
                var trcontent = "<tr id='actiontr" + i + "' align='center' style='height: 26px;'>";
                trcontent += "<td style='width:50%'>";
                if (actionList.rows[(i - 2) * 2 + 0]) {
                    trcontent += "<input type='checkbox' name='actionchk' onclick='UpdatePAList()' ";
                    if (actionList.rows[(i - 2) * 2 + 0].Usy == "Y")
                        trcontent += " checked ";
                    trcontent += "id='" + actionList.rows[(i - 2) * 2 + 0].ActionId + "'>";
                    trcontent += actionList.rows[(i - 2) * 2 + 0].ActionName;
                    trcontent += "</input>";
                }
                trcontent += "</td>";
                trcontent += "<td style='width:50%'>";
                if (actionList.rows[(i - 2) * 2 + 1]) {
                    trcontent += "<input type='checkbox' name='actionchk' onclick='UpdatePAList()' ";
                    if (actionList.rows[(i - 2) * 2 + 1].Usy == "Y")
                        trcontent += " checked ";
                    trcontent += "id='" + actionList.rows[(i - 2) * 2 + 1].ActionId + "'>";
                    trcontent += actionList.rows[(i - 2) * 2 + 1].ActionName;
                    trcontent += "</input>";
                }
                trcontent += "</td>";
                trcontent += "</tr>";
                $("#tbAction").append(trcontent);
            }
        }
    }

    function CheckAllAction(all) {
        var chk = $('input[name="actionchk"]');
        for (var i = 0; i < chk.length; i++)
            chk[i].checked = all.checked;
        UpdatePAList();
    }
    //獲取表格中的CheckBox中的值
    function GetActionIds() {
        var actionIdList = [];
        var chk = $('input[name="actionchk"]');
        for (var i = 0; i < chk.length; i++) {
            if (chk[i].checked)
                actionIdList.push(chk[i].id);
        }
        return actionIdList.toString().replace(/,/g, ";");
    }

    function UpdatePAList() {
        var node = $('#ProgramStatus').tree("getSelected");
        if (node && node.attributes) {
            $.each(PAList, function (index, value) {
                if (value.toString().indexOf(node.id.toString()) < 0)
                    tempList.push(value);
            });
            tempList.push(node.id + "|" + GetActionIds());
            PAList = tempList;
            tempList = [];
        }
    }

    function SaveProgramSetting() {
        var list = PAList.toString();
        var programId = [];
        var node = $('#ProgramStatus').tree("getChecked");
        for (var i = 0; i < node.length; i++) {
            if (node[i] && node[i].attributes) {
                programId.push(node[i].id);
            }
        }
        var programs = programId.toString();

        var tempDataList = [];
        var rowDataList = [];
        $.each(PDList, function (index, value) {
            if (value.toString().indexOf(pernode.id.toString()) < 0)
                tempDataList.push(value);
        });
        var chks = $('input[name="IsDataUsy"]');
        var drow = $('#tbData').datagrid('getData').rows;
        if (chks != null && chks.length > 0) {
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].checked) {
                    var deptId = chks[i].id.replace("IsDataUsy", "");
                    var IsAll = 'N';
                    if ($('input[id="IsAll' + deptId + '"]')[0].checked) {
                        IsAll = 'Y';
                    }
                    rowDataList.push(deptId + '|' + IsAll);
                    if (drow[i].deptID != drow[i].falseDeptID) {
                        rowDataList.push(drow[i].falseDeptID + '|' + IsAll);
                    }
                }
            }
            tempDataList.push(pernode.id + ':' + rowDataList.toString().replace(/,/g, ';'));
        }
        else if (pernode) {
            tempDataList.push(pernode.id + ':');
        }
        PDList = tempDataList;
        tempDataList = [];
        pernode = null;
        var datas = PDList.toString();
        PDList = [];

        $.post('../../ASHX/Permission/UserRightManage.ashx?M=saveprogram',
                   { UB: $('#userIdInfo').val(), PAList: list, Programs: programs, PData: datas },
                   function (result) {
                       $('#tbProgram').datagrid('reload');
                       $('#ProgramSetting').dialog('close');
                   }, 'json');
    }
</script>
<div id="ProgramSetting" class="easyui-dialog" data-options="closed:true,modal:true"
    style="width: 780px; height: 400px; padding: 5px;" buttons="#data-buttons1">
    <table border="0" cellpadding="0" cellspacing="1" bgcolor="#a8c7ce" style="width: 100%;
        height: 95%;">
        <tr>
            <td width="300px" height="20" bgcolor="#E0ECFF" style="text-align: center;">
                <%=Language("ProgramRight")%>
            </td>
            <td width="200px" height="20" bgcolor="#E0ECFF" style="text-align: center;">
                <%=Language("ActionRight")%>
            </td>
            <td width="280px" height="20" bgcolor="#E0ECFF" style="text-align: center;">
                <%=Language("datepermin")%>
                <%--datepermin 資料權限--%>
            </td>
        </tr>
        <tr>
            <td style="background: #fff; height: 290px; vertical-align: top;">
                <div style="height: 290px; overflow: auto;">
                    <ul id="ProgramStatus" data-options="animate:true,dnd:false,checkbox:true">
                    </ul>
                </div>
            </td>
            <td style="background: #fff; padding: 3px; vertical-align: top;">
                <table id="tbAction" class="tbStyle">
                    <tr style="height: 26px;" id="actiontr1">
                        <td colspan="3" bgcolor="#E0ECFF">
                            <input type="checkbox" id="actionchkall" onclick="CheckAllAction(this)"><%=Language("CheckAll")%></input>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="background: #fff; padding: 3px; vertical-align: top;">
                <div id="divData" style="height: 290px; overflow: auto;">
                    <table id="tbData" width="98%" fit="false">
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
<div id="data-buttons1">
    <a class="easyui-linkbutton" href="javascript:void(0)" onclick="SaveProgramSetting()">
        <%=Language("Save")%></a> <a class="easyui-linkbutton" href="javascript:void(0)"
            onclick="closeProgramSetting()">
            <%=Language("Cancel")%></a>
</div>
