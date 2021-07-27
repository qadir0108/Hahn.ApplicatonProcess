using System;
using FakeItEasy;
using FluentValidation.TestHelper;
using Hahn.ApplicatonProcess.July2021.Domain.Interfaces;
using Hahn.ApplicatonProcess.July2021.Domain.Models;
using Hahn.ApplicatonProcess.July2021.Domain.Validators;
using Xunit;

namespace Hahn.ApplicatonProcess.July2021.Domain.Test.Validator
{
    public class UserValidatorTests
    {
        private readonly UserValidator _testee;

        public UserValidatorTests()
        {
            _testee = new UserValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void FirstName_WhenEmpty_ShouldHaveValidationError(string firstName)
        {
            _testee.ShouldHaveValidationErrorFor(x => x.FirstName, firstName).WithErrorMessage("First Name is required.");
        }

        [Theory]
        [InlineData("a")]
        public void FirstName_WhenShorterThanThreeCharacter_ShouldHaveValidationError(string firstName)
        {
            _testee.ShouldHaveValidationErrorFor(x => x.FirstName, firstName).WithErrorMessage("First Name must be at least 3 characters");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void LastName_WhenEmpty_ShouldHaveValidationError(string lastName)
        {
            _testee.ShouldHaveValidationErrorFor(x => x.LastName, lastName).WithErrorMessage("Last Name is required.");
        }

        [Theory]
        [InlineData("a")]
        public void LastName_WhenShorterThanThreeCharacter_ShouldHaveValidationError(string lastName)
        {
            _testee.ShouldHaveValidationErrorFor(x => x.LastName, lastName).WithErrorMessage("Last Name must be at least 3 characters");
        }

        [Theory]
        [InlineData(12)]
        public void Age_WhenLessThen18_ShouldHaveValidationError(int age)
        {
            _testee.ShouldHaveValidationErrorFor(x => x.Age, age).WithErrorMessage("Age must be at greater then 18");
        }

        [Theory]
        [InlineData(45)]
        public void Age_WhenGreaterThen18_ShouldNotHaveValidationError(int age)
        {
            _testee.ShouldNotHaveValidationErrorFor(x => x.Age, age);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Email_WhenEmpty_ShouldHaveValidationError(string email)
        {
            _testee.ShouldHaveValidationErrorFor(x => x.Email, email).WithErrorMessage("Email is required.");
        }

        [Theory]
        [InlineData("wrongemail")]
        public void Email_WhenWrong_ShouldHaveValidationError(string email)
        {
            _testee.ShouldHaveValidationErrorFor(x => x.Email, email).WithErrorMessage("Please enter valid email address");
        }

        [Theory]
        [InlineData("good@email.com")]
        public void Email_WhenGood_ShouldNotHaveValidationError(string email)
        {
            _testee.ShouldNotHaveValidationErrorFor(x => x.Email, email);
        }

        [Theory]
        [InlineData(null)]
        public void Address_WhenEmpty_ShouldHaveValidationError(AddressVm address)
        {
            _testee.ShouldHaveValidationErrorFor(x => x.Address, address).WithErrorMessage("Address is required.");
        }

        [Theory]
        [ClassData(typeof(UserAddressValitorTestData))]
        public void Address_WhenData_ShouldNotHaveValidationError(AddressVm address)
        {
            _testee.ShouldNotHaveValidationErrorFor(x => x.Address, address);
        }
    }
}