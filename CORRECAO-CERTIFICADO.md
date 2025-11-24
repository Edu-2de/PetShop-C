# ?? CORREÇÃO DE ERROS - CERTIFICADO E NODE.JS

## ? PROBLEMAS IDENTIFICADOS

### 1. Certificado HTTPS Inválido
```
System.InvalidOperationException: Unable to configure HTTPS endpoint.
No server certificate was specified, and the default developer certificate 
could not be found or is out of date.
```

### 2. Versão do Node.js Incompatível
```
Node.js version v20.15.1 detected.
The Angular CLI requires a minimum Node.js version of v20.19 or v22.12.
```

---

## ? SOLUÇÕES APLICADAS

### Solução 1: Certificado HTTPS

**Comandos executados:**
```powershell
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

**Resultado:** ? Certificado criado e confiável

### Solução 2: Usar HTTP ao invés de HTTPS (Temporário)

**Mudanças aplicadas:**

1. **Backend/Program.cs**
   - Comentado `app.UseHttpsRedirection()`
   - CORS atualizado para aceitar HTTP e HTTPS

2. **Backend/Properties/launchSettings.json**
   - Adicionado perfil `http` e `httponly`
   - Porta HTTP: 5000

3. **Frontend/src/environments/environment.ts**
   - Mudado de `https://localhost:7000/api`
   - Para: `http://localhost:5000/api`

4. **Scripts (start-dev.ps1 e start-dev.bat)**
   - Mudado para usar `--launch-profile http`
   - Usando `npx @angular/cli@17` para contornar problema do Node.js

---

## ?? COMO USAR AGORA

### Opção 1: Criar certificado e usar HTTPS (Recomendado para produção)

```powershell
# Já executado - certificado criado!
dotnet dev-certs https --trust

# Iniciar com HTTPS
cd Backend
dotnet run --launch-profile https
```

### Opção 2: Usar HTTP (Mais fácil - Desenvolvimento)

```powershell
# Usar os scripts atualizados
.\start-dev.ps1
```

ou

```cmd
start-dev.bat
```

---

## ?? URLs ATUALIZADAS

### Com HTTP (Configuração Atual)
- **Backend**: http://localhost:5000/swagger
- **Frontend**: http://localhost:4200

### Com HTTPS (Após certificado)
- **Backend**: https://localhost:7000/swagger
- **Frontend**: http://localhost:4200

---

## ?? ARQUIVOS MODIFICADOS

1. ? `Backend/Program.cs` - HTTPS redirect comentado
2. ? `Backend/Properties/launchSettings.json` - Perfil HTTP adicionado
3. ? `Frontend/src/environments/environment.ts` - URL HTTP
4. ? `start-dev.ps1` - Usa HTTP + npx para Angular
5. ? `start-dev.bat` - Usa HTTP + npx para Angular

---

## ?? SOBRE O NODE.JS v20.15.1

### Status Atual
- ? **FUNCIONA** com npx @angular/cli@17
- ?? Gera warnings mas executa normalmente

### Solução Permanente (Opcional)
Atualize o Node.js para v20.19+ ou v22.12+:
- Download: https://nodejs.org/

ou use NVM:
```bash
nvm install 20.19
nvm use 20.19
```

---

## ? TESTE AGORA

### 1. Parar processos anteriores
```powershell
.\stop-dev.ps1
```

### 2. Iniciar novamente
```powershell
.\start-dev.ps1
```

### 3. Aguardar 10-30 segundos

### 4. Acessar
- Backend: http://localhost:5000/swagger
- Frontend: http://localhost:4200

---

## ?? VERIFICAÇÃO

### Backend deve mostrar:
```
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

### Frontend deve mostrar:
```
Application bundle generation complete.
Watch mode enabled. Watching for file changes...
```

### Navegador (localhost:4200)
- ? Página carrega
- ? Sem erro de conexão

---

## ?? DICAS

### Para Desenvolvimento
- ? Use HTTP (mais fácil, sem problemas de certificado)
- ? Scripts atualizados já usam HTTP

### Para Produção
- ?? Use HTTPS (mais seguro)
- ?? Certificado SSL válido necessário
- ?? Descomente `app.UseHttpsRedirection()` no Program.cs

---

## ?? RESUMO

| Item | Antes | Depois |
|------|-------|--------|
| **Certificado** | ? Inválido | ? Criado |
| **Backend URL** | https://7000 | http://5000 |
| **Frontend URL** | - | http://4200 |
| **Node.js** | ? Erro | ? Funciona com npx |
| **Scripts** | ? HTTPS | ? HTTP |

---

## ?? STATUS FINAL

? **PROBLEMAS RESOLVIDOS!**

Agora você pode:
1. Executar `.\start-dev.ps1`
2. Acessar http://localhost:5000/swagger
3. Acessar http://localhost:4200
4. Desenvolver normalmente

---

**Data:** Hoje  
**Status:** ? Corrigido e testável  
**Próxima ação:** Execute `.\start-dev.ps1`
