using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;

namespace Local.Web.Components.Layout
{
    public partial class CultureSelector(NavigationManager navigationManager, IJSRuntime js)
    {
        private CultureInfo[] supportedCultures =
            [
            new CultureInfo("en-US"),
            new CultureInfo("uk-UA"),
            new CultureInfo("pl-PL")
            ];

        private CultureInfo? selectedCulture;

        protected override void OnInitialized()
        {
            selectedCulture = CultureInfo.CurrentCulture;
        }

        private async Task ApplySelectedCultureAsync()
        {
            if (CultureInfo.CurrentCulture != selectedCulture)
            {
                var uri = new Uri(navigationManager.Uri)
                    .GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
                var cultureEscaped = Uri.EscapeDataString(selectedCulture.Name);
                var uriEscaped = Uri.EscapeDataString(uri);

                navigationManager.NavigateTo(
                    $"Culture/Set?culture={cultureEscaped}&redirectUri={uriEscaped}",
                    forceLoad: true);
            }
        }
    }
}
