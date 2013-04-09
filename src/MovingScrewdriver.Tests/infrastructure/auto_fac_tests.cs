using System;
using System.Linq;
using System.Web.Mvc;
using MovingScrewdriver.Web;
using MovingScrewdriver.Web.Controllers.PostComment;
using MovingScrewdriver.Web.Infrastructure.Validation;
using MovingScrewdriver.Web.Services;
using Raven.Client;
using Xunit;

namespace MovingScrewdriver.Tests.infrastructure
{
    public class auto_fac_tests
    {
         public auto_fac_tests()
         {
             AutofacConfig.Configure();
         }

        [Fact(Skip = "Issue with raven embedded")]
        public void all_required_components_are_registered()
        {
            Assert.True(AutofacConfig.IoC.IsRegisterd<IAkismetService>());
            Assert.True(AutofacConfig.IoC.IsRegisterd<IArchiveDateValidator>());
            Assert.True(AutofacConfig.IoC.IsRegisterd<IDocumentStore>());
            Assert.True(AutofacConfig.IoC.IsRegisterd<IDocumentSession>());
            Assert.True(AutofacConfig.IoC.IsRegisterd<PostCommentController>());
        }

        [Fact(Skip = "Issue with raven embedded")]
        public void can_resolve_akisment_service()
        {
            Assert.DoesNotThrow(() => AutofacConfig.IoC.Resolve<IAkismetService>());
        }

        [Fact(Skip = "Issue with raven embedded")]
        public void can_resolve_raven()
        {
            Assert.DoesNotThrow(() => AutofacConfig.IoC.Resolve<IDocumentStore>());
            Assert.DoesNotThrow(() => AutofacConfig.IoC.Resolve<IDocumentSession>());
        }

        [Fact(Skip = "Issue with raven embedded")]
        public void can_resolve_controllers()
        {
            var types = typeof(PostCommentController).Assembly.GetTypes();

            var controllers = from t in types
                              where typeof(IController).IsAssignableFrom(t)
                                    && t.Name.EndsWith("Controller", StringComparison.Ordinal)
                                    && t.IsAbstract == false
                              select t;

            foreach (var controller in controllers)
            {
                var local = controller;
                Console.WriteLine("trying to resolve: {0}", local.Name);
                Assert.DoesNotThrow(() => AutofacConfig.IoC.Resolve(local));
            }
        }
    }
}