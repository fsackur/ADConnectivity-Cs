<#
    .Synopsis
    Find module manifest and update it

    .Description
    Run as a post-build event
#>
param(
    [string]$TargetPath
)


$ProjectDir = $PSScriptRoot
$TargetDir = Split-Path $TargetPath
Set-Location $TargetDir

#Increment version number in .psd1 file to match .dll
try {
    $Version = (Get-Item $TargetPath -ErrorAction Stop).VersionInfo.ProductVersion;
    $PsdPath = $TargetPath -replace 'dll$', 'psd1'
    $Content = Get-Content $PsdPath -Raw
    $Content.Replace("ModuleVersion = '(\d*\.?){0,4}'", "ModuleVersion = '$Version'") | Out-File $PsdPath -Force
} catch {}

#Move out of the "PowerShell" folder; that is just for organisation of source code
Get-ChildItem '.\PowerShell\*.*' | %{Move-Item $_ . -Force}
Remove-Item '.\PowerShell' -Force

#Move into public / private folders
Remove-Item .\System.Management.Automation.dll -Force -ErrorAction SilentlyContinue  #We don't need this in output, our cmdlets are running in PowerShell
[void](New-Item -ItemType Directory -Path Private -Force)
[void](New-Item -ItemType Directory -Path Public -Force)
Get-ChildItem '.\*.dll' | %{Move-Item $_ .\Private -Force}
Get-ChildItem '.\*.pdb' | %{Move-Item $_ .\Private -Force}
Get-ChildItem '.\*.ps1xml' | %{Move-Item $_ .\Private -Force}
Get-ChildItem '.\*.psm1' | %{Move-Item $_ .\Public -Force}
Get-ChildItem '.\*.ps1' | %{Move-Item $_ .\Public -Force}

#Update help file
Import-Module PlatyPS
Import-Module $PsdPath
$Cmdlets = Get-Command -Module ADConnectivity -CommandType Cmdlet
New-MarkdownHelp -Command $Cmdlets -OutputFolder $ProjectDir\docs -ErrorAction SilentlyContinue
Update-MarkdownHelp -Path $ProjectDir\docs
[void](New-Item -ItemType Directory -Path en-US -Force)
New-ExternalHelp -Path $ProjectDir\docs -OutputPath .\en-US -Force
