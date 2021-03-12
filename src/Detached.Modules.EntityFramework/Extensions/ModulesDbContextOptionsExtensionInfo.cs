using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;

namespace Detached.Modules.EntityFramework.DbContextExtension
{
    public class ModulesDbContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        readonly Application _application;

        public ModulesDbContextOptionsExtensionInfo(IDbContextOptionsExtension extension, Application application)
            : base(extension)
        {
            _application = application;
        }

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => nameof(ModulesDbContextOptionsExtensionInfo);

        public override long GetServiceProviderHashCode() => 0;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {

        }
    }
}
