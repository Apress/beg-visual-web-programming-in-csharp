' Changes the login mode of MSDE
Option Explicit

Dim shell
Set shell = CreateObject("WScript.Shell")
Rem Write For default SDK tutorials installation.
shell.RegWrite "HKLM\SOFTWARE\Microsoft\Microsoft SQL Server\NetSDK\MSSQLServer\LoginMode", &H2, "REG_DWORD"
Rem Write For standalone MSDE installation (from asp.net site) or full SQL Server setups.
shell.RegWrite "HKLM\SOFTWARE\Microsoft\MSSQLServer\MSSQLServer\LoginMode", &H2, "REG_DWORD"