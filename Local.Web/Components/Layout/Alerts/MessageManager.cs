namespace Local.Web.Components.Layout.Alerts
{
    public class MessageManager : IMessageManager
    {
        public IList<Message> Messages { get; } = [];

        public void AddMessage(Message message)
        {
            Messages.Add(message);
        }

        public void RemoveMessage(Message message)
        {
            Messages.Remove(message);
        }

        public void ClearMessages() => Messages.Clear();
    }
}
