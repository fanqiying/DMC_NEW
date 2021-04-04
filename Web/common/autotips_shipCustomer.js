/**
* 智能提示插件
* 使用:$.autoTips({input:op,keywords:op.value});
* 說明:input為當事輸入框,keywords為模糊查詢內容
*/
var divhei = 144; //div高度
var pagesize = 6; // 每頁顯示個數
var TipsDiv = $('<div id="TipsDiv" style="overflow:auto;height: ' + divhei + 'px;width:300px;position: absolute;z-index:999999;border:1px solid #E8E8E8;background: #fff;">' +
		'<span id="Tips-result"></span>' +
		'</div>');

var inputEl;

var doLi = function (liop) {
    //inputEl.value = $(liop).text();
    var str = $(liop).text().split("-")[0];
    inputEl.value = str;

    $.post('../../ASHX/Basic/ERPDataManage.ashx?M=customer', { Key: inputEl.value, IsLike: 'Y' },
                        function (result) {
                            try {
                                if (result.length > 0) {
                                 $("#customername").val('');
                                    $("#customername").val(result[0].cusname);
                                    BindCustomerItem(inputEl.value);
                                    BindCustomerSty(inputEl.value);


                                } else {
                                    $.messager.show({	// show error message
                                        title: 'Error',
                                        msg: '您輸入的客戶代碼不存在，請重新輸入'
                                    });
                                    $("#customerid").val('');
                                    $("#customerid").focus();
                                }
                            }
                            catch (e) {
                                $.messager.show({
                                    title: '異常提示',
                                    msg: '您輸入的客戶代碼不存在，請重新輸入'
                                });
                                $("#customerid").val('');

                            }
                        }, 'json').error(function () {
                            $.messager.show({
                                title: res.ExceptionTips,
                                msg: res.SubmitDataException
                            });
                        });


};

var hideDiv = function () {
    if ($('#TipsDiv').length > 0) {
        $('#Tips-result').html('');
        $('#TipsDiv').hide();
    }
};

var keywordsold; //搜索關鍵字
var scval = 0; //偏移量
var atdiv; //div

function autoTips(opts) {
    var op = opts || { keywords: "" };
    var keywords = op.keywords;
    if (keywordsold == keywords) return;
    keywordsold = keywords;
    var selIndex = 0; //選中的索引
    if (keywords.length = 0) {
        hideDiv();
    } else {
        inputEl = op.input;

        if (keywords != "") {
            $.ajax({
                type: 'POST',
                url: '../../ASHX/shipment/CustomerManage.ashx?M=autotips',
                dataType: 'json',
                data: { customerID: keywords },
                success: function (data) {
                    if (data.length > 0) {
                        scval = 0;
                        selIndex = 0;
                        showTipsView();
                        $.each(data, function (i, n) {
                            $('#Tips-result').append('<li style="padding:5px;" id="Tips-result-' + i + '" onclick="doLi(this)">' + n.CusCode + '-' + n.CusName + '' + '</li>');
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
                            if (ev.keyCode == 40 || ev.keyCode == 38 ) {
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
                                    doLi($('#Tips-result-' + selIndex));
                                    $('#TipsDiv').hide();
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
        var lis = $('#Tips-result > li');
        $.each(lis, function (i, n) {
            var tmp = $('#Tips-result-' + i);
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
        if ($('#TipsDiv').length == 0) {
            $('body').append(TipsDiv);
        }
        atdiv = $('#TipsDiv');
        var offset = $(inputEl).offset();

        atdiv.css('top', offset.top + inputEl.offsetHeight);
        atdiv.css('left', offset.left);
        $('#Tips-result').html('');
        $('#TipsDiv').show();
    };
};