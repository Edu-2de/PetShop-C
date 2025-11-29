# Banners do Carousel

Esta pasta contém os banners que aparecem no topo da página inicial (dashboard).

## ⚠️ IMPORTANTE: Erros 404 no Console

**Os erros 404 que aparecem no console são NORMAIS e ESPERADOS!**

O sistema tenta carregar banners de `banner1.jpg` até `banner10.jpg`. Se um banner não existir, ele simplesmente ignora e continua. Esses erros 404 não afetam o funcionamento do site.

**Exemplo:** Se você tem apenas `banner1.jpg` e `banner2.jpg`, verá erros 404 para banner3 até banner10 - isso é **normal**.

## Como adicionar banners

1. Adicione suas imagens nesta pasta com os seguintes nomes:
   - `banner1.jpg` ✅
   - `banner2.jpg` ✅
   - `banner3.jpg` ✅
   - ... até `banner10.jpg`

2. O sistema detecta automaticamente quantos banners existem (de 1 até 10)

3. Recomendações:
   - **Formato**: JPG (não use .jpeg ou .png, apenas .jpg)
   - **Dimensões recomendadas**: 1920x500 pixels (proporção 3.84:1)
   - **Peso máximo**: 500KB por imagem
   - **Conteúdo**: Imagens de produtos, promoções, serviços oferecidos

4. Os banners são exibidos em rotação automática de 5 segundos
   - O usuário pode pausar passando o mouse sobre o banner
   - Navegação por setas (anterior/próximo)
   - Indicadores de posição na parte inferior

## Se não houver banners

Se nenhum banner for encontrado, o sistema exibirá automaticamente uma seção hero padrão com:
- Título "SIGA-PET"
- Descrição
- Botão "Explorar Produtos"

## Estrutura atual

```
Frontend/src/assets/images/carousel/
├── banner1.jpg  ← ✅ Seu primeiro banner
├── banner2.jpg  ← ✅ Seu segundo banner
├── .gitkeep
└── README.md    ← Este arquivo
```

## Como adicionar mais banners

Basta copiar suas imagens para esta pasta seguindo a numeração:
- Se você já tem banner1 e banner2, adicione banner3.jpg
- Se você já tem banner1, banner2 e banner3, adicione banner4.jpg
- E assim por diante até banner10.jpg

## Dica Pro

Para evitar os erros 404 no console (embora sejam inofensivos), adicione apenas a quantidade de banners que você tem. Não precisa ter todos os 10 banners!
