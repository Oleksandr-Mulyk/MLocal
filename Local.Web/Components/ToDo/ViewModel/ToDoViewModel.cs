using Local.Web.Data.ToDo;
using System.ComponentModel.DataAnnotations;

namespace Local.Web.Components.ToDo.ViewModel
{
    public class ToDoViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public ToDoItemStatus Status { get; set; } = ToDoItemStatus.Plane;

        public string Description { get; set; } = string.Empty;

        public DateTime? DeathLine { get; set; }

        public string[] AssignedTo { get; set; } = [];

        public string[] VisibleFor { get; set; } = [];

        public string? StatusChangedBy { get; set; }

        public DateTime? StatusChanged { get; set; }
    }
}
