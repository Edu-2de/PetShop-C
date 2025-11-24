@echo off
echo ========================================
echo  SIGA-PET - Parando servidores...
echo ========================================
echo.

echo Parando Backend (dotnet)...
taskkill /F /IM dotnet.exe 2>nul
if %errorlevel% equ 0 (
    echo Backend parado com sucesso.
) else (
    echo Nenhum processo Backend em execucao.
)

echo.
echo Parando Frontend (node/Angular)...
taskkill /F /IM node.exe 2>nul
if %errorlevel% equ 0 (
    echo Frontend parado com sucesso.
) else (
    echo Nenhum processo Frontend em execucao.
)

echo.
echo ========================================
echo  Todos os servidores foram parados!
echo ========================================
pause
