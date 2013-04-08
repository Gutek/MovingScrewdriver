using System;
using Raven.Client;
using Raven.Client.Embedded;

namespace MovingScrewdriver.Tests
{
    public abstract class ravendb_test_base : IDisposable
    {
        protected IDocumentStore Store {get; private set; }
        private IDocumentSession Session { get; set; }

        public ravendb_test_base()
        {
            Store = new EmbeddableDocumentStore
            {
                RunInMemory = true
            };

            Store.Initialize();
        }

        public void set_data(Action<IDocumentSession> action)
        {
            using (var session = Store.OpenSession())
            {
                action(session);
                session.SaveChanges();
            }
        }

        public IDocumentSession get_session()
        {
            Session = Store.OpenSession();
            return Session;
        }

        public virtual void Dispose()
        {
            if (Session != null)
            {
                Session.Dispose();
            }

            Store.Dispose();
        }
    }
}