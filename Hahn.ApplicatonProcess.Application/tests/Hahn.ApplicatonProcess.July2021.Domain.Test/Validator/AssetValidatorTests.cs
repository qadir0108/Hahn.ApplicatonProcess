using System;
using FakeItEasy;
using FluentValidation.TestHelper;
using Hahn.ApplicatonProcess.July2021.Domain.Interfaces;
using Hahn.ApplicatonProcess.July2021.Domain.Models;
using Hahn.ApplicatonProcess.July2021.Domain.Validators;
using Xunit;

namespace Hahn.ApplicatonProcess.July2021.Domain.Test.Validator
{
    public class AssetValidatorTests
    {
        private readonly AssetValidator _testee;

        public AssetValidatorTests()
        {
            var cache = A.Fake<ICache>();
            _testee = new AssetValidator(cache);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Id_WhenEmpty_ShouldHaveValidationError(string id)
        {
            _testee.ShouldHaveValidationErrorFor(x => x.Id, id).WithErrorMessage("Id is required.");
        }

        [Theory]
        [InlineData("bitcoin")]
        public void Id_WhenGood_ShouldNotHaveValidationError(string id)
        {
            _testee.ShouldNotHaveValidationErrorFor(x => x.Id, id);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Symbol_WhenEmpty_ShouldHaveValidationError(string symbol)
        {
            _testee.ShouldHaveValidationErrorFor(x => x.Symbol, symbol).WithErrorMessage("Symbol is required.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Name_WhenEmpty_ShouldHaveValidationError(string name)
        {
            _testee.ShouldHaveValidationErrorFor(x => x.Name, name).WithErrorMessage("Name is required.");
        }

        [Theory]
        [ClassData(typeof(AssetValidatorTestData))]
        public void ValidAssetName_ShouldNotHaveValidationError(AssetVm asset)
        {
            _testee.ShouldNotHaveValidationErrorFor(x => x.Id, asset);
            _testee.ShouldNotHaveValidationErrorFor(x => x.Symbol, asset);
            _testee.ShouldNotHaveValidationErrorFor(x => x.Name, asset);

        }

    }
}