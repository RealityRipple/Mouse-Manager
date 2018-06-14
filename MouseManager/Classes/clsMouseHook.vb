Imports System.Runtime.InteropServices
Public Class MouseHook
  <DllImport("user32", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)> _
  Private Shared Function SetWindowsHookEx(ByVal idHook As Integer, ByVal lpfn As MouseProcDelegate, ByVal hmod As Integer, ByVal dwThreadId As Integer) As Integer
  End Function
  <DllImport("user32", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)> _
  Private Shared Function CallNextHookEx(ByVal hHook As Integer, ByVal nCode As Integer, ByVal wParam As Integer, ByVal lParam As MSLLHOOKSTRUCT) As Integer
  End Function
  <DllImport("user32", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)> _
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

  Public Class XButtonEventArgs
    Inherits EventArgs
    Public Button As UInt32
    Public Location As Point
    Public Handled As Boolean
    Public Sub New(button As UInt32, location As Point)
      Me.Button = button
      Me.Location = location
      Handled = False
    End Sub
  End Class
  Public Event Mouse_XButton_Down(sender As Object, ByRef e As XButtonEventArgs)
  Public Event Mouse_XButton_Up(sender As Object, ByRef e As XButtonEventArgs)
  Public Event Mouse_XButton_DoubleClick(sender As Object, ByRef e As XButtonEventArgs)

  Public Sub New()
    MouseHookDelegate = New MouseProcDelegate(AddressOf MouseProc)
    MouseHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookDelegate, System.Runtime.InteropServices.Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0)).ToInt32, 0)
  End Sub

  Private Function MouseProc(ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As MSLLHOOKSTRUCT) As Integer
    If (nCode = HC_ACTION) Then
      Select Case wParam
        Case WM_XBUTTONDOWN
          Dim xb As New XButtonEventArgs(lParam.mouseData, lParam.pt)
          RaiseEvent Mouse_XButton_Down(Me, xb)
          If xb.Handled Then Return 1
        Case WM_XBUTTONUP
          Dim xb As New XButtonEventArgs(lParam.mouseData, lParam.pt)
          RaiseEvent Mouse_XButton_Up(Me, xb)
          If xb.Handled Then Return 1
        Case WM_XBUTTONDBLCLK
          Dim xb As New XButtonEventArgs(lParam.mouseData, lParam.pt)
          RaiseEvent Mouse_XButton_DoubleClick(Me, xb)
          If xb.Handled Then Return 1
      End Select
    End If
    Return CallNextHookEx(MouseHook, nCode, wParam, lParam)
  End Function

  Protected Overrides Sub Finalize()
    UnhookWindowsHookEx(MouseHook)
    MyBase.Finalize()
  End Sub
End Class

