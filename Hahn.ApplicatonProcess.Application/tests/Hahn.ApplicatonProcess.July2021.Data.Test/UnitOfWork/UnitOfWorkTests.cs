using System;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using Hahn.ApplicatonProcess.July2021.Data.Entities;
using Hahn.ApplicatonProcess.July2021.Data.Repository;
using Hahn.ApplicatonProcess.July2021.Data.Test.Infrastructure;
using Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork;
using Xunit;

namespace Hahn.ApplicatonProcess.July2021.Data.Test
{
    public class UnitOfWorkTests : DatabaseTestBase
    {
        private readonly HahnContext _contextFake;
        private readonly IUnitOfWork _uowFake;
        private readonly IUnitOfWork _testeeFake;
        private readonly User _newUser;

        public UnitOfWorkTests()
        {
            _contextFake = A.Fake<HahnContext>();
            _uowFake = A.Fake<IUnitOfWork>();
            _testeeFake = new UnitOfWork(_contextFake);
            _newUser = new User
            {
                Age = 1282,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "first@last.com"
            };
        }


        [Fact]
        public void Update_WhenExceptionOccurs_ThrowsException()
        {
            A.CallTo(() => _uowFake.SaveChangesAsync()).Throws<Exception>();

            _testeeFake.Invoking(x => { x.UsersRepository.Update(new User()); _uowFake.SaveChangesAsync(); }).Should().Throw<Exception>().WithMessage("Exception of type 'System.Exception' was thrown.");
        }

        [Fact]
        public void AddAsync_WhenExceptionOccurs_ThrowsException()
        {
            A.CallTo(() => _uowFake.SaveChangesAsync()).Throws<Exception>();

            _testeeFake.Invoking(x => { x.UsersRepository.AddAsync(new User()); _uowFake.SaveChangesAsync(); }).Should().Throw<Exception>().WithMessage("Exception of type 'System.Exception' was thrown.");
        }

        [Fact]
        public async void CreateUserAsync_WhenUserIsNotNull_ShouldShouldAddUser()
        {
            var UserCount = Context.Users.Count();

            await _testeeFake.UsersRepository.AddAsync(_newUser);
            await _testeeFake.SaveChangesAsync();

            Context.Users.Count().Should().Be(UserCount);
        }
    }
}