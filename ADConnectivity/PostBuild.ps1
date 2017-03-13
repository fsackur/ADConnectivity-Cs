<#
    .Synopsis
    Find module manifest and update it

    .Description
    Run as a post-build event
#>
param(
    [string]$TargetPath = 'C:\dev\ADConnectivity\ADConnectivity\bin\Debug\ADConnectivity.dll'
)


<#Increment version number in .psd1 file to match .dll
$Version = (Get-Item $TargetPath).VersionInfo.ProductVersion;
$PsdPath = $TargetPath -replace 'dll$', 'psd1'
$Content = Get-Content $PsdPath -Raw
$Content.Replace("ModuleVersion = '1.0'", "ModuleVersion = '$Version'") | Out-File $PsdPath -Force
#>
Set-Location $TargetPath
Remove-Item .\System.Management.Automation.dll -Force
[void](New-Item -ItemType Directory -Path Private -Force)
[void](New-Item -ItemType Directory -Path Public -Force)
Get-ChildItem '.\*.dll' | %{Move-Item $_ .\Private}
Get-ChildItem '.\*.pdb' | %{Move-Item $_ .\Private}
Get-ChildItem '.\*.psm1' | %{Move-Item $_ .\Public}
Get-ChildItem '.\*.ps1' | %{Move-Item $_ .\Public}