using Microsoft.Extensions.ML;
using Microsoft.ML.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPredictionEnginePool<Entrada, Saida>()
    .FromFile("sentimento", "modelo-sentimento.zip",
        watchForChanges: true);

var app = builder.Build();

app.MapPost("/sentimento",
    (PredictionEnginePool<Entrada, Saida> pool, Entrada e) =>
        pool.Predict("sentimento", e));

app.Run();

public class Entrada { public string Texto { get; set; } = ""; }

public class Saida
{
    [ColumnName("PredictedLabel")]
    public bool Positiva { get; set; }
    public float Probability { get; set; }
}
