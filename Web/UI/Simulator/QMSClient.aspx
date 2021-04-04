<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QMSClient.aspx.cs" Inherits="Web.UI.Simulator.QMSClient" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>电子看板预览</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.json-2.4.js"></script>
    <style type="text/css">
        body {
            width: calc(100% - 40px);
            height: calc(100vh);
            /*background: url("../../images/client/SendLog.png") no-repeat;*/
            /*filter: "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale')";*/
            /*-moz-background-size: 100% 100%;*/
            background-size: 100% 100%;
        }

        .divself {
            text-align: center;
            vertical-align: middle;
            background-color: transparent;
            position: absolute;
            display: none;
            margin: 0,0,0,0;
        }

            .divself:hover {
                cursor: pointer;
                /*box-shadow: 0 0 0 rgba(0,0,0,0.1);*/
                transform: scale(1.1);
            }

        div:focus {
            border-color: green;
            border-width: 1px;
        }

        /*.selectDiv {
            transform: scale(1);
            border: 1px solid blue;
        }*/
    </style>
    <script type="text/javascript">
        var messageid = "<%= Request.Params.Get("messageid")%>";
        var CurPage = 0;
        var maxPage = 1;
        $(document).ready(function () {
            $(document).click(function (e) {
                var a = $('.div'); //设置空白以外的目标区域
                if (!a.is(e.target) && a.has(e.target).length === 0) {
                    if (curPdf != null && curPdf.length > 0 && CheckIsNullOrEmpty(curPdf[CurPage].StationNo)) {
                        for (var j = 0; j < cacheButton.length; j++) {
                            if (cacheButton[j].StationNo == curPdf[CurPage].StationNo) {
                                $(cacheButton[j].btnName).focus();
                            }
                        }
                    }
                }
            });
            if (CheckIsNullOrEmpty(messageid)) {
                LoadData(messageid);
            }
            $("#btnPer").click(function () {
                if (CurPage > 1) {
                    CurPage = CurPage - 1;
                }
                else {
                    CurPage = 0;
                }
                SetCurrentPage(CurPage);
            });
            $("#btnNext").click(function () {
                if (CurPage < maxPage) {
                    CurPage = CurPage + 1;
                }
                else {
                    CurPage = maxPage;
                }
                SetCurrentPage(CurPage);
            });
            $("#btnVideo").click(function () {
                if (curPdf != null && curPdf.length > 0 && curPdf[CurPage].VideoFile != null) {
                    document.getElementById("playvideo").src = curPdf[CurPage].VideoFile.UrlPath;
                    document.getElementById("playvideo").play();
                }
                $('#divView').dialog('open').dialog('setTitle', '视频播放');
            });
            //根据id加载json

        });
        //pdf文件路径
        var FileId = "";
        //页码信息
        var curPdf = [];
        function LoadData(messageid) {
            //加载明细
            $.post('../../ASHX/ESOP/SendLog.ashx?M=GetMessage',
               {
                   messageid: messageid
               },
               function (msg) {
                   //返回json
                   if (msg != null) {
                       if (msg.Operate == "2") {
                           switch (msg.TransferType) {
                               case 1:
                                   //SOP
                                   if (msg.Datas != null && msg.Datas.length > 0) {
                                       FileId = msg.Datas[0].SopFile.FileId;
                                       //站点加载 
                                       //msg.Datas.foreach(ForEachStation);
                                       for (var s = 0; s < msg.Datas.length; s++) {
                                           $("#station" + (s + 1)).html(msg.Datas[s].StationId + "-" + msg.Datas[s].StationName);
                                           $("#station" + (s + 1)).show();
                                           cacheButton.push({ StationNo: msg.Datas[s].StationId, btnName: "#station" + (s + 1) });
                                           //2.处理播放页
                                           var pageNums = GetPageNumbers(msg.Datas[s].SopFile.Pages);
                                           for (var i = 0; i < pageNums.length; i++) {
                                               curPdf.push({
                                                   Page: parseInt(pageNums[i]),
                                                   StationNo: msg.Datas[s].StationId,
                                                   VideoFile: msg.Datas[s].VideoFile
                                               });
                                           }
                                       }
                                       CurPage = 0;
                                       maxPage = curPdf.length - 1;
                                       if (curPdf.length > 1) {
                                           $("#btnPer").show();
                                           $("#btnNext").show();
                                       }
                                       SetCurrentPage(CurPage);
                                   }
                                   break;
                               case 2:
                                   //通知
                                   break;
                           }
                       }
                   }
               },
        'json');
        }

        function SetCurrentPage(SelectPage) {
            var iPage = curPdf[SelectPage].Page;
            //加载pdf内容
            try {
                $("body").css("background", "url(../../ASHX/ESOP/SendLog.ashx?M=GetPdfImg&ipage=" + iPage + "&fileid=" + FileId + ") no-repeat center");
            }
            catch (exception) {
            }
            //控制视频按钮播放显示
            if (curPdf[SelectPage].VideoFile == null) {
                $("#btnVideo").hide();
            } else {
                $("#btnVideo").show();
            }
            //设置站点选中
            if (CheckIsNullOrEmpty(curPdf[SelectPage].StationNo)) {
                for (var j = 0; j < cacheButton.length; j++) {
                    if (cacheButton[j].StationNo == curPdf[SelectPage].StationNo) {
                        $(cacheButton[j].btnName).focus();
                    }
                }
            }
            //控制上下页是否可见
            if (SelectPage == 0) {
                $("#btnPer").hide();
            } else {
                $("#btnPer").show();
            }

            if (SelectPage == maxPage) {
                $("#btnNext").hide();
            } else {
                $("#btnNext").show();
            }
        }

        function CheckIsNullOrEmpty(value) {
            //正则表达式用于判斷字符串是否全部由空格或换行符组成
            var reg = /^\s*$/
            //返回值为true表示不是空字符串
            return (value != null && value != undefined && !reg.test(value))
        }
        var cacheButton = [];
        function GetPageNumbers(Pages) {
            var pageNums = [];
            if (Pages.indexOf(",") > -1) {
                pageNums = Pages.split(",");
            }
            else if (Pages.indexOf("-") > -1) {
                var sePageNums = Pages.split("-");
                var iStartNum = parseInt(sePageNums[0]);
                var iEndNum = parseInt(sePageNums[1]);
                for (var i = iStartNum; i < iEndNum; i++) {
                    pageNums.push(i.ToString());
                }
            }
            else {
                pageNums.push(Pages);
            }
            return pageNums;
        }

        function selectStation(div) {
            var StationIdAndName = div.innerHTML;
            var StationId = StationIdAndName.split("-")[0];
            for (var i = 0; i < curPdf.length; i++) {
                if (curPdf[i].StationNo == StationId) {
                    CurPage = i;
                    SetCurrentPage(CurPage);
                    break;
                }
            }
        }
    </script>
</head>
<body>
    <div id="station1" class="divself" onclick="selectStation(this)" tabindex="1" style="line-height: 23px; width: 120px; font-size: 12px; height: 23px; float: right; background-color: white; top: 5px; left: 5px;">
        A00001-组装1站
    </div>
    <div id="station2" class="divself" onclick="selectStation(this)" tabindex="2" style="line-height: 23px; width: 120px; font-size: 12px; height: 23px; float: right; background-color: white; top: 5px; left: 126px;">
        A00002-组装2站
    </div>
    <div id="station3" class="divself" onclick="selectStation(this)" tabindex="3" style="line-height: 23px; width: 120px; font-size: 12px; height: 23px; float: right; background-color: white; top: 5px; left: 247px;">
        A00003-组装3站
    </div>
    <div id="station4" class="divself" onclick="selectStation(this)" tabindex="4" style="line-height: 23px; width: 120px; font-size: 12px; height: 23px; float: right; background-color: white; top: 5px; left: 368px;">
        A00004-组装4站
    </div>
    <div id="station5" class="divself" onclick="selectStation(this)" tabindex="5" style="line-height: 23px; width: 120px; font-size: 12px; height: 23px; float: right; background-color: white; top: 5px; left: 489px;">
        A00005-组装5站
    </div>
    <div id="btnPer" class="divself" style="width: 66px; height: 66px; float: left; background-image: url(../../images/client/per64.png); top: 50%; margin-top: -33px; left: 5px;">
    </div>
    <div id="btnNext" class="divself" style="width: 66px; height: 66px; float: right; background-image: url(../../images/client/next64.png); top: 50%; margin-top: -33px; right: 5px;">
    </div>

    <div id="btnVideo" class="divself" style="width: 32px; height: 32px; float: right; background-image: url(../../images/client/play.png); top: 5px; margin-top: 5px; right: 42px;">
    </div>

    <div id="btnClose" class="divself" style="display: block; width: 32px; height: 32px; float: right; background-image: url(../../images/client/close.png); top: 5px; margin-top: 5px; right: 5px;">
    </div>

    <div id="gpOpter" class="divself" style="display: none; width: 100px; height: 180px; float: left; background-color: gray; bottom: 5px; left: 5px;">
    </div>

    <div id="btnHide" class="divself" style="width: 20px; height: 20px; float: left; background-image: url(../../images/client/expand.png); bottom: 5px; left: 5px;">
    </div>
    <%--<div id="lblMessage" style="width: 99%; height: 64px; background-color: green; position: absolute; bottom: 0px;">
    </div>--%>

    <div id="divView" class="easyui-dialog" data-options="closed:true,modal:true,maximizable:false"
        style="width: 850px; height: 450px; padding: 5px;">
        <video id="playvideo" width="820" height="400" controls></video>
    </div>
</body>
</html>
