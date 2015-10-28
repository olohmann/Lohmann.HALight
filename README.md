# Lohmann.HALight

> A simple [HAL](http://stateless.co/hal_specification.html) formatter for ASP.NET 5 - MVC 6.

**Note:**
This is a port of my [ASP.NET WebApi 2 implementation](https://github.com/olohmann/WebApi.HALight).

**Note:**
A more in-depth usage sample should follow soon.

## Installation
Install via NuGet:
```
install-package Lohmann.HALight
```

## Usage

In your Startup.cs:
```

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
