using Microsoft.Data.Sqlite;
using UserBase.Database;

namespace UserBase.Database.LocalFileCreator
{
    public class SqliteDatabaseCreator : IDatabaseCreator
    {
        public DbProvider Provider => DbProvider.Sqlite;

        private static bool IsSqliteFile(string path)
        {
            try
            {
                Span<byte> header = stackalloc byte[16];
                using var fs = File.OpenRead(path);
                return fs.Read(header) == 16 &&
                       System.Text.Encoding.ASCII.GetString(header) == "SQLite format 3\0";
            }
            catch { return false; }
        }

        public void Create(string path)
        {
            var csb = new SqliteConnectionStringBuilder
            {
                DataSource = path,
                Mode = SqliteOpenMode.ReadWriteCreate,
                ForeignKeys = true,
                Pooling = false
            };

            if (File.Exists(path) && !IsSqliteFile(path))
            {
                SqliteConnection.ClearAllPools();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                File.Delete(path);
            }

            using var conn = new SqliteConnection(csb.ToString());
            conn.Open();

            Execute(conn, @"
            CREATE TABLE IF NOT EXISTS Genders (
                Id   INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE
            );");

            Execute(conn, @"
            CREATE TABLE IF NOT EXISTS Employees (
                Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                Surname     TEXT NOT NULL,
                FirstName   TEXT NOT NULL,
                Patronymic  TEXT,
                DateOfBirth TEXT NOT NULL,
                GenderId    INTEGER NOT NULL,
                FOREIGN KEY (GenderId) REFERENCES Genders(Id)
            );");

            Execute(conn, "CREATE INDEX IF NOT EXISTS IX_Employees_GenderId ON Employees(GenderId);");

            using (var check = conn.CreateCommand())
            {
                check.CommandText = "SELECT COUNT(*) FROM Genders;";
                long count = (long)check.ExecuteScalar()!;
                if (count == 0)
                {
                    foreach (var name in new[] { "Мужской", "Женский" })
                    {
                        using var ins = conn.CreateCommand();
                        ins.CommandText = "INSERT INTO Genders(Name) VALUES ($n);";
                        ins.Parameters.AddWithValue("$n", name);
                        ins.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void Execute(SqliteConnection conn, string sql)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
    }
}