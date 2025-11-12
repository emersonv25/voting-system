# Etapa 1 — Build da aplicação Next.js
FROM node:22-alpine AS builder

WORKDIR /app

# Recebe argumento do build
ARG NEXT_PUBLIC_API_URL
ENV NEXT_PUBLIC_API_URL=$NEXT_PUBLIC_API_URL

# Copia os arquivos de dependência
COPY package*.json ./

# Instala dependências
RUN npm ci

# Copia o restante do código
COPY . .

# Build do projeto
RUN npm run build

# Etapa 2 — Runtime (produção)
FROM node:22-alpine AS runner
WORKDIR /app

ENV NODE_ENV=production

# Copia os artefatos do build
COPY --from=builder /app/public ./public
COPY --from=builder /app/.next ./.next
COPY --from=builder /app/package*.json ./

# Instala apenas dependências de produção
RUN npm ci --omit=dev

EXPOSE 3010

# Inicia o servidor Next.js
CMD ["npm", "run", "start"]
