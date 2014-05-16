<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOptions
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
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
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Me.tbsManager = New System.Windows.Forms.TabControl()
    Me.tabSettings = New System.Windows.Forms.TabPage()
    Me.pnlSettings = New System.Windows.Forms.TableLayoutPanel()
    Me.chkStart = New System.Windows.Forms.CheckBox()
    Me.chkEnable = New System.Windows.Forms.CheckBox()
    Me.tabProfiles = New System.Windows.Forms.TabPage()
    Me.pnlProfiles = New System.Windows.Forms.TableLayoutPanel()
    Me.lblButton1 = New System.Windows.Forms.Label()
    Me.cmdRem = New System.Windows.Forms.Button()
    Me.lblButton2 = New System.Windows.Forms.Label()
    Me.lvProfiles = New System.Windows.Forms.ListView()
    Me.colButton1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.colButton2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.txtExtra2 = New System.Windows.Forms.TextBox()
    Me.txtExtra1 = New System.Windows.Forms.TextBox()
    Me.pnlAddRemove = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdAdd = New System.Windows.Forms.Button()
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
    Me.tmrDetection = New System.Windows.Forms.Timer(Me.components)
    Me.tmrInit = New System.Windows.Forms.Timer(Me.components)
    Me.pnlManager = New System.Windows.Forms.TableLayoutPanel()
    Me.tbsManager.SuspendLayout()
    Me.tabSettings.SuspendLayout()
    Me.pnlSettings.SuspendLayout()
    Me.tabProfiles.SuspendLayout()
    Me.pnlProfiles.SuspendLayout()
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
    Me.pnlSettings.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlSettings.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlSettings.Controls.Add(Me.chkStart, 0, 1)
    Me.pnlSettings.Controls.Add(Me.chkEnable, 0, 0)
    Me.pnlSettings.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlSettings.Location = New System.Drawing.Point(3, 3)
    Me.pnlSettings.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlSettings.Name = "pnlSettings"
    Me.pnlSettings.RowCount = 2
    Me.pnlSettings.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.pnlSettings.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.pnlSettings.Size = New System.Drawing.Size(302, 161)
    Me.pnlSettings.TabIndex = 0
    '
    'chkStart
    '
    Me.chkStart.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkStart.AutoSize = True
    Me.chkStart.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkStart.Location = New System.Drawing.Point(3, 111)
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
    Me.chkEnable.Location = New System.Drawing.Point(3, 31)
    Me.chkEnable.Name = "chkEnable"
    Me.chkEnable.Size = New System.Drawing.Size(145, 18)
    Me.chkEnable.TabIndex = 0
    Me.chkEnable.Text = "&Enable Mouse Manager"
    Me.chkEnable.UseVisualStyleBackColor = True
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
    Me.pnlProfiles.Controls.Add(Me.lblButton1, 0, 1)
    Me.pnlProfiles.Controls.Add(Me.cmdRem, 2, 3)
    Me.pnlProfiles.Controls.Add(Me.lblButton2, 0, 2)
    Me.pnlProfiles.Controls.Add(Me.lvProfiles, 0, 0)
    Me.pnlProfiles.Controls.Add(Me.txtExtra2, 1, 2)
    Me.pnlProfiles.Controls.Add(Me.txtExtra1, 1, 1)
    Me.pnlProfiles.Controls.Add(Me.pnlAddRemove, 0, 3)
    Me.pnlProfiles.Controls.Add(Me.cmdAdd, 1, 3)
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
    'lblButton1
    '
    Me.lblButton1.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblButton1.AutoSize = True
    Me.lblButton1.Location = New System.Drawing.Point(3, 90)
    Me.lblButton1.Name = "lblButton1"
    Me.lblButton1.Size = New System.Drawing.Size(77, 13)
    Me.lblButton1.TabIndex = 1
    Me.lblButton1.Text = "Extra Button &1:"
    '
    'cmdRem
    '
    Me.cmdRem.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdRem.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdRem.Location = New System.Drawing.Point(230, 139)
    Me.cmdRem.Name = "cmdRem"
    Me.cmdRem.Size = New System.Drawing.Size(75, 25)
    Me.cmdRem.TabIndex = 6
    Me.cmdRem.TabStop = False
    Me.cmdRem.Text = "&Remove"
    Me.cmdRem.UseVisualStyleBackColor = True
    '
    'lblButton2
    '
    Me.lblButton2.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblButton2.AutoSize = True
    Me.lblButton2.Location = New System.Drawing.Point(3, 116)
    Me.lblButton2.Name = "lblButton2"
    Me.lblButton2.Size = New System.Drawing.Size(77, 13)
    Me.lblButton2.TabIndex = 3
    Me.lblButton2.Text = "Extra Button &2:"
    '
    'lvProfiles
    '
    Me.lvProfiles.CheckBoxes = True
    Me.lvProfiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colButton1, Me.colButton2})
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
    Me.lvProfiles.Size = New System.Drawing.Size(302, 78)
    Me.lvProfiles.TabIndex = 0
    Me.lvProfiles.TabStop = False
    Me.lvProfiles.UseCompatibleStateImageBehavior = False
    Me.lvProfiles.View = System.Windows.Forms.View.Details
    '
    'colButton1
    '
    Me.colButton1.Text = "Extra Button 1"
    Me.colButton1.Width = 125
    '
    'colButton2
    '
    Me.colButton2.Text = "Extra Button 2"
    Me.colButton2.Width = 125
    '
    'txtExtra2
    '
    Me.txtExtra2.AcceptsReturn = True
    Me.txtExtra2.AcceptsTab = True
    Me.pnlProfiles.SetColumnSpan(Me.txtExtra2, 2)
    Me.txtExtra2.Dock = System.Windows.Forms.DockStyle.Fill
    Me.txtExtra2.Enabled = False
    Me.txtExtra2.Location = New System.Drawing.Point(86, 113)
    Me.txtExtra2.Name = "txtExtra2"
    Me.txtExtra2.Size = New System.Drawing.Size(219, 20)
    Me.txtExtra2.TabIndex = 4
    Me.txtExtra2.TabStop = False
    '
    'txtExtra1
    '
    Me.txtExtra1.AcceptsReturn = True
    Me.txtExtra1.AcceptsTab = True
    Me.pnlProfiles.SetColumnSpan(Me.txtExtra1, 2)
    Me.txtExtra1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.txtExtra1.Enabled = False
    Me.txtExtra1.Location = New System.Drawing.Point(86, 87)
    Me.txtExtra1.Name = "txtExtra1"
    Me.txtExtra1.Size = New System.Drawing.Size(219, 20)
    Me.txtExtra1.TabIndex = 2
    Me.txtExtra1.TabStop = False
    '
    'pnlAddRemove
    '
    Me.pnlAddRemove.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.pnlAddRemove.AutoSize = True
    Me.pnlAddRemove.ColumnCount = 2
    Me.pnlAddRemove.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlAddRemove.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlAddRemove.Location = New System.Drawing.Point(83, 151)
    Me.pnlAddRemove.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlAddRemove.Name = "pnlAddRemove"
    Me.pnlAddRemove.RowCount = 1
    Me.pnlAddRemove.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAddRemove.Size = New System.Drawing.Size(0, 0)
    Me.pnlAddRemove.TabIndex = 3
    '
    'cmdAdd
    '
    Me.cmdAdd.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdAdd.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdAdd.Location = New System.Drawing.Point(149, 139)
    Me.cmdAdd.Name = "cmdAdd"
    Me.cmdAdd.Size = New System.Drawing.Size(75, 25)
    Me.cmdAdd.TabIndex = 5
    Me.cmdAdd.TabStop = False
    Me.cmdAdd.Text = "&Add"
    Me.cmdAdd.UseVisualStyleBackColor = True
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
    Me.lblWebsite.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
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
    Me.Icon = Global.MouseManager.My.Resources.Resources.Icon
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
    Me.tabProfiles.ResumeLayout(False)
    Me.pnlProfiles.ResumeLayout(False)
    Me.pnlProfiles.PerformLayout()
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
  Friend WithEvents lblButton1 As System.Windows.Forms.Label
  Friend WithEvents lblButton2 As System.Windows.Forms.Label
  Friend WithEvents lvProfiles As System.Windows.Forms.ListView
  Friend WithEvents tabAbout As System.Windows.Forms.TabPage
  Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents cmdSave As System.Windows.Forms.Button
  Friend WithEvents colButton1 As System.Windows.Forms.ColumnHeader
  Friend WithEvents colButton2 As System.Windows.Forms.ColumnHeader
  Friend WithEvents chkEnable As System.Windows.Forms.CheckBox
  Friend WithEvents pnlAbout As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblAbout As System.Windows.Forms.Label
  Friend WithEvents lblTitle As System.Windows.Forms.Label
  Friend WithEvents tmrDetection As System.Windows.Forms.Timer
  Friend WithEvents txtExtra1 As System.Windows.Forms.TextBox
  Friend WithEvents txtExtra2 As System.Windows.Forms.TextBox
  Friend WithEvents tmrInit As System.Windows.Forms.Timer
  Friend WithEvents lblVersion As System.Windows.Forms.Label
  Friend WithEvents pnlAddRemove As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdAdd As System.Windows.Forms.Button
  Friend WithEvents cmdRem As System.Windows.Forms.Button
  Friend WithEvents lblWebsite As MouseManager.LinkLabel
  Friend WithEvents pnlManager As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdDonate As System.Windows.Forms.Button


End Class
