<%@ Page language="c#" Codebehind="Default.aspx.cs" AutoEventWireup="false" Inherits="Acudei._Default" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>ACUDEI English Academy</title>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<p>Welcome to ACUDEI English Academy.
			</p>
			<p>We are an&nbsp;associate of the Friends Reunion community, and
				<asp:label id="txtCount" runat="server"></asp:label>&nbsp;of its members 
				have&nbsp;taken at least one course here.&nbsp;<br>
				To have a quick look at your contacts in the Friends Reunion community, please 
				enter your login and password:</p>
			<p>Login:
				<asp:textbox id="txtLogin" runat="server"></asp:textbox><br>
				Password:
				<asp:textbox id="txtPassword" runat="server"></asp:textbox><br>
				<asp:button id="btnRefresh" runat="server" text="Refresh"></asp:button></p>
			<p>
				<asp:datagrid id="grdContacts" runat="server" />
			</p>
			<asp:label id="lblError" runat="server" forecolor="Red" enableviewstate="False" visible="False"></asp:label>
		</form>
	</body>
</html>
