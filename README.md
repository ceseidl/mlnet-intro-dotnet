English | [Português](README.pt-BR.md)

[![CI](https://github.com/ceseidl/mlnet-intro-dotnet/actions/workflows/ci.yml/badge.svg)](https://github.com/ceseidl/mlnet-intro-dotnet/actions/workflows/ci.yml) [![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

# mlnet-intro-dotnet

> **Quick start**

```bash
dotnet build
dotnet run --project src/SentimentoMl --no-build
```

Needs only the .NET 10 SDK.

A first-steps example of machine learning in .NET 10 with [ML.NET](https://learn.microsoft.com/dotnet/machine-learning/) (`Microsoft.ML` 5.0.0): binary sentiment classification of product reviews, plus an integration with an external AI service behind the `Microsoft.Extensions.AI` abstraction (`IChatClient`).

## What it is

- **Training pipeline**: loads `avaliacoes.tsv`, splits train/test (80/20), featurizes text (`FeaturizeText`) and trains `SdcaLogisticRegression`.
- **Evaluation**: Accuracy, AUC and F1 on the test set.
- **Model file**: saves the model to a `.zip` and loads it back (what you would ship to production).
- **Prediction**: `AnalisadorLocal` wraps a `PredictionEngine` behind the `IAnalisadorSentimento` interface.
- **External service**: `AnalisadorExterno` calls any `IChatClient`. The demo uses `ChatClientSimulado`, so it runs without keys or network.
- **Hybrid strategy**: `AnalisadorHibrido` uses the local model and only escalates to the external service when confidence is below a threshold (0.75).

## Dataset

`Data/avaliacoes.tsv` has 360 short reviews in Portuguese (180 positive, 180 negative): 60 written by hand and 300 generated from templates. It is **synthetic and tiny**, built only to make the example self-contained. The high metrics do not say anything about real-world quality.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## How to run

```bash
dotnet build
dotnet run --project src/SentimentoMl --no-build
```

Expected output (values vary slightly between runs):

```
Accuracy: 97%
AUC:      100%
F1:       98%
Produto excelente, entrega rápida -> positivo (93%, ML.NET local)
O atendimento é péssimo -> negativo (89%, ML.NET local)
Chegou ontem -> negativo (100%, serviço externo)
```

## Structure

```
src/SentimentoMl/
  Avaliacao.cs               input and prediction types
  Treinador.cs               train, evaluate, save, load
  IAnalisadorSentimento.cs   contract used by the app
  AnalisadorLocal.cs         ML.NET implementation
  AnalisadorExterno.cs       IChatClient implementation
  AnalisadorHibrido.cs       local first, external on doubt
  ChatClientSimulado.cs      offline stand-in for a real provider
  Program.cs                 end-to-end demo
  Data/avaliacoes.tsv        sample dataset
```

## Notes

- To use a real provider, register its `IChatClient` implementation instead of `ChatClientSimulado`. Keep API keys in user secrets or environment variables, never in code.
- `PredictionEngine` is not thread-safe. In a web API use `PredictionEnginePool` (package `Microsoft.Extensions.ML`).
- Comments and identifiers in the code are in Portuguese, matching the article.

## License

[MIT](LICENSE)
