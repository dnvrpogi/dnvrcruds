# Open browser and start GitHub CLI web auth flow
# Usage: .\scripts\gh_login.ps1

if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
  Write-Error "gh (GitHub CLI) not found in PATH. Install it and restart your shell."
  exit 1
}

Write-Host "Starting GitHub web-auth flow (will open your browser)..."
# This opens the browser and runs the web auth flow
gh auth login --web

if ($LASTEXITCODE -eq 0) {
  Write-Host "Authenticated successfully."
} else {
  Write-Error "gh auth login failed. See output above for details."
}
