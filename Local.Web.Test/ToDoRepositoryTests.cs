using Local.Web.Data;
using Local.Web.Data.ToDo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using MockQueryable.Moq;
using System.Reflection.Metadata;

namespace Local.Web.Test
{
    public class ToDoRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _mockDbContext;
        private readonly ToDoRepository _toDoRepository;

        public ToDoRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _mockDbContext = new Mock<ApplicationDbContext>(options);
            _toDoRepository = new ToDoRepository(_mockDbContext.Object);
        }

        [Fact]
        public void GetAll_ReturnsAllToDoItems()
        {
            // Arrange
            var toDoItems = new List<ToDoItem>
            {
                new() { Id = 1, Title = "Task 1", Created = DateTime.Now, CreatedBy = null },
                new() { Id = 2, Title = "Task 2", Created = DateTime.Now, CreatedBy = null }
            }.AsQueryable();

            Mock<DbSet<ToDoItem>> mockDbSet = toDoItems.BuildMockDbSet();

            _mockDbContext.Setup(c => c.ToDoItems).Returns(mockDbSet.Object);

            // Act
            var result = _toDoRepository.GetAll();

            // Assert
            Assert.Equal(toDoItems, result);
        }

        [Fact]
        public void GetAllByUser_ReturnsFilteredToDoItems()
        {
            // Arrange
            var user = new ApplicationUser { Id = "1", Email = "user@example.com" };
            var toDoItems = new List<ToDoItem>
                {
                    new() { Id = 1, Title = "Task 1", Created = DateTime.Now, CreatedBy = user },
                    new() { Id = 2, Title = "Task 2", Created = DateTime.Now,CreatedBy = null }
            }.AsQueryable();

            Mock<DbSet<ToDoItem>> mockDbSet = toDoItems.BuildMockDbSet();

            _mockDbContext.Setup(c => c.ToDoItems).Returns(mockDbSet.Object);

            // Act
            var result = _toDoRepository.GetAllByUser(user);

            // Assert
            Assert.Equal(toDoItems.Where(item => item.CreatedBy == user), result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsToDoItem_WhenItemExists()
        {
            // Arrange
            ToDoItem toDoItem = new() { Id = 1, Title = "Task 1", Created = DateTime.Now, CreatedBy = null };
            _mockDbContext.Setup(db => db.ToDoItems.FindAsync(1)).ReturnsAsync(toDoItem);

            // Act
            var result = await _toDoRepository.GetByIdAsync(1);

            // Assert
            Assert.Equal(toDoItem, result);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WhenItemNotFound()
        {
            // Arrange
            _mockDbContext.Setup(db => db.ToDoItems.FindAsync(1)).ReturnsAsync((ToDoItem)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _toDoRepository.GetByIdAsync(1));
        }

        [Fact]
        public async Task CreateAsync_CreatesToDoItem_WhenSuccessful()
        {
            // Arrange
            ToDoItem toDoItem = new() { Id = 1, Title = "Task 1", Created = DateTime.Now, CreatedBy = null };

            var mockSet = new Mock<DbSet<ToDoItem>>();
            _mockDbContext.Setup(db => db.ToDoItems).Returns(mockSet.Object);

            // Act
            var result = await _toDoRepository.CreateAsync(toDoItem);

            // Assert
            Assert.Equal(toDoItem, result);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesToDoItem_WhenSuccessful()
        {
            // Arrange
            ToDoItem toDoItem = new() { Id = 1, Title = "Task 1", Created = DateTime.Now, CreatedBy = null };

            List<ToDoItem> toDoItems = [toDoItem];
            Mock<DbSet<ToDoItem>> mockDbSet = toDoItems.BuildMockDbSet();

            _mockDbContext.Setup(db => db.ToDoItems).Returns(mockDbSet.Object);

            // Act
            ToDoItem updatedToDoItem = new() { Id = 1, Title = "Task 2", Created = DateTime.Now, CreatedBy = null };

            var result = await _toDoRepository.UpdateAsync(updatedToDoItem);

            // Assert
            mockDbSet.Verify(m => m.Update(It.IsAny<ToDoItem>()), Times.Once());
            _mockDbContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_DeletesToDoItem_WhenSuccessful()
        {
            // Arrange
            ToDoItem toDoItem = new() { Id = 1, Title = "Task 1", Created = DateTime.Now, CreatedBy = null };
            _mockDbContext.Setup(db => db.ToDoItems.Remove(toDoItem)).Returns((EntityEntry<ToDoItem>)null);
            _mockDbContext.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            await _toDoRepository.DeleteAsync(toDoItem);

            // Assert
            _mockDbContext.Verify(db => db.ToDoItems.Remove(toDoItem), Times.Once);
        }
    }
}
