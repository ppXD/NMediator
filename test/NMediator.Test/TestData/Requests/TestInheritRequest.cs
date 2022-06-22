namespace NMediator.Test.TestData.Requests;

public interface ITestOneWayRequest : ITestRequest
{
}

public interface ITestTwoWayRequest : ITestOneWayRequest
{
}

public class TestInterfaceRequest : ITestRequest
{
}

public class TestOneWayRequest : ITestOneWayRequest
{
}

public class TestTwoWayRequest : ITestTwoWayRequest
{
}

public class TestAllWayRequest : ITestOneWayRequest, ITestTwoWayRequest
{
}

public class TestInheritAllWayRequest : TestAllWayRequest
{
}

public class TestParentInheritRequest : TestInheritAllWayRequest
{
}