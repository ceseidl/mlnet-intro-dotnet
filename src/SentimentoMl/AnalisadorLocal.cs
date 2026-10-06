using Microsoft.ML;

namespace SentimentoMl;

// PredictionEngine não é thread-safe: serializamos o acesso.
public sealed class AnalisadorLocal : IAnalisadorSentimento
{
    private readonly PredictionEngine<Avaliacao, Previsao> _motor;
    private readonly object _trava = new();

    public AnalisadorLocal(MLContext ml, ITransformer modelo) =>
        _motor = ml.Model.CreatePredictionEngine
            <Avaliacao, Previsao>(modelo);

    public Task<Sentimento> AnalisarAsync(
        string texto, CancellationToken ct = default)
    {
        Previsao p;
        lock (_trava)
            p = _motor.Predict(new Avaliacao { Texto = texto });

        var conf = p.Positiva ? p.Probability : 1 - p.Probability;
        return Task.FromResult(
            new Sentimento(p.Positiva, conf, "ML.NET local"));
    }
}
