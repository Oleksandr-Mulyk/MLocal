using Local.Web.Data;
using Local.Web.Data.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Local.Web.Test
{
    public class UserRepositoryTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                store.Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                Array.Empty<IUserValidator<ApplicationUser>>(),
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object
                );
            _userRepository = new UserRepository(_userManagerMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsUser()
        {
            // Arrange
            var user = new ApplicationUser { Id = "1", UserName = "user1" };
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);

            // Act
            var result = await _userRepository.GetByIdAsync("1");

            // Assert
            Assert.Equal("user1", result.UserName);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WhenUserNotFound()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync((ApplicationUser)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _userRepository.GetByIdAsync("1"));
        }

        [Fact]
        public async Task CreateAsync_CreatesUser()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "user1" };
            _userManagerMock.Setup(x => x.CreateAsync(user, "password")).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userRepository.CreateAsync(user, "password");

            // Assert
            Assert.Equal("user1", result.UserName);
        }

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenUserCreationFails()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "user1" };
            _userManagerMock.Setup(
                x => x
                .CreateAsync(user, "password"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed" })
                );

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _userRepository.CreateAsync(user, "password"));
        }

        [Fact]
        public async Task UpdateAsync_UpdatesUser()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "user1" };
            _userManagerMock.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userRepository.UpdateAsync(user);

            // Assert
            Assert.Equal("user1", result.UserName);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenUserUpdateFails()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "user1" };
            _userManagerMock.Setup(
                x => x
                .UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User update failed" })
                );

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _userRepository.UpdateAsync(user));
        }

        [Fact]
        public async Task DeleteAsync_DeletesUser()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "user1" };
            _userManagerMock.Setup(x => x.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            await _userRepository.DeleteAsync(user);

            // Assert
            _userManagerMock.Verify(x => x.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WhenUserDeletionFails()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "user1" };
            _userManagerMock.Setup(
                x => x
                .DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User deletion failed" })
                );

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _userRepository.DeleteAsync(user));
        }
    }
}
