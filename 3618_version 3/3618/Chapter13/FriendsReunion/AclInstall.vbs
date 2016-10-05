Dim oShell
Set oShell = CreateObject("WScript.Shell")
Dim path
path = WScript.Arguments(0)
If Right(path, 1) <> "\" Then
	path = path & "\"
End If
oShell.Exec "cacls " + path + "upload.xml /E /G Everyone:R"
oShell.Exec "cacls " + path + "Friends.xsd /E /G Everyone:R"

WScript.Echo("Finished")