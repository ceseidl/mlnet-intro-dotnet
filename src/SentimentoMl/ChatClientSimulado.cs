using Microsoft.Extensions.AI;

namespace SentimentoMl;

// Substituto de IChatClient: roda sem chave nem rede.
// Em produção, troque por um cliente real do fornecedor.
public sealed class ChatClientSimulado(string resposta)
    : IChatClient
{
    public Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(new ChatResponse(
            new ChatMessage(ChatRole.Assistant, resposta)));

    public IAsyncEnumerable<ChatResponseUpdate>
        GetStreamingResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default) =>
        throw new NotSupportedException();

    public object? GetService(
        Type serviceType, object? serviceKey = null) => null;

    public void Dispose() { }
}
