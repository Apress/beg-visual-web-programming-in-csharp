<%@ Page language="c#" Codebehind="Users.aspx.cs" AutoEventWireup="false" Inherits="FriendsReunion.Admin.Users" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Users</title>
		<link href="../Style/iestyle.css" rel="stylesheet" type="text/css">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<p>Welcome to the Users Administration page. This is the complete list of users:</p>
			<asp:datagrid id="grdUsers" runat="server" bordercolor="#E7E7FF" borderstyle="None" borderwidth="1px"
				backcolor="White" cellpadding="3" gridlines="Horizontal" width="100%" datasource="<%# dsData %>">
				<selecteditemstyle font-bold="True" forecolor="#F7F7F7" backcolor="#738A9C"></selecteditemstyle>
				<alternatingitemstyle backcolor="#F7F7F7"></alternatingitemstyle>
				<itemstyle forecolor="#4A3C8C" backcolor="#E7E7FF"></itemstyle>
				<headerstyle font-bold="True" forecolor="#F7F7F7" backcolor="#4A3C8C"></headerstyle>
				<footerstyle forecolor="#4A3C8C" backcolor="#B5C7DE"></footerstyle>
				<pagerstyle horizontalalign="Right" forecolor="#4A3C8C" backcolor="#E7E7FF" mode="NumericPages"></pagerstyle>
			</asp:datagrid>
		</form>
	</body>
</html>
