using System.Configuration;
using Autofac;
using MovingScrewdriver.Web.Infrastructure.Indexes;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;

namespace MovingScrewdriver.Web.Infrastructure.Modules
{
    public class RavenDbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var embeddedMode = ConfigurationManager.ConnectionStrings["RavenDb"] == null;

            IDocumentStore documentStore;

            if (embeddedMode)
            {
                documentStore = new EmbeddableDocumentStore
                {
                    DataDirectory = "~/App_Data/Raven"
                };
            }
            else
            {
                documentStore = new DocumentStore
                {
                    ConnectionStringName = "RavenDb"
                };
            }

            
            documentStore.Initialize();

            // not sure if this is a good idea to have this in module registration...
            IndexCreation.CreateIndexes(typeof(Tags_Count).Assembly, documentStore);

            builder
                .RegisterInstance(documentStore)
                .As<IDocumentStore>()
                .SingleInstance();
        }
    }
}