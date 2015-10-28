using System.Linq;
using System.Collections.Generic;
using Lohmann.HALight.Converters;
using Newtonsoft.Json;
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
    }
}
