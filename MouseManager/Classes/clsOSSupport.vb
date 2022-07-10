Public Class OSSupport
  Public Shared Function NativeTLS12() As Boolean
    If (Environment.OSVersion.Version.Major < 6 Or (Environment.OSVersion.Version.Major = 6 And Environment.OSVersion.Version.Minor = 0)) Then Return False
    If (Environment.Version.Major = 4 And Environment.Version.Minor = 0 And Environment.Version.Build = 30319 And Environment.Version.Revision < 17929) Then Return False
    Return True
  End Function

  Public Shared Function ProtoURL(ByVal sURL As String) As String
    If NativeTLS12() Then Return "https:" & sURL
    Return "http:" & sURL
  End Function
End Class
