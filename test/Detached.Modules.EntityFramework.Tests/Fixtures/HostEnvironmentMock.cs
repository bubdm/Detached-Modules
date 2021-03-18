using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Detached.Modules.EntityFramework.Tests.Mocks
{
    public class HostEnvironmentMock : IHostEnvironment
    {
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string ContentRootPath { get; set; }

        public IFileProvider ContentRootFileProvider { get; set; }
    }
}