namespace Local.Web.Components.Layout.Alerts
{
    public partial class Alert(IMessageManager messageManager)
    {
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
