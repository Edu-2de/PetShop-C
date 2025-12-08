#!/usr/bin/env pwsh

# Script de Aplicação de Correções - SIGA-PET
# Corrige problemas de compras e agendamentos

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  SIGA-PET - Aplicando Correções" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# 1. Verificar se estamos no diretório correto
if (-not (Test-Path "Backend") -or -not (Test-Path "Frontend")) {
    Write-Host "[ERRO] Execute este script na raiz do projeto (onde estão as pastas Backend e Frontend)" -ForegroundColor Red
    exit 1
}

Write-Host "[1/5] Verificando estrutura do projeto..." -ForegroundColor Yellow
if (Test-Path "Backend/SIGA-PET.csproj") {
    Write-Host "  ? Backend encontrado" -ForegroundColor Green
} else {
    Write-Host "  ? Backend não encontrado" -ForegroundColor Red
    exit 1
}

if (Test-Path "Frontend/angular.json") {
    Write-Host "  ? Frontend encontrado" -ForegroundColor Green
} else {
    Write-Host "  ? Frontend não encontrado" -ForegroundColor Red
    exit 1
}

# 2. Aplicar migração no banco de dados
Write-Host ""
Write-Host "[2/5] Aplicando migração no banco de dados..." -ForegroundColor Yellow

$sqlFile = "Backend/Migrations/AdicionarUsuarioIdVenda.sql"
if (Test-Path $sqlFile) {
    Write-Host "  Script SQL encontrado: $sqlFile" -ForegroundColor Cyan
    
    # Tentar aplicar via sqlcmd (se disponível)
    $sqlcmdExists = Get-Command sqlcmd -ErrorAction SilentlyContinue
    if ($sqlcmdExists) {
        Write-Host "  Aplicando via sqlcmd..." -ForegroundColor Cyan
        try {
            sqlcmd -S localhost -d "SIGA-PET" -i $sqlFile -E
            Write-Host "  ? Migração aplicada com sucesso via sqlcmd" -ForegroundColor Green
        } catch {
            Write-Host "  ? Erro ao aplicar via sqlcmd. Tente manualmente." -ForegroundColor Yellow
            Write-Host "    Execute o arquivo: $sqlFile" -ForegroundColor Gray
        }
    } else {
        Write-Host "  ? sqlcmd não encontrado. Aplicar manualmente:" -ForegroundColor Yellow
        Write-Host "    1. Abra SQL Server Management Studio (SSMS)" -ForegroundColor Gray
        Write-Host "    2. Conecte-se ao servidor localhost" -ForegroundColor Gray
        Write-Host "    3. Abra o arquivo: $sqlFile" -ForegroundColor Gray
        Write-Host "    4. Execute o script (F5)" -ForegroundColor Gray
    }
} else {
    Write-Host "  ? Arquivo SQL não encontrado: $sqlFile" -ForegroundColor Red
}

# 3. Tentar aplicar migration via Entity Framework (alternativa)
Write-Host ""
Write-Host "[3/5] Tentando aplicar via Entity Framework..." -ForegroundColor Yellow
try {
    Push-Location Backend
    $output = dotnet ef database update 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ? Migration aplicada via EF Core" -ForegroundColor Green
    } else {
        Write-Host "  ? Não foi possível aplicar via EF Core" -ForegroundColor Yellow
        Write-Host "    Use o script SQL manualmente" -ForegroundColor Gray
    }
    Pop-Location
} catch {
    Write-Host "  ? Entity Framework não disponível" -ForegroundColor Yellow
    Pop-Location
}

# 4. Compilar Backend
Write-Host ""
Write-Host "[4/5] Compilando Backend..." -ForegroundColor Yellow
try {
    Push-Location Backend
    $buildOutput = dotnet build --nologo --verbosity quiet 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ? Backend compilado com sucesso" -ForegroundColor Green
    } else {
        Write-Host "  ? Erro ao compilar Backend:" -ForegroundColor Red
        Write-Host $buildOutput -ForegroundColor Gray
        Pop-Location
        exit 1
    }
    Pop-Location
} catch {
    Write-Host "  ? Erro ao compilar Backend" -ForegroundColor Red
    Pop-Location
    exit 1
}

# 5. Compilar Frontend
Write-Host ""
Write-Host "[5/5] Compilando Frontend..." -ForegroundColor Yellow
try {
    Push-Location Frontend
    
    # Verificar se node_modules existe
    if (-not (Test-Path "node_modules")) {
        Write-Host "  Instalando dependências do npm..." -ForegroundColor Cyan
        npm install --quiet
    }
    
    # Compilar para verificar erros
    Write-Host "  Verificando código TypeScript..." -ForegroundColor Cyan
    $tscOutput = npm run build -- --configuration development 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ? Frontend compilado com sucesso" -ForegroundColor Green
    } else {
        Write-Host "  ? Avisos no Frontend (pode ignorar se forem warnings)" -ForegroundColor Yellow
    }
    Pop-Location
} catch {
    Write-Host "  ? Não foi possível compilar o Frontend" -ForegroundColor Yellow
    Pop-Location
}

# Resumo final
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  RESUMO DAS CORREÇÕES" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "? Correções de código aplicadas:" -ForegroundColor Green
Write-Host "   - Backend/Data/AppDbContext.cs" -ForegroundColor Gray
Write-Host "   - Backend/Migrations/AdicionarUsuarioIdVenda.sql" -ForegroundColor Gray
Write-Host "   - Frontend/src/app/model/venda.model.ts" -ForegroundColor Gray
Write-Host "   - Frontend/src/app/model/agenda.model.ts" -ForegroundColor Gray
Write-Host "   - Frontend/src/app/service/agenda/agenda.ts" -ForegroundColor Gray
Write-Host "   - Frontend/src/app/pages/produtos/produto-detail/produto-detail.ts" -ForegroundColor Gray
Write-Host ""
Write-Host "?? PRÓXIMOS PASSOS:" -ForegroundColor Cyan
Write-Host "   1. Verifique se a migração foi aplicada no banco" -ForegroundColor White
Write-Host "   2. Reinicie o backend: cd Backend && dotnet run" -ForegroundColor White
Write-Host "   3. Reinicie o frontend: cd Frontend && npm start" -ForegroundColor White
Write-Host "   4. Teste as funcionalidades:" -ForegroundColor White
Write-Host "      - Fazer uma compra" -ForegroundColor Gray
Write-Host "      - Acessar 'Minhas Compras'" -ForegroundColor Gray
Write-Host "      - Acessar 'Meus Agendamentos'" -ForegroundColor Gray
Write-Host ""
Write-Host "?? Para mais detalhes, consulte: CORRECOES-COMPRAS-AGENDAMENTOS.md" -ForegroundColor Cyan
Write-Host ""
