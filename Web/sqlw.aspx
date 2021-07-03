<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sqlw.aspx.cs" Inherits="Web.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">  
    <div>
    
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
    
    </div>
        <asp:TextBox ID="textSQL" runat="server" Height="517px" TextMode="MultiLine" Width="1026px"></asp:TextBox>
    </form>
</body>
</html>  
