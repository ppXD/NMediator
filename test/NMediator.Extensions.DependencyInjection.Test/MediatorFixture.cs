using NMediator.Extensions.DependencyInjection.Test.Base;
using Shouldly;
using Xunit;

namespace NMediator.Extensions.DependencyInjection.Test;

public class MediatorFixture : TestFixtureBase
{
    [Theory]
    [InlineData(DependencyInjectionType.Autofac)]
    [InlineData(DependencyInjectionType.MicrosoftDependencyInjection)]
    public void ShouldMediatorResolved(DependencyInjectionType dependencyInjectionType)
    {
        RegisterMediator(dependencyInjectionType, null, new[] { typeof(MediatorFixture).Assembly });
        
        var mediatorResolvedByClass = GetMediator<Mediator>(dependencyInjectionType);
        var mediatorResolvedByInterface = GetMediator<IMediator>(dependencyInjectionType);

        mediatorResolvedByClass.ShouldNotBeNull();
        mediatorResolvedByInterface.ShouldNotBeNull();
    }
}