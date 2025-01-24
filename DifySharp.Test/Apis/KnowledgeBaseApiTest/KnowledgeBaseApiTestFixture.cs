using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.DependencyInjection.Logging;

namespace DifySharp.Test.Apis.KnowledgeBaseApiTest;

public abstract class KnowledgeBaseApiTestFixture : IDisposable
{
    public IServiceProvider Services;

    protected KnowledgeBaseApiTestFixture()
    {
        // var knowledgeApiKey = Environment.GetEnvironmentVariable("KNOWLEDGE_BASE_API_KEY");
    
        // var host = Host.CreateDefaultBuilder()
        //         .ConfigureLogging(lb => { lb.AddXunitOutput(); })
        //         .ConfigureServices((context, services) =>
        //         {
        //             services.AddDifySdk(option => { option.KnowledgeBaseApiKey = knowledgeApiKey!; });
        //             // services.AddLogging(lb => lb.AddXunitOutput());
        //         })
        //         .Build()
        //     ;

        Services = Startup.ServiceProvider;
    }

    public virtual void Dispose()
    {
        // TODO release managed resources here
    }
}