/**
* 智能提示插件
* 使用:$.autoTips({input:op,keyword:op.value});
* 說明:input為當事輸入框,keyword為模糊查詢內容
*/
var divhei = 144; //div高度
var pagesize = 6; // 每頁顯示個數
var autoTipsDiv = $('<div id="autoTipsDiv" style="overflow:auto;height: ' + divhei + 'px;width:150px;position: absolute;z-index:999999;border:1px solid #E8E8E8;background: #fff;">' +
		'<span id="aotoTips-result"></span>' +
		'</div>');

var inputEl;

var doLiClick = function (liop, Nature) {

  
    inputEl.value =$(liop).text().split("(")[0];

    if (Nature)
        $('#hideSelectGroup').val(Nature);
    if (inputEl.value != '') {

        $.post('../../ASHX/Organize/GetdeptHeader.ashx', { deptID: encodeURIComponent(inputEl.value) },
                         function (result) {
                             try {
                                 if (result.substring(0, 3) != 'Err') {
                                     $("#signerid").val(result);

                                 } else {
                                     $.messager.show({	// show error message
                                         title: 'Error',
                                         msg: result.msg
                                     });
                                 }
                             }
                             catch (e) {
                                 $.messager.show({
                                     title: '異常提示',
                                     msg: '提交數據異常，請確認網絡連接狀況'
                                 });

                             }
                         }, 'text').error(function () {
                             $.messager.show({
                                 title: res.ExceptionTips,
                                 msg: res.SubmitDataException
                             });
                         });


    }
};

var hideDiv = function () {
    if ($('#autoTipsDiv').length > 0) {
        $('#aotoTips-result').html('');
        $('#autoTipsDiv').hide();
    }
};

var keywordold; //搜索關鍵字
var scval = 0; //偏移量
var atdiv; //div

function Tips(opts) {
    var op = opts || { keyword: "" };
    var keyword = op.keyword;
    if (keywordold == keyword) return;
    keywordold = keyword;
    var selIndex = 0; //選中的索引
    if (keyword.length = 0) {

        hideDiv();
    } else {
        inputEl = op.input;

        if (keyword != "") {
            $.ajax({
                type: 'POST',
                url: '../../ASHX/Permission/GetUserInfo.ashx',
                dataType: 'json',
                data: { deptID: keyword },
                success: function (data) {
                    if (data.length > 0) {
                        scval = 0;
                        selIndex = 0;
                        showTipsView();
                        $.each(data, function (i, n) {
                            $('#aotoTips-result').append('<li style="padding:5px;" id="aotoTips-result-' + i + '" onclick="doLiClick(this,\'' + n.deptNature + '\')">'
									+ n.deptID + '(' + n.simpleName + '-'+ n.fullName +')' + '</li>');
                        });

                        //默認選中第一行
                        moveSelectLi(selIndex);

                        $('span li').mouseover(function () {
                            $(this).css("cursor", "pointer");
                            $(this).css("backgroundColor", "#F0F0F0");
                            selIndex = $(this).attr('id').substring($(this).attr('id').lastIndexOf("-") + 1, $(this).attr('id').length);
                        }).mouseout(function () {
                            $(this).css("backgroundColor", "");
                        });

                        atdiv.hover(function () { }, function () {
                            $(this).hide();
                        });

                        $(inputEl).keyup(function (ev) {
                            //40=下 38=上 13=回車
                            if (ev.keyCode == 40 || ev.keyCode == 38 || ev.keyCode == 13) {
                                if (ev.keyCode == 40 && selIndex < data.length - 1) {
                                    //計算偏移量
                                    scval = Math.floor((selIndex + 1) / pagesize) * divhei;
                                    selIndex++;
                                    moveSelectLi(selIndex);
                                }
                                if (ev.keyCode == 38 && selIndex > 0) {
                                    selIndex--;
                                    //計算偏移量
                                    scval = Math.floor((selIndex) / pagesize) * divhei;
                                    moveSelectLi(selIndex);
                                }
                                if (ev.keyCode == 13) {
                                    doLiClick($('#aotoTips-result-' + selIndex));
                                    $('#autoTipsDiv').hide();
                                }
                            }
                        });
                    } else {
                        hideDiv();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    parent.ajaxLoadEnd();
                    $.messager.show({
                        title: res.FailureTips,
                        msg: res.IntenetException
                    });
                }
            });
        }
    }

    var moveSelectLi = function (mindex) {
        atdiv.scrollTop(scval);
        var lis = $('#aotoTips-result > li');
        $.each(lis, function (i, n) {
            var tmp = $('#aotoTips-result-' + i);
            if (i == mindex) {
                selectLi(tmp);
            } else {
                noSelectLi(tmp);
            }
        });
    };

    var selectLi = function (ops) {
        $(ops).css("cursor", "pointer");
        $(ops).css("backgroundColor", "#F0F0F0");
    };

    var noSelectLi = function (ops) {
        $(ops).css("backgroundColor", "");
    };

    var showTipsView = function () {
        if ($('#autoTipsDiv').length == 0) {
            $('body').append(autoTipsDiv);
        }
        atdiv = $('#autoTipsDiv');
        var offset = $(inputEl).offset();

        atdiv.css('top', offset.top + inputEl.offsetHeight);
        atdiv.css('left', offset.left);
        $('#aotoTips-result').html('');
        $('#autoTipsDiv').show();
    };
};