using Microsoft.ML;
using SentimentoMl;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var ml = new MLContext(seed: 1);
var caminhoDados = Path.Combine(
    AppContext.BaseDirectory, "Data", "avaliacoes.tsv");
var caminhoModelo = Path.Combine(
    AppContext.BaseDirectory, "modelo-sentimento.zip");

// 1. Carregar e separar treino/teste (20% para teste).
var dados = ml.Data.LoadFromTextFile<Avaliacao>(caminhoDados);
var divisao = ml.Data.TrainTestSplit(dados, testFraction: 0.2);

// 2. Treinar e avaliar.
var modelo = Treinador.Treinar(ml, divisao.TrainSet);
var m = Treinador.Avaliar(ml, modelo, divisao.TestSet);
Console.WriteLine($"Accuracy: {m.Accuracy:P0}");
Console.WriteLine($"AUC:      {m.AreaUnderRocCurve:P0}");
Console.WriteLine($"F1:       {m.F1Score:P0}");

// 3. Salvar e carregar o modelo (o que vai para produção).
Treinador.Salvar(ml, modelo, divisao.TrainSet, caminhoModelo);
var carregado = Treinador.Carregar(ml, caminhoModelo);

// 4. Prever com o modelo local e com a estratégia híbrida.
var local = new AnalisadorLocal(ml, carregado);
var externo = new AnalisadorExterno(
    new ChatClientSimulado("NEGATIVO"));
var hibrido = new AnalisadorHibrido(local, externo);

string[] textos =
[
    "Produto excelente, entrega rápida",
    "O atendimento é péssimo",
    "Chegou ontem",
];
foreach (var texto in textos)
{
    var r = await hibrido.AnalisarAsync(texto);
    var rotulo = r.Positivo ? "positivo" : "negativo";
    Console.WriteLine(
        $"{texto} -> {rotulo} ({r.Confianca:P0}, {r.Origem})");
}
