---
external help file: ADConnectivity.dll-Help.xml
online version: 
schema: 2.0.0
---

# Get-DnsResolver

## SYNOPSIS
{{Fill in the Synopsis}}

## SYNTAX

### DnsOnly (Default)
```
Get-DnsResolver -DnsServer <String[]> [<CommonParameters>]
```

### AdSpecifiedDomain
```
Get-DnsResolver -DnsServer <String[]> -AdDomain <String> [<CommonParameters>]
```

### AdMachineDomain
```
Get-DnsResolver -DnsServer <String[]> [-UseMachineDomain] [-UseMachineDnsServers] [<CommonParameters>]
```

## DESCRIPTION
{{Fill in the Description}}

## EXAMPLES

### Example 1
```
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -AdDomain
{{Fill AdDomain Description}}

```yaml
Type: String
Parameter Sets: AdSpecifiedDomain
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DnsServer
{{Fill DnsServer Description}}

```yaml
Type: String[]
Parameter Sets: (All)
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -UseMachineDnsServers
{{Fill UseMachineDnsServers Description}}

```yaml
Type: SwitchParameter
Parameter Sets: AdMachineDomain
Aliases: 

Required: True
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -UseMachineDomain
{{Fill UseMachineDomain Description}}

```yaml
Type: SwitchParameter
Parameter Sets: AdMachineDomain
Aliases: 

Required: True
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String[]
System.Net.IPAddress\[\]

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

