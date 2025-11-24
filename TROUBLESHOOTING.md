# ?? TROUBLESHOOTING - SIGA-PET

## ? PROBLEMAS COMUNS E SOLUÇÕES

### 1. Erro: "DirectoryNotFoundException: wwwroot"

**Erro:**
```
System.IO.DirectoryNotFoundException: C:\...\wwwroot\
```

**Causa:** Pasta wwwroot foi deletada mas o ASP.NET Core ainda a procura.

**Solução Rápida:**
```powershell
# PowerShell
New-Item -Path ".\Backend\wwwroot" -ItemType Directory -Force
```

```cmd
# CMD
mkdir Backend\wwwroot
```

**Solução Permanente:** Os scripts `start-dev.ps1` e `start-dev.bat` agora criam automaticamente.

---

### 2. Warning: "Node.js version v20.15.1 detected"

**Warning:**
```
The Angular CLI requires a minimum Node.js version of v20.19 or v22.12
```

**Causa:** Versão do Node.js levemente desatualizada.

**Status:** ?? **PODE IGNORAR** - O projeto funciona normalmente com v20.15.1

**Recomendação (Opcional):**
- Atualize para Node.js v20.19+ ou v22.12+
- Download: https://nodejs.org/

**Alternativa:** Use NVM (Node Version Manager)
```bash
nvm install 20.19
nvm use 20.19
```

---

### 3. Vulnerabilidades NPM

**Warning:**
```
4 vulnerabilities (3 moderate, 1 high)
```

**Status:** ? **CORRIGIDO AUTOMATICAMENTE**

**Se ainda aparecer:**
```bash
cd Frontend
npm audit fix --force
```

**Nota:** Vulnerabilidades relacionadas ao Vite em ambiente de desenvolvimento são de **baixo risco**.

---

### 4. Porta já em uso

**Erro:**
```
Address already in use
EADDRINUSE
```

**Solução PowerShell:**
```powershell
.\stop-dev.ps1
```

**Solução CMD:**
```cmd
.\stop-dev.bat
```

**Solução Manual:**
```powershell
# Parar Backend
Get-Process -Name "dotnet" | Stop-Process -Force

# Parar Frontend
Get-Process -Name "node" | Stop-Process -Force
```

---

### 5. Erro de Compilação do Backend

**Erro:**
```
Build FAILED
```

**Solução 1 - Limpar e Recompilar:**
```bash
cd Backend
dotnet clean
dotnet build
```

**Solução 2 - Restaurar Pacotes:**
```bash
cd Backend
dotnet restore
dotnet build
```

---

### 6. Erro "Cannot find module '@angular/core'"

**Erro:**
```
Cannot find module '@angular/core'
```

**Solução:**
```bash
cd Frontend
Remove-Item -Recurse -Force node_modules
npm install
```

---

### 7. Banco de Dados não encontrado

**Erro:**
```
Cannot open database
Login failed
```

**Solução:**
```bash
cd Backend
dotnet ef database update
```

**Se não funcionar:**
```bash
cd Backend
dotnet ef database drop -f
dotnet ef database update
```

---

### 8. Erro de CORS no Browser

**Erro no Console:**
```
Access to XMLHttpRequest blocked by CORS policy
```

**Verificações:**
1. Backend está rodando em `https://localhost:7000`?
2. Frontend está rodando em `http://localhost:4200`?
3. Reinicie ambos os servidores

**Solução:**
```powershell
.\stop-dev.ps1
.\start-dev.ps1
```

---

### 9. Certificado SSL não confiável

**Erro no Browser:**
```
NET::ERR_CERT_AUTHORITY_INVALID
Your connection is not private
```

**Solução:**
```bash
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

Ou simplesmente **aceite o aviso no navegador** (é seguro em desenvolvimento).

---

### 10. Frontend não carrega após iniciar

**Problema:** Página em branco ou erro 404

**Causa:** Angular ainda está compilando

**Solução:** Aguarde 10-30 segundos após iniciar

**Verificar se está pronto:**
- Veja a janela do terminal do Frontend
- Espere aparecer: "Application bundle generation complete"

---

### 11. Swagger não abre

**Problema:** https://localhost:7000/swagger retorna erro

**Verificações:**
1. Backend está rodando?
2. Porta 7000 está sendo usada?

**Solução:**
```bash
cd Backend
dotnet run --launch-profile https
```

Aguarde ver: "Now listening on: https://localhost:7000"

---

### 12. "dotnet ef" comando não encontrado

**Erro:**
```
'dotnet-ef' is not recognized
```

**Solução:**
```bash
dotnet tool install --global dotnet-ef
```

---

### 13. Performance lenta do Angular

**Problema:** Frontend demora muito para compilar

**Causa:** Modo desenvolvimento compila em tempo real

**Solução (Build de Produção):**
```bash
cd Frontend
npm run build
```

---

### 14. Erro ao parar servidores

**Problema:** `stop-dev.ps1` não para todos os processos

**Solução Manual:**
```powershell
# Listar processos
Get-Process dotnet
Get-Process node

# Matar todos
Get-Process dotnet | Stop-Process -Force
Get-Process node | Stop-Process -Force
```

---

## ?? COMANDOS DE DIAGNÓSTICO

### Verificar versões instaladas
```bash
# Node.js
node --version

# NPM
npm --version

# .NET
dotnet --version

# Entity Framework Tools
dotnet ef --version

# Angular CLI
ng version
```

### Verificar se portas estão em uso
```powershell
# Porta 7000 (Backend)
netstat -ano | findstr :7000

# Porta 4200 (Frontend)
netstat -ano | findstr :4200
```

### Logs detalhados

**Backend:**
```bash
cd Backend
dotnet run --launch-profile https --verbosity detailed
```

**Frontend:**
```bash
cd Frontend
npm start -- --verbose
```

---

## ?? STATUS DOS PROBLEMAS CONHECIDOS

| Problema | Status | Solução |
|----------|--------|---------|
| wwwroot não existe | ? CORRIGIDO | Script cria automaticamente |
| Node.js v20.15.1 | ?? FUNCIONA | Atualizar é opcional |
| Vulnerabilidades NPM | ? CORRIGIDO | npm audit fix |
| CORS | ? CONFIGURADO | Funciona out-of-the-box |
| SSL Certificate | ?? ESPERADO | Normal em desenvolvimento |

---

## ?? RESET COMPLETO

Se nada funcionar, faça um reset completo:

```powershell
# 1. Parar tudo
.\stop-dev.ps1

# 2. Limpar Backend
cd Backend
dotnet clean
Remove-Item -Recurse -Force bin, obj
dotnet restore
dotnet build

# 3. Limpar Frontend
cd ..\Frontend
Remove-Item -Recurse -Force node_modules, .angular
npm install

# 4. Recriar Banco
cd ..\Backend
dotnet ef database drop -f
dotnet ef database update

# 5. Criar wwwroot
cd ..
if (-not (Test-Path "Backend\wwwroot")) {
    New-Item -Path "Backend\wwwroot" -ItemType Directory -Force
}

# 6. Iniciar
.\start-dev.ps1
```

---

## ?? AINDA COM PROBLEMAS?

1. Verifique os logs nas janelas dos terminais
2. Consulte README.md para requisitos
3. Consulte PRIMEIRO-USO.md para setup inicial
4. Abra uma issue no GitHub com:
   - Mensagem de erro completa
   - Versões instaladas (node, dotnet, npm)
   - Sistema operacional
   - Prints da tela de erro

---

## ? CHECKLIST DE VERIFICAÇÃO

Antes de reportar problema, verifique:

- [ ] Node.js instalado (v20.15+)
- [ ] .NET 8.0 SDK instalado
- [ ] SQL Server LocalDB instalado
- [ ] Pasta wwwroot existe em Backend
- [ ] npm install executado no Frontend
- [ ] dotnet restore executado no Backend
- [ ] Portas 7000 e 4200 disponíveis
- [ ] Certificado SSL confiável
- [ ] Banco de dados criado (dotnet ef database update)

---

**Última atualização:** Hoje
**Problemas corrigidos:** 3 principais
**Status geral:** ? Funcionando
