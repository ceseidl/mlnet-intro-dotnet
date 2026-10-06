[English](README.md) | Português

[![CI](https://github.com/ceseidl/mlnet-intro-dotnet/actions/workflows/ci.yml/badge.svg)](https://github.com/ceseidl/mlnet-intro-dotnet/actions/workflows/ci.yml) [![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

# mlnet-intro-dotnet

> **Início rápido**

```bash
dotnet build
dotnet run --project src/SentimentoMl --no-build
```

Precisa apenas do SDK do .NET 10.

Exemplo de primeiros passos em machine learning com .NET 10 e [ML.NET](https://learn.microsoft.com/dotnet/machine-learning/) (`Microsoft.ML` 5.0.0): classificação binária de sentimento em avaliações de produtos, mais a integração com um serviço de IA externo atrás da abstração `Microsoft.Extensions.AI` (`IChatClient`).

## O que é

- **Pipeline de treino**: carrega `avaliacoes.tsv`, separa treino/teste (80/20), transforma o texto em features (`FeaturizeText`) e treina um `SdcaLogisticRegression`.
- **Avaliação**: Accuracy, AUC e F1 no conjunto de teste.
- **Arquivo do modelo**: salva o modelo em `.zip` e o carrega de volta (o que iria para produção).
- **Predição**: `AnalisadorLocal` encapsula um `PredictionEngine` atrás da interface `IAnalisadorSentimento`.
- **Serviço externo**: `AnalisadorExterno` chama qualquer `IChatClient`. A demo usa `ChatClientSimulado`, então roda sem chaves nem rede.
- **Estratégia híbrida**: `AnalisadorHibrido` usa o modelo local e só chama o serviço externo quando a confiança fica abaixo de um limite (0,75).

## Dataset

`Data/avaliacoes.tsv` tem 360 avaliações curtas em português (180 positivas, 180 negativas): 60 escritas à mão e 300 geradas a partir de modelos de frase. É **sintético e pequeno**, feito só para o exemplo ser autocontido. As métricas altas não dizem nada sobre qualidade em dados reais.

## Requisitos

- [SDK do .NET 10](https://dotnet.microsoft.com/download)

## Como rodar

```bash
dotnet build
dotnet run --project src/SentimentoMl --no-build
```

Saída esperada (os valores variam um pouco entre execuções):

```
Accuracy: 97%
AUC:      100%
F1:       98%
Produto excelente, entrega rápida -> positivo (93%, ML.NET local)
O atendimento é péssimo -> negativo (89%, ML.NET local)
Chegou ontem -> negativo (100%, serviço externo)
```

## Estrutura

```
src/SentimentoMl/
  Avaliacao.cs               tipos de entrada e de predição
  Treinador.cs               treinar, avaliar, salvar, carregar
  IAnalisadorSentimento.cs   contrato usado pela aplicação
  AnalisadorLocal.cs         implementação com ML.NET
  AnalisadorExterno.cs       implementação com IChatClient
  AnalisadorHibrido.cs       local primeiro, externo na dúvida
  ChatClientSimulado.cs      substituto offline de um fornecedor
  Program.cs                 demo de ponta a ponta
  Data/avaliacoes.tsv        dataset de exemplo
```

## Notas

- Para usar um fornecedor real, registre a implementação de `IChatClient` dele no lugar de `ChatClientSimulado`. Guarde chaves em user secrets ou variáveis de ambiente, nunca no código.
- `PredictionEngine` não é thread-safe. Em uma Web API use `PredictionEnginePool` (pacote `Microsoft.Extensions.ML`).

## Licença

[MIT](LICENSE)
