Public Class cSettings
  Public Structure PROFILEINFO
    Public ID As String
    Public Button4 As String
    Public Button5 As String
    Public Sub New(ByVal sID As String, ByVal sButton4 As String, ByVal sButton5 As String)
      ID = sID
      Button4 = sButton4
      Button5 = sButton5
    End Sub
  End Structure
  Private Shared useReg As TriState = TriState.UseDefault
  Private Class cDefault
    Public Structure DefaultValue
      Public [Return] As Object
      Public Registry As KeyValuePair(Of String, Object)
      Public File As KeyValuePair(Of String, KeyValuePair(Of String, String))
      Public Sub New(ByVal ret As Object, ByVal regKey As String, ByVal regVal As Object, ByVal iniGroup As String, ByVal iniKey As String, ByVal iniVal As String)
        [Return] = ret
        Registry = New KeyValuePair(Of String, Object)(regKey, regVal)
        File = New KeyValuePair(Of String, KeyValuePair(Of String, String))(iniGroup, New KeyValuePair(Of String, String)(iniKey, iniVal))
      End Sub
    End Structure
    Public Shared ReadOnly Property HideTray As DefaultValue
      Get
        Return New DefaultValue(False, "HideTray", 0, "Settings", "HideTray", "N")
      End Get
    End Property
    Public Shared ReadOnly Property Enabled As DefaultValue
      Get
        Return New DefaultValue(False, "", 0, "Settings", "Enabled", "N")
      End Get
    End Property
    Public Shared ReadOnly Property SelectedProfile As DefaultValue
      Get
        Return New DefaultValue(Nothing, "", "", "Settings", "Profile", "")
      End Get
    End Property
    Public Shared ReadOnly Property ProfileIDs As String()
      Get
        Return (New List(Of String)).ToArray
      End Get
    End Property
    Public Shared ReadOnly Property Profile(ByVal ID As String) As PROFILEINFO
      Get
        Return Nothing
      End Get
    End Property
    Public Class ProfileEntry
      Public Shared ReadOnly Property Button4 As DefaultValue
        Get
          Return New DefaultValue("PageUp", "Button4", "PageUp", Nothing, "Button4", "PageUp")
        End Get
      End Property
      Public Shared ReadOnly Property Button5 As DefaultValue
        Get
          Return New DefaultValue("PageDown", "Button5", "PageDown", Nothing, "Button5", "PageDown")
        End Get
      End Property
    End Class
  End Class
  Private Class cRegistry
    Private Const sProfiles As String = "Profiles"
    Private Shared Function saveToRegistry(Optional ByVal writable As Boolean = False) As Microsoft.Win32.RegistryKey
      Try
        If Not My.Computer.Registry.CurrentUser.GetSubKeyNames.Contains("Software", StringComparer.OrdinalIgnoreCase) Then My.Computer.Registry.CurrentUser.CreateSubKey("Software")
        If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName) Then My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).CreateSubKey(Application.CompanyName)
        If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey(Application.CompanyName).GetSubKeyNames.Contains(Application.ProductName) Then My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).OpenSubKey(Application.CompanyName, True).CreateSubKey(Application.ProductName)
        Return My.Computer.Registry.CurrentUser.OpenSubKey("Software", writable).OpenSubKey(Application.CompanyName, writable).OpenSubKey(Application.ProductName, writable)
      Catch ex As Exception
        Return Nothing
      End Try
    End Function
    Public Shared ReadOnly Property HideTray As Boolean
      Get
        Try
          Dim myRegKey As Microsoft.Win32.RegistryKey = saveToRegistry(False)
          If myRegKey Is Nothing Then Return cDefault.HideTray.Return
          If Not myRegKey.GetValueNames.Contains(cDefault.HideTray.Registry.Key) Then Return cDefault.HideTray.Return
          If Not myRegKey.GetValueKind(cDefault.HideTray.Registry.Key) = Microsoft.Win32.RegistryValueKind.DWord Then Return cDefault.HideTray.Return
          Return myRegKey.GetValue(cDefault.HideTray.Registry.Key, cDefault.HideTray.Registry.Value) = 1
        Catch ex As Exception
          Return cDefault.HideTray.Return
        End Try
      End Get
    End Property
    Private Shared Sub cleanLegacyEnabled(ByVal change As Boolean)
      Try
        Dim myRegKey As Microsoft.Win32.RegistryKey = saveToRegistry(True)
        If myRegKey Is Nothing Then Return
        If Not myRegKey.GetValueNames.Contains("Enabled") Then Return
        If change AndAlso myRegKey.GetValueKind("Enabled") = Microsoft.Win32.RegistryValueKind.String Then
          Dim state As String = myRegKey.GetValue("Enabled", "Unset")
          If state = "Enabled" Then
            myRegKey.SetValue("", 1, Microsoft.Win32.RegistryValueKind.DWord)
          Else
            myRegKey.SetValue("", 0, Microsoft.Win32.RegistryValueKind.DWord)
          End If
        End If
        myRegKey.DeleteValue("Enabled")
      Catch ex As Exception
      End Try
    End Sub
    Public Shared Property Enabled As Boolean
      Get
        Try
          cleanLegacyEnabled(True)
          Dim myRegKey As Microsoft.Win32.RegistryKey = saveToRegistry(False)
          If myRegKey Is Nothing Then Return cDefault.Enabled.Return
          If Not myRegKey.GetValueNames.Contains(cDefault.Enabled.Registry.Key) Then Return cDefault.Enabled.Return
          If Not myRegKey.GetValueKind(cDefault.Enabled.Registry.Key) = Microsoft.Win32.RegistryValueKind.DWord Then Return cDefault.Enabled.Return
          Return myRegKey.GetValue(cDefault.Enabled.Registry.Key, cDefault.Enabled.Registry.Value) = 1
        Catch ex As Exception
          Return cDefault.Enabled.Return
        End Try
      End Get
      Set(ByVal value As Boolean)
        Try
          cleanLegacyEnabled(False)
          Dim myRegKey As Microsoft.Win32.RegistryKey = saveToRegistry(True)
          If myRegKey Is Nothing Then Return
          myRegKey.SetValue(cDefault.Enabled.Registry.Key, IIf(value, 1, 0), Microsoft.Win32.RegistryValueKind.DWord)
        Catch ex As Exception
        End Try
      End Set
    End Property
    Public Shared Property SelectedProfile As String
      Get
        Try
          Dim myRegKey As Microsoft.Win32.RegistryKey = saveToRegistry(False)
          If myRegKey Is Nothing Then Return cDefault.SelectedProfile.Return
          If Not myRegKey.GetSubKeyNames.Contains(sProfiles) Then Return cDefault.SelectedProfile.Return
          If Not myRegKey.OpenSubKey(sProfiles).GetValueNames.Contains(cDefault.SelectedProfile.Registry.Key) Then Return cDefault.SelectedProfile.Return
          If Not myRegKey.OpenSubKey(sProfiles).GetValueKind(cDefault.SelectedProfile.Registry.Key) = Microsoft.Win32.RegistryValueKind.String Then Return cDefault.SelectedProfile.Return
          Return myRegKey.OpenSubKey(sProfiles).GetValue(cDefault.SelectedProfile.Registry.Key, cDefault.SelectedProfile.Registry.Value)
        Catch ex As Exception
          Return cDefault.SelectedProfile.Return
        End Try
      End Get
      Set(ByVal value As String)
        Try
          Dim myRegKey As Microsoft.Win32.RegistryKey = saveToRegistry(True)
          If myRegKey Is Nothing Then Return
          If Not myRegKey.GetSubKeyNames.Contains(sProfiles) Then myRegKey.CreateSubKey(sProfiles)
          myRegKey.OpenSubKey(sProfiles, True).SetValue(cDefault.SelectedProfile.Registry.Key, value, Microsoft.Win32.RegistryValueKind.String)
        Catch ex As Exception
        End Try
      End Set
    End Property
    Public Shared ReadOnly Property ProfileIDs As String()
      Get
        Try
          Dim myRegKey As Microsoft.Win32.RegistryKey = saveToRegistry(False)
          If myRegKey Is Nothing Then Return cDefault.ProfileIDs
          If Not myRegKey.GetSubKeyNames.Contains(sProfiles) Then Return cDefault.ProfileIDs
          Return myRegKey.OpenSubKey(sProfiles).GetSubKeyNames
        Catch ex As Exception
          Return cDefault.ProfileIDs
        End Try
      End Get
    End Property
    Public Shared Property Profile(ByVal ID As String) As PROFILEINFO
      Get
        Try
          cleanLegacyButtons(ID, True)
          Dim myRegKey As Microsoft.Win32.RegistryKey = saveToRegistry(False)
          If myRegKey Is Nothing Then Return cDefault.Profile(ID)
          If Not myRegKey.GetSubKeyNames.Contains(sProfiles) Then Return cDefault.Profile(ID)
          If Not myRegKey.OpenSubKey(sProfiles).GetSubKeyNames.Contains(ID) Then Return cDefault.Profile(ID)
          Dim pKey As Microsoft.Win32.RegistryKey = myRegKey.OpenSubKey(sProfiles).OpenSubKey(ID)
          Dim b4 As String = cDefault.ProfileEntry.Button4.Registry.Value
          Try
            b4 = IIf(pKey.GetValueNames.Contains(cDefault.ProfileEntry.Button4.Registry.Key) AndAlso pKey.GetValueKind(cDefault.ProfileEntry.Button4.Registry.Key) = Microsoft.Win32.RegistryValueKind.String, pKey.GetValue(cDefault.ProfileEntry.Button4.Registry.Key, cDefault.ProfileEntry.Button4.Registry.Value), cDefault.ProfileEntry.Button4.Registry.Value)
          Catch ex As Exception
          End Try
          Dim b5 As String = cDefault.ProfileEntry.Button5.Registry.Value
          Try
            b5 = IIf(pKey.GetValueNames.Contains(cDefault.ProfileEntry.Button5.Registry.Key) AndAlso pKey.GetValueKind(cDefault.ProfileEntry.Button5.Registry.Key) = Microsoft.Win32.RegistryValueKind.String, pKey.GetValue(cDefault.ProfileEntry.Button5.Registry.Key, cDefault.ProfileEntry.Button5.Registry.Value), cDefault.ProfileEntry.Button5.Registry.Value)
          Catch ex As Exception
          End Try
          Return New PROFILEINFO(ID, b4, b5)
        Catch ex As Exception
          Return cDefault.Profile(ID)
        End Try
      End Get
      Set(ByVal value As PROFILEINFO)
        Try
          cleanLegacyButtons(ID, False)
          Dim myRegKey As Microsoft.Win32.RegistryKey = saveToRegistry(True)
          If myRegKey Is Nothing Then Return
          If Not myRegKey.GetSubKeyNames.Contains(sProfiles) Then myRegKey.CreateSubKey(sProfiles)
          If Not myRegKey.OpenSubKey(sProfiles).GetSubKeyNames.Contains(ID) Then myRegKey.OpenSubKey(sProfiles, True).CreateSubKey(ID)
          Dim pKey As Microsoft.Win32.RegistryKey = myRegKey.OpenSubKey(sProfiles).OpenSubKey(ID, True)
          pKey.SetValue(cDefault.ProfileEntry.Button4.Registry.Key, value.Button4, Microsoft.Win32.RegistryValueKind.String)
          pKey.SetValue(cDefault.ProfileEntry.Button5.Registry.Key, value.Button5, Microsoft.Win32.RegistryValueKind.String)
        Catch ex As Exception
        End Try
      End Set
    End Property
    Public Shared Sub RemoveProfile(ByVal ID As String)
      Try
        Dim myRegKey As Microsoft.Win32.RegistryKey = saveToRegistry(True)
        If myRegKey Is Nothing Then Return
        If Not myRegKey.GetSubKeyNames.Contains(sProfiles) Then Return
        If Not myRegKey.OpenSubKey(sProfiles).GetSubKeyNames.Contains(ID) Then Return
        myRegKey.OpenSubKey(sProfiles, True).DeleteSubKeyTree(ID, False)
        If SelectedProfile = ID Then SelectedProfile = ""
      Catch ex As Exception
      End Try
    End Sub
    Public Shared Sub RemoveAll()
      Try
        If Not My.Computer.Registry.CurrentUser.GetSubKeyNames.Contains("Software", StringComparer.OrdinalIgnoreCase) Then Return
        If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName) Then Return
        If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey(Application.CompanyName).GetSubKeyNames.Contains(Application.ProductName) Then Return
        My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey(Application.CompanyName, True).DeleteSubKeyTree(Application.ProductName, False)
        If My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey(Application.CompanyName).SubKeyCount = 0 Then My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).DeleteSubKeyTree(Application.CompanyName, False)
      Catch ex As Exception
      End Try
    End Sub
    Private Shared Sub cleanLegacyButtons(ByVal ID As String, ByVal change As Boolean)
      Try
        Dim myRegKey As Microsoft.Win32.RegistryKey = saveToRegistry(True)
        If myRegKey Is Nothing Then Return
        If Not myRegKey.GetSubKeyNames.Contains(sProfiles) Then Return
        If Not myRegKey.OpenSubKey(sProfiles).GetSubKeyNames.Contains(ID) Then Return
        Dim pKey As Microsoft.Win32.RegistryKey = myRegKey.OpenSubKey(sProfiles).OpenSubKey(ID, True)
        If pKey.GetValueNames.Contains("Button1") AndAlso pKey.GetValueKind("Button1") = Microsoft.Win32.RegistryValueKind.String AndAlso Not pKey.GetValue("Button1", "Unset") = "Unset" Then
          If change Then pKey.SetValue(cDefault.ProfileEntry.Button4.Registry.Key, pKey.GetValue("Button1", "Unset"), Microsoft.Win32.RegistryValueKind.String)
          pKey.DeleteValue("Button1")
        End If
        If pKey.GetValueNames.Contains("Button2") AndAlso pKey.GetValueKind("Button2") = Microsoft.Win32.RegistryValueKind.String AndAlso Not pKey.GetValue("Button2", "Unset") = "Unset" Then
          If change Then pKey.SetValue(cDefault.ProfileEntry.Button5.Registry.Key, pKey.GetValue("Button2", "Unset"), Microsoft.Win32.RegistryValueKind.String)
          pKey.DeleteValue("Button2")
        End If
      Catch ex As Exception
      End Try
    End Sub
  End Class
  Private Class cFile
    Private Shared storedPath As String = Nothing
    Private Shared showedError As Boolean = False
    Private Shared saveLock As New Object
    Private Shared profLock As New Object
    Private Shared tSave As Threading.Timer = Nothing
    Private Shared tRead As Threading.Timer = Nothing
    Private Shared eLATIN As System.Text.Encoding = System.Text.Encoding.GetEncoding(28591)
    Private Const saveWait As Integer = 500
    Private Const readInterval As Integer = 120000
    Private Const Unix2000 As Int64 = &H386D4380L
    Private Shared ReadOnly Property uNix As Int64
      Get
        Return DateDiff(DateInterval.Second, New Date(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), Date.UtcNow)
      End Get
    End Property
    Private Shared m_PossiblePaths As String() = Nothing
    Private Shared ReadOnly Property PossiblePaths As String()
      Get
        If m_PossiblePaths IsNot Nothing Then Return m_PossiblePaths
        Dim lPaths As New List(Of String)
        If IsInstalled Then
          If Not String.IsNullOrEmpty(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) Then lPaths.Add(IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.CompanyName, Application.ProductName))
          If Not String.IsNullOrEmpty(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) Then lPaths.Add(IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.CompanyName, Application.ProductName))
          If Not String.IsNullOrEmpty(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) Then lPaths.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
          If Not String.IsNullOrEmpty(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)) Then lPaths.Add(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
        ElseIf IsLocal Then
          lPaths.Add(My.Application.Info.DirectoryPath)
          If Not String.IsNullOrEmpty(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) Then lPaths.Add(IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.CompanyName, Application.ProductName))
          If Not String.IsNullOrEmpty(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) Then lPaths.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
          If Not String.IsNullOrEmpty(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)) Then lPaths.Add(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
        Else
          lPaths.Add(My.Application.Info.DirectoryPath)
          lPaths.Add(IO.Path.GetPathRoot(My.Application.Info.DirectoryPath))
        End If
        m_PossiblePaths = lPaths.ToArray
        Return m_PossiblePaths
      End Get
    End Property
    Private Shared Function getLatestINI() As String
      Dim lLatest As Long = 0
      Dim dLatest As String = "[Settings]" & vbNewLine
      For I As Integer = 0 To PossiblePaths.Length - 1
        Dim sTest As String = IO.Path.Combine(PossiblePaths(I), CleanProductName & ".ini")
        If Not IO.File.Exists(sTest) Then Continue For
        Dim uTest As Long = GetINITimestamp(sTest)
        If uTest < Unix2000 Then Continue For
        If uTest < lLatest Then Continue For
        Dim dTest As String = Nothing
        Try
          dTest = IO.File.ReadAllText(sTest, eLATIN)
        Catch ex As Exception
          Continue For
        End Try
        If String.IsNullOrEmpty(dTest) Then Continue For
        dLatest = dTest
        lLatest = uTest
      Next
      Return dLatest
    End Function
    Private Shared Function testWritableINI(ByVal sPath As String) As Boolean
      Dim oPath As String() = sPath.Split(IO.Path.DirectorySeparatorChar)
      Dim testPath As String = ""
      For P As Integer = 0 To oPath.Length - 1
        If String.IsNullOrEmpty(testPath) Then
          testPath = oPath(P) & IO.Path.DirectorySeparatorChar
        Else
          testPath = IO.Path.Combine(testPath, oPath(P))
        End If
        If Not IO.Directory.Exists(testPath) Then IO.Directory.CreateDirectory(testPath)
      Next
      testPath = IO.Path.Combine(testPath, CleanProductName & ".ini")
      Const sEmpty As String = "[Settings]" & vbNewLine & "Dummy=1" & vbNewLine
      If Not IO.File.Exists(testPath) Then
        Try
          IO.File.WriteAllText(testPath, sEmpty, eLATIN)
          Dim bSuccess As Boolean = IO.File.ReadAllText(testPath, eLATIN) = sEmpty
          IO.File.Delete(testPath)
          Return bSuccess
        Catch ex As Exception
          Return False
        End Try
      End If
      Dim sOld As String = IO.File.ReadAllText(testPath, eLATIN)
      Dim pOld As String = IO.Path.ChangeExtension(testPath, "bak")
      Try
        IO.File.Move(testPath, pOld)
        IO.File.WriteAllText(testPath, sEmpty, eLATIN)
        Dim bSuccessA As Boolean = IO.File.ReadAllText(testPath, eLATIN) = sEmpty
        IO.File.Delete(testPath)
        IO.File.Move(pOld, testPath)
        Dim bSuccessB As Boolean = IO.File.ReadAllText(testPath, eLATIN) = sOld
        Return bSuccessA And bSuccessB
      Catch ex As Exception
        Return False
      End Try
    End Function
    Private Shared Function saveToINI() As String
      If Not String.IsNullOrEmpty(storedPath) Then Return storedPath
      storedPath = "NO"
      Try
        Dim dLatest As String = getLatestINI()
        For I As Integer = 0 To PossiblePaths.Length - 1
          If Not testWritableINI(PossiblePaths(I)) Then Continue For
          Try
            Dim savePath As String = IO.Path.Combine(PossiblePaths(I), CleanProductName & ".ini")
            IO.File.WriteAllText(savePath, dLatest, eLATIN)
            If Not IO.File.ReadAllText(savePath, eLATIN) = dLatest Then Continue For
            storedPath = savePath
            Exit For
          Catch ex As Exception
            Continue For
          End Try
        Next
      Catch ex As Exception
      Finally
        CleanOldINIs()
      End Try
      Return storedPath
    End Function
    Private Shared Sub CleanOldINIs()
      Try
        If String.IsNullOrEmpty(storedPath) Then Return
        If storedPath = "NO" Then Return
        For I As Integer = 0 To PossiblePaths.Count - 1
          Dim sTest As String = IO.Path.Combine(PossiblePaths(I), CleanProductName & ".ini")
          If sTest = storedPath Then Continue For
          If Not IO.File.Exists(sTest) Then Continue For
          FileSafeDelete(sTest)
        Next
      Catch ex As Exception
      End Try
    End Sub
    Public Shared ReadOnly Property CanSave As Boolean
      Get
        Return Not saveToINI() = "NO"
      End Get
    End Property
    Private Shared Function INIRead(ByVal INIPath As String, ByVal SectionName As String, ByVal KeyName As String, ByVal DefaultValue As String) As String
      Try
        Dim sData As String = Space(1024)
        Dim n As Integer = NativeMethods.GetPrivateProfileStringW(SectionName, KeyName, DefaultValue, sData, sData.Length, INIPath)
        If n > 0 Then Return sData.Substring(0, n)
      Catch ex As Exception
      End Try
      Return DefaultValue
    End Function
    Private Shared Function GetValidLng(ByVal s As String) As Int64
      Dim dS As String = s.Where(Function(c As Char) As Boolean
                                   Return Char.IsDigit(c)
                                 End Function).ToArray()
      Try
        Dim uS As UInt64 = CULng(dS)
        If uS > Int64.MaxValue Then Return 0
        Return CLng(uS)
      Catch ex As Exception
        Return 0
      End Try
    End Function
    Private Shared Function IsINIOK(ByVal INIPath As String) As Boolean
      Return Not GetINITimestamp(INIPath) = 0
    End Function
    Private Shared Function GetINITimestamp(ByVal INIPath As String) As Int64
      Try
        If Not IO.File.Exists(INIPath) Then Return 0
        Dim sNum As String = INIRead(INIPath, "META", "save", "0")
        Dim uNum As Int64 = GetValidLng(sNum)
        If uNum < Unix2000 Then Return 0
        If sNum = CStr(uNum) Then Return uNum
      Catch ex As Exception
      End Try
      Return 0
    End Function
    Private Shared Function FileSafeDelete(ByVal sFile As String) As Boolean
      Try
        Dim fFile As New IO.FileInfo(sFile)
        If Not fFile.Exists Then Return True
        If fFile.IsReadOnly Then fFile.IsReadOnly = False
        fFile.Delete()
        Return True
      Catch ex As Exception
      End Try
      Return False
    End Function
    Private Shared Function FileSafeDelete(ByVal fFile As IO.FileInfo) As Boolean
      Try
        If Not fFile.Exists Then Return True
        If fFile.IsReadOnly Then fFile.IsReadOnly = False
        fFile.Delete()
        Return True
      Catch ex As Exception
      End Try
      Return False
    End Function
    Private Shared Function FileSafeMove(ByVal sFrom As String, ByVal sTo As String, Optional ByVal noFrom As Boolean = True) As Boolean
      Try
        Dim fFrom As New IO.FileInfo(sFrom)
        If Not fFrom.Exists Then Return noFrom
        FileSafeDelete(sTo)
        fFrom.MoveTo(sTo)
        Return True
      Catch ex As Exception
      End Try
      Return False
    End Function
    Private Shared Function FileSafeCopy(ByVal sFrom As String, ByVal sTo As String, Optional ByVal noFrom As Boolean = True) As Boolean
      Try
        Dim fFrom As New IO.FileInfo(sFrom)
        If Not fFrom.Exists Then Return noFrom
        FileSafeDelete(sTo)
        Dim fTo As IO.FileInfo = fFrom.CopyTo(sTo, True)
        If Not fTo.Exists Then Return False
        If fTo.IsReadOnly Then fTo.IsReadOnly = False
        Return True
      Catch ex As Exception
      End Try
      Return False
    End Function
    Private Shared Function FileSafeRead(ByVal fFile As IO.FileInfo, Optional ByVal encoding As System.Text.Encoding = Nothing) As String
      If encoding Is Nothing Then encoding = eLATIN
      Try
        Using r As IO.FileStream = fFile.Open(IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
          Try
            r.Lock(0, r.Length)
            Dim bData As Byte()
            ReDim bData(r.Length - 1)
            r.Read(bData, 0, r.Length)
            r.Unlock(0, r.Length)
            Return encoding.GetString(bData)
          Catch ex As Exception
          Finally
            r.Close()
          End Try
        End Using
      Catch ex As Exception
      End Try
      Return Nothing
    End Function
    Private Shared Function FileSafeWrite(ByVal fFile As IO.FileInfo, ByVal sData As String, Optional ByVal encoding As System.Text.Encoding = Nothing) As Boolean
      If encoding Is Nothing Then encoding = eLATIN
      Try
        If fFile.Exists AndAlso fFile.IsReadOnly Then fFile.IsReadOnly = False
      Catch ex As Exception
        Return False
      End Try
      Try
        Using w As IO.FileStream = fFile.Open(IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.None)
          Try
            Dim bData As Byte() = encoding.GetBytes(sData)
            w.Lock(0, bData.Length)
            w.Write(bData, 0, bData.Length)
            w.Flush()
            w.Unlock(0, bData.Length)
            Return True
          Catch ex As Exception
            Return False
          Finally
            w.Close()
          End Try
        End Using
      Catch ex As Exception
      End Try
      Return False
    End Function
    Private Shared Function IsRunningProcess(ByVal pID As Int64) As Boolean
      If pID < Int32.MinValue Then Return False
      If pID > Int32.MaxValue Then Return False
      Try
        If Not System.Diagnostics.Process.GetProcessById(pID).HasExited Then Return True
      Catch ex As Exception
      End Try
      Return False
    End Function
    Private Shared Function SafeRead(ByVal INIPath As String, ByVal SectionName As String, ByVal KeyName As String, ByVal DefaultValue As String) As String
      Dim sNew As String = IO.Path.Combine(IO.Path.GetDirectoryName(INIPath), "~" & IO.Path.GetFileName(INIPath))
      Dim sOld As String = IO.Path.Combine(IO.Path.GetDirectoryName(INIPath), IO.Path.GetFileName(INIPath) & "~")
      If IsINIOK(INIPath) Then
        FileSafeDelete(sNew)
        FileSafeDelete(sOld)
        Return INIRead(INIPath, SectionName, KeyName, DefaultValue)
      End If
      If IsINIOK(sNew) Then
        FileSafeDelete(INIPath)
        FileSafeDelete(sOld)
        Return INIRead(sNew, SectionName, KeyName, DefaultValue)
      End If
      If IsINIOK(sOld) Then
        FileSafeDelete(INIPath)
        FileSafeDelete(sNew)
        Return INIRead(sOld, SectionName, KeyName, DefaultValue)
      End If
      Return DefaultValue
    End Function
    Private Shared Function ReadSections(ByVal INIPath As String) As String()
      Dim sTmp As String = SafeRead(INIPath, Nothing, Nothing, "").TrimEnd(vbNullChar).Replace(vbNullChar & vbNullChar, vbNullChar)
      If String.IsNullOrEmpty(sTmp) Then Return (New List(Of String)).ToArray
      Return sTmp.Split(vbNullChar)
    End Function
    Private Shared Function ReadKeys(ByVal INIPath As String, ByVal SectionName As String) As String()
      Dim sTmp As String = SafeRead(INIPath, SectionName, Nothing, "").TrimEnd(vbNullChar).Replace(vbNullChar & vbNullChar, vbNullChar)
      If String.IsNullOrEmpty(sTmp) Then Return (New List(Of String)).ToArray
      Return sTmp.Split(vbNullChar)
    End Function
    Private Shared Function FLock(ByVal Path As String) As Boolean
      Dim sLock As String = IO.Path.Combine(IO.Path.GetDirectoryName(Path), IO.Path.ChangeExtension(IO.Path.GetFileName(Path), "lock"))
      Dim fLocker As New IO.FileInfo(sLock)
      If fLocker.Exists AndAlso IsRunningProcess(GetValidLng(FileSafeRead(fLocker))) Then Return False
      Return FileSafeWrite(fLocker, System.Diagnostics.Process.GetCurrentProcess.Id)
    End Function
    Private Shared Sub FUnlock(ByVal Path As String)
      Dim sLock As String = IO.Path.Combine(IO.Path.GetDirectoryName(Path), IO.Path.ChangeExtension(IO.Path.GetFileName(Path), "lock"))
      Dim fLocker As New IO.FileInfo(sLock)
      If fLocker.Exists AndAlso Not GetValidLng(FileSafeRead(fLocker)) = System.Diagnostics.Process.GetCurrentProcess.Id Then Return
      FileSafeDelete(fLocker)
    End Sub
    Private Shared Sub SafeWrite(ByVal INIPath As String, ByVal Entries As Dictionary(Of String, Dictionary(Of String, String)))
      If Not FLock(INIPath) Then
        If showedError Then Return
        MsgBox("Unable to Save Configuration" & vbNewLine & vbNewLine & My.Application.Info.ProductName & "'s config file is locked. It may be in use by another task.", MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal, Application.ProductName)
        showedError = True
        Return
      End If
      Dim sNew As String = IO.Path.Combine(IO.Path.GetDirectoryName(INIPath), "~" & IO.Path.GetFileName(INIPath))
      Dim sOld As String = IO.Path.Combine(IO.Path.GetDirectoryName(INIPath), IO.Path.GetFileName(INIPath) & "~")
      Try
        FileSafeDelete(sNew)
        FileSafeDelete(sOld)
        Dim uNum As Int64 = GetINITimestamp(INIPath)
        If uNum < Unix2000 Then uNum = Unix2000
        uNum += 1
        If uNix > uNum Then uNum = uNix
        Dim iFail As Integer = 0
        For Each SectionName As String In Entries.Keys
          For Each KeyName As String In Entries(SectionName).Keys
            Dim Value As String = Entries(SectionName)(KeyName)
            If NativeMethods.WritePrivateProfileStringW(SectionName, KeyName, Value, sNew) = 0 Then iFail = System.Runtime.InteropServices.Marshal.GetLastWin32Error
            If iFail > 0 Then Exit For
          Next
          If iFail > 0 Then Exit For
        Next
        If iFail = 0 AndAlso NativeMethods.WritePrivateProfileStringW("META", "save", CStr(uNum), sNew) = 0 Then iFail = System.Runtime.InteropServices.Marshal.GetLastWin32Error
        If Not iFail = 0 Then
          If showedError Then Return
          MsgBox("Unable to Save Configuration" & vbNewLine & vbNewLine & "There was a problem while trying to write to " & My.Application.Info.ProductName & "'s new config file: " & Conversion.ErrorToString(iFail), MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal, Application.ProductName)
          showedError = True
          Return
        End If
        If Not FileSafeMove(INIPath, sOld) Then
          If showedError Then Return
          MsgBox("Unable to Save Configuration" & vbNewLine & vbNewLine & "There was a problem while trying to move " & My.Application.Info.ProductName & "'s old config file to a backup location.", MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal, Application.ProductName)
          showedError = True
          Return
        End If
        If Not FileSafeMove(sNew, INIPath, False) Then
          If showedError Then Return
          MsgBox("Unable to Save Configuration" & vbNewLine & vbNewLine & "There was a problem while trying to finalize " & My.Application.Info.ProductName & "'s new config file.", MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal, Application.ProductName)
          showedError = True
          Return
        End If
        Dim readOK = True
        For Each SectionName As String In Entries.Keys
          For Each KeyName As String In Entries(SectionName).Keys
            Dim Value As String = Entries(SectionName)(KeyName)
            If String.IsNullOrEmpty(KeyName) Then
              If ReadSections(INIPath).Contains(SectionName) Then readOK = False
            ElseIf String.IsNullOrEmpty(Value) Then
              If ReadKeys(INIPath, SectionName).Contains(KeyName) Then readOK = False
            Else
              If Not GetValidLng(INIRead(INIPath, "META", "save", "0")) = uNum Then readOK = False
            End If
          Next
        Next
        If Not readOK Then
          FileSafeDelete(INIPath)
          If showedError Then Return
          MsgBox("Unable to Save Configuration" & vbNewLine & vbNewLine & "There was a fidelity test failure: " & My.Application.Info.ProductName & "'s config was not saved.", MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal, Application.ProductName)
          showedError = True
          Return
        End If
        FileSafeDelete(sOld)
        showedError = False
      Catch ex As Exception
        If showedError Then Return
        MsgBox("Unable to Save Configuration" & vbNewLine & vbNewLine & "There was a problem while saving " & My.Application.Info.ProductName & "'s config: " & ex.Message, MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal, Application.ProductName)
        showedError = True
      Finally
        FUnlock(INIPath)
      End Try
    End Sub
    Private Shared Sub ReadSettings(ByVal state As Object)
      If canUseReg() Then Return
      SyncLock saveLock
        m_HideTray = ReadHideTray()
        m_Enabled = ReadEnabled()
        m_SelectedProfile = ReadSelectedProfile()
        SyncLock profLock
          m_Profiles = ReadProfiles()
        End SyncLock
      End SyncLock
    End Sub
    Private Shared Sub WriteSettings()
      If canUseReg() Then Return
      SyncLock saveLock
        If tRead IsNot Nothing Then
          tRead.Dispose()
          tRead = Nothing
        End If
        If tSave IsNot Nothing Then
          tSave.Dispose()
          tSave = Nothing
        End If
        tSave = New Threading.Timer(AddressOf TrueWriteSettings, Nothing, saveWait, System.Threading.Timeout.Infinite)
      End SyncLock
    End Sub
    Private Shared Sub TrueWriteSettings(ByVal state As Object)
      If canUseReg() Then Return
      SyncLock saveLock
        If tSave Is Nothing Then Return
        tSave.Dispose()
        tSave = Nothing
        Dim cPath As String = saveToINI()
        If cPath = "NO" Then Return
        Dim wList As New Dictionary(Of String, Dictionary(Of String, String))
        If Not wList.Keys.Contains(cDefault.Enabled.File.Key) Then wList.Add(cDefault.Enabled.File.Key, New Dictionary(Of String, String))
        wList.Item(cDefault.Enabled.File.Key).Add(cDefault.Enabled.File.Value.Key, IIf(m_Enabled, "Y", "N"))
        If m_HideTray Then
          If Not wList.Keys.Contains(cDefault.Enabled.File.Key) Then wList.Add(cDefault.HideTray.File.Key, New Dictionary(Of String, String))
          wList.Item(cDefault.HideTray.File.Key).Add(cDefault.HideTray.File.Value.Key, "Y")
        End If
        If Not wList.Keys.Contains(cDefault.SelectedProfile.File.Key) Then wList.Add(cDefault.Enabled.File.Key, New Dictionary(Of String, String))
        wList.Item(cDefault.SelectedProfile.File.Key).Add(cDefault.SelectedProfile.File.Value.Key, m_SelectedProfile)
        SyncLock profLock
          For Each pInfo As PROFILEINFO In m_Profiles.Values
            Dim wProfile As New Dictionary(Of String, String)
            wProfile.Add(cDefault.ProfileEntry.Button4.File.Value.Key, pInfo.Button4)
            wProfile.Add(cDefault.ProfileEntry.Button5.File.Value.Key, pInfo.Button5)
            wList.Add(pInfo.ID, wProfile)
          Next
        End SyncLock
        SafeWrite(cPath, wList)
        tRead = New Threading.Timer(AddressOf ReadSettings, Nothing, readInterval, readInterval)
      End SyncLock
    End Sub
    Shared Sub New()
      m_HideTray = cDefault.HideTray.Return
      m_Enabled = cDefault.Enabled.Return
      m_SelectedProfile = cDefault.SelectedProfile.Return
      SyncLock profLock
        m_Profiles = New Dictionary(Of String, PROFILEINFO)
      End SyncLock
      ReadSettings(Nothing)
      tRead = New Threading.Timer(AddressOf ReadSettings, Nothing, readInterval, readInterval)
    End Sub
    Private Shared m_HideTray As Boolean
    Private Shared Function ReadHideTray() As Boolean
      Try
        Dim cPath As String = saveToINI()
        If cPath = "NO" Then Return cDefault.HideTray.Return
        Dim r As String = SafeRead(cPath, cDefault.HideTray.File.Key, cDefault.HideTray.File.Value.Key, cDefault.HideTray.File.Value.Value)
        If r.Length < 1 Then Return cDefault.HideTray.Return
        r = r.Substring(0, 1).ToUpper
        Return r = "Y" OrElse r = "T" OrElse r = "1"
      Catch ex As Exception
        Return cDefault.HideTray.Return
      End Try
    End Function
    Public Shared ReadOnly Property HideTray As Boolean
      Get
        Return m_HideTray
      End Get
    End Property
    Private Shared m_Enabled As Boolean
    Private Shared Function ReadEnabled() As Boolean
      Try
        Dim cPath As String = saveToINI()
        If cPath = "NO" Then Return cDefault.Enabled.Return
        Dim r As String = SafeRead(cPath, cDefault.Enabled.File.Key, cDefault.Enabled.File.Value.Key, cDefault.Enabled.File.Value.Value)
        If r.Length < 1 Then Return cDefault.Enabled.Return
        r = r.Substring(0, 1).ToUpper
        Return r = "Y" OrElse r = "T" OrElse r = "1"
      Catch ex As Exception
        Return cDefault.Enabled.Return
      End Try
    End Function
    Public Shared Property Enabled As Boolean
      Get
        Return m_Enabled
      End Get
      Set(ByVal value As Boolean)
        m_Enabled = value
        WriteSettings()
      End Set
    End Property
    Private Shared m_SelectedProfile As String
    Private Shared Function ReadSelectedProfile() As String
      Try
        Dim cPath As String = saveToINI()
        If cPath = "NO" Then Return cDefault.SelectedProfile.Return
        Return SafeRead(cPath, cDefault.SelectedProfile.File.Key, cDefault.SelectedProfile.File.Value.Key, cDefault.SelectedProfile.File.Value.Value)
      Catch ex As Exception
        Return cDefault.SelectedProfile.Return
      End Try
    End Function
    Public Shared Property SelectedProfile As String
      Get
        Return m_SelectedProfile
      End Get
      Set(ByVal value As String)
        m_SelectedProfile = value
        WriteSettings()
      End Set
    End Property
    Private Shared m_Profiles As Dictionary(Of String, PROFILEINFO)
    Private Shared Function ReadProfiles() As Dictionary(Of String, PROFILEINFO)
      Dim cPath As String = saveToINI()
      If cPath = "NO" Then Return New Dictionary(Of String, PROFILEINFO)
      Dim sections As String() = ReadSections(cPath)
      Dim r As New Dictionary(Of String, PROFILEINFO)
      For I As Integer = 0 To sections.Length - 1
        If sections(I) = cDefault.Enabled.File.Key Then Continue For
        If sections(I) = cDefault.HideTray.File.Key Then Continue For
        If sections(I) = cDefault.SelectedProfile.File.Key Then Continue For
        If sections(I) = "META" Then Continue For
        Try
          r.Add(sections(I), New PROFILEINFO(sections(I), SafeRead(cPath, sections(I), cDefault.ProfileEntry.Button4.File.Value.Key, cDefault.ProfileEntry.Button4.File.Value.Value), SafeRead(cPath, sections(I), cDefault.ProfileEntry.Button5.File.Value.Key, cDefault.ProfileEntry.Button5.File.Value.Value)))
        Catch ex As Exception
          Continue For
        End Try
      Next
      Return r
    End Function
    Public Shared ReadOnly Property ProfileIDs As String()
      Get
        SyncLock profLock
          Return m_Profiles.Keys.ToArray
        End SyncLock
      End Get
    End Property
    Public Shared Property Profile(ByVal ID As String) As PROFILEINFO
      Get
        SyncLock profLock
          Return m_Profiles(ID)
        End SyncLock
      End Get
      Set(ByVal value As PROFILEINFO)
        SyncLock profLock
          If m_Profiles.ContainsKey(ID) Then
            m_Profiles(ID) = value
          Else
            m_Profiles.Add(ID, value)
          End If
        End SyncLock
        WriteSettings()
      End Set
    End Property
    Public Shared Sub RemoveProfile(ByVal ID As String)
      Try
        SyncLock profLock
          If Not m_Profiles.ContainsKey(ID) Then Return
          m_Profiles.Remove(ID)
        End SyncLock
        WriteSettings()
      Catch ex As Exception
      End Try
    End Sub
    Public Shared Sub RemoveAll()
      Try
        m_HideTray = cDefault.HideTray.Return
        m_Enabled = cDefault.Enabled.Return
        m_SelectedProfile = cDefault.SelectedProfile.Return
        SyncLock profLock
          m_Profiles = New Dictionary(Of String, PROFILEINFO)
        End SyncLock
        Dim cPath As String = saveToINI()
        If cPath = "NO" Then Return
        FileSafeDelete(cPath)
        While cPath.Length > 3
          cPath = IO.Path.GetDirectoryName(cPath)
          If IO.Directory.GetFileSystemEntries(cPath).Length > 0 Then Return
          IO.Directory.Delete(cPath, False)
        End While
      Catch ex As Exception
      End Try
    End Sub
  End Class
  Private Shared Function canUseReg() As Boolean
    Try
      If useReg = TriState.True Then Return True
      If useReg = TriState.False Then Return False
      If IsInstalledIsh Then
        If My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName & "-writeTest") Then My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).DeleteSubKeyTree(Application.CompanyName & "-writeTest", False)
        My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).CreateSubKey(Application.CompanyName & "-writeTest")
        My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey(Application.CompanyName & "-writeTest", True).SetValue("", Application.ProductName, Microsoft.Win32.RegistryValueKind.String)
        If My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey(Application.CompanyName & "-writeTest").GetValue("", "") = Application.ProductName Then useReg = TriState.True
        My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).DeleteSubKeyTree(Application.CompanyName & "-writeTest", True)
        If useReg = TriState.True Then Return True
      End If
    Catch ex As Exception
    Finally
      My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).DeleteSubKeyTree(Application.CompanyName & "-writeTest", False)
    End Try
    useReg = TriState.False
    Return False
  End Function
  Private Shared m_CleanProd As String = Nothing
  Private Shared ReadOnly Property CleanProductName As String
    Get
      If String.IsNullOrEmpty(m_CleanProd) Then m_CleanProd = IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath).
        Where(Function(c As Char) As Boolean
                Return Char.IsLetterOrDigit(c)
              End Function).ToArray()
      Return m_CleanProd
    End Get
  End Property
  Public Shared ReadOnly Property SomeoneStartsWithWindows As Boolean
    Get
      Try
        Return My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("Run").GetValueNames.Contains(CleanProductName)
      Catch ex As Exception
        Return False
      End Try
    End Get
  End Property
  Public Shared Property StartWithWindows As Boolean
    Get
      Try
        Dim regRun As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("Run", True)
        If regRun.GetValue(CleanProductName, "") = Application.ExecutablePath Then regRun.SetValue(CleanProductName, """" & Application.ExecutablePath & """")
        Return regRun.GetValue(CleanProductName, "") = """" & Application.ExecutablePath & """"
      Catch ex As Exception
        Return False
      End Try
    End Get
    Set(ByVal value As Boolean)
      Try
        Dim regRun As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("Run", True)
        If value Then
          regRun.SetValue(CleanProductName, """" & Application.ExecutablePath & """", Microsoft.Win32.RegistryValueKind.String)
        Else
          regRun.DeleteValue(CleanProductName, False)
        End If
      Catch ex As Exception
      End Try
    End Set
  End Property
  Public Shared ReadOnly Property HideTray As Boolean
    Get
      If canUseReg() Then Return cRegistry.HideTray
      Return cFile.HideTray
    End Get
  End Property
  Public Shared Property Enabled As Boolean
    Get
      If canUseReg() Then Return cRegistry.Enabled
      Return cFile.Enabled
    End Get
    Set(ByVal value As Boolean)
      If canUseReg() Then
        cRegistry.Enabled = value
      Else
        cFile.Enabled = value
      End If
    End Set
  End Property
  Public Shared Property SelectedProfile As String
    Get
      If canUseReg() Then Return cRegistry.SelectedProfile
      Return cFile.SelectedProfile
    End Get
    Set(ByVal value As String)
      If canUseReg() Then
        cRegistry.SelectedProfile = value
      Else
        cFile.SelectedProfile = value
      End If
    End Set
  End Property
  Public Shared ReadOnly Property ProfileIDs As String()
    Get
      If canUseReg() Then Return cRegistry.ProfileIDs
      Return cFile.ProfileIDs
    End Get
  End Property
  Public Shared Property Profile(ByVal ID As String) As PROFILEINFO
    Get
      If canUseReg() Then Return cRegistry.Profile(ID)
      Return cFile.Profile(ID)
    End Get
    Set(ByVal value As PROFILEINFO)
      If canUseReg() Then
        cRegistry.Profile(ID) = value
      Else
        cFile.Profile(ID) = value
      End If
    End Set
  End Property
  Public Shared Sub RemoveProfile(ByVal ID As String)
    If canUseReg() Then
      cRegistry.RemoveProfile(ID)
    Else
      cFile.RemoveProfile(ID)
    End If
  End Sub
  Public Shared Sub RemoveAll()
    If canUseReg() Then
      cRegistry.RemoveAll()
    Else
      cFile.RemoveAll()
    End If
  End Sub
  Public Shared ReadOnly Property IsInstalled As Boolean
    Get
      Dim pfLegacy As String = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
      If pfLegacy.Length > 0 AndAlso pfLegacy.Length <= My.Application.Info.DirectoryPath.Length AndAlso My.Application.Info.DirectoryPath.Substring(0, pfLegacy.Length) = pfLegacy Then Return True
      Dim pfNative As String = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
      If pfNative.Length > 0 AndAlso pfNative.Length <= My.Application.Info.DirectoryPath.Length AndAlso My.Application.Info.DirectoryPath.Substring(0, pfNative.Length) = pfNative Then Return True
      Return False
    End Get
  End Property
  Public Shared ReadOnly Property IsLocal As Boolean
    Get
      Dim pfLocalAppData As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
      If pfLocalAppData.Length = 0 Then Return False
      pfLocalAppData = IO.Path.Combine(pfLocalAppData, "Programs")
      If pfLocalAppData.Length <= My.Application.Info.DirectoryPath.Length AndAlso My.Application.Info.DirectoryPath.Substring(0, pfLocalAppData.Length) = pfLocalAppData Then Return True
      Return False
    End Get
  End Property
  Public Shared ReadOnly Property IsInstalledIsh As Boolean
    Get
      If IsInstalled Then Return True
      If IsLocal Then Return True
      Return False
    End Get
  End Property
End Class
