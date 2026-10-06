using Microsoft.Extensions.AI;

namespace SentimentoMl;

// Depende só de IChatClient: o fornecedor é detalhe.
public sealed class AnalisadorExterno(IChatClient chat)
    : IAnalisadorSentimento
{
    public async Task<Sentimento> AnalisarAsync(
        string texto, CancellationToken ct = default)
    {
        var prompt =
            "Classifique o sentimento do texto. " +
            "Responda apenas POSITIVO ou NEGATIVO.\n\n" + texto;

        var resposta = await chat.GetResponseAsync(
            prompt, cancellationToken: ct);

        var positivo = resposta.Text.Trim().StartsWith(
            "POSITIVO", StringComparison.OrdinalIgnoreCase);
        return new Sentimento(positivo, 1f, "serviço externo");
    }
}
