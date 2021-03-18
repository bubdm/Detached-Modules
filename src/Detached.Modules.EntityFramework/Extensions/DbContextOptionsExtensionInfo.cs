using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;

namespace Detached.Modules.EntityFramework.Extensions
{
    public class DbContextOptionsExtensionInfo : Microsoft.EntityFrameworkCore.Infrastructure.DbContextOptionsExtensionInfo
    {
        readonly Application _application;

        public DbContextOptionsExtensionInfo(IDbContextOptionsExtension extension, Application application)
            : base(extension)
        {
            _application = application;
        }

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => nameof(DbContextOptionsExtensionInfo);

        public override long GetServiceProviderHashCode() => _application.GetHashCode();

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {

        }
    }
}
