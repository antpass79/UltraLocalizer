namespace Globe.Client.Platofrm.Events
{
    public enum MessageType
    {
        None = 0,
        Information = 1,
        Warning = 2,
        Error = 3
    }

    public class StatusBarMessage
    {
        public MessageType MessageType { get; set; }
        public string Text { get; set; }
    }
}