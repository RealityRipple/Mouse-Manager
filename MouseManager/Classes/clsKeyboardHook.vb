Imports System.Runtime.InteropServices

Public Class KeyboardHook
  <DllImport("user32", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
  Private Shared Function SetWindowsHookEx(ByVal idHook As Integer, ByVal lpfn As KeyboardProcDelegate, ByVal hmod As Integer, ByVal dwThreadId As Integer) As Integer
  End Function
  <DllImport("user32", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
  Private Shared Function CallNextHookEx(ByVal hHook As Integer, ByVal nCode As Integer, ByVal wParam As Integer, ByVal lParam As KBDLLHOOKSTRUCT) As Integer
  End Function
  <DllImport("user32", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
  Private Shared Function UnhookWindowsHookEx(ByVal hHook As Integer) As Integer
  End Function
  Private Delegate Function KeyboardProcDelegate(ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As KBDLLHOOKSTRUCT) As Integer
  Private Structure KBDLLHOOKSTRUCT
    Public vkCode As UInt32
    Public scanCode As UInt32
    Public flags As KBDLLHOOKSTRUCTFlags
    Public time As UInt32
    Public dwExtraInfo As UIntPtr
  End Structure
  Public Enum KBDLLHOOKSTRUCTFlags As UInt32
    LLKHF_EXTENDED = &H1
    LLKHF_INJECTED = &H10
    LLKHF_ALTDOWN = &H20
    LLKHF_UP = &H80
  End Enum
  Private Const HC_ACTION As Integer = 0
  Private Const WH_KEYBOARD_LL As Integer = 13
  Private Const WM_KEYDOWN As Integer = &H100
  Private Const WM_KEYUP As Integer = &H101
  Private KeyboardHook As Integer
  Private KeyboardHookDelegate As KeyboardProcDelegate
  Private Block As Boolean = False
  Public Event Keyboard_Press(ByVal sender As Object, ByVal e As KeyEventArgs)

  Public Sub New()
    KeyboardHookDelegate = New KeyboardProcDelegate(AddressOf KeyboardProc)
    KeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookDelegate, System.Runtime.InteropServices.Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0)).ToInt32, 0)
  End Sub

  Public Property BlockWin As Boolean
    Get
      Return Block
    End Get
    Set(value As Boolean)
      Block = value
    End Set
  End Property

  Private Function KeyboardProc(ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As KBDLLHOOKSTRUCT) As Integer
    If (nCode = HC_ACTION) Then
      If Block Then
        If (wParam = WM_KEYDOWN) Then
          Dim xb As New KeyEventArgs(lParam.vkCode)
          RaiseEvent Keyboard_Press(Me, xb)
          If xb.Handled Then Return 1
        End If
        If (wParam = WM_KEYUP) Then Return 1
      End If
    End If
    Return CallNextHookEx(KeyboardHook, nCode, wParam, lParam)
  End Function

  Protected Overrides Sub Finalize()
    UnhookWindowsHookEx(KeyboardHook)
    MyBase.Finalize()
  End Sub
End Class
