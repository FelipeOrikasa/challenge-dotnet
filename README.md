# 🏍️ Mottu API - Gerenciamento de Pátios e Frotas

## 📖 Descrição do Projeto

Mottu API é um sistema de back-end desenvolvido em **ASP.NET Core 8** para o gerenciamento e rastreamento de frotas de motocicletas em um sistema de pátios.
A API permite o cadastro e a gestão de **filiais, pátios, sensores e motos**, além de registrar o histórico de localização de cada veículo ao passar por um sensor.

O projeto foi construído utilizando uma arquitetura em camadas bem definida, com foco em boas práticas de desenvolvimento como **Separação de Responsabilidades (SoC)**,
**Injeção de Dependência (DI)** e o uso do padrão de **Repositório e Serviços**, visando garantir um código limpo, testável e de fácil manutenção.

---

## 👨‍💻 Integrantes

- Felipe Gomes Costa Orikasa - Rm: 557435
- Marcelo Siqueira Bonfim - Rm: 558254
- Antônio Cauê Araújo da Silva - Rm: 558891

---

## 🏗️ Arquitetura do Projeto

Foi implementada uma **Arquitetura em Camadas (Layered Architecture)**, que organiza o código em áreas de responsabilidade distintas,
desacoplando a lógica de negócio das demais partes da aplicação.

### Estrutura de Pastas

- `/Models`: Contém as entidades que espelham a estrutura do banco de dados (Filial, Patio, Moto, etc.).
- `/Repositories`: Implementa a lógica de acesso a dados (Entity Framework Core).
- `/Services`: Contém toda a lógica de negócio da aplicação.
- `/DTOs`: Objetos de transferência de dados usados pela API.
- `/Controllers`: Recebe as requisições HTTP, chama os serviços apropriados e retorna respostas.

### Benefícios

- **Manutenibilidade**: Cada componente possui responsabilidade única.
- **Testabilidade**: Serviços podem ser testados de forma isolada.
- **Desacoplamento**: Negócio independente do banco de dados.

---

## 🛠️ Tecnologias Utilizadas

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core 8**
- **Oracle Database**
- **AutoMapper**
- **Swagger/OpenAPI**

---

## 🚀 Como Executar o Projeto

### Pré-requisitos

- .NET 8 SDK
- Git
- Banco de Dados Oracle

### Passo a Passo

1. **Clonar o repositório**

```bash
git clone https://github.com/FelipeOrikasa/challenge-dotnet
cd Mottu.Api
```

2. **Configurar a Conexão com o Banco de Dados**
   No arquivo `appsettings.json`, configure:

```json
"ConnectionStrings": {
  "OracleDb": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=SEU_DATASOURCE"
}
```

3. **Aplicar as Migrations**

```bash
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

---

## 📌 Exemplos de Uso dos Endpoints

### Criar uma nova Filial

**POST** `/api/filiais`

```json
{
  "nomeFilial": "Mottu - Sede Administrativa",
  "cidade": "São Paulo"
}
```

### Criar um novo Pátio

**POST** `/api/patios`

```json
{
  "nomePatio": "Pátio Principal - Vistorias",
  "filialId": 1
}
```

### Cadastrar uma nova Moto

**POST** `/api/motos`

```json
{
  "placa": "XYZ9A87",
  "modelo": "Honda ADV",
  "ano": 2025,
  "patioId": 1
}
```

### Registrar um Evento de Localização

**POST** `/api/localizacoes`

```json
{
  "motoId": 1,
  "sensorId": 1
}
```

### Consultar o Histórico de uma Moto (com paginação)

**GET** `/api/motos/1/localizacoes?pageNumber=1&pageSize=5`

---

## 🧪 Testes

A arquitetura permite **testes unitários isolados** nos serviços.

Executar os testes:

```bash
dotnet test
```
