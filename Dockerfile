# ─────────────────────────────────────────────
# Stage 1: Build
# ─────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto e restaura dependências primeiro
# (camada cacheável — só invalida se o .csproj mudar)
COPY API/minimal-api.csproj ./API/
RUN dotnet restore ./API/minimal-api.csproj

# Copia o restante do código e publica
COPY API/ ./API/
WORKDIR /src/API
RUN dotnet publish minimal-api.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

# ─────────────────────────────────────────────
# Stage 2: Runtime (imagem final, bem menor)
# ─────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Cria usuário não-root por segurança
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
USER appuser

COPY --from=build /app/publish .

# A porta que o ASP.NET Core vai escutar dentro do container
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "minimal-api.dll"]