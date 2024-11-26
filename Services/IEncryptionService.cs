using DrCryptFast.Domain;

namespace DrCryptFast.Services;

public interface IEncryptionService
{
    ValueTask<Message> Encrypt(Message message);

    ValueTask<Message> Decrypt(Message message);
}