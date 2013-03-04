using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;

namespace KnockoutTodo.App_Start
{
    public static class RavenDbConfig
    {
        public static IDocumentStore Start()
        {
            var store = new EmbeddableDocumentStore
            {
                ConnectionStringName = "RavenDb",
                UseEmbeddedHttpServer = true,
                Conventions = { IdentityPartsSeparator = "-" }
            }.Initialize();

            IndexCreation.CreateIndexes(typeof(RavenDbConfig).Assembly, store);

            return store;
        }
    }
}