# 🏍️ Mottu API - Gerenciamento de Pátios e Frotas

## 📖 Descrição do Projeto

Mottu API é um sistema de back-end desenvolvido em **ASP.NET Core 8** para o gerenciamento e rastreamento de frotas de motocicletas em um sistema de pátios. A API permite o cadastro e a gestão de **filiais, pátios, sensores, motos, entregadores e locações**, além de registrar o histórico de localização de cada veículo ao passar por um sensor.

O projeto foi construído utilizando uma arquitetura em camadas bem definida, com foco em boas práticas de desenvolvimento como **Separação de Responsabilidades (SoC)**, **Injeção de Dependência (DI)** e o uso do padrão de **Repositório e Serviços**, visando garantir um código limpo, testável e de fácil manutenção.

---

## 👨‍💻 Integrantes

- Felipe Gomes Costa Orikasa - RM: 557435
- Marcelo Siqueira Bonfim - RM: 558254
- Antônio Cauê Araújo da Silva - RM: 558891

---

## 🏗️ Arquitetura do Projeto

Foi implementada uma **Arquitetura em Camadas (Layered Architecture)**, que organiza o código em áreas de responsabilidade distintas, desacoplando a lógica de negócio das demais partes da aplicação.

### Estrutura de Pastas

```
Mottu.Api/
├── Controllers/          # Recebe requisições HTTP e retorna respostas
├── Services/            # Lógica de negócio da aplicação
├── Repositories/        # Acesso a dados (Entity Framework Core)
├── Models/              # Entidades do banco de dados
├── DTOs/                # Objetos de transferência de dados
├── Mappers/             # Perfis do AutoMapper
├── Data/                # DbContext e DataSeeder
├── Utils/               # Classes utilitárias (ApiResponse, etc.)
└── MLModels/            # Modelos de Machine Learning (ML.NET)
```

### Benefícios

- **Manutenibilidade**: Cada componente possui responsabilidade única
- **Testabilidade**: Serviços podem ser testados de forma isolada
- **Desacoplamento**: Negócio independente do banco de dados
- **Escalabilidade**: Fácil adicionar novas funcionalidades

---

## 🛠️ Tecnologias Utilizadas

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core 8**
- **Oracle Database**
- **AutoMapper**
- **Swagger/OpenAPI 3.0.1**
- **ML.NET** (para predições de tempo de entrega)
- **JWT Authentication**
- **xUnit** (para testes)

---

## 🚀 Como Executar o Projeto

### Pré-requisitos

- .NET 8 SDK
- Git
- Banco de Dados Oracle (ou acesso ao servidor Oracle da FIAP)

### Passo a Passo

1. **Clonar o repositório**

```bash
git clone <url-do-repositorio>
cd projeto_final_sprint4
```

2. **Configurar a Conexão com o Banco de Dados**

No arquivo `Mottu.Api/appsettings.json`, configure a connection string:

```json
{
  "ConnectionStrings": {
    "OracleDb": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL;"
  }
}
```

3. **Aplicar as Migrations**

```bash
cd Mottu.Api
dotnet ef database update
```

4. **Executar a Aplicação**

```bash
dotnet run
```

A API estará disponível em: `http://localhost:5287`

5. **Acessar a Documentação Swagger**

```
http://localhost:5287/swagger
```

**🔑 API Key para Acesso ao Swagger:**

Para acessar os endpoints protegidos no Swagger, use a seguinte API Key:

```
DEV_KEY_CHANGE_ME
```

No Swagger UI, clique no botão **"Authorize"** (🔒) e insira:
- **Name**: `X-API-KEY`
- **Value**: `DEV_KEY_CHANGE_ME`

Ou adicione o header manualmente nas requisições:
```
X-API-KEY: DEV_KEY_CHANGE_ME
```

---

## 📋 Endpoints Principais

### 🏢 Filiais

- `GET /api/filiais` - Lista todas as filiais (paginado)
- `GET /api/filiais/{id}` - Busca filial por ID
- `POST /api/filiais` - Cria nova filial
- `PUT /api/filiais/{id}` - Atualiza filial
- `DELETE /api/filiais/{id}` - Remove filial

### 🏗️ Pátios

- `GET /api/filiais/{filialId}/patios` - Lista pátios de uma filial (paginado)
- `GET /api/patios/{id}` - Busca pátio por ID
- `POST /api/patios` - Cria novo pátio
- `PUT /api/patios/{id}` - Atualiza pátio
- `DELETE /api/patios/{id}` - Remove pátio

### 🏍️ Motos

- `GET /api/motos` - Lista todas as motos
- `GET /api/motos/{id}` - Busca moto por ID (Guid)
- `POST /api/motos` - Cadastra nova moto
- `PUT /api/motos/{id}` - Atualiza moto (apenas placa)
- `DELETE /api/motos/{id}` - Remove moto

### 📍 Sensores

- `GET /api/patios/{patioId}/sensores` - Lista sensores de um pátio (paginado)
- `GET /api/sensores/{id}` - Busca sensor por ID
- `POST /api/sensores` - Cadastra novo sensor
- `PUT /api/sensores/{id}` - Atualiza sensor
- `DELETE /api/sensores/{id}` - Remove sensor

### 📍 Localizações

- `GET /api/motos/{motoId}/localizacoes` - Histórico de localizações de uma moto (paginado)
- `GET /api/localizacoes/{id}` - Busca localização por ID (Guid)
- `POST /api/localizacoes` - Registra nova localização
- `DELETE /api/localizacoes/{id}` - Remove localização

### 👤 Entregadores

- `GET /api/entregadores` - Lista todos os entregadores
- `GET /api/entregadores/{id}` - Busca entregador por ID (Guid)
- `POST /api/entregadores` - Cadastra novo entregador
- `PUT /api/entregadores/{id}` - Atualiza entregador
- `DELETE /api/entregadores/{id}` - Remove entregador

### 📝 Locações

- `GET /api/locacao` - Lista todas as locações
- `GET /api/locacao/{id}` - Busca locação por ID (Guid)
- `POST /api/locacao` - Inicia nova locação de moto
- `PUT /api/locacao/{id}/devolucao?dataDevolucao={data}` - Registra devolução de moto

### 🤖 Machine Learning

- `POST /api/v1/ml/predict` - Predição de tempo de entrega (ML.NET)
- `POST /api/v1/prediction/delivery-time` - Predição de tempo de entrega (alternativo)

### 🔐 Autenticação

- `POST /api/auth/login` - Login e obtenção de token JWT

### ❤️ Health Checks

- `GET /health` - Verifica saúde da aplicação

---

## 📌 Exemplos de Uso dos Endpoints

### Criar uma nova Filial

**POST** `/api/filiais`

```json
{
  "nomeFilial": "Mottu - Sede Administrativa",
  "endereco": "Av. Paulista, 1000 - São Paulo/SP"
}
```

### Criar um novo Pátio

**POST** `/api/patios`

```json
{
  "nomePatio": "Pátio Principal - Vistorias",
  "capacidadeMaxima": 50,
  "filialId": 1
}
```

### Cadastrar uma nova Moto

**POST** `/api/motos`

```json
{
  "placa": "XYZ9A87",
  "modelo": "Honda CB 300F",
  "ano": 2025,
  "patioId": 1
}
```

### Cadastrar um Entregador

**POST** `/api/entregadores`

```json
{
  "nome": "João Silva",
  "cnpj": "12.345.678/0001-90",
  "dataNascimento": "1990-05-15",
  "cnh": "12345678901",
  "tipoCNH": "AB",
  "imagemCNH": "/uploads/cnh/joao_silva.jpg"
}
```

### Iniciar uma Locação

**POST** `/api/locacao`

```json
{
  "entregadorId": "guid-do-entregador",
  "motoId": "guid-da-moto",
  "planoDias": 7
}
```

**Planos disponíveis**: 7, 15, 30, 45 ou 50 dias

### Registrar um Evento de Localização

**POST** `/api/localizacoes`

```json
{
  "motoId": "guid-da-moto",
  "sensorId": 1
}
```

### Consultar o Histórico de uma Moto (com paginação)

**GET** `/api/motos/{motoId}/localizacoes?pageNumber=1&pageSize=10`

---

## 🔐 Autenticação e Segurança

### API Key

A API utiliza autenticação por **API Key** para proteger os endpoints. A chave padrão para desenvolvimento é:

```
DEV_KEY_CHANGE_ME
```

**⚠️ IMPORTANTE**: Altere esta chave em produção!

### Como usar a API Key

#### No Swagger UI:
1. Clique no botão **"Authorize"** (🔒) no topo da página
2. No campo `X-API-KEY`, insira: `DEV_KEY_CHANGE_ME`
3. Clique em **"Authorize"** e depois em **"Close"**

#### Em requisições HTTP:
Adicione o header:
```
X-API-KEY: DEV_KEY_CHANGE_ME
```

#### Exemplo com cURL:
```bash
curl -X GET "http://localhost:5287/api/motos" \
  -H "X-API-KEY: DEV_KEY_CHANGE_ME"
```

### JWT Authentication

Alguns endpoints também suportam autenticação JWT. Para obter um token:

**POST** `/api/auth/login`

```json
{
  "email": "usuario@exemplo.com",
  "password": "senha123"
}
```

---

## 🗄️ Banco de Dados

### Oracle Database

O projeto utiliza **Oracle Database** como banco de dados. A connection string está configurada no `appsettings.json`.

### Data Seeder

O projeto inclui um **DataSeeder** que popula automaticamente o banco de dados com dados iniciais na primeira execução:

- **Filiais**: 3 filiais de exemplo
- **Pátios**: 4 pátios distribuídos pelas filiais
- **Sensores**: 5 sensores nos pátios
- **Motos**: 6 motos de exemplo
- **Entregadores**: 4 entregadores
- **Locações**: 3 locações (ativa, finalizada antecipada, finalizada atrasada)
- **Localizações**: 4 registros de localização

O DataSeeder é executado automaticamente ao iniciar a aplicação e só popula tabelas que estão vazias.

---

## 🧪 Testes

A arquitetura permite **testes unitários isolados** nos serviços.

### Executar os testes:

```bash
cd tests
dotnet test
```

Os testes de integração usam um fallback para **InMemory database** quando não há connection string Oracle disponível, permitindo executar os testes sem precisar de um banco Oracle local.

---

## 🤖 Machine Learning (ML.NET)

A API inclui endpoints para predição de tempo de entrega usando **ML.NET**:

### Endpoint Principal

**POST** `/api/v1/ml/predict`

**Request:**
```json
{
  "DistanceKm": 12.5,
  "PackageWeightKg": 2.3,
  "VehicleType": "motorcycle"
}
```

**Response:**
```json
{
  "estimatedTimeMinutes": 27.4
}
```

> **Nota**: O repositório inclui um modelo pré-treinado em `MLModels/DeliveryTimeModel.zip` (placeholder). Substitua por um modelo real para resultados precisos.

---

## 📊 Estrutura de Dados

### Principais Entidades

- **Filial**: Representa uma filial da empresa
- **Patio**: Pátio pertencente a uma filial
- **Sensor**: Sensor instalado em um pátio
- **Moto**: Motocicleta cadastrada no sistema
- **Entregador**: Entregador cadastrado
- **Locacao**: Locação de moto por entregador
- **Localizacao**: Registro de localização de uma moto através de um sensor

### Relacionamentos

- Filial → Patios (1:N)
- Patio → Sensores (1:N)
- Patio → Motos (1:N)
- Entregador → Locacoes (1:N)
- Moto → Locacoes (1:N)
- Sensor → Localizacoes (1:N)

---

## 🔧 Configurações

### appsettings.json

```json
{
  "ConnectionStrings": {
    "OracleDb": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL;"
  },
  "ApiKey": "DEV_KEY_CHANGE_ME",
  "Swagger": {
    "Title": "Mottu API - Gerenciamento de Pátios",
    "Version": "v1",
    "Description": "API RESTful para gerenciar Filiais, Pátios e o histórico de localização de Motos através de Sensores."
  }
}
```

---

## 📝 Regras de Negócio

### Locações

- Um entregador só pode ter **uma locação ativa** por vez
- Uma moto só pode estar **alugada para um entregador** por vez
- Apenas entregadores com CNH tipo **A ou AB** podem alugar motos
- Planos disponíveis: **7, 15, 30, 45 ou 50 dias**
- **Multa por devolução antecipada**: 20% (7 dias), 40% (15 dias), 60% (30 dias), 80% (45 dias), 90% (50 dias)
- **Multa por atraso**: R$ 50,00 por dia de atraso

### Motos

- A placa deve ser **única** no sistema
- Não é possível excluir uma moto com histórico de localização

### Entregadores

- CNPJ e CNH devem ser **únicos** no sistema

---

## 🐛 Troubleshooting

### Problema: API retorna vazio

**Solução**: Verifique se o DataSeeder foi executado. Os logs no console mostrarão:
```
DataSeeder: Entregadores criados.
DataSeeder: Locacoes criadas.
DataSeeder: Sensores criados.
```

### Problema: Erro de conexão com Oracle

**Solução**: Verifique a connection string no `appsettings.json` e se o servidor Oracle está acessível.

### Problema: Erro ao aplicar migrations

**Solução**: Certifique-se de que o banco de dados Oracle está acessível e que as permissões do usuário estão corretas.

---

## 📚 Documentação Adicional

- **Swagger UI**: `http://localhost:5287/swagger`
- **Health Check**: `http://localhost:5287/health`

---

## 📄 Licença

Este projeto foi desenvolvido para fins acadêmicos.

---

## 👥 Autoria / Equipe

- **Felipe Gomes Costa Orikasa** - RM: 557435
- **Marcelo Siqueira Bonfim** - RM: 558254
- **Antônio Cauê Araújo da Silva** - RM: 558891

---

## 🔄 Changelog

### Sprint 4
- ✅ Implementação completa do sistema de locações
- ✅ Cadastro e gerenciamento de entregadores
- ✅ DataSeeder automático para popular banco de dados
- ✅ Logs detalhados para diagnóstico
- ✅ Suporte completo ao Oracle Database
- ✅ Migrations idempotentes e compatíveis com Oracle
- ✅ Tratamento de dados antigos incompatíveis

---

**🔑 Lembre-se**: A API Key para acesso ao Swagger é `DEV_KEY_CHANGE_ME`
