using Microsoft.Extensions.DependencyInjection;

namespace DifySharp.Test.Apis;

public abstract class ApiTestFixture : IDisposable
{
	protected IServiceProvider ServiceProvider => _scope.ServiceProvider;
	private   IServiceScope    _scope;

	public ApiTestFixture()
	{
		_scope = Startup.ServiceProvider.CreateScope();
	}

	public virtual void Dispose()
	{
		_scope.Dispose();
	}
}