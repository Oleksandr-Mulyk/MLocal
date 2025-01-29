namespace Local.Web.Components.Layout.Alerts
{
    public interface IMessageManager
    {
        IList<Message> Messages { get; }

        void AddMessage(Message message);

        void RemoveMessage(Message message);

        void ClearMessages();
    }
}
