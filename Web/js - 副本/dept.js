function GetDeptGroup() {
    var newGroup = "";
    var index = 0;
    var nature = $("#dept_nature").combobox("getValue");
    if (nature == 0) {
        newGroup = "";
    }
    else if (nature < 5) {
        index = parseInt(nature) + 1;
        newGroup = $("#dept_nature").combobox("getData")[index].displaytext;

    }
    else if (nature == 5) {
        newGroup = ($("#dept_nature ").combobox("getData")[5].displaytext);
    }

    $("#hideGroup").val(index)

}


function delMuti(index) {
    var ids = [];
    if (index) {
        var row = $('#tbDept').datagrid('getData').rows[index];
        ids.push(row.deptid);
    }
    else {
        var rows = $('#tbDept').datagrid('getChecked');
        for (var i = 0; i < rows.length; i++) {
            //每行ID放入數組中
            ids.push(rows[i].deptid);
        }

    }
    if (ids.length > 0) {
        //必須為string類型，否則傳輸不過去 
        var idlist = ids.toString();
        $.messager.defaults = { ok: "是", cancel: "否" };
        checkDeldept(idlist);
    }

    else {
        $.messager.alert('刪除數據', '請選擇需要刪除的數據');
    }
}

function checkDeldept(idlist) {
    $.post('../../ASHX/Organize/DelCheck.ashx', { deptID: idlist },
                         function (result) {
                             try {
                                 if (result == "True") {
                                     delStatStr = "部門中包含員工數據，確定要刪除嗎？";
                                     tt(idlist);
                                 } else {
                                     delStatStr = "確定要刪除嗎？";
                                     tt(idlist);
                                 }
                             }
                             catch (e) {
                                 $.messager.alert({
                                     title: '異常提示',
                                     msg: '提交數據異常，請確認網絡連接狀況'
                                 });

                             }
                         }, 'text').error(function () {
                             $.messager.alert({
                                 title: res.ExceptionTips,
                                 msg: res.SubmitDataException
                             });
                         });
}

function tt(idlist) {
    $.messager.confirm('刪除確認', delStatStr, function (r) {
        if (r) {
            $.post('../../ASHX/Organize/DeptManage.ashx?M=delete', { deptIDstr: idlist },
                         function (result) {
                             try {
                                 if (result.success) {
                                     $.messager.defaults = { ok: "確定" };
                                     $.messager.alert("成功提示", "數據已刪除");
                                     $('#tbDept').datagrid('reload'); // reload the user data
                                 } else {
                                     $.messager.alert({	// show error message
                                         title: 'Error',
                                         msg: result.msg
                                     });
                                 }
                             }
                             catch (e) {
                                 $.messager.alert({
                                     title: '異常提示',
                                     msg: '提交數據異常，請確認網絡連接狀況'
                                 });

                             }
                         }, 'json').error(function () {
                             $.messager.alert({
                                 title: res.ExceptionTips,
                                 msg: res.SubmitDataException
                             });
                         });
        }
    });
}
var url = "";
var delStatStr = '';

function addDept(index) {
    try {
        $('#addNewDiv').dialog({
            title: '新增部門資料維護'
        });
        $('#addNewDiv').dialog('open');
        $('#fm').form('clear');
        $('#deptid').removeAttr("readonly");
        //$('#deptid').removeAttr("style");
        $("#dept_usy").combobox("setValue", "Y");
        $("#dept_nature").combobox("setValue", '0');

        url = '../../ASHX/Organize/DeptManage.ashx?M=add';
    }
    catch (e) {
        $.messager.alert({
            title: '異常提示',
            msg: '數據異常，請確認網絡連接狀況'

        });
    }
}

function viewDept(index) {

    var row = $('#tbDept').datagrid('getData').rows[index];
    if (row) {
        try {
            $('#fmView').form('load', row);
            LoadTelInfo();
            $('#vdeptnature').val(ReadText(row.deptnature));
            $('#divDeptDetail').dialog('open');
        }
        catch (e) {
            $.messager.alert({
                title: '異常提示',
                msg: '網絡異常，請確認網絡連接狀況'
            });
        }
    }

}

function ReadText(id) {
    if (id == "1") {
        return "組"
    }
    else if (id == "2") {
        return "課";
    } else if (id == "3") {
        return "部";
    } else if (id == "4") {
        return "處";
    }
    else if (id == "5") {
        return "群";
    }
}


function editDept(index) {
    var row = $('#tbDept').datagrid('getData').rows[index];
    $("#deptid").attr("readOnly", "readOnly");
    //$("#deptid").css("background-color", "#e1e1e1");
    if (row) {
        try {
            $('#tbDept').attr("readonly", "readonly");
            $('#fm').form('load', row);
            $('#addNewDiv').dialog({
                title: '編輯部門資料維護'
            });
            $('#addNewDiv').dialog('open');
            url = '../../ASHX/Organize/DeptManage.ashx?M=update';
        }
        catch (e) {
            $.messager.alert({
                title: '異常提示',
                msg: '網絡異常，請確認網絡連接狀況'
            });
        }
    }
}

function saveDept() {
    GetDeptGroup();
    var tagGroup = $('#hideGroup').val();
    var selectGroup = $('#hideSelectGroup').val();


    if (tagGroup != "" && selectGroup != "") {
        if (tagGroup != selectGroup) {
            $.messager.defaults = { ok: "是", cancel: "否" };
            $.messager.confirm('Confirm', '非正常部門歸屬，確認保存嗎?', function (r) {
                if (r) {
                    doSave();
                }
            });

        }
        else {
            doSave();
        }
    }
    else {
        doSave();
    }

}


function doSave() {
    $('#fm').form('submit', {
        url: url + "&dd=" + document.getElementById("hideGroup").value,
        onSubmit: function () {

            var error = '';
            if ($.trim($('#deptid').val()) == '') {
                error = '部門編號不能為空';
                $("#deptid").focus();

            }
            else if (!$('#deptid').val().match(/^\w+$/)) {
                error = '部門編號只能是英文，數字或者下劃綫！';
                $("#deptid").focus();

            }

            else if ($.trim($('#simplename').val()) == '') {
                error = '部門簡稱不能為空';
                $("#simplename").focus();
            }

            if (error != '') {
                $.messager.defaults = { ok: "確定" };
                $.messager.alert('溫馨提示', error);
                return false;
            }
            return $(this).form('validate');

        },
        success: function (result) {
            try {
                var result = eval('(' + result + ')');
                if (result.success) {
                    $.messager.defaults = { ok: "確定" };
                    $.messager.alert('成功提示', '數據已保存成功');
                    $('#addNewDiv').dialog('close'); 	// close the dialog
                    $('#tbDept').datagrid('reload'); // reload the user data
                } else {
                    $.messager.alert({
                        title: 'Error',
                        msg: result.msg
                    });
                }
            }
            catch (e) {
                $.messager.alert({
                    title: '異常提示',
                    msg: '提交數據異常，請確認網絡連接狀況'
                });
            }
        }
    });
}




function GetUserList(op) {

    autoTips({ input: op, keywords: op.value });
}

function GetDeptIDListByKey(op) {
    $('#hideSelectGroup').val('');
    Tips({ input: op, keyword: op.value });
}


function GetFlseID() {
    $("#falseDeptID").val($('#deptid').val());
}