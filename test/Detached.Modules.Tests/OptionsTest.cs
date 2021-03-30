using Detached.Modules.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Xunit;

namespace Detached.Modules.Tests
{
    public class OptionsTest
    {
        [Fact]
        public void TestSimpleOptions()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("Options.json").Build();

            Module root = new Module() { Name = "Root" };
            root.AddOptions<TestOptions>();

            IServiceCollection services = new ServiceCollection();
            root.ConfigureServices(services, configuration, null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IOptions<TestOptions> testOptions = serviceProvider.GetRequiredService<IOptions<TestOptions>>();

            Assert.NotNull(testOptions.Value);
            Assert.Equal("this is the value 1", testOptions.Value.OptionValue1);
            Assert.Equal("this is the value 2", testOptions.Value.OptionValue2);
        }

        [Fact]
        public void TestGetOptions()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("Options.json").Build();

            Module root = new Module() { Name = "Root" };
            root.AddOptions<TestOptions>();

            IServiceCollection services = new ServiceCollection();
            root.ConfigureServices(services, configuration, null);

            TestOptions testOptions = root.GetOptions<TestOptions>(configuration);

            Assert.NotNull(testOptions);
            Assert.Equal("this is the value 1", testOptions.OptionValue1);
            Assert.Equal("this is the value 2", testOptions.OptionValue2);
        }

        [Fact]
        public void TestOptionsAnnotation()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("Options.json").Build();

            Module root = new Module() { Name = "Root" };
            root.AddComponents(x => x.Namespace.StartsWith("Detached.Modules.Tests"), GetType().Assembly);

            IServiceCollection services = new ServiceCollection();
            root.ConfigureServices(services, configuration, null);

            TestOptions testOptions = root.GetOptions<TestOptions>(configuration);

            Assert.NotNull(testOptions);
            Assert.Equal("this is the value 1", testOptions.OptionValue1);
            Assert.Equal("this is the value 2", testOptions.OptionValue2);
        }


        [OptionsComponent]
        public class TestOptions
        {
            public string OptionValue1 { get; set; }

            public string OptionValue2 { get; set; }
        }
    }
}
