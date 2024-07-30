Param(
    [string]$email,
    [string]$appPassword,
    [string]$imapHost
)

$appSettingsPath = "$PSScriptRoot/Configs/appsettings.json"

if (-Not (Test-Path $appSettingsPath)) {
    Write-Host "File not found: $appSettingsPath"
    exit 1
}

$json = Get-Content $appSettingsPath -Raw | ConvertFrom-Json

$json.TestSettings.EmailSettings.Email = $email
$json.TestSettings.EmailSettings.AppPassword = $appPassword
$json.TestSettings.EmailSettings.Host = $imapHost

$json | ConvertTo-Json -Depth 32 | Set-Content $appSettingsPath

Write-Host "Updated appsettings.json successfully."
