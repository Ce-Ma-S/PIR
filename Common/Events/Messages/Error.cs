using Common.Validation;
using System;

namespace Common.Events.Messages
{
    public class Error :
        Message
    {
        public Error(object header, Exception value, object content = null) :
            base(header, content, MessageLevel.Error)
        {
            ArgumentValidation.NonNull(value, nameof(value));
            if (content == null)
                content = value.GetBaseException().Message;
            Value = value;
        }

        public Exception Value { get; }
    }
}
