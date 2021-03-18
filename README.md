![Detached Banner](banner.png?raw=true)

# Modules

Modern .net applications (and parts of the .net framework itself) relies on dependency injection, which is a good thing.

As projects grow, there are more and more services, business logic services, repository services, framework-related or hardware-related services, etc.
At some point, everthing is a service and it's difficult to say what service belongs to each feature or even what is the main purpose of the service.

Another common issue is that not all the pieces of an application are 'services' (i.e.: registered to the IServiceCollecion) or naturally fit in the DI mechanism.

That's why I'd like to introduce two concepts on top of the DI framework: Components and Modules.

Components are specific units of an application, focused in a single middle-level functionality. 
A Repository, a Seed File, a GraphQL Mutation, some Options, o a REST Endpoint are examples of Components.
A Component may register as many services as it needs, or no services at all. It may be used by other tools or Components, and discarded during 
the initialization.

Modules are parts of the application focused in high-level functionality, for example a "User Manager" or a "Security" module. 
A Module may contain Components and/or other Modules, a name and a version number.

#### How it works

An application starts with a root Module, then more Modules are added. Once the configuration is ready, ConfigureServices is 
called to apply the needed services to the IServiceCollection, if any.
This is usually added in Startup.cs/ConfigureServices.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    IModule app = new Module { Name = "Application" };
    app.AddModule(new SecurityModule());
    app.AddModule(new AccountingModule());
    app.AddModule(new CustomerModule());
    app.AddModule(new StockModule());
    app.AddModule(new ProviderModule());
    app.AddDbContext<MainDbContext>(cfg =>
    {
        cfg.UseSql...
    });
    app.ConfigureServices(services, Configuration, HostEnviornment);

    services.AddControllers();
}
```
###### Modules

Modules are classes that inherit from Module and define Components and/or other Modules. One feature per Module or a Module containing
many features may be implemented. It is up to the developers.

```csharp
public class SecurityModule : Module
{
    public SecurityModule()
    {
        this.AddModule(new UserManagerModule());
    }
}

public class UserManagerModule : Module
{
    public UserManagerModule()
    {
        this.AddRepository<UserRepository>();
        this.AddOptions<UserManagerOptions>();
        this.AddMutation<UserMutation>();
        this.AddQuery<UserQuery>();
    }
}
```

###### Components
Components are basically any kind of classes that implement IComponent.

This is the source code for "Service" component, it is the simplest one and just register a given type to the IServiceCollection container.

```csharp
public class ServiceComponent : IComponent
{
    readonly ServiceDescriptor _serviceDescriptor;

    public ServiceComponent(ServiceDescriptor serviceDescriptor)
    {
        _serviceDescriptor = serviceDescriptor;
    }

    public IModule Module { get; set; }

    public void ConfigureServices(IModule module, IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        services.Add(_serviceDescriptor);
    }
}
```

Other components do not register services, another tool finds them and uses the data to perform actions.
For example, SeedFile. A tool looks for Components of type SeedFile and executes UpdateDataAsync method. There are no services registered
to the IServiceCollection instance, however a Seed File is a valid component and an useful part of an application.
(More info on Components below).


```csharp
public class SeedFileComponent<TDbContext, TEntity> : SeedFileComponent
        where TDbContext : DbContext
        where TEntity : class
{
    public override Type DbContextType => typeof(TDbContext);

    public override Type EntityType => typeof(TEntity);

    public override async Task UpdateDataAsync(DbContext dbContext)
    {
        using (Stream fileStream = File.OpenRead(Path)) 
        {
            // this uses Detached.Mapper library! check it out.
            await dbContext.ImportJsonAsync<TEntity>(fileStream);
        }
    }
}
```

## Entity Framework Components




## GraphQL Components