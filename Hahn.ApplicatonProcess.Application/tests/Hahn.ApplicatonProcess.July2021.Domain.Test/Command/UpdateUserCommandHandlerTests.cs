using FakeItEasy;
using FluentAssertions;
using Hahn.ApplicatonProcess.July2021.Data.Entities;
using Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork;
using Hahn.ApplicatonProcess.July2021.Domain.Command;
using Xunit;

namespace Hahn.ApplicatonProcess.July2021.Domain.Test.Command
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly UpdateUserCommandHandler _testee;
        private readonly IUnitOfWork _uow;
        private readonly User _user;

        public UpdateUserCommandHandlerTests()
        {
            _uow = A.Fake<IUnitOfWork>();

            _testee = new UpdateUserCommandHandler(_uow);

            _user = new User
            {
                FirstName = "Beeba"
            };
        }

        [Fact]
        public async void Handle_ShouldCallUserUpdater()
        {
            A.CallTo(() => _uow.UsersRepository.Update(A<User>._)).Returns(_user);

            await _testee.Handle(new UpdateUserCommand() { User = _user }, default);

        }

        [Fact]
        public async void Handle_ShouldReturnUpdatedUser()
        {
            A.CallTo(() => _uow.UsersRepository.Update(A<User>._)).Returns(_user);

            var result = await _testee.Handle(new UpdateUserCommand() { User = _user }, default);

            result.Should().BeOfType<User>();
            result.FirstName.Should().Be(_user.FirstName);
        }

        [Fact]
        public async void Handle_ShouldUpdateAsync()
        {
            await _testee.Handle(new UpdateUserCommand() { User = _user }, default);

            A.CallTo(() => _uow.UsersRepository.Update(A<User>._)).MustHaveHappenedOnceExactly();
        }
    }
}