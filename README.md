# Lohmann.HALight

> A simple [HAL](http://stateless.co/hal_specification.html) formatter for ASP.NET Core MVC.

**Note:**
This is a port of my [ASP.NET WebApi 2 implementation](https://github.com/olohmann/WebApi.HALight).

## Installation
Install via NuGet:
```
install-package Lohmann.HALight
```

## Usage

**Note**: There is a sample in the repo to help you getting started.

In your Startup.cs:
```csharp
public class Startup
{
    private readonly ILoggerFactory _loggerFactory;

    public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public void ConfigureServices(
        IServiceCollection services)
    {
        var logger = _loggerFactory.CreateLogger<HalInputFormatter>();
        services.AddMvc(options =>
        {                
            options.InputFormatters.Add(new HalInputFormatter(logger));
            options.OutputFormatters.Add(new HalOutputFormatter());
        });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {            
        // ...

        app.UseMvc();
    }
}
```

In your resource model:
```
using Lohmann.HALight;

public class UserResource : Resource
{
    public int Id { get; set; }

    public string Name { get; set; }
}
```

In your controller:
```
public class UsersController : Controller
{
    private readonly UserRepository _repository;

    // ...
    
    // Route: "/Users/{id}"
    public IActionResult Get(int id)
    {
        var user = _repository.Get(id);

        if (user == null)
        {
            return NotFound();
        }

        var userResource = new UserResource
        {
            Id = user.Id,
            Name = user.Name,            
        };

        userResource.Relations.Add(
            Link.CreateSelfLink(
                Url.Link(
                    "DefaultApi", 
                    new { controller = "Users", id = userDetailResource.Id })
            )
        );

        return Ok(userDetailResource);
    }
}
```

## Acknowledgements
There a couple of other HAL libraries for ASP .NET Web API around that inspired my take on this subject. A big thank you to:
* [WebApi.Hal](https://github.com/JakeGinnivan/WebApi.Hal)
* [PointW.WebApi.ResourceModel](https://github.com/biscuit314/PointW.WebApi.ResourceModel)
