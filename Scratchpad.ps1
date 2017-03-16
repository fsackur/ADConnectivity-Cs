$S = Get-DnsResolver 134.213.29.116
$S.Query("corp.dustyfox.uk", "A")

$R = @{}
$Domain = "corp.dustyfox.uk"
$R.A = $S.Query($Domain, "A")
$R.NS = $S.Query($Domain, "NS")
$R.SOA = $S.Query($Domain, "SOA")
$R.TXT = $S.Query("dustyfox.uk", "TXT")
$R.SRV = $S.Query("_ldap._tcp.pdc._msdcs.$Domain", "SRV")


$R.Keys | %{$_.PadRight(6, ' ') + ($R.$_.Answers.Record | %{$_.ToString()}) -join '\r\n'}

#(Heijden.DNS.Response).Error => Write-Error, throw

$SR = @{}
$SR.A = $S.SimpleQuery($Domain, "A")
$SR.NS = $S.SimpleQuery($Domain, "NS")
$SR.SOA = $S.SimpleQuery($Domain, "SOA")
$SR.TXT = $S.SimpleQuery("dustyfox.uk", "TXT")
$SR.SRV = $S.SimpleQuery("_ldap._tcp.pdc._msdcs.$Domain", "SRV")

$SR.Keys | %{$_.PadRight(6, ' ') + ($SR.$_)}



Get-DnsResponse $S $Domain -QueryType "A"

$R.SRV.Answers.Record.ToString() -replace '\d* \d* \d* '





$a = Get-DnsResolver 8.8.8.8
$b = $a.Query("Ibm.com", "A")
$b
$b.ToString()



$a = Get-DnsResolver 8.8.8.8
$b = $a.Query("Ibm.com", "A")

$x = Get-DnsResolver 8.8.8.8
$y = $x.Query("Ibm.com", "A")

$b.Equals($y)





$L1 = New-Object "System.Collections.Generic.List[string]"
$L2 = New-Object "System.Collections.Generic.List[string]"
$L1.Equals($L2)






$S1 = New-Object Dusty.ADConnectivity.AdDnsResolver(
    (New-Object IPEndpoint(([ipaddress]"134.213.29.116"),53)),
    "corp.dustyfox.uk"
)






$S = Get-DnsResolver 134.213.29.116
$S2 = Get-DnsResolver 134.213.29.116
$S.DnsServer
$S.DnsServer.GetType()
$S.GetType()
$S | gm
$S.DnsServers


$S1 = New-Object Dusty.ADConnectivity.AdDnsResolver(
    (New-Object IPEndpoint(([ipaddress]"134.213.29.116"),53)),
    "corp.dustyfox.uk"
)
$S2 = New-Object Dusty.ADConnectivity.AdDnsResolver(
    (New-Object IPEndpoint(([ipaddress]"134.213.29.116"),53)),
    "corp.dustyfox.uk"
)

$S1.QueryAd()
$S1.Pdc
$S1.Pdc.ToString()
$S1.Pdc.GetAnswerString()
$S1.Pdc.GetAnswerStrings()
$S1.Equals($S2)
$S2.QueryAd()
$S1.Equals($S2)



$S1 = New-Object Dusty.ADConnectivity.AdDnsResolver(
    (New-Object IPEndpoint(([ipaddress]"134.213.29.116"),53)),
    "corp.dustyfox.uk"
)

$S1.QueryPdc()
$S1.QueryAd()
$Q = [Dusty.ADConnectivity.AdDnsResolver]::DefaultAdQueries
$S1.QueryAd($Q)
$Q.Add("SOA", [scriptblock]::Create('param($R) return $R.Query($this.AdDomain, "SOA")'))
$t = $S1.QueryAd($Q)
$t
$t | fl *




















