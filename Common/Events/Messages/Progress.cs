using Common.Validation;

namespace Common.Events.Messages
{
    public class Progress :
        Message
    {
        public Progress(object header, object content, bool isBusy = true, MessageLevel level = MessageLevel.Information) :
            base(header, content, level)
        {
            IsBusy = isBusy;
        }
        public Progress(object header, object content, float value, MessageLevel level = MessageLevel.Information) :
            base(header, content, level)
        {
            ArgumentValidation.In(value, 0, 1);
            Value = value;
            IsBusy = value < 1;
        }

        public bool IsBusy { get; }
        public float? Value { get; }
        public float? Percentage => Value * 100;
    }
}
