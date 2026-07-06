using FluentAssertions;
using Moq;
using TrueCodeExample.Users.Application.Features.Login;
using TrueCodeExample.Users.Domain.Entities;
using TrueCodeExample.Users.Domain.Exceptions;
using TrueCodeExample.Users.Tests.Helpers;

namespace TrueCodeExample.Users.Tests.Features.Login;

public sealed class LoginCommandHandlerTests
{
    private static readonly User ExistingUser = User.Restore(Guid.NewGuid(), "alice", "hashed-password");

    [Fact]
    public async Task Handle_WhenCredentialsAreValid_ReturnsTokens()
    {
        var users = new Mock<ILoginUserStore>();
        var passwordVerifier = new Mock<ILoginPasswordVerifier>();
        var tokenIssuer = TokenIssuerFactory.Create(out _, out _, out _, ExistingUser.Id);

        users.Setup(x => x.GetByNameAsync("alice", It.IsAny<CancellationToken>())).ReturnsAsync(ExistingUser);
        passwordVerifier.Setup(x => x.Verify("password", ExistingUser.PasswordHash)).Returns(true);

        var handler = new LoginCommandHandler(users.Object, passwordVerifier.Object, tokenIssuer);

        var response = await handler.Handle(new LoginCommand("alice", "password"), CancellationToken.None);

        response.UserId.Should().Be(ExistingUser.Id);
        response.AccessToken.Should().Be("access-token");
        response.RefreshToken.Should().Be("refresh-token");
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ThrowsInvalidCredentialsException()
    {
        var users = new Mock<ILoginUserStore>();
        var passwordVerifier = new Mock<ILoginPasswordVerifier>();
        var tokenIssuer = TokenIssuerFactory.Create(out _, out _, out _);

        users.Setup(x => x.GetByNameAsync("alice", It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

        var handler = new LoginCommandHandler(users.Object, passwordVerifier.Object, tokenIssuer);

        var act = async () => await handler.Handle(new LoginCommand("alice", "password"), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidCredentialsException>();
        passwordVerifier.Verify(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPasswordIsInvalid_ThrowsInvalidCredentialsException()
    {
        var users = new Mock<ILoginUserStore>();
        var passwordVerifier = new Mock<ILoginPasswordVerifier>();
        var tokenIssuer = TokenIssuerFactory.Create(out _, out _, out _);

        users.Setup(x => x.GetByNameAsync("alice", It.IsAny<CancellationToken>())).ReturnsAsync(ExistingUser);
        passwordVerifier.Setup(x => x.Verify("wrong", ExistingUser.PasswordHash)).Returns(false);

        var handler = new LoginCommandHandler(users.Object, passwordVerifier.Object, tokenIssuer);

        var act = async () => await handler.Handle(new LoginCommand("alice", "wrong"), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }
}
