#!/bin/bash
# =============================================================
#  setup-azure.sh
#  Cria toda a infraestrutura Azure para o CD do vehicle-management
#  Pré-requisito: az cli instalado e logado (az login)
# =============================================================

set -e  # Para em qualquer erro

# ─── VARIÁVEIS — edite aqui antes de rodar ───────────────────
RESOURCE_GROUP="rg-vehicle-management"
LOCATION="eastus"                          # ou brazilsouth (mais caro ~20%)
ACR_NAME="acrvehicle$RANDOM"               # nome único no Azure
CONTAINER_APP_ENV="cae-vehicle"
CONTAINER_APP_NAME="vehicle-api"
STATIC_WEB_APP_NAME="swa-vehicle-frontend"
# ─────────────────────────────────────────────────────────────

echo ""
echo "╔══════════════════════════════════════════════╗"
echo "║   Setup Azure — Vehicle Management           ║"
echo "╚══════════════════════════════════════════════╝"
echo ""

# 1. Resource Group
echo "▶ [1/6] Criando Resource Group..."
az group create \
  --name "$RESOURCE_GROUP" \
  --location "$LOCATION" \
  --output none

echo "  ✅ Resource Group: $RESOURCE_GROUP"

# 2. Azure Container Registry (SKU Basic = ~R$15/mês, 10 GB free)
echo ""
echo "▶ [2/6] Criando Azure Container Registry..."
az acr create \
  --resource-group "$RESOURCE_GROUP" \
  --name "$ACR_NAME" \
  --sku Basic \
  --admin-enabled true \
  --output none

ACR_LOGIN_SERVER=$(az acr show --name "$ACR_NAME" --query loginServer -o tsv)
echo "  ✅ ACR: $ACR_LOGIN_SERVER"

# 3. Container Apps Environment (infraestrutura compartilhada)
echo ""
echo "▶ [3/6] Criando Container Apps Environment..."
az containerapp env create \
  --name "$CONTAINER_APP_ENV" \
  --resource-group "$RESOURCE_GROUP" \
  --location "$LOCATION" \
  --output none

echo "  ✅ Environment: $CONTAINER_APP_ENV"

# 4. Container App (backend) — imagem placeholder, o pipeline atualiza depois
echo ""
echo "▶ [4/6] Criando Container App (backend)..."
az containerapp create \
  --name "$CONTAINER_APP_NAME" \
  --resource-group "$RESOURCE_GROUP" \
  --environment "$CONTAINER_APP_ENV" \
  --image "mcr.microsoft.com/dotnet/samples:aspnetapp" \
  --target-port 8080 \
  --ingress external \
  --min-replicas 0 \
  --max-replicas 2 \
  --cpu 0.5 \
  --memory 1.0Gi \
  --registry-server "$ACR_LOGIN_SERVER" \
  --output none

BACKEND_URL=$(az containerapp show \
  --name "$CONTAINER_APP_NAME" \
  --resource-group "$RESOURCE_GROUP" \
  --query "properties.configuration.ingress.fqdn" -o tsv)

echo "  ✅ Backend URL: https://$BACKEND_URL"

# 5. Static Web App (frontend Angular)
echo ""
echo "▶ [5/6] Criando Static Web App (frontend)..."
az staticwebapp create \
  --name "$STATIC_WEB_APP_NAME" \
  --resource-group "$RESOURCE_GROUP" \
  --location "eastus2" \
  --sku Free \
  --output none

SWA_TOKEN=$(az staticwebapp secrets list \
  --name "$STATIC_WEB_APP_NAME" \
  --query "properties.apiKey" -o tsv)

SWA_URL=$(az staticwebapp show \
  --name "$STATIC_WEB_APP_NAME" \
  --query "defaultHostname" -o tsv)

echo "  ✅ Frontend URL: https://$SWA_URL"

# 6. Credenciais do ACR para o pipeline
echo ""
echo "▶ [6/6] Coletando credenciais para o Azure DevOps..."
ACR_USERNAME=$(az acr credential show --name "$ACR_NAME" --query username -o tsv)
ACR_PASSWORD=$(az acr credential show --name "$ACR_NAME" --query "passwords[0].value" -o tsv)

# ─── RESUMO FINAL ────────────────────────────────────────────
echo ""
echo "╔══════════════════════════════════════════════════════════════╗"
echo "║   ✅ Infraestrutura criada com sucesso!                      ║"
echo "╠══════════════════════════════════════════════════════════════╣"
echo "║  Guarde estas informações — você vai precisar delas          ║"
echo "║  para configurar as variáveis no Azure DevOps:               ║"
echo "╠══════════════════════════════════════════════════════════════╣"
echo "║"
echo "║  ACR_LOGIN_SERVER  = $ACR_LOGIN_SERVER"
echo "║  ACR_USERNAME      = $ACR_USERNAME"
echo "║  ACR_PASSWORD      = $ACR_PASSWORD"
echo "║"
echo "║  CONTAINER_APP_NAME      = $CONTAINER_APP_NAME"
echo "║  RESOURCE_GROUP          = $RESOURCE_GROUP"
echo "║"
echo "║  SWA_DEPLOYMENT_TOKEN    = $SWA_TOKEN"
echo "║"
echo "║  BACKEND_URL   = https://$BACKEND_URL"
echo "║  FRONTEND_URL  = https://$SWA_URL"
echo "╚══════════════════════════════════════════════════════════════╝"
echo ""
echo "  📋 PRÓXIMO PASSO: No Azure DevOps, vá em:"
echo "     Pipelines → Library → + Variable Group"
echo "     Crie um grupo chamado 'vehicle-cd-secrets' e adicione"
echo "     todas as variáveis acima como Secret Variables."
echo ""
echo "  📋 TAMBÉM: Configure a Service Connection em:"
echo "     Project Settings → Service Connections → New → Azure Resource Manager"
echo "     Nome: azure-vehicle-connection"
echo ""
