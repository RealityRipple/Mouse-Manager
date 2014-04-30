Imports System.Runtime.InteropServices
Public Class MouseHook
  <DllImport("user32", CharSet:=CharSet.Auto)> _
  Private Shared Function SetWindowsHookEx(ByVal idHook As Integer, ByVal lpfn As MouseProcDelegate, ByVal hmod As Integer, ByVal dwThreadId As Integer) As Integer
  End Function
  <DllImport("user32", CharSet:=CharSet.Auto)> _
  Private Shared Function CallNextHookEx(ByVal hHook As Integer, ByVal nCode As Integer, ByVal wParam As Integer, ByVal lParam As MSLLHOOKSTRUCT) As Integer
  End Function
  <DllImport("user32", CharSet:=CharSet.Auto)> _
  Private Shared Function UnhookWindowsHookEx(ByVal hHook As Integer) As Integer
  End Function
  Private Delegate Function MouseProcDelegate(ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As MSLLHOOKSTRUCT) As Integer
  Private Structure MSLLHOOKSTRUCT
    Public pt As Point
    Public mouseData As UInt32
    Public flags As UInt32
    Public time As UInt32
    Public dwExtraInfo As UInt32
  End Structure

  Private Const HC_ACTION As Integer = 0
  Private Const WH_MOUSE_LL As Integer = 14
  Private Const WM_XBUTTONDOWN As Integer = &H20B
  Private Const WM_XBUTTONUP As Integer = &H20C
  Private Const WM_XBUTTONDBLCLK As Integer = &H20D

  Private MouseHook As Integer
  Private MouseHookDelegate As MouseProcDelegate

  Public Event Mouse_XButton_Down(ByVal ptLocat As Point, ByVal ButtonNo As UInt32)
  Public Event Mouse_XButton_Up(ByVal ptLocat As Point, ByVal ButtonNo As UInt32)
  Public Event Mouse_XButton_DoubleClick(ByVal ptLocat As Point, ByVal ButtonNo As UInt32)

  Public Sub New()
    MouseHookDelegate = New MouseProcDelegate(AddressOf MouseProc)
    MouseHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookDelegate, System.Runtime.InteropServices.Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0)).ToInt32, 0)
  End Sub

  Private Function MouseProc(ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As MSLLHOOKSTRUCT) As Integer
    If (nCode = HC_ACTION) Then
      Select Case wParam
        Case WM_XBUTTONDOWN
          RaiseEvent Mouse_XButton_Down(lParam.pt, lParam.mouseData)
        Case WM_XBUTTONUP
          RaiseEvent Mouse_XButton_Up(lParam.pt, lParam.mouseData)
        Case WM_XBUTTONDBLCLK
          RaiseEvent Mouse_XButton_DoubleClick(lParam.pt, lParam.mouseData)
      End Select
    End If
    Return 0
    'Return CallNextHookEx(MouseHook, nCode, wParam, lParam)
  End Function

  Protected Overrides Sub Finalize()
    UnhookWindowsHookEx(MouseHook)
    MyBase.Finalize()
  End Sub
End Class

