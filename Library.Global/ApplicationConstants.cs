namespace Library.Global
{
    /// <summary>
    /// All the needed constants for the <see cref="ILibrary"/> system.
    /// </summary>
    public static class ApplicationConstants
    {
        public const string MongoUserName = "francisco";

        public const string MongoClusterPassword = "IN3ihAfUZ5LWrFko";

        public static readonly string MongoConnectionString =
            String.Format("mongodb+srv://{0}:{1}@democluster.w6bc9.mongodb.net/?retryWrites=true&w=majority&appName=DemoCluster",
                MongoUserName, MongoClusterPassword);
    }
}
