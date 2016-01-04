using System.Collections.Generic;
using System.Linq;

namespace Lohmann.HALight.Sample.Model
{
    public class UserRepository
    {
        #region In-Memory Dummy Data

        private static readonly List<User> _UsersInMemory = new List<User>
        {
            new User {Id = 1, Name = "Hans Peter", Mail = "hans.peter@contoso.com", Address = "Contoso Town"},
            new User {Id = 2, Name = "Georg Mayer", Mail = "georg.mayer@fabrikam.com", Address = "Fabrikam City"},
            new User {Id = 3, Name = "Frank Franklyn", Mail = "frank.franklyn@contoso.com", Address = "Contoso Village"}
        };

        #endregion

        public IEnumerable<User> GetAll()
        {
            return _UsersInMemory;
        }

        public User Get(int id)
        {
            return _UsersInMemory.SingleOrDefault(_ => _.Id == id);
        }

        public IEnumerable<User> GetUsers(IEnumerable<int> ids)
        {
            return _UsersInMemory.Where(_ => ids.Contains(_.Id));
        }
    }
}