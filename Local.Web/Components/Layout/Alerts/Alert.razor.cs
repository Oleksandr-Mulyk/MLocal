namespace Local.Web.Components.Layout.Alerts
{
    public partial class Alert(IMessageManager messageManager)
    {
        private static string GetMessageClass(MessageType messageType) =>
            messageType switch
            {
                MessageType.Error => "alert-danger",
                MessageType.Warning => "alert-warning",
                MessageType.Info => "alert-info",
                MessageType.Success => "alert-success",
                _ => "alert-info"
            };

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Delay(5000);
                messageManager.ClearMessages();
                StateHasChanged();
            }
        }
    }
}
