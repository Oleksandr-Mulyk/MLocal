using Microsoft.AspNetCore.Components;

namespace Local.Web.Components.Layout
{
    public partial class Pagination
    {
        [Parameter]
        public int TotalItems { get; set; }

        [Parameter]
        public int ItemsPerPage { get; set; } = 10;

        [Parameter]
        public int CurrentPage { get; set; } = 1;

        [Parameter]
        public EventCallback<int> OnPageChanged { get; set; }

        private int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);

        private bool IsFirstPage => CurrentPage == 1;

        private bool IsLastPage => CurrentPage == TotalPages;

        private async Task GoToPage(int pageNumber)
        {
            if (pageNumber != CurrentPage)
            {
                CurrentPage = pageNumber;
                await OnPageChanged.InvokeAsync(pageNumber);
            }
        }

        private async Task PreviousPage()
        {
            if (!IsFirstPage)
            {
                CurrentPage--;
                await OnPageChanged.InvokeAsync(CurrentPage);
            }
        }

        private async Task NextPage()
        {
            if (!IsLastPage)
            {
                CurrentPage++;
                await OnPageChanged.InvokeAsync(CurrentPage);
            }
        }
    }
}
