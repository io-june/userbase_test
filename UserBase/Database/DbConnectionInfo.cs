namespace UserBase.Database
{
    public class DbConnectionInfo
    {
        public DbProvider Provider { get; init; }
        public string ConnectionString { get; init; } = "";
    }

    public class DbConnectionInfoHolder
    {
        public DbConnectionInfo? Info { get; set; }
    }
}
