namespace Common.Events.Messages
{
    public class Message
    {
        public Message(object header, object content, MessageLevel level = MessageLevel.Information)
        {
            Level = level;
            Header = header;
            Content = content;
        }

        public MessageLevel Level { get; }
        public object Header { get; }
        public object Content { get; }
    }
}
