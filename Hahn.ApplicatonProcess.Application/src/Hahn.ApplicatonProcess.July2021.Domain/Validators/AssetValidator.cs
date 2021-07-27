using FluentValidation;
using Hahn.ApplicatonProcess.July2021.Data.Remote;
using Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork;
using Hahn.ApplicatonProcess.July2021.Domain.Interfaces;
using Hahn.ApplicatonProcess.July2021.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Domain.Validators
{
    public class AssetValidator : AbstractValidator<AssetVm>
    {
        private readonly ICache _cache;

        public AssetValidator(ICache cache)
        {
            this._cache = cache;

            RuleFor(x => x.Id).Cascade(CascadeMode.Stop).NotNull().WithMessage("{PropertyName} is required.").NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.Symbol).Cascade(CascadeMode.Stop).NotNull().WithMessage("{PropertyName} is required.").NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.Name).Cascade(CascadeMode.Stop).NotNull().WithMessage("{PropertyName} is required.").NotEmpty().WithMessage("{PropertyName} is required.");

            When(x => !string.IsNullOrEmpty(x.Id) && !string.IsNullOrEmpty(x.Symbol) && !string.IsNullOrEmpty(x.Name), () => {
                RuleFor(x => new { x.Id, x.Symbol, x.Name })
                    .Must(x => ValidAssetName(x.Id, x.Symbol, x.Name))
                    .WithMessage("Asset Id, Symbol and Name is not valid.");
            });
        }

        private bool ValidAssetName(string id, string symbol, string name)
        {
            string key = "remoteassets";
            var remoteAssets = _cache.Get<List<RemoteAsset>>(key);
            var exists = remoteAssets
                .Any(x => x.id.ToLower().Equals(id.ToLower()) && x.symbol.ToLower().Equals(symbol.ToLower()) && x.name.ToLower().Equals(name.ToLower()));
            
            return exists;
        }
    }
}
