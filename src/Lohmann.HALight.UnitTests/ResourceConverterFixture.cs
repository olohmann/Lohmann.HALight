using System.Linq;
using System.Collections.Generic;
using Lohmann.HALight.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Lohmann.HALight.UnitTests
{
    public class ResourceConverterFixture
    {
        #region Dummy Models
        public class UserResource : Resource
        {
            public string FirstName { get; set; }
            public List<string> Values { get; set; }
            public NestedObject NestedObject { get; set; }
        }

        public class NestedObject
        {
            public string NestedString { get; set; }
            public int NestedInt { get; set; }
        }

        public class MetaResource : Resource
        {
            public string Meta { get; set; }
        }

        public class UserCollectionResource : CollectionResource<UserResource>
        {
            public MetaResource Meta { get; set; }

            public int MaximumCapacity { get; set; }
        }
        #endregion

        [Fact]
        public void VerifyRoundtrip()
        {
            // Arrange
            var userResource = new UserResource()
            {
                FirstName = "Hans",
                Values = new List<string>()
                {
                    "One",
                    "Two"
                }
            };
            var userCollectionResource = new UserCollectionResource()
            {
                Meta = new MetaResource() { Meta = "MetaInfo" },
                MaximumCapacity = 10
            };

            userCollectionResource.Items.Add(userResource);

            userResource.Relations.Add(Link.CreateSelfLink("http://locahost/users/1"));

            userCollectionResource.Relations.Add(Link.CreateSelfLink("http://localhost/users"));
            userCollectionResource.Relations.Add(Link.CreateLink("admins", "http://locahost/users/admins"));
            userCollectionResource.Relations.Add(Link.CreateLink("user", "http://locahost/users/1"));

            // Act
            var serializedResources = JsonConvert.SerializeObject(userCollectionResource, new RelationsConverter(), new ResourceConverter());
            var deserializedResources = JsonConvert.DeserializeObject<UserCollectionResource>(serializedResources, new RelationsConverter(), new ResourceConverter());
            var serializedDeserializedResources = JsonConvert.SerializeObject(deserializedResources, new RelationsConverter(), new ResourceConverter());

            // Assert
            Assert.Equal(userCollectionResource.Relations, deserializedResources.Relations);
            Assert.Equal(userCollectionResource.Items.ElementAt(0).Relations, deserializedResources.Items.ElementAt(0).Relations);
            Assert.Equal(serializedResources, serializedDeserializedResources);
        }
        [Fact]
        public void VerifyNestedObject()
        {
            // Arrange
            var userResource = new UserResource()
            {
                FirstName = "Hans",
                Values = new List<string>()
                {
                    "One",
                    "Two"
                },
                NestedObject = new NestedObject()
                {
                    NestedString = "nested string",
                    NestedInt = 100
                }
            };


            // Act
            var jsonSerializationSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            jsonSerializationSettings.Converters.Add(new RelationsConverter());
            jsonSerializationSettings.Converters.Add(new ResourceConverter());

            string serializedResources = JsonConvert.SerializeObject(userResource, jsonSerializationSettings);
            Assert.Contains("nestedObject", serializedResources);
            Assert.Contains("nestedString", serializedResources);
            Assert.Contains("nestedInt", serializedResources);
        }

    }
}
