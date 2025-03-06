using DifySharp.Apis;
using Microsoft.Extensions.DependencyInjection;

namespace DifySharp.DifyClient;

public class DifyClientFactory
{
    private static IServiceProvider? ServiceProvider;

    public DifyClientFactory(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// Creates a factory function for creating DifyClient instances.
    /// </summary>
    /// <param name="difyApiSecret"></param>
    /// <typeparam name="TApi">the type of api interfaces</typeparam>
    /// <returns></returns>
    internal static Func<IServiceProvider, object?, DifyClient<TApi>>
        DifyClientGenericFactory<TApi>(DifyApiSecret difyApiSecret) where TApi : notnull
    {
        return (sp, key) =>
        {
            if (ServiceProvider == null)
            {
                var difyFactory = sp.GetRequiredService<DifyClientFactory>();
            }
            return new DifyClient<TApi>(difyApiSecret, "", sp);
        };
    }

    internal static Func<IServiceProvider, object?, KnowledgeBaseClient>
        MakeKnowledgeBaseClientFactory()
        => (sp, key) => new KnowledgeBaseClient(sp.GetRequiredKeyedService<DifyClient<IKnowledgeBaseApi>>(key));

    internal static Func<IServiceProvider, object?, CompletionClient>
        MakeCompletionClientFactory()
        => (sp, key) => new CompletionClient(sp.GetRequiredKeyedService<DifyClient<ICompletionApi>>(key));

    internal static Func<IServiceProvider, object?, ChatClient>
        MakeChatClientFactory()
        => (sp, key) => new ChatClient(sp.GetRequiredKeyedService<DifyClient<IChatApi>>(key));

    internal static Func<IServiceProvider, object?, WorkflowClient>
        MakeWorkflowClientFactory()
        => (sp, key) => new WorkflowClient(sp.GetRequiredKeyedService<DifyClient<IWorkflowApi>>(key));
}