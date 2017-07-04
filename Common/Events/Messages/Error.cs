using Common.Validation;
using System;

namespace Common.Events.Messages
{
    public class Error :
        Message
    {
        public Error(object header, object content, Exception value) :
            base(header, GetContent(content, value), MessageLevel.Error)
        {
            Value = value;
        }

        public Exception Value { get; }

        private static object GetContent(object content, Exception value)
        {
            ArgumentValidation.NonNull(value, nameof(value));
            var message = value.GetBaseException().Message;
            if (content is string)
            {
                content = string.Concat(
                    (string)content,
                    Environment.NewLine,
                    Environment.NewLine,
                    message
                    );
            }
            else if (content == null)
            {
                content = message;
            }
            return content;
        }
    }
}
