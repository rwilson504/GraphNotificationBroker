param (
    
    [Parameter(Mandatory,ValueFromPipelineByPropertyName)]
    [string] $FrontendClientId,
    [Parameter(Mandatory,ValueFromPipelineByPropertyName)]
    [string] $BackendClientId,
    [Parameter(Mandatory,ValueFromPipelineByPropertyName)]
    [string] $RedirectUri,
    [Parameter(Mandatory,ValueFromPipelineByPropertyName)]
    [string] $AuthorityUri
)
try
{
    Write-Host "Replacing Tokens in index.html Test Client"
    $frontEndToken = "__FRONTEND_CLIENT_ID__"
    $backendToken = "__BACKEND_CLIENT_ID__"
    $functionAppHostToken = "__FUNCTION_APP_HOST__"
    $authorityUriToken = "__AUTHORITY_URI__"

    $content = Get-Content -Path './TestClient/index-template.html'
    
    Write-Host "Replacing $($frontEndToken) token with $($FrontendClientId)"
    $content = $content -replace $frontEndToken, $FrontendClientId
    Write-Host "Replacing $($backendToken) token with $($BackendClientId)"
    $content = $content -replace $backendToken, $BackendClientId
    Write-Host "Replacing $($functionAppHostToken) token with $($RedirectUri)"
    $content = $content -replace $functionAppHostToken, $RedirectUri
    Write-Host "Replacing $($authorityUriToken) token with $($AuthorityUri)"
    $content = $content -replace $authorityUriToken, $AuthorityUri

    $content | Out-File './TestClient/index.html'
}
catch
{
    Write-Warning $_
    Write-Warning $_.exception
}