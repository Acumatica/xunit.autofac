namespace Xunit.Autofac
{
    internal static class LifetimeScopeTags
    {
        private const string Guid = "0463C29A-D9C3-4C1E-8FBB-901E6DE19071";
        public static readonly object TestAssemblyExecution = nameof(TestAssemblyExecution) + ":" + Guid;
    }

    internal static class Keys
    {
        public const string ExecutionMessageSink = nameof(ExecutionMessageSink);
    }
}
