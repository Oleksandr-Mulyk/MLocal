namespace Local.Web.Data.ToDo
{
    public class ToDoItem : IToDoItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public ToDoItemStatus Status { get; set; } = ToDoItemStatus.Plane;

        public string Description { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public ApplicationUser CreatedBy { get; set; }

        public IList<ApplicationUser> AssignedTo { get; set; } = [];

        public IList<ApplicationUser> VisibleFor { get; set; } = [];

        public DateTime? DeathLine { get; set; }

        public DateTime? StatusChanged { get; set; }

        public ApplicationUser? StatusChangedBy { get; set; }

        public string? StatusComment { get; set; } = string.Empty;
    }
}
