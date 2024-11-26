using DrCryptFast.Contracts.Responses;
using DrCryptFast.Services;
using DrCryptFast.Settings;
using DrCryptFast.Validation;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.Configure<AppSettings>(config.GetSection(nameof(AppSettings)));

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

builder.Services.AddSingleton<IEncryptionService, EncryptionService>();

var app = builder.Build();

app.UseMiddleware<ValidationExceptionMiddleware>();
app.UseFastEndpoints(x =>
{
	x.Errors.ResponseBuilder = (failures, ctx, _) =>
	{
		return new ValidationFailureResponse
		{
			Errors = failures.Select(y => y.ErrorMessage).ToList()
		};
	};
});

app.UseSwaggerGen();

await app.RunAsync();