using FakeItEasy;
using FluentAssertions;
using Hahn.ApplicatonProcess.July2021.Data.Entities;
using Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork;
using Hahn.ApplicatonProcess.July2021.Domain.Command;
using Xunit;

namespace Hahn.ApplicatonProcess.July2021.Domain.Test.Command
{
    public class CreateUserCommandHandlerTests
    {
        private readonly CreateUserCommandHandler _testee;
        private readonly IUnitOfWork _uow;
        private readonly User _user;

        public CreateUserCommandHandlerTests()
        {
            _uow = A.Fake<IUnitOfWork>();

            _testee = new CreateUserCommandHandler(_uow);

            _user = new User
            {
                FirstName = "Beeba"
            };
        }

        [Fact]
        public async void Handle_ShouldCallAddAsync()
        {
            await _testee.Handle(new CreateUserCommand() { User = _user }, default);

            A.CallTo(() => _uow.UsersRepository.AddAsync(A<User>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void Handle_ShouldReturnCreatedUser()
        {
            A.CallTo(() => _uow.UsersRepository.AddAsync(A<User>._)).Returns(_user);

            var result = await _testee.Handle(new CreateUserCommand() { User = _user }, default);

            result.Should().BeOfType<User>();
            result.FirstName.Should().Be("Beeba");
        }

    }
}