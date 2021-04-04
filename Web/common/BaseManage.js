

var Url
/*
    GridId:編輯的GridId；
    RowIndex：編輯的行號；
    FormId：綁定的FormId
    WindowsId:需要顯示的窗體
    Title：顯示標題名稱
    Url
*/
function Edit(GridId,RowIndex,FormId,WindowsId,Title,Url,NotEditInput) {
    $('#' + GridId).datagrid('selectRow', index);
    var row = $('#' + GridId).datagrid('getSelected');
    if (row) {
        $('#' + WindowsId).dialog('open').dialog('setTitle', Title);
        $('#' + FormId).form('load', row);
        $('#ResourceIdInfo').attr("readonly", "readonly");
        Url = Url + "M=update";
    }
}
