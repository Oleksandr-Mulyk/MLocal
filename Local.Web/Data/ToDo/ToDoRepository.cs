using Microsoft.EntityFrameworkCore;

namespace Local.Web.Data.ToDo
{
    public class ToDoRepository(ApplicationDbContext applicationDbContext) :
        ApplicationDbContextRepository<IToDoItem, ToDoItem>(applicationDbContext.ToDoItems, applicationDbContext),
        IRepository<IToDoItem>
    {
        public override IQueryable<IToDoItem> GetAll() =>
            applicationDbContext.ToDoItems
                .Include(item => item.CreatedBy)
                .Include(item => item.AssignedTo)
                .Include(item => item.VisibleFor)
                .AsQueryable()
                .Select(item => (IToDoItem)item);
    }
}
