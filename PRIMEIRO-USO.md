# ?? PRIMEIRO USO - SIGA-PET

## ?? Pré-requisitos - VERIFIQUE ANTES DE COMEÇAR

### 1. Verificar .NET instalado
```powershell
dotnet --version
```
**Esperado**: 8.0 ou superior

### 2. Verificar Node.js instalado
```powershell
node --version
```
**Esperado**: 18.0 ou superior

### 3. Verificar NPM instalado
```powershell
npm --version
```
**Esperado**: 9.0 ou superior

### 4. Verificar Angular CLI instalado
```powershell
ng version
```
**Se não estiver instalado**:
```powershell
npm install -g @angular/cli
```

---

## ?? CONFIGURAÇÃO INICIAL (FAÇA UMA VEZ)

### Passo 1: Instalar dependências do Frontend
```powershell
cd Frontend
npm install
cd ..
```

### Passo 2: Restaurar pacotes do Backend
```powershell
cd Backend
dotnet restore
cd ..
```

### Passo 3: Criar o banco de dados
```powershell
cd Backend
dotnet ef database update
cd ..
```

**?? Se o comando acima falhar**, instale a ferramenta EF Core:
```powershell
dotnet tool install --global dotnet-ef
```

---

## ?? INICIAR A APLICAÇÃO

### Opção 1: Script Automático (RECOMENDADO)

**PowerShell (Recomendado):**
```powershell
.\start-dev.ps1
```

**CMD:**
```cmd
start-dev.bat
```

**O script fará automaticamente:**
1. ? Restaurar pacotes do backend
2. ? Instalar dependências do frontend
3. ? Iniciar backend na porta 7000
4. ? Iniciar frontend na porta 4200

### Opção 2: Manual

**Abra 2 terminais:**

**Terminal 1 - Backend:**
```powershell
cd Backend
dotnet run --launch-profile https
```

**Terminal 2 - Frontend:**
```powershell
cd Frontend
npm start
```

---

## ?? ACESSAR A APLICAÇÃO

Após iniciar, aguarde alguns segundos e acesse:

### Frontend (Aplicação Angular)
```
http://localhost:4200
```

### Backend API (Swagger)
```
https://localhost:7000/swagger
```

### Backend API (Base URL)
```
https://localhost:7000/api
```

---

## ? TESTAR SE ESTÁ FUNCIONANDO

### 1. Testar Backend (Swagger)

1. Acesse: https://localhost:7000/swagger
2. Você verá a lista de todos os endpoints
3. Teste o endpoint `GET /api/Tutor`:
   - Clique em "GET /api/Tutor"
   - Clique em "Try it out"
   - Clique em "Execute"
   - Deve retornar uma lista (vazia ou com dados)

### 2. Testar Frontend

1. Acesse: http://localhost:4200
2. A página inicial deve carregar
3. Navegue pelos menus

### 3. Testar Integração

**No Swagger:**
1. Vá em `POST /api/Tutor`
2. Clique em "Try it out"
3. Cole este JSON:
```json
{
  "nome": "João Silva",
  "telefone": "(11) 98765-4321",
  "email": "joao@exemplo.com",
  "endereco": "Rua Exemplo, 123"
}
```
4. Clique em "Execute"
5. Deve retornar status 201 (Created)

**No Frontend:**
1. Navegue até a lista de tutores
2. O tutor "João Silva" deve aparecer na lista

---

## ?? PARAR A APLICAÇÃO

### Usando Script
```powershell
.\stop-dev.ps1
```
ou
```cmd
stop-dev.bat
```

### Manual
Pressione `Ctrl+C` em cada terminal aberto

---

## ?? PROBLEMAS COMUNS E SOLUÇÕES

### ? Erro: "Porta já em uso"

**Solução:**
```powershell
# PowerShell
.\stop-dev.ps1

# Ou manualmente
Get-Process -Name "dotnet" | Stop-Process -Force
Get-Process -Name "node" | Stop-Process -Force
```

### ? Erro: "Cannot find module '@angular/core'"

**Solução:**
```powershell
cd Frontend
Remove-Item -Recurse -Force node_modules
npm install
```

### ? Erro: "Connection to database failed"

**Solução 1 - Recriar banco:**
```powershell
cd Backend
dotnet ef database drop -f
dotnet ef database update
```

**Solução 2 - Verificar SQL Server:**
- Certifique-se que o SQL Server LocalDB está instalado
- Execute: `sqllocaldb start mssqllocaldb`

### ? Erro: "CORS policy"

**Solução:**
- Verifique se o backend está rodando em https://localhost:7000
- Verifique se o frontend está acessando a URL correta
- Reinicie ambos os servidores

### ? Erro: "Certificate is not trusted"

**Solução:**
```powershell
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

### ? Erro: "dotnet ef not found"

**Solução:**
```powershell
dotnet tool install --global dotnet-ef
```

---

## ?? VERIFICAÇÃO DE SAÚDE

Execute estes comandos para verificar se tudo está OK:

```powershell
# Verificar backend
cd Backend
dotnet build

# Verificar frontend
cd ../Frontend
npm run build
```

**Ambos devem compilar sem erros!**

---

## ?? PRÓXIMOS PASSOS

Agora que tudo está funcionando:

1. ? Explore o Swagger (https://localhost:7000/swagger)
2. ? Teste todos os endpoints
3. ? Navegue pelo frontend
4. ? Crie alguns dados de teste
5. ? Experimente as funcionalidades

---

## ?? DOCUMENTAÇÃO ADICIONAL

- **README.md** - Documentação completa
- **GUIA-RAPIDO.md** - Comandos úteis
- **CHECKLIST.md** - Verificação de funcionalidades
- **RESUMO.md** - Resumo das alterações

---

## ?? DICAS

### Desenvolvimento

1. **Mantenha 2 terminais abertos**: um para backend, outro para frontend
2. **Use o Swagger**: teste a API antes de usar no frontend
3. **Verifique o console do navegador**: mostra erros do frontend
4. **Verifique o terminal do backend**: mostra erros da API

### Produtividade

1. Use `start-dev.ps1` para iniciar tudo de uma vez
2. Use `stop-dev.ps1` quando terminar
3. Deixe o Swagger aberto em uma aba para referência
4. Use hot reload (código atualiza automaticamente)

---

## ?? SUPORTE

Se encontrar problemas:

1. Consulte a seção "PROBLEMAS COMUNS" acima
2. Verifique os logs no terminal
3. Consulte a documentação (README.md)
4. Abra uma issue no GitHub

---

## ?? PRONTO!

**Seu ambiente está configurado e funcionando!**

Comece a desenvolver ou usar a aplicação. Boa sorte! ??
