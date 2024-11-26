using DrCryptFast.Contracts.Requests;
using DrCryptFast.Contracts.Responses;
using DrCryptFast.Extensions;
using DrCryptFast.Mapping;
using DrCryptFast.Services;
using FastEndpoints;

namespace DrCryptFast.Endpoints;

public class EncryptionEndpoint(IEncryptionService encryptionService) : Endpoint<EncryptionRequest, EncryptionResponse>
{
	private readonly IEncryptionService _encryptionService = encryptionService;

	public override void Configure()
	{
		Put("api/encryption/{action}");
		AllowAnonymous();
	}

	public override async Task HandleAsync(EncryptionRequest request, CancellationToken token)
	{
		var action = request.Action;
		var message = request.ToMessage();

		if (action.EqualsIgnoreCase("decrypt"))
		{
			var decrypted = await _encryptionService.Decrypt(message);
			var cryptionResponse = decrypted.ToEncryptionResponse();
			await SendOkAsync(cryptionResponse, token);
		}
		else if (action.EqualsIgnoreCase("encrypt"))
		{
			var encrypted = await _encryptionService.Encrypt(message);
			var cryptionResponse = encrypted.ToEncryptionResponse();
			await SendOkAsync(cryptionResponse, token);
		}
	}
}