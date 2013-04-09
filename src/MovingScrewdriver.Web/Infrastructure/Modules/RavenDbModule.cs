using Autofac;
using MovingScrewdriver.Web.Infrastructure.Indexes;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace MovingScrewdriver.Web.Infrastructure.Modules
{
    public class RavenDbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var documentStore = new DocumentStore
                {
                    ConnectionStringName = "RavenDb"
                };

            documentStore.Initialize();

            // not sure if this is a good idea to have this in module registration...
            IndexCreation.CreateIndexes(typeof(Posts_Statistics).Assembly, documentStore);

            builder
                .RegisterInstance(documentStore)
                .As<IDocumentStore>()
                .SingleInstance();
        }
    }
}