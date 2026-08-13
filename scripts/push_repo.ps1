# Push repo script for C:\Crude
# Usage: powershell -ExecutionPolicy Bypass -File .\scripts\push_repo.ps1
param(
  [string]$RemoteUrl = 'https://github.com/dnvrpogi/dnvrcruds.git',
  [string]$CommitMessage = 'Initial commit: Flask CRUD app'
)

function Fail($msg) { Write-Error $msg; exit 1 }

git remote remove origin 2>$null
git remote add origin $RemoteUrl || Fail "git remote add failed for $RemoteUrl"
git branch -M main 2>$null
if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
  Fail 'git not found in PATH. Install Git and restart your shell.'
}

Push-Location 'C:\Crude'

# Helper to run git and fail on error
function Run-Git {
  param([Parameter(Mandatory=$true)] [string[]]$Args)
  & git @Args
  if ($LASTEXITCODE -ne 0) {
    Fail "git $($Args -join ' ') failed with exit code $LASTEXITCODE"
  }
}

# Ensure user identity
$uname = git config user.name 2>$null
$uemail = git config user.email 2>$null
if (-not $uname -or -not $uemail) {
  Write-Host "Git user.name or user.email not set. You can set them now or skip."
  $name = Read-Host 'Your Name (leave empty to skip)'
  $email = Read-Host 'Your Email (leave empty to skip)'
  if ($name) { Run-Git -Args @('config','user.name',$name) }
  if ($email) { Run-Git -Args @('config','user.email',$email) }
}

# Init repo if needed
if (-not (Test-Path .git)) {
  Run-Git -Args @('init')
}

# Add remote (replace existing)
try { git remote remove origin 2>$null } catch {}
Run-Git -Args @('remote','add','origin',$RemoteUrl)

# Add and commit if there are changes
$st = git status --porcelain
if ($st) {
  Run-Git -Args @('add','.')
  # commit may fail if no changes remain
  & git commit -m "$CommitMessage"
  if ($LASTEXITCODE -ne 0) {
    Write-Host 'Commit step returned non-zero (perhaps nothing to commit)'
  }
} else {
  Write-Host 'No changes to commit.'
}

# Ensure branch
try { & git branch -M main 2>$null } catch {}

# Push
Write-Host "Pushing to $RemoteUrl (may prompt for credentials)..."
& git push -u origin main
if ($LASTEXITCODE -ne 0) {
  Write-Host 'Push failed. Try running `gh auth login` then re-run this script.'
  Write-Host 'Or run the commands manually and paste the output here.'
  exit $LASTEXITCODE
}

Write-Host 'Push succeeded.'
Pop-Location
