Imports System.ComponentModel
Imports System.Runtime.InteropServices

<DefaultEvent("LinkClicked"), DefaultProperty("LabelColor")>
Public Class LinkLabel
  Inherits System.Windows.Forms.Label
  <DllImport("user32.dll")>
  Private Shared Function LoadCursor(hInstance As IntPtr, lpCursorName As Integer) As Integer
  End Function
  <DllImport("user32.dll")>
  Private Shared Function SetCursor(hCursor As Integer) As Integer
  End Function
  Private c_Hovering As Boolean
  Private c_Link As Boolean
  Private c_Visited As Boolean
  Private c_LabelColor As Color
  Private c_LinkColor As Color
  Private c_LinkHover As LinkBehavior
  Private c_LinkHoverColor As Color
  Private c_LinkActiveColor As Color
  Private c_LinkVisitedColor As Color
  Public Event LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)

  <DefaultValue(True)>
  Public Property Link As Boolean
    Get
      Return c_Link
    End Get
    Set(value As Boolean)
      c_Link = value
      RedrawLabel()
    End Set
  End Property

  <DefaultValue(False)>
  Public Property Visited As Boolean
    Get
      Return c_Visited
    End Get
    Set(value As Boolean)
      c_Visited = value
      RedrawLabel()
    End Set
  End Property

  <Browsable(False), EditorBrowsable(False)>
  Public Overrides Property ForeColor As System.Drawing.Color
    Get
      Return MyBase.ForeColor
    End Get
    Set(value As System.Drawing.Color)
      MyBase.ForeColor = value
    End Set
  End Property

  <DefaultValue(GetType(System.Drawing.Color), "ControlText")>
  Public Property LabelColor As Color
    Get
      Return c_LabelColor
    End Get
    Set(value As Color)
      c_LabelColor = value
      RedrawLabel()
    End Set
  End Property

  <DefaultValue(GetType(System.Drawing.Color), "MediumBlue")>
  Public Property LinkColor As Color
    Get
      Return c_LinkColor
    End Get
    Set(value As Color)
      c_LinkColor = value
      RedrawLabel()
    End Set
  End Property

  <DefaultValue(LinkBehavior.SystemDefault)>
  Public Property LinkHoverBehavior As LinkBehavior
    Get
      Return c_LinkHover
    End Get
    Set(value As LinkBehavior)
      c_LinkHover = value
      RedrawLabel()
    End Set
  End Property

  <DefaultValue(GetType(System.Drawing.Color), "Blue")>
  Public Property LinkHoverColor As Color
    Get
      Return c_LinkHoverColor
    End Get
    Set(value As Color)
      c_LinkHoverColor = value
      RedrawLabel()
    End Set
  End Property

  <DefaultValue(GetType(System.Drawing.Color), "Red")>
  Public Property LinkActiveColor As Color
    Get
      Return c_LinkActiveColor
    End Get
    Set(value As Color)
      c_LinkActiveColor = value
      RedrawLabel()
    End Set
  End Property

  <DefaultValue(GetType(System.Drawing.Color), "Purple")>
  Public Property LinkVisitedColor As Color
    Get
      Return c_LinkVisitedColor
    End Get
    Set(value As Color)
      c_LinkVisitedColor = value
      RedrawLabel()
    End Set
  End Property

  <DefaultValue(False), Browsable(False)>
  Public ReadOnly Property Hovering As Boolean
    Get
      Return c_Hovering
    End Get
  End Property

  Public Sub New()
    MyBase.New()
    c_Hovering = False
    c_LabelColor = SystemColors.ControlText
    c_Link = True
    c_LinkActiveColor = Color.Red
    c_LinkColor = Color.MediumBlue
    c_LinkHover = LinkBehavior.SystemDefault
    c_LinkHoverColor = Color.Blue
    c_LinkVisitedColor = Color.Purple
    c_Visited = False
    RedrawLabel()
  End Sub

  Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
    If c_Link Then
      c_Hovering = True
      RedrawLabel()
    End If
    MyBase.OnMouseEnter(e)
  End Sub

  Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
    If c_Link Then
      c_Hovering = False
      RedrawLabel()
    End If
    MyBase.OnMouseLeave(e)
  End Sub

  Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
    If e.Button = Windows.Forms.MouseButtons.Left And c_Link Then
      MyBase.ForeColor = c_LinkActiveColor
    End If
    MyBase.OnMouseDown(e)
  End Sub

  Protected Overrides Sub OnMouseClick(ByVal e As System.Windows.Forms.MouseEventArgs)
    If e.Button = Windows.Forms.MouseButtons.Left And c_Link Then
      c_Visited = True
      RaiseEvent LinkClicked(Me, New LinkLabelLinkClickedEventArgs(New System.Windows.Forms.LinkLabel.Link(0, Me.Text.Length, Me.Text), e.Button))
    End If
    MyBase.OnMouseClick(e)
  End Sub

  Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
    If e.Button = Windows.Forms.MouseButtons.Left And c_Link Then
      RedrawLabel()
    End If
    MyBase.OnMouseUp(e)
  End Sub

  Public Overrides Property Font As System.Drawing.Font
    Get
      Return MyBase.Font
    End Get
    Set(value As System.Drawing.Font)
      MyBase.Font = value
      RedrawLabel()
    End Set
  End Property

  Private Sub RedrawLabel()
    If c_Link Then
      If c_Hovering Then
        MyBase.ForeColor = c_LinkHoverColor
      Else
        If c_Visited Then
          MyBase.ForeColor = c_LinkVisitedColor
        Else
          MyBase.ForeColor = c_LinkColor
        End If
      End If
      If c_LinkHover = LinkBehavior.AlwaysUnderline Then
        MyBase.Font = New Font(MyBase.Font, FontStyle.Underline)
      ElseIf c_LinkHover = LinkBehavior.HoverUnderline Then
        If c_Hovering Then
          MyBase.Font = New Font(MyBase.Font, FontStyle.Underline)
        Else
          MyBase.Font = New Font(MyBase.Font, FontStyle.Regular)
        End If
      ElseIf c_LinkHover = LinkBehavior.NeverUnderline Then
        MyBase.Font = New Font(MyBase.Font, FontStyle.Regular)
      ElseIf c_LinkHover = LinkBehavior.SystemDefault Then
        Dim SysSetting As String = "yes"
        Try
          Dim regSoftware = My.Computer.Registry.CurrentUser.OpenSubKey("Software")
          Dim regMicrosoft = regSoftware.OpenSubKey("Microsoft")
          Dim regIE = regMicrosoft.OpenSubKey("Internet Explorer")
          Dim regMain = regIE.OpenSubKey("Main")
          SysSetting = regMain.GetValue("Anchor Underline")
        Catch ex As Exception
        End Try
        If SysSetting = "yes" Then
          MyBase.Font = New Font(MyBase.Font, FontStyle.Underline)
        ElseIf SysSetting = "no" Then
          MyBase.Font = New Font(MyBase.Font, FontStyle.Regular)
        ElseIf SysSetting = "hover" Then
          If c_Hovering Then
            MyBase.Font = New Font(MyBase.Font, FontStyle.Underline)
          Else
            MyBase.Font = New Font(MyBase.Font, FontStyle.Regular)
          End If
        Else
          MyBase.Font = New Font(MyBase.Font, FontStyle.Underline)
        End If
      Else
        MyBase.Font = New Font(MyBase.Font, FontStyle.Underline)
      End If
      MyBase.Cursor = Cursors.Hand
    Else
      MyBase.ForeColor = c_LabelColor
      MyBase.Font = New Font(MyBase.Font, FontStyle.Regular)
      MyBase.Cursor = Cursors.Default
    End If
  End Sub

  Protected Overrides Sub WndProc(ByRef msg As System.Windows.Forms.Message)
    If msg.Msg = 32 Then
      If MyBase.Cursor = Cursors.Hand Then
        SetCursor(LoadCursor(0, 32649))
        msg.Result = IntPtr.Zero
        Return
      End If
    End If
    MyBase.WndProc(msg)
  End Sub
End Class
