using Local.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Local.Web.Test
{
    public class RoleRepositoryTests
    {
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly RoleRepository _roleRepository;

        public RoleRepositoryTests()
        {
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new List<IRoleValidator<IdentityRole>>(),
                new Mock<ILookupNormalizer>().Object,
                new IdentityErrorDescriber(),
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object
            );
            _roleRepository = new RoleRepository(_roleManagerMock.Object);
        }

        [Fact]
        public void GetAll_ReturnsAllRoles()
        {
            // Arrange
            var roles = new List<IdentityRole>
            {
                new() { Id = "1", Name = "Admin" },
                new() { Id = "2", Name = "User" }
            }.AsQueryable();

            _roleManagerMock.Setup(r => r.Roles).Returns(roles);

            // Act
            var result = _roleRepository.GetAll();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsRole_WhenRoleExists()
        {
            // Arrange
            var role = new IdentityRole { Id = "1", Name = "Admin" };
            _roleManagerMock.Setup(r => r.FindByIdAsync("1")).ReturnsAsync(role);

            // Act
            var result = await _roleRepository.GetByIdAsync("1");

            // Assert
            Assert.Equal(role, result);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WhenRoleNotFound()
        {
            // Arrange
            _roleManagerMock.Setup(r => r.FindByIdAsync("1")).ReturnsAsync((IdentityRole?)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _roleRepository.GetByIdAsync("1"));
        }

        [Fact]
        public async Task CreateAsync_CreatesRole_WhenSuccessful()
        {
            // Arrange
            var role = new IdentityRole { Id = "1", Name = "Admin" };
            var identityResult = IdentityResult.Success;
            _roleManagerMock.Setup(r => r.CreateAsync(role)).ReturnsAsync(identityResult);

            // Act
            var result = await _roleRepository.CreateAsync(role);

            // Assert
            Assert.Equal(role, result);
        }

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenFailed()
        {
            // Arrange
            var role = new IdentityRole { Id = "1", Name = "Admin" };
            var identityResult = IdentityResult.Failed(new IdentityError { Description = "Error" });
            _roleManagerMock.Setup(r => r.CreateAsync(role)).ReturnsAsync(identityResult);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _roleRepository.CreateAsync(role));
        }

        [Fact]
        public async Task UpdateAsync_UpdatesRole_WhenSuccessful()
        {
            // Arrange
            var role = new IdentityRole { Id = "1", Name = "Admin" };
            var identityResult = IdentityResult.Success;
            _roleManagerMock.Setup(r => r.UpdateAsync(role)).ReturnsAsync(identityResult);

            // Act
            var result = await _roleRepository.UpdateAsync(role);

            // Assert
            Assert.Equal(role, result);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenFailed()
        {
            // Arrange
            var role = new IdentityRole { Id = "1", Name = "Admin" };
            var identityResult = IdentityResult.Failed(new IdentityError { Description = "Error" });
            _roleManagerMock.Setup(r => r.UpdateAsync(role)).ReturnsAsync(identityResult);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _roleRepository.UpdateAsync(role));
        }

        [Fact]
        public async Task DeleteAsync_DeletesRole_WhenSuccessful()
        {
            // Arrange
            var role = new IdentityRole { Id = "1", Name = "Admin" };
            var identityResult = IdentityResult.Success;
            _roleManagerMock.Setup(r => r.DeleteAsync(role)).ReturnsAsync(identityResult);

            // Act
            await _roleRepository.DeleteAsync(role);

            // Assert
            _roleManagerMock.Verify(r => r.DeleteAsync(role), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WhenFailed()
        {
            // Arrange
            var role = new IdentityRole { Id = "1", Name = "Admin" };
            var identityResult = IdentityResult.Failed(new IdentityError { Description = "Error" });
            _roleManagerMock.Setup(r => r.DeleteAsync(role)).ReturnsAsync(identityResult);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _roleRepository.DeleteAsync(role));
        }
    }
}
