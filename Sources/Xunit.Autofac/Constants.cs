namespace Xunit.Autofac
{
    internal static class LifetimeScopeTags
    {
        private const string TestCollectionGuid = "E55DC22E-F507-4CAF-841E-B62818EF3243";
        public static readonly object TestCollection = nameof(TestCollection) + ":" + TestCollectionGuid;
    }

    internal static class Keys
    {
        public const string ExecutionMessageSink = nameof(ExecutionMessageSink);
    }
}
