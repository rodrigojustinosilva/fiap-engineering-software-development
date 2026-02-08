# fiap-engineering-software-development
**MBA FIAP - Avaliação Final da disciplina: Engineering Software Development**

# ConvertFile API

## 📋 Descrição

Microsserviço desenvolvido em .NET Core para conversão de arquivos entre diferentes formatos.


## 🎯 Objetivo

Desenvolver um microsserviço que converte arquivos entre os seguintes formatos:
- **Fixed Position** (Posição Fixa)
- **Delimited** (CSV, TSV, etc)
- **JSON**

## 🏗️ Arquitetura e Design Patterns

### Princípios SOLID Aplicados

#### 1. **Single Responsibility Principle (SRP)**
- Cada classe tem uma única responsabilidade:
  - `FixedPositionReader`: apenas lê arquivos de posição fixa
  - `DelimitedReader`: apenas lê arquivos delimitados
  - `JsonReader`: apenas lê arquivos JSON
  - Writers seguem o mesmo padrão

#### 2. **Open/Closed Principle (OCP)**
- Sistema aberto para extensão, fechado para modificação
- Novos formatos podem ser adicionados sem modificar código existente
- Basta criar nova classe que implemente `IFileReader` ou `IFileWriter`

#### 3. **Liskov Substitution Principle (LSP)**
- Todas as implementações de `IFileReader` são intercambiáveis
- Todas as implementações de `IFileWriter` são intercambiáveis
- O `FileConverterService` funciona com qualquer implementação

#### 4. **Interface Segregation Principle (ISP)**
- Interfaces pequenas e específicas:
  - `IFileReader`: apenas para leitura
  - `IFileWriter`: apenas para escrita
  - `IFileConverterService`: apenas para conversão

#### 5. **Dependency Inversion Principle (DIP)**
- Classes dependem de abstrações (interfaces), não de implementações concretas
- `FileConverterService` depende de `IFileReader` e `IFileWriter`
- Injeção de dependência configurada no `Program.cs`

### Padrões GRASP Aplicados

- **Controller**: `FileConverterController` coordena as requisições
- **Creator**: `FileConverterService` cria e coordena leitores e escritores
- **Information Expert**: Cada reader/writer conhece seu próprio formato
- **Low Coupling**: Dependências através de interfaces
- **High Cohesion**: Cada classe tem responsabilidade bem definida

## 🚀 Como Usar

### Endpoints Disponíveis

#### POST /api/fileconverter/convert
Converte arquivo de um formato para outro.

**Request Body:**
```json
{
  "fileName": "example.txt",
  "fileContent": "conteúdo do arquivo",
  "sourceFormat": "FixedPosition",
  "targetFormat": "Json",
  "configuration": {
    "positions": "[{\"Name\":\"Id\",\"Start\":0,\"Length\":5}]",
    "indent": "true"
  }
}
```

**Response:**
```json
{
  "success": true,
  "convertedContent": "conteúdo convertido",
  "message": "Arquivo convertido com sucesso"
}
```

#### GET /api/fileconverter/formats
Retorna os formatos suportados.

## 🧪 Testes

Suite de testes implementada com xUnit, FluentAssertions e Moq.

### Cenários de Teste

1. **FixedPosition → JSON**: Converte arquivo de posição fixa para JSON
2. **Delimited → JSON**: Converte CSV para JSON
3. **JSON → Delimited**: Converte JSON para CSV

### Executar Testes
```bash
dotnet test
```

## 📦 Estrutura do Projeto
```
ConvertFile/
├── src/
│   └── ConvertFile.Api/
│       ├── Controllers/
│       ├── Interfaces/
│       ├── Models/
│       ├── Services/
│       │   ├── Readers/
│       │   └── Converters/
│       └── Program.cs
├── tests/
│   └── ConvertFile.Tests/
│       ├── Readers/
│       └── Integration/
└── docs/
    └── README.md
```

## 📋 Comandos para teste e execução

1. Buildar e testar
dotnet build
dotnet test

2. Executar API
cd src/ConvertFile.Api
dotnet run

## 👥 Equipe

- Rodrigo Justino da Silva
