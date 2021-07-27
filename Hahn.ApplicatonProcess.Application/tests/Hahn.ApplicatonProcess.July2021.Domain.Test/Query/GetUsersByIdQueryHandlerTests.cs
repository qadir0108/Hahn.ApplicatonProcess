using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Hahn.ApplicatonProcess.July2021.Data.Entities;
using Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork;
using Hahn.ApplicatonProcess.July2021.Domain.Query;
using Xunit;

namespace Hahn.ApplicatonProcess.July2021.Domain.Test.Query
{
    public class GetUsersByIdQueryHandlerTests
    {
        private readonly IUnitOfWork _uow;

        private readonly GetUserByIdQueryHandler _testee;
        private readonly List<User> _users;
        private readonly int _id = 1;

        public GetUsersByIdQueryHandlerTests()
        {
            _uow = A.Fake<IUnitOfWork>();

            _testee = new GetUserByIdQueryHandler(_uow);

            _users = new List<User>
            {
                new User
                {
                    Id = _id,
                    Age = 42,
                }
            };
        }

        [Fact]
        public async Task Handle_WithValidId_ShouldReturnUser()
        {
            A.CallTo(() => _uow.UsersRepository.GetAll(true)).Returns(_users);

            var result = await _testee.Handle(new GetUserByIdQuery { Id = _id }, default);

            A.CallTo(() => _uow.UsersRepository.GetAll(true)).MustHaveHappenedOnceExactly();
            result.Age.Should().Be(42);
        }
    }
}