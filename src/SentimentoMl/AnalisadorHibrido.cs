namespace SentimentoMl;

// Modelo local primeiro; serviço externo só quando há dúvida.
public sealed class AnalisadorHibrido(
    IAnalisadorSentimento local,
    IAnalisadorSentimento externo,
    float confiancaMinima = 0.75f) : IAnalisadorSentimento
{
    public async Task<Sentimento> AnalisarAsync(
        string texto, CancellationToken ct = default)
    {
        var resultado = await local.AnalisarAsync(texto, ct);
        return resultado.Confianca >= confiancaMinima
            ? resultado
            : await externo.AnalisarAsync(texto, ct);
    }
}
