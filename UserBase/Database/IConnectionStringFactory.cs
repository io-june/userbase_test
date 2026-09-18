using Microsoft.Data.Sqlite;
using System.Data.OleDb;

namespace UserBase.Database
{
    public interface IConnectionStringFactory
    {
        string ForLocalFile(DbProvider provider, string path);
    }

    public class ConnectionStringFactory : IConnectionStringFactory
    {
        public string ForLocalFile(DbProvider provider, string path) => provider switch
        {
            DbProvider.Access => new OleDbConnectionStringBuilder
            {
                Provider = "Microsoft.ACE.OLEDB.12.0",
                DataSource = path
            }.ConnectionString,
            DbProvider.Sqlite => new SqliteConnectionStringBuilder
            {
                DataSource = path,
                Mode = SqliteOpenMode.ReadWriteCreate
            }.ToString(),
            _ => throw new NotSupportedException($"{provider} не поддерживается.")
        };
    }
}
