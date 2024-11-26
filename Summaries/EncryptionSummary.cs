using DrCryptFast.Contracts.Responses;
using DrCryptFast.Endpoints;
using FastEndpoints;

namespace DrCryptFast.Summaries;

public class EncryptionSummary : Summary<EncryptionEndpoint>
{
    public EncryptionSummary()
    {
        Summary = "Encrypts or decrypts a message";
        Description = "Encrypts or decrypts a message";
        Response<EncryptionResponse>(201, "Message was successfully encrypted or decrypted");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}