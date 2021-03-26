using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Detached.Modules.Tests
{
    public class ModuleTests
    {
        [Fact]
        public void TestSimpleModule()
        {
            Module module = new Module();
            module.AddService<TestService>();

            IServiceCollection services = new ServiceCollection();
            module.ConfigureServices(services, null, null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            TestService testService = serviceProvider.GetRequiredService<TestService>();

            Assert.Equal(1, testService.TestMethod());
        }

        [Fact]
        public void TestNestedModule()
        {
            Module root = new Module();
            Module level1 = new Module();
            Module level2 = new Module();

            root.Modules.Add(level1);
            level1.Modules.Add(level2);

            level2.AddService<TestService>();

            IServiceCollection services = new ServiceCollection();
            root.ConfigureServices(services, null, null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            TestService testService = serviceProvider.GetRequiredService<TestService>();

            Assert.Equal(1, testService.TestMethod());
        }

        [Fact]
        public void TestCustomModule()
        {
            SampleModule module = new SampleModule();

            Assert.Equal("Sample", module.Name);
            Assert.Equal("1.0.0.0", module.Version.ToString());

            IServiceCollection services = new ServiceCollection();
            module.ConfigureServices(services, null, null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            TestService testService = serviceProvider.GetRequiredService<TestService>();

            Assert.Equal(1, testService.TestMethod());
        }

        public class TestService
        {
            public int TestMethod() => 1;
        }

        public class SampleModule : Module
        {
            public SampleModule()
            {
                this.AddService<TestService>();
            }
        }
    }
}