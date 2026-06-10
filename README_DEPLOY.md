# ClickHome - Deploy

Este repositorio contem:

- Backend ASP.NET em `Imobiliaria/`
- Frontend estatico em `frontend/`
- `Dockerfile` na raiz para publicar a API no Render ou Railway

## Backend

No Render/Railway, crie um Web Service usando Docker.

Configuracao sugerida:

- Root directory: raiz do repositorio
- Dockerfile: `Dockerfile`
- Porta: usar a variavel `PORT` da plataforma

Depois do deploy, a API ficara em uma URL parecida com:

```text
https://seu-backend.onrender.com/api
```

## Frontend

Antes de publicar o frontend, edite:

```text
frontend/api-config.js
```

e troque a URL local pela URL publica do backend:

```js
window.CLICKHOME_API_BASE =
  window.CLICKHOME_API_BASE || "https://seu-backend.onrender.com/api";
```

Na Vercel ou Netlify:

- Root directory: `frontend`
- Build command: deixe vazio
- Publish directory: `.`

## Observacao

O backend usa armazenamento em memoria. Usuarios, favoritos e imoveis criados podem sumir quando o servidor reiniciar.
