Write-Host "================================"
Write-Host "  Semgrep - Análise de Segurança"
Write-Host "================================"

Write-Host ""
Write-Host "[1/2] Rodando regras customizadas..."
semgrep --config .semgrep.yml --output semgrep-custom-report.json --json API/ Test/ 

Write-Host ""
Write-Host "[2/2] Rodando ruleset oficial (p/csharp)..."
semgrep --config p/csharp --output semgrep-full-report.json --json API/ Test/ 

Write-Host ""
Write-Host "Análise concluída!"
Write-Host "Relatórios gerados:"
Write-Host "  - semgrep-custom-report.json"
Write-Host "  - semgrep-full-report.json"