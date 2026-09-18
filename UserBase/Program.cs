using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserBase.Database;
using UserBase.Database.Context;
using UserBase.Database.LocalFileCreator;
using UserBase.Database.RemoteConnectionHandler;
using UserBase.Logging;
using UserBase.Service;

namespace UserBase
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var builder = Host.CreateApplicationBuilder();

            builder.Logging.AddDebug();
            builder.Logging.AddProvider(new FileLoggerProvider(FileLoggerProvider.DefaultDirectory));

            builder.Services.AddKeyedSingleton<IDatabaseCreator, AccessDatabaseCreator>(DbProvider.Access);
            builder.Services.AddKeyedSingleton<IDatabaseCreator, SqliteDatabaseCreator>(DbProvider.Sqlite);
            builder.Services.AddSingleton<IDatabaseCreatorResolver, DatabaseCreatorResolver>();
            builder.Services.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();
            builder.Services.AddSingleton<IConnectionTester, ConnectionTester>();
            builder.Services.AddSingleton<DbConnectionInfoHolder>();
            builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
            builder.Services.AddTransient<StartForm>();
            builder.Services.AddTransient<MainForm>();

            builder.Services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var info = sp.GetRequiredService<DbConnectionInfoHolder>().Info
                    ?? throw new InvalidOperationException("Connection not configured.");

                switch (info.Provider)
                {
                    case DbProvider.Access: options.UseJet(info.ConnectionString); break;
                    case DbProvider.Sqlite: options.UseSqlite(info.ConnectionString); break;
                    case DbProvider.SqlServer: options.UseSqlServer(info.ConnectionString); break;
                    default: throw new NotSupportedException($"Провайдер {info.Provider} не поддерживается.");
                }
            });

            using var host = builder.Build();

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            DbConnectionInfo? info = null;
            using (var setup = host.Services.GetRequiredService<StartForm>())
            {
                if (setup.ShowDialog() == DialogResult.OK) info = setup.Result;
            }

            if (info is null) return;
            host.Services.GetRequiredService<DbConnectionInfoHolder>().Info = info;

            using var main = host.Services.GetRequiredService<MainForm>();
            Application.Run(main);
        }
    }
}
