namespace SentimentoMl;

public record Sentimento(
    bool Positivo, float Confianca, string Origem);

public interface IAnalisadorSentimento
{
    Task<Sentimento> AnalisarAsync(
        string texto, CancellationToken ct = default);
}
