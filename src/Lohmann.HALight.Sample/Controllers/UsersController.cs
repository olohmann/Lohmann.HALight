﻿using System.Linq;
using Lohmann.HALight.Sample.Controllers.Ressources;
using Lohmann.HALight.Sample.Model;
using Microsoft.AspNet.Mvc;

namespace Lohmann.HALight.Sample.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserRepository _repository;

        public UsersController()
        {
            _repository = new UserRepository();
        }

        // "/api/users/"
        [HttpGet(Name = "UsersRoute")]
        public IActionResult GetAll()
        {
            var userResources = _repository
                .GetAll()
                .Select(c => new UserResource
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

            var userListResource = new UserListResource
            {
                Items = userResources
            };

            foreach (var resource in userResources)
            {
                var selfLink = Link.CreateSelfLink(Url.Link("UserDetailsRoute", new {id = resource.Id}));
                userListResource.Relations.Add(Link.CreateLink("user", selfLink.Href));
            }

            userListResource.Relations.Add(
                Link.CreateSelfLink(Url.Link("UsersRoute", new {})));

            return Ok(userListResource);
        }

        // "/api/users/{id}"
        [HttpGet("{id}", Name = "UserDetailsRoute")]
        public IActionResult Get(int id)
        {
            var user = _repository.Get(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userDetailResource = new UserDetailResource
            {
                Id = user.Id,
                Name = user.Name,
                Mail = user.Mail,
                Address = user.Address
            };

            userDetailResource.Relations.Add(
                Link.CreateSelfLink(Url.Link("UserDetailsRoute", new {id = userDetailResource.Id})));

            return Ok(userDetailResource);
        }
    }
}