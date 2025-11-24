# SIGA-PET - Script para Parar Servidores
Write-Host "========================================" -ForegroundColor Cyan
Write-Host " SIGA-PET - Parando servidores..." -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Parando Backend (dotnet)..." -ForegroundColor Yellow
$dotnetProcesses = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue
if ($dotnetProcesses) {
    $dotnetProcesses | Stop-Process -Force
    Write-Host "Backend parado com sucesso." -ForegroundColor Green
} else {
    Write-Host "Nenhum processo Backend em execucao." -ForegroundColor Gray
}

Write-Host ""
Write-Host "Parando Frontend (node/Angular)..." -ForegroundColor Yellow
$nodeProcesses = Get-Process -Name "node" -ErrorAction SilentlyContinue
if ($nodeProcesses) {
    $nodeProcesses | Stop-Process -Force
    Write-Host "Frontend parado com sucesso." -ForegroundColor Green
} else {
    Write-Host "Nenhum processo Frontend em execucao." -ForegroundColor Gray
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host " Todos os servidores foram parados!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green

Read-Host "Pressione Enter para sair"
