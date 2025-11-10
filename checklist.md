# CHECKLIST – 4ª SPRINT (.NET Advanced Business Development)

## Equipe
- Felipe Orikasa — 557435
- Marcelo Bonfim — 558254
- Antonio Caue — 558891

## Itens
- Projeto compila e roda corretamente (`dotnet build`, `dotnet run`) ✅
- Endpoint `/health` implementado e acessível ✅
- Versionamento `/api/v1` configurado ✅
- Segurança (API Key) implementada e configurável ✅
- Endpoint ML.NET `POST /api/v1/ml/predict` adicionado (modelo placeholder) ✅
- InMemoryStorage incluído (`Mottu.Api/InMemory/InMemoryStorage.cs`) ✅
- README atualizado com arquitetura e instruções ✅
- Swagger atualizado com suporte a ApiKey e Bearer ✅

## Observações finais
- Substitua `MLModels/DeliveryTimeModel.zip` por um modelo ML.NET real para obter previsões reais.
- Mude a chave padrão `DEV_KEY_CHANGE_ME` em `appsettings` antes de publicar.
- Rode `dotnet restore` e `dotnet build` localmente para validar o build antes de submissão.
