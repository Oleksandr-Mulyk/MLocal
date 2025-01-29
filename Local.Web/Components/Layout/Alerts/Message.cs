namespace Local.Web.Components.Layout.Alerts
{
    public struct Message(string content, MessageType type)
    {
        public string Content { get; set; } = content;

        public MessageType Type { get; set; } = type;
    }
}
