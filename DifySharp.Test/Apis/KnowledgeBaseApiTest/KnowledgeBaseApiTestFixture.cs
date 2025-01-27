using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.DependencyInjection.Logging;

namespace DifySharp.Test.Apis.KnowledgeBaseApiTest;

public abstract class KnowledgeBaseApiTestFixture : IDisposable
{
    protected readonly IServiceProvider Services = Startup.ServiceProvider;


    // public KnowledgeBaseApiTestFixture()
    // {
    //     // global set up here
    // }

    public virtual void Dispose()
    {
        // TODO release managed resources here
        
    }
}