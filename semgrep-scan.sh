#!/bin/bash

echo "================================"
echo "  Semgrep - Análise de Segurança"
echo "================================"

# Roda regras customizadas
echo ""
echo "[1/2] Rodando regras customizadas..."
semgrep --config .semgrep.yml --output semgrep-custom-report.json --json API/ Test/ 

# Roda ruleset oficial OWASP para .NET
echo ""
echo "[2/2] Rodando ruleset oficial (p/csharp)..."
semgrep --config p/csharp --output semgrep-full-report.json --json API/ Test/ 

echo ""
echo "Análise concluída!"
echo "Relatórios gerados:"
echo "  - semgrep-custom-report.json"
echo "  - semgrep-full-report.json"