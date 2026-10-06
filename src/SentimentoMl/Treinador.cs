using Microsoft.ML;
using Microsoft.ML.Data;

namespace SentimentoMl;

public static class Treinador
{
    public static ITransformer Treinar(
        MLContext ml, IDataView dados)
    {
        var pipeline = ml.Transforms.Text
            .FeaturizeText("Features", nameof(Avaliacao.Texto))
            .Append(ml.BinaryClassification.Trainers
                .SdcaLogisticRegression("Label", "Features"));

        return pipeline.Fit(dados);
    }

    public static CalibratedBinaryClassificationMetrics Avaliar(
        MLContext ml, ITransformer modelo, IDataView teste)
    {
        var previsoes = modelo.Transform(teste);
        return ml.BinaryClassification
            .Evaluate(previsoes, "Label");
    }

    public static void Salvar(MLContext ml, ITransformer modelo,
        IDataView dados, string caminho) =>
        ml.Model.Save(modelo, dados.Schema, caminho);

    public static ITransformer Carregar(
        MLContext ml, string caminho) =>
        ml.Model.Load(caminho, out _);
}
