using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserBase.Database.LocalFileCreator
{
    public interface IDatabaseCreator
    {
        DbProvider Provider { get; }
        void Create(string path);
    }

    public interface IDatabaseCreatorResolver
    {
        IDatabaseCreator Resolve(DbProvider provider);
    }

    public class DatabaseCreatorResolver(IServiceProvider sp) : IDatabaseCreatorResolver
    {
        public IDatabaseCreator Resolve(DbProvider provider) =>
            sp.GetRequiredKeyedService<IDatabaseCreator>(provider);
    }
}
