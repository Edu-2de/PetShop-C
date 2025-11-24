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
echo [2/3] Iniciando Backend (https://localhost:7000)...
cd Backend
start "SIGA-PET Backend" cmd /k "echo === SIGA-PET Backend === && echo Aguarde o inicio do servidor... && echo. && dotnet run --launch-profile https"

timeout /t 5 /nobreak > nul

echo.
echo [3/3] Iniciando Frontend (http://localhost:4200)...
cd ..\Frontend
start "SIGA-PET Frontend" cmd /k "echo === SIGA-PET Frontend === && echo Aguarde o inicio do servidor... && echo NOTA: Warnings de versao do Node sao esperados e podem ser ignorados && echo. && npm start"

cd ..

echo.
echo ========================================
echo  Aplicacao iniciada com sucesso!
echo.
echo  Backend:  https://localhost:7000/swagger
echo  Frontend: http://localhost:4200
echo.
echo  NOTA: O Node.js v20.15.1 funciona, mas e recomendado
echo        atualizar para v20.19+ ou v22.12+ futuramente
echo.
echo  Aguarde 10-20 segundos para os servidores iniciarem
echo.
echo  Pressione qualquer tecla para fechar este terminal
echo  (Os servidores continuarao rodando)
echo ========================================
pause > nul
