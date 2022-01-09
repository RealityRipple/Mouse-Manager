Imports System.Runtime.InteropServices
Public Class frmOptions
  <DllImport("user32", CharSet:=CharSet.Auto)> _
  Private Shared Sub keybd_event(ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As UInt32, ByVal dwExtraInfo As UIntPtr)
  End Sub
  <DllImport("user32", CharSet:=CharSet.Auto)> _
  Public Shared Function MapVirtualKey(ByVal wCode As Long, ByVal wMapType As Long) As Byte
  End Function
  Public Const KEYEVENTF_KEYDOWN As UInt32 = &H0
  Public Const KEYEVENTF_KEYUP As UInt32 = &H2
  Private WithEvents mHook As MouseHook
  Private WithEvents kHook As KeyboardHook
  Private tDetection As Threading.Timer
  Private selButton4Action As New List(Of Keys)
  Private selButton5Action As New List(Of Keys)
  Private Mouse4Down As UShort = 0
  Private Mouse5Down As UShort = 0
  Private delKey As Boolean = False
  Private loaded As Boolean = False
  Private Const HomeURL As String = "http://realityripple.com"
  Private Const HomeSURL As String = "https://realityripple.com"
  Private Const DonateURL As String = "http://realityripple.com/donate.php?itm=Mouse+Manager"
  Private Const DonateSURL As String = "https://realityripple.com/donate.php?itm=Mouse+Manager"
  Private Const PurchaseURL As String = "http://realityripple.com/Software/Applications/Advanced-Mouse-Manager/"
  Private Const PurchaseSURL As String = "https://realityripple.com/Software/Applications/Advanced-Mouse-Manager/"

  Private Sub frmOptions_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If e.CloseReason = CloseReason.UserClosing Then
      e.Cancel = True
      Me.Hide()
    End If
  End Sub

  Private Sub frmOptions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    trayIcon.Icon = My.Resources.Icon
    Me.Icon = My.Resources.Icon
    chkStart.Checked = My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("Run", True).GetValue("MouseManager", vbNullString) = Application.ExecutablePath
    If My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName) AndAlso
      My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey(Application.CompanyName).GetSubKeyNames.Contains(Application.ProductName) Then
      Dim myParentKey As Microsoft.Win32.RegistryKey = GetRegKey(False)
      Dim myRegKey As Microsoft.Win32.RegistryKey = Nothing
      If (myParentKey.SubKeyCount > 0 AndAlso myParentKey.GetSubKeyNames.Contains("Profiles")) Or (myParentKey.ValueCount > 0) Then
        myRegKey = myParentKey
      ElseIf myParentKey.GetSubKeyNames.Contains("1.1.0.0") Then
        myRegKey = myParentKey.OpenSubKey("1.1.0.0")
        delKey = True
      ElseIf myParentKey.GetSubKeyNames.Contains("1.0.0.0") Then
        myRegKey = myParentKey.OpenSubKey("1.0.0.0")
        delKey = True
      End If
      If myRegKey IsNot Nothing Then
        Dim iDefault As Integer = -1
        Dim myProfileKey As Microsoft.Win32.RegistryKey = GetRegProfiles(myRegKey, False)
        If myProfileKey IsNot Nothing Then
          For Each Key As String In myProfileKey.GetSubKeyNames()
            Dim lvItem As New ListViewItem
            If String.IsNullOrEmpty(myProfileKey.OpenSubKey(Key).GetValue("Button4")) And String.IsNullOrEmpty(myProfileKey.OpenSubKey(Key).GetValue("Button5")) Then
              lvItem.Text = myProfileKey.OpenSubKey(Key).GetValue("Button1").ToString
              lvItem.SubItems.Add(myProfileKey.OpenSubKey(Key).GetValue("Button2").ToString)
            Else
              lvItem.Text = myProfileKey.OpenSubKey(Key).GetValue("Button4").ToString
              lvItem.SubItems.Add(myProfileKey.OpenSubKey(Key).GetValue("Button5").ToString)
            End If
            lvProfiles.Items.Add(lvItem)
          Next
          If lvProfiles.Items.Count <> 0 Then tmrInit.Enabled = True
          iDefault = CInt(myProfileKey.GetValue("Default"))
        End If
        chkEnable.Checked = myRegKey.GetValue("Enabled")
        If chkEnable.Checked Then
          If lvProfiles.Items.Count > iDefault Then lvProfiles.Items(iDefault).Checked = True
          selButton4Action = StrToKeys(lvProfiles.CheckedItems(0).Text)
          selButton5Action = StrToKeys(lvProfiles.CheckedItems(0).SubItems(1).Text)
        End If
        If iDefault > -1 And lvProfiles.Items.Count > iDefault Then lvProfiles.Items(iDefault).Tag = True
      End If
    End If
    lblVersion.Text = "v" & Application.ProductVersion
    If OSSupport.NativeTLS12 Then
      lblWebsite.Text = HomeSURL
    Else
      lblWebsite.Text = HomeURL
    End If
    cmdSave.Enabled = False
    cmdRem.Enabled = False
    loaded = True
  End Sub

  Private Sub frmOptions_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged
    If tbsManager.SelectedIndex = 1 Then
      lvProfiles.Columns(0).Width = CInt(Math.Floor(lvProfiles.ClientSize.Width / 2) - 1)
      lvProfiles.Columns(1).Width = CInt(Math.Floor(lvProfiles.ClientSize.Width / 2) - 1)
    End If
  End Sub

  Private Sub frmOptions_Deactivate(sender As Object, e As EventArgs) Handles Me.Deactivate
    If chkEnable.Checked AndAlso mHook Is Nothing Then mHook = New MouseHook()
    If Not IsNothing(kHook) Then kHook.BlockWin = False
  End Sub

  Private Sub frmOptions_Activated(sender As Object, e As EventArgs) Handles Me.Activated
    If Me.Visible Then
      If mHook IsNot Nothing Then mHook = Nothing
    End If
    If Not IsNothing(kHook) AndAlso (Me.ActiveControl.Name = txtButton4.Name Or Me.ActiveControl.Name = txtButton5.Name) Then kHook.BlockWin = True
  End Sub

  Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
    Dim lvItem As New ListViewItem
    lvItem.Text = "PageUp"
    lvItem.SubItems.Add("PageDown")
    lvProfiles.Items.Add(lvItem)
    lvItem.Selected = True
    txtButton4.Focus()
    If (From lvFind As ListViewItem In lvProfiles.Items Where lvFind.Checked Select lvFind).Count = 0 Then lvItem.Checked = True
    If loaded Then cmdSave.Enabled = ChangesMade()
  End Sub

  Private Sub cmdRem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRem.Click
    lvProfiles.Items.Remove(lvProfiles.SelectedItems(0))
    If lvProfiles.Items.Count = 0 Then chkEnable.Checked = False
    If loaded Then cmdSave.Enabled = ChangesMade()
  End Sub

  Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
    Me.Hide()
  End Sub

  Private Sub lvProfiles_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvProfiles.ItemChecked
    If e.Item.Checked Then
      e.Item.Tag = True
      If Not chkEnable.Checked Then chkEnable.Checked = True
      For Each item As ListViewItem In lvProfiles.Items
        If Not e.Item.Index = item.Index Then
          item.Checked = False
          item.Tag = Nothing
        End If
      Next
    Else
      If lvProfiles.CheckedItems.Count = 0 Then
        If chkEnable.Checked Then chkEnable.Checked = False
        e.Item.Tag = True
      End If
    End If
    selButton4Action = StrToKeys(e.Item.Text)
    selButton5Action = StrToKeys(e.Item.SubItems(1).Text)
    If loaded Then cmdSave.Enabled = ChangesMade()
  End Sub

  Private Sub lvProfiles_ItemSelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvProfiles.ItemSelectionChanged
    If e.IsSelected Then
      txtButton4.Text = e.Item.Text
      txtButton5.Text = e.Item.SubItems(1).Text
    End If
    If lvProfiles.SelectedItems.Count = 0 Then
      cmdRem.Enabled = False
      txtButton4.Text = Nothing
      txtButton5.Text = Nothing
    Else
      cmdRem.Enabled = True
    End If
  End Sub

  Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
    Application.Exit()
  End Sub

  Private Sub mnuManagement_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuManagement.Click
    Me.Show()
  End Sub

  Private ReadOnly Property KeyboardDelay As UInteger
    Get
      Select Case SystemInformation.KeyboardDelay
        Case 0 : Return 250
        Case 1 : Return 500
        Case 2 : Return 750
        Case 3 : Return 1000
      End Select
      Return 500
    End Get
  End Property

  Private ReadOnly Property KeyboardRepeat As UInteger
    Get
      Return ((31 - SystemInformation.KeyboardSpeed) * 12.258) + 20
    End Get
  End Property

  Private Sub KeyHold()
    If tDetection IsNot Nothing Then
      tDetection.Dispose()
      tDetection = Nothing
    End If
    tDetection_Tick(Nothing)
    tDetection = New Threading.Timer(New Threading.TimerCallback(AddressOf tDetection_Tick), Nothing, KeyboardDelay, KeyboardRepeat)
  End Sub

  Private Sub ClearKeyHold()
    If Not Mouse4Down = 0 Then Return
    If Not Mouse5Down = 0 Then Return
    If tDetection IsNot Nothing Then
      tDetection.Dispose()
      tDetection = Nothing
    End If
  End Sub

  Private Sub mHook_Mouse_XButton_Down(sender As Object, ByRef e As MouseHook.XButtonEventArgs) Handles mHook.Mouse_XButton_Down
    Select Case e.Button
      Case &H10000
        If selButton4Action Is Nothing Then Return
        If Not Mouse4Down = 0 Then Return
        If Mouse5Down = 0 Then
          Mouse4Down = 1
        Else
          Mouse4Down = 2
        End If
      Case &H20000
        If selButton5Action Is Nothing Then Return
        If Not Mouse5Down = 0 Then Return
        If Mouse4Down = 0 Then
          Mouse5Down = 1
        Else
          Mouse5Down = 2
        End If
      Case Else
        Return
    End Select
    KeyHold()
    e.Handled = True
  End Sub

  Private Sub mHook_Mouse_XButton_Up(sender As Object, ByRef e As MouseHook.XButtonEventArgs) Handles mHook.Mouse_XButton_Up
    Dim useAction As List(Of Keys) = Nothing
    Select Case e.Button
      Case &H10000
        If Mouse4Down = 0 Then Return
        If selButton4Action Is Nothing Then Return
        useAction = New List(Of Keys)
        useAction.AddRange(selButton4Action)
        If Mouse5Down = 2 Then Mouse5Down = 1
        Mouse4Down = 0
      Case &H20000
        If Mouse5Down = 0 Then Return
        If selButton5Action Is Nothing Then Return
        useAction = New List(Of Keys)
        useAction.AddRange(selButton5Action)
        If Mouse4Down = 2 Then Mouse4Down = 1
        Mouse5Down = 0
      Case Else
        Return
    End Select
    ClearKeyHold()
    If useAction Is Nothing Then Return
    For Each Key As Keys In useAction
      If Not Key = Keys.None Then keybd_event(Key, MapVirtualKey(Key, 0), KEYEVENTF_KEYUP, UIntPtr.Zero)
    Next
    e.Handled = True
  End Sub

  Private Sub tDetection_Tick(ByVal state As Object)
    If Mouse4Down = 0 And Mouse5Down = 0 Then Return
    If (Not Mouse4Down = 0) And (selButton4Action Is Nothing) Then Return
    If (Not Mouse5Down = 0) And (selButton5Action Is Nothing) Then Return
    Dim useAction As List(Of Keys) = Nothing
    If Mouse4Down = 1 Then
      useAction = New List(Of Keys)
      useAction.AddRange(selButton4Action)
    ElseIf Mouse5Down = 1 Then
      useAction = New List(Of Keys)
      useAction.AddRange(selButton5Action)
    End If
    If Mouse4Down = 2 Then
      useAction.AddRange(selButton4Action)
    ElseIf Mouse5Down = 2 Then
      useAction.AddRange(selButton5Action)
    End If
    If useAction Is Nothing Then Return
    For Each Key As Keys In useAction
      If Not Key = Keys.None Then keybd_event(Key, MapVirtualKey(Key, 0), KEYEVENTF_KEYDOWN, UIntPtr.Zero)
    Next
  End Sub

  Private Sub txtExtra_GotFocus(ByVal sender As TextBox, ByVal e As System.EventArgs) Handles txtButton4.GotFocus, txtButton5.GotFocus
    sender.Tag = sender.Text
    sender.Text = vbNullString
    If IsNothing(kHook) Then kHook = New KeyboardHook()
    kHook.BlockWin = True
  End Sub

  Private Sub txtExtra_KeyDown(ByVal sender As TextBox, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtButton4.KeyDown, txtButton5.KeyDown
    If sender.SelectionLength > 0 Then sender.SelectedText = vbNullString
    If sender.Text.Length > 0 Then
      sender.SelectionStart = 0
      sender.SelectionLength = sender.Text.Length
      sender.SelectedText &= " " & KeyToStr(e.KeyCode)
    Else
      sender.SelectedText &= KeyToStr(e.KeyCode)
    End If
    e.SuppressKeyPress = True
    sender.SelectionLength = 0
  End Sub

  Private Sub txtExtra_LostFocus(ByVal sender As TextBox, ByVal e As System.EventArgs) Handles txtButton4.LostFocus, txtButton5.LostFocus
    If Not IsNothing(kHook) Then
      kHook.BlockWin = False
      kHook = Nothing
    End If
    If Not IsNothing(sender.Tag) And sender.Text = vbNullString Then
      sender.Text = sender.Tag
      sender.Tag = Nothing
    End If
  End Sub

  Private Sub txtButton4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtButton4.TextChanged
    If lvProfiles.SelectedItems.Count > 0 Then
      Dim lvItem As ListViewItem = lvProfiles.SelectedItems(0)
      lvItem.Text = txtButton4.Text
      If loaded Then
        If Not (txtButton4.Tag IsNot Nothing AndAlso String.IsNullOrEmpty(txtButton4.Text)) Then
          cmdSave.Enabled = ChangesMade()
        End If
      End If
    End If
    cmdClearExtra1.Enabled = Not txtButton4.Text = "Disabled"
  End Sub

  Private Sub cmdClearExtra1_DoubleClick(sender As Object, e As System.EventArgs) Handles cmdClearExtra1.Click
    txtButton4.Text = "Disabled"
    txtButton4.Tag = "Disabled"
  End Sub

  Private Sub kHook_Keyboard_Press(sender As Object, ByRef e As KeyEventArgs) Handles kHook.Keyboard_Press
    txtExtra_KeyDown(Me.ActiveControl, e)
  End Sub

  Private Sub txtButton5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtButton5.TextChanged
    If lvProfiles.SelectedItems.Count > 0 Then
      Dim lvItem As ListViewItem = lvProfiles.SelectedItems(0)
      lvItem.SubItems(1).Text = txtButton5.Text
      If loaded Then
        If Not (txtButton5.Tag IsNot Nothing AndAlso String.IsNullOrEmpty(txtButton5.Text)) Then
          cmdSave.Enabled = ChangesMade()
        End If
      End If
    End If
    cmdClearExtra2.Enabled = Not txtButton5.Text = "Disabled"
  End Sub

  Private Sub cmdClearExtra2_DoubleClick(sender As Object, e As System.EventArgs) Handles cmdClearExtra2.Click
    txtButton5.Text = "Disabled"
    txtButton5.Tag = "Disabled"
  End Sub

  Private Sub lvProfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvProfiles.SelectedIndexChanged
    If lvProfiles.SelectedItems.Count > 0 Then
      txtButton4.Enabled = True
      txtButton5.Enabled = True
      cmdClearExtra1.Enabled = Not txtButton4.Text = "Disabled"
      cmdClearExtra2.Enabled = Not txtButton5.Text = "Disabled"
    Else
      txtButton4.Enabled = False
      txtButton5.Enabled = False
      cmdClearExtra1.Enabled = False
      cmdClearExtra2.Enabled = False
    End If
  End Sub

  Private Function GetRegKey(WriteEnabled As Boolean) As Microsoft.Win32.RegistryKey
    If WriteEnabled Then
      If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName) Then My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).CreateSubKey(Application.CompanyName)
      If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey(Application.CompanyName).GetSubKeyNames.Contains(Application.ProductName) Then My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).OpenSubKey(Application.CompanyName, True).CreateSubKey(Application.ProductName)
      Return My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).OpenSubKey(Application.CompanyName, True).OpenSubKey(Application.ProductName, True)
    Else
      If My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName) Then
        If My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey(Application.CompanyName).GetSubKeyNames.Contains(Application.ProductName) Then
          Return My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey(Application.CompanyName).OpenSubKey(Application.ProductName)
        End If
      End If
      Return Nothing
    End If
  End Function

  Private Function GetRegProfiles(myRegKey As Microsoft.Win32.RegistryKey, writeEnabled As Boolean) As Microsoft.Win32.RegistryKey
    Const sProfiles As String = "Profiles"
    If writeEnabled Then
      If myRegKey.SubKeyCount = 0 Then myRegKey.CreateSubKey(sProfiles)
      If Not myRegKey.GetSubKeyNames.Contains(sProfiles) Then myRegKey.CreateSubKey(sProfiles)
      Return myRegKey.OpenSubKey(sProfiles, True)
    Else
      If myRegKey.SubKeyCount > 0 Then
        If myRegKey.GetSubKeyNames.Contains(sProfiles) Then
          Return myRegKey.OpenSubKey(sProfiles)
        End If
      End If
      Return Nothing
    End If
  End Function

  Private Sub DelRegProfiles(myRegKey As Microsoft.Win32.RegistryKey)
    Const sProfiles As String = "Profiles"
    If myRegKey.SubKeyCount > 0 AndAlso myRegKey.GetSubKeyNames.Contains(sProfiles) Then myRegKey.DeleteSubKeyTree(sProfiles)
  End Sub

  Private Function ChangesMade() As Boolean
    Dim myRegKey As Microsoft.Win32.RegistryKey = GetRegKey(False)
    If myRegKey Is Nothing Then Return True
    Dim regEnabled As Boolean = myRegKey.GetValue("Enabled", False)
    If Not chkEnable.Checked = regEnabled Then Return True
    If Not chkStart.Checked = My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("Run").GetValueNames.Contains("MouseManager") Then Return True
    Dim myProfileKey As Microsoft.Win32.RegistryKey = GetRegProfiles(myRegKey, False)
    If lvProfiles.CheckedItems.Count = 0 Then
      If Not myProfileKey.GetValue("Default", -1) = -1 Then Return True
    Else
      If Not myProfileKey.GetValue("Default", 0) = lvProfiles.CheckedIndices(0) Then Return True
    End If
    If myProfileKey Is Nothing Then
      If Not lvProfiles.Items.Count = 0 Then Return True
    Else
      If Not lvProfiles.Items.Count = myProfileKey.SubKeyCount Then Return True
      Dim I As Integer = 0
      For Each Key As String In myProfileKey.GetSubKeyNames()
        If lvProfiles.Items.Count <= I Then Return True
        If Not lvProfiles.Items(I).Text = myProfileKey.OpenSubKey(Key).GetValue("Button4", "PageUp") And
           Not lvProfiles.Items(I).Text = myProfileKey.OpenSubKey(Key).GetValue("Button1", "PageUp") Then Return True
        If Not lvProfiles.Items(I).SubItems(1).Text = myProfileKey.OpenSubKey(Key).GetValue("Button5", "PageDown") And
           Not lvProfiles.Items(I).SubItems(1).Text = myProfileKey.OpenSubKey(Key).GetValue("Button2", "PageDown") Then Return True
        I += 1
      Next
    End If
    Return False
  End Function

  Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
    Dim myRegKey = GetRegKey(True)
    myRegKey.SetValue("Enabled", chkEnable.Checked)
    If chkStart.Checked Then
      My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("Run", True).SetValue("MouseManager", Application.ExecutablePath)
    Else
      My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("Run", True).DeleteValue("MouseManager", False)
    End If
    DelRegProfiles(myRegKey)
    Dim myProfileKey As Microsoft.Win32.RegistryKey = GetRegProfiles(myRegKey, True)
    If lvProfiles.CheckedItems.Count > 0 Then
      If lvProfiles.CheckedIndices.Count > 0 Then
        myProfileKey.SetValue("Default", lvProfiles.CheckedIndices(0))
      Else
        myProfileKey.DeleteValue("Default", False)
      End If
      If String.IsNullOrEmpty(lvProfiles.CheckedItems(0).Text) Then
        selButton4Action = New List(Of Keys)({Keys.None})
      Else
        selButton4Action = StrToKeys(lvProfiles.CheckedItems(0).Text)
      End If
      If String.IsNullOrEmpty(lvProfiles.CheckedItems(0).SubItems(1).Text) Then
        selButton5Action = New List(Of Keys)({Keys.None})
      Else
        selButton5Action = StrToKeys(lvProfiles.CheckedItems(0).SubItems(1).Text)
      End If
    Else
      selButton4Action = Nothing
      selButton5Action = Nothing
      myProfileKey.DeleteValue("Default", False)
    End If
    For I As Integer = 0 To lvProfiles.Items.Count - 1
      Dim sButton4 As String = lvProfiles.Items(I).Text
      Dim sButton5 As String = lvProfiles.Items(I).SubItems(1).Text
      Dim hmd5 As New Security.Cryptography.MD5Cng
      Dim sAID As String = BitConverter.ToString(hmd5.ComputeHash(System.Text.Encoding.GetEncoding(28591).GetBytes(sButton4 & sButton5)), 0, 4).Replace("-", "")
      myProfileKey.CreateSubKey("Profile" & sAID).SetValue("Button4", sButton4)
      myProfileKey.CreateSubKey("Profile" & sAID).SetValue("Button5", sButton5)
    Next
    cmdSave.Enabled = False
    If delKey Then
      If myRegKey.GetSubKeyNames.Contains("1.1.0.0") Then myRegKey.DeleteSubKeyTree("1.1.0.0")
      If myRegKey.GetSubKeyNames.Contains("1.0.0.0") Then myRegKey.DeleteSubKeyTree("1.0.0.0")
    End If
  End Sub

  Private Sub trayIcon_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles trayIcon.MouseClick
    If e.Button = Windows.Forms.MouseButtons.Left Then
      If Me.Visible Then
        Me.Activate()
      Else
        Me.Show()
      End If
    End If
  End Sub

  Private Sub trayIcon_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles trayIcon.MouseDown
    If mnuProfiles.MenuItems.Count > 0 Then mnuProfiles.MenuItems.Clear()
    Dim mnuNoProfile As New MenuItem("No Profile", AddressOf ProfileItem_Click)
    mnuProfiles.MenuItems.Add(mnuNoProfile)
    mnuNoProfile.Checked = Not chkEnable.Checked
    If lvProfiles.Items.Count > 0 Then mnuProfiles.MenuItems.Add(New MenuItem("-"))
    For I As Integer = 0 To lvProfiles.Items.Count - 1
      Dim sButton4 As String = lvProfiles.Items(I).Text
      Dim sButton5 As String = lvProfiles.Items(I).SubItems(1).Text
      If String.IsNullOrEmpty(sButton4) Then sButton4 = "Disabled"
      If String.IsNullOrEmpty(sButton5) Then sButton5 = "Disabled"
      Dim mnuTmp As New MenuItem(sButton4 & "/" & sButton5, AddressOf ProfileItem_Click)
      mnuTmp.Tag = lvProfiles.Items(I)
      mnuProfiles.MenuItems.Add(mnuTmp)
      If Not mnuNoProfile.Checked Then mnuTmp.Checked = lvProfiles.Items(I).Checked
    Next
  End Sub

  Private Sub ProfileItem_Click(ByVal sender As MenuItem, ByVal e As System.EventArgs)
    If sender.Text = "No Profile" Then
      chkEnable.Checked = sender.Checked
      mHook = Nothing
    Else
      chkEnable.Checked = True
      For Each mnuItem As MenuItem In (From mnuTmp In mnuProfiles.MenuItems Where mnuTmp.GetType.ToString = "MenuItem" Select mnuTmp)
        mnuItem.Checked = False
      Next
      sender.Checked = True
      Dim xItem As ListViewItem = sender.Tag
      xItem.Checked = True
      lvProfiles_ItemChecked(sender, New ItemCheckedEventArgs(xItem))
      mHook = New MouseHook
    End If
    cmdSave_Click(sender, e)
  End Sub

  Private Sub tmrInit_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrInit.Tick
    tmrInit.Enabled = False
    Me.Hide()
  End Sub

  Private Sub chkEnable_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnable.CheckedChanged
    If lvProfiles.Items.Count = 0 Then
      chkEnable.Checked = False
      tbsManager.SelectedTab = tabProfiles
      cmdSave.Enabled = ChangesMade()
      Return
    End If
    If chkEnable.Checked Then
      If lvProfiles.CheckedItems.Count = 0 Then
        If lvProfiles.Items.Count > 0 Then
          For Each lvItem As ListViewItem In lvProfiles.Items
            If lvItem.Tag = True Then
              lvItem.Checked = True
              lvItem.Tag = Nothing
            End If
          Next
        End If
      End If
    Else
      For Each lvItem As ListViewItem In lvProfiles.CheckedItems
        If lvItem.Checked Then
          lvItem.Tag = True
          lvItem.Checked = False
        End If
      Next
    End If
    If loaded Then cmdSave.Enabled = ChangesMade()
  End Sub

  Private Function StrToKeys(ByVal sKeys As String) As List(Of Keys)
    Dim cKeys As New List(Of Keys)
    Dim sKeyList() As String = Split(sKeys)
    For Each sKey As String In sKeyList
      Select Case sKey
        Case "A"
          cKeys.Add(Keys.A)
        Case "Add"
          cKeys.Add(Keys.Add)
        Case "Alt"
          cKeys.Add(Keys.Menu)
        Case "Apps"
          cKeys.Add(Keys.Apps)
        Case "Attn"
          cKeys.Add(Keys.Attn)
        Case "B"
          cKeys.Add(Keys.B)
        Case "Backspace"
          cKeys.Add(Keys.Back)
        Case "BrowserBack"
          cKeys.Add(Keys.BrowserBack)
        Case "BrowserFavorites"
          cKeys.Add(Keys.BrowserFavorites)
        Case "BrowserForward"
          cKeys.Add(Keys.BrowserForward)
        Case "BrowserHome"
          cKeys.Add(Keys.BrowserHome)
        Case "BrowserRefresh"
          cKeys.Add(Keys.BrowserRefresh)
        Case "BrowserSearch"
          cKeys.Add(Keys.BrowserSearch)
        Case "BrowserStop"
          cKeys.Add(Keys.BrowserStop)
        Case "C"
          cKeys.Add(Keys.C)
        Case "Cancel"
          cKeys.Add(Keys.Cancel)
        Case "CapsLock"
          cKeys.Add(Keys.CapsLock)
        Case "Clear"
          cKeys.Add(Keys.Clear)
        Case "Control"
          cKeys.Add(Keys.ControlKey)
        Case "Crsel"
          cKeys.Add(Keys.Crsel)
        Case "D"
          cKeys.Add(Keys.D)
        Case "0"
          cKeys.Add(Keys.D0)
        Case "1"
          cKeys.Add(Keys.D1)
        Case "2"
          cKeys.Add(Keys.D2)
        Case "3"
          cKeys.Add(Keys.D3)
        Case "4"
          cKeys.Add(Keys.D4)
        Case "5"
          cKeys.Add(Keys.D5)
        Case "6"
          cKeys.Add(Keys.D6)
        Case "7"
          cKeys.Add(Keys.D7)
        Case "8"
          cKeys.Add(Keys.D8)
        Case "9"
          cKeys.Add(Keys.D9)
        Case "Decimal"
          cKeys.Add(Keys.Decimal)
        Case "Delete"
          cKeys.Add(Keys.Delete)
        Case "Divide"
          cKeys.Add(Keys.Divide)
        Case "Down"
          cKeys.Add(Keys.Down)
        Case "E"
          cKeys.Add(Keys.E)
        Case "End"
          cKeys.Add(Keys.End)
        Case "Enter"
          cKeys.Add(Keys.Enter)
        Case "EraseEOF"
          cKeys.Add(Keys.EraseEof)
        Case "Escape"
          cKeys.Add(Keys.Escape)
        Case "Execute"
          cKeys.Add(Keys.Execute)
        Case "Exsel"
          cKeys.Add(Keys.Exsel)
        Case "F"
          cKeys.Add(Keys.F)
        Case "F1"
          cKeys.Add(Keys.F1)
        Case "F2"
          cKeys.Add(Keys.F2)
        Case "F3"
          cKeys.Add(Keys.F3)
        Case "F4"
          cKeys.Add(Keys.F4)
        Case "F5"
          cKeys.Add(Keys.F5)
        Case "F6"
          cKeys.Add(Keys.F6)
        Case "F7"
          cKeys.Add(Keys.F7)
        Case "F8"
          cKeys.Add(Keys.F8)
        Case "F9"
          cKeys.Add(Keys.F9)
        Case "F10"
          cKeys.Add(Keys.F10)
        Case "F11"
          cKeys.Add(Keys.F11)
        Case "F12"
          cKeys.Add(Keys.F12)
        Case "F13"
          cKeys.Add(Keys.F13)
        Case "F14"
          cKeys.Add(Keys.F14)
        Case "F15"
          cKeys.Add(Keys.F15)
        Case "F16"
          cKeys.Add(Keys.F16)
        Case "F17"
          cKeys.Add(Keys.F17)
        Case "F18"
          cKeys.Add(Keys.F18)
        Case "F19"
          cKeys.Add(Keys.F19)
        Case "F20"
          cKeys.Add(Keys.F20)
        Case "F21"
          cKeys.Add(Keys.F21)
        Case "F22"
          cKeys.Add(Keys.F22)
        Case "F23"
          cKeys.Add(Keys.F23)
        Case "F24"
          cKeys.Add(Keys.F24)
        Case "IME_FinalMode"
          cKeys.Add(Keys.FinalMode)
        Case "G"
          cKeys.Add(Keys.G)
        Case "H"
          cKeys.Add(Keys.H)
        Case "IME_HanguelMode"
          cKeys.Add(Keys.HanguelMode)
        Case "IME_HangulMode"
          cKeys.Add(Keys.HangulMode)
        Case "IME_HanjaMode"
          cKeys.Add(Keys.HanjaMode)
        Case "Help"
          cKeys.Add(Keys.Help)
        Case "Home"
          cKeys.Add(Keys.Home)
        Case "I"
          cKeys.Add(Keys.I)
        Case "IME_Accept"
          cKeys.Add(Keys.IMEAccept)
        Case "IME_Convert"
          cKeys.Add(Keys.IMEConvert)
        Case "IME_ModeChange"
          cKeys.Add(Keys.IMEModeChange)
        Case "IME_Nonconvert"
          cKeys.Add(Keys.IMENonconvert)
        Case "Insert"
          cKeys.Add(Keys.Insert)
        Case "J"
          cKeys.Add(Keys.J)
        Case "IME_JunjaMode"
          cKeys.Add(Keys.JunjaMode)
        Case "K"
          cKeys.Add(Keys.K)
        Case "IME_KanjaMode"
          cKeys.Add(Keys.KanaMode)
        Case "IME_KanjiMode"
          cKeys.Add(Keys.KanjiMode)
        Case "L"
          cKeys.Add(Keys.L)
        Case "LaunchApp1"
          cKeys.Add(Keys.LaunchApplication1)
        Case "LaunchApp2"
          cKeys.Add(Keys.LaunchApplication2)
        Case "LaunchMail"
          cKeys.Add(Keys.LaunchMail)
        Case "Mouse_Left"
          cKeys.Add(Keys.LButton)
        Case "LControl"
          cKeys.Add(Keys.LControlKey)
        Case "Left"
          cKeys.Add(Keys.Left)
        Case "LineFeed"
          cKeys.Add(Keys.LineFeed)
        Case "LMenu"
          cKeys.Add(Keys.LMenu)
        Case "LShift"
          cKeys.Add(Keys.LShiftKey)
        Case "LWin"
          cKeys.Add(Keys.LWin)
        Case "M"
          cKeys.Add(Keys.M)
        Case "Mouse_Middle"
          cKeys.Add(Keys.MButton)
        Case "Media_Next"
          cKeys.Add(Keys.MediaNextTrack)
        Case "Media_PlayPause"
          cKeys.Add(Keys.MediaPlayPause)
        Case "Media_Previous"
          cKeys.Add(Keys.MediaPreviousTrack)
        Case "Media_Stop"
          cKeys.Add(Keys.MediaStop)
        Case "Multiply"
          cKeys.Add(Keys.Multiply)
        Case "N"
          cKeys.Add(Keys.N)
        Case "NumLock"
          cKeys.Add(Keys.NumLock)
        Case "NumPad0"
          cKeys.Add(Keys.NumPad0)
        Case "NumPad1"
          cKeys.Add(Keys.NumPad1)
        Case "NumPad2"
          cKeys.Add(Keys.NumPad2)
        Case "NumPad3"
          cKeys.Add(Keys.NumPad3)
        Case "NumPad4"
          cKeys.Add(Keys.NumPad4)
        Case "NumPad5"
          cKeys.Add(Keys.NumPad5)
        Case "NumPad6"
          cKeys.Add(Keys.NumPad6)
        Case "NumPad7"
          cKeys.Add(Keys.NumPad7)
        Case "NumPad8"
          cKeys.Add(Keys.NumPad8)
        Case "NumPad9"
          cKeys.Add(Keys.NumPad9)
        Case "O"
          cKeys.Add(Keys.O)
        Case "OEM_1"
          cKeys.Add(Keys.Oem1)
        Case "OEM_102"
          cKeys.Add(Keys.Oem102)
        Case "OEM_2"
          cKeys.Add(Keys.Oem2)
        Case "OEM_3"
          cKeys.Add(Keys.Oem3)
        Case "OEM_4"
          cKeys.Add(Keys.Oem4)
        Case "OEM_5"
          cKeys.Add(Keys.Oem5)
        Case "OEM_6"
          cKeys.Add(Keys.Oem6)
        Case "OEM_7"
          cKeys.Add(Keys.Oem7)
        Case "OEM_8"
          cKeys.Add(Keys.Oem8)
        Case "OEM_Backslash"
          cKeys.Add(Keys.OemBackslash)
        Case "OEM_Clear"
          cKeys.Add(Keys.OemClear)
        Case "OEM_CloseBrackets"
          cKeys.Add(Keys.OemCloseBrackets)
        Case "OEM_Comma"
          cKeys.Add(Keys.Oemcomma)
        Case "OEM_Minus"
          cKeys.Add(Keys.OemMinus)
        Case "OEM_OpenBrackets"
          cKeys.Add(Keys.OemOpenBrackets)
        Case "OEM_Period"
          cKeys.Add(Keys.OemPeriod)
        Case "OEM_Pipe"
          cKeys.Add(Keys.OemPipe)
        Case "OEM_Plus"
          cKeys.Add(Keys.Oemplus)
        Case "OEM_Question"
          cKeys.Add(Keys.OemQuestion)
        Case "OEM_Quotes"
          cKeys.Add(Keys.OemQuotes)
        Case "OEM_Semicolon"
          cKeys.Add(Keys.OemSemicolon)
        Case "OEM_Tilde"
          cKeys.Add(Keys.Oemtilde)
        Case "P"
          cKeys.Add(Keys.P)
        Case "Pa1"
          cKeys.Add(Keys.Pa1)
        Case "PageDown"
          cKeys.Add(Keys.PageDown)
        Case "PageUp"
          cKeys.Add(Keys.PageUp)
        Case "Pause"
          cKeys.Add(Keys.Pause)
        Case "Play"
          cKeys.Add(Keys.Play)
        Case "Print"
          cKeys.Add(Keys.Print)
        Case "PrintScreen"
          cKeys.Add(Keys.PrintScreen)
        Case "Process"
          cKeys.Add(Keys.ProcessKey)
        Case "Q"
          cKeys.Add(Keys.Q)
        Case "R"
          cKeys.Add(Keys.R)
        Case "Mouse_Right"
          cKeys.Add(Keys.RButton)
        Case "RControl"
          cKeys.Add(Keys.RControlKey)
        Case "Return"
          cKeys.Add(Keys.Return)
        Case "Right"
          cKeys.Add(Keys.Right)
        Case "RMenu"
          cKeys.Add(Keys.RMenu)
        Case "RShift"
          cKeys.Add(Keys.RShiftKey)
        Case "RWin"
          cKeys.Add(Keys.RWin)
        Case "S"
          cKeys.Add(Keys.S)
        Case "ScrollLock"
          cKeys.Add(Keys.Scroll)
        Case "Select"
          cKeys.Add(Keys.Select)
        Case "SelectMedia"
          cKeys.Add(Keys.SelectMedia)
        Case "Seperator"
          cKeys.Add(Keys.Separator)
        Case "Shift"
          cKeys.Add(Keys.ShiftKey)
        Case "Sleep"
          cKeys.Add(Keys.Sleep)
        Case "Space"
          cKeys.Add(Keys.Space)
        Case "Subtract"
          cKeys.Add(Keys.Subtract)
        Case "T"
          cKeys.Add(Keys.T)
        Case "Tab"
          cKeys.Add(Keys.Tab)
        Case "U"
          cKeys.Add(Keys.U)
        Case "Up"
          cKeys.Add(Keys.Up)
        Case "V"
          cKeys.Add(Keys.V)
        Case "Volume_Down"
          cKeys.Add(Keys.VolumeDown)
        Case "Volume_Mute"
          cKeys.Add(Keys.VolumeMute)
        Case "Volume_Up"
          cKeys.Add(Keys.VolumeUp)
        Case "W"
          cKeys.Add(Keys.W)
        Case "X"
          cKeys.Add(Keys.X)
        Case "Mouse_XButton1"
          cKeys.Add(Keys.XButton1)
        Case "Mouse_XButton2"
          cKeys.Add(Keys.XButton2)
        Case "Y"
          cKeys.Add(Keys.Y)
        Case "Z"
          cKeys.Add(Keys.Z)
        Case "Zoom"
          cKeys.Add(Keys.Zoom)
        Case "Disabled"
          cKeys.Add(Keys.None)
      End Select
    Next
    Return cKeys
  End Function

  Private Function KeyToStr(ByRef Key As Keys) As String
    Select Case Key
      Case Keys.A
        Return "A"
      Case Keys.Add
        Return "Add"
      Case Keys.Alt, Keys.Menu
        Return "Alt"
      Case Keys.Apps
        Return "Apps"
      Case Keys.Attn
        Return "Attn"
      Case Keys.B
        Return "B"
      Case Keys.Back
        Return "Backspace"
      Case Keys.BrowserBack
        Return "BrowserBack"
      Case Keys.BrowserFavorites
        Return "BrowserFavorites"
      Case Keys.BrowserForward
        Return "BrowserForward"
      Case Keys.BrowserHome
        Return "BrowserHome"
      Case Keys.BrowserRefresh
        Return "BrowserRefresh"
      Case Keys.BrowserSearch
        Return "BrowserSearch"
      Case Keys.BrowserStop
        Return "BrowserStop"
      Case Keys.C
        Return "C"
      Case Keys.Cancel
        Return "Cancel"
      Case Keys.CapsLock
        Return "CapsLock"
      Case Keys.Clear
        Return "Clear"
      Case Keys.ControlKey
        Return "Control"
      Case Keys.Crsel
        Return "Crsel"
      Case Keys.D
        Return "D"
      Case Keys.D0
        Return "0"
      Case Keys.D1
        Return "1"
      Case Keys.D2
        Return "2"
      Case Keys.D3
        Return "3"
      Case Keys.D4
        Return "4"
      Case Keys.D5
        Return "5"
      Case Keys.D6
        Return "6"
      Case Keys.D7
        Return "7"
      Case Keys.D8
        Return "8"
      Case Keys.D9
        Return "9"
      Case Keys.Decimal
        Return "Decimal"
      Case Keys.Delete
        Return "Delete"
      Case Keys.Divide
        Return "Divide"
      Case Keys.Down
        Return "Down"
      Case Keys.E
        Return "E"
      Case Keys.End
        Return "End"
      Case Keys.Enter
        Return "Enter"
      Case Keys.EraseEof
        Return "EraseEOF"
      Case Keys.Escape
        Return "Escape"
      Case Keys.Execute
        Return "Execute"
      Case Keys.Exsel
        Return "Exsel"
      Case Keys.F
        Return "F"
      Case Keys.F1
        Return "F1"
      Case Keys.F2
        Return "F2"
      Case Keys.F3
        Return "F3"
      Case Keys.F4
        Return "F4"
      Case Keys.F5
        Return "F5"
      Case Keys.F6
        Return "F6"
      Case Keys.F7
        Return "F7"
      Case Keys.F8
        Return "F8"
      Case Keys.F9
        Return "F9"
      Case Keys.F10
        Return "F10"
      Case Keys.F11
        Return "F11"
      Case Keys.F12
        Return "F12"
      Case Keys.F13
        Return "F13"
      Case Keys.F14
        Return "F14"
      Case Keys.F15
        Return "F15"
      Case Keys.F16
        Return "F16"
      Case Keys.F17
        Return "F17"
      Case Keys.F18
        Return "F18"
      Case Keys.F19
        Return "F19"
      Case Keys.F20
        Return "F20"
      Case Keys.F21
        Return "F21"
      Case Keys.F22
        Return "F22"
      Case Keys.F23
        Return "F23"
      Case Keys.F24
        Return "F24"
      Case Keys.FinalMode
        Return "IME_FinalMode"
      Case Keys.G
        Return "G"
      Case Keys.H
        Return "H"
      Case Keys.HanguelMode
        Return "IME_HanguelMode"
      Case Keys.HangulMode
        Return "IME_HangulMode"
      Case Keys.HanjaMode
        Return "IME_HanjaMode"
      Case Keys.Help
        Return "Help"
      Case Keys.Home
        Return "Home"
      Case Keys.I
        Return "I"
      Case Keys.IMEAccept
        Return "IME_Accept"
      Case Keys.IMEConvert
        Return "IME_Convert"
      Case Keys.IMEModeChange
        Return "IME_ModeChange"
      Case Keys.IMENonconvert
        Return "IME_Nonconvert"
      Case Keys.Insert
        Return "Insert"
      Case Keys.J
        Return "J"
      Case Keys.JunjaMode
        Return "IME_JunjaMode"
      Case Keys.K
        Return "K"
      Case Keys.KanaMode
        Return "IME_KanjaMode"
      Case Keys.KanjiMode
        Return "IME_KanjiMode"
      Case Keys.L
        Return "L"
      Case Keys.LaunchApplication1
        Return "LaunchApp1"
      Case Keys.LaunchApplication2
        Return "LaunchApp2"
      Case Keys.LaunchMail
        Return "LaunchMail"
      Case Keys.LButton
        Return "Mouse_Left"
      Case Keys.LControlKey
        Return "LControl"
      Case Keys.Left
        Return "Left"
      Case Keys.LineFeed
        Return "LineFeed"
      Case Keys.LMenu
        Return "LMenu"
      Case Keys.LShiftKey
        Return "LShift"
      Case Keys.LWin
        Return "LWin"
      Case Keys.M
        Return "M"
      Case Keys.MButton
        Return "Mouse_Middle"
      Case Keys.MediaNextTrack
        Return "Media_Next"
      Case Keys.MediaPlayPause
        Return "Media_PlayPause"
      Case Keys.MediaPreviousTrack
        Return "Media_Previous"
      Case Keys.MediaStop
        Return "Media_Stop"
      Case Keys.Multiply
        Return "Multiply"
      Case Keys.N
        Return "N"
      Case Keys.NumLock
        Return "NumLock"
      Case Keys.NumPad0
        Return "NumPad0"
      Case Keys.NumPad1
        Return "NumPad1"
      Case Keys.NumPad2
        Return "NumPad2"
      Case Keys.NumPad3
        Return "NumPad3"
      Case Keys.NumPad4
        Return "NumPad4"
      Case Keys.NumPad5
        Return "NumPad5"
      Case Keys.NumPad6
        Return "NumPad6"
      Case Keys.NumPad7
        Return "NumPad7"
      Case Keys.NumPad8
        Return "NumPad8"
      Case Keys.NumPad9
        Return "NumPad9"
      Case Keys.O
        Return "O"
      Case Keys.Oem1
        Return "OEM_1"
      Case Keys.Oem102
        Return "OEM_102"
      Case Keys.Oem2
        Return "OEM_2"
      Case Keys.Oem3
        Return "OEM_3"
      Case Keys.Oem4
        Return "OEM_4"
      Case Keys.Oem5
        Return "OEM_5"
      Case Keys.Oem6
        Return "OEM_6"
      Case Keys.Oem7
        Return "OEM_7"
      Case Keys.Oem8
        Return "OEM_8"
      Case Keys.OemBackslash
        Return "OEM_Backslash"
      Case Keys.OemClear
        Return "OEM_Clear"
      Case Keys.OemCloseBrackets
        Return "OEM_CloseBrackets"
      Case Keys.Oemcomma
        Return "OEM_Comma"
      Case Keys.OemMinus
        Return "OEM_Minus"
      Case Keys.OemOpenBrackets
        Return "OEM_OpenBrackets"
      Case Keys.OemPeriod
        Return "OEM_Period"
      Case Keys.OemPipe
        Return "OEM_Pipe"
      Case Keys.Oemplus
        Return "OEM_Plus"
      Case Keys.OemQuestion
        Return "OEM_Question"
      Case Keys.OemQuotes
        Return "OEM_Quotes"
      Case Keys.OemSemicolon
        Return "OEM_Semicolon"
      Case Keys.Oemtilde
        Return "OEM_Tilde"
      Case Keys.P
        Return "P"
      Case Keys.Pa1
        Return "Pa1"
      Case Keys.PageDown
        Return "PageDown"
      Case Keys.PageUp
        Return "PageUp"
      Case Keys.Pause
        Return "Pause"
      Case Keys.Play
        Return "Play"
      Case Keys.Print
        Return "Print"
      Case Keys.PrintScreen
        Return "PrintScreen"
      Case Keys.ProcessKey
        Return "Process"
      Case Keys.Q
        Return "Q"
      Case Keys.R
        Return "R"
      Case Keys.RButton
        Return "Mouse_Right"
      Case Keys.RControlKey
        Return "RControl"
      Case Keys.Return
        Return "Return"
      Case Keys.Right
        Return "Right"
      Case Keys.RMenu
        Return "RMenu"
      Case Keys.RShiftKey
        Return "RShift"
      Case Keys.RWin
        Return "RWin"
      Case Keys.S
        Return "S"
      Case Keys.Scroll
        Return "ScrollLock"
      Case Keys.Select
        Return "Select"
      Case Keys.SelectMedia
        Return "SelectMedia"
      Case Keys.Separator
        Return "Seperator"
      Case Keys.ShiftKey
        Return "Shift"
      Case Keys.Sleep
        Return "Sleep"
      Case Keys.Space
        Return "Space"
      Case Keys.Subtract
        Return "Subtract"
      Case Keys.T
        Return "T"
      Case Keys.Tab
        Return "Tab"
      Case Keys.U
        Return "U"
      Case Keys.Up
        Return "Up"
      Case Keys.V
        Return "V"
      Case Keys.VolumeDown
        Return "Volume_Down"
      Case Keys.VolumeMute
        Return "Volume_Mute"
      Case Keys.VolumeUp
        Return "Volume_Up"
      Case Keys.W
        Return "W"
      Case Keys.X
        Return "X"
      Case Keys.XButton1
        Return "Mouse_XButton1"
      Case Keys.XButton2
        Return "Mouse_XButton2"
      Case Keys.Y
        Return "Y"
      Case Keys.Z
        Return "Z"
      Case Keys.Zoom
        Return "Zoom"
      Case Keys.None
        Return "Disabled"
      Case Else
        Debug.Print(Key.ToString())
        Return Nothing
    End Select
  End Function

  Private Sub lblWebsite_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblWebsite.LinkClicked
    Dim linkURL As String = HomeURL
    If OSSupport.NativeTLS12 Then linkURL = HomeSURL
    Process.Start(linkURL)
  End Sub

  Private Sub cmdDonate_Click(sender As System.Object, e As System.EventArgs) Handles cmdDonate.Click
    Dim linkURL As String = DonateURL
    If OSSupport.NativeTLS12 Then linkURL = DonateSURL
    Process.Start(linkURL)
  End Sub

  Private Sub lblAdvancedWebsite_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblAdvancedWebsite.LinkClicked
    Dim linkURL As String = PurchaseURL
    If OSSupport.NativeTLS12 Then linkURL = PurchaseSURL
    Process.Start(linkURL)
  End Sub

  Private Sub chkStart_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkStart.CheckedChanged
    If loaded Then cmdSave.Enabled = ChangesMade()
  End Sub

  Private Sub tbsManager_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles tbsManager.SelectedIndexChanged
    If tbsManager.SelectedIndex = 1 Then
      lvProfiles.Columns(0).Width = CInt(Math.Floor(lvProfiles.ClientSize.Width / 2) - 1)
      lvProfiles.Columns(1).Width = CInt(Math.Floor(lvProfiles.ClientSize.Width / 2) - 1)
    End If
  End Sub

  Private Sub frmOptions_VisibleChanged(sender As Object, e As System.EventArgs) Handles Me.VisibleChanged
    If Me.Visible Then
      If mHook IsNot Nothing Then mHook = Nothing
    Else
      If chkEnable.Checked Then mHook = New MouseHook
    End If
  End Sub
End Class
