<%@ Page language="c#" Codebehind="Components.aspx.cs" AutoEventWireup="false" Inherits="FriendsReunion.Components" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Components</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<asp:label id="Label1" runat="server" text='<%# ((DateTime)dsUser.Tables[0].Rows[0]["DateOfBirth"]).ToLongDateString() %>' >
			</asp:label>
		</form>
	</body>
</html>
