using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace NMediator.Extensions.Microsoft.DependencyInjection.Test;

public class MediatorFixture
{
    private readonly IServiceProvider _container;
    
    public MediatorFixture()
    {
        _container = new ServiceCollection().AddNMediator(typeof(MediatorFixture).Assembly).BuildServiceProvider();
    }

    [Fact]
    public void ShouldMediatorResolved()
    {
        var mediatorResolvedByClass = _container.GetRequiredService<Mediator>();
        var mediatorResolvedByInterface = _container.GetRequiredService<IMediator>();

        mediatorResolvedByClass.ShouldNotBeNull();
        mediatorResolvedByInterface.ShouldNotBeNull();
    }
}