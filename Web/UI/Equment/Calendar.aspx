<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calendar.aspx.cs" Inherits="Web.UI.Equment.Calendar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="../../easyUI15/jquery.min.js" type="text/javascript"></script>
    <link href="../../easyUI15/themes/public.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyUI15/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyUI15/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.json-2.4.js"></script>
    <script src="../../easyUI15/locale/easyui-lang-zh_TW.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="easyui-calendar" style="width:800px;height:800px;" data-options="formatter:formatDay"></div>
    </form>
    <script>
		var d1 = Math.floor((Math.random()*30)+1);
		var d2 = Math.floor((Math.random()*30)+1);
		function formatDay(date){
			var m = date.getMonth()+1;
			var d = date.getDate();
			var opts = $(this).calendar('options');
			if (opts.month == m && d == d1){
			    return '<div style="width:30px;height:30px;">' + d + "eewewer" + '</div>';
			} else if (opts.month == m && d == d2){
				return '<div >' + d + '</div>';
			}
			return d;
		}
	</script>
	<style scoped="scoped">
		.md{
			height:60px;
			line-height:16px;
			background-position:2px center;
			text-align:right;
			font-weight:bold;
			padding:0 2px;
			color:red;
		}
	</style>
</body>
</html>
