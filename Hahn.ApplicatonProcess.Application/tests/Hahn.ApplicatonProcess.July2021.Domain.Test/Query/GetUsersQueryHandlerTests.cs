using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Hahn.ApplicatonProcess.July2021.Data.Entities;
using Hahn.ApplicatonProcess.July2021.Data.Repository;
using Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork;
using Hahn.ApplicatonProcess.July2021.Domain.Models;
using Hahn.ApplicatonProcess.July2021.Domain.Query;
using Xunit;

namespace Hahn.ApplicatonProcess.July2021.Domain.Test.Query
{
    public class GetUsersQueryHandlerTests
    {
        private readonly IUnitOfWork _uow;

        private readonly GetUsersQueryHandler _testee;
        private readonly List<User> _users;

        public GetUsersQueryHandlerTests()
        {
            _uow = A.Fake<IUnitOfWork>();
            _testee = new GetUsersQueryHandler(_uow);

            _users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Age = 42,
                    FirstName = "FirstName",
                    LastName = "LastName",
                    Email = "first@user.com"
                },
                new User
                {
                    Id = 2,
                    Age = 22,
                    FirstName = "First",
                    LastName = "Last",
                    Email = "last@user.com"
                }
            };
        }

        [Fact]
        public async Task Handle_ShouldReturnUsers()
        {
            A.CallTo(() =>  _uow.UsersRepository.GetAll(true)).Returns(_users);

            var result = await _testee.Handle(new GetUsersQuery(), default);

            A.CallTo(() => _uow.UsersRepository.GetAll(true)).MustHaveHappenedOnceExactly();
            result.Should().BeOfType<List<User>>();
            result.Count.Should().Be(2);
        }
    }
}