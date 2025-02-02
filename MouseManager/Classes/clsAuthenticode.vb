Imports System.Runtime.InteropServices
Imports System.Security.Cryptography.X509Certificates
Public Class Authenticode
  <DllImport("wintrust", PreserveSig:=True, SetLastError:=True)>
  Private Shared Function WinVerifyTrust(ByVal hWnd As IntPtr, ByVal pgActionID As IntPtr, ByVal pWinTrustData As IntPtr) As UInt32
  End Function
  Private Const RRSignThumb As String = "4A6495CD107A2BA72CF54E15E5E7D87BFC43D911"
  Private Const RRSignSerial As String = "673C039A"
  Private Const RRSignSubject As String = "CN=RealityRipple Software, OU=Software Development, O=RealityRipple Software, L=Los Berros Canyon, S=California, C=US"
  Private Structure WINTRUST_FILE_INFO
    Implements IDisposable
    Public cbStruct As UInt32
    <MarshalAs(UnmanagedType.LPTStr)>
    Public pcwszFilePath As String
    Public hFile As IntPtr
    Public pgKnownSubject As IntPtr
    Public Sub New(ByVal sFile As String, ByVal gSubject As Guid)
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
    Public Sub New(ByVal FileInfo As WINTRUST_FILE_INFO)
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
    Public Sub New(ByVal ptr As IntPtr, ByVal method As AllocMethod)
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
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
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
  Public Enum Validity As UInteger
    Unsigned = &H800B0100UI
    SignedButBad = &H80096010UI
    SignedButInvalid = &H800B0000UI
    SignedButUntrusted = &H800B0109UI
    SignedAndValid = 0
    NTE_BAD_UID = &H80090001UI
    NTE_BAD_HASH = &H80090002UI
    NTE_BAD_KEY = &H80090003UI
    NTE_BAD_LEN = &H80090004UI
    NTE_BAD_DATA = &H80090005UI
    NTE_BAD_SIGNATURE = &H80090006UI
    NTE_BAD_VER = &H80090007UI
    NTE_BAD_ALGID = &H80090008UI
    NTE_BAD_FLAGS = &H80090009UI
    NTE_BAD_TYPE = &H8009000AUI
    NTE_BAD_KEY_STATE = &H8009000BUI
    NTE_BAD_HASH_STATE = &H8009000CUI
    NTE_NO_KEY = &H8009000DUI
    NTE_NO_MEMORY = &H8009000EUI
    NTE_EXISTS = &H8009000FUI
    NTE_PERM = &H80090010UI
    NTE_NOT_FOUND = &H80090011UI
    NTE_DOUBLE_ENCRYPT = &H80090012UI
    NTE_BAD_PROVIDER = &H80090013UI
    NTE_BAD_PROV_TYPE = &H80090014UI
    NTE_BAD_PUBLIC_KEY = &H80090015UI
    NTE_BAD_KEYSET = &H80090016UI
    NTE_PROV_TYPE_NOT_DEF = &H80090017UI
    NTE_PROV_TYPE_ENTRY_BAD = &H80090018UI
    NTE_KEYSET_NOT_DEF = &H80090019UI
    NTE_KEYSET_ENTRY_BAD = &H8009001AUI
    NTE_PROV_TYPE_NO_MATCH = &H8009001BUI
    NTE_SIGNATURE_FILE_BAD = &H8009001CUI
    NTE_PROVIDER_DLL_FAIL = &H8009001DUI
    NTE_PROV_DLL_NOT_FOUND = &H8009001EUI
    NTE_BAD_KEYSET_PARAM = &H8009001FUI
    NTE_FAIL = &H80090020UI
    NTE_SYS_ERR = &H80090021UI
    CRYPT_E_MSG_ERROR = &H80091001UI
    CRYPT_E_UNKNOWN_ALGO = &H80091002UI
    CRYPT_E_OID_FORMAT = &H80091003UI
    CRYPT_E_INVALID_MSG_TYPE = &H80091004UI
    CRYPT_E_UNEXPECTED_ENCODING = &H80091005UI
    CRYPT_E_AUTH_ATTR_MISSING = &H80091006UI
    CRYPT_E_HASH_VALUE = &H80091007UI
    CRYPT_E_INVALID_INDEX = &H80091008UI
    CRYPT_E_ALREADY_DECRYPTED = &H80091009UI
    CRYPT_E_NOT_DECRYPTED = &H8009100AUI
    CRYPT_E_RECIPIENT_NOT_FOUND = &H8009100BUI
    CRYPT_E_CONTROL_TYPE = &H8009100CUI
    CRYPT_E_ISSUER_SERIALNUMBER = &H8009100DUI
    CRYPT_E_SIGNER_NOT_FOUND = &H8009100EUI
    CRYPT_E_ATTRIBUTES_MISSING = &H8009100FUI
    CRYPT_E_STREAM_MSG_NOT_READY = &H80091010UI
    CRYPT_E_STREAM_INSUFFICIENT_DATA = &H80091011UI
    CRYPT_E_BAD_LEN = &H80092001UI
    CRYPT_E_BAD_ENCODE = &H80092002UI
    CRYPT_E_FILE_ERROR = &H80092003UI
    CRYPT_E_NOT_FOUND = &H80092004UI
    CRYPT_E_EXISTS = &H80092005UI
    CRYPT_E_NO_PROVIDER = &H80092006UI
    CRYPT_E_SELF_SIGNED = &H80092007UI
    CRYPT_E_DELETED_PREV = &H80092008UI
    CRYPT_E_NO_MATCH = &H80092009UI
    CRYPT_E_UNEXPECTED_MSG_TYPE = &H8009200AUI
    CRYPT_E_NO_KEY_PROPERTY = &H8009200BUI
    CRYPT_E_NO_DECRYPT_CERT = &H8009200CUI
    CRYPT_E_BAD_MSG = &H8009200DUI
    CRYPT_E_NO_SIGNER = &H8009200EUI
    CRYPT_E_PENDING_CLOSE = &H8009200FUI
    CRYPT_E_REVOKED = &H80092010UI
    CRYPT_E_NO_REVOCATION_DLL = &H80092011UI
    CRYPT_E_NO_REVOCATION_CHECK = &H80092012UI
    CRYPT_E_REVOCATION_OFFLINE = &H80092013UI
    CRYPT_E_NOT_IN_REVOCATION_DATABASE = &H80092014UI
    CRYPT_E_INVALID_NUMERIC_STRING = &H80092020UI
    CRYPT_E_INVALID_PRINTABLE_STRING = &H80092021UI
    CRYPT_E_INVALID_IA5_STRING = &H80092022UI
    CRYPT_E_INVALID_X500_STRING = &H80092023UI
    CRYPT_E_NOT_CHAR_STRING = &H80092024UI
    CRYPT_E_FILERESIZED = &H80092025UI
    CRYPT_E_SECURITY_SETTINGS = &H80092026UI
    CRYPT_E_NO_VERIFY_USAGE_DLL = &H80092027UI
    CRYPT_E_NO_VERIFY_USAGE_CHECK = &H80092028UI
    CRYPT_E_VERIFY_USAGE_OFFLINE = &H80092029UI
    CRYPT_E_NOT_IN_CTL = &H8009202AUI
    CRYPT_E_NO_TRUSTED_SIGNER = &H8009202BUI
    CRYPT_E_OSS_ERROR = &H80093000UI
    CERTSRV_E_BAD_REQUESTSUBJECT = &H80094001UI
    CERTSRV_E_NO_REQUEST = &H80094002UI
    CERTSRV_E_BAD_REQUESTSTATUS = &H80094003UI
    CERTSRV_E_PROPERTY_EMPTY = &H80094004UI
    TRUST_E_SYSTEM_ERROR = &H80096001UI
    TRUST_E_NO_SIGNER_CERT = &H80096002UI
    TRUST_E_COUNTER_SIGNER = &H80096003UI
    TRUST_E_CERT_SIGNATURE = &H80096004UI
    TRUST_E_TIME_STAMP = &H80096005UI
    'TRUST_E_BAD_DIGEST = &H80096010UI
    TRUST_E_BASIC_CONSTRAINTS = &H80096019UI
    TRUST_E_FINANCIAL_CRITERIA = &H8009601EUI
    TRUST_E_PROVIDER_UNKNOWN = &H800B0001UI
    TRUST_E_ACTION_UNKNOWN = &H800B0002UI
    TRUST_E_SUBJECT_FORM_UNKNOWN = &H800B0003UI
    TRUST_E_SUBJECT_NOT_TRUSTED = &H800B0004UI
    DIGSIG_E_ENCODE = &H800B0005UI
    DIGSIG_E_DECODE = &H800B0006UI
    DIGSIG_E_EXTENSIBILITY = &H800B0007UI
    DIGSIG_E_CRYPTO = &H800B0008UI
    PERSIST_E_SIZEDEFINITE = &H800B0009UI
    PERSIST_E_SIZEINDEFINITE = &H800B000AUI
    PERSIST_E_NOTSELFSIZING = &H800B000BUI
    'TRUST_E_NOSIGNATURE = &H800B0100UI
    CERT_E_EXPIRED = &H800B0101UI
    CERT_E_VALIDITYPERIODNESTING = &H800B0102UI
    CERT_E_ROLE = &H800B0103UI
    CERT_E_PATHLENCONST = &H800B0104UI
    CERT_E_CRITICAL = &H800B0105UI
    CERT_E_PURPOSE = &H800B0106UI
    CERT_E_ISSUERCHAINING = &H800B0107UI
    CERT_E_MALFORMED = &H800B0108UI
    'CERT_E_UNTRUSTEDROOT = &H800B0109UI
    CERT_E_CHAINING = &H800B010AUI
    TRUST_E_FAIL = &H800B010BUI
    CERT_E_REVOKED = &H800B010CUI
    CERT_E_UNTRUSTEDTESTROOT = &H800B010DUI
    CERT_E_REVOCATION_FAILURE = &H800B010EUI
    CERT_E_CN_NO_MATCH = &H800B010FUI
    CERT_E_WRONG_USAGE = &H800B0110UI
    BadThumb = &HA0090001UI
    BadSerial = &HA0090002UI
    BadSubject = &HA0090003UI
  End Enum
  Private Shared Function VerifyTrust(ByVal sFile As String) As Validity
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
  Private Shared Function SignerIsRealityRipple(ByVal sFile As String) As Validity
    Dim theCertificate As X509Certificate2
    Try
      Dim theSigner As X509Certificate = X509Certificate.CreateFromSignedFile(sFile)
      theCertificate = New X509Certificate2(theSigner)
    Catch ex As Exception
      Return False
    End Try
    Dim theCertificateChain As New X509Chain(True)
    theCertificateChain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain
    theCertificateChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck
    theCertificateChain.ChainPolicy.UrlRetrievalTimeout = New TimeSpan(0, 0, 0, 15, 500)
    theCertificateChain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag
    theCertificateChain.Build(theCertificate)
    Dim Signer As X509Certificate2 = theCertificateChain.ChainElements(0).Certificate
    If Not Signer.Thumbprint = RRSignThumb Then Return Validity.BadThumb
    If Not Signer.SerialNumber = RRSignSerial Then Return Validity.BadSerial
    If Not Signer.Subject = RRSignSubject Then Return Validity.BadSubject
    Return 0
  End Function
  Public Shared Function IsSelfSigned(ByVal sFile As String) As Validity
    Dim iRet As Validity = SignerIsRealityRipple(sFile)
    If Not iRet = Validity.SignedAndValid Then Return iRet
    Return VerifyTrust(sFile)
  End Function
End Class
