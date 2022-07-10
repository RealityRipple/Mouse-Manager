<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmOptions
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()>
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()>
  Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOptions))
    Me.tbsManager = New System.Windows.Forms.TabControl()
    Me.tabSettings = New System.Windows.Forms.TabPage()
    Me.pnlSettings = New System.Windows.Forms.TableLayoutPanel()
    Me.chkStart = New System.Windows.Forms.CheckBox()
    Me.chkEnable = New System.Windows.Forms.CheckBox()
    Me.pnlAdvanced = New System.Windows.Forms.TableLayoutPanel()
    Me.lblAdvanced = New System.Windows.Forms.Label()
    Me.lblAdvancedWebsite = New MouseManager.LinkLabel()
    Me.tabProfiles = New System.Windows.Forms.TabPage()
    Me.pnlProfiles = New System.Windows.Forms.TableLayoutPanel()
    Me.lblButton4 = New System.Windows.Forms.Label()
    Me.lblButton5 = New System.Windows.Forms.Label()
    Me.lvProfiles = New System.Windows.Forms.ListView()
    Me.colButton4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.colButton5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.txtButton5 = New System.Windows.Forms.TextBox()
    Me.txtButton4 = New System.Windows.Forms.TextBox()
    Me.pnlAddRemove = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdAdd = New System.Windows.Forms.Button()
    Me.cmdRem = New System.Windows.Forms.Button()
    Me.cmdClearButton4 = New System.Windows.Forms.Button()
    Me.cmdClearButton5 = New System.Windows.Forms.Button()
    Me.tabAbout = New System.Windows.Forms.TabPage()
    Me.pnlAbout = New System.Windows.Forms.TableLayoutPanel()
    Me.lblTitle = New System.Windows.Forms.Label()
    Me.lblVersion = New System.Windows.Forms.Label()
    Me.lblAbout = New System.Windows.Forms.Label()
    Me.cmdDonate = New System.Windows.Forms.Button()
    Me.lblWebsite = New MouseManager.LinkLabel()
    Me.mnuTray = New System.Windows.Forms.ContextMenu()
    Me.mnuProfiles = New System.Windows.Forms.MenuItem()
    Me.mnuManagement = New System.Windows.Forms.MenuItem()
    Me.mnuSeparator = New System.Windows.Forms.MenuItem()
    Me.mnuExit = New System.Windows.Forms.MenuItem()
    Me.trayIcon = New System.Windows.Forms.NotifyIcon(Me.components)
    Me.cmdClose = New System.Windows.Forms.Button()
    Me.cmdSave = New System.Windows.Forms.Button()
    Me.tmrInit = New System.Windows.Forms.Timer(Me.components)
    Me.pnlManager = New System.Windows.Forms.TableLayoutPanel()
    Me.tbsManager.SuspendLayout()
    Me.tabSettings.SuspendLayout()
    Me.pnlSettings.SuspendLayout()
    Me.pnlAdvanced.SuspendLayout()
    Me.tabProfiles.SuspendLayout()
    Me.pnlProfiles.SuspendLayout()
    Me.pnlAddRemove.SuspendLayout()
    Me.tabAbout.SuspendLayout()
    Me.pnlAbout.SuspendLayout()
    Me.pnlManager.SuspendLayout()
    Me.SuspendLayout()
    '
    'tbsManager
    '
    Me.tbsManager.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlManager.SetColumnSpan(Me.tbsManager, 2)
    Me.tbsManager.Controls.Add(Me.tabSettings)
    Me.tbsManager.Controls.Add(Me.tabProfiles)
    Me.tbsManager.Controls.Add(Me.tabAbout)
    Me.tbsManager.Location = New System.Drawing.Point(9, 9)
    Me.tbsManager.Name = "tbsManager"
    Me.tbsManager.SelectedIndex = 0
    Me.tbsManager.Size = New System.Drawing.Size(316, 193)
    Me.tbsManager.TabIndex = 0
    Me.tbsManager.TabStop = False
    '
    'tabSettings
    '
    Me.tabSettings.Controls.Add(Me.pnlSettings)
    Me.tabSettings.Location = New System.Drawing.Point(4, 22)
    Me.tabSettings.Name = "tabSettings"
    Me.tabSettings.Padding = New System.Windows.Forms.Padding(3)
    Me.tabSettings.Size = New System.Drawing.Size(308, 167)
    Me.tabSettings.TabIndex = 0
    Me.tabSettings.Text = "Settings"
    Me.tabSettings.UseVisualStyleBackColor = True
    '
    'pnlSettings
    '
    Me.pnlSettings.ColumnCount = 1
    Me.pnlSettings.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlSettings.Controls.Add(Me.chkStart, 0, 1)
    Me.pnlSettings.Controls.Add(Me.chkEnable, 0, 0)
    Me.pnlSettings.Controls.Add(Me.pnlAdvanced, 0, 2)
    Me.pnlSettings.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlSettings.Location = New System.Drawing.Point(3, 3)
    Me.pnlSettings.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlSettings.Name = "pnlSettings"
    Me.pnlSettings.RowCount = 3
    Me.pnlSettings.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlSettings.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlSettings.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlSettings.Size = New System.Drawing.Size(302, 161)
    Me.pnlSettings.TabIndex = 0
    '
    'chkStart
    '
    Me.chkStart.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkStart.AutoSize = True
    Me.chkStart.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkStart.Location = New System.Drawing.Point(3, 70)
    Me.chkStart.Name = "chkStart"
    Me.chkStart.Size = New System.Drawing.Size(123, 18)
    Me.chkStart.TabIndex = 1
    Me.chkStart.Text = "Start with &Windows"
    Me.chkStart.UseVisualStyleBackColor = True
    '
    'chkEnable
    '
    Me.chkEnable.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkEnable.AutoSize = True
    Me.chkEnable.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkEnable.Location = New System.Drawing.Point(3, 17)
    Me.chkEnable.Name = "chkEnable"
    Me.chkEnable.Size = New System.Drawing.Size(145, 18)
    Me.chkEnable.TabIndex = 0
    Me.chkEnable.Text = "&Enable Mouse Manager"
    Me.chkEnable.UseVisualStyleBackColor = True
    '
    'pnlAdvanced
    '
    Me.pnlAdvanced.ColumnCount = 1
    Me.pnlAdvanced.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAdvanced.Controls.Add(Me.lblAdvanced, 0, 0)
    Me.pnlAdvanced.Controls.Add(Me.lblAdvancedWebsite, 0, 1)
    Me.pnlAdvanced.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlAdvanced.Location = New System.Drawing.Point(3, 109)
    Me.pnlAdvanced.Name = "pnlAdvanced"
    Me.pnlAdvanced.RowCount = 2
    Me.pnlAdvanced.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvanced.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAdvanced.Size = New System.Drawing.Size(296, 49)
    Me.pnlAdvanced.TabIndex = 2
    '
    'lblAdvanced
    '
    Me.lblAdvanced.AutoSize = True
    Me.lblAdvanced.Location = New System.Drawing.Point(3, 0)
    Me.lblAdvanced.Name = "lblAdvanced"
    Me.lblAdvanced.Size = New System.Drawing.Size(235, 26)
    Me.lblAdvanced.TabIndex = 0
    Me.lblAdvanced.Text = "For more features such as Application Profiles," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Automatic Updates, and Translati" & _
    "ons, check out"
    '
    'lblAdvancedWebsite
    '
    Me.lblAdvancedWebsite.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblAdvancedWebsite.AutoSize = True
    Me.lblAdvancedWebsite.Cursor = System.Windows.Forms.Cursors.Hand
    Me.lblAdvancedWebsite.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline)
    Me.lblAdvancedWebsite.ForeColor = System.Drawing.Color.MediumBlue
    Me.lblAdvancedWebsite.Location = New System.Drawing.Point(157, 31)
    Me.lblAdvancedWebsite.Name = "lblAdvancedWebsite"
    Me.lblAdvancedWebsite.Size = New System.Drawing.Size(136, 13)
    Me.lblAdvancedWebsite.TabIndex = 1
    Me.lblAdvancedWebsite.Text = "Advanced Mouse Manager"
    '
    'tabProfiles
    '
    Me.tabProfiles.Controls.Add(Me.pnlProfiles)
    Me.tabProfiles.Location = New System.Drawing.Point(4, 22)
    Me.tabProfiles.Name = "tabProfiles"
    Me.tabProfiles.Size = New System.Drawing.Size(308, 167)
    Me.tabProfiles.TabIndex = 1
    Me.tabProfiles.Text = "Profiles"
    Me.tabProfiles.UseVisualStyleBackColor = True
    '
    'pnlProfiles
    '
    Me.pnlProfiles.ColumnCount = 3
    Me.pnlProfiles.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlProfiles.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlProfiles.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlProfiles.Controls.Add(Me.lblButton4, 0, 1)
    Me.pnlProfiles.Controls.Add(Me.lblButton5, 0, 2)
    Me.pnlProfiles.Controls.Add(Me.lvProfiles, 0, 0)
    Me.pnlProfiles.Controls.Add(Me.txtButton5, 1, 2)
    Me.pnlProfiles.Controls.Add(Me.txtButton4, 1, 1)
    Me.pnlProfiles.Controls.Add(Me.pnlAddRemove, 0, 3)
    Me.pnlProfiles.Controls.Add(Me.cmdClearButton4, 2, 1)
    Me.pnlProfiles.Controls.Add(Me.cmdClearButton5, 2, 2)
    Me.pnlProfiles.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlProfiles.Location = New System.Drawing.Point(0, 0)
    Me.pnlProfiles.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlProfiles.Name = "pnlProfiles"
    Me.pnlProfiles.RowCount = 4
    Me.pnlProfiles.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlProfiles.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProfiles.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProfiles.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProfiles.Size = New System.Drawing.Size(308, 167)
    Me.pnlProfiles.TabIndex = 0
    '
    'lblButton4
    '
    Me.lblButton4.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblButton4.AutoSize = True
    Me.lblButton4.Location = New System.Drawing.Point(3, 83)
    Me.lblButton4.Name = "lblButton4"
    Me.lblButton4.Size = New System.Drawing.Size(85, 13)
    Me.lblButton4.TabIndex = 1
    Me.lblButton4.Text = "Mouse Button &4:"
    '
    'lblButton5
    '
    Me.lblButton5.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblButton5.AutoSize = True
    Me.lblButton5.Location = New System.Drawing.Point(3, 114)
    Me.lblButton5.Name = "lblButton5"
    Me.lblButton5.Size = New System.Drawing.Size(85, 13)
    Me.lblButton5.TabIndex = 4
    Me.lblButton5.Text = "Mouse Button &5:"
    '
    'lvProfiles
    '
    Me.lvProfiles.CheckBoxes = True
    Me.lvProfiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colButton4, Me.colButton5})
    Me.pnlProfiles.SetColumnSpan(Me.lvProfiles, 3)
    Me.lvProfiles.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lvProfiles.FullRowSelect = True
    Me.lvProfiles.GridLines = True
    Me.lvProfiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
    Me.lvProfiles.HideSelection = False
    Me.lvProfiles.Location = New System.Drawing.Point(3, 3)
    Me.lvProfiles.MultiSelect = False
    Me.lvProfiles.Name = "lvProfiles"
    Me.lvProfiles.ShowGroups = False
    Me.lvProfiles.Size = New System.Drawing.Size(302, 68)
    Me.lvProfiles.TabIndex = 0
    Me.lvProfiles.TabStop = False
    Me.lvProfiles.UseCompatibleStateImageBehavior = False
    Me.lvProfiles.View = System.Windows.Forms.View.Details
    '
    'colButton4
    '
    Me.colButton4.Text = "Button 4"
    Me.colButton4.Width = 150
    '
    'colButton5
    '
    Me.colButton5.Text = "Button 5"
    Me.colButton5.Width = 150
    '
    'txtButton5
    '
    Me.txtButton5.AcceptsReturn = True
    Me.txtButton5.AcceptsTab = True
    Me.txtButton5.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtButton5.Enabled = False
    Me.txtButton5.Location = New System.Drawing.Point(94, 110)
    Me.txtButton5.Name = "txtButton5"
    Me.txtButton5.ShortcutsEnabled = False
    Me.txtButton5.Size = New System.Drawing.Size(183, 20)
    Me.txtButton5.TabIndex = 5
    Me.txtButton5.TabStop = False
    '
    'txtButton4
    '
    Me.txtButton4.AcceptsReturn = True
    Me.txtButton4.AcceptsTab = True
    Me.txtButton4.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtButton4.Enabled = False
    Me.txtButton4.Location = New System.Drawing.Point(94, 79)
    Me.txtButton4.Name = "txtButton4"
    Me.txtButton4.ShortcutsEnabled = False
    Me.txtButton4.Size = New System.Drawing.Size(183, 20)
    Me.txtButton4.TabIndex = 2
    Me.txtButton4.TabStop = False
    '
    'pnlAddRemove
    '
    Me.pnlAddRemove.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.pnlAddRemove.AutoSize = True
    Me.pnlAddRemove.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlAddRemove.ColumnCount = 2
    Me.pnlProfiles.SetColumnSpan(Me.pnlAddRemove, 3)
    Me.pnlAddRemove.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlAddRemove.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlAddRemove.Controls.Add(Me.cmdAdd, 0, 0)
    Me.pnlAddRemove.Controls.Add(Me.cmdRem, 1, 0)
    Me.pnlAddRemove.Location = New System.Drawing.Point(146, 136)
    Me.pnlAddRemove.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlAddRemove.Name = "pnlAddRemove"
    Me.pnlAddRemove.RowCount = 1
    Me.pnlAddRemove.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAddRemove.Size = New System.Drawing.Size(162, 31)
    Me.pnlAddRemove.TabIndex = 3
    '
    'cmdAdd
    '
    Me.cmdAdd.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdAdd.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdAdd.Location = New System.Drawing.Point(3, 3)
    Me.cmdAdd.Name = "cmdAdd"
    Me.cmdAdd.Size = New System.Drawing.Size(75, 25)
    Me.cmdAdd.TabIndex = 7
    Me.cmdAdd.TabStop = False
    Me.cmdAdd.Text = "&Add"
    Me.cmdAdd.UseVisualStyleBackColor = True
    '
    'cmdRem
    '
    Me.cmdRem.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdRem.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdRem.Location = New System.Drawing.Point(84, 3)
    Me.cmdRem.Name = "cmdRem"
    Me.cmdRem.Size = New System.Drawing.Size(75, 25)
    Me.cmdRem.TabIndex = 8
    Me.cmdRem.TabStop = False
    Me.cmdRem.Text = "&Remove"
    Me.cmdRem.UseVisualStyleBackColor = True
    '
    'cmdClearButton4
    '
    Me.cmdClearButton4.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdClearButton4.Enabled = False
    Me.cmdClearButton4.Image = Global.MouseManager.My.Resources.Resources.clr
    Me.cmdClearButton4.Location = New System.Drawing.Point(280, 77)
    Me.cmdClearButton4.Margin = New System.Windows.Forms.Padding(0, 3, 3, 3)
    Me.cmdClearButton4.Name = "cmdClearButton4"
    Me.cmdClearButton4.Size = New System.Drawing.Size(25, 25)
    Me.cmdClearButton4.TabIndex = 3
    Me.cmdClearButton4.TabStop = False
    Me.cmdClearButton4.UseVisualStyleBackColor = True
    '
    'cmdClearButton5
    '
    Me.cmdClearButton5.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdClearButton5.Enabled = False
    Me.cmdClearButton5.Image = Global.MouseManager.My.Resources.Resources.clr
    Me.cmdClearButton5.Location = New System.Drawing.Point(280, 108)
    Me.cmdClearButton5.Margin = New System.Windows.Forms.Padding(0, 3, 3, 3)
    Me.cmdClearButton5.Name = "cmdClearButton5"
    Me.cmdClearButton5.Size = New System.Drawing.Size(25, 25)
    Me.cmdClearButton5.TabIndex = 6
    Me.cmdClearButton5.TabStop = False
    Me.cmdClearButton5.UseVisualStyleBackColor = True
    '
    'tabAbout
    '
    Me.tabAbout.Controls.Add(Me.pnlAbout)
    Me.tabAbout.Location = New System.Drawing.Point(4, 22)
    Me.tabAbout.Name = "tabAbout"
    Me.tabAbout.Size = New System.Drawing.Size(308, 167)
    Me.tabAbout.TabIndex = 2
    Me.tabAbout.Text = "About"
    Me.tabAbout.UseVisualStyleBackColor = True
    '
    'pnlAbout
    '
    Me.pnlAbout.ColumnCount = 1
    Me.pnlAbout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAbout.Controls.Add(Me.lblTitle, 0, 0)
    Me.pnlAbout.Controls.Add(Me.lblVersion, 0, 1)
    Me.pnlAbout.Controls.Add(Me.lblAbout, 0, 2)
    Me.pnlAbout.Controls.Add(Me.cmdDonate, 0, 3)
    Me.pnlAbout.Controls.Add(Me.lblWebsite, 0, 4)
    Me.pnlAbout.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlAbout.Location = New System.Drawing.Point(0, 0)
    Me.pnlAbout.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlAbout.Name = "pnlAbout"
    Me.pnlAbout.RowCount = 5
    Me.pnlAbout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAbout.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAbout.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAbout.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAbout.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAbout.Size = New System.Drawing.Size(308, 167)
    Me.pnlAbout.TabIndex = 1
    '
    'lblTitle
    '
    Me.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblTitle.AutoSize = True
    Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblTitle.Location = New System.Drawing.Point(9, 6)
    Me.lblTitle.Name = "lblTitle"
    Me.lblTitle.Size = New System.Drawing.Size(289, 50)
    Me.lblTitle.TabIndex = 0
    Me.lblTitle.Text = "Mouse Manager"
    Me.lblTitle.UseMnemonic = False
    '
    'lblVersion
    '
    Me.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblVersion.AutoSize = True
    Me.lblVersion.Location = New System.Drawing.Point(277, 67)
    Me.lblVersion.Margin = New System.Windows.Forms.Padding(3, 5, 3, 10)
    Me.lblVersion.Name = "lblVersion"
    Me.lblVersion.Size = New System.Drawing.Size(28, 13)
    Me.lblVersion.TabIndex = 1
    Me.lblVersion.Text = "v1.0"
    Me.lblVersion.UseMnemonic = False
    '
    'lblAbout
    '
    Me.lblAbout.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblAbout.AutoSize = True
    Me.lblAbout.Location = New System.Drawing.Point(90, 95)
    Me.lblAbout.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
    Me.lblAbout.Name = "lblAbout"
    Me.lblAbout.Size = New System.Drawing.Size(128, 13)
    Me.lblAbout.TabIndex = 2
    Me.lblAbout.Text = "by RealityRipple Software"
    Me.lblAbout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    Me.lblAbout.UseMnemonic = False
    '
    'cmdDonate
    '
    Me.cmdDonate.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdDonate.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdDonate.Location = New System.Drawing.Point(195, 116)
    Me.cmdDonate.Name = "cmdDonate"
    Me.cmdDonate.Size = New System.Drawing.Size(110, 25)
    Me.cmdDonate.TabIndex = 3
    Me.cmdDonate.TabStop = False
    Me.cmdDonate.Text = "Make a Donation"
    Me.cmdDonate.UseVisualStyleBackColor = True
    '
    'lblWebsite
    '
    Me.lblWebsite.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblWebsite.AutoSize = True
    Me.lblWebsite.Cursor = System.Windows.Forms.Cursors.Hand
    Me.lblWebsite.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline)
    Me.lblWebsite.ForeColor = System.Drawing.Color.MediumBlue
    Me.lblWebsite.Location = New System.Drawing.Point(97, 149)
    Me.lblWebsite.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
    Me.lblWebsite.Name = "lblWebsite"
    Me.lblWebsite.Size = New System.Drawing.Size(113, 13)
    Me.lblWebsite.TabIndex = 3
    Me.lblWebsite.TabStop = True
    Me.lblWebsite.Text = "http://realityripple.com"
    '
    'mnuTray
    '
    Me.mnuTray.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuProfiles, Me.mnuManagement, Me.mnuSeparator, Me.mnuExit})
    '
    'mnuProfiles
    '
    Me.mnuProfiles.Index = 0
    Me.mnuProfiles.Text = "&Profiles"
    '
    'mnuManagement
    '
    Me.mnuManagement.Index = 1
    Me.mnuManagement.Text = "&Management..."
    '
    'mnuSeparator
    '
    Me.mnuSeparator.Index = 2
    Me.mnuSeparator.Text = "-"
    '
    'mnuExit
    '
    Me.mnuExit.Index = 3
    Me.mnuExit.Text = "E&xit"
    '
    'trayIcon
    '
    Me.trayIcon.ContextMenu = Me.mnuTray
    Me.trayIcon.Text = "Mouse Manager"
    Me.trayIcon.Visible = True
    '
    'cmdClose
    '
    Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClose.Location = New System.Drawing.Point(250, 208)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Size = New System.Drawing.Size(75, 25)
    Me.cmdClose.TabIndex = 2
    Me.cmdClose.TabStop = False
    Me.cmdClose.Text = "&Close"
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'cmdSave
    '
    Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdSave.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdSave.Location = New System.Drawing.Point(169, 208)
    Me.cmdSave.Name = "cmdSave"
    Me.cmdSave.Size = New System.Drawing.Size(75, 25)
    Me.cmdSave.TabIndex = 1
    Me.cmdSave.TabStop = False
    Me.cmdSave.Text = "&Save"
    Me.cmdSave.UseVisualStyleBackColor = True
    '
    'tmrInit
    '
    '
    'pnlManager
    '
    Me.pnlManager.ColumnCount = 2
    Me.pnlManager.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlManager.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlManager.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlManager.Controls.Add(Me.cmdClose, 1, 1)
    Me.pnlManager.Controls.Add(Me.cmdSave, 0, 1)
    Me.pnlManager.Controls.Add(Me.tbsManager, 0, 0)
    Me.pnlManager.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlManager.Location = New System.Drawing.Point(0, 0)
    Me.pnlManager.Name = "pnlManager"
    Me.pnlManager.Padding = New System.Windows.Forms.Padding(6)
    Me.pnlManager.RowCount = 2
    Me.pnlManager.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlManager.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlManager.Size = New System.Drawing.Size(334, 242)
    Me.pnlManager.TabIndex = 3
    '
    'frmOptions
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(334, 242)
    Me.Controls.Add(Me.pnlManager)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.MinimumSize = New System.Drawing.Size(350, 280)
    Me.Name = "frmOptions"
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Mouse Manager"
    Me.tbsManager.ResumeLayout(False)
    Me.tabSettings.ResumeLayout(False)
    Me.pnlSettings.ResumeLayout(False)
    Me.pnlSettings.PerformLayout()
    Me.pnlAdvanced.ResumeLayout(False)
    Me.pnlAdvanced.PerformLayout()
    Me.tabProfiles.ResumeLayout(False)
    Me.pnlProfiles.ResumeLayout(False)
    Me.pnlProfiles.PerformLayout()
    Me.pnlAddRemove.ResumeLayout(False)
    Me.tabAbout.ResumeLayout(False)
    Me.pnlAbout.ResumeLayout(False)
    Me.pnlAbout.PerformLayout()
    Me.pnlManager.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents tbsManager As System.Windows.Forms.TabControl
  Friend WithEvents tabSettings As System.Windows.Forms.TabPage
  Friend WithEvents tabProfiles As System.Windows.Forms.TabPage
  Friend WithEvents mnuTray As System.Windows.Forms.ContextMenu
  Friend WithEvents trayIcon As System.Windows.Forms.NotifyIcon
  Friend WithEvents mnuProfiles As System.Windows.Forms.MenuItem
  Friend WithEvents mnuManagement As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSeparator As System.Windows.Forms.MenuItem
  Friend WithEvents mnuExit As System.Windows.Forms.MenuItem
  Friend WithEvents pnlSettings As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents chkStart As System.Windows.Forms.CheckBox
  Friend WithEvents pnlProfiles As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblButton4 As System.Windows.Forms.Label
  Friend WithEvents lblButton5 As System.Windows.Forms.Label
  Friend WithEvents lvProfiles As System.Windows.Forms.ListView
  Friend WithEvents tabAbout As System.Windows.Forms.TabPage
  Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents cmdSave As System.Windows.Forms.Button
  Friend WithEvents colButton4 As System.Windows.Forms.ColumnHeader
  Friend WithEvents colButton5 As System.Windows.Forms.ColumnHeader
  Friend WithEvents chkEnable As System.Windows.Forms.CheckBox
  Friend WithEvents pnlAbout As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblAbout As System.Windows.Forms.Label
  Friend WithEvents lblTitle As System.Windows.Forms.Label
  Friend WithEvents txtButton4 As System.Windows.Forms.TextBox
  Friend WithEvents txtButton5 As System.Windows.Forms.TextBox
  Friend WithEvents tmrInit As System.Windows.Forms.Timer
  Friend WithEvents lblVersion As System.Windows.Forms.Label
  Friend WithEvents pnlAddRemove As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdAdd As System.Windows.Forms.Button
  Friend WithEvents cmdRem As System.Windows.Forms.Button
  Friend WithEvents lblWebsite As MouseManager.LinkLabel
  Friend WithEvents pnlManager As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdDonate As System.Windows.Forms.Button
  Friend WithEvents cmdClearButton4 As System.Windows.Forms.Button
  Friend WithEvents cmdClearButton5 As System.Windows.Forms.Button
  Friend WithEvents pnlAdvanced As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblAdvanced As System.Windows.Forms.Label
  Friend WithEvents lblAdvancedWebsite As MouseManager.LinkLabel
End Class
