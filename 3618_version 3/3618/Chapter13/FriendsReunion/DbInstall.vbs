Dim oShell
Set oShell = CreateObject("WScript.Shell")
Dim path
path = WScript.Arguments(0)
If Right(path, 1) <> "\" Then
	path = path & "\"
End If
oShell.Exec "osql -S (local) -E -Q ""sp_attach_db N'FriendsData', N'" + path + "Friends_Data.MDF', N'" + path + "Friends_Log.LDF'"