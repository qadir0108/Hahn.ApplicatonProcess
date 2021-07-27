using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Hahn.ApplicatonProcess.July2021.Data.Entities;
using Hahn.ApplicatonProcess.July2021.Domain.Command;
using Hahn.ApplicatonProcess.July2021.Domain.Models;
using Hahn.ApplicatonProcess.July2021.Domain.Query;
using Hahn.ApplicatonProcess.July2021.Web.ApiControllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Hahn.ApplicatonProcess.July2021.Web.Test.Controllers
{
    public class UsersControllerTests
    {
        private readonly IMediator _mediator;
        private readonly UsersController _testee;
        private readonly UserVm _createUserModel;
        private readonly UserVm _updateUserModel;
        private readonly int _id = 1;

        public UsersControllerTests()
        {
            //var mapper = A.Fake<IMapper>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<UserVm, User>().ReverseMap();
                mc.CreateMap<AssetVm, Asset>().ReverseMap();
                mc.CreateMap<AddressVm, Address>().ReverseMap();
            });
            var mapper = mappingConfig.CreateMapper();
            _mediator = A.Fake<IMediator>();
            _testee = new UsersController(_mediator, mapper);

            _createUserModel = new UserVm
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "test@test.com",
                Age = 30
            };
            _updateUserModel = new UserVm
            {
                Id = _id,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "test@test.com",
                Age = 30
            };
            var Users = new List<User>
            {
                new User
                {
                    Id = _id,
                    FirstName = "FirstName",
                    LastName = "LastName",
                    Email = "test@test.com",
                    Age = 30
                },
                new User
                {
                    Id = 1,
                    FirstName = "Darth",
                    LastName = "Vader",
                    Email = "test@test.com",
                    Age = 43
                }
            };

            //A.CallTo(() => mapper.Map<User>(A<User>._)).Returns(Users.First());
            A.CallTo(() => _mediator.Send(A<CreateUserCommand>._, default)).Returns(Users.First());
            A.CallTo(() => _mediator.Send(A<UpdateUserCommand>._, default)).Returns(Users.First());
            A.CallTo(() => _mediator.Send(A<GetUsersQuery>._, default)).Returns(Users);
        }

        [Theory]
        [InlineData("CreateUserAsync: User is null")]
        public async void Post_WhenAnExceptionOccurs_ShouldReturnBadRequest(string exceptionMessage)
        {
            A.CallTo(() => _mediator.Send(A<CreateUserCommand>._, default)).Throws(new ArgumentException(exceptionMessage));

            var result = await _testee.Post(_createUserModel);

            (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            (result.Result as BadRequestObjectResult)?.Value.Should().Be(exceptionMessage);
        }

        [Theory]
        [InlineData("UpdateUserAsync: User is null")]
        [InlineData("No user with this id found")]
        public async void Put_WhenAnExceptionOccurs_ShouldReturnBadRequest(string exceptionMessage)
        {
            A.CallTo(() => _mediator.Send(A<UpdateUserCommand>._, default)).Throws(new Exception(exceptionMessage));

            var result = await _testee.Put(_id, _updateUserModel);

            (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            (result.Result as BadRequestObjectResult)?.Value.Should().Be(exceptionMessage);
        }

        [Fact]
        public async void Get_ShouldReturnUsers()
        {
            var result = await _testee.Get();

            (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int) HttpStatusCode.OK);
            result.Value.Should().BeOfType<List<UserVm>>();
            result.Value.Count.Should().Be(2);
        }

        [Theory]
        [InlineData("Users could not be loaded")]
        public async void Get_WhenAnExceptionOccurs_ShouldReturnBadRequest(string exceptionMessage)
        {
            A.CallTo(() => _mediator.Send(A<GetUsersQuery>._, default)).Throws(new Exception(exceptionMessage));

            var result = await _testee.Get();

            (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result.Result as BadRequestObjectResult)?.Value.Should().Be(exceptionMessage);
        }

        [Fact]
        public async void Post_ShouldReturnUser()
        {
            var result = await _testee.Post(_createUserModel);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
            
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult.StatusCode.Should().Be((int) HttpStatusCode.Created);
            createdResult.Value.Should().BeOfType<UserVm>();

            var createdResultValue = createdResult.Value as UserVm;
            createdResultValue.Id.Should().Be(_id);
        }

        [Fact]
        public async void Put_ShouldReturnUser()
        {
            var result = await _testee.Put(_id, _updateUserModel);

            (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int) HttpStatusCode.OK);
            result.Value.Should().BeOfType<UserVm>();
            result.Value.Id.Should().Be(_id);
        }
    }
}