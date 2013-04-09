using System;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using MovingScrewdriver.Web.Infrastructure.Modules;
using MovingScrewdriver.Web.Infrastructure.Validation;
using MovingScrewdriver.Web.Services;
using Raven.Client;

namespace MovingScrewdriver.Web
{
    public class AutofacConfig
    {
        public static IocFactory IoC { get; set; }
        public static void Configure()
        {
             var builder = new ContainerBuilder();

             builder.RegisterModule<RavenDbModule>();
             builder.RegisterModule<RavenSessionModule>();
             builder.RegisterModule<AutoMapperModule>();

             builder.RegisterType<ArchiveDateValidator>()
                    .As<IArchiveDateValidator>()
                 ;
             builder.RegisterType<AkismetService>()
                 .As<IAkismetService>()
                ;

             builder.RegisterControllers(Assembly.GetExecutingAssembly())
                    .PropertiesAutowired();


             //builder.RegisterAssemblyTypes()
             //       .AsSelf()
             //       .AsImplementedInterfaces();

             var container = builder.Build();
             IoC = new IocFactory(container);
             DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public class IocFactory
        {
            private readonly IContainer _container;

            public IocFactory(IContainer container)
            {
                _container = container;
            }

            public object Resolve(Type type)
            {
                return _container.Resolve(type);
            }

            public T Resolve<T>()
            {
                return _container.Resolve<T>();
            }

            public bool IsRegisterd<T>()
            {
                return _container.IsRegistered<T>();
            }
        }
    }
}