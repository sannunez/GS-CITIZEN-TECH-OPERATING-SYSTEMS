# Gera um pacote de fontes para copiar à VM, sem binários, dependências ou .env.
$ErrorActionPreference = 'Stop'
$repoRoot = (Resolve-Path -LiteralPath (Join-Path $PSScriptRoot '..')).Path
$outputRoot = Join-Path $repoRoot 'output'
New-Item -ItemType Directory -Path $outputRoot -Force | Out-Null
$archivePath = Join-Path $outputRoot 'aja-publicacao.tar.gz'

# Pastas copiadas do OneDrive podem carregar o atributo Windows ReadOnly.
# O tar traduz esse atributo para 0555, impedindo extração/build pelo usuário Linux.
# Normalize apenas as pastas desta cópia no repositório de OS.
$sourceDirectories = @('app', 'config', 'scripts', 'tests') | ForEach-Object {
    $sourcePath = Join-Path $repoRoot $_
    Get-Item -LiteralPath $sourcePath
    Get-ChildItem -LiteralPath $sourcePath -Directory -Recurse -Force
}
foreach ($sourceDirectory in $sourceDirectories) {
    $sourceDirectory.Attributes = $sourceDirectory.Attributes -band `
        (-bnot [System.IO.FileAttributes]::ReadOnly)
}

& tar.exe --exclude=node_modules --exclude=bin --exclude=obj --exclude=.expo `
    --exclude=dist --exclude=.publish --exclude=.git --exclude='.env*' `
    --format=ustar -czf $archivePath -C $repoRoot app config scripts tests README.md
if ($LASTEXITCODE -ne 0) {
    throw 'Não foi possível gerar o pacote da aplicação.'
}
Write-Output "Pacote de publicação criado: $archivePath"
