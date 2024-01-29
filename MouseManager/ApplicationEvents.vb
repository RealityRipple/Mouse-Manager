Namespace My
  Partial Friend Class MyApplication
    Private Sub MyApplication_Startup(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
      Dim v As Authenticode.Validity = Authenticode.IsSelfSigned(Reflection.Assembly.GetExecutingAssembly().Location)
      If Not (v = Authenticode.Validity.SignedAndValid Or v = Authenticode.Validity.SignedButUntrusted) Then
        Dim sErr As String = "0x" & v.ToString("x")
        If Not CStr(v) = v.ToString Then sErr = v.ToString & " (0x" & v.ToString("x") & ")"
        If MsgBox("The Executable """ & IO.Path.GetFileName(Reflection.Assembly.GetExecutingAssembly().Location) & """ is not signed and may be corrupted or modified." & vbNewLine & "Error Code: " & sErr & vbNewLine & vbNewLine & "Would you like to continue loading " & My.Application.Info.ProductName & " anyway?", MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal Or MsgBoxStyle.YesNo, My.Application.Info.ProductName) = MsgBoxResult.No Then
          e.Cancel = True
          Return
        End If
      End If
      If e.CommandLine.Contains("/uninstall") Then
        If My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(My.Application.Info.CompanyName) Then
          If My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey(My.Application.Info.CompanyName).GetSubKeyNames.Contains(My.Application.Info.ProductName) Then
            My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).OpenSubKey(My.Application.Info.CompanyName, True).DeleteSubKeyTree(My.Application.Info.ProductName)
            If My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey(My.Application.Info.CompanyName).SubKeyCount = 0 Then
              My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).DeleteSubKeyTree(My.Application.Info.CompanyName)
            End If
          End If
        End If
        e.Cancel = True
        Return
      End If
    End Sub

    Private Sub MyApplication_StartupNextInstance(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
      If e.CommandLine.Contains("/uninstall") Then
        If My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(My.Application.Info.CompanyName) Then
          If My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey(My.Application.Info.CompanyName).GetSubKeyNames.Contains(My.Application.Info.ProductName) Then
            My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).OpenSubKey(My.Application.Info.CompanyName, True).DeleteSubKeyTree(My.Application.Info.ProductName)
            If My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey(My.Application.Info.CompanyName).SubKeyCount = 0 Then
              My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).DeleteSubKeyTree(My.Application.Info.CompanyName)
            End If
          End If
        End If
        System.Windows.Forms.Application.Exit()
        Return
      End If
      If frmOptions.IsHandleCreated Then
        If Not frmOptions.Visible Then frmOptions.mnuManagement.PerformClick()
        e.BringToForeground = True
      End If
    End Sub
  End Class
End Namespace
