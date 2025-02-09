using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Local.Web.Components.Layout.ProjectInputs
{
    public partial class PInputText : InputText
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

        protected override void OnParametersSet()
        {
            var additionalAttributes = new Dictionary<string, object>(AdditionalAttributes);

            if (additionalAttributes.ContainsKey("class"))
            {
                additionalAttributes["class"] += " form-control";
            }
            else
            {
                additionalAttributes.Add("class", "form-control");
            }

            AdditionalAttributes = additionalAttributes;

            base.OnParametersSet();
        }
    }
}
