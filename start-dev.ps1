# SIGA-PET - Script de Inicializacao
Write-Host "========================================" -ForegroundColor Cyan
Write-Host " SIGA-PET - Sistema Integrado de Gestao" -ForegroundColor Cyan
Write-Host " Iniciando Backend e Frontend..." -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Matar processos anteriores se existirem
Write-Host "Limpando processos anteriores..." -ForegroundColor Yellow
Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force
Get-Process -Name "node" -ErrorAction SilentlyContinue | Stop-Process -Force
Start-Sleep -Seconds 2

Write-Host "[1/3] Verificando estrutura do projeto..." -ForegroundColor Yellow
# Criar wwwroot se não existir
if (-not (Test-Path ".\Backend\wwwroot")) {
    New-Item -Path ".\Backend\wwwroot" -ItemType Directory -Force | Out-Null
    Write-Host "  ? Pasta wwwroot criada" -ForegroundColor Green
}

Write-Host ""
Write-Host "[2/3] Iniciando Backend (http://localhost:5000)..." -ForegroundColor Yellow
Set-Location -Path ".\Backend"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Write-Host '=== SIGA-PET Backend ===' -ForegroundColor Cyan; Write-Host 'Aguarde o inicio do servidor...' -ForegroundColor Yellow; Write-Host ''; dotnet run --launch-profile http"

Start-Sleep -Seconds 8

Write-Host ""
Write-Host "[3/3] Iniciando Frontend (http://localhost:4200)..." -ForegroundColor Yellow
Set-Location -Path "..\Frontend"

# Forçar uso do Angular mesmo com Node.js v20.15.1
$env:NG_FORCE_TTY="true"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "`$env:NG_FORCE_TTY='true'; Write-Host '=== SIGA-PET Frontend ===' -ForegroundColor Cyan; Write-Host 'Aguarde o inicio do servidor...' -ForegroundColor Yellow; Write-Host 'NOTA: Warnings de versao do Node podem aparecer mas serao ignorados' -ForegroundColor Gray; Write-Host ''; npx -y @angular/cli@17 serve --port 4200"

Set-Location -Path ".."

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host " Aplicacao iniciada com sucesso!" -ForegroundColor Green
Write-Host "" -ForegroundColor Green
Write-Host " Backend:  http://localhost:5000/swagger" -ForegroundColor White
Write-Host " Frontend: http://localhost:4200" -ForegroundColor White
Write-Host "" -ForegroundColor Green
Write-Host " NOTA: Usando HTTP (sem SSL) para evitar problemas" -ForegroundColor Yellow
Write-Host "       de certificado. Para producao, use HTTPS." -ForegroundColor Yellow
Write-Host "" -ForegroundColor Green
Write-Host " NOTA sobre Node.js: v20.15.1 pode gerar warnings" -ForegroundColor Yellow
Write-Host "       mas funciona normalmente. Atualizacao opcional." -ForegroundColor Yellow
Write-Host "" -ForegroundColor Green
Write-Host " Aguarde 10-30 segundos para os servidores iniciarem" -ForegroundColor Cyan
Write-Host "" -ForegroundColor Green
Write-Host " Pressione qualquer tecla para fechar este terminal" -ForegroundColor Yellow
Write-Host " (Os servidores continuarao rodando)" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Green

Read-Host
