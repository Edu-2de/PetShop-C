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
Write-Host "[2/3] Iniciando Backend (https://localhost:7000)..." -ForegroundColor Yellow
Set-Location -Path ".\Backend"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Write-Host '=== SIGA-PET Backend ===' -ForegroundColor Cyan; Write-Host 'Aguarde o inicio do servidor...' -ForegroundColor Yellow; Write-Host ''; dotnet run --launch-profile https"

Start-Sleep -Seconds 5

Write-Host ""
Write-Host "[3/3] Iniciando Frontend (http://localhost:4200)..." -ForegroundColor Yellow
Set-Location -Path "..\Frontend"

# Configurar NODE_OPTIONS para ignorar warnings de versão
$env:NODE_OPTIONS="--no-warnings"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "`$env:NODE_OPTIONS='--no-warnings'; Write-Host '=== SIGA-PET Frontend ===' -ForegroundColor Cyan; Write-Host 'Aguarde o inicio do servidor...' -ForegroundColor Yellow; Write-Host 'NOTA: Warnings de versao do Node sao esperados e podem ser ignorados' -ForegroundColor Gray; Write-Host ''; npm start"

Set-Location -Path ".."

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host " Aplicacao iniciada com sucesso!" -ForegroundColor Green
Write-Host "" -ForegroundColor Green
Write-Host " Backend:  https://localhost:7000/swagger" -ForegroundColor White
Write-Host " Frontend: http://localhost:4200" -ForegroundColor White
Write-Host "" -ForegroundColor Green
Write-Host " NOTA: O Node.js v20.15.1 funciona, mas e recomendado" -ForegroundColor Yellow
Write-Host "       atualizar para v20.19+ ou v22.12+ futuramente" -ForegroundColor Yellow
Write-Host "" -ForegroundColor Green
Write-Host " Aguarde 10-20 segundos para os servidores iniciarem" -ForegroundColor Cyan
Write-Host "" -ForegroundColor Green
Write-Host " Pressione qualquer tecla para fechar este terminal" -ForegroundColor Yellow
Write-Host " (Os servidores continuarao rodando)" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Green

Read-Host
