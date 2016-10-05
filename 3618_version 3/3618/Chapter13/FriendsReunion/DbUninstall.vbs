Dim oShell
Set oShell = CreateObject("WScript.Shell")
oShell.Exec "osql -S (local) -E -Q ""sp_detach_db N'FriendsData'"