using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;

namespace Detached.Modules.EntityFramework.Extensions
{
    public class DbContextOptionsExtensionInfo : Microsoft.EntityFrameworkCore.Infrastructure.DbContextOptionsExtensionInfo
    {
        readonly IModule _module;

        public DbContextOptionsExtensionInfo(IDbContextOptionsExtension extension, IModule module)
            : base(extension)
        {
            _module = module;
        }

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => nameof(DbContextOptionsExtensionInfo);

        public override long GetServiceProviderHashCode() => _module.GetHashCode();

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {

        }
    }
}
