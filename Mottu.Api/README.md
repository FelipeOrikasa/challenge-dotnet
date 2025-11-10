# Mottu.Api

Atualizações adicionadas para a submissão:
- Endpoint de Health Checks: `/health`
- Versionamento de API via URL (`/api/v{version}/...`) e header `x-api-version`
- Autenticação JWT (configurar `Jwt:Key` em appsettings)
- Endpoint de ML.NET: `GET /api/v1/ml/predict?feature1=1&feature2=2` (modelo placeholder incluso)
- Swagger atualizado para suportar autenticação Bearer
- Testes: projeto de testes xUnit em `/tests` com `WebApplicationFactory`

## Como compilar e executar

Requisitos:
- .NET 7 SDK instalado

No diretório raiz do projeto:
```bash
dotnet build Mottu.Api.sln
dotnet run --project Mottu.Api
```

## Testes

Para executar os testes:
```bash
cd tests
dotnet test
```

## Arquitetura

Veja o arquivo `../arquitetura_diagrama.pdf` (diagrama de alto nível) na raiz do repositório.


## Nota sobre testes

Os testes de integração configurados usam um fallback para **InMemory database** quando não há `Oracle` connection string disponível. Isso permite executar `dotnet test` sem precisar de um banco Oracle local.


### ML.NET - Estimativa de Tempo de Entrega

Adicionado endpoint para previsão de tempo de entrega usando ML.NET:

- **Endpoint**: `POST /api/v1/ml/predict`
- **Entrada (JSON)**:
```json
{
  "DistanceKm": 12.5,
  "PackageWeightKg": 2.3,
  "VehicleType": "motorcycle"
}
```
- **Resposta (200 OK)**:
```json
{
  "estimatedTimeMinutes": 27.4
}
```

> Observação: o repositório inclui um modelo pre-treinado em `MLModels/DeliveryTimeModel.zip` (placeholder). Substitua por um modelo real para resultados precisos.
