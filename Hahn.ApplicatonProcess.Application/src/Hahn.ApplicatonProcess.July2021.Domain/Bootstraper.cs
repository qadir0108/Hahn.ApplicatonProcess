using AutoMapper;
using FluentValidation.AspNetCore;
using Hahn.ApplicatonProcess.July2021.Data.Entities;
using Hahn.ApplicatonProcess.July2021.Domain.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Hahn.ApplicatonProcess.July2021.Domain
{
    public static class Bootstraper
    {

        public static void AddDomainMediatR(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        } 
        
        public static void AddMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> action)
        {
            services.AddSingleton(typeof(IMapper), _ =>
            {
                var mapperConfiguration = new MapperConfiguration(config => action(config));
                return mapperConfiguration.CreateMapper();
            });
        }

        public static void AddMaps(this IMapperConfigurationExpression configurationProvider) 
        {
            configurationProvider.CreateMap<UserVm, User>().ReverseMap();
            configurationProvider.CreateMap<AssetVm, Asset>().ReverseMap();
            configurationProvider.CreateMap<AddressVm, Address>().ReverseMap();
        }

        public static void UseFluentValidators(this FluentValidationMvcConfiguration fluentConfiguration)
        {
            fluentConfiguration.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
