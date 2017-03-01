<#
    .Synopsis
    Find module manifest and update it

    .Description
    Run as a post-build event
#>
param(
    [string]$TargetPath = 'C:\dev\ADConnectivity\ADConnectivity\bin\Debug\ADConnectivity.dll'
)


$Version = (Get-Item $TargetPath).VersionInfo.ProductVersion;
$PsdPath = $TargetPath -replace 'dll$', 'psd1'
$Content = Get-Content $PsdPath -Raw
$Content.Replace("ModuleVersion = '1.0'", "ModuleVersion = '$Version'") | Out-File $PsdPath -Force
