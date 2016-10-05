<%@ Page language="c#" Codebehind="Logout.aspx.cs" AutoEventWireup="false" Inherits="FriendsReunion.Secure.Logout" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Logout</title>
		<link href="Style/iestyle.css" rel="stylesheet" type="text/css">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
				<tr>
					<td valign="top">
						<asp:image id="Image1" runat="server" imageurl="Images/question.gif"></asp:image></td>
					<td>You are about to leave the application. After this process, you will have to 
						enter your user name and password in order to use the application.
						<br>
						Do you want to proceed?</td>
				</tr>
			</table>
			<p>
				<asp:button id="btnLogout" runat="server" text="Logout" cssclass="Button"></asp:button></p>
		</form>
	</body>
</html>
