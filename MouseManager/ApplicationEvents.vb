Namespace My
  Partial Friend Class MyApplication
    Private WithEvents sia As SingleInstance
    Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As ApplicationServices.StartupEventArgs) Handles Me.Startup
      sia = New SingleInstance(frmOptions.Name)
      If Not sia.NewMutex() Then
        sia.Send(e.CommandLine)
        e.Cancel = True
        Return
      End If
      sia.NewPipe()
      Dim v As Authenticode.Validity = Authenticode.IsSelfSigned(Reflection.Assembly.GetExecutingAssembly().Location)
      If Not (v = Authenticode.Validity.SignedAndValid Or v = Authenticode.Validity.SignedButUntrusted) Then
        Dim sErr As String = "0x" & v.ToString("x")
        If Not CStr(v) = v.ToString Then sErr = v.ToString & " (0x" & v.ToString("x") & ")"
        If MsgBox("The Executable """ & IO.Path.GetFileName(Reflection.Assembly.GetExecutingAssembly().Location) & """ is not signed and may be corrupted or modified." & vbNewLine & "Error Code: " & sErr & vbNewLine & vbNewLine & "Would you like to continue loading " & My.Application.Info.ProductName & " anyway?", MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal Or MsgBoxStyle.YesNo, My.Application.Info.ProductName) = MsgBoxResult.No Then
          e.Cancel = True
          Return
        End If
      End If
      If e.CommandLine IsNot Nothing AndAlso e.CommandLine.Count > 0 Then
        If e.CommandLine.Contains("/uninstall") OrElse e.CommandLine.Contains("-uninstall") Then
          cSettings.RemoveAll()
          e.Cancel = True
          Return
        End If
      End If
    End Sub
    Private Sub sia_NewInstanceStartup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles sia.NewInstanceStartup
      If e.CommandLine IsNot Nothing AndAlso e.CommandLine.Count > 0 Then
        If e.CommandLine.Contains("/uninstall") OrElse e.CommandLine.Contains("-uninstall") Then
          cSettings.RemoveAll()
          System.Windows.Forms.Application.Exit()
          Return
        End If
      End If
      e.BringToForeground = True
    End Sub
  End Class
  Friend Class SingleInstance
    Public Event NewInstanceStartup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs)
    Private mutex As Threading.Mutex
    Private pipe As IO.Pipes.NamedPipeServerStream
    Private MutexID As String = Nothing
    Private MainForm As String = Nothing
    Public Sub New(ByVal sMain As String)
      mutex = Nothing
      pipe = Nothing
      MutexID = System.Runtime.InteropServices.Marshal.GetTypeLibGuidForAssembly(System.Reflection.Assembly.GetExecutingAssembly).ToString("D")
      MainForm = sMain
    End Sub
    Public Function NewMutex() As Boolean
      Try
        Dim isNew As Boolean = True
        mutex = New Threading.Mutex(True, MutexID, isNew)
        Return isNew
      Catch ex As Exception
        Return False
      End Try
    End Function
    Public Function NewPipe() As Boolean
      Try
        If pipe IsNot Nothing Then
          pipe.Close()
          pipe.Dispose()
          pipe = Nothing
        End If
        pipe = New IO.Pipes.NamedPipeServerStream(MutexID, IO.Pipes.PipeDirection.In, IO.Pipes.NamedPipeServerStream.MaxAllowedServerInstances, IO.Pipes.PipeTransmissionMode.Message, IO.Pipes.PipeOptions.Asynchronous Or IO.Pipes.PipeOptions.WriteThrough)
        pipe.BeginWaitForConnection(AddressOf Connected, Nothing)
        Return True
      Catch ex As Exception
        Return False
      End Try
    End Function
    Public Function Send(ByVal CommandLine As Collections.ObjectModel.ReadOnlyCollection(Of String)) As Boolean
      Try
        Using sendPipe As New IO.Pipes.NamedPipeClientStream(".", MutexID, IO.Pipes.PipeDirection.Out, IO.Pipes.PipeOptions.Asynchronous Or IO.Pipes.PipeOptions.WriteThrough, Security.Principal.TokenImpersonationLevel.Anonymous)
          sendPipe.Connect(2000)
          If Not sendPipe.IsConnected Then Return False
          Using wStream As New IO.StreamWriter(sendPipe)
            wStream.WriteLine(My.Application.Info.ProductName & " - " & MutexID)
            wStream.WriteLine(CommandLine.Count)
            For Each n As String In CommandLine
              wStream.WriteLine(n)
            Next
          End Using
          sendPipe.Close()
          Return True
        End Using
      Catch ex As Exception
        Return False
      End Try
    End Function
    Private Sub Connected(ByVal ar As IAsyncResult)
      Try
        pipe.EndWaitForConnection(ar)
        Dim CommandLine As New List(Of String)
        Using rStream As New IO.StreamReader(pipe)
          If Not rStream.ReadLine() = My.Application.Info.ProductName & " - " & MutexID Then Return
          Dim sLen As String = rStream.ReadLine()
          Dim iLen As Integer = CInt(sLen)
          Do Until rStream.EndOfStream
            CommandLine.Add(rStream.ReadLine())
          Loop
          If Not iLen = CommandLine.Count Then CommandLine = New List(Of String)
        End Using
        NewPipe()
        Dim e As New Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs(CommandLine.AsReadOnly, False)
        RaiseEvent NewInstanceStartup(Me, e)
        If e.BringToForeground Then
          For Each frm As Form In Application.OpenForms
            If Not frm.Name = MainForm Then Continue For
            If Not frm.Visible Then frm.Show()
            frm.BringToFront()
            frm.Activate()
            Exit For
          Next
        End If
      Catch ex As Exception
      End Try
    End Sub
  End Class
End Namespace
