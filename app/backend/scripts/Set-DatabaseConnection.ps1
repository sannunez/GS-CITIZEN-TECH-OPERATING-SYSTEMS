[CmdletBinding()]
param(
    [string]$DatabaseHost = 'localhost',
    [ValidateRange(1, 65535)][int]$Port = 3306,
    [string]$DatabaseName = 'aja_helperapp',
    [string]$UserName = 'root'
)

$ErrorActionPreference = 'Stop'
$projectPath = Join-Path (Split-Path -Parent $PSScriptRoot) 'src\Aja.HelperApp.Api\Aja.HelperApp.Api.csproj'
$password = Read-Host "Senha MySQL do usuario $UserName" -AsSecureString
$connection = [System.Data.Common.DbConnectionStringBuilder]::new()
$connection['Server'] = $DatabaseHost
$connection['Port'] = $Port
$connection['Database'] = $DatabaseName
$connection['User ID'] = $UserName
$connection['Password'] = [System.Net.NetworkCredential]::new('', $password).Password

try {
    @{ ConnectionStrings = @{ DefaultConnection = $connection.ConnectionString } } |
        ConvertTo-Json -Depth 3 |
        dotnet user-secrets set --project $projectPath
    if ($LASTEXITCODE -ne 0) { throw 'Nao foi possivel salvar a configuracao local da conexao.' }
    Write-Host "Conexao configurada para $DatabaseHost`:$Port/$DatabaseName. A senha nao foi gravada no repositorio."
}
finally {
    $connection.Clear()
    $password.Dispose()
}
