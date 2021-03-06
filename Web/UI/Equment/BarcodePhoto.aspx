<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BarcodePhoto.aspx.cs" Inherits="Web.UI.Equment.BarcodePhoto" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <title>拍照上传</title>
    <style type="text/css">
        body {
            margin: 0;
        }

        .content {
            padding: 0.5rem;
            display: flex;
            align-items: center;
            border-bottom: 1px #999 solid;
        }

        .label {
            width: 5rem;
        }

        .img-area {
            flex: 1;
        }

        .container {
            background-color: #e7e7e7;
            position: relative;
        }

            .container div {
                text-align: center;
                padding: 0.5rem 0;
            }

            .container input {
                opacity: 0;
                filter: alpha(opacity=0);
                height: 100%;
                width: 100%;
                position: absolute;
                top: 0;
                left: 0;
                z-index: 9;
            }

            .container p {
                font-size: 0.9rem;
                color: #999;
            }

        .btn {
            background-color: #4363ab;
            color: #fff;
            text-align: center;
            padding: 0.5rem 1rem;
            width: 80%;
            border-radius: 0.2rem;
            margin: 2rem auto;
            font-weight: 600;
            font-size: 1.2rem;
        }
    </style>
    <script type='text/javascript' charset='utf-8'>
        window.onload = function () {
            document.getElementById("id-face").addEventListener("change", function () {
                onFileChange(this, "face-result", "face-empty-result")
            });
            document.getElementById("id-back").addEventListener("change", function () {
                onFileChange(this, "back-result", "back-empty-result")
            });
            document.getElementsByClassName("btn")[0].addEventListener("click", function () {
                submit();
            });
        };
        /**
         * 选中图片时的处理
         * @param {*} fileObj input file元素
         * @param {*} el //选中后用于显示图片的元素ID
         * @param {*} btnel //未选中图片时显示的按钮区域ID
         */
        function onFileChange(fileObj, el, btnel) {
            var windowURL = window.URL || window.webkitURL;
            var dataURL;
            var imgObj = document.getElementById(el);
            document.getElementById(btnel).style.display = "none";
            imgObj.style.display = "block";
            if (fileObj && fileObj.files && fileObj.files[0]) {
                dataURL = windowURL.createObjectURL(fileObj.files[0]);
                imgObj.src = dataURL;
            } else {
                dataURL = fileObj.value;
                imgObj.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale)";
                imgObj.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = dataURL;
            }
        }
        /**
         * 将图片压缩后返回base64格式的数据
         * @param {*} image img元素
         * @param {*} width 压缩后图片宽度
         * @param {*} height 压缩后图片高度
         * @param {*} qua //图片质量1-100
         */
        function compressImageTobase64(image, width, height, qua) {
            var quality = qua ? qua / 100 : 0.8;
            var canvas = document.createElement("canvas"),
                ctx = canvas.getContext('2d');
            var w = image.naturalWidth,
                h = image.naturalHeight;
            canvas.width = width || w;
            canvas.height = height || h;
            ctx.drawImage(image, 0, 0, w, h, 0, 0, width || w, height || h);
            var data = canvas.toDataURL("image/jpeg", quality);
            return data;
        }
        //提交
        function submit() {
            //1、form提交
            //document.getElementById("mainForm").submit();
            //2、压缩后ajax提交
            var face_data = compressImageTobase64(document.getElementById("face-result"), 200, 100, 90);
            var back_data = compressImageTobase64(document.getElementById("back-result"), 200, 100, 90);
            var formData = new FormData();
            formData.append("face", face_data);
            formData.append("back", back_data);
            //需引入jQuery
            $.ajax({
                url: "/地址",
                type: 'POST',
                cache: false,
                data: formData,
                timeout: 180000,
                processData: false,
                contentType: false,
                success: function (r) {
                },
                error: function (r) {
                }
            });
        }
    </script>
</head>
<body>
    <form id="mainForm">
        <div class="content">
            <div class="label">身份证</div>
            <div class="img-area">
                <div class="container">
                    <input type="file" id='id-face' name='face' accept="image/*" />
                    <div id='face-empty-result'>
                        <img style='width: 4rem' src="" alt="">
                        <p>身份证正面照</p>
                    </div>
                    <img style='width: 100%' id='face-result' />
                </div>
                <div class="container" style='margin-top: 0.5rem;'>
                    <input type="file" id='id-back' name='back' accept="image/*" />
                    <div id='back-empty-result'>
                        <img style='width: 4rem' src="" alt="">
                        <p>身份证反面照</p>
                    </div>
                    <img style='width: 100%' id='back-result' />
                </div>
            </div>
        </div>
        <div class="btn">
            提交
        </div>
    </form>
</body>
</html>
