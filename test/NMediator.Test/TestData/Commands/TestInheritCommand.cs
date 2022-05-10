namespace NMediator.Test.TestData.Commands;

public interface ITestCommandOneWayCommand : ITestCommand
{
}

public interface ITestCommandTwoWayCommand : ITestCommandOneWayCommand
{
}

public class TestInterfaceCommand : ITestCommand
{
}

public class TestCommandOneWayCommand : ITestCommandOneWayCommand
{
}

public class TestCommandTwoWayCommand : ITestCommandTwoWayCommand
{
}

public class TestCommandAllWayCommand : ITestCommandOneWayCommand, ITestCommandTwoWayCommand
{
}

public class TestInheritAllWayCommand : TestCommandAllWayCommand
{
}

public class TestParentInheritCommand : TestInheritAllWayCommand
{
}