@echo off
echo ========================================
echo  SIGA-PET - Sistema Integrado de Gestao
echo  Iniciando Backend e Frontend...
echo ========================================
echo.

REM Matar processos anteriores se existirem
echo Limpando processos anteriores...
taskkill /F /IM dotnet.exe 2>nul
taskkill /F /IM node.exe 2>nul
timeout /t 2 /nobreak > nul

echo [1/3] Verificando estrutura do projeto...
REM Criar wwwroot se não existir
if not exist "Backend\wwwroot" (
    mkdir "Backend\wwwroot"
    echo   OK - Pasta wwwroot criada
)

echo.
echo [2/3] Iniciando Backend (http://localhost:5000)...
cd Backend
start "SIGA-PET Backend" cmd /k "echo === SIGA-PET Backend === && echo Aguarde o inicio do servidor... && echo. && dotnet run --launch-profile http"

timeout /t 8 /nobreak > nul

echo.
echo [3/3] Iniciando Frontend (http://localhost:4200)...
cd ..\Frontend
start "SIGA-PET Frontend" cmd /k "echo === SIGA-PET Frontend === && echo Aguarde o inicio do servidor... && echo NOTA: Warnings de versao do Node podem aparecer mas serao ignorados && echo. && npx -y @angular/cli@17 serve --port 4200"

cd ..

echo.
echo ========================================
echo  Aplicacao iniciada com sucesso!
echo.
echo  Backend:  http://localhost:5000/swagger
echo  Frontend: http://localhost:4200
echo.
echo  NOTA: Usando HTTP (sem SSL) para evitar problemas
echo        de certificado. Para producao, use HTTPS.
echo.
echo  NOTA sobre Node.js: v20.15.1 pode gerar warnings
echo        mas funciona normalmente. Atualizacao opcional.
echo.
echo  Aguarde 10-30 segundos para os servidores iniciarem
echo.
echo  Pressione qualquer tecla para fechar este terminal
echo  (Os servidores continuarao rodando)
echo ========================================
pause > nul
