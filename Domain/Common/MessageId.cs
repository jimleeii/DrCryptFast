using ValueOf;

namespace DrCryptFast.Domain.Common;

public class MessageId : ValueOf<Guid, MessageId>
{
    protected override void Validate()
    {
        if (Value == Guid.Empty)
        {
            throw new ArgumentException("Message Id cannot be empty", nameof(MessageId));
        }
    }
}