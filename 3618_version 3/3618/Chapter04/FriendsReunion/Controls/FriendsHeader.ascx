<%@ Control Language="c#" AutoEventWireup="false" Codebehind="FriendsHeader.ascx.cs" Inherits="FriendsReunion.Controls.FriendsHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:panel id="pnlHeaderGlobal" runat="server" cssclass="HeaderFriends">Friends Reunion
<asp:image id="imgFriends" runat="server" cssclass="HeaderImage" imageurl="../Images/friends.gif"></asp:image></asp:panel>
<asp:panel id="pnlHeaderLocal" runat="server" cssclass="HeaderTitle">
	<asp:image id="imgIcon" runat="server" cssclass="HeaderImage" imageurl="../Images/homeconnected.gif"></asp:image>
	<asp:label id="lblWelcome" runat="server">Welcome!</asp:label>
</asp:panel>
