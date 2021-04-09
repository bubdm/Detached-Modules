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

The goal of this library is mainly to prototype and implement small and medium applications faster.

#### How it works

An application starts with a root Module, then more Modules are added. Once the configuration is ready, ConfigureServices is 
called to apply the needed services to the IServiceCollection, if any.
This is usually added in Startup.cs/ConfigureServices.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    Module app = new Module { Name = "Application" };
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

Modules can be organized as classes that inherit from Module and define Components and/or other Modules. One feature per Module or a Module containing
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
        this.AddDbContextConfiguration<ModuleDbContextConfiguration>();
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

    public Module Module { get; set; }

    public void ConfigureServices(Module module, IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        services.Add(_serviceDescriptor);
    }
}
```

Other components do not register services, a tool finds them and uses the data to perform actions.
For example, SeedFile. A tool looks for Components of type SeedFile and executes UpdateDataAsync method. There are no services registered
to the IServiceCollection instance, however a Seed File is a valid component and an useful part of an application.
(More info on Components below).

```
###### Annotations
Components can be automatically added using Annotations. Annotations are Attributes that inherit IComponentType.

```csharp
[ServiceComponent]
public class MyService
{

}
```
Use AddComponents to scan an assembly and add all components. Optionally, components can be filtered using a lambda predicate.

```csharp
public class MyModule()
{
    public MyModule()
    {
        AddComponents();
    }
}
```

## Entity Framework Components
These Components allows to distribute Entity Framework mapping and provides extra functionality.

###### DbContext Component
Allows to define a DbContext for the current Module and all the descendants, if any. 
It also configures [Detached.Mappers](https://github.com/leonardoporro/Detached-Mapper) library. 

Child modules can add entities or change mapping through the Mapping or the Repository Component.

In order to add a DbContext, call AddDbContext on the Module that will contain it (usually, the root module).
Then, configure EF in the same way as if it were added directly to the IServiceCollection.

```csharp
Module module = new Module();
module.AddDbContext<MyDbContext>(cfg =>
{
    // configure here any db.
});
```

The DbContext will be registered to the IServiceCollection, so in order to use it just inject it as usual.

###### DbContext Configuration Component
Allows Modules to configure the DbContext model and mapper (see [Detached.Mappers](https://github.com/leonardoporro/Detached-Mapper)).
Also provides data seeding feature using InitializeDataAsync extension method.
The configuration must be defined on a class and can be added to a Module using AddDbContextConfiguration or automatically, adding [DbContextConfigurationComponent]
annotation.
This is how a DbContext configuration class looks like:

```csharp
[DbContextConfigurationComponent(typeof(MainDbContext))] // this is the annotation to auto-add the component using Module.AddAllComponents().
public class SecurityDbContextConfiguration : DbContextConfiguration // inherit from DbContextConfiguration is recommended but not required.
{
    // use ConfigureModel to add/customize entities to the db context
    // this is called when DbContext is initialized
    public override void ConfigureModel(DbContext dbContext, ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Role>().ToTable("Roles");
    }

    // use ConfigureMapper to change mapping options (Detached.Mappers only)
    // this is called when DbContext is initialized
    public override void ConfigureMapper(DbContext dbContext, MapperOptions mapperOptions)
    {
        mapperOptions.Configure<User>().TypeOptions.Annotations.Add("custom annotation", true);
    }

    // use InitializeDataAsync to populate the database
    // this is called when you manually invoke DbContext.InitializeDataAsync();
    public override async Task InitializeDataAsync(DbContext dbContext)
    {
        await MapFileAsync<User>(dbContext, "path/to/file.json"); // import entities from a file
        await MapResourceAsync<Role>(dbContext, "namespace.namespace.resource.json"); // import entities from an embedded resource
    }
}

```

## GraphQL Components (HotChocolate)
Components for the awesome [HotChocolate] (https://github.com/ChilliCream/hotchocolate) library includes Mutation, Query and a general configuration 
component called just GraphQL.

###### GraphQL Component
Configuring any aspect of HotChocolate using GraphQL component:
```csharp 
 module.AddGraphQL(cfg =>
{
    cfg.AddType(...); // access to IRequestExecutorBuilder object.
    cfg.UseField(...)
});
```

###### Mutation
Extend the Mutation object using the given type with zero configuration:

```csharp
module.AddMutation<MyEntityMutations>();
```

###### Query
Extend the Query object using the given type with zero configuration:

```csharp
module.AddQuery<MyEntityQueries>();
```

###### Configuring HotChocolate
Once all components are defined, AddModule should be called on GraphQL initialization. This is usually done in Startup.cs/ConfigureServices.

```csharp
services.AddGraphQLServer()
        .AddMutationType(t => t.Name("Mutation"))
        .AddQueryType(t => t.Name("Query"))
        .AddModule(myRootModule) // adding the module will apply all its configurations and children configurations to HotChocolate.
        .AddFiltering()
        .AddSorting()
        .AddProjections()
        .AddAuthorization();
```

## Notes
Any feedback or help would be very welcome!