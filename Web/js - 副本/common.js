
//ajax操作

function openHttpRequest() {
    http_request = false;
    if (window.XMLHttpRequest) { // Mozilla, Safari,...
        http_request = new XMLHttpRequest();
    } else if (window.ActiveXObject) { // IE
        try {
            http_request = new ActiveXObject("Msxml2.XMLHTTP");
        } catch (e) {
            try {
                http_request = new ActiveXObject("Microsoft.XMLHTTP");
            } catch (e) { }
        }
    }
}
function dialogTransferNoTitle(id) {
    var default_left;
    var default_top;
    $('#' + id).dialog({
        modal: true,
        onOpen: function () {

            //dialog原始left
            default_left = $('#' + id).panel('options').left;
            //dialog原始top
            default_top = $('#' + id).panel('options').top;
        },
        onMove: function (left, top) {  //鼠标拖动时事件

            var body_width = document.body.offsetWidth; //body的宽度
            var body_height = document.body.offsetHeight; //body的高度
            if (left < 1 || left > (body_width - dd_width) || top < 1 || top > (body_height - dd_height)) {
                var dd_width = $('#' + id).panel('options').width; //dialog的宽度
                var dd_height = $('#' + id).panel('options').height; //dialog的高度				   if(left<1||left>(body_width-dd_width)||top<1||top>(body_height-dd_height)){
                $('#' + id).dialog('move', {
                    left: default_left,
                    top: default_top
                });
            }
        }
    });
}


var base64EncodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
var base64DecodeChars = new Array(
    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 62, -1, -1, -1, 63,
    52, 53, 54, 55, 56, 57, 58, 59, 60, 61, -1, -1, -1, -1, -1, -1,
    -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
    15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, -1, -1, -1, -1, -1,
    -1, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
    41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, -1, -1, -1, -1, -1);

function base64encode(str) {
    var out, i, len;
    var c1, c2, c3;

    len = str.length;
    i = 0;
    out = "";
    while (i < len) {
        c1 = str.charCodeAt(i++) & 0xff;
        if (i == len) {
            out += base64EncodeChars.charAt(c1 >> 2);
            out += base64EncodeChars.charAt((c1 & 0x3) << 4);
            out += "==";
            break;
        }
        c2 = str.charCodeAt(i++);
        if (i == len) {
            out += base64EncodeChars.charAt(c1 >> 2);
            out += base64EncodeChars.charAt(((c1 & 0x3) << 4) | ((c2 & 0xF0) >> 4));
            out += base64EncodeChars.charAt((c2 & 0xF) << 2);
            out += "=";
            break;
        }
        c3 = str.charCodeAt(i++);
        out += base64EncodeChars.charAt(c1 >> 2);
        out += base64EncodeChars.charAt(((c1 & 0x3) << 4) | ((c2 & 0xF0) >> 4));
        out += base64EncodeChars.charAt(((c2 & 0xF) << 2) | ((c3 & 0xC0) >> 6));
        out += base64EncodeChars.charAt(c3 & 0x3F);
    }
    return out;
}

function base64decode(str) {
    var c1, c2, c3, c4;
    var i, len, out;

    len = str.length;
    i = 0;
    out = "";
    while (i < len) {
        /* c1 */
        do {
            c1 = base64DecodeChars[str.charCodeAt(i++) & 0xff];
        } while (i < len && c1 == -1);
        if (c1 == -1)
            break;

        /* c2 */
        do {
            c2 = base64DecodeChars[str.charCodeAt(i++) & 0xff];
        } while (i < len && c2 == -1);
        if (c2 == -1)
            break;

        out += String.fromCharCode((c1 << 2) | ((c2 & 0x30) >> 4));

        /* c3 */
        do {
            c3 = str.charCodeAt(i++) & 0xff;
            if (c3 == 61)
                return out;
            c3 = base64DecodeChars[c3];
        } while (i < len && c3 == -1);
        if (c3 == -1)
            break;

        out += String.fromCharCode(((c2 & 0XF) << 4) | ((c3 & 0x3C) >> 2));

        /* c4 */
        do {
            c4 = str.charCodeAt(i++) & 0xff;
            if (c4 == 61)
                return out;
            c4 = base64DecodeChars[c4];
        } while (i < len && c4 == -1);
        if (c4 == -1)
            break;
        out += String.fromCharCode(((c3 & 0x03) << 6) | c4);
    }
    return out;
}

function utf16to8(str) {
    var out, i, len, c;

    out = "";
    len = str.length;
    for (i = 0; i < len; i++) {
        c = str.charCodeAt(i);
        if ((c >= 0x0001) && (c <= 0x007F)) {
            out += str.charAt(i);
        } else if (c > 0x07FF) {
            out += String.fromCharCode(0xE0 | ((c >> 12) & 0x0F));
            out += String.fromCharCode(0x80 | ((c >> 6) & 0x3F));
            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
        } else {
            out += String.fromCharCode(0xC0 | ((c >> 6) & 0x1F));
            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
        }
    }
    return out;
}

function utf8to16(str) {
    var out, i, len, c;
    var char2, char3;

    out = "";
    len = str.length;
    i = 0;
    while (i < len) {
        c = str.charCodeAt(i++);
        switch (c >> 4) {
            case 0: case 1: case 2: case 3: case 4: case 5: case 6: case 7:
                // 0xxxxxxx 
                out += str.charAt(i - 1);
                break;
            case 12: case 13:
                // 110x xxxx 10xx xxxx 
                char2 = str.charCodeAt(i++);
                out += String.fromCharCode(((c & 0x1F) << 6) | (char2 & 0x3F));
                break;
            case 14:
                // 1110 xxxx 10xx xxxx 10xx xxxx 
                char2 = str.charCodeAt(i++);
                char3 = str.charCodeAt(i++);
                out += String.fromCharCode(((c & 0x0F) << 12) |
                       ((char2 & 0x3F) << 6) |
                       ((char3 & 0x3F) << 0));
                break;
        }
    }

    return out;
}


function saveItem() {

    var id = base64encode("it-00001");

   // var colOne = $("#coloneID" + id).val();
    if (id != "") {
        openHttpRequest();
        if (!http_request) {
            alert('Error :( 创建组件失败，请联系系统管理员！ )');
            return false;
        }

        http_request.onreadystatechange = doSaveItem;
        http_request.open('GET', "../ajax/saveItemByID.aspx?&x=" + Math.random() + "&id=" + id, true);
        http_request.send(null);
    }
    else {

    }
}

function doSaveItem() {
    if (http_request.readyState == 4) {
        if (http_request.status == 200) {
            var strText = http_request.responseText;
            alert(strText);
            if (strText.substring(0, 3) != 'ERR') {
                var strArr = new Array();
                strArr = strText.split('|');
                var obj = $("#content" + strArr[1]);
                var modObj = $("#modContent" + strArr[1]);
                //var displayColObj = $("#disCol" + strArr[1]);
                //displayColObj.innerHTML = strArr[2];
                $("#disCol" + strArr[1]).html(strArr[2]);
               // modObj.style.display = "none";
                // obj.style.display = '';
                $("#modContent" + strArr[1]).show();
                $("#content" + strArr[1]).hide();
            }
        }

        else {
            alert(strText.replace("ERR|", "温馨提示："));
        }
    }
}

function openSearch(obj) {
    var obj = $("#"+obj+"");
    if (obj.css('display') == "block") {
     
        obj.hide();
      
    }
    else {
      
        obj.show();
       
    }
}
//打开新增窗體
function openAddWindow(name) {
    $('#' + name).window('open');
}
function closeAddWindow(name) {
    $('#' + name).window('close');
}

//點擊更新按鈕
function ManageCol(id, userNcik, type) {

    var obj = $("#content" + id);
    var modObj = $("#modContent" + id);
    var viewObj = $("#viewContent" + id);
    if (type == 'edit') {
        obj.hide();
        modObj.show();
        viewObj.hide();

    }
    else if (type == 'view') {
        if (viewObj.css('display') == "block")
            viewObj.hide();
        else
            viewObj.show();
        obj.show();
        modObj.hide();
    }
    else if (type == 'cancel') {
        obj.show();
        modObj.hide();
        viewObj.hide();
    }
}



//function ManageCol(id, type) {
//    alert("21");
//    var obj = document.getElementById("content" + id);
//    var modObj = document.getElementById("modContent" + id);
//    if (type == 'edit') {
//        obj.style.display = 'none';
//        modObj.style.display = "block";
//        document.getElementById("txtPerNumber" + id).value = id;
//    }
//    else if (type == 'cancel') {
//        obj.style.display = 'block';
//        modObj.style.display = "none";
//    }
//}


//取消更新的操作
function cancelItem(id) {
    var obj = $("#content" + id);
    var modObj = $("#modContent" + id);
    obj.show();
    modObj.hide();
}

function openProgramSetting() {
    $('#ProgramSetting').window('open');
}

function closeProgramSetting() {
    $('#ProgramSetting').window('close');
}

function openCopyPermission() {
    $('#CopyPermission').window('open');
}
function closeCopyPermission() {
    $('#CopyPermission').window('close');
}


function SettingDetail() {
    var obj = document.getElementById("divDetail");
    obj.style.display = "block";
}

function programManage(id, type) {
    var obj = document.getElementById("program" + id);
    var modObj = document.getElementById("modprogram" + id);
    if (type == 'edit') {
        obj.style.display = 'none';
        modObj.style.display = "block";
    }
    else if (type == 'cancel') {
        obj.style.display = 'block';
        modObj.style.display = "none";
    }
}


function openEditWindow() {
    $('#addNew').window('open');
}
function closeEditWindow() {
    $('#addNew').window('close');
}
// divUpdatePwd
function openPwd() {
    $('#divUpdatePwd').window('open');
}
function closePwd() {
    $('#divUpdatePwd').window('close');
}  



function perRight(type) {
    var obj = document.getElementById("perRightView");
    var objEdit = document.getElementById("perRightEdit");
    if (type == "edit") {
        obj.style.display = "none";
        objEdit.style.display = "block";
    }
    else if (type == "cancel") {
        obj.style.display = "block";
        objEdit.style.display = "none";
    }
}
function expandEditTree(id) {
    var obj = document.getElementById("divEditTree" + id);
    var txt = document.getElementById("txtEditPCode" + id);
    obj.setAttribute("top", txt.offsetTop + 20);
    obj.style.left = txt.offsetLeft;
    obj.style.display = "block";
}
function ConfirmEditTree(id) {
    var obj = document.getElementById("divEditTree" + id);
    var txt = document.getElementById("txtEditPCode" + id);
    obj.style.display = "none";
    txt.value = "P001,D001";
}


function checkAll(id, id2) {
    if ($("#" + id).attr("checked")) {
        $("[id$='" + id2 + "']").each(function () {//查找每一个Id以Item结尾的checkbox 
            $(this).attr("checked", true); //选中或者取消选中 
        });
    }
    else {
        $("[id$='" + id2 + "']").each(function () {//查找每一个Id以Item结尾的checkbox 
            $(this).attr("checked", false); //选中或者取消选中 

        });
    }
}

function check(id, id2) {
    if ($("#" + id).prop("checked") == false) {
        $("#" + id2).attr("checked", false);
    }
    else {
        $("#" + id).attr("checked", true);
    }
}
function dialogTransfer(id) {
    var default_left;
    var default_top;
    $('#'+id).dialog({
        title: '详细信息',
        modal: true,
        onOpen: function () {

            //dialog原始left
            default_left = $('#' + id).panel('options').left;
            //dialog原始top
            default_top = $('#' + id).panel('options').top;
        },
        onMove: function (left, top) {  //鼠标拖动时事件

            var body_width = document.body.offsetWidth; //body的宽度
            var body_height = document.body.offsetHeight; //body的高度
            if (left < 1 || left > (body_width - dd_width) || top < 1 || top > (body_height - dd_height)) {
                var dd_width = $('#' + id).panel('options').width; //dialog的宽度
                var dd_height = $('#' + id).panel('options').height; //dialog的高度				   if(left<1||left>(body_width-dd_width)||top<1||top>(body_height-dd_height)){
                $('#' + id).dialog('move', {
                    left: default_left,
                    top: default_top
                });
            }
        }
    });
}
