﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.DependencyInjection.Logging;

namespace DifySharp.Test.Apis.KnowledgeBaseApiTest;

public abstract class KnowledgeBaseApiTestFixture : IDisposable
{
	protected IServiceProvider ServiceProvider => _scope.ServiceProvider;
	private   IServiceScope    _scope;

	public KnowledgeBaseApiTestFixture()
	{
		_scope = Startup.ServiceProvider.CreateScope();
	}

	public virtual void Dispose()
	{
		_scope.Dispose();
	}
}