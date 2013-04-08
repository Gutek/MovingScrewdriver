using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using AutoMapper.Mappers;
using Autofac;
using Autofac.Builder;
using Module = Autofac.Module;

namespace MovingScrewdriver.Web.Infrastructure.Modules
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterAssemblyTypes()
            //       .AsClosedTypesOf(typeof(ITypeConverter<,>))
            //       .AsSelf();

            
            builder.Register(ctx => 
                new ConfigurationStore(new TypeMapFactory(), MapperRegistry.AllMappers())
            )
            .AsImplementedInterfaces()
            .SingleInstance()
            .OnActivating(x =>
            {
                var profiles = x.Context.Resolve<IEnumerable<Profile>>().ToList();
                foreach (var profile in profiles)
                {
                    x.Instance.AddProfile(profile);
                }
            });

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .AssignableTo<Profile>()
                   .As<Profile>();

            builder.RegisterType<MappingEngine>()
                   .As<IMappingEngine>();

            //builder.RegisterGeneric(typeof(ITypeConverter<,>));
        }
    }
}