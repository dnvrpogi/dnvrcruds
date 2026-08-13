<#
.SYNOPSIS
Starts the complete CrudeAspNet web app and opens it in the default browser.

Usage:
  .\start.ps1
  .\start.ps1 -Url "http://localhost:5000"
#>
param(
    [Alias('Urls')]
    [string] $Url = 'http://localhost:5000'
)

$projectPath = Join-Path $PSScriptRoot '..\CrudeAspNet'

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Error "dotnet not found. Install the .NET 8 SDK and re-open PowerShell."
    exit 1
}

Write-Host "Starting CrudeAspNet from $projectPath"
Push-Location $projectPath

Write-Host 'Restoring NuGet packages...'
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Error 'dotnet restore failed.'
    Pop-Location
    exit $LASTEXITCODE
}

$baseUrl = $Url.TrimEnd('/')
try {
    $uri = [Uri] $baseUrl
    if ($uri.Scheme -notin @('http', 'https') -or -not $uri.Host) {
        throw 'The URL must include an http:// or https:// address.'
    }
}
catch {
    Write-Error "Invalid URL '$Url'. Use a value such as http://localhost:5000."
    Pop-Location
    exit 1
}

$appUrl = "$baseUrl/Students"
$runArgs = @('run', '--urls', $baseUrl)

Write-Host "Starting backend and Razor frontend at $appUrl"
Write-Host 'The app will open in your default browser when it is ready.'

# The web server stays attached to this window, while this job waits for it to
# become available and opens the application exactly once.
$browserJob = Start-Job -ArgumentList $appUrl -ScriptBlock {
    param([string] $targetUrl)
    for ($attempt = 1; $attempt -le 30; $attempt++) {
        try {
            Invoke-WebRequest -Uri $targetUrl -UseBasicParsing -TimeoutSec 2 -ErrorAction Stop | Out-Null
            & cmd.exe /c start "" $targetUrl
            return
        }
        catch {
            Start-Sleep -Seconds 1
        }
    }
}

try {
    & dotnet @runArgs
}
finally {
    Remove-Job -Job $browserJob -Force -ErrorAction SilentlyContinue
    Pop-Location
}

Pop-Location
