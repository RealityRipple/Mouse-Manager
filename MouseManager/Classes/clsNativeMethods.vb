Imports System.Runtime.InteropServices
Public NotInheritable Class NativeMethods
  <DllImport("user32", CharSet:=CharSet.Auto)>
  Public Shared Sub keybd_event(ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As UInt32, ByVal dwExtraInfo As UIntPtr)
  End Sub
  <DllImport("user32", CharSet:=CharSet.Auto)>
  Public Shared Function MapVirtualKey(ByVal wCode As Long, ByVal wMapType As Long) As Byte
  End Function
  Public Const KEYEVENTF_KEYDOWN As UInt32 = &H0
  Public Const KEYEVENTF_KEYUP As UInt32 = &H2
End Class
