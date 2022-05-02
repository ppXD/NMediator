namespace NMediator.Test.TestData.Events;

public interface ITestAbstractEvent : ITestEvent
{
}

public abstract class TestAbstractEventBase : ITestAbstractEvent
{
}

public class TestAbstractEvent : TestAbstractEventBase
{
}