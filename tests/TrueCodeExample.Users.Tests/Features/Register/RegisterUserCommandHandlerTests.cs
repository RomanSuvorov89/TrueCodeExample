using FluentAssertions;
using Moq;
using TrueCodeExample.Users.Application.Features.Register;
using TrueCodeExample.Users.Domain.Entities;
using TrueCodeExample.Users.Domain.Exceptions;
using TrueCodeExample.Users.Tests.Helpers;

namespace TrueCodeExample.Users.Tests.Features.Register;

public sealed class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenUserDoesNotExist_CreatesUserAndReturnsTokens()
    {
        var users = new Mock<IRegisterUserStore>();
        var passwordHasher = new Mock<IRegisterPasswordHasher>();
        var tokenIssuer = TokenIssuerFactory.Create(
            out _,
            out _,
            out var refreshTokenStore);

        users.Setup(x => x.ExistsByNameAsync("alice", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        users.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        users.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        passwordHasher.Setup(x => x.Hash("password")).Returns("hashed-password");

        var handler = new RegisterUserCommandHandler(users.Object, passwordHasher.Object, tokenIssuer);

        var response = await handler.Handle(new RegisterUserCommand("alice", "password"), CancellationToken.None);

        response.AccessToken.Should().Be("access-token");
        response.RefreshToken.Should().Be("refresh-token");
        users.Verify(x => x.AddAsync(It.Is<User>(u => u.Name == "alice" && u.PasswordHash == "hashed-password"), It.IsAny<CancellationToken>()), Times.Once);
        users.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        refreshTokenStore.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserAlreadyExists_ThrowsUserAlreadyExistsException()
    {
        var users = new Mock<IRegisterUserStore>();
        var passwordHasher = new Mock<IRegisterPasswordHasher>();
        var tokenIssuer = TokenIssuerFactory.Create(out _, out _, out _);

        users.Setup(x => x.ExistsByNameAsync("alice", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var handler = new RegisterUserCommandHandler(users.Object, passwordHasher.Object, tokenIssuer);

        var act = async () => await handler.Handle(new RegisterUserCommand("alice", "password"), CancellationToken.None);

        await act.Should().ThrowAsync<UserAlreadyExistsException>();
        users.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
