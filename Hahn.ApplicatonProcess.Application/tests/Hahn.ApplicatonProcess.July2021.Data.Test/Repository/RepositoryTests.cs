using System;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using Hahn.ApplicatonProcess.July2021.Data.Entities;
using Hahn.ApplicatonProcess.July2021.Data.Repository;
using Hahn.ApplicatonProcess.July2021.Data.Test.Infrastructure;
using Xunit;

namespace Hahn.ApplicatonProcess.July2021.Data.Test.Repository
{
    public class RepositoryTests : DatabaseTestBase
    {
        private readonly HahnContext _contextFake;
        private readonly Repository<User> _testee;
        private readonly Repository<User> _testeeFake;
        private readonly User _newUser;

        public RepositoryTests()
        {
            _contextFake = A.Fake<HahnContext>();
            _testeeFake = new Repository<User>(_contextFake);
            _testee = new Repository<User>(Context);
            _newUser = new User
            {
                Age = 1282,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "first@last.com"
            };
        }

        [Fact]
        public void AddAsync_WhenEntityIsNull_ThrowsException()
        {
            _testee.Invoking(x => x.AddAsync(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async void CreateUserAsync_WhenUserIsNotNull_ShouldReturnUser()
        {
            var result = await _testee.AddAsync(_newUser);

            result.Should().BeOfType<User>();
        }

        [Fact]
        public void GetAll_WhenExceptionOccurs_ThrowsException()
        {
            A.CallTo(() => _contextFake.Set<User>()).Throws<Exception>();

            _testeeFake.Invoking(x => x.GetAll(true)).Should().Throw<Exception>().WithMessage("Couldn't retrieve entities: Unable to cast object of type 'Castle.Proxies.ObjectProxy_4' to type 'Microsoft.EntityFrameworkCore.Metadata.Internal.Model'.");
        }

        [Fact]
        public void Update_WhenEntityIsNull_ThrowsException()
        {
            _testee.Invoking(x => x.Update(null)).Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("Changed")]
        public async void Update_WhenUserIsNotNull_ShouldReturnUser(string firstName)
        {
            var user = Context.Users.First();
            user.FirstName = firstName;

            var result = _testee.Update(user);

            result.Should().BeOfType<User>();
            result.FirstName.Should().Be(firstName);
        }

    }
}