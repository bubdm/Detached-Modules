![Detached Banner](banner.png?raw=true)

# Modules
#### What is it

The base library introduces the concepts of high level Component, Module and Application in order to improve project organization.
- *Application*: is the root of the project organization.
- *Module*: is a relevant part of the application, define components and ideally has minimum or no dependencies to other modules.
- *Component*: provides a single and well defined functionality. Examples of components are repositories, managers, mappings, data files, etc.
During the initialization, a component may register zero or many services in the IServiceCollection, for example, a Repository may need to be
registered as a service, but a data file may be used during the initialization and disposed.


#### What does it solve

As project grow, tracking the services and modules become more difficult. Interactions between modules are not easy to identify and moving a piece of the application to a separate solution its really hard.
On the other hand, while some classes are truly services (registered in IServiceCollection) other classes aren't, as they might be part of the initialization or part of another service.
That's why I wanted to introduce Components and Modules as a way to organize small or medium projects.


#### How it works

Initialize an application, passing the configuration and the environment as parameters (those will be available to all modules and components).
Add the components and modules of your application. Detached.Modules provides the basic service and options components but other packages 
(later in this doc) provides more components.
Then use the application to register the services to an IServiceCollection instance, this is usually done in Startup.cs/ConfigureServices.

```csharp
public void ConfigureServices(IServiceCollection services)
{
	Application _app = new Application(configuration, environment);
	_app.AddService<MyBusinessLogic>();
	_app.ConfigureServices(services);
} 
```
Application instance registers itself into the container, so it can be injected any time in the project.


#### Modules

