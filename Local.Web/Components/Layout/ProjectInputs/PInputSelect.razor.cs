using Microsoft.AspNetCore.Components;

namespace Local.Web.Components.Layout.ProjectInputs
{
    public partial class PInputSelect<TValue>
    {
        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public string? Comment { get; set; }

        [Parameter]
        public string? OuterClass { get; set; }

        [Parameter]
        public string? LabelClass { get; set; }

        [Parameter]
        public string? CommentClass { get; set; }

        private readonly string? additionalClass = " form-control select";

        protected override void OnParametersSet()
        {
            if (!string.IsNullOrWhiteSpace(additionalClass))
            {
                var additionalAttributes = new Dictionary<string, object>(AdditionalAttributes);

                if (additionalAttributes.ContainsKey("class"))
                {
                    additionalAttributes["class"] += additionalClass;
                }
                else
                {
                    additionalAttributes.Add("class", additionalClass);
                }

                AdditionalAttributes = additionalAttributes;
            }

            base.OnParametersSet();
        }
    }
}
