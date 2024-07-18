using Pdf.Api.Extensions;
using Pdf.Processor.Generator;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureCorsServices();
builder.ConfigureApiServices();

builder.Services.AddScoped<IGenerator, ITextGenerator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseCors();

app.MapControllers();

app.Run();