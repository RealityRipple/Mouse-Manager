Imports System.Runtime.InteropServices
Imports System.Security.Cryptography.X509Certificates

Public Class Authenticode
  <DllImport("wintrust", PreserveSig:=True, SetLastError:=True)>
  Private Shared Function WinVerifyTrust(hWnd As IntPtr, pgActionID As IntPtr, pWinTrustData As IntPtr) As UInt32
  End Function
  Private Const RRRootThumb As String = "25E10B83C6F3EA44EE5E8C290EB17200A5F77EBB"
  Private Const RRRootSerial As String = "0087448327"
  Private Const RRRootSubject As String = "CN=RealityRipple Software Root CA, OU=RealityRipple Software Certificate Authority, O=RealityRipple Software, L=Los Berros Canyon, S=California, C=US"
  Private Const RRSignThumb As String = "F526764D46CB25CB11C914B09660AD7E911BFBDA"
  Private Const RRSignSerial As String = "32126882"
  Private Const RRSignSubject As String = "CN=RealityRipple Software, OU=Software Development, O=RealityRipple Software, L=Los Berros Canyon, S=California, C=US"
  Private Structure WINTRUST_FILE_INFO
    Implements IDisposable
    Public cbStruct As UInt32
    <MarshalAs(UnmanagedType.LPTStr)>
    Public pcwszFilePath As String
    Public hFile As IntPtr
    Public pgKnownSubject As IntPtr
    Public Sub New(sFile As String, gSubject As Guid)
      cbStruct = Marshal.SizeOf(GetType(WINTRUST_FILE_INFO))
      pcwszFilePath = sFile
      If Not gSubject = Guid.Empty Then
        pgKnownSubject = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(Guid)))
        Marshal.StructureToPtr(gSubject, pgKnownSubject, True)
      Else
        pgKnownSubject = IntPtr.Zero
      End If
      hFile = IntPtr.Zero
    End Sub
    Public Sub Dispose() Implements System.IDisposable.Dispose
      If Not pgKnownSubject = IntPtr.Zero Then
        Marshal.DestroyStructure(pgKnownSubject, GetType(Guid))
        Marshal.FreeHGlobal(pgKnownSubject)
      End If
    End Sub
  End Structure
  <StructLayout(LayoutKind.Sequential)>
  Private Structure WINTRUST_DATA
    Implements IDisposable
    Public cbStruct As UInt32
    Public pPolicyCallbackData As IntPtr
    Public pSIPCallbackData As IntPtr
    Public dwUIChoice As UiChoice
    Public fdwRevocationChecks As RevocationCheckFlags
    Public dwUnionChoice As UnionChoice
    Public pInfoStruct As IntPtr
    Public dwStateAction As StateAction
    Public hWVTStateData As IntPtr
    Private pwszURLReference As IntPtr
    Public dwProvFlags As TrustProviderFlags
    Public dwUIContext As UIContext
    Public Sub New(FileInfo As WINTRUST_FILE_INFO)
      cbStruct = Marshal.SizeOf(GetType(WINTRUST_DATA))
      pInfoStruct = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(WINTRUST_FILE_INFO)))
      Marshal.StructureToPtr(FileInfo, pInfoStruct, False)
      dwUnionChoice = UnionChoice.File
      pPolicyCallbackData = IntPtr.Zero
      pSIPCallbackData = IntPtr.Zero
      dwUIChoice = UiChoice.NoUI
      fdwRevocationChecks = RevocationCheckFlags.None
      dwStateAction = StateAction.Ignore
      hWVTStateData = IntPtr.Zero
      pwszURLReference = IntPtr.Zero
      dwProvFlags = TrustProviderFlags.Safer
      dwUIContext = UIContext.Execute
    End Sub
    Public Sub Dispose() Implements System.IDisposable.Dispose
      If dwUnionChoice = UnionChoice.File Then
        Dim info As New WINTRUST_FILE_INFO
        Marshal.PtrToStructure(pInfoStruct, info)
        info.Dispose()
        Marshal.DestroyStructure(pInfoStruct, GetType(WINTRUST_FILE_INFO))
      End If
      Marshal.FreeHGlobal(pInfoStruct)
    End Sub
  End Structure
  Private Class UnmanagedPointer
    Implements IDisposable
    Private m_ptr As IntPtr
    Private m_meth As AllocMethod
    Public Sub New(ptr As IntPtr, method As AllocMethod)
      m_meth = method
      m_ptr = ptr
    End Sub
    Public ReadOnly Property Pointer() As IntPtr
      Get
        Return m_ptr
      End Get
    End Property
#Region "IDisposable Support"
    Private disposedValue As Boolean
    Protected Overridable Sub Dispose(disposing As Boolean)
      If Not Me.disposedValue Then
        If Not m_ptr = IntPtr.Zero Then
          If m_meth = AllocMethod.HGlobal Then
            Marshal.FreeHGlobal(m_ptr)
          ElseIf m_meth = AllocMethod.CoTaskMem Then
            Marshal.FreeCoTaskMem(m_ptr)
          End If
          m_ptr = IntPtr.Zero
        End If
        If disposing Then
          GC.SuppressFinalize(Me)
        End If
      End If
      Me.disposedValue = True
    End Sub
    Protected Overrides Sub Finalize()
      Dispose(False)
      MyBase.Finalize()
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
      Dispose(True)
    End Sub
#End Region
  End Class
  Private Enum AllocMethod
    HGlobal
    CoTaskMem
  End Enum
  Private Enum UnionChoice
    File = 1
    Catalog
    Blob
    Signer
    Cert
  End Enum
  Private Enum UiChoice
    All = 1
    NoUI
    NoBad
    NoGood
  End Enum
  Private Enum RevocationCheckFlags
    None = 0
    WholeChain
  End Enum
  Private Enum StateAction
    Ignore = 0
    Verify
    Close
    AutoCache
    AutoCacheFlush
  End Enum
  <Flags()>
  Private Enum TrustProviderFlags
    UseIE4Trust = 1
    NoIE4Chain = 2
    NoPolicyUsage = 4
    RevocationCheckNone = 16
    RevocationCheckEndCert = 32
    RevocationCheckChain = 64
    RevocationCheckChainExcludeRoot = 128
    Safer = 256
    HashOnly = 512
    UseDefaultOSVerCheck = 1024
    LifetimeSigning = 2048
  End Enum
  Private Enum UIContext
    Execute = 0
    Install
  End Enum
  Private Enum Validity As UInteger
    Unsigned = &H800B0100UI
    SignedButBad = &H80096010UI
    SignedButInvalid = &H800B0000UI
    SignedButUntrusted = &H800B0109UI
    SignedAndValid = 0
  End Enum
  Private Shared Function VerifyTrust(sFile As String) As UInteger
    Dim v2ID As New Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}")
    Dim result As UInteger = Validity.Unsigned
    Dim fileInfo As New WINTRUST_FILE_INFO(sFile, Guid.Empty)
    Using guidPtr As New UnmanagedPointer(Marshal.AllocHGlobal(Marshal.SizeOf(GetType(Guid))), AllocMethod.HGlobal)
      Using wvtDataPtr As New UnmanagedPointer(Marshal.AllocHGlobal(Marshal.SizeOf(GetType(WINTRUST_DATA))), AllocMethod.HGlobal)
        Dim data As New WINTRUST_DATA(fileInfo)
        Dim pGuid As IntPtr = guidPtr.Pointer
        Dim pData As IntPtr = wvtDataPtr.Pointer
        Marshal.StructureToPtr(v2ID, pGuid, True)
        Marshal.StructureToPtr(data, pData, True)
        result = WinVerifyTrust(IntPtr.Zero, pGuid, pData)
      End Using
    End Using
    fileInfo.Dispose()
    fileInfo = Nothing
    Return result
  End Function
  Private Shared Function RootIsRealityRipple(sFile As String) As Boolean
    Dim theCertificate As X509Certificate2
    Try
      Dim theSigner As X509Certificate = X509Certificate.CreateFromSignedFile(sFile)
      theCertificate = New X509Certificate2(theSigner)
    Catch ex As Exception
      Return False
    End Try
    Dim theCertificateChain = New X509Chain(True)
    theCertificateChain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain
    theCertificateChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck
    theCertificateChain.ChainPolicy.UrlRetrievalTimeout = New TimeSpan(0, 0, 15)
    theCertificateChain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag
    theCertificateChain.Build(theCertificate)
    Dim Root As X509Certificate2 = theCertificateChain.ChainElements(theCertificateChain.ChainElements.Count - 1).Certificate
    If Root.Thumbprint = RRRootThumb And Root.SerialNumber = RRRootSerial And Root.Subject = RRRootSubject Then Return True
    Return False
  End Function
  Private Shared Function SignerIsRealityRipple(sFile As String) As Boolean
    Dim theCertificate As X509Certificate2
    Try
      Dim theSigner As X509Certificate = X509Certificate.CreateFromSignedFile(sFile)
      theCertificate = New X509Certificate2(theSigner)
    Catch ex As Exception
      Return False
    End Try
    Dim theCertificateChain = New X509Chain(True)
    theCertificateChain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain
    theCertificateChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck
    theCertificateChain.ChainPolicy.UrlRetrievalTimeout = New TimeSpan(0, 0, 15)
    theCertificateChain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag
    theCertificateChain.Build(theCertificate)
    Dim Signer As X509Certificate2 = theCertificateChain.ChainElements(0).Certificate
    If Signer.Thumbprint = RRSignThumb And Signer.SerialNumber = RRSignSerial And Signer.Subject = RRSignSubject Then Return True
    Return False
  End Function
  Public Shared Function IsSelfSigned(sFile As String) As Boolean
    If Not RootIsRealityRipple(sFile) Then Return False
    If Not SignerIsRealityRipple(sFile) Then Return False
    Dim iRet As Validity = VerifyTrust(sFile)
    Select Case iRet
      Case Validity.SignedAndValid : Return True
      Case Validity.SignedButUntrusted : Return True
    End Select
    Return False
  End Function
End Class
