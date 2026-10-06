using Microsoft.ML.Data;

namespace SentimentoMl;

// Linha do arquivo de treino: texto e rótulo (1 = positivo).
public class Avaliacao
{
    [LoadColumn(0)]
    public string Texto { get; set; } = "";

    [LoadColumn(1), ColumnName("Label")]
    public bool Positiva { get; set; }
}

// Colunas que o modelo acrescenta ao predizer.
public class Previsao
{
    [ColumnName("PredictedLabel")]
    public bool Positiva { get; set; }

    public float Probability { get; set; }

    public float Score { get; set; }
}
