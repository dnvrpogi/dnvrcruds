<##
.SYNOPSIS
Starts the StudentHub REST API.

.EXAMPLE
.\scripts\start-api.ps1
.\scripts\start-api.ps1 -Url "http://0.0.0.0:5000"
#>
param(
    [string] $Url = 'http://0.0.0.0:5000'
)

$projectPath = Join-Path $PSScriptRoot '..\CrudeAspNet'

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Error 'dotnet not found. Install the .NET 8 SDK.'
    exit 1
}

Push-Location $projectPath
try {
    dotnet run --urls $Url
}
finally {
    Pop-Location
}
