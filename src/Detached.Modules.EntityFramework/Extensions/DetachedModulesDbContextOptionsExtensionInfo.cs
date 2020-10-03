using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;

namespace Detached.Modules.EntityFramework.DbContextExtension
{
    public class DetachedModulesDbContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        public DetachedModulesDbContextOptionsExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => nameof(DetachedModulesDbContextOptionsExtensionInfo);

        public override long GetServiceProviderHashCode()
        {
            return GetHashCode();
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {

        }
    }
}
