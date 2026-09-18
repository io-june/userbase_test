using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Data.OleDb;

namespace UserBase.Database.RemoteConnectionHandler
{
    public sealed record ConnectionProgress(int Percent, string Message);

    public interface IConnectionTester
    {
        Task TestAsync(DbConnectionInfo info,
                       IProgress<ConnectionProgress>? progress = null,
                       CancellationToken ct = default);
    }

    public class ConnectionTester : IConnectionTester
    {
        public async Task TestAsync(DbConnectionInfo info,
                                    IProgress<ConnectionProgress>? progress = null,
                                    CancellationToken ct = default)
        {
            progress?.Report(new(0, "Подготовка..."));

            await using DbConnection conn = CreateConnection(info);

            progress?.Report(new(30, "Инициирую соединение..."));
            await conn.OpenAsync(ct);

            progress?.Report(new(80, "Соединение установлено. Верификация..."));
            await using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT 1";
                await cmd.ExecuteScalarAsync(ct);
            }

            progress?.Report(new(80, "Проверка структуры базы данных..."));
            await using (var probe = conn.CreateCommand())
            {
                probe.CommandText = "SELECT COUNT(*) FROM Employees";
                try { await probe.ExecuteScalarAsync(ct); }
                catch (DbException ex)
                {
                    throw new InvalidOperationException(
                        $"В выбранной базе данных нет таблицы Employees: {ex.Message}", ex);
                }
            }

            progress?.Report(new(100, "Готово."));
        }

        private static DbConnection CreateConnection(DbConnectionInfo info) => info.Provider switch
        {
            DbProvider.Access => new OleDbConnection(info.ConnectionString),
            DbProvider.Sqlite => new SqliteConnection(
                new SqliteConnectionStringBuilder(info.ConnectionString)
                {
                    Pooling = false
                }.ToString()),

            DbProvider.SqlServer => new SqlConnection(info.ConnectionString),
            _ => throw new NotSupportedException($"{info.Provider} не поддерживается.")
        };
    }
}
