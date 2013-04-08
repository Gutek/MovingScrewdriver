using Autofac;
using Raven.Client;

namespace MovingScrewdriver.Web.Infrastructure.Modules
{
    // extracted as seperate module, as if I will not like it, will be easier for me to extract that to
    // Begin and End requests, or filters, or whatever
    public class RavenSessionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => c.Resolve<IDocumentStore>().OpenSession())
                   .As<IDocumentSession>()
                   .InstancePerLifetimeScope()
                   .OnRelease(session =>
                   {
                       if (HttpContextFactory.GetHttpContext().Server.GetLastError() == null)
                       {
                           session.SaveChanges();
                       }

                       session.Dispose();
                   });
        }
    }
}