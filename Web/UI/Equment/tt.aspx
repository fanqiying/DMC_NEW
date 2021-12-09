﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tt.aspx.cs" Inherits="Web.UI.Equment.tt" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">  
	<meta name="viewport" content="initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <title>Basic DataGrid - jQuery EasyUI Mobile Demo</title>  
    <link rel="stylesheet" type="text/css" href="../../easyUI15/themes/metro/easyui.css">  
    <link rel="stylesheet" type="text/css" href="../../easyUI15/themes/mobile.css">  
    <link rel="stylesheet" type="text/css" href="../../easyUI15/themes/icon.css">  
    <script type="text/javascript" src="../../easyUI15/jquery.min.js"></script>  
    <script type="text/javascript" src="../../easyUI15/jquery.easyui.min.js"></script> 
    <script type="text/javascript" src="../../easyUI15/jquery.easyui.mobile.js"></script> 
</head>
<body>
    <table id="dg" data-options="header:'#hh',singleSelect:true,border:false,fit:true,fitColumns:true,scrollbarSize:0">  
        <thead>  
            <tr>  
                <th data-options="field:'itemid',width:80">Item ID</th>  
                <th data-options="field:'productid',width:100">Product</th>  
                <th data-options="field:'listprice',width:80,align:'right'">List Price</th>  
                <th data-options="field:'unitcost',width:80,align:'right'">Unit Cost</th>  
            </tr>
        </thead>  
    </table>
    <div id="hh">
    	<div class="m-toolbar">
    		<div class="m-title">Basic DataGrid</div>
    	</div>
    </div>
	<script>
		var data = 	[
			{"productid":"FI-SW-01","productname":"Koi","unitcost":10.00,"status":"P","listprice":36.50,"attr1":"Large","itemid":"EST-1"},
			{"productid":"K9-DL-01","productname":"Dalmation","unitcost":12.00,"status":"P","listprice":18.50,"attr1":"Spotted Adult Female","itemid":"EST-10"},
			{"productid":"RP-SN-01","productname":"Rattlesnake","unitcost":12.00,"status":"P","listprice":38.50,"attr1":"Venomless","itemid":"EST-11"},
			{"productid":"RP-SN-01","productname":"Rattlesnake","unitcost":12.00,"status":"P","listprice":26.50,"attr1":"Rattleless","itemid":"EST-12"},
			{"productid":"RP-LI-02","productname":"Iguana","unitcost":12.00,"status":"P","listprice":35.50,"attr1":"Green Adult","itemid":"EST-13"},
			{"productid":"FL-DSH-01","productname":"Manx","unitcost":12.00,"status":"P","listprice":158.50,"attr1":"Tailless","itemid":"EST-14"},
			{"productid":"FL-DSH-01","productname":"Manx","unitcost":12.00,"status":"P","listprice":83.50,"attr1":"With tail","itemid":"EST-15"},
			{"productid":"FL-DLH-02","productname":"Persian","unitcost":12.00,"status":"P","listprice":23.50,"attr1":"Adult Female","itemid":"EST-16"},
			{"productid":"FL-DLH-02","productname":"Persian","unitcost":12.00,"status":"P","listprice":89.50,"attr1":"Adult Male","itemid":"EST-17"},
			{"productid":"AV-CB-01","productname":"Amazon Parrot","unitcost":92.00,"status":"P","listprice":63.50,"attr1":"Adult Male","itemid":"EST-18"}
		];
		$(function(){
			$('#dg').datagrid({
				data: data
			});
		});
	</script>
</body>	
</html>