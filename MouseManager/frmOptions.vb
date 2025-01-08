Public Class frmOptions
  Private WithEvents mHook As MouseHook = Nothing
  Private WithEvents kHook As KeyboardHook = Nothing
  Private tDetect As Threading.Timer
  Private Const iDetection As Integer = 250
  Private tPress As Threading.Timer
  Private tRelease As Threading.Timer
  Private selButton4Action As New List(Of Keys)
  Private selButton5Action As New List(Of Keys)
  Private Mouse4State As MSTATE = MSTATE.Null
  Private Mouse5State As MSTATE = MSTATE.Null
  Private Enum MSTATE As Byte
    Null
    Down
    Press
    Up
  End Enum
  Private profArr As List(Of cSettings.PROFILEINFO)
  Private loaded As Boolean = False
  Private noWin As Boolean = False
  Private noTray As Boolean = False
  Private makingChanges As Boolean = False
  Private Const HomeURL As String = "realityripple.com"
  Private Const PurchaseURL As String = "realityripple.com/Software/Applications/Advanced-Mouse-Manager/"
  Private Const DonateURL As String = "realityripple.com/donate.php?itm=Mouse+Manager"
#Region "Form Events"
  Public Sub New()
    Me.Opacity = 0
    InitializeComponent()
    If cSettings.HideTray Then
      noTray = True
    Else
      trayIcon.Visible = True
    End If
  End Sub
  Private Sub frmOptions_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
    lblVersion.Text = "v" & Application.ProductVersion
    lblWebsite.Text = OSSupport.ProtoURL(HomeURL)
    Dim doInit As Boolean = False
    trayIcon.Icon = My.Resources.Icon
    Me.Icon = My.Resources.Icon
    makingChanges = True
    If Not cSettings.IsInstalledIsh Then
      chkStart.ThreeState = False
      chkStart.Checked = False
      chkStart.Enabled = False
    ElseIf cSettings.SomeoneStartsWithWindows Then
      If cSettings.StartWithWindows Then
        chkStart.Checked = True
        chkStart.ThreeState = False
      Else
        chkStart.Checked = True
        chkStart.ThreeState = True
        chkStart.CheckState = CheckState.Indeterminate
      End If
    Else
      chkStart.Checked = False
      chkStart.ThreeState = False
    End If
    LoadProfiles()
    If profArr.Count > 0 Then
      Dim defID As String = cSettings.SelectedProfile
      For Each prof As cSettings.PROFILEINFO In profArr
        Dim lvItem As New ListViewItem
        lvItem.Text = prof.Button4
        lvItem.SubItems.Add(prof.Button5)
        If prof.ID = defID Then
          If cSettings.Enabled Then
            lvItem.Checked = True
          Else
            lvItem.Tag = True
          End If
        Else
          lvItem.Checked = False
          lvItem.Tag = Nothing
        End If
        lvProfiles.Items.Add(lvItem)
      Next
      If lvProfiles.Items.Count > 0 Then doInit = True
    End If
    chkEnable.Checked = cSettings.Enabled
    If chkEnable.Checked Then
      If mHook Is Nothing Then mHook = New MouseHook()
      If tDetect Is Nothing Then tDetect = New Threading.Timer(New Threading.TimerCallback(AddressOf tDetect_Tick), Nothing, 0, iDetection)
    End If
    cmdSave.Enabled = False
    If doInit Then
      tmrInit.Enabled = True
    Else
      Me.Opacity = 1
    End If
    cmdRem.Enabled = False
    makingChanges = False
    loaded = True
  End Sub
  Private Sub tmrInit_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrInit.Tick
    tmrInit.Enabled = False
    Me.Hide()
    Me.Opacity = 1
  End Sub
  Private Sub frmOptions_SizeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Me.SizeChanged
    ResizeProfileColumns()
  End Sub
  Private Sub frmOptions_VisibleChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Me.VisibleChanged
    If Me.Visible Then
      If noTray Then trayIcon.Visible = True
    Else
      If noTray Then trayIcon.Visible = False
      If chkEnable.Checked Then
        If mHook Is Nothing Then mHook = New MouseHook()
        If tDetect Is Nothing Then tDetect = New Threading.Timer(New Threading.TimerCallback(AddressOf tDetect_Tick), Nothing, 0, iDetection)
      End If
    End If
  End Sub
  Private Sub frmOptions_Deactivate(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Deactivate
    If chkEnable.Checked Then
      If mHook Is Nothing Then mHook = New MouseHook()
      If tDetect Is Nothing Then tDetect = New Threading.Timer(New Threading.TimerCallback(AddressOf tDetect_Tick), Nothing, 0, iDetection)
    End If
    If Not IsNothing(kHook) Then kHook.BlockWin = False
  End Sub
  Private Sub frmOptions_Activated(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Activated
    If Me.Visible Then
      If mHook IsNot Nothing Then mHook = Nothing
      If tDetect IsNot Nothing Then
        tDetect.Dispose()
        tDetect = Nothing
      End If
    End If
    If Not IsNothing(kHook) AndAlso (Me.ActiveControl.Name = txtButton4.Name Or Me.ActiveControl.Name = txtButton5.Name) Then kHook.BlockWin = True
  End Sub
  Private Sub frmOptions_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If e.CloseReason = CloseReason.UserClosing And Not My.Computer.Keyboard.ShiftKeyDown Then
      e.Cancel = True
      Me.Hide()
    End If
  End Sub
  Private Sub ResizeProfileColumns()
    If Not tbsManager.SelectedIndex = 1 Then Return
    Dim defWidth As Integer = CInt(Math.Floor((lvProfiles.ClientSize.Width) / 2) - 1)
    If Not lvProfiles.Columns(0).Width = defWidth Then lvProfiles.Columns(0).Width = defWidth
    If Not lvProfiles.Columns(1).Width = defWidth Then lvProfiles.Columns(1).Width = defWidth
  End Sub
#End Region
  Private firstTab As Boolean = True
  Private Sub tbsManager_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbsManager.SelectedIndexChanged
    If tbsManager.SelectedIndex = 1 Then
      If firstTab Then firstTab = False
      ResizeProfileColumns()
    End If
  End Sub
#Region "Settings"
  Private Sub chkEnable_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkEnable.CheckedChanged
    If makingChanges Then Return
    makingChanges = True
    If lvProfiles.Items.Count = 0 Then
      chkEnable.Checked = False
      tbsManager.SelectedTab = tabProfiles
      cmdSave.Enabled = ChangesMade()
      makingChanges = False
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
          If lvProfiles.CheckedItems.Count = 0 Then lvProfiles.Items(0).Checked = True
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
    makingChanges = False
  End Sub
  Private Sub chkStart_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkStart.CheckedChanged
    If Not cSettings.IsInstalledIsh Then
      chkStart.ThreeState = False
      chkStart.Checked = False
      chkStart.Enabled = False
      Return
    End If
    If makingChanges Then Return
    makingChanges = True
    If chkStart.ThreeState Then
      chkStart.ThreeState = False
      chkStart.Checked = True
    End If
    If loaded Then cmdSave.Enabled = ChangesMade()
    makingChanges = False
  End Sub
#End Region
#Region "Profiles"
  Private Sub LoadProfiles()
    profArr = Nothing
    profArr = New List(Of cSettings.PROFILEINFO)
    For Each Key As String In cSettings.ProfileIDs
      Dim pEntry As cSettings.PROFILEINFO = cSettings.Profile(Key)
      If pEntry.ID.StartsWith("Profile") Then pEntry.ID = Key.Substring(7)
      profArr.Add(pEntry)
    Next
  End Sub
  Private Sub lvProfiles_ColumnWidthChanging(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnWidthChangingEventArgs) Handles lvProfiles.ColumnWidthChanging
    e.Cancel = True
    e.NewWidth = lvProfiles.Columns(e.ColumnIndex).Width
  End Sub
  Private Sub lvProfiles_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lvProfiles.ItemCheck
    If makingChanges Then Return
    makingChanges = True
    If Not e.NewValue = CheckState.Checked Then
      For I As Integer = 0 To lvProfiles.Items.Count - 1
        If I = e.Index Then
          lvProfiles.Items(I).Tag = True
        Else
          lvProfiles.Items(I).Tag = Nothing
        End If
      Next
      chkEnable.Checked = False
      If loaded And Not firstTab Then cmdSave.Enabled = ChangesMade()
      makingChanges = False
      Return
    End If
    chkEnable.Checked = True
    For I As Integer = 0 To lvProfiles.Items.Count - 1
      If I = e.Index Then Continue For
      lvProfiles.Items(I).Tag = Nothing
      lvProfiles.Items(I).Checked = False
    Next
    If loaded And Not firstTab Then cmdSave.Enabled = ChangesMade()
    makingChanges = False
  End Sub
  Private Sub lvProfiles_ItemSelectionChanged(ByVal sender As Object, ByVal e As ListViewItemSelectionChangedEventArgs) Handles lvProfiles.ItemSelectionChanged
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
  Private Sub lvProfiles_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lvProfiles.SelectedIndexChanged
    If lvProfiles.SelectedItems.Count > 0 Then
      txtButton4.Enabled = True
      txtButton5.Enabled = True
      cmdClearButton4.Enabled = Not txtButton4.Text = "Disabled"
      cmdClearButton5.Enabled = Not txtButton5.Text = "Disabled"
    Else
      txtButton4.Enabled = False
      txtButton5.Enabled = False
      cmdClearButton4.Enabled = False
      cmdClearButton5.Enabled = False
    End If
  End Sub
  Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAdd.Click
    Dim lvItem As New ListViewItem
    lvItem.Text = "PageUp"
    lvItem.SubItems.Add("PageDown")
    lvProfiles.Items.Add(lvItem)
    lvItem.Selected = True
    txtButton4.Focus()
    If (From lvFind As ListViewItem In lvProfiles.Items Where lvFind.Checked Select lvFind).Count = 0 Then lvItem.Checked = True
    If loaded Then cmdSave.Enabled = ChangesMade()
    ResizeProfileColumns()
  End Sub
  Private Sub cmdRem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdRem.Click
    lvProfiles.Items.Remove(lvProfiles.SelectedItems(0))
    If lvProfiles.Items.Count = 0 Then chkEnable.Checked = False
    If loaded Then cmdSave.Enabled = ChangesMade()
    ResizeProfileColumns()
  End Sub
#Region "Mouse Button Settings"
  Private Sub txtButton_GotFocus(ByVal sender As TextBox, ByVal e As EventArgs) Handles txtButton4.GotFocus, txtButton5.GotFocus
    sender.Tag = sender.Text
    sender.Text = Nothing
    If IsNothing(kHook) Then kHook = New KeyboardHook()
    kHook.BlockWin = True
  End Sub
  Private Sub txtButton_KeyDown(ByVal sender As TextBox, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtButton4.KeyDown, txtButton5.KeyDown
    e.SuppressKeyPress = True
    If sender.Text.Contains(" ") Then
      Dim lStart As Integer = sender.SelectionStart
      Dim lEnd As Integer = sender.SelectionStart + sender.SelectionLength
      Dim sKeys As String() = Split(sender.Text, " ")
      Dim sNew As New List(Of String)
      Dim p As Integer = 0
      Dim newS As Integer = 0
      For I As Integer = 0 To sKeys.Length - 1
        If p >= lEnd Then
          sNew.Add(sKeys(I))
          Continue For
        End If
        p += sKeys(I).Length
        If p <= lStart Then
          p += 1
          newS = p
          sNew.Add(sKeys(I))
          Continue For
        End If
        p += 1
      Next
      sender.Text = Join(sNew.ToArray, " ")
      sender.SelectionStart = newS
    ElseIf sender.SelectionLength > 0 Then
      sender.SelectedText = Nothing
    End If
    If sender.Text.Length > 0 Then
      sender.SelectedText &= " " & KeyToStr(e.KeyCode) & " "
    Else
      sender.Text = KeyToStr(e.KeyCode)
    End If
    sender.Text = Replace(sender.Text, "  ", " ").Trim
    sender.SelectionStart = sender.Text.Length
    sender.SelectionLength = 0
    Select Case sender.Name
      Case txtButton4.Name
        cmdClearButton4.Enabled = Not (sender.Text = "Disabled")
      Case txtButton5.Name
        cmdClearButton5.Enabled = Not (sender.Text = "Disabled")
    End Select
  End Sub
  Private Sub txtButton_LostFocus(ByVal sender As TextBox, ByVal e As EventArgs) Handles txtButton4.LostFocus, txtButton5.LostFocus
    If Not IsNothing(kHook) Then
      kHook.BlockWin = False
      kHook = Nothing
    End If
    If Not IsNothing(sender.Tag) And String.IsNullOrEmpty(sender.Text) Then
      sender.Text = sender.Tag
      sender.Tag = Nothing
    End If
  End Sub
  Private Sub txtButton4_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtButton4.TextChanged
    If lvProfiles.SelectedItems.Count > 0 Then
      Dim lvItem As ListViewItem = lvProfiles.SelectedItems(0)
      If Not lvItem.Text = txtButton4.Text Then
        lvItem.Text = txtButton4.Text
        If loaded Then
          If Not (txtButton4.Tag IsNot Nothing AndAlso String.IsNullOrEmpty(txtButton4.Text)) Then
            cmdSave.Enabled = ChangesMade()
            ResizeProfileColumns()
          End If
        End If
      End If
    End If
    cmdClearButton4.Enabled = Not txtButton4.Text = "Disabled"
  End Sub
  Private Sub cmdClearButton4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdClearButton4.Click
    txtButton4.Text = "Disabled"
    txtButton4.Tag = "Disabled"
  End Sub
  Private Sub txtButton5_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtButton5.TextChanged
    If lvProfiles.SelectedItems.Count > 0 Then
      Dim lvItem As ListViewItem = lvProfiles.SelectedItems(0)
      If Not lvItem.SubItems(1).Text = txtButton5.Text Then
        lvItem.SubItems(1).Text = txtButton5.Text
        If loaded Then
          If Not (txtButton5.Tag IsNot Nothing AndAlso String.IsNullOrEmpty(txtButton5.Text)) Then
            cmdSave.Enabled = ChangesMade()
            ResizeProfileColumns()
          End If
        End If
      End If
    End If
    cmdClearButton5.Enabled = Not txtButton5.Text = "Disabled"
  End Sub
  Private Sub cmdClearButton5_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdClearButton5.Click
    txtButton5.Text = "Disabled"
    txtButton5.Tag = "Disabled"
  End Sub
  Private Sub kHook_Keyboard_Press(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles kHook.Keyboard_Press
    e.Handled = True
    txtButton_KeyDown(Me.ActiveControl, e)
  End Sub
#End Region
#End Region
#Region "Button Hooks"
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
  Private Sub ProcessProfile()
    Dim new4Action As List(Of Keys) = Nothing
    Dim new5Action As List(Of Keys) = Nothing
    Dim defID As String = cSettings.SelectedProfile
    For Each prof As cSettings.PROFILEINFO In profArr
      If Not prof.ID = defID Then Continue For
      new4Action = StrToKeys(prof.Button4)
      new5Action = StrToKeys(prof.Button5)
      Exit For
    Next
    If new4Action Is Nothing Then
      selButton4Action = Nothing
    Else
      selButton4Action = new4Action
    End If
    If new5Action Is Nothing Then
      selButton5Action = Nothing
    Else
      selButton5Action = new5Action
    End If
  End Sub
  Private Sub mHook_Mouse_XButton_Down(ByVal sender As Object, ByVal e As MouseHook.XButtonEventArgs) Handles mHook.Mouse_XButton_Down
    Select Case e.Button
      Case &H10000
        Mouse4State = MSTATE.Down
      Case &H20000
        Mouse5State = MSTATE.Down
      Case Else
        Return
    End Select
    If tPress IsNot Nothing Then
      tPress.Dispose()
      tPress = Nothing
    End If
    tPress = New Threading.Timer(New Threading.TimerCallback(AddressOf tPress_Tick), False, 0, -1)
    e.Handled = True
  End Sub
  Private Sub mHook_Mouse_XButton_Up(ByVal sender As Object, ByVal e As MouseHook.XButtonEventArgs) Handles mHook.Mouse_XButton_Up
    If selButton4Action Is Nothing And selButton5Action Is Nothing Then Return
    Select Case e.Button
      Case &H10000
        If Not (Mouse4State = MSTATE.Down Or Mouse4State = MSTATE.Press) Then Return
        Mouse4State = MSTATE.Up
      Case &H20000
        If Not (Mouse5State = MSTATE.Down Or Mouse5State = MSTATE.Press) Then Return
        Mouse5State = MSTATE.Up
      Case Else
        Return
    End Select
    If tRelease IsNot Nothing Then
      tRelease.Dispose()
      tRelease = Nothing
    End If
    tRelease = New Threading.Timer(New Threading.TimerCallback(AddressOf tRelease_Tick), Nothing, 0, -1)
    e.Handled = True
  End Sub
  Private Sub tDetect_Tick(ByVal state As Object)
    ProcessProfile()
  End Sub
  Private Sub tPress_Tick(ByVal state As Object)
    If tPress Is Nothing Then Return
    Dim downAction As New List(Of Keys)
    Dim pressAction As New List(Of Keys)
    If selButton4Action IsNot Nothing Then
      If Mouse4State = MSTATE.Down Then
        Mouse4State = MSTATE.Press
        downAction.AddRange(selButton4Action)
      ElseIf Mouse4State = MSTATE.Press Then
        pressAction.AddRange(selButton4Action)
      End If
    End If
    If selButton5Action IsNot Nothing Then
      If Mouse5State = MSTATE.Down Then
        Mouse5State = MSTATE.Press
        downAction.AddRange(selButton5Action)
      ElseIf Mouse5State = MSTATE.Press Then
        pressAction.AddRange(selButton5Action)
      End If
    End If
    If downAction.Count = 0 And pressAction.Count = 0 Then Return
    If downAction.Count > 0 Then
      For Each k As Keys In downAction
        Dim ei As UIntPtr = UIntPtr.Zero
        NativeMethods.keybd_event(k, NativeMethods.MapVirtualKey(k, 0), NativeMethods.KEYEVENTF_KEYDOWN, ei)
        Threading.Thread.Sleep(KeyboardRepeat)
      Next
    End If
    If pressAction.Count > 0 Then
      For Each k As Keys In pressAction
        Dim ei As UIntPtr = UIntPtr.Zero
        NativeMethods.keybd_event(k, NativeMethods.MapVirtualKey(k, 0), NativeMethods.KEYEVENTF_KEYDOWN, ei)
      Next
    End If
    Dim kTime = KeyboardDelay
    If state Then kTime = KeyboardRepeat
    If tPress IsNot Nothing Then
      tPress.Dispose()
      tPress = Nothing
    End If
    tPress = New Threading.Timer(New Threading.TimerCallback(AddressOf tPress_Tick), True, kTime, -1)
  End Sub
  Private Sub tRelease_Tick(ByVal state As Object)
    If tRelease Is Nothing Then Return
    Dim upAction As New List(Of Keys)
    If selButton4Action IsNot Nothing Then
      If Mouse4State = MSTATE.Up Then
        Mouse4State = MSTATE.Null
        upAction.AddRange(selButton4Action)
      End If
    End If
    If selButton5Action IsNot Nothing Then
      If Mouse5State = MSTATE.Up Then
        Mouse5State = MSTATE.Null
        upAction.AddRange(selButton5Action)
      End If
    End If
    If upAction.Count > 0 Then
      For Each k As Keys In upAction
        Dim ei As UIntPtr = UIntPtr.Zero
        NativeMethods.keybd_event(k, NativeMethods.MapVirtualKey(k, 0), NativeMethods.KEYEVENTF_KEYUP, ei)
      Next
    End If
  End Sub
#End Region
  Private Sub lblAdvancedWebsite_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblAdvancedWebsite.LinkClicked
    If e.Button = Windows.Forms.MouseButtons.Left Then OpenURL(PurchaseURL)
  End Sub
  Private Sub cmdDonate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDonate.Click
    OpenURL(DonateURL)
  End Sub
  Private Sub lblWebsite_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblWebsite.LinkClicked
    If e.Button = Windows.Forms.MouseButtons.Left Then OpenURL(HomeURL)
  End Sub
#Region "Save/Close Buttons"
  Private Function ChangesMade() As Boolean
    If Not chkEnable.Checked = cSettings.Enabled Then Return True
    If cSettings.IsInstalledIsh Then
      If chkStart.ThreeState Then
        If chkStart.CheckState = CheckState.Indeterminate Then
          If Not cSettings.SomeoneStartsWithWindows Then Return True
          If cSettings.StartWithWindows Then Return True
        Else
          chkStart.ThreeState = False
        End If
      End If
      If Not chkStart.Checked = cSettings.StartWithWindows Then Return True
    End If
    Dim sIDs As String() = cSettings.ProfileIDs
    If Not lvProfiles.Items.Count = sIDs.Length Then Return True
    Dim I As Integer = 0
    Dim bChecked As Boolean = False
    For Each Key As String In sIDs
      If lvProfiles.Items.Count <= I Then Return True
      Dim pInfo As cSettings.PROFILEINFO = cSettings.Profile(Key)
      If lvProfiles.Items(I).Checked Then bChecked = True
      If cSettings.Enabled Then
        If Not lvProfiles.Items(I).Checked = (cSettings.SelectedProfile = Key) Then Return True
      Else
        If cSettings.SelectedProfile = Key Then
          If Not lvProfiles.Items(I).Tag = True Then Return True
        Else
          If Not IsNothing(lvProfiles.Items(I).Tag) Then Return True
        End If
      End If
      If Not lvProfiles.Items(I).Text = pInfo.Button4 Then Return True
      If Not lvProfiles.Items(I).SubItems(1).Text = pInfo.Button5 Then Return True
      I += 1
    Next
    Return False
  End Function
  Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
    cSettings.Enabled = chkEnable.Checked
    If cSettings.IsInstalledIsh AndAlso Not chkStart.ThreeState Then cSettings.StartWithWindows = chkStart.Checked
    Dim savedIDs As New List(Of String)
    For I As Integer = 0 To lvProfiles.Items.Count - 1
      Dim sButton4 As String = lvProfiles.Items(I).Text
      Dim sButton5 As String = lvProfiles.Items(I).SubItems(1).Text
      Dim iChecked As Integer = IIf(lvProfiles.Items(I).Checked, 1, 0)
      Dim hmd5 As New Security.Cryptography.HMACMD5
      Dim sAID As String = BitConverter.ToString(hmd5.ComputeHash(System.Text.Encoding.GetEncoding(28591).GetBytes(sButton4 & sButton5)), 0, 4).Replace("-", "")
      savedIDs.Add(sAID)
      cSettings.Profile(sAID) = New cSettings.PROFILEINFO(sAID, sButton4, sButton5)
      If iChecked Then cSettings.SelectedProfile = sAID
    Next
    For Each pID As String In cSettings.ProfileIDs
      If Not savedIDs.Contains(pID) Then cSettings.RemoveProfile(pID)
    Next
    cmdSave.Enabled = False
    ResizeProfileColumns()
    LoadProfiles()
  End Sub
  Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdClose.Click
    If My.Computer.Keyboard.ShiftKeyDown Then
      Application.Exit()
    Else
      Me.Hide()
    End If
  End Sub
#End Region
#Region "Tray Icon"
  Private TrayDown As Long = -1
  Private Sub trayIcon_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles trayIcon.MouseDown
    If e.Button = Windows.Forms.MouseButtons.Left Then
      TrayDown = TickCount()
    Else
      TrayDown = -1
    End If
    If mnuProfiles.MenuItems.Count > 0 Then mnuProfiles.MenuItems.Clear()
    Dim mnuNoProfile As New MenuItem("No Profile", AddressOf ProfileItem_Click)
    mnuProfiles.MenuItems.Add(mnuNoProfile)
    mnuNoProfile.Checked = Not chkEnable.Checked
    If lvProfiles.Items.Count > 0 Then mnuProfiles.MenuItems.Add(New MenuItem("-"))
    For I As Integer = 0 To lvProfiles.Items.Count - 1
      Dim sButton4 As String = lvProfiles.Items(I).Text
      Dim sButton5 As String = lvProfiles.Items(I).SubItems(1).Text
      Dim bEnabled As Boolean = lvProfiles.Items(I).Checked
      If mnuNoProfile.Checked Then bEnabled = False
      If String.IsNullOrEmpty(sButton4) Then sButton4 = "Disabled"
      If String.IsNullOrEmpty(sButton5) Then sButton5 = "Disabled"
      Dim mnuTmp As MenuItem = mnuProfiles.MenuItems.Add(sButton4 & "/" & sButton5, AddressOf ProfileItem_Click)
      mnuTmp.Tag = lvProfiles.Items(I)
      mnuTmp.Checked = bEnabled
    Next
  End Sub
  Private Sub ProfileItem_Click(ByVal sender As MenuItem, ByVal e As EventArgs)
    If sender.Text = "No Profile" Then
      chkEnable.Checked = sender.Checked
      chkEnable_CheckedChanged(sender, e)
      cmdSave_Click(sender, e)
      If mHook IsNot Nothing Then mHook = Nothing
      If tDetect IsNot Nothing Then
        tDetect.Dispose()
        tDetect = Nothing
      End If
    Else
      Dim xItem As ListViewItem = sender.Tag
      For Each mnuItem As MenuItem In (From mnuTmp In mnuProfiles.MenuItems Where mnuTmp.GetType.ToString = "MenuItem" Select mnuTmp)
        mnuItem.Checked = False
      Next
      sender.Checked = True
      chkEnable.Checked = True
      Dim wasChecked As Boolean = xItem.Checked
      xItem.Checked = True
      lvProfiles_ItemCheck(sender, New ItemCheckEventArgs(xItem.Index, IIf(sender.Checked, CheckState.Checked, CheckState.Unchecked), IIf(wasChecked, CheckState.Checked, CheckState.Unchecked)))
      cmdSave_Click(sender, e)
      If mHook Is Nothing Then mHook = New MouseHook()
      If tDetect Is Nothing Then tDetect = New Threading.Timer(New Threading.TimerCallback(AddressOf tDetect_Tick), Nothing, 0, iDetection)
    End If
  End Sub
  Private Sub trayIcon_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles trayIcon.MouseUp
    If Not e.Button = Windows.Forms.MouseButtons.Left Then Return
    If TrayDown = -1 Then Return
    If TickCount() - TrayDown > 300 Then Return
    TrayDown = -1
    If Not Me.Visible Then Me.Show()
    Me.Activate()
  End Sub
  Private Sub mnuManagement_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuManagement.Click
    Me.Show()
  End Sub
  Private Sub mnuExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuExit.Click
    Application.Exit()
  End Sub
#End Region
#Region "Helpful Routines"
  Private Shared Function TickCount() As Long
    Return (Stopwatch.GetTimestamp / Stopwatch.Frequency) * 1000
  End Function
  Private Function StrToKeys(ByVal sKeys As String) As List(Of Keys)
    Dim cKeys As New List(Of Keys)
    Dim sKeyList As String() = Split(sKeys)
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
  Private Function KeyToStr(ByVal Key As Keys) As String
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
  Public Sub OpenURL(ByVal sURL As String)
    Dim sOpen As String = OSSupport.ProtoURL(sURL)
    Try
      Process.Start(sOpen)
    Catch ex As Exception
      Try
        Clipboard.SetText(sOpen)
      Catch ex2 As Exception
        MsgBox("Failed to open your Browser." & vbNewLine & vbNewLine & My.Application.Info.ProductName & " could not navigate to """ & sURL & """!" & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical, Application.ProductName)
        Return
      End Try
      MsgBox("Failed to open your Browser." & vbNewLine & vbNewLine & My.Application.Info.ProductName & " could not navigate to """ & sURL & """, so the URL was copied to your clipboard!", MsgBoxStyle.Information, Application.ProductName)
    End Try
  End Sub
#End Region
End Class
