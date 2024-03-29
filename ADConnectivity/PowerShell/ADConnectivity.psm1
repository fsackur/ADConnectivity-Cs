#testing in ISE
#gci $PSScriptRoot\..\Private\*.dll | Import-Module


function Test-AdDnsResolution {
    [CmdletBinding()]
	param(
        [Dusty.Net.ValidateDnsHostnameOrIpAddressAttribute()]
        [Dusty.ADCOnnectivity.AdDnsResolver[]]$DnsResolver = ("corp.dustyfox.uk", "8.8.8.8"),
        [Dusty.Net.ValidateDnsHostnameAttribute()]
        $AdDomain = "corp.dustyfox.uk"
	)

    #$Servers = Get-AdDnsResolver "corp.dustyfox.uk", 8.8.8.8 -AdDomain "corp.dustyfox.uk"
    
    #Collection to hold responses
    $AdDnsResponses = New-Object 'System.Collections.Generic.List[Dusty.ADConnectivity.AdDnsResponse]'
    
    #Customise the base set of queries
    $QueryItems = [Dusty.ADConnectivity.AdDnsResolver]::DefaultAdQueries
    <#
    #This adds to the dictionary, but generates a runtime error
    $QueryItems.Add(
        "NS",
        {param($Resolver); return $Resolver.Query($AdDomain, "NS")}
    )
    #>

    #Run the AD queries on all servers
    #Write-Verbose $Servers.Count
    foreach ($Server in $DnsResolver) {
        $AdDnsResponse = $Server.QueryAd($QueryItems)
        $AdDnsResponse.GetErrors() | Write-Verbose
        $AdDnsResponses.Add($AdDnsResponse)
    }


    foreach ($Query in $QueryItems.Keys) {
        #$UniqueResponses = $AdDnsResponses | foreach {$_.GetResponses()} | sort $Query -Unique
        $UniqueResponses = $AdDnsResponses | sort $Query -Unique

        $UniqueResponses | select -Skip 1 | foreach {
            Write-Verbose -Message ([string]::Format(
                "Server {0} has {1} {2} in DNS, but server {3} has {4}; this could indicate a replication error",
                $UniqueResponses[0].DnsServer,
                $Query,
                "'" + ($UniqueResponses[0].$Query.Answers -join "', '") + "'",
                $_.DnsServer,
                "'" + ($_.$Query.Answers -join "', '") + "'"
            ))
        }
    }
}
