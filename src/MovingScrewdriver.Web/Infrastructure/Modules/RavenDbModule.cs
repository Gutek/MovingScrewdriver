using System;
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

            IDocumentStore documentStore;

#if DEBUG
            documentStore = new DocumentStore
                {
                    Url = "http://localhost:8080",
                    DefaultDatabase = "MovingScrewdriver"
                };
#else
            documentStore = new EmbeddableDocumentStore
                {
                    DataDirectory = "~\\App_Data\\Raven",
                    //UseEmbeddedHttpServer = true,
                    //Configuration =
                    //{
                    //    Port = 8088,
                    //}
                };
#endif
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