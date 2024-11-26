using DrCryptFast.Domain;
using DrCryptFast.Domain.Common;
using DrCryptFast.Settings;
using Microsoft.Extensions.Options;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace DrCryptFast.Services;

public class EncryptionService : IEncryptionService
{
	private const string INVALID_KEY = "Invalid encryption key.";
	private const string NO_KEY = "No encryption key.";

	private readonly ILogger _logger;
	private readonly Lazy<(byte[] Iv, byte[] Key)> _ivKey;

	public EncryptionService(IOptions<AppSettings> appSettings, ILogger logger = default!)
	{
		var encrptKey = File.ReadAllBytes(appSettings.Value.V6API_Key!);
		_ivKey = new Lazy<(byte[], byte[])>(() => GetSymmetricKey(encrptKey), true);
		_logger = logger;
	}

	internal static (byte[] Iv, byte[] Key) GetSymmetricKey(byte[] keyData)
	{
		using var stream = new MemoryStream(keyData);
		using var reader = new BinaryReader(stream, Encoding.Default, false);
		int ivLength = reader.ReadInt32();
		int keyLength = reader.ReadInt32();

		var iv = new byte[ivLength];
		var key = new byte[keyLength];

		if ((iv.Length == 0) || (key.Length == 0))
		{
			throw new ArgumentException(INVALID_KEY, nameof(keyData));
		}

		reader.Read(iv, 0, iv.Length);
		reader.Read(key, 0, key.Length);

		return new(iv, key);
	}

	internal byte[] Decrypt(byte[] data)
	{
		var ivKey = _ivKey.Value;

		if ((ivKey.Iv == null) || (ivKey.Key == null) || (ivKey.Iv.Length == 0) || (ivKey.Key.Length == 0))
		{
			throw new SecurityException(NO_KEY);
		}

		if ((data == null) || (data.Length == 0))
		{
			return [];
		}

		using Aes aes = Aes.Create();
		using ICryptoTransform transform = aes.CreateDecryptor(ivKey.Key, ivKey.Iv);
		using var stream = new MemoryStream();
		using var writer = new CryptoStream(stream, transform, CryptoStreamMode.Write);
		writer.Write(data, 0, data.Length);

		if (!writer.HasFlushedFinalBlock)
		{
			writer.FlushFinalBlock();
		}

		return stream.ToArray();
	}

	internal byte[] Encrypt(byte[] data)
	{
		var ivKey = _ivKey.Value;

		if ((ivKey.Iv == null) || (ivKey.Key == null) || (ivKey.Iv.Length == 0) || (ivKey.Key.Length == 0))
		{
			throw new SecurityException(NO_KEY);
		}

		if ((data == null) || (data.Length == 0))
		{
			return [];
		}

		using Aes aes = Aes.Create();
		using ICryptoTransform transform = aes.CreateEncryptor(ivKey.Key, ivKey.Iv);
		using var stream = new MemoryStream();
		using var writer = new CryptoStream(stream, transform, CryptoStreamMode.Write);
		writer.Write(data, 0, data.Length);
		writer.FlushFinalBlock();

		return stream.ToArray();
	}

	internal string DecryptString(byte[] value, Encoding? textEncoding = null)
	{
		if ((value == null) || (value.Length == 0))
		{
			return string.Empty;
		}

		if (textEncoding == null)
		{
			textEncoding = Encoding.UTF8;
		}

		return textEncoding.GetString(Decrypt(value));
	}

	internal byte[] EncryptString(string? value, Encoding? textEncoding = null)
	{
		if (string.IsNullOrEmpty(value))
		{
			return Array.Empty<byte>();
		}

		if (textEncoding == null)
		{
			textEncoding = Encoding.UTF8;
		}

		return Encrypt(textEncoding.GetBytes(value));
	}

	internal string DecryptBase64String(string? base64String, Encoding? textEncoding = null)
	{
		return string.IsNullOrEmpty(base64String) ? string.Empty : DecryptString(Convert.FromBase64String(base64String));
	}

	internal string EncryptBase64String(string? value, Encoding? textEncoding = null)
	{
		return string.IsNullOrEmpty(value) ? string.Empty : Convert.ToBase64String(EncryptString(value, textEncoding));
	}

	public ValueTask<Message> Decrypt(Message message)
	{
		var content = DecryptBase64String(message.EncryptedContent.Value.ToString());
		message.Content = Content.From(content);
		return ValueTask.FromResult(message);
	}

	public ValueTask<Message> Encrypt(Message message)
	{
		var encryptedContent = EncryptBase64String(message.Content.Value);
		message.EncryptedContent = EncrytedContent.From(encryptedContent);
		return ValueTask.FromResult(message);
	}
}