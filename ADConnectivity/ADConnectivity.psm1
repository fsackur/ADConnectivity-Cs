
function Test-AdDns {
    $Server = Get-DnsResolver "8.8.8.8", "134.213.29.116"
    $Server.Count
}